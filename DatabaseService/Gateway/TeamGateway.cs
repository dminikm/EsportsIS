using System;
using Microsoft.Data.SqlClient;
using DataTypes;
using LanguageExt;
using System.Data;
using System.Collections.Generic;

namespace DatabaseService
{
    namespace Gateway
    {
        public class TeamGateway
        {
            public static Team Create(string name, string game, Option<User> coach)
            {
                var db = Database.Instance;

                insertCommand.Parameters["@coach_id"].Value = coach.Map((x) => x.UserID.IfNone(-1) as object).IfNone(() => DBNull.Value);
                insertCommand.Parameters["@name"].Value = name;
                insertCommand.Parameters["@game"].Value = game;

                db.BeginTransaction();
                var id = db.ExecuteScalar<int>(insertCommand);
                db.EndTransaction();

                // Created value should always exist?
                return new Team()
                {
                    TeamID = id,
                    Game = game,
                    Name = name,
                    CoachID = coach.Match((x) => x.UserID, () => Option<int>.None)
                };
            }

            public static Option<Team> Find(int id)
            {
                var db = Database.Instance;

                findCommand.Parameters["@id"].Value = id;

                var result = db.ExecuteQuery(findCommand);

                var table = new DataTable();
                table.Load(result);

                result.Close();

                return ParseFromQuery(table, 0);
            }

            public static Option<Team> FindByCoach(int id)
            {
                var db = Database.Instance;

                findByCoachCommand.Parameters["@id"].Value = id;

                var result = db.ExecuteQuery(findByCoachCommand);

                var table = new DataTable();
                table.Load(result);

                result.Close();

                if (table.Rows.Count == 0)
                    return Option<Team>.None;

                return ParseFromQuery(table, 0);
            }

            public static Option<Team> FindByCoach(User user)
            {
                return user.UserID.Match((id) => FindByCoach(id), () => Option<Team>.None);
            }

            public static List<Team> FindAll()
            {
                var db = Database.Instance;

                var result = db.ExecuteQuery(findAllCommand);

                var table = new DataTable();
                table.Load(result);
                result.Close();

                var teams = new List<Team>();
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    ParseFromQuery(table, i).IfSome((x) => teams.Add(x));
                }

                return teams;
            }

            public static void Update(Team team)
            {
                var db = Database.Instance;

                var teamID = team.TeamID.IfNone(() => throw new InvalidCastException("UserID must have a value!"));

                updateCommand.Parameters["@id"].Value = teamID;
                updateCommand.Parameters["@coach_id"].Value = team.CoachID.Match((x) => x as object, () => DBNull.Value);
                updateCommand.Parameters["@name"].Value = team.Name;
                updateCommand.Parameters["@game"].Value = team.Game;

                db.BeginTransaction();
                db.ExecuteCommand(updateCommand);
                db.EndTransaction();
            }

            public static void Delete(int id)
            {
                var db = Database.Instance;
                deleteCommand.Parameters["@id"].Value = id;

                db.BeginTransaction();
                db.ExecuteCommand(deleteCommand);
                db.EndTransaction();
            }

            public static void Delete(Team team)
            {
                Delete(team.TeamID.IfNone(() => throw new InvalidCastException("TeamID must have a value!")));
            }

            private static Option<Team> ParseFromQuery(DataTable table, int rowNum)
            {
                if (table.Rows.Count <= rowNum)
                {
                    return Option<Team>.None;
                }

                var row = table.Rows[rowNum];

                if (table.Rows.Count == 0)
                {
                    return Option<Team>.None;
                }

                var team = new Team();

                team.TeamID = Helpers.ConvertType<int>(row[table.Columns[table.Columns.IndexOf("team_id")]].ToString());
                team.Name = Helpers.ConvertType<string>(row[table.Columns[table.Columns.IndexOf("name")]].ToString());
                team.Game = Helpers.ConvertType<string>(row[table.Columns[table.Columns.IndexOf("game")]].ToString());
                team.CoachID = Helpers.ConvertType<int?>(row[table.Columns[table.Columns.IndexOf("coach_id")]].ToString()).ToOption();

                return Option<Team>.Some(team);
            }

            /// Static constructor, prepare commands
            static TeamGateway()
            {
                var db = Database.Instance;

                // Prepare the insert command!
                insertCommand = db.CreateCommand(insertStatement);
                insertCommand.Parameters.Add("@coach_id", System.Data.SqlDbType.Int);
                insertCommand.Parameters.Add("@name", System.Data.SqlDbType.VarChar, 30);
                insertCommand.Parameters.Add("@game", System.Data.SqlDbType.VarChar, 30);
                insertCommand.Prepare();

                // Prepare the find all command!
                findAllCommand = db.CreateCommand(findAllStatement);
                findAllCommand.Prepare();

                // Prepare the find command
                findCommand = db.CreateCommand(findStatement);
                findCommand.Parameters.Add("@id", System.Data.SqlDbType.Int);
                findCommand.Prepare();

                findByCoachCommand = db.CreateCommand(findByCoachStatement);
                findByCoachCommand.Parameters.Add("@id", System.Data.SqlDbType.Int);
                findByCoachCommand.Prepare();

                // Prepare the update command!
                updateCommand = db.CreateCommand(updateStatement);
                updateCommand.Parameters.Add("@id", System.Data.SqlDbType.Int);
                updateCommand.Parameters.Add("@coach_id", System.Data.SqlDbType.Int);
                updateCommand.Parameters.Add("@name", System.Data.SqlDbType.VarChar, 30);
                updateCommand.Parameters.Add("@game", System.Data.SqlDbType.VarChar, 30);
                updateCommand.Prepare();

                // Prepare the delete command
                deleteCommand = db.CreateCommand(deleteStatement);
                deleteCommand.Parameters.Add("@id", System.Data.SqlDbType.Int);
                deleteCommand.Prepare();
            }

            // Generic stuff

            private static string insertStatement = "INSERT INTO [Team] VALUES (@coach_id, @name, @game); SELECT SCOPE_IDENTITY() AS newID;";
            private static SqlCommand insertCommand;

            private static string findAllStatement = "SELECT * FROM [Team];";
            private static SqlCommand findAllCommand;

            private static string findStatement = "SELECT * FROM [Team] WHERE [Team].[team_id] = @id;";
            private static SqlCommand findCommand;

            private static string findByCoachStatement = "SELECT * FROM [Team] where [Team].[coach_id] = @id";
            private static SqlCommand findByCoachCommand;

            private static string updateStatement = "UPDATE [Team] SET [Team].[coach_id] = @coach_id, [Team].[name] = @name, [Team].[game] = @game WHERE [Team].[team_id] = @id;";
            private static SqlCommand updateCommand;

            private static string deleteStatement = "DELETE FROM [Team] WHERE [Team].[team_id] = @id;";
            private static SqlCommand deleteCommand;
        }
    }
}