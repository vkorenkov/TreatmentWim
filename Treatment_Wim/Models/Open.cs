using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Treatment_Wim
{
    class Open : IOpenFOlder
    {
        public string OpenFolderPath { get; set; }
       
        public Open()
        {          
        }

        public string OpenPath()
        {
            CommonOpenFileDialog cofd = new CommonOpenFileDialog();

            cofd.IsFolderPicker = true;

            if (cofd.ShowDialog() == CommonFileDialogResult.Ok)
            { 
                return OpenFolderPath = cofd.FileName; 
            }
            else
            { 
                return OpenFolderPath = string.Empty; 
            }
        }

        public string OpenFile(params string[] format)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            string tempFormat = string.Empty;

            foreach(var t in format)
            {
                tempFormat = tempFormat + $"*.{t};";
            }

            ofd.Filter = $"Файлы {tempFormat.Replace(";"," ")}|{tempFormat}|Все файлы|*.*";

            if (ofd.ShowDialog() == true)
            {
                return ofd.FileName;
            }
            else
            {
                return string.Empty;
            }
        }

        public string SaveFile(string fileName, string format)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.FileName = fileName;

            sfd.Filter = $"Файл {format} (*.{format})|*.{format}";

            if(sfd.ShowDialog() == true)
            {
                return sfd.FileName;
            }
            else
            {
                return string.Empty;
            }
        }

        public string SaveFile(string fileName, string format1, string format2)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.FileName = fileName;

            sfd.Filter = $"Файл {format1} (*.{format1})|*.{format1}|Файл {format2} (*.{format2})| *.{format2}";

            if (sfd.ShowDialog() == true)
            {
                return sfd.FileName;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
