using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using OfficeOpenXml;
using System.IO;

namespace VIO
{
    /// <summary>
    /// Логика взаимодействия для WindowParameters.xaml
    /// </summary> 
    public partial class WindowParameters : Window
    {
        AccountManager accountManager;
        Calculation calculation;
        private const string IniFilePath = "settings.ini"; 
        private IniFile iniFile;
        private double height; // рост
        private double weight; // вес
        private double waist; // талия
        private double hips; // бёдра
        private double bust; // грудь
        private int age;
        private int coef;
        private string language;

        public WindowParameters(string initialLanguage)
        {
            InitializeComponent();
            MainWindow.LanguageChanged += OnLanguageChanged;
            ChangeLanguage(initialLanguage);

            language = initialLanguage;

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
            iniFile.Write("Parameters", "Height", UpDownHeight.Value.ToString());
            iniFile.Write("Parameters", "Breast", UpDownGirthBreast.Value.ToString());
            iniFile.Write("Parameters", "Waist", UpDownGirthWaist.Value.ToString());
            iniFile.Write("Parameters", "Wrist", comboboxWrist.Text);
            iniFile.Write("Parameters", "Hips", UpDownGirthHips.Value.ToString());
            iniFile.Write("Parameters", "Weight", UpDownGirthWeight.Value.ToString());
        }

        // добавление последних введённых параметров при открытии окна
        private void LoadSettings()
        {
            UpDownHeight.Value = int.TryParse(iniFile.Read("Parameters", "Height"), out int height) ? height : 10;
            UpDownGirthBreast.Value = int.TryParse(iniFile.Read("Parameters", "Breast"), out int breast) ? breast : 10;
            UpDownGirthWaist.Value = int.TryParse(iniFile.Read("Parameters", "Waist"), out int waist) ? waist : 10;
            comboboxWrist.Text = iniFile.Read("Parameters", "Wrist");
            UpDownGirthHips.Value = int.TryParse(iniFile.Read("Parameters", "Hips"), out int hips) ? hips : 10;
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
            labelHeight.Content = Application.Current.Resources["Height"];
            labelWeight.Content = Application.Current.Resources["Weight"];
            labelWrist.Content = Application.Current.Resources["Wrist"];
            labelGirthWaist.Content = Application.Current.Resources["GirthWaist"];
            labelGirthBreast.Content = Application.Current.Resources["GirthBreast"];
            labelGirthHips.Content = Application.Current.Resources["GirthHips"];
            buttonRecord.Content = Application.Current.Resources["Record"];

            labelPlan.Content = Application.Current.Resources["IndividualPlan"];
            labelType.Content = Application.Current.Resources["Type"];
            labelBMI.Content = Application.Current.Resources["BMI"];
            labelIdealWeight.Content = Application.Current.Resources["IdealWeight"];
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
            buttonExcel.Content = Application.Current.Resources["ForExcel"];
            buttonWorkout.Content = Application.Current.Resources["Workout"];

            TextBlockPreferences.Text = (string)Application.Current.Resources["FoodPreferences"];
            RBVegetarian.Content = Application.Current.Resources["Vegetarian"];
            RBVegan.Content = Application.Current.Resources["Vegan"];
            RBStandart.Content = Application.Current.Resources["Standart"];
        }

        // вычисление типа фигуры
        private string DetermineBodyType(double waist, double hips, double bust)
        {
            int gender = accountManager.UserDataSelect();
            string suffix = (gender == 0) ? "_w" : "_m"; // Определение суффикса в зависимости от гендера

            if (hips < bust && hips > waist)
            {
                return "InvertedTriangle" + suffix; // Перевернутый треугольник
            }
            else if (bust < hips && bust > waist)
            {
                return "Pear" + suffix; // Треугольник
            }
            else if (bust == hips && waist == bust)
            {
                return "Rectangle" + suffix; // Прямоугольник
            }
            else if (bust == hips && waist < bust)
            {
                return "Hourglass" + suffix; // Песочные часы
            }
            else if (waist > bust && waist > hips)
            {
                return "Round" + suffix; // Круг
            }
            else
            {
                return "Rectangle" + suffix; // Прямоугольник
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

        // кнопка сохранения в базу данных
        private void ButtonRecord_Click(object sender, RoutedEventArgs e)
        {
            float coef = 0;
            string recordingDateText = DatePickerRecording.Text; // дата записи
            DateTime recordingDate;
            string[] formats = { "MM/dd/yyyy", "M/d/yyyy", "yyyy-MM-dd" };
            DateTime.TryParseExact(recordingDateText, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out recordingDate);

            // Проверка на дату позже текущей
            if (recordingDate > DateTime.Now)
            {
                MessageBox.Show((string)Application.Current.Resources["InvalidDateMessage"]);
                return;
            }

            // Проверка на дату раньше более чем на два года
            if (recordingDate < DateTime.Now.AddYears(-2))
            {
                MessageBox.Show((string)Application.Current.Resources["DateMessage"]);
                return;
            }

            int height = UpDownHeight.Value ?? 0; // рост
            double weight = UpDownGirthWeight.Value ?? 0; // вес
            int wrist = comboboxWrist.SelectedIndex; // запястье
            int waist = UpDownGirthWaist.Value ?? 0; // талия
            int hips = UpDownGirthHips.Value ?? 0; // бёдра
            int breast = UpDownGirthBreast.Value ?? 0; // грудь

            if (wrist == 0)
            {
                coef = 0.9f;
            }
            else if (wrist == 1)
            {
                coef = 1;
            }
            else if (wrist == 2)
            {
                coef = 1.1f;
            }

            accountManager = AccountManager.getInstance();

            int result = accountManager.UserParameters(recordingDateText, height, (float)weight, coef, breast, waist, hips);

            if (result == 0)
            {
                MessageBox.Show((string)Application.Current.Resources["SuccessfulRecording"]);
            }
            if (result == 1)
            {
                MessageBox.Show((string)Application.Current.Resources["UpdatedRecording"]);
            }
            if (result == 2)
            {
                MessageBox.Show((string)Application.Current.Resources["RecordingError"]);
            }
        }

        private void UpdatePicture()
        {
            double waist = UpDownGirthWaist.Value ?? 0; // талия
            double hips = UpDownGirthHips.Value ?? 0; // 
            double bust = UpDownGirthBreast.Value ?? 0; // 

            string bodyType = DetermineBodyType(waist, hips, bust);
            DisplayBodyTypeImage(bodyType);
        }

        // получение плана питания, открытие окна с предпочтениями
        private void ButtonPlan_Click(object sender, RoutedEventArgs e)
        {
            NotificationPopup.IsOpen = true;
        }

        // выбор предпочтений
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            int resultRB = 0;

            // Получаем значение выбора
            if (RBVegetarian.IsChecked == true)
            {
                resultRB = 1;
            }
            else if (RBVegan.IsChecked == true)
            {
                resultRB = 2;
            }
            else if (RBStandart.IsChecked == true)
            {
                resultRB = 3;
            }

            // Закрываем Popup
            NotificationPopup.IsOpen = false;

            // Выполняем основное действие после получения выбора
            ExecuteMealPlan(resultRB);
        }

        private void ExecuteMealPlan(int resultRB)
        {
            MealPlan plan = new MealPlan(resultRB);

            Word word = new Word();
            word.RecordWord(resultRB);
        }

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void tabResults_Loaded(object sender, RoutedEventArgs e)
        {
            UpDownGirthWaist.ValueChanged += UpDown_ValueChanged;
            UpDownGirthBreast.ValueChanged += UpDown_ValueChanged;
            UpDownGirthHips.ValueChanged += UpDown_ValueChanged;

            UpdatePicture();
        }

        private void UpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            UpdatePicture();
        }


        private void buttonWorkout_Click(object sender, RoutedEventArgs e)
        {
            int sliderValue = (int)sliderActivity.Value;
            string url = "";

            switch (sliderValue)
            {
                case 1: // URL для низкого уровня активности
                    url = "https://www.youtube.com/watch?v=IbdS_z2Pu4c";
                    break;
                case 2: // URL для среднего уровня активности
                    url = "https://www.youtube.com/watch?v=rb3-BMof2lg";
                    break;
                case 3: // URL для высокого уровня активности
                    url = "https://www.youtube.com/watch?v=ESMmH-JfPCY";
                    break;
            }

            if (!string.IsNullOrEmpty(url))
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
        }

        private void tabParameters_Loaded(object sender, RoutedEventArgs e)
        {
            DatePickerRecording.SelectedDate = DateTime.Now;
        }

        private void mainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                TabItem selectedTab = (TabItem)mainTabControl.SelectedItem;
            }

            int goal = comboboxGoal.SelectedIndex; // ЦЕЛЬ!!! Ю НОУ???

            accountManager = AccountManager.getInstance();
            int gender = accountManager.UserDataSelect();

            double bmi = 0;
            float calories = 0;
            int water = 0;
            float idealWeight = 0;

            if (calculation == null)
            {
                calculation = new Calculation();
            }

            // Проверка значения bmi
            bmi = calculation.CalculationBmi();
            if (labelBMINumber != null)
            {
                labelBMINumber.Content = double.IsNaN(bmi) ? "0" : bmi.ToString("F2");
            }

            // Проверка значения calories
            calories = calculation.CalculationCalories();
            if (labelCaloriesNumber != null)
            {
                labelCaloriesNumber.Content = float.IsNaN(calories) ? "0" : ((int)calories).ToString();
            }

            // Проверка значения water
            water = calculation.CalculationWater();
            if (labelWaterNumber != null)
            {
                labelWaterNumber.Content = water == 0 ? "0" : water.ToString();
            }

            // Проверка значения idealWeight
            idealWeight = calculation.IdealWeight();
            if (labelIdealWeightNumber != null)
            {
                labelIdealWeightNumber.Content = float.IsNaN(idealWeight) ? "0" : idealWeight.ToString("F2");
            }

            // Проверка значения bmi для отображения NotificationImage
            if (bmi < 18.5 || bmi > 24.9)
            {
                if (NotificationImage != null)
                {
                    NotificationImage.Visibility = Visibility.Visible;
                }
            }
            else
            {
                if (NotificationImage != null)
                {
                    NotificationImage.Visibility = Visibility.Hidden;
                }
            }
        }

        private void NotificationImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            double bmi = 0;
            calculation = new Calculation();
            bmi = calculation.CalculationBmi();
            labelBMINumber.Content = bmi.ToString("F2");

            if (bmi < 18.4)
            {
                MessageBox.Show((string)Application.Current.Resources["LackOfWeight"]);
            }
            else if (bmi > 25 && bmi < 30)
            {
                MessageBox.Show((string)Application.Current.Resources["SmallExcessOfTheBody"]);
            }
            else if (bmi > 30 && bmi < 35)
            {
                MessageBox.Show((string)Application.Current.Resources["FirstDegreeOfObesity"]);
            }
            else if (bmi > 35 && bmi < 40)
            {
                MessageBox.Show((string)Application.Current.Resources["SecondDegreeOfObesity"]);
            }
            else if (bmi > 40)
            {
                MessageBox.Show((string)Application.Current.Resources["ThirdDegreeOfObesity"]);
            }
        }

        private void buttonExcel_Click(object sender, RoutedEventArgs e)
        {
            Excel excel = new Excel();
            List<object> records = accountManager.GettingAllParameters();

            excel.RecordExcel(records);
        }
    }
}