using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Data.SQLite;
using System.Data;
using System.Reflection;
using System.Xml.Linq;
using System.Windows.Media.Media3D;
using System.Data.Entity;
using System.Windows;
using System.Windows.Controls;



namespace VIO
{
    class AccountManager // Работа с аккаунтами
    {
        private static AccountManager? instance;
        private string? login; 
        private string? password;
        private string? hashPassword;
        private DatabaseManager database;
        private int idEntrance;
        private DateTime birthday;

        private AccountManager() 
        {
            database = new DatabaseManager();
        }
        public static AccountManager getInstance()
        {
            if (instance == null)
                instance = new AccountManager();
            return instance;
        }

        public static AccountManager getInstance(bool init)
        {
            if (instance == null || init == true)
                instance = new AccountManager();
            return instance;
        }

        public void SetUserCred(string login, string password)
        {
            this.login = login;
            this.password = password;
        }

        public int Authorization() // Авторизация пользователей
        {
            hashPassword = HashPassword();
            //database = new DatabaseManager();
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
            if (table.Rows.Count == 1)
            {
                idEntrance = Convert.ToInt32(table.Rows[0]["IDLogin"]);
                return idEntrance;
            }
            else return 0;
            //return table.Rows.Count == 1 ? 1 : 0;
        }

        public int Registration(string name, string birthday, int gender) // Регистрация пользователй
        {
            hashPassword= HashPassword();
            //database = new DatabaseManager();
            

            if (CheckUser())
            {
                return 2;
            }
            else
            {
                string queryEnsertEntrance = $"INSERT INTO Entrance (Login, Password) VALUES (@login, @hashPassword)";
                string querySelectLastID = "SELECT last_insert_rowid()";

                try
                {
                    database.OpenConnection();

                    using (SQLiteCommand command = new SQLiteCommand(queryEnsertEntrance, database.GetConnection()))
                    {
                        command.Parameters.AddWithValue("@login", login);
                        command.Parameters.AddWithValue("@hashPassword", hashPassword);

                        
                        int result = command.ExecuteNonQuery();

                        if (result == 1)
                        {
                            using (SQLiteCommand selectCommand = new SQLiteCommand(querySelectLastID, database.GetConnection()))
                            {
                                long lastID = (long)selectCommand.ExecuteScalar();
                                idEntrance = (int)lastID;
                            }
                            UserDataInsert(idEntrance, name, birthday, gender);

                            return 1;
                        }
                        else return 0;
                    }
                }
                finally
                {
                    database.CloseConnection();
                }
            }
        }

        private bool CheckUser() // Проверка на существование аккаунта
        {
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
        private string HashPassword() // Хеширование пароля
        {
            MD5 md5 = MD5.Create();

            byte[]b = Encoding.ASCII.GetBytes(password);
            byte[]hash = md5.ComputeHash(b);

            StringBuilder sb = new StringBuilder();
            foreach(var a in hash)
                sb.Append(a.ToString("X2"));

            return Convert.ToString(sb);
        }

        public void UserDataInsert(int idEntrance, string name, string birthday, int gender) // Запись данных пользователя
        {
            // Перечислим возможные форматы дат
            string[] formats = { "MM/dd/yyyy", "M/d/yyyy", "yyyy-MM-dd" }; // Попробуем преобразовать строку в DateTime, учитывая несколько возможных форматов
            DateTime birthDate; bool isValidDate = DateTime.TryParseExact(birthday, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out birthDate);

            string querystring = $"INSERT INTO UserData (IDEntrance, Name, Birthday, Gender) VALUES (@IDEntrance, @Name, @Birthday, @Gender)";

            using (SQLiteCommand command = new SQLiteCommand(querystring, database.GetConnection()))
            {
                command.Parameters.AddWithValue("@IDEntrance", idEntrance);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Birthday", birthDate);
                command.Parameters.AddWithValue("@Gender", gender);

                database.OpenConnection();
                int result = command.ExecuteNonQuery();
                database.CloseConnection();
            }
        }

        // Добавила
        public int UserDataSelect() // Запись данных пользователя
        {
            //// Перечислим возможные форматы дат
            string[] formats = { "MM/dd/yyyy", "M/d/yyyy", "yyyy-MM-dd" }; // Попробуем преобразовать строку в DateTime, учитывая несколько возможных форматов
            //DateTime birthDate; bool isValidDate = DateTime.TryParseExact(birthday, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out birthDate);

            string querystring = $"SELECT IDEntrance, Name, Birthday, Gender FROM UserData WHERE IDEntrance = @IDEntrance";

            using (SQLiteCommand command = new SQLiteCommand(querystring, database.GetConnection()))
            {
                command.Parameters.AddWithValue("@IDEntrance", idEntrance);

                database.OpenConnection();
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string name = reader["Name"].ToString();
                        birthday = Convert.ToDateTime(reader["Birthday"]);
                        //birthday = DateTime.ParseExact(, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
                        int gender = Convert.ToInt32(reader["Gender"]);
                        return gender;
                    }
                }
               
            }
            database.CloseConnection();
            return 2;
                
        }

        private int DateCount(DateTime recordingDate)
        {
            int dateCount = 0;

            string queryStringDate = "SELECT COUNT(*) FROM Parameters WHERE RecordingDate = @RecordingDate AND IDUser = @IDUser";

            using (SQLiteCommand command = new SQLiteCommand(queryStringDate, database.GetConnection()))
            {
                command.Parameters.AddWithValue("@RecordingDate", recordingDate);
                command.Parameters.AddWithValue("@IDUser", idEntrance);

                database.OpenConnection();
                dateCount = Convert.ToInt32(command.ExecuteScalar());
                database.CloseConnection();
            }

            return dateCount;
        }

        public int UserParameters(string recordingDate, int hight, float weight, float coef, int girthBreast, int girthWaist, int girthHips)
        {
            string[] formats = { "MM/dd/yyyy", "M/d/yyyy", "yyyy-MM-dd" }; // Попробуем преобразовать строку в DateTime, учитывая несколько возможных форматов
            DateTime recordingDateFormats; 
            bool isValidDate = DateTime.TryParseExact(recordingDate, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out recordingDateFormats);

            int dateCount = DateCount(recordingDateFormats);
            if (dateCount == 0)
            {
                string queryString = $"INSERT INTO Parameters (IDUser, RecordingDate, Hight, Weight, CoefGirthWrist,GirthBreast, GirthWaist, GirthHips) VALUES (@IDUser, @RecordingDate, @Hight, @Weight, @CoefGirthWrist, @GirthBreast, @GirthWaist, @GirthHips)";

                using (SQLiteCommand command = new SQLiteCommand(queryString, database.GetConnection()))
                {
                    command.Parameters.AddWithValue("@IDUser", idEntrance);
                    command.Parameters.AddWithValue("@RecordingDate", recordingDateFormats);
                    command.Parameters.AddWithValue("@Hight", hight);
                    command.Parameters.AddWithValue("@Weight", weight);
                    command.Parameters.AddWithValue("@CoefGirthWrist", coef);
                    command.Parameters.AddWithValue("@GirthBreast", girthBreast);
                    command.Parameters.AddWithValue("@GirthWaist", girthWaist);
                    command.Parameters.AddWithValue("@GirthHips", girthHips);

                    database.OpenConnection();
                    int result = command.ExecuteNonQuery();
                    database.CloseConnection();
                }
                return 0;
            }
            else if(dateCount > 0)
            {
                string queryString = "UPDATE Parameters SET IDUser = @IDUser, RecordingDate = @RecordingDate, Hight = @Hight, Weight = @Weight, CoefGirthWrist = @CoefGirthWrist,GirthBreast = @GirthBreast, GirthWaist = @GirthWaist, GirthHips = @GirthHips WHERE RecordingDate = @RecordingDate AND IDUser = @IDUser";

                using (SQLiteCommand command = new SQLiteCommand(queryString, database.GetConnection()))
                {
                    command.Parameters.AddWithValue("@IDUser", idEntrance);
                    command.Parameters.AddWithValue("@RecordingDate", recordingDateFormats);
                    command.Parameters.AddWithValue("@Hight", hight);
                    command.Parameters.AddWithValue("@Weight", weight);
                    command.Parameters.AddWithValue("@CoefGirthWrist", coef);
                    command.Parameters.AddWithValue("@GirthBreast", girthBreast);
                    command.Parameters.AddWithValue("@GirthWaist", girthWaist);
                    command.Parameters.AddWithValue("@GirthHips", girthHips);

                    database.OpenConnection();
                    int result = command.ExecuteNonQuery();
                    database.CloseConnection();
                }
                return 1;
            }
            else return 2;

        }
        // Добавила

        public (int age, int height, float weight, float coef) CalculateData() 
        {

            SQLiteDataAdapter adapter = new SQLiteDataAdapter();
            DataTable table = new DataTable();

            string querystring = $"SELECT * FROM Parameters WHERE IDUser = @IDUser ORDER BY rowid DESC LIMIT 1";

            using (SQLiteCommand command = new SQLiteCommand(querystring, database.GetConnection()))
            {
                command.Parameters.AddWithValue("@IDUser", idEntrance);

                adapter.SelectCommand = command;
                adapter.Fill(table);
            }
            if (table.Rows.Count == 1)
            {
                DateTime recordingDate = DateTime.Parse(table.Rows[0]["RecordingDate"].ToString());
                int height = Convert.ToInt32(table.Rows[0]["Hight"]);
                float weight = Convert.ToSingle(table.Rows[0]["Weight"]);
                float coef = Convert.ToSingle(table.Rows[0]["CoefGirthWrist"]);

                int age = recordingDate.Year - birthday.Year;

                if (recordingDate < birthday.AddYears(age)) { age--; }

                return (age, height, weight, coef);
            }

            return (0, 0, 0, 0);
        }

        public List<object> GettingAllParameters()
        {
            List<object> records = new List<object>(); 
            SQLiteDataAdapter adapter = new SQLiteDataAdapter();
            DataTable table = new DataTable();

            string querystring = "SELECT * FROM Parameters WHERE IDUser = @IDUser";
            using (SQLiteCommand command = new SQLiteCommand(querystring, database.GetConnection())) // замените connectionString на вашу строку подключения
            {
                
                command.Parameters.AddWithValue("@IDUser", idEntrance); 
                adapter.SelectCommand = command;
                adapter.Fill(table);
            }

            foreach (DataRow row in table.Rows)
            {
                DateTime recordingDate = DateTime.Parse(row["RecordingDate"].ToString());
                int height = Convert.ToInt32(row["Hight"]);
                float weight = Convert.ToSingle(row["Weight"]);
                float coef = Convert.ToSingle(row["CoefGirthWrist"]);
                int girthBreast = Convert.ToInt32(row["GirthBreast"]);
                int girthWaist = Convert.ToInt32(row["GirthWaist"]);
                int girthHips = Convert.ToInt32(row["GirthHips"]);


                records.Add(new
                {
                    RecordingDate = recordingDate,
                    Height = height,
                    Weight = weight,
                    CoefGirthWrist = coef,
                    GirthBreast = girthBreast,
                    GirthWaist = girthWaist,
                    GirthHips = girthHips 
                });
            }
            return records;
        }
    }
}


