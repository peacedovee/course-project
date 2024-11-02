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

namespace VIO
{
    /// <summary>
    /// Логика взаимодействия для WindowParameters.xaml
    /// </summary>
    public partial class WindowParameters : Window
    {
        public WindowParameters(string initialLanguage)
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

            labelDate.Content = Application.Current.Resources["RecordingDate"];
            labelHight.Content = Application.Current.Resources["Hight"];
            labelWeight.Content = Application.Current.Resources["Weight"];
            labelGirthWaist.Content = Application.Current.Resources["GirthWaist"];
            labelGirthHips.Content = Application.Current.Resources["GirthHips"];
            labelGirthBreast.Content = Application.Current.Resources["GirthBreast"];
        }
    }
}
