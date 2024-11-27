using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Data.SQLite;
using System.Data;


namespace VIO
{
    class AccountManager // Работа с аккаунтами
    {
        private string login;
        private string password;
        private string hashPassword;
        DatabaseManager database;

        public AccountManager(string login, string password) 
        {
            this.login = login;
            this.password = password; 
        }
        public int Authorization() // Авторизация пользователей
        {
            hashPassword = HashPassword(password);
            database = new DatabaseManager();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter();
            DataTable table = new DataTable();

            string querystring = $"SELECT IDLogin, Login, Password FROM Entrance WHERE Login = @login AND Password = @hashPassword";

            using (SQLiteCommand command = new SQLiteCommand(querystring, database.GetConnection()))
            {
                command.Parameters.AddWithValue("@login", login);
                command.Parameters.AddWithValue("@hashPassword", hashPassword);
                adapter.SelectCommand = command;
                adapter.Fill(table);
            }
            //if (table.Rows.Count == 1)
            //{
            //    return 1;
            //}
            //else
            //{
            //    return 0;
            //}
            return table.Rows.Count == 1 ? 1 : 0;
        }

        public int Registration() // Регистрация пользователй
        {
            hashPassword= HashPassword(password);
            database = new DatabaseManager();
            if (CheckUser())
            {
                return 2;
            }
            else
            {
                string querystring = $"INSERT INTO Entrance (Login, Password) VALUES (@login, @hashPassword)";

                using (SQLiteCommand command = new SQLiteCommand(querystring, database.GetConnection()))
                {
                    command.Parameters.AddWithValue("@login", login);
                    command.Parameters.AddWithValue("@hashPassword", hashPassword);

                    database.OpenConnection();
                    int result = command.ExecuteNonQuery();
                    database.CloseConnection();

                    return result == 1 ? 1 : 0;
                }
            }
        }

        private bool CheckUser() // Проверка на существование аккаунта
        {
            database = new DatabaseManager();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter();
            DataTable table = new DataTable();

            string querystring = $"SELECT IDLogin, Login FROM Entrance WHERE Login = @login";

            using (SQLiteCommand command = new SQLiteCommand(querystring, database.GetConnection()))
            {
                command.Parameters.AddWithValue("@login", login);
                adapter.SelectCommand = command;
                adapter.Fill(table);
            }

            return table.Rows.Count > 0;
        }
        public string HashPassword(string password)
        {
            MD5 md5 = MD5.Create();

            byte[]b = Encoding.ASCII.GetBytes(password);
            byte[]hash = md5.ComputeHash(b);

            StringBuilder sb = new StringBuilder();
            foreach(var a in hash)
                sb.Append(a.ToString("X2"));

            //hashPassword = Convert.ToString(password);
            return Convert.ToString(sb);
        }
    }
}
