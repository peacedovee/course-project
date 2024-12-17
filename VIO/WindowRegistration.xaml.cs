using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Xml.Linq;
using VIO;

namespace VIO
{
    /// <summary>
    /// Логика взаимодействия для WindowRegistration.xaml
    /// </summary>
    public partial class WindowRegistration : Window
    {
        DatabaseManager database;
        AccountManager accountManager;

        string login;
        string password;
        public WindowRegistration(string initialLanguage)
        {
            InitializeComponent();
            MainWindow.LanguageChanged += OnLanguageChanged;
            ChangeLanguage(initialLanguage);
            InitializeVisibility();
        }

        private void InitializeVisibility()
        {
            passwordBox.Visibility = Visibility.Visible;
            textBox.Visibility = Visibility.Collapsed;
            ClosedEyeImage.Visibility = Visibility.Visible;
            OpenedEyeImage.Visibility = Visibility.Collapsed;
        }

        private void ClosedEyeImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            textBox.Text = passwordBox.Password;
            passwordBox.Visibility = Visibility.Collapsed;
            textBox.Visibility = Visibility.Visible;
            ClosedEyeImage.Visibility = Visibility.Collapsed;
            OpenedEyeImage.Visibility = Visibility.Visible;
        }

        private void OpenedEyeImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            passwordBox.Password = textBox.Text;
            textBox.Visibility = Visibility.Collapsed;
            passwordBox.Visibility = Visibility.Visible;
            OpenedEyeImage.Visibility = Visibility.Collapsed;
            ClosedEyeImage.Visibility = Visibility.Visible;
        }

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

            labelRegistration.Content = Application.Current.Resources["RegistrationWord"];
            labelLogin.Content = Application.Current.Resources["Login"];
            labelName.Content = Application.Current.Resources["Name"];
            labelBirthday.Content = Application.Current.Resources["Birthday"];
            labelPassword.Content = Application.Current.Resources["Password"];
            labelGender.Content = Application.Current.Resources["Gender"];
            buttonRegisrtation.Content = Application.Current.Resources["RegistrationButton"];
            buttonBack.Content = Application.Current.Resources["Back"];

            UpdateComboBoxItems();
        }

        private void UpdateComboBoxItems()
        {
            ComboBoxItem femaleItem = (ComboBoxItem)FindName("ComboBoxFemale");
            ComboBoxItem maleItem = (ComboBoxItem)FindName("ComboBoxMale");

            if (femaleItem != null)
                femaleItem.Content = Application.Current.Resources["Female"];
            if (maleItem != null)
                maleItem.Content = Application.Current.Resources["Male"];
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private bool AreFieldsValid()
        {
            string loginPattern = @"^[a-zA-Z0-9]{4,12}$"; // Логин может содержать только буквы латиницы и цифры, длина от 4 до 12 символов
            string passwordPattern = @"^[a-zA-Zа-яА-Я0-9]{4,12}$"; // Пароль может содержать буквы (латиница и кириллица) и цифры, длина от 4 до 12 символов

            if (string.IsNullOrWhiteSpace(textBoxLogin.Text) ||
                !Regex.IsMatch(textBoxLogin.Text, loginPattern))
            {
                MessageBox.Show((string)Application.Current.Resources["LoginMessage"]);
                return false;
            }

            if (string.IsNullOrWhiteSpace(passwordBox.Password) ||
                !Regex.IsMatch(passwordBox.Password, passwordPattern))
            {
                MessageBox.Show((string)Application.Current.Resources["PasswordMessage"]);
                return false;
            }

            return !string.IsNullOrWhiteSpace(textBoxName.Text) &&
                   !string.IsNullOrWhiteSpace(DatePickerBirthday.Text) &&
                   !string.IsNullOrWhiteSpace(comboboxGender.Text);
        }

        private void initAccount(string login, string password)
        {
            accountManager = AccountManager.getInstance(true);
            accountManager.SetUserCred(login, password);
        }

        private void buttonRegisrtation_Click(object sender, RoutedEventArgs e)
        {
            login = textBoxLogin.Text;
            password = passwordBox.Password;

            string name = textBoxName.Text;
            string birthdayString = DatePickerBirthday.Text;
            DateTime birthday;
            string[] formats = { "MM/dd/yyyy", "M/d/yyyy", "yyyy-MM-dd" }; 
            DateTime.TryParseExact(birthdayString, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out birthday);

            int gender = comboboxGender.SelectedIndex;

            initAccount(login, password);

            // Проверка возраста
            int age = DateTime.Now.Year - birthday.Year;
            if (DateTime.Now < birthday.AddYears(age)) // Корректируем возраст, если день рождения еще не наступил в этом году
            {
                age--;
            }

            if (age < 18)
            {
                MessageBox.Show((string)Application.Current.Resources["AgeRestrictionMessage"]);
                return;
            }

            if (AreFieldsValid())
            {
                int result = accountManager.Registration(name, birthdayString, gender);

                switch (result)
                {
                    case 1:
                        MessageBox.Show((string)Application.Current.Resources["SuccessMessage"]);
                        this.Close();
                        break;
                    case 2:
                        MessageBox.Show((string)Application.Current.Resources["LoginExistsMessage"]);
                        break;
                    default:
                        MessageBox.Show((string)Application.Current.Resources["RegistrationErrorMessage"]);
                        break;
                }
            }
            else
            {
                MessageBox.Show((string)Application.Current.Resources["EmptyFieldsMessage"]);
            }
        }

    }
}