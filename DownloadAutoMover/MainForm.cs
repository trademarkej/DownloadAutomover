using Microsoft.Data.Sqlite;
using DownloadAutoMover.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Routing.Constraints;

namespace DownloadAutoMover
{
    public class MainForm
    {
        private readonly string sqlDb;

        public MainForm(string sqlDb)
        {
            this.sqlDb = sqlDb;
        }

        public SqliteConnection GetSqliteConnection()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = sqlDb;
            return new SqliteConnection(connectionStringBuilder.ConnectionString);
        }

        private DataTable GetQueryResuls(string sqlCmd)
        {
            DataTable dt = new DataTable();
            using (var connection = GetSqliteConnection())
            {
                connection.Open();
                var selectCmd = connection.CreateCommand();
                selectCmd.CommandText = sqlCmd;

                using var reader = selectCmd.ExecuteReader();
                dt.Load(reader);
            }
            return dt;
        }

        public List<Category> GetCategories()
        {
            var list = new List<Category>();
            string sqlCmd = "SELECT * FROM Categories;";
            DataTable dt = GetQueryResuls(sqlCmd);
            foreach (DataRow dr in dt.Rows)
            {
                int i = 0;
                list.Add(
                    new Category
                    {
                        CatId = (int)dr.Field<long>(i++),
                        Value = dr.Field<string>(i++)
                    }
                );
            }
            return list;
        }

        public List<IgnoreItem> GetIgnoreItems()
        {
            var list = new List<IgnoreItem>();
            string sqlCmd = "SELECT * FROM IgnoreItems;";
            DataTable dt = GetQueryResuls(sqlCmd);
            foreach (DataRow dr in dt.Rows)
            {
                int i = 0;
                list.Add(
                    new IgnoreItem
                    {
                        IgnrId = (int)dr.Field<long>(i++),
                        Value = dr.Field<string>(i++),
                        Type = (int)dr.Field<long>(i)
                    }
                );
            }
            return list;
        }

        public List<MediaType> GetMediaTypes()
        {
            var list = new List<MediaType>();
            string sqlCmd = "SELECT * FROM MediaTypes;";
            DataTable dt = GetQueryResuls(sqlCmd);
            foreach (DataRow dr in dt.Rows)
            {
                int i = 0;
                list.Add(
                    new MediaType
                    {
                        MedId = (int)dr.Field<long>(i++),
                        Value = dr.Field<string>(i++),
                        Type = (int)dr.Field<long>(i)
                    }
                );
            }
            return list;
        }

        public List<RedirectItem> GetRedirectItems()
        {
            var list = new List<RedirectItem>();
            string sqlCmd = "SELECT * FROM RedirectItems;";
            DataTable dt = GetQueryResuls(sqlCmd);
            foreach (DataRow dr in dt.Rows)
            {
                int i = 0;
                list.Add(
                    new RedirectItem
                    {
                        RedId = (int)dr.Field<long>(i++),
                        Value = dr.Field<string>(i++),
                        Type = (int)dr.Field<long>(i)
                    }
                );
            }
            return list;
        }

        public List<RenameItem> GetRenameItems()
        {
            var list = new List<RenameItem>();
            string sqlCmd = "SELECT * FROM RenameItems;";
            DataTable dt = GetQueryResuls(sqlCmd);
            foreach (DataRow dr in dt.Rows)
            {
                int i = 0;
                list.Add(
                    new RenameItem
                    {
                        RenId = (int)dr.Field<long>(i++),
                        Value = dr.Field<string>(i++),
                        Rename = dr.Field<string>(i)
                    }
                );
            }
            return list;
        }

        public List<Setting> GetSettings()
        {
            var list = new List<Setting>();
            string sqlCmd = "SELECT * FROM Settings;";
            DataTable dt = GetQueryResuls(sqlCmd);
            foreach (DataRow dr in dt.Rows)
            {
                int i = 0;
                list.Add(
                    new Setting
                    {
                        SetId = (int)dr.Field<long>(i++),
                        Description = dr.Field<string>(i++),
                        Value = dr.Field<string>(i)
                    }
                );
            }
            return list;
        }

        public List<SubFolder> GetSubFolders()
        {
            var list = new List<SubFolder>();
            string sqlCmd = "SELECT * FROM SubFolders;";
            DataTable dt = GetQueryResuls(sqlCmd);
            foreach (DataRow dr in dt.Rows)
            {
                int i = 0;
                list.Add(
                    new SubFolder
                    {
                        SubId = (int)dr.Field<long>(i++),
                        Value = dr.Field<string>(i)
                    }
                );
            }
            return list;
        }

        public List<TabCategories> GetTab_Categories()
        {
            int i = 0;
            var list = new List<TabCategories>();
            var categories = GetCategories();
            var subFldrs = GetSubFolders();
            foreach (Category category in categories)
            {
                list.Add(
                    new TabCategories
                    {
                        Id = category.CatId,
                        Category = category.Value,
                        SubFolder = subFldrs[i++].Value
                    }
                );
            }
            return list;
        }

        public List<TabRedirects> GetTab_Redirects()
        {
            var list = new List<TabRedirects>();
            var redItems = GetRedirectItems();
            var subFldrs = GetSubFolders();
            foreach (RedirectItem redItem in redItems)
            {
                list.Add(
                    new TabRedirects
                    {
                        Id = redItem.RedId,
                        RedItem = redItem.Value,
                        SubFolder = subFldrs[redItem.Type].Value
                    }
                );
            }
            return list;
        }

        public List<TabRenames> GetTab_Renames()
        {
            var list = new List<TabRenames>();
            var renItems = GetRenameItems();
            foreach (RenameItem renItem in renItems)
            {
                list.Add(
                    new TabRenames
                    {
                        Id = renItem.RenId,
                        Value = renItem.Value,
                        Rename = renItem.Rename
                    }
                );
            }
            return list;
        }

        public List<TabIgnores> GetTab_Ignores()
        {
            var list = new List<TabIgnores>();
            var ignrItems = GetIgnoreItems();
            foreach (IgnoreItem ignrItem in ignrItems)
            {
                list.Add(
                    new TabIgnores
                    {
                        Id = ignrItem.IgnrId,
                        Ignores = ignrItem.Value,
                        Value = (ignrItem.Type == 0) ? "Disabled" : "Enabled"
                    }
                );
            }
            return list;
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

        public string FirstCharToUpper(string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }

        public string[] GetRegExPattern(string key)
        {
            return key.Split(';');
        }
    }
}
