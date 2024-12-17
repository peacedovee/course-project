using Xunit;
using VIO; 

namespace TestCalculation
{ 
    public class UnitTestCalculation // ����� ��� ������������
    {
        private TestableCalculation instance;

        public UnitTestCalculation()
        {
            instance = new TestableCalculation();
        }

        [Fact]
        public void CalculationCalories_Male_ReturnsCorrectCalories() // ���� ������ CalculationCalories
        {
            // Arrange
            instance.Data(); // ��������� ������
            float expectedCaloriesMale = 1648.75f; 

            // Act
            float calories = instance.CalculationCalories(1); // ����� � ���������� ����

            // Assert
            Assert.Equal(expectedCaloriesMale, calories);
        }

        [Fact]
        public void CalculationCalories_Female_ReturnsCorrectCalories() // ���� ������ CalculationCalories
        {
            // Arrange
            instance.Data(); // ��������� ������
            float expectedCaloriesFemale = 1482.75f; 

            // Act
            float calories = instance.CalculationCalories(0); // ����� � ���������� ����

            // Assert
            Assert.Equal(expectedCaloriesFemale, calories);
        }

        [Fact]
        public void CalculationWater_ReturnsCorrectWaterIntake() // ���� ������ CalculationWater
        {
            // Arrange
            instance.Data(); // ��������� ������
            int expectedWater = 2100; 

            // Act
            int water = instance.CalculationWater(); 

            // Assert
            Assert.Equal(expectedWater, water);
        }

        [Fact]
        public void IdealWeight_ReturnsCorrectIdealWeight() // ���� ������ IdealWeight
        {
            // Arrange
            instance.Data(); // ��������� ������
            float expectedIdealWeight = 70.2f; 

            // Act
            float idealWeight = instance.IdealWeight(); 

            // Assert
            Assert.Equal(expectedIdealWeight, idealWeight);
        }

        [Fact]
        public void CalculationBmi_ReturnsCorrectBmi() // ���� ������ CalculationBmi
        {
            // Arrange
            instance.Data(); // ��������� ������
            double expectedBmi = 22.86; 

            // Act
            double bmi = instance.CalculationBmi();

            // Assert
            Assert.Equal(expectedBmi, bmi, 2); // ��������� �������� ��������� �������� ���������
        }
    }
}
