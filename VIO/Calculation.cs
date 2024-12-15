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
    internal class Calculation
    {
        AccountManager accountManager;
        int height;
        float weight;
        int age;
        float coef;

        private void Data()
        {
            accountManager = AccountManager.getInstance();
            (age, height, weight, coef) = accountManager.CalculateData();


        }

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
    }
}
