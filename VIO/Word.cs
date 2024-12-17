using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace VIO
{
    /** 
     * \class Word 
     * \brief Класс для записи плана питания в Word 
     */
    class Word // Класс для записи плана питания в Word 
    {
        /** 
         * \brief Записывает план питания в файл Word. 
         * 
         * Этот метод создает план питания на день и сохраняет его в файл Word. 
         * 
         * \param resultRB Результат, определяющий тип плана питания. 
         */
        public void RecordWord(int resultRB) // Запись в Word
        {
            MealPlan plan = new MealPlan(resultRB); 
            var dailyPlan = plan.GetRandomMealPlan(); 
            SaveFileDialog saveFileDialog = new SaveFileDialog(); 
            saveFileDialog.Filter = "Word Files|*.docx"; 
            saveFileDialog.Title = "Сохранить файл Word"; 
            saveFileDialog.FileName = "План питания.docx"; 
            if (saveFileDialog.ShowDialog() == true) 
            {
                string filePath = saveFileDialog.FileName; 
                using (var document = DocX.Create(filePath)) 
                { 
                    document.InsertParagraph("Ваш план питания на день:").FontSize(16).Bold().Alignment = Alignment.center; 

                    foreach (var meal in dailyPlan) 
                    { 
                        document.InsertParagraph($"- {meal.ProductName}").FontSize(12).SpacingAfter(10); 
                    } 
                    document.Save();  
                    OpenFile(filePath); 
                } 
            }
        }

        /** 
        * \brief Открывает план питания 
        */
        private void OpenFile(string filePath) // Открытие файла
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = filePath;
            process.StartInfo.UseShellExecute = true;
            process.Start();
        }
    }
}
