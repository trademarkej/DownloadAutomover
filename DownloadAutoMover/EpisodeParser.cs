using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DownloadAutoMover.Classes;

namespace DownloadAutoMover
{
    public class EpisodeParser
    {
        MediaFile mediaFile = new MediaFile();
        MainForm mainForm;
        private readonly string ext = "(avi|mp4|mkv)";
        private readonly string codec = @"((x|X|h|H)26(4|5|6))";
        private readonly string quality = @"((\d{2,3})0p)";
        private readonly string year = @"(19|20)\d{2}";
        private readonly string episode = @"((e|E)[0-9]{1,3})";
        private readonly string season = @"((s|S)[0-9]{1,2})";
        private readonly string sp = @"(\s|\.)";

        public EpisodeParser(string dbName)
        {
            mainForm = new MainForm(dbName);
        }

        public string GetCodec(string str)
        {
            string result = "N/A";
            string[] rexs = { codec };
            foreach (string rex in rexs)
            {
                var m = Regex.Match(str, rex, RegexOptions.IgnoreCase);
                if (m.Length > 0)
                {
                    result = m.ToString();
                }
            }
            return result;
        }

        
        public string GetEpisode(string str)
        {
            string result = "";
            string[] rexs = {
                episode,
                @"((e|E)pisode[0-9]{1,3})",
                @"(\d{1,3})"

            };
            foreach (string rex in rexs)
            {
                var m = Regex.Match(str, rex, RegexOptions.IgnoreCase);
                if (m.Length > 0)
                {
                    var tmp = Regex.Match(str, rex).ToString();
                    string[] prefixes = { "e", "E" };
                    result = prefixes.Any(tmp.Contains) ? tmp.Remove(0, 1) : tmp;
                    break;
                }
            }
            return result;
        }

        public string GetGroup(string str)
        {
            string result = "N/A";
            string[] rexs = {
                @"((\w+)|)(\[\w+\])[^(\[" + quality + @"\])]",
                codec + @"(.*)?(?=(\." + ext + "))"
            };
            foreach (string rex in rexs)
            {
                var m = Regex.Match(str, rex, RegexOptions.IgnoreCase);
                if (m.Length > 1)
                {
                    var tmp = GetRexValue(m.ToString(), codec, "");
                    result = mediaFile.CapitalizeFirstLetters(
                        tmp.Substring(0, 1).Equals("-") ? tmp.Remove(0, 1) : tmp).Trim();
                    break;
                }
            }
            return result;
        }

        public string GetQuality(string str)
        {
            string result = "N/A";
            string[] rexs = { quality };
            foreach (string rex in rexs)
            {
                var m = Regex.Match(str, rex, RegexOptions.IgnoreCase);
                if (m.Length > 0 && m.Length <= 5)
                {
                    result = m.ToString();
                }
            }
            return result;
        }

        private string GetRenameValue(string str)
        {
            var renameItems = mainForm.GetRenameItems();
            foreach (RenameItem renameItem in renameItems)
            {
                var tmp = Regex.Match(str, renameItem.Value, RegexOptions.IgnoreCase);
                if (tmp.Length > 0)
                {
                    str = renameItem.Rename;
                    break;
                }
            }
            return str;
        }

        private string GetRexValue(string str, string rex, string sub)
        {
            if (sub.Equals(year) && Regex.Match(str, rex).ToString().Length > 0)
            {
                sub = "(" + Regex.Match(str, rex).ToString() + ")";
            }
            var tmp = Regex.Match(str, rex).ToString().Length > 0 ?
                (str.Replace(Regex.Match(str, rex).ToString(), sub)).Trim() :
                str.Trim();
            return tmp;
        }

        public string GetSeason(string str)
        {
            string result = "";
            string[] rexs = {
                year,
                season,
                @"((s|S)eason[0-9]{1,2})"
            };
            foreach (string rex in rexs)
            {
                var m = Regex.Match(str, rex, RegexOptions.IgnoreCase);
                if (m.Length > 0)
                {
                    var tmp = Regex.Match(str, rex).ToString();
                    string[] prefixes = { "s", "S" };
                    result = prefixes.Any(tmp.Contains) ? tmp.Remove(0, 1) : tmp;
                    break;
                }
            }
            return result.Equals("") ? "01" : result;
        }

        public string[] GetSeasonEpisode(string str)
        {
            string[] se = { "", "" };
            string[] rexs = { 
                year + @".[0-9]{1,2}.[0-9]{1,2}",
                sp + @"(" + season + episode + @")" + sp,
                sp + @"((s|S)eason[0-9]{1,2}(e|E)pisode[0-9]{1,3})" + sp,
                @"(" + sp + @"\d{1,3}" + sp + ")"
            };
            foreach (string rex in rexs)
            {
                var m = Regex.Match(str, rex, RegexOptions.IgnoreCase);
                if (m.Length > 0)
                {
                    se[0] = GetSeason(m.ToString());
                    se[1] = GetEpisode(m.ToString());
                    break;
                }
            }
            return se;
        }

        public string GetTitle(string str)
        {
            string title = "";
            string[] rexs = { 
                @"(.*)?(?=(." + season + episode + "|." + year + @"|(\s\-\s\d{1,2})))"
            };
            string group = @"(\[.*?\])";
            foreach (string rex in rexs)
            {
                var m = Regex.Match(str, rex, RegexOptions.IgnoreCase);
                if (m.Length > 0)
                {
                    var tmp = mediaFile.CapitalizeFirstLetters(
                        m.ToString().Replace("."," ")).Trim();
                    title = GetRexValue(tmp, group, "");
                    title = GetRexValue(title, year, year);
                    title = GetRenameValue(title);
                    title = title.EndsWith("-") ? 
                        title.Substring(0, title.Length - 1).Trim() : title;
                    break;
                }
            }
            return title;
        }

        public TvEpisode GetTvEpisode(string str)
        {
            string[] se = GetSeasonEpisode(str);
            TvEpisode tvEpisode = new TvEpisode
            {
                Season = se[0],
                Episode = se[1],
                Title = GetTitle(str),
                Codec = GetCodec(str),
                Group = GetGroup(str),
                Quality = GetQuality(str),
                Category = 0
            };
            return tvEpisode;
        }
    }   
}
