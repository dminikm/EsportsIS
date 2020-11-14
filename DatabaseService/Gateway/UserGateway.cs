using System;
using Microsoft.Data.SqlClient;
using DataTypes;
using LanguageExt;
using System.Data;

namespace DatabaseService {
    namespace Gateway {
        public class UserGateway {
            public static User Create(string firstName, string lastName, string password, UserRole role) {                
                var db = Database.Instance;

                var login = lastName.Substring(0, 3) + "0000";

                // TODO: Properly setup ID
                UserGateway.insertCommand.Parameters["@first_name"].Value = firstName;
                UserGateway.insertCommand.Parameters["@last_name"].Value = lastName;
                UserGateway.insertCommand.Parameters["@login"].Value = login;
                UserGateway.insertCommand.Parameters["@password"].Value = password;
                UserGateway.insertCommand.Parameters["@role"].Value = UserRoleStrings.roles[role];

                var id = db.ExecuteScalar<int>(UserGateway.insertCommand);

                // Created value should always exist?
                return new User() {
                    ID = id,
                    FirstName = firstName,
                    LastName = lastName,
                    Login = login,
                    Password = password,
                    Role = role,
                };
            }

            public static Option<User> Select(int id) {
                var db = Database.Instance;

                UserGateway.selectCommand.Parameters["@id"].Value = id;

                var result = db.ExecuteQuery(UserGateway.selectCommand);

                var table = new DataTable();
                table.Load(result);

                result.Close();

                return UserGateway.ParseFromQuery(table, 0);
            }

            private static Option<User> ParseFromQuery(DataTable table, int rowNum) {
                var row = table.Rows[rowNum];

                if (table.Rows.Count == 0) {
                    return Option<User>.None;
                }

                var user = new User();

                user.ID = Helpers.ConvertType<int>(row[table.Columns[table.Columns.IndexOf("id")]].ToString());
                user.FirstName = Helpers.ConvertType<string>(row[table.Columns[table.Columns.IndexOf("first_name")]].ToString());
                user.LastName = Helpers.ConvertType<string>(row[table.Columns[table.Columns.IndexOf("last_name")]].ToString());
                user.Login = Helpers.ConvertType<string>(row[table.Columns[table.Columns.IndexOf("login")]].ToString());
                user.Password = Helpers.ConvertType<string>(row[table.Columns[table.Columns.IndexOf("password")]].ToString());
                user.Role = UserRoleStrings.rolesReverse[row[table.Columns[table.Columns.IndexOf("role")]].ToString()];

                return Option<User>.Some(user);
            }

            /// Static constructor, prepare commands
            static UserGateway() {
                var db = Database.Instance;

                // Prepare the insert command!
                UserGateway.insertCommand = db.CreateCommand(insertStatement);
                UserGateway.insertCommand.Parameters.Add("@first_name", System.Data.SqlDbType.VarChar);
                UserGateway.insertCommand.Parameters.Add("@last_name", System.Data.SqlDbType.VarChar);
                UserGateway.insertCommand.Parameters.Add("@login", System.Data.SqlDbType.VarChar);
                UserGateway.insertCommand.Parameters.Add("@password", System.Data.SqlDbType.VarChar);
                UserGateway.insertCommand.Parameters.Add("@role", System.Data.SqlDbType.VarChar);
                UserGateway.insertCommand.Prepare();

                // Prepare the select command
                UserGateway.selectCommand = db.CreateCommand(selectStatement);
                UserGateway.selectCommand.Parameters.Add("@id", System.Data.SqlDbType.Int);
                UserGateway.selectCommand.Prepare();
            }

            private static string insertStatement = "INSERT INTO [User] VALUES (@first_name, @last_name, @login, @password, @role); SELECT SCOPE_IDENTITY() AS newID;";
            private static SqlCommand insertCommand;

            private static string selectStatement = "SELECT * FROM [User] WHERE [User].[user_id] = @id;";
            private static SqlCommand selectCommand;
        }
    }
}