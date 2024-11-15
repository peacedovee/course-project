using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIO
{
    internal class BMI
    {
        private double CalculateBMI(int h, double weight)
        {
            int hight = h / 100;
            double bmi = weight / Math.Pow(hight, 2);

            return bmi;
        }

        private string VerdictBMI(int age, double bmi)
        {
            string verdict = "";

            if (age >= 18 && age <= 24 )
            {

            }

            return verdict;
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
