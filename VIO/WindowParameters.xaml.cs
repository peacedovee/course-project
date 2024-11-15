using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VIO;

namespace VIO
{
    /// <summary>
    /// Логика взаимодействия для WindowParameters.xaml
    /// </summary>
    public partial class WindowParameters : Window
    {
        private const string IniFilePath = "settings.ini"; 
        private IniFile iniFile;

        public WindowParameters(string initialLanguage)
        {
            InitializeComponent();
            MainWindow.LanguageChanged += OnLanguageChanged;
            ChangeLanguage(initialLanguage);

            iniFile = new IniFile(IniFilePath);
            this.Loaded += WindowParameters_Loaded;
            this.Closing += WindowParameters_Closing;
        }

        private void WindowParameters_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSettings();
        }

        private void WindowParameters_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveSettings();
        }

        // сохранение параметров пользователя при закрытии окна
        private void SaveSettings()
        {
            iniFile.Write("Parameters", "Height", UpDownHight.Value.ToString());
            iniFile.Write("Parameters", "Hips", UpDownGirthHips.Value.ToString());
            iniFile.Write("Parameters", "Waist", UpDownGirthWaist.Value.ToString());
            iniFile.Write("Parameters", "Breast", UpDownGirthBreast.Value.ToString());
            iniFile.Write("Parameters", "Weight", UpDownGirthWeight.Value.ToString());
        }

        // добавление последних введённых параметров при открытии окна
        private void LoadSettings()
        {
            UpDownHight.Value = int.TryParse(iniFile.Read("Parameters", "Height"), out int height) ? height : 10;
            UpDownGirthHips.Value = int.TryParse(iniFile.Read("Parameters", "Hips"), out int hips) ? hips : 10;
            UpDownGirthWaist.Value = int.TryParse(iniFile.Read("Parameters", "Waist"), out int waist) ? waist : 10;
            UpDownGirthBreast.Value = int.TryParse(iniFile.Read("Parameters", "Breast"), out int breast) ? breast : 10;
            UpDownGirthWeight.Value = int.TryParse(iniFile.Read("Parameters", "Weight"), out int weight) ? weight : 10;
        }

        // изменение языка
        private void OnLanguageChanged(string lang)
        {
            ChangeLanguage(lang);
        }

        private void ChangeLanguage(string lang)
        {
            var dict = new ResourceDictionary
            {
                Source = new Uri("ResourceDictionary.xaml", UriKind.Relative)
            };

            var updatedResources = new ResourceDictionary();
            foreach (var key in dict.Keys)
            {
                if (key.ToString().EndsWith($"_{lang}"))
                {
                    string baseKey = key.ToString().Substring(0, key.ToString().Length - lang.Length - 1);
                    updatedResources[baseKey] = dict[key];
                }
                else if (!key.ToString().EndsWith($"_{lang}") && lang != "en")
                {
                    updatedResources[key.ToString()] = dict[key];
                }
            }

            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(updatedResources);

            tabParameters.Header = Application.Current.Resources["TabParameters"];
            tabResults.Header = Application.Current.Resources["TabResults"];
            labelDate.Content = Application.Current.Resources["RecordingDate"];
            labelHight.Content = Application.Current.Resources["Hight"];
            labelWeight.Content = Application.Current.Resources["Weight"];
            labelGirthWaist.Content = Application.Current.Resources["GirthWaist"];
            labelGirthHips.Content = Application.Current.Resources["GirthHips"];
            labelGirthBreast.Content = Application.Current.Resources["GirthBreast"];
            buttonRecord.Content = Application.Current.Resources["Record"];

            labelPlan.Content = Application.Current.Resources["IndividualPlan"];
            labelType.Content = Application.Current.Resources["Type"];
            labelBMI.Content = Application.Current.Resources["BMI"];
            labelCalories.Content = Application.Current.Resources["Calories"];
            labelWater.Content = Application.Current.Resources["Water"];
            labelGoal.Content = Application.Current.Resources["Purpose"];
            labelActivity.Content = Application.Current.Resources["PhysicalActivity"];
            labelActivityLevelLow.Content = Application.Current.Resources["ActivityLevelLow"];
            labelActivityLevelMedium.Content = Application.Current.Resources["ActivityLevelMedium"];
            labelActivityLevelHigh.Content = Application.Current.Resources["ActivityLevelHigh"];

            ComboBoxGainWeight.Content = Application.Current.Resources["GainWeight"];
            ComboBoxLoseWeight.Content = Application.Current.Resources["LoseWeight"];
            ComboBoxMaintainWeight.Content = Application.Current.Resources["MaintainWeight"];

            buttonPlan.Content = Application.Current.Resources["MealPlan"];
            buttonWorkout.Content = Application.Current.Resources["Workout"];

            TextBlockPreferences.Text = (string)Application.Current.Resources["FoodPreferences"];
            RBVegetarian.Content = Application.Current.Resources["Vegetarian"];
            RBVegan.Content = Application.Current.Resources["Vegan"];
            RBStandart.Content = Application.Current.Resources["Standart"];
        }

        // вычисление типа фигуры
        private string DetermineBodyType(double waist, double hips, double bust)
        {
            if (hips > bust && hips > waist)
            {
                return "InvertedTriangle_w"; // Перевернутый треугольник
            }
            else if (bust > hips && bust > waist)
            {
                return "Pear_w"; // Треугольник
            }
            else if (bust == hips && waist == bust)
            {
                return "Rectangle_w"; // Прямоугольник
            }
            else if (bust == hips && waist < bust)
            {
                return "Hourglass_w"; // Песочные часы
            }
            else if (waist > bust && waist > hips)
            {
                return "Round_w"; // Круг
            }
            else
            {
                return "Square"; // Квадрат
            }
        }

        // вывод изображения с типом фигуры (нужно добавить изменение в зависимости от пола)
        private void DisplayBodyTypeImage(string bodyType)
        {
            try
            {
                string imagePath = $"pictures/{bodyType}.png";
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(imagePath, UriKind.Relative);
                bitmap.EndInit();
                BodyTypeImage.Source = bitmap;
                BodyTypeImage.Stretch = Stretch.Uniform;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}");
            }
        }

        // кнопка записи в базу данных (тут пока код для определения картинки)
        private void buttonRecord_Click(object sender, RoutedEventArgs e)
        {
            double waist = UpDownGirthWaist.Value ?? 0; // талия
            double hips = UpDownGirthHips.Value ?? 0; // грудь
            double bust = UpDownGirthBreast.Value ?? 0; // бёдра

            string bodyType = DetermineBodyType(waist, hips, bust);
            DisplayBodyTypeImage(bodyType);
        }

        // изменение картинки с типом фигуры
        private void IntegerUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            //UpdatePicture();
        }

        private void UpdatePicture()
        {
            double waist = UpDownGirthWaist.Value ?? 0; // талия
            double hips = UpDownGirthHips.Value ?? 0; // грудь
            double bust = UpDownGirthBreast.Value ?? 0; // бёдра

            string bodyType = DetermineBodyType(waist, hips, bust);
            DisplayBodyTypeImage(bodyType);
        }

        // получение плана питания, открытие окна с предпочтениями
        private void buttonPlan_Click(object sender, RoutedEventArgs e)
        {
            NotificationPopup.IsOpen = true;
        }

        // вывод предпочтений (вместо этого будем создавать план на основе предпочтений)
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedOption = "Ни один вариант не выбран";

            if (RBVegetarian.IsChecked == true)
            {
                selectedOption = "Вегетарианец";
            }
            else if (RBVegan.IsChecked == true)
            {
                selectedOption = "Веган";
            }
            else if (RBStandart.IsChecked == true)
            {
                selectedOption = "Стандартный";
            }

            MessageBox.Show($"Вы выбрали: {selectedOption}");
            NotificationPopup.IsOpen = false;
        }
    }
}