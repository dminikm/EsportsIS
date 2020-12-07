using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using Newtonsoft.Json.Linq;
using LanguageExt;

namespace DatabaseService {
    public class JSONDatabase {
        private JSONDatabase() {
            dbPath = ConfigurationManager.ConnectionStrings["ConnectionStringJSON"].ConnectionString;
            transactionCount = 0;
        }

        private int GetBiggestIDInTable(string table) {
            if (!dbCache.ContainsKey(table)) {
                return 0;
            }

            var tbl = dbCache[table];
            if (tbl.Type != JTokenType.Array) {
                return 0;
            }

            int largest = 0;
            foreach (var row in (tbl as JArray)) {
                if (!(row as JObject).ContainsKey("id")) {
                    continue;
                }

                var id = row["id"].Value<int>();

                if (id > largest) {
                    largest = id;
                }
            }

            return largest;
        }

        public int CreateInTable(string table, Dictionary<string, object> value) {
            if (!dbCache.ContainsKey(table)) {
                return -1;
            }

            var tbl = dbCache[table] as JArray;
            var newId = GetBiggestIDInTable(table) + 1;

            value.Add("id", newId);
            tbl.Add(JObject.FromObject(value));

            return newId;
        }

        public Option<Dictionary<string, object>> GetInTable(string table, int id) {
            if (!dbCache.ContainsKey(table)) {
                return Option<Dictionary<string, object>>.None;
            }

            var tbl = dbCache[table] as JArray;

            foreach (var row in tbl) {
                if (!(row as JObject).ContainsKey("id")) {
                    continue;
                }

                if (row["id"].ToString().Equals(id.ToString())) {
                    return (row as JObject).ToObject<Dictionary<string, object>>();
                }
            }

            return Option<Dictionary<string, object>>.None;
        }

        public List<Dictionary<string, object>> GetAllInTable(string table) {
            if (!dbCache.ContainsKey(table)) {
                return new List<Dictionary<string, object>>();
            }

            var tbl = dbCache[table] as JArray;

            var lst = new List<Dictionary<string, object>>();
            foreach (var row in tbl) {
                lst.Add((row as JObject).ToObject<Dictionary<string, object>>());
            }

            return lst;
        }

        public void SetInTable(string table, int id, Dictionary<string, object> value) {
            if (!dbCache.ContainsKey(table)) {
                return;
            }

            var tbl = dbCache[table] as JArray;

            for (var i = 0; i < tbl.Count; i++) {
                var row = tbl[i] as JObject;

                if (!row.ContainsKey("id")) {
                    continue;
                }

                if (row["id"].Value<int>() == id) {
                    tbl[i] = JObject.FromObject(value);
                    return;
                }
            }
        }

        public void RemoveFromTable(string table, int id) {
            if (!dbCache.ContainsKey(table)) {
                return;
            }

            var tbl = dbCache[table] as JArray;

            for (var i = 0; i < tbl.Count; i++) {
                var row = tbl[i] as JObject;

                if (!row.ContainsKey("id")) {
                    continue;
                }

                if (row["id"].Value<int>() == id) {
                    row.Remove();
                    return;
                }
            }
        }

        private void LoadCache() {
            var file = File.ReadAllText(dbPath);
            dbCache = JObject.Parse(file);
        }

        private void SaveCache() {
            var result = dbCache.ToString();
            File.WriteAllText(dbPath, result);
        }

        public void BeginTransaction() {
            transactionCount++;

            if (transactionCount == 1) {
                LoadCache();
            }
        }

        public void EndTransaction() {
            if (transactionCount > 0) {
                transactionCount--;
            }

            if (transactionCount == 0) {
                SaveCache();
            }
        }

        public void AbortTransaction() {
            transactionCount = 0;
            LoadCache();
        }

        public static JSONDatabase Instance {
            get => db.Value; 
        }

        private string dbPath;
        private JObject dbCache;
        private int transactionCount;
        private static readonly Lazy<JSONDatabase> db = new Lazy<JSONDatabase>(() => new JSONDatabase());
    }
}