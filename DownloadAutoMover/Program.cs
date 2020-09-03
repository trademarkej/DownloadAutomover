using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DownloadAutoMover;
using DownloadAutoMover.Classes;
using Microsoft.Extensions.Hosting;

namespace DownloadAutoMover
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var jsonString = File.ReadAllText("databases.json");
            dynamic jsonDbs = JObject.Parse(jsonString);

            string dbName = "DownloadAutoMover.db";
            if (File.Exists(dbName))
            {
                File.Delete(dbName);
            }
            using var dbContext = new MyDbContext();
            // Ensure database is created
            dbContext.Database.EnsureCreated();
            if (!dbContext.Categories.Any())
            {
                int i = 1;
                foreach (var json in jsonDbs.Categories)
                {
                    var tmp = JsonConvert.DeserializeObject(json.ToString());
                    var id = tmp.CatId == null ? i++ : tmp.CatId;
                    dbContext.Categories.AddRange(new Category[]
                    {
                        new Category{ CatId=id, Value=tmp.Value }
                    });
                }
            }
            if (!dbContext.IgnoreItems.Any())
            {
                int i = 1;
                foreach (var json in jsonDbs.IgnoreItems)
                {
                    var tmp = JsonConvert.DeserializeObject(json.ToString());
                    var id = tmp.IgnrId == null ? i++ : tmp.IgnrId;
                    dbContext.IgnoreItems.AddRange(new IgnoreItem[]
                    {
                        new IgnoreItem{ IgnrId=id, Value=tmp.Value, Type=tmp.Type }
                    });
                }
            }
            if (!dbContext.MediaTypes.Any())
            {
                int i = 1;
                foreach (var json in jsonDbs.MediaTypes)
                {
                    var tmp = JsonConvert.DeserializeObject(json.ToString());
                    var id = tmp.MedId == null ? i++ : tmp.MedId;
                    dbContext.MediaTypes.AddRange(new MediaType[]
                    {
                        new MediaType{ MedId=id, Value=tmp.Value, Type=tmp.Type }
                    });
                }
            }
            if (!dbContext.RedirectItems.Any())
            {
                int i = 1;
                foreach (var json in jsonDbs.RedirectItems)
                {
                    var tmp = JsonConvert.DeserializeObject(json.ToString());
                    var id = tmp.RedId == null ? i++ : tmp.RedId;
                    dbContext.RedirectItems.AddRange(new RedirectItem[]
                    {
                        new RedirectItem{ RedId=id, Value=tmp.Value, Type=tmp.Type }
                    });
                }
            }
            if (!dbContext.RenameItems.Any())
            {
                int i = 1;
                foreach (var json in jsonDbs.RenameItems)
                {
                    var tmp = JsonConvert.DeserializeObject(json.ToString());
                    var id = tmp.RenId == null ? i++ : tmp.RenId;
                    dbContext.RenameItems.AddRange(new RenameItem[]
                    {
                        new RenameItem{ RenId=id, Value=tmp.Value, Rename=tmp.Rename }
                    });
                }
            }
            if (!dbContext.Settings.Any())
            {
                int i = 1;
                foreach (var json in jsonDbs.Settings)
                {
                    var tmp = JsonConvert.DeserializeObject(json.ToString());
                    var id = tmp.SetId == null ? i++ : tmp.SetId;
                    dbContext.Settings.AddRange(new Setting[]
                    {
                        new Setting{ SetId=id, Description=tmp.Description, Value=tmp.Value }
                    });
                }
            }
            if (!dbContext.SubFolders.Any())
            {
                foreach (var json in jsonDbs.SubFolders)
                {
                    var tmp = JsonConvert.DeserializeObject(json.ToString());
                    dbContext.SubFolders.AddRange(new SubFolder[]
                    {
                        new SubFolder{ SubId=tmp.SubId, Value=tmp.Value }
                    });
                }
            }

            dbContext.SaveChanges();

            //var mainForm = new MainForm(dbName);
            //var test = mainForm.GetAppList();

            //CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
