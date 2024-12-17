using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace VIO
{
    /** 
     * \class DatabaseManager
     * \brief Класс для работы с базой данных
     */
    class DatabaseManager // Класс для работы с базой данных
    {
        SQLiteConnection sqliteConnection;

        public DatabaseManager() 
        {
            string connectionString = @"Data Source=HealthCare.db;Version=3;";
            sqliteConnection = new SQLiteConnection(connectionString);
        }

        /** 
        * \brief Открытие подключения
        */
        public void OpenConnection() // Открытие подключения
        {
            if (sqliteConnection.State == ConnectionState.Closed)
            {
                sqliteConnection.Open();
            }
        }

        /** 
        * \brief Закрытие подключения
        */
        public void CloseConnection() // Закрытие подключения
        {
            if (sqliteConnection.State == ConnectionState.Open)
            {
                sqliteConnection.Close();
            }
        }

        /** 
        * \brief Получение подключения
        */
        public SQLiteConnection GetConnection() // Получение подключения
        {
            return sqliteConnection;
        }
    }
}
