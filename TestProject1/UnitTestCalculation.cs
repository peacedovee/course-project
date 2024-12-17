using Xunit;
using VIO; 

namespace TestCalculation
{ 
    public class UnitTestCalculation // Класс для тестирования
    {
        private TestableCalculation instance;

        public UnitTestCalculation()
        {
            instance = new TestableCalculation();
        }

        [Fact]
        public void CalculationCalories_Male_ReturnsCorrectCalories() // Тест метода CalculationCalories
        {
            // Arrange
            instance.Data(); // Установка данных
            float expectedCaloriesMale = 1648.75f; 

            // Act
            float calories = instance.CalculationCalories(1); // Вызов с параметром пола

            // Assert
            Assert.Equal(expectedCaloriesMale, calories);
        }

        [Fact]
        public void CalculationCalories_Female_ReturnsCorrectCalories() // Тест метода CalculationCalories
        {
            // Arrange
            instance.Data(); // Установка данных
            float expectedCaloriesFemale = 1482.75f; 

            // Act
            float calories = instance.CalculationCalories(0); // Вызов с параметром пола

            // Assert
            Assert.Equal(expectedCaloriesFemale, calories);
        }

        [Fact]
        public void CalculationWater_ReturnsCorrectWaterIntake() // Тест метода CalculationWater
        {
            // Arrange
            instance.Data(); // Установка данных
            int expectedWater = 2100; 

            // Act
            int water = instance.CalculationWater(); 

            // Assert
            Assert.Equal(expectedWater, water);
        }

        [Fact]
        public void IdealWeight_ReturnsCorrectIdealWeight() // Тест метода IdealWeight
        {
            // Arrange
            instance.Data(); // Установка данных
            float expectedIdealWeight = 70.2f; 

            // Act
            float idealWeight = instance.IdealWeight(); 

            // Assert
            Assert.Equal(expectedIdealWeight, idealWeight);
        }

        [Fact]
        public void CalculationBmi_ReturnsCorrectBmi() // Тест метода CalculationBmi
        {
            // Arrange
            instance.Data(); // Установка данных
            double expectedBmi = 22.86; 

            // Act
            double bmi = instance.CalculationBmi();

            // Assert
            Assert.Equal(expectedBmi, bmi, 2); // Последний параметр указывает точность сравнения
        }
    }
}
