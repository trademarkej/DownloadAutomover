using System;
using System.Linq;
using System.Text.RegularExpressions;
using DownloadAutoMover.Classes;

namespace DownloadAutoMover.Classes
{
    public class MovieParser
    {
        MediaFile mediaFile = new MediaFile();
        MainForm mainForm;
        private readonly string ext = "(avi|mp4|mkv)";
        private readonly string codec = @"((x|X|h|H)26(4|5|6))";
        private readonly string quality = @"((36|48|72|108)0(p|))";
        private readonly string year = @"(19|20)\d{2}";
        private readonly string sp = @"(\s|\.)";

        public MovieParser(string dbName)
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

        public string GetGroup(string str)
        {
            string result = "N/A";
            string[] rexs = {
                @"(\[.*?\])",
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

        public string GetTitle(string str)
        {
            string[] rexs = {
                @"\." + ext,
                @"(\[.*?\])",
                @"(\-.\d{1,2})",
                @"((s|S)[0-9]{1,2})((e|E)[0-9]{1,3})(.*)?",
                @"(\s|\.)(s|S)\d{1,2}(.*?)(\d{1,3})",
                codec + @"(.*)?",
                year + @".\d{2}.\d{2}(.*)?"
            };
            string tmp = str;
            foreach (string rex in rexs)
            {
                do
                {
                    var m = Regex.Match(tmp, rex);
                    if (m.Success)
                    {
                        tmp = tmp.Replace(m.ToString(), "").Replace(".", " ").Trim();
                        tmp = tmp.EndsWith("-") ?
                            tmp.Substring(0, tmp.Length - 1) : tmp;
                    }
                } while (Regex.Match(tmp, rex).Success);
            }
            tmp = GetRexValue(tmp, year, year);
            //tmp = GetRenameValue(tmp);
            return mediaFile.CapitalizeFirstLetters(tmp).Trim();
        }

        public string[] GetYear(string str)
        {
            string[] se = { "", "" };
            string[] rexs = {
                year
            };
            foreach (string rex in rexs)
            {
                var m = Regex.Match(str, rex, RegexOptions.IgnoreCase);
                if (m.Length > 0)
                {
                    
                    break;
                }
            }
            return se;
        }

        public Movie GetMovie(string str)
        {
            Movie movie = new Movie
            {
                Title = GetTitle(str),
                Codec = GetCodec(str),
                Group = GetGroup(str),
                Quality = GetQuality(str),
                Year = 0,
                Resolution = "",
                Audio = "",
                Category = 0
            };
            return movie;
        }
    }
}
