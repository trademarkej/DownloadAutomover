using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DownloadAutoMover
{
    public class FileUtils
    {
        EpisodeParser episodeParser = new EpisodeParser();

        //----------------------------------------------------------
        // Cleanup sub-folders from main Category directory
        //----------------------------------------------------------
        private void CleanupFolders(string path)
        {
            var dirs = new DirectoryInfo(path).EnumerateDirectories().OrderBy(d => d.Name);
            if (ClnFldrs)
            {
                foreach (var dir in dirs)
                {
                    string d = path + "\\" + dir.ToString() + "\\";
                    DeleteFolder(d);
                }
                if (!Directory.EnumerateFileSystemEntries(path).Any())
                {
                    DeleteFolder(path);
                }
            }
        }

        //----------------------------------------------------------
        // Parses thru all torrents and deletes
        // ones older than the offset
        //----------------------------------------------------------
        public bool CleanupTorrents(string path)
        {
            bool b = false;
            string ext = ".torrent";
            int offset;
            int i = 0;
            if (Debug)
            {
                offset = -1;
                path = DebugCloneFolder(path, " - Copy", ext);
            }
            else
            {
                offset = -7;
            }
            DateTime limit = DateTime.Today.AddDays(offset);

            if (Directory.Exists(path))
            {
                FileInfo[] files = new DirectoryInfo(path).GetFiles("*" + ext);
                foreach (FileInfo file in files)
                {
                    if (file.CreationTime < limit || file.LastWriteTime < limit)
                    {
                        File.Delete(file.FullName);
                        b = true;
                        i++;
                    }
                }
                if (i > 0 && UseLog)
                    new LogWriter(LogLocation, "[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "] Cleaned " + i + "/" + files.Length + " files");
            }
            return b;
        }

        //----------------------------------------------------------
        // Create folder from path
        //----------------------------------------------------------
        public bool CreateFolder(string path)
        {
            bool fldrExists = false;
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    if (UseLog)
                        new LogWriter(LogLocation, "[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "] " + path);
                }
                fldrExists = Directory.Exists(path);
            }
            catch (Exception)
            {

            }
            return fldrExists;
        }

        private string DebugCloneFolder(string path, string p2,string ext)
        {
            DirectoryInfo dir = new DirectoryInfo(path + p2);
            
            if (!dir.Exists)
            {
                CreateFolder(path + p2);
            }
            FileInfo[] files = new DirectoryInfo(path).GetFiles("*" + ext);
            path += p2;
            foreach (FileInfo file in files)
            {
                file.CopyTo(Path.Combine(path, file.Name), true);
            }
            return path;
        }

        //----------------------------------------------------------
        // Delete the folder from path if !debug and write to log
        //----------------------------------------------------------
        private void DeleteFolder(string path)
        {
            if (!Debug && Directory.Exists(path))
                Directory.Delete(path, true);
            if (UseLog)
                new LogWriter(LogLocation, "[" + System.Reflection.MethodBase.GetCurrentMethod().Name + ((Debug) ? " (Debug)" : "") + "] " + path);
        }

        public string FirstCharToUpper(string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }

        //----------------------------------------------------------
        // Check if the path is an ignored folder
        //----------------------------------------------------------
        public bool IsIgnored(string path)
        {
            int i = 0;
            foreach (string ig in IgList)
            {
                FileInfo fi = new FileInfo(path);
                if (Regex.IsMatch(path, ig, RegexOptions.IgnoreCase))
                {
                    if (UseLog)
                        new LogWriter(LogLocation, "[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "] " + path + " | " + ig.ToUpper());
                    i++;
                }
            }
            return (i > 0) ? true : false;
        }

        //----------------------------------------------------------
        // Get recurisve list of files in the path
        //----------------------------------------------------------
        public IEnumerable<string> GetFiles(string path)
        {
            IEnumerable<string> files = Directory
                    .EnumerateFiles(path, "*.*", SearchOption.AllDirectories)
                    .Where(s => MediaTypes.Contains(Path.GetExtension(s).ToLowerInvariant()))
                    .Where(s => !IsIgnored(s));
            return files;
        }

        public string[] GetRegExPattern(string key)
        {
            return key.Split(';');
        }

        //----------------------------------------------------------
        // Copy / Move the file if debug = true and writes to the log
        //----------------------------------------------------------
        private void MoveFile(string file, string showDest)
        {
            if (Debug)
            {
                File.Copy(@file, showDest, true);
            }
            else
            {
                if (File.Exists(showDest))
                    File.Delete(showDest);
                File.Move(@file, @showDest);
            }
            if (UseLog)
                new LogWriter(LogLocation, "[" + System.Reflection.MethodBase.GetCurrentMethod().Name + ((Debug) ? " (Copy)" : "") + "] " + file + " => " + showDest);
        }

        //https://www.npmjs.com/package/episode-parser
        public void ParseFilesNew(string source, string dest)
        {
            int i = 0;
            int j = 0;
            CatLists.ForEach(dir =>
            {
                string fp = Path.Combine(source, dir);
                if (Directory.Exists(fp))
                {
                    var files = GetFiles(fp);
                    foreach (string file in files)
                    {
                        FileInfo fi = new FileInfo(file);
                        var episode = episodeParser.GetTvEpisode(fi.Name);
                        string msf = SfLists[i] + '\\' + fi.Name;
                        string mDest = Path.Combine(dest, msf);
                    }
                }
            });
        }

        //----------------------------------------------------------
        // Parses thru all files in the source folder 
        // and move media files to the dest
        //----------------------------------------------------------
        public void ParseFiles(string source, string dest)
        {
            int i = 0;
            int j = 0;
            CatLists.ForEach(dir => {
                string fp = Path.Combine(source, dir);
                if (Directory.Exists(fp))
                {
                    var files = GetFiles(fp);
                    foreach (string file in files)
                    {
                        FileInfo fi = new FileInfo(file);
                        string msf = SfLists[i] + '\\' + fi.Name;
                        string mDest = Path.Combine(dest, msf);
                        MediaFile mediaFile = new MediaFile(i, SfLists, RiLists, RenLists);
                        mediaFile.appSettings = ConfigurationManager.AppSettings;
                        string showDest = mDest;
                        if (mediaFile.aOrT)
                        {
                            string[] details = new string[] { source, dest, dir, SfLists[i], file };
                            string[] show = mediaFile.getShow(details);
                            string dn = new FileInfo(show[0]).DirectoryName;
                            if (!Directory.Exists(dn))
                            {
                                string tmp = dest;
                                foreach (string ndir in show[1].Split(','))
                                {
                                    tmp = System.IO.Path.Combine(tmp, ndir);
                                    CreateFolder(tmp);
                                }
                            }
                            showDest = show[0];
                        }
                        else if (!Directory.Exists(System.IO.Path.Combine(dest, SfLists[i])))
                        {
                            CreateFolder(System.IO.Path.Combine(dest, SfLists[i]));
                        }
                        MoveFile(file, showDest);
                        j++;
                    }
                    CleanupFolders(fp);
                }
                i++;
            });
            MoveCount = j;
        }

        public List<string> ProcessDirectory(string targetDirectory)
        {
            List<string> list = new List<string>();
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
            {
                list.Add(fileName);
            }

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);

            return list;
        }

        public List<string> CatLists { get; set; }
        public bool ClnFldrs { get; set; }
        public bool Debug { get; set; }
        public string[] IgList { get; set; }
        public string LogLocation { get; set; }
        public List<string> MediaTypes { get; set; }
        public int MoveCount { get; set; }
        public string[] RiLists { get; set; }
        public string[] RenLists { get; set; }
        public string[] SfLists { get; set; }
        public bool UseLog { get; set; }
    }
}
