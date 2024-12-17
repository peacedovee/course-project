using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;

namespace VIO
{
    /** 
     * \class Excel 
     * \brief Класс для выгрузки всех записей пользователя в Excel
     */
    internal class Excel // Класс для выгрузки всех записей пользователя в Excel
    {
        AccountManager accountManager;
        string date;
        int height;
        float weight;
        float wrist;
        int breast, waist, hips;

        public Excel() 
        {
            accountManager = AccountManager.getInstance();
        }

        /** 
         * \brief Выгружает записи пользователя в Excel 
         */
        public void RecordExcel(List<object> records) // Запись в Excel
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel Files|*.xlsx";
            saveFileDialog.Title = "Сохранить файл Excel";
            saveFileDialog.FileName = "Записи.xlsx";

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Data"); // Запись заголовков
                    worksheet.Cells[1, 1].Value = "Дата записи";
                    worksheet.Cells[1, 2].Value = "Рост";
                    worksheet.Cells[1, 3].Value = "Вес";
                    worksheet.Cells[1, 4].Value = "Коэффициент запястья";
                    worksheet.Cells[1, 5].Value = "Обхват груди";
                    worksheet.Cells[1, 6].Value = "Обхват талии";
                    worksheet.Cells[1, 7].Value = "Обхват бёдер";

                    int row = 2;
                    foreach (var record in records)
                    {
                        dynamic rec = record; // Использование dynamic для доступа к полям анонимных объектов
                                            
                        worksheet.Cells[row, 1].Value = rec.RecordingDate.ToString("yyyy-MM-dd");
                        worksheet.Cells[row, 2].Value = rec.Height;
                        worksheet.Cells[row, 3].Value = rec.Weight;
                        worksheet.Cells[row, 4].Value = rec.CoefGirthWrist;
                        worksheet.Cells[row, 5].Value = rec.GirthBreast;
                        worksheet.Cells[row, 6].Value = rec.GirthWaist;
                        worksheet.Cells[row, 7].Value = rec.GirthHips;

                        row++;
                    }

                    // Автоматическая подгонка ширины колонок
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                    // Сохранение файла
                    FileInfo file = new FileInfo(filePath);
                    package.SaveAs(file);
                    OpenFile(filePath);
                }
            }
        }
        /** 
         * \brief Открывает файл с записями
         */
        public void OpenFile(string filePath) // Открытие файла
        { 
            Process process = new Process(); 
            process.StartInfo.FileName = filePath; 
            process.StartInfo.UseShellExecute = true; 
            process.Start();
        }
    }
}
