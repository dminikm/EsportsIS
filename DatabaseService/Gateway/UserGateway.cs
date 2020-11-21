using System;
using Microsoft.Data.SqlClient;
using DataTypes;
using LanguageExt;
using System.Data;

namespace DatabaseService
{
    namespace Gateway
    {
        public class UserGateway
        {
            public static string GetLoginFor(string firstName, string lastName)
            {
                var db = Database.Instance;

                var prefix = (lastName.Length >= 3) ?
                    lastName.Substring(0, 3) :
                    firstName.Substring(0, 3 - Math.Min(lastName.Length, 3)) + lastName.Substring(0, lastName.Length);

                prefix = prefix.ToLower();


                getHighestLoginCommand.Parameters["@prefix"].Value = prefix + '%';
                var highest = db.ExecuteScalar<int>(getHighestLoginCommand);

                return (prefix + (highest + 1).ToString().PadLeft(4, '0')).ToLower();
            }

            public static User Create(string login, string firstName, string lastName, string password, UserRole role)
            {
                var db = Database.Instance;

                insertCommand.Parameters["@first_name"].Value = firstName;
                insertCommand.Parameters["@last_name"].Value = lastName;
                insertCommand.Parameters["@login"].Value = login;
                insertCommand.Parameters["@password"].Value = password;
                insertCommand.Parameters["@role"].Value = UserRoleStrings.roles[role];

                db.BeginTransaction();
                var id = db.ExecuteScalar<int>(insertCommand);
                db.EndTransaction();

                // Created value should always exist?
                return new User()
                {
                    UserID = id,
                    FirstName = firstName,
                    LastName = lastName,
                    Login = login,
                    Password = password,
                    Role = role,
                };
            }

            public static Option<User> Select(int id)
            {
                var db = Database.Instance;

                selectCommand.Parameters["@id"].Value = id;

                var result = db.ExecuteQuery(selectCommand);

                var table = new DataTable();
                table.Load(result);

                result.Close();

                return ParseFromQuery(table, 0);
            }

            public static void Update(User user)
            {
                var db = Database.Instance;

                var userID = user.UserID.IfNone(() => throw new InvalidCastException("UserID must have a value!"));

                updateCommand.Parameters["@id"].Value = userID;
                updateCommand.Parameters["@first_name"].Value = user.FirstName;
                updateCommand.Parameters["@last_name"].Value = user.LastName;
                updateCommand.Parameters["@login"].Value = user.Login;
                updateCommand.Parameters["@password"].Value = user.Password;
                updateCommand.Parameters["@role"].Value = UserRoleStrings.roles[user.Role];

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

            public static void Delete(User user)
            {
                Delete(user.UserID.IfNone(() => throw new InvalidCastException("UserID must have a value!")));
            }

            private static Option<User> ParseFromQuery(DataTable table, int rowNum)
            {
                var row = table.Rows[rowNum];

                if (table.Rows.Count == 0)
                {
                    return Option<User>.None;
                }

                var user = new User();

                user.UserID = Helpers.ConvertType<int>(row[table.Columns[table.Columns.IndexOf("user_id")]].ToString());
                user.FirstName = Helpers.ConvertType<string>(row[table.Columns[table.Columns.IndexOf("first_name")]].ToString());
                user.LastName = Helpers.ConvertType<string>(row[table.Columns[table.Columns.IndexOf("last_name")]].ToString());
                user.Login = Helpers.ConvertType<string>(row[table.Columns[table.Columns.IndexOf("login")]].ToString());
                user.Password = Helpers.ConvertType<string>(row[table.Columns[table.Columns.IndexOf("password")]].ToString());
                user.Role = UserRoleStrings.rolesReverse[row[table.Columns[table.Columns.IndexOf("role")]].ToString()];

                return Option<User>.Some(user);
            }

            /// Static constructor, prepare commands
            static UserGateway()
            {
                var db = Database.Instance;

                getHighestLoginCommand = db.CreateCommand(getHighestLoginStatement);
                getHighestLoginCommand.Parameters.Add("@prefix", SqlDbType.VarChar, 4);
                getHighestLoginCommand.Prepare();

                // Prepare the insert command!
                insertCommand = db.CreateCommand(insertStatement);
                insertCommand.Parameters.Add("@first_name", System.Data.SqlDbType.VarChar, 30);
                insertCommand.Parameters.Add("@last_name", System.Data.SqlDbType.VarChar, 30);
                insertCommand.Parameters.Add("@login", System.Data.SqlDbType.VarChar, 7);
                insertCommand.Parameters.Add("@password", System.Data.SqlDbType.VarChar, 40);
                insertCommand.Parameters.Add("@role", System.Data.SqlDbType.VarChar, 10);
                insertCommand.Prepare();

                // Prepare the select command
                selectCommand = db.CreateCommand(selectStatement);
                selectCommand.Parameters.Add("@id", System.Data.SqlDbType.Int);
                selectCommand.Prepare();

                // Prepare the update command!
                updateCommand = db.CreateCommand(updateStatement);
                updateCommand.Parameters.Add("@id", System.Data.SqlDbType.Int);
                updateCommand.Parameters.Add("@first_name", System.Data.SqlDbType.VarChar, 30);
                updateCommand.Parameters.Add("@last_name", System.Data.SqlDbType.VarChar, 30);
                updateCommand.Parameters.Add("@login", System.Data.SqlDbType.VarChar, 7);
                updateCommand.Parameters.Add("@password", System.Data.SqlDbType.VarChar, 40);
                updateCommand.Parameters.Add("@role", System.Data.SqlDbType.VarChar, 10);
                updateCommand.Prepare();

                // Prepare the delete command
                deleteCommand = db.CreateCommand(deleteStatement);
                deleteCommand.Parameters.Add("@id", System.Data.SqlDbType.Int);
                deleteCommand.Prepare();
            }

            // Specialized stuff

            private static string getHighestLoginStatement = "SELECT ISNULL(MAX(CAST(SUBSTRING([login], 4, 4) as int)), 0) FROM [User] WHERE [User].[login] LIKE @prefix;";
            private static SqlCommand getHighestLoginCommand;

            // Generic stuff

            private static string insertStatement = "INSERT INTO [User] VALUES (@first_name, @last_name, @login, @password, @role); SELECT SCOPE_IDENTITY() AS newID;";
            private static SqlCommand insertCommand;

            private static string selectStatement = "SELECT * FROM [User] WHERE [User].[user_id] = @id;";
            private static SqlCommand selectCommand;

            private static string updateStatement = "UPDATE [User] SET [User].[first_name] = @first_name, [User].[last_name] = @last_name, [User].[login] = @login, [User].[password] = @password, [User].[role] = @role WHERE [User].[user_id] = @id;";
            private static SqlCommand updateCommand;

            private static string deleteStatement = "DELETE FROM [User] WHERE [User].[user_id] = @id;";
            private static SqlCommand deleteCommand;
        }
    }
}