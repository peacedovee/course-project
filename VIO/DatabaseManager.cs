using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace VIO
{
    class DatabaseManager // Работа с БД
    {
        SQLiteConnection sqliteConnection;

        public DatabaseManager()
        {
            string connectionString = @"Data Source=HealthCare.db;Version=3;";
            sqliteConnection = new SQLiteConnection(connectionString);
        }

        public void OpenConnection()
        {
            if (sqliteConnection.State == ConnectionState.Closed)
            {
                sqliteConnection.Open();
            }
        }

        public void CloseConnection()
        {
            if (sqliteConnection.State == ConnectionState.Open)
            {
                sqliteConnection.Close();
            }
        }

        public SQLiteConnection GetConnection()
        {
            return sqliteConnection;
        }

    }
}
