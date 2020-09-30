using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Treatment_Wim.Interfaces;

namespace Treatment_Wim
{
    class CaptureMountImage : PropertyChangeClass, IMount
    {
        public int Index { get; set; }

        private string captureDir;
        public string CaptureDir
        {
            get { return captureDir; }
            set { captureDir = value; OnPropertyChange(nameof(CaptureDir)); }
        }

        private string image;
        public string Image
        {
            get { return image; }
            set
            {
                image = value; OnPropertyChange(nameof(Image));

                RunCommand(Image, $@"/capture-image /imagefile:""{Image}"" /capturedir:{CaptureDir} /name:Build{DateTime.Now.ToShortDateString().Replace(".", "")}");
            }
        }

        private string mountImage;
        public string MountImage
        {
            get { return mountImage; }
            set { mountImage = value; OnPropertyChange(nameof(MountImage)); }
        }

        private string mountDir;
        public string MountDir
        {
            get { return mountDir; }
            set
            {
                mountDir = value; OnPropertyChange(nameof(MountDir));

                if (!string.IsNullOrEmpty(MountDir))
                {
                    if (Directory.GetDirectories(MountDir).Length + Directory.GetFiles(MountDir).Length == 0)
                    {
                        RunCommand(MountDir, $@"/mount-image /imagefile:""{MountImage}"" /index:{Index} /mountdir:""{MountDir}""");
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
