using System;
using System.Data;
using System.Diagnostics;
using System.Configuration;
using Microsoft.Data.SqlClient;

namespace DatabaseService {
    public class Database
    {
        private Database()
        {
            dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStringMsSql"].ConnectionString);
            dbConnection.Open();
            transactionCount = 0;
        }

        public static Database Instance
        {
            get
            {
                return db.Value;
            }
        }

        public SqlCommand CreateCommand(string command)
        {
            var cmd = new SqlCommand(command, dbConnection);

            if (transaction != null)
                cmd.Transaction = transaction;

            return cmd;
        }

        public SqlDataReader ExecuteQuery(string command)
        {
            Debug.WriteLine($"Executing Query: {command}");

            return CreateCommand(command).ExecuteReader();
        }

        public SqlDataReader ExecuteQuery(SqlCommand command)
        {
            Debug.WriteLine($"Executing Query: {command.CommandText}");

            return command.ExecuteReader();
        }

        public object ExecuteScalar(Type type, string command)
        {
            Debug.WriteLine($"Executing scalar: {command}");

            return Helpers.ConvertType(CreateCommand(command).ExecuteScalar(), type);
        }

        public T ExecuteScalar<T>(string command)
        {
            return (T)ExecuteScalar(typeof(T), command);
        }

        public object ExecuteScalar(Type type, SqlCommand command)
        {
            Debug.WriteLine($"Executing scalar: {command.CommandText}");

            return Helpers.ConvertType(command.ExecuteScalar(), type);
        }

        public T ExecuteScalar<T>(SqlCommand command)
        {
            return (T)ExecuteScalar(typeof(T), command);
        }

        public int ExecuteCommand(string command)
        {
            Debug.WriteLine($"Executing command: {command}");

            return CreateCommand(command).ExecuteNonQuery();
        }

        public bool BeginTransaction()
        {
            if (transactionCount != 0)
            {
                transactionCount++;
                return false;
            }

            if (transaction == null)
            {
                transaction = dbConnection.BeginTransaction(IsolationLevel.Serializable);
                transactionCount++;
                return true;
            }

            return false;
        }

        public bool EndTransaction()
        {
            if (transaction == null)
                return false;

            if (--transactionCount > 0)
                return false;

            transaction.Commit();
            transaction = null;
            transactionCount = 0;

            return true;
        }

        public void Rollback()
        {
            if (transaction != null)
            {
                transaction.Rollback();
                transaction = null;
                transactionCount = 0;
            }
        }

        private SqlConnection dbConnection;
        private SqlTransaction transaction;
        private int transactionCount;
        private static readonly Lazy<Database> db = new Lazy<Database>(() => new Database());
    }
}