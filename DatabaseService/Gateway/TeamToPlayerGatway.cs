using System;
using Microsoft.Data.SqlClient;
using DataTypes;
using LanguageExt;
using System.Data;
using System.Collections.Generic;

namespace DatabaseService {
    namespace Gateway {
        public class TeamToPlayerGateway {
            public static TeamToPlayer Create(Team team, User user) {
                var db = Database.Instance;

                createCommand.Parameters["@team_id"].Value = team.TeamID.Value;
                createCommand.Parameters["@player_id"].Value = user.UserID.Value;

                // ID is (team_id + player_id), no id gets returned
                db.BeginTransaction();
                db.ExecuteCommand(createCommand);
                db.EndTransaction();

                return new TeamToPlayer() {
                    TeamID = team.TeamID,
                    PlayerID = user.UserID
                };
            }

            public static List<TeamToPlayer> FindByPlayer(User user) {
                var db = Database.Instance;

                findByPlayerCommand.Parameters["@id"].Value = user.UserID.Value;

                var result = db.ExecuteQuery(findByPlayerCommand);

                var table = new DataTable();
                table.Load(result);
                result.Close();

                var lst = new List<TeamToPlayer>();

                for (var i = 0; i < table.Rows.Count; i++) {
                    var parsed = ParseFromQuery(table, i);
                    parsed.IfSome((x) => lst.Add(x));
                }

                return lst;
            }

            public static List<TeamToPlayer> FindByTeam(Team team) {
                var db = Database.Instance;

                findByTeamCommand.Parameters["@id"].Value = team.TeamID.Value;

                var result = db.ExecuteQuery(findByTeamCommand);

                var table = new DataTable();
                table.Load(result);
                result.Close();

                var lst = new List<TeamToPlayer>();

                for (var i = 0; i < table.Rows.Count; i++) {
                    var parsed = ParseFromQuery(table, i);
                    parsed.IfSome((x) => lst.Add(x));
                }

                return lst;
            }

            public static void Delete(TeamToPlayer ttp) {
                var db = Database.Instance;

                deleteCommand.Parameters["@team_id"].Value = ttp.TeamID.Value;
                deleteCommand.Parameters["@player_id"].Value = ttp.PlayerID.Value;

                db.BeginTransaction();
                db.ExecuteCommand(deleteCommand);
                db.EndTransaction();
            }

            public static void DeleteAllForPlayer(User user) {
                var db = Database.Instance;

                deleteAllForPlayerCommand.Parameters["@player_id"].Value = user.UserID.Value;

                db.BeginTransaction();
                db.ExecuteCommand(deleteAllForPlayerCommand);
                db.EndTransaction();
            }

            public static void DeleteAllForTeam(Team team) {
                var db = Database.Instance;

                deleteAllForTeamCommand.Parameters["@team_id"].Value = team.TeamID.Value;

                db.BeginTransaction();
                db.ExecuteCommand(deleteAllForTeamCommand);
                db.EndTransaction();
            }

            private static Option<TeamToPlayer> ParseFromQuery(DataTable table, int rowNum) {
                var row = table.Rows[rowNum];

                if (table.Rows.Count == 0) {
                    return Option<TeamToPlayer>.None;
                }

                var ttp = new TeamToPlayer();

                ttp.TeamID = Helpers.ConvertType<int>(row[table.Columns[table.Columns.IndexOf("team_id")]].ToString());
                ttp.PlayerID = Helpers.ConvertType<int>(row[table.Columns[table.Columns.IndexOf("player_id")]].ToString());

                return Option<TeamToPlayer>.Some(ttp);
            }

            static TeamToPlayerGateway() {
                var db = Database.Instance;
                
                // Setup create
                createCommand = db.CreateCommand(createStatement);
                createCommand.Parameters.Add("@team_id", SqlDbType.Int);
                createCommand.Parameters.Add("@player_id", SqlDbType.Int);
                createCommand.Prepare();

                // Setup findByPlayer
                findByPlayerCommand = db.CreateCommand(findByPlayerStatement);
                findByPlayerCommand.Parameters.Add("@id", SqlDbType.Int);
                findByPlayerCommand.Prepare();

                // Setup findByTeam
                findByTeamCommand = db.CreateCommand(findByTeamStatement);
                findByTeamCommand.Parameters.Add("@id", SqlDbType.Int);
                findByTeamCommand.Prepare();

                // Setup delete
                deleteCommand = db.CreateCommand(deleteStatement);
                deleteCommand.Parameters.Add("@team_id", SqlDbType.Int);
                deleteCommand.Parameters.Add("@player_id", SqlDbType.Int);
                deleteCommand.Prepare();

                // Setup deleteAllForPlayer
                deleteAllForPlayerCommand = db.CreateCommand(deleteAllForPlayerStatement);
                deleteAllForPlayerCommand.Parameters.Add("@player_id", SqlDbType.Int);
                deleteAllForPlayerCommand.Prepare();

                // Setup deleteAllForTeam
                deleteAllForTeamCommand = db.CreateCommand(deleteAllForTeamStatement);
                deleteAllForTeamCommand.Parameters.Add("@team_id", SqlDbType.Int);
                deleteAllForTeamCommand.Prepare();
            }

            // Generic
            private static string createStatement = "INSERT INTO [TeamToPlayer]([team_id], [player_id]) VALUES (@team_id, @player_id);";
            private static SqlCommand createCommand;

            private static string findByPlayerStatement = "SELECT * FROM [TeamToPlayer] WHERE [TeamToPlayer].[player_id] = @id;";
            private static SqlCommand findByPlayerCommand;

            private static string findByTeamStatement = "SELECT * FROM [TeamToPlayer] WHERE [TeamToPlayer].[team_id] = @id;";
            private static SqlCommand findByTeamCommand;

            private static string deleteStatement = "DELETE FROM [TeamToPlayer] WHERE [TeamToPlayer].[team_id] = @team_id AND [TeamToPlayer].[player_id] = @player_id;";
            private static SqlCommand deleteCommand;

            private static string deleteAllForPlayerStatement = "DELETE FROM [TeamToPlayer] WHERE [TeamToPlayer].[player_id] = @player_id;";
            private static SqlCommand deleteAllForPlayerCommand;

            private static string deleteAllForTeamStatement = "DELETE FROM [TeamToPlayer] WHERE [TeamToPlayer].[team_id] = @team_id;";
            private static SqlCommand deleteAllForTeamCommand;
        }
    }
}