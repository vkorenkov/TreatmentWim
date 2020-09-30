using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Treatment_Wim.Interfaces;

namespace Treatment_Wim.ViewModels
{
    class Vhd : PropertyChangeClass, IMount
    {
        private string image;
        public string Image 
        {
            get { return image; }
            set { image = value; OnPropertyChange(nameof(Image)); }
        }

        private string mountDir;
        public string MountDir 
        {
            get { return mountDir; }
            set 
            { 
                mountDir = value; 
                OnPropertyChange(nameof(MountDir));

                if (!string.IsNullOrEmpty(MountDir))
                {
                    if (Directory.GetDirectories(MountDir).Length + Directory.GetFiles(MountDir).Length == 0)
                    {
                        RunCommand(MountDir, $@"/mount-image /imagefile:""{Image}"" /index:1 /mountdir:""{MountDir}""");
                    }
                    else
                    {
                        MessageBox.Show("Папка монтирования должна быть пуста.");
                        MountDir = string.Empty;
                    }
                }
            }
        }

        public void RunCommand(string whatCheck, string command)
        {
            Task.Run(() =>
            {
                Treatment treatment = new Treatment();

                if (!string.IsNullOrEmpty(whatCheck))
                {
                    treatment.UseCommand(command, "Dism.exe");
                }
            });
        }
    }
}
