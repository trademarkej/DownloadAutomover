using DownloadAutoMover.Classes;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text.RegularExpressions;

namespace DownloadAutoMover
{
    public class MediaFile
    {
        public MediaFile()
        {

        }

        public MediaFile(int i, string[] sfs, List<RedirectItem> ris, string[] res)
        {
            var rex = @"^(.*)?Movies(.*)?";
            aOrT = !Regex.IsMatch(sfs[i], rex, RegexOptions.IgnoreCase);
            this.sfs = sfs;
            this.ris = ris;
            this.res = res;
        }

        public string CapitalizeFirstLetters(string str)
        {
            string[] words = str.Split(' ');
            string newStr = "";
            foreach (string word in words)
            {
                string tmp = "";
                if (word.Length == 0)
                    Console.WriteLine("Empty String");
                else if (word.Length == 1)
                    tmp = char.ToUpper(word[0]).ToString();
                else
                    tmp = char.ToUpper(word[0]) + word.Substring(1);
                newStr += " " + tmp;
            }
            return newStr;
        }

        public string[] GetRegExPattern(string key)
        {
            return key.Split(';');
        }

        //----------------------------------------------------------
        // Get a new string based on the values of a RegEx array
        //----------------------------------------------------------
        private string getNewStringFromRexArr(string path, string[] arr)
        {
            string rexMatch = "";
            //new List<string>(arr).ForEach(val => {
            foreach (string val in arr)
            {
                if (Regex.IsMatch(path, val, RegexOptions.IgnoreCase))
                {
                    int matchCount = Regex.Matches(path, val).Count;
                    for (int i = 0; i < matchCount; i++)
                    {
                        rexMatch = Regex.Match(path, val, RegexOptions.IgnoreCase).ToString();
                        if (!rexMatch.Equals(""))
                            path = path.Replace(rexMatch, "").Trim();
                    }
                }
            }
            //);
            return path.TrimEnd('-').Trim();
        }

        //----------------------------------------------------------
        // Update the shows details if it's a redirect item
        //----------------------------------------------------------
        public int getRedirectValue(string showName)
        {
            bool rexMatch = false;
            int i = 0;
            foreach (RedirectItem ri in ris)
            {
                string[] tmpVals = ri.Value.Split(',');
                rexMatch = Regex.IsMatch(showName, tmpVals[0], RegexOptions.IgnoreCase);
                if (rexMatch)
                {
                    i = Int32.Parse(tmpVals[1]);
                    break;
                }
            }
            return i;
        }

        public string getRenameValue(string str)
        {
            string nStr = str;
            foreach (string val in res)
            {
                string[] tmpVals = val.Split(',');
                if (Regex.IsMatch(str, tmpVals[0], RegexOptions.IgnoreCase))
                {
                    nStr = str.Replace(Regex.Match(str, tmpVals[0], RegexOptions.IgnoreCase).ToString(), tmpVals[1]);
                    break;
                }
            }
            return nStr;
        }

        //----------------------------------------------------------
        // Get the details of the show based on the path
        //----------------------------------------------------------
        public string[] getShow(string[] arr)
        {
            FileInfo file = new FileInfo(arr[4]);
            string mPath = arr[4].Replace(file.Extension, "");
            string show = mPath.Replace(file.DirectoryName, "").TrimStart('\\');
            string showName = getShowName(show);
            string[] seasonEpisode = getShowSeasonEpisode(show);
            string showSeason = "Season " + seasonEpisode[1];
            string showEpisode = "S" + seasonEpisode[1] + "E" + seasonEpisode[2];
            string vidFldr = (getRedirectValue(showName) != 0) ? sfs[getRedirectValue(showName)] : arr[3];
            string fileName = (!Regex.IsMatch(showEpisode, @"S\d{2}E\d{2,3}")) ? file.Name : file.Name.Replace(seasonEpisode[0], showEpisode);
            string nDest = (file.DirectoryName).Replace(file.DirectoryName, arr[1]) + "\\" + vidFldr + "\\" + showName + "\\" + showSeason + "\\" + ((renameFile) ? fileName : getRenameValue(fileName));
            string subFldrs = vidFldr + "," + showName + "," + showSeason;
            return new string[] { nDest, subFldrs };
        }

        //----------------------------------------------------------
        // Get the show's episode
        //----------------------------------------------------------
        public string getShowEpisode(string path)
        {
            string[] arr1 = GetRegExPattern(appSettings.Get("ShowEpisode1"));
            string[] arr2 = GetRegExPattern(appSettings.Get("ShowEpisode2"));
            string episode = path;
            foreach (string val1 in arr1)
            {
                if (Regex.IsMatch(episode, val1, RegexOptions.IgnoreCase))
                {
                    string rexMatch = Regex.Match(episode, val1, RegexOptions.IgnoreCase).ToString().Trim();
                    episode = (getNewStringFromRexArr(rexMatch, arr2).Length == 1) ? "0" + getNewStringFromRexArr(rexMatch, arr2) : getNewStringFromRexArr(rexMatch, arr2);
                    break;
                }
            }
            return (!int.TryParse(episode, out _) && path.Length != 2 && episode.Equals(path)) ? "01" : episode;
        }

        //----------------------------------------------------------
        // Get the show's name
        //----------------------------------------------------------
        public string getShowName(string path)
        {
            string[] arr = GetRegExPattern(appSettings.Get("ShowName"));
            string showName = getRenameValue(CapitalizeFirstLetters(getNewStringFromRexArr(path, arr).Replace('.', ' ')).Trim());
            return showName;
        }

        public string[] getShowSeasonEpisode(string path)
        {
            string[] arr = GetRegExPattern(appSettings.Get("ShowSeasonEpisode"));
            string se = path;
            string rexMatch;
            int i = 0;
            foreach (string val1 in arr)
            {
                if (Regex.IsMatch(se, val1, RegexOptions.IgnoreCase))
                {
                    rexMatch = Regex.Match(se, val1, RegexOptions.IgnoreCase).ToString().Trim().Trim('.');
                    se = rexMatch;
                    break;
                }
                i++;
            }
            string season = getShowSeason(se);
            string episode = getShowEpisode(se);
            return new string[] { se, season, episode };
        }

        //----------------------------------------------------------
        // Get the show's name
        //----------------------------------------------------------
        public string getShowSeason(string path)
        {
            string[] arr1 = GetRegExPattern(appSettings.Get("ShowSeason1"));
            string[] arr2 = GetRegExPattern(appSettings.Get("ShowSeason2"));
            string season = path;
            foreach (string val1 in arr1)
            {
                if (Regex.IsMatch(season, val1, RegexOptions.IgnoreCase))
                {
                    string rexMatch = Regex.Match(season, val1, RegexOptions.IgnoreCase).ToString().Trim();
                    season = (getNewStringFromRexArr(rexMatch, arr2).Length == 1) ? "0" + getNewStringFromRexArr(rexMatch, arr2) : getNewStringFromRexArr(rexMatch, arr2);
                    break;
                }
            }
            return (season.Equals(path)) ? "01" : season;
        }

        public bool aOrT { get; set; }

        public string[] res { get; set; }

        public List<RedirectItem> ris { get; set; }

        public string[] sfs { get; set; }

        public bool renameFile { get; set; }

        public NameValueCollection appSettings { get; set; }
    }
}
