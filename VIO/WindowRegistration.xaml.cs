using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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

        // PasswordBox ёу
        private void buttonRegisrtation_Click(object sender, RoutedEventArgs e)
        {
            login = textBoxLogin.Text;
            password = textBoxPassword.Text;

            accountManager = new AccountManager(login,password);
            //database = new DatabaseManager();

            int result = accountManager.Registration();
            if (result == 1)
            {
                MessageBox.Show("Ура");
            }
            else if (result == 2)
            {
                MessageBox.Show("Такой логин уже есть");
            }
        }
    }
}