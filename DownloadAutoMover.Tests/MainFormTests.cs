using DownloadAutoMover.Classes;
using Xunit;

namespace DownloadAutoMover.Tests
{
    public class MainFormTests
    {
        public MainForm mainForm;

        public MainFormTests()
        {
            string dbName = "C:\\Users\\patrick.hewes\\Google Drive\\Workspace\\vs_source\\DownloadAutoMover\\DownloadAutoMover\\DownloadAutoMover.db";
            mainForm = new MainForm(dbName);
        }

        [Fact]
        public void Categories_Tests()
        {
            int i = 0;
            string[] values = { "Anime", "Anime_Movies", "Animated_Movies", "Movies", "TV", "TV_Babs", "TV_Colleen", "TV_Kids" };
            var results = mainForm.GetCategories();
            foreach (string value in values)
            {
                Assert.True(
                    results[i].Value.Equals(values[i++])
                    );
            }
        }

        [Fact]
        public void IgnoreItems_Tests()
        {
            int i = 0;
            string[] values = { "featurette", "sample" };
            int[] types = { 1, 1 };
            var results = mainForm.GetIgnoreItems();
            foreach (string value in values)
            {
                Assert.True(
                    results.Find(x => x.Value.Contains(value)).Type.Equals(types[i++])
                    );
            }
        }

        [Fact]
        public void MediaTypes_Tests()
        {
            int i = 0;
            string[] values = { ".mkv", ".mp4", ".srt", ".sub" };
            int[] types = { 0, 0, 1, 1 };
            var results = mainForm.GetMediaTypes();
            foreach (string value in values)
            {
                Assert.True(
                    results.Find(x => x.Value.Contains(value)).Type.Equals(types[i++])
                    );
            }
        }

        [Fact]
        public void RedirectItems_Tests()
        {
            int i = 0;
            string[] values = { "teen.titans.go", "spongebob.squarepants", "Outlander", "This.Is.Us", "Power", "Homeland", "Mcmillions", "One.Punch.Man", "Snowpiercer", "Hightown", "The.Chi", "Boruto(.*)?", "Fruits.Basket", "ill.be.gone.in.the.dark", "brave.new.world" };
            int[] types = { 7, 7, 6, 6, 5, 4, 5, 0, 4, 5, 5, 0, 0, 5, 5 };
            var results = mainForm.GetRedirectItems();
            foreach (string value in values)
            {
                Assert.True(
                    results.Find(x => x.Value.Contains(value)).Type.Equals(types[i++])
                    );
            }
        }

        [Fact]
        public void RenameItems_Tests()
        {
            int i = 0;
            string[] values = { "my.hero.academia", "mugen.no.juunin.(blade.of.the.immortal)", "shameless.us", "lego.masters.us", "Penny.dreadful.city.of.angels", "one.punch.man", "america.s.got.talent", "perry.mason", "ill.be.gone.in.the.dark", "brave.new.world" };
            string[] renames = { "Boku no Hero Academia", "Blade of the Immortal", "Shameless (US)", "Lego Masters", "Penny Dreadful- City of Angels", "One Punch Man", "America's Got Talent", "Perry Mason (2020)", "I'll Be Gone in the Dark", "Brave New World (2020)", };
            var results = mainForm.GetRenameItems();
            foreach (string value in values)
            {
                Assert.True(
                    results.Find(x => x.Value.Contains(value)).Rename.Equals(renames[i++])
                    );
            }
        }

        [Fact]
        public void Settings_Tests()
        {
            int i = 0;
            string[] descs = { "MonitorLocation", "MediaFolder", "IsDbSetup", "CleanFolders" };
            string[] values = { "D:\\Downloads\\Completed", "D:\\Videos", "True", "True", };
            var results = mainForm.GetSettings();
            foreach (string desc in descs)
            {
                Assert.True(results.Find(x => x.Description.Contains(desc)).Value.Equals(values[i]));
                i++;
            }
        }

        [Fact]
        public void SubFolders_Tests()
        {
            int i = 0;
            string[] values = { "Anime Shows", "Anime Movies", "Animated Movies", "Movies", "TV", "TV_Babs", "TV_Colleen", "TV_Kids" };
            var results = mainForm.GetSubFolders();
            foreach (string value in values)
            {
                Assert.True(
                    results[i].Value.Equals(values[i++])
                    );
            }
        }

        [Fact]
        public void CategorySelected_Tests()
        {
            int i = 0;
            string[,] values = new string[8,2] {
                { "Anime", "Anime Shows" }, 
                { "Anime_Movies", "Anime Movies" }, 
                { "Animated_Movies", "Animated Movies" }, 
                { "Movies", "Movies" },
                { "TV", "TV" },
                { "TV_Babs", "TV_Babs" },
                { "TV_Colleen", "TV_Colleen" },
                { "TV_Kids", "TV_Kids" }
            };
            var tabResults = mainForm.GetTab_Categories();
            foreach (TabCategories ctr in tabResults)
            {
                Assert.True(ctr.Category.Equals(values[i, 0]));
                Assert.True(ctr.SubFolder.Equals(values[i++, 1]));
            }
        }

        [Fact]
        public void RedirectSelected_Tests()
        {
            int i = 0;
            string[,] values = new string[15, 2] {
                { "teen.titans.go", "TV_Kids" },
                { "spongebob.squarepants", "TV_Kids" },
                { "Outlander", "TV_Colleen" },
                { "This.Is.Us", "TV_Colleen" },
                { "Power", "TV_Babs" },
                { "Homeland", "TV" },
                { "Mcmillions", "TV_Babs" },
                { "One.Punch.Man", "Anime Shows" },
                { "Snowpiercer", "TV" },
                { "Hightown", "TV_Babs" },
                { "The.Chi", "TV_Babs" },
                { "Boruto(.*)?", "Anime Shows" },
                { "Fruits.Basket", "Anime Shows" },
                { "ill.be.gone.in.the.dark", "TV_Babs" },
                { "brave.new.world", "TV_Babs" }
            };
            var tabResults = mainForm.GetTab_Redirects();
            foreach (TabRedirects x in tabResults)
            {
                Assert.True(x.RedItem.Equals(values[i, 0]));
                Assert.True(x.SubFolder.Equals(values[i++, 1]));
            }
        }

        [Fact]
        public void RenameSelected_Tests()
        {
            int i = 0;
            string[,] values = new string[10, 2] {
                { "my.hero.academia", "Boku no Hero Academia" },
                { "mugen.no.juunin.(blade.of.the.immortal)", "Blade of the Immortal" },
                { "shameless.us", "Shameless (US)" },
                { "lego.masters.us", "Lego Masters" },
                { "Penny.dreadful.city.of.angels", "Penny Dreadful- City of Angels" },
                { "one.punch.man", "One Punch Man" },
                { "america.s.got.talent", "America's Got Talent" },
                { "perry.mason", "Perry Mason (2020)" },
                { "ill.be.gone.in.the.dark", "I'll Be Gone in the Dark" },
                { "brave.new.world", "Brave New World (2020)" }
            };
            var tabResults = mainForm.GetTab_Renames();
            foreach (TabRenames x in tabResults)
            {
                Assert.True(x.Value.Equals(values[i, 0]));
                Assert.True(x.Rename.Equals(values[i++, 1]));
            }
        }

        [Fact]
        public void IgnoreSelected_Tests()
        {
            int i = 0;
            string[,] values = new string[2, 2] {
                { "featurette", "Enabled" },
                { "sample", "Enabled" }
            };
            var tabResults = mainForm.GetTab_Ignores();
            foreach (TabIgnores x in tabResults)
            {
                Assert.True(x.Ignores.Equals(values[i, 0]));
                Assert.True(x.Value.Equals(values[i++, 1]));
            }
        }
    }
}
