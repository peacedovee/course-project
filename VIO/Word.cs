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
    class Word
    {
        public void RecordWord(int resultRB)
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
        private void OpenFile(string filePath)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = filePath;
            process.StartInfo.UseShellExecute = true;
            process.Start();
        }
    }
}
