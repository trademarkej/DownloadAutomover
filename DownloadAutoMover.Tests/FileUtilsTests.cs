using DownloadAutoMover.Classes;
using System.Linq;
using Xunit;
using System.Text.RegularExpressions;

namespace DownloadAutoMover.Tests
{
    public class FileUtilsTests
    {
        private MainForm mainForm;
        private FileUtils fileUtils;
        readonly string path = "D:\\Downloads\\";

        public FileUtilsTests()
        {
            string dbName = "C:\\Users\\patrick.hewes\\Google Drive\\Workspace\\vs_source\\DownloadAutoMover\\DownloadAutoMover\\DownloadAutoMover.db";
            mainForm = new MainForm(dbName);
            fileUtils = new FileUtils(dbName)
            {
                Debug = true,
                LogLocation = path + "Torrents\\Logs",
                UseLog = false
            };
        }

        private void InitializeMainForm()
        {
            // Valid MediaTypes are Type = 0
            fileUtils.MediaTypes = mainForm.GetMediaTypes().FindAll(x => x.Type.Equals(0)).Select(x => x.Value).ToList();
            // Valid IgnoreList are Type = 1
            fileUtils.IgList = mainForm.GetIgnoreItems().FindAll(x => x.Type.Equals(1)).Select(x => x.Value).ToArray();
            fileUtils.CatLists = mainForm.GetCategories().Select(x => x.Value).ToList();
            fileUtils.RenLists = mainForm.GetRenameItems().Select(x => x.Value).ToArray();
            fileUtils.RiLists = mainForm.GetRedirectItems();
            fileUtils.SfLists = mainForm.GetSubFolders().Select(x => x.Value).ToArray();
        }

        [Fact]
        public void FolderContentsTorrents_Tests()
        {
            var fileList = fileUtils.ProcessDirectory(path + "Torrents\\");
            Assert.True(fileList.Count() > 0);
        }

        [Fact]
        public void FirstCharToUpper_Tests()
        {
            /* https://github.com/guessit-io/guessit */

            int i = 0;
            string[] values = { "test", "blah" };
            string[] results = { "Test", "Blah" };
            foreach (string value in values)
            {
                Assert.True(results[i++].Equals(fileUtils.FirstCharToUpper(value)));
            }
        }

        [Fact]
        public void FolderContents_Tests()
        {
            int i = 0;
            string[] values = { "New Text Document.txt" };
            var fileList = fileUtils.ProcessDirectory(path);
            foreach (string file in fileList)
            {
                Assert.False(file.Equals(path + "\\Torrents\\" + values[i++]));
            }
        }

        [Fact]
        public void GetFiles_Tests()
        {
            InitializeMainForm();
            var files = fileUtils.GetFiles(path + "Completed");
            foreach (string file in files)
            {
                var tmp = Regex.Match(file, @"(.*)?\.(avi|mkv|mp4)").ToString();
                Assert.True(tmp.Length>0);
            }
        }

        [Fact]
        public void CleanupTorrents_Tests()
        {
            Assert.True(fileUtils.CleanupTorrents(path + "Torrents"));
        }

        [Fact]
        public void IsIgnored_Tests()
        {
            int i = 0;
            var results = mainForm.GetIgnoreItems().FindAll(x => x.Type.Equals(1));
            string[] values = { "featurette", "sample" };
            foreach (IgnoreItem result in results)
            {
                Assert.Contains(values[i++], result.Value);
            }
        }

        [Fact]
        public void ParseFiles_Tests()
        {
            InitializeMainForm();
            string src = mainForm.GetSettings().Find(x => x.Description.Equals("MonitorLocation")).Value.ToString();
            string dest = mainForm.GetSettings().Find(x => x.Description.Equals("MediaFolder")).Value.ToString();
            fileUtils.ParseFilesNew(src, dest);
        }
    }
}
