using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIO
{
    /** 
     * \class MealPlan
     * \brief Класс для создания плана питания 
     */
    internal class MealPlan // Класс для создания плана питания
    {
        DatabaseManager database;
        int resultRB;

        public MealPlan(int resultRB) 
        {
            database = new DatabaseManager();
            this.resultRB = resultRB;
        }

        /** 
         * \brief Создаёт случайный план питания 
         */
        public List<(int Id, string ProductName, int Vegan, int Vegetarian, int Standard, int Breakfast, int Lunch, int Dinner, int Snack)> GetRandomMealPlan() // Создание случайного плана питания
        {
            var breakfasts = GetMealsByType("Breakfast");
            var lunches = GetMealsByType("Lunch");
            var dinners = GetMealsByType("Dinner");
            var snacks = GetMealsByType("Snack");

            var dailyPlan = new List<(int, string, int, int, int, int, int, int, int)>();
            var random = new Random();

            // Выбираем случайные продукты для каждого приема пищи
            if (breakfasts.Count > 0) dailyPlan.Add(breakfasts[random.Next(breakfasts.Count)]);
            if (lunches.Count > 0) dailyPlan.Add(lunches[random.Next(lunches.Count)]);
            if (dinners.Count > 0) dailyPlan.Add(dinners[random.Next(dinners.Count)]);
            if (snacks.Count > 0) dailyPlan.Add(snacks[random.Next(snacks.Count)]);

            return dailyPlan;
        }

        /** 
         * \brief Проверяет, какой тип питания выбран
         */
        private List<(int Id, string ProductName, int Vegan, int Vegetarian, int Standard, int Breakfast, int Lunch, int Dinner, int Snack)> GetMealsByType(string mealType) // Проверка на тип питания
        {
            var meals = new List<(int, string, int, int, int, int, int, int, int)>(); 
            string column = mealType; 
            string queryString = $"SELECT * FROM MealPlan WHERE {column} = 1";
            using (SQLiteCommand command = new SQLiteCommand(queryString, database.GetConnection()))
            {
                database.OpenConnection(); 
                using (SQLiteDataReader reader = command.ExecuteReader()) 
                {
                    while (reader.Read())
                    {
                        // Создание кортежа с информацией о текущем продукте
                        var meal = (Id: reader.GetInt32(0), 
                            ProductName: reader.GetString(1), 
                            Vegan: reader.GetInt32(2), 
                            Vegetarian: reader.GetInt32(3), 
                            Standard: reader.GetInt32(4), 
                            Breakfast: reader.GetInt32(5), 
                            Lunch: reader.GetInt32(6), 
                            Dinner: reader.GetInt32(7), 
                            Snack: reader.GetInt32(8));

                        // Фильтрация продуктов в зависимости от типа питания
                        if ((resultRB == 1 && meal.Vegetarian == 1) || 
                            (resultRB == 2 && meal.Vegan == 1) || 
                            (resultRB == 3 && meal.Standard == 1)) 
                        {
                            meals.Add(meal); 
                        }
                    }
                }
                database.CloseConnection(); 
            } 
            return meals; 
        }
    }
}
