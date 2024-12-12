﻿using System.Reflection.Emit;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace VIO
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AccountManager accountManager;
        DatabaseManager database;

        string login;
        string password;
        public MainWindow()
        {
            InitializeComponent();
            InitializeVisibility();
        }

        private void InitializeVisibility() 
        { 
            passwordBox.Visibility = Visibility.Visible; 
            textBox.Visibility = Visibility.Collapsed; 
            ClosedEyeImage.Visibility = Visibility.Visible; 
            OpenedEyeImage.Visibility = Visibility.Collapsed; 
        }

        private void buttonRegistration_Click(object sender, RoutedEventArgs e)
        {
            var selectedLanguage = ((ComboBoxItem)comboBoxLanguage.SelectedItem).Tag.ToString();
            var windowRegistration = new WindowRegistration(selectedLanguage);
            windowRegistration.ShowDialog();
        }

        public delegate void LanguageChangedEventHandler(string lang);
        public static event LanguageChangedEventHandler LanguageChanged;

        private void comboBoxLanguage_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selectedLanguage = ((ComboBoxItem)comboBoxLanguage.SelectedItem).Tag.ToString();
            ChangeLanguage(selectedLanguage);
            LanguageChanged?.Invoke(selectedLanguage);
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

            // Обновить содержимое элементов интерфейса
            labelWelcome.Content = Application.Current.Resources["Welcome"];
            buttonEntrance.Content = Application.Current.Resources["Entrance"];
            labelLogin.Content = Application.Current.Resources["Login"];
            labelPassword.Content = Application.Current.Resources["Password"];
            buttonRegistration.Content = Application.Current.Resources["Registration"];
        }

        private void initAccount(string login, string password)
        {
            accountManager = AccountManager.getInstance();
            accountManager.SetUserCred(login, password);
        }
        private void buttonEntrance_Click(object sender, RoutedEventArgs e)
        {
            login = textBoxLogin.Text;
            password = passwordBox.Password;

            //accountManager = AccountManager.getInstance();
            initAccount(login, password);

            int proverka = accountManager.Authorization(); // Я ИЗМЕНИЛА ПРОВЕРКУ, ВОЗВРАЩЕТСЯ ID ПОЛЬЗОВАТЕЛЯ, Ю НОУ?

            if (proverka > 0)
            {
                var selectedLanguage = ((ComboBoxItem)comboBoxLanguage.SelectedItem).Tag.ToString();
                var windowParameters = new WindowParameters(selectedLanguage);
                windowParameters.ShowDialog();
            }
            else
            {
                MessageBox.Show((string)Application.Current.Resources["LoginOrPasswordWrongMessage"]);
            }
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

        private void textBoxLogin_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}