using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace VIO
{
    //public interface ICalculation // Объявление интерфейса
    //{
    //    //double CalculationBmi();
    //}
    internal class Calculation// : ICalculation
    {
        AccountManager accountManager;
        DatabaseManager database;
        int height;
        float weight;
        int age;
        float coef;

        private void Data()
        {
            accountManager = AccountManager.getInstance();
            (age, height, weight, coef) = accountManager.Calc();


        }
        //public Calculation(double height, double weight, int age, int coef)
        //{
        //    this.height = height;
        //    this.weight = weight;
        //    this.age = age;
        //    this.coef = coef;
        //}

        //public void df ()
        //{
        //    string querySelect = "SELECT last_insert_rowid() FROM  Parameters WHERE IDUser = @IDUser";

        //    using (SQLiteCommand selectCommand = new SQLiteCommand(querySelect, database.GetConnection()))
        //    {
        //        command.Parameters.AddWithValue("@IDUser", idEntrance);
        //        //long lastID = (long)selectCommand.ExecuteScalar();
        //        //idEntrance = (int)lastID;
        //    }
        //}
        public double CalculationBmi()
        {
            Data();
            double bmi = weight / Math.Pow(height, 2) * 10000;

            return bmi;
        }
        public float CalculationCalories()
        {
            float calories = 0;

            calories = (float)((10 * weight) + (6.25 * height) - (5 * age) - 161);

            return calories;
        }

        public int CalculationWater()
        {
            int water = 0;

            water = 30 * (int)weight;

            return water;
        }

        public float IdealWeight()
        {
            float idealWeight = 0;

            idealWeight = (float)((height - 100 + (age / 10)) * 0.9 * coef); 
            //Идеальный вес = (рост(см) – 100 + (возраст / 10)) х 0.9 х коэффициент запястья

            return idealWeight;
        }

        /* Категории ИМТ
        16 и меньше - значительный недостаток массы тела
        16-18,4 - дефицит массы тела
        18,5-24,9 - норма
        25-29,9 - избыток массы тела
        30-34,9 - ожирение 1 степени
        35-39,9 - ожирение 2 степени
        от 40 - ожирение 3 степени
         */

        /* Норма ИМТ
        19-24 года - 19-24
        25-34 года - 20-25
        35-44 года - 21-26
        45-54 года - 22-27
        55-64 года - 23-28
        от 65 лет - 24-29
         */

        /* Риск для здоровья
        менее 18,4 - Высокий, необходимо лечение анорексии и увеличение массы тела
        18,5-22,9 - Отсутствует
        23-27,4 - Желательно снизить вес
        27,5-29,9 - Необходимо снизить вес
        30-39,9 - Крайне необходимо похудение
        от 40 - Необходимы срочные меры по снижению веса
         */
    }
}
