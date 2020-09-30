using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Treatment_Wim
{
    class Treatment : PropertyChangeClass
    {
        public delegate void Out(string output, bool start, bool tabEnable);
        public static event Out OutputEvent;

        public delegate void OutOscdimg(string output, bool start, bool tabEnable);
        public static event OutOscdimg OscdimgEvent;

        private string output;

        public void UseCommand(string command, string programm)
        {
            Process process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    FileName = programm,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    Arguments = command,
                    StandardOutputEncoding = Encoding.GetEncoding(866)
                }
            };

            process.Start();

            OutputMsg(process);
        }
       
        public void UseOscdimg(string command)
        {
            Process process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    FileName = "Resources/oscdimg.exe",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    Arguments = command,
                    StandardOutputEncoding = Encoding.GetEncoding(866)
                }
            };

            process.Start();

            StreamReader sr = process.StandardOutput;

            while (!sr.EndOfStream)
            {
                output = sr.ReadLine();

                OscdimgEvent?.Invoke("выполняется. Это может занять продолжительное время", true, false);
            }

            OscdimgEvent?.Invoke("Операция сборки завершена", false, true);
        }

        private void OutputMsg(Process process)
        {
            StreamReader sr = process.StandardOutput;

            while (!sr.EndOfStream)
            {
                output = sr.ReadLine();

                if (string.IsNullOrEmpty(output))
                {
                    output = "Подготовка. Это может занять продолжительное время. Подождите.";
                    OutputEvent?.Invoke(output, true, false);
                }
                else
                {
                    OutputEvent?.Invoke(output, false, false);
                }

                if (output.Contains("Ошибка"))
                {
                    OutputEvent?.Invoke(output, false, true);
                    break;
                }
            }

            OutputEvent?.Invoke(output, false, true);
        }

        public void Copy(string whereFrom, string to, string fileName)
        {
            try
            {
                if (fileName.Contains("install"))
                {
                    CheckInstallWim(to);
                }

                #region old conditions
                //if (fileName == "Winre.wim" && File.Exists($@"{to}\{fileName}"))
                //{
                //    File.Delete($@"{to}\{fileName}");
                //}
                //else if (fileName == "install.wim" && File.Exists($@"{to}\{fileName}"))
                //{
                //    File.Delete($@"{to}\{fileName}");
                //}
                //else if (fileName == "install.esd" && File.Exists($@"{to}\{fileName}"))
                //{
                //    File.Delete($@"{to}\{fileName}");
                //}
                #endregion

                OutputEvent?.Invoke($"Импорт нового {fileName}", true, false);

                File.Copy(whereFrom, $@"{to}\{fileName}", true);

                OutputEvent?.Invoke($"Импорт {fileName} завершен", false, true);
            }
            catch (Exception ex)
            {
                OutputEvent?.Invoke(ex.Message, false, true);
            }            
        }

        private void CheckInstallWim(string path)
        {
            foreach (var t in Directory.GetFiles(path))
            {
                if(t.Contains("install"))
                {
                    path = t;

                    if (MessageBox.Show($@"Обнаружен предыдущий {t.Substring(t.LastIndexOf(@"\") + 1)}. Удалить?", "Внимание", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        File.Delete(t);
                    }

                    break;
                }
            }
        }
    }
}
