using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Text.Json;
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

            int largest = 0;
            foreach (var row in tbl) {
                if (!row.ContainsKey("id")) {
                    continue;
                }

                //var idElem = Helpers.ConvertType<JsonValueKind>(row["id"]);
                var strId = row["id"].ToString();
                var id = Helpers.ConvertType<int>(strId);

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

            var tbl = dbCache[table];
            var newId = GetBiggestIDInTable(table) + 1;

            value.Add("id", newId);
            tbl.Add(value);

            return newId;
        }

        public Option<Dictionary<string, object>> GetInTable(string table, int id) {
            if (!dbCache.ContainsKey(table)) {
                return Option<Dictionary<string, object>>.None;
            }

            var tbl = dbCache[table];

            foreach (var row in tbl) {
                if (!row.ContainsKey("id")) {
                    continue;
                }

                if (row["id"].Equals(id.ToString())) {
                    return Option<Dictionary<string, object>>.Some(row);
                }
            }

            return Option<Dictionary<string, object>>.None;
        }

        public void SetInTable(string table, int id, Dictionary<string, object> value) {
            if (!dbCache.ContainsKey(table)) {
                return;
            }

            var tbl = dbCache[table];

            for (var i = 0; i < tbl.Count; i++) {
                var row = tbl[i];

                if (!row.ContainsKey("id")) {
                    continue;
                }

                if (row["id"].Equals(id.ToString())) {
                    tbl[i] = value;
                    return;
                }
            }
        }

        private void LoadCache() {
            var file = File.ReadAllText(dbPath);
            dbCache = JsonSerializer.Deserialize<Dictionary<string, List<Dictionary<string, object>>>>(file);
        }

        private void SaveCache() {
            var result = JsonSerializer.Serialize(dbCache);
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

        public static JSONDatabase Instance {
            get => db.Value; 
        }

        private string dbPath;
        private Dictionary<string, List<Dictionary<string, object>>> dbCache;
        private int transactionCount;
        private static readonly Lazy<JSONDatabase> db = new Lazy<JSONDatabase>(() => new JSONDatabase());
    }
}