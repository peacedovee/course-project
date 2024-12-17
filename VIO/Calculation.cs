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
    /** 
    * \class Calculation
    * \brief Класс для вычисления данных
    */
    public class Calculation  // Класс для вычисления данных
    {
        AccountManager accountManager;
        int height;
        float weight;
        int age;
        float coef;

         /** 
         * \brief Устанавливает значения 
         */
        public void SetData(int age, int height, float weight, float coef) // Установка значений
        {
            this.age = age; 
            this.height = height; 
            this.weight = weight; 
            this.coef = coef;
        }

         /** 
         * \brief Получает данные из другого класса
         */
        public virtual void Data() // Получение данных из AccountManager
        {
            accountManager = AccountManager.getInstance();
            (age, height, weight, coef) = accountManager.CalculateData();
        }

         /** 
         * \brief Рассчитывает индекс массы тела 
         */
        public double CalculationBmi() // Расчёт ИМТ
        {
            Data();

            if (weight == 0 || height == 0)
            {
                return 0;
            }

            double bmi = weight / Math.Pow(height, 2) * 10000;
            return bmi;
        }

         /** 
         * \brief Расчитывает суточное потребление калорий 
         */
        public float CalculationCalories(int gender) // Расчёт калорий
        {
            float calories = 0;

            if (gender == 0)
            {
                calories = (float)((10 * weight) + (6.25 * height) - (5 * age) - 161);
            }
            else if (gender == 1) 
            {
                calories = (float)((10 * weight) + (6.25 * height) - (5 * age) + 5);
            }

            if (calories < 100)
            {
                return 0;
            }

            return calories;
        }

         /** 
         * \brief Рассчитывает суточное потребление воды
         */
        public int CalculationWater() // Расчёт потребления воды
        {
            int water = 0;

            water = 30 * (int)weight;

            return water;
        }

         /** 
         * \brief Рассчитывает идеальный вес
         */
        public float IdealWeight() // Расчёт иделаьного веса
        {
            float idealWeight = 0;

            idealWeight = (float)((height - 100 + (age / 10)) * 0.9 * coef);

            if (idealWeight == 0)
            {
                return 0;
            }

            return idealWeight;
        }
    }

    /** 
    * \class TestableCalculation
    * \brief Класс для Unit-тестов
    */
    public class TestableCalculation : Calculation  // Класс для Unit-тестов
    {
         /** 
         * \brief Получение даннных
         */
        public override void Data() // Получение данных
        {
            SetData(30, 175, 70, 1);
        }
    }

}
