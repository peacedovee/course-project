using System;
using System.Collections.Generic;
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
        double height; 
        double weight;
        int age;
        int coef;
        public Calculation(double height, double weight, int age, int coef)
        {
            this.height = height;
            this.weight = weight;
            this.age = age;
            this.coef = coef;
        }

        public double CalculationBmi()
        {
            double bmi = weight / Math.Pow(height, 2) * 10000;

            return bmi;
        }

        private string VerdictBMI(int age, double bmi)
        {
            string verdict = "";

            if (age >= 18 && age <= 24)
            {

            }

            return verdict;
        }

        private double IdealWeight()
        {
            double idealWeight = 0;

            idealWeight = (height - 100 + (age / 10)) * 0.9 * coef; 
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
