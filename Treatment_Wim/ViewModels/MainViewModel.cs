using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Treatment_Wim.ViewModels;

namespace Treatment_Wim
{
    class MainViewModel : PropertyChangeClass
    {
        private string output;
        public string Output
        {
            get { return output; }
            set { output = value; OnPropertyChange(nameof(Output)); }
        }

        private bool progBarStart;
        public bool ProgBarStart
        {
            get { return progBarStart; }
            set { progBarStart = value; OnPropertyChange(nameof(ProgBarStart)); }
        }

        private int progBarValue;
        public int ProgBarValue
        {
            get { return progBarValue; }
            set { progBarValue = value; OnPropertyChange(nameof(ProgBarValue)); }
        }

        private bool tabEnable = true;
        public bool TabEnable
        {
            get { return tabEnable; }
            set { tabEnable = value; OnPropertyChange(nameof(TabEnable)); }
        }

        private string operation;
        public string Operation
        {
            get { return operation; }
            set { operation = value; OnPropertyChange(nameof(Operation)); }
        }

        private ObservableCollection<string> operationLog = new ObservableCollection<string>() { "Выполненные действия: " };
        public ObservableCollection<string> OperationLog
        {
            get { return operationLog; }
            set { operationLog = value; OnPropertyChange(nameof(OperationLog)); }
        }

        public CaptureMountImage CaptureMount { get; set; } = new CaptureMountImage();

        public Options Options { get; set; } = new Options();

        public Iso Iso { get; set; } = new Iso();

        public Vhd Vhd { get; set; } = new Vhd();

        public ConvertBuild Convert { get; set; } = new ConvertBuild();

        private string path;
        public string Path
        {
            get { return path; }
            set { path = value; OnPropertyChange(nameof(Path)); }
        }

        public int Index { get; set; }

        public bool Discard { get; set; }

        public ICommand WhereFrom => new RelayCommand<object>(obj =>
        {
            Open open = new Open();

            Options.Where = CaptureMount.MountDir;

            switch (obj)
            {
                case "txbWhereFromVhd":
                    Vhd.Image = open.OpenFile("vhd");
                    break;
                case "txbToFolderVhd":
                    Vhd.MountDir = open.OpenPath();
                    Operation = "Монтирование VHD";
                    Application.Current.Dispatcher.Invoke(() => OperationLog.Add(Operation));
                    break;
                case "txbWhereFromCapture":
                    CaptureMount.CaptureDir = open.OpenPath();
                    break;
                case "txbToFolderCapture":
                    CaptureMount.Image = open.SaveFile("install", "wim");
                    Operation = "Захват образа";
                    Application.Current.Dispatcher.Invoke(() => OperationLog.Add(Operation));
                    break;
                case "txbWhereFromMount":
                    CaptureMount.MountImage = open.OpenFile("wim");
                    break;
                case "txbToFolderMount":
                    CaptureMount.Index = Index;
                    CaptureMount.MountDir = open.OpenPath();
                    Operation = "Монтирование WIM";
                    Application.Current.Dispatcher.Invoke(() => OperationLog.Add(Operation));
                    break;
                case "txbAppAssoc":
                    Options.AppAssocPath = open.OpenFile("xml");
                    Operation = "Импорт ассоциаций";
                    Application.Current.Dispatcher.Invoke(() => OperationLog.Add(Operation));
                    break;
                case "txbStartLayout":
                    Options.LayoutPath = open.OpenFile("xml");
                    Operation = "Импорт меню пуск";
                    Application.Current.Dispatcher.Invoke(() => OperationLog.Add(Operation));
                    break;
                case "txbWinRe":
                    Options.WinRePath = open.OpenFile("wim");
                    Operation = "Импорт среды восстановления";
                    Application.Current.Dispatcher.Invoke(() => OperationLog.Add(Operation));
                    break;
                case "txbUnattend":
                    Options.UnattendPath = open.OpenFile("xml");
                    Operation = "Импорт файла ответов";
                    Application.Current.Dispatcher.Invoke(() => OperationLog.Add(Operation));
                    break;
                case "txbWhereFromWim":
                    Iso.InstallWim = open.OpenFile("wim", "esd");
                    break;
                case "txbWhereFromBoot":
                    Iso.BootWim = open.OpenFile("wim");
                    break;
                case "txbWhereFromIso":
                    Iso.WhereFrom = open.OpenPath();
                    break;
                case "txbToFolderIso":
                    Iso.To = open.SaveFile("Windows_Iso", "iso");
                    break;
                case "txbWhereFromConvert":
                    Convert.Image = open.OpenFile("wim", "esd");
                    break;
                case "txbToFolderConvert":
                    Convert.MountDir = open.SaveFile("install", "esd", "wim");
                    break;
            }
        });

        public ICommand Unmount => new RelayCommand<object>(obj =>
        {
            Treatment treatment = new Treatment();

            var copyVhdPath = Vhd.MountDir;
            var copyMOuntPath = CaptureMount.MountDir;

            Task.Run(() =>
            {
                Open open = new Open();

                var temp = string.Empty;

                Application.Current.Dispatcher.Invoke(() => temp = open.OpenPath());

                if (!string.IsNullOrEmpty(temp))
                {
                    if (MessageBox.Show("Убедитесь, что все программы, которые просматривают папку монтирования закрыты. Продолжить?", "Внимание", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        Operation = "Отключение образа";

                        treatment.UseCommand($"/unmount-image /mountdir:{temp} {UnmountMode((string)obj)}", "Dism.exe");

                        Application.Current.Dispatcher.Invoke(() => OperationLog.Add(Operation));
                    }
                }
            });
        });

        public ICommand Collect => new RelayCommand<object>(obj =>
        {
            Treatment treatment = new Treatment();

            Task.Run(() =>
            {
                Operation = "Сборка ISO";

                Application.Current.Dispatcher.Invoke(() => OperationLog.Add(Operation));

                if (!string.IsNullOrEmpty(Iso.InstallWim))
                {
                    ProgBarStart = true;

                    if (Iso.InstallWim.Contains(".wim"))
                    {
                        treatment.Copy(Iso.InstallWim, $@"{Iso.WhereFrom}\sources", "install.wim");
                    }
                    else if (Iso.InstallWim.Contains(".esd"))
                    {
                        treatment.Copy(Iso.InstallWim, $@"{Iso.WhereFrom}\sources", "install.esd");
                    }

                    ProgBarStart = false;
                }
                if (!string.IsNullOrEmpty(Iso.BootWim))
                {
                    ProgBarStart = true;

                    treatment.Copy(Iso.BootWim, $@"{Iso.WhereFrom}\sources", "boot.wim");

                    ProgBarStart = false;
                }

                treatment.UseOscdimg($@"-u2 -m -b{Iso.WhereFrom}\efi\microsoft\boot\efisys.bin {Iso.WhereFrom} {Iso.To}");
            });
        });

        public ICommand Abort => new RelayCommand<object>(obj =>
        {
            var temp = GetActualProcess();

            if (temp != null)
            {
                MessageBoxResult result = MessageBox.Show("Отменить операцию?", "Предупреждение", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                    AbortMethod(temp);
            }
        });

        public ICommand Convertation => new RelayCommand<object>(obj =>
        {
            Treatment treatment = new Treatment();

            string compress = string.Empty;

            if (Convert.NoCompress)
            {
                compress = "none";

                Operation = "Экспорт без сжатия. ";
            }
            if (Convert.Normal)
            {
                compress = "fast";

                Operation = "Экспорт с нормальным сжатием. ";
            }
            if (Convert.Strong)
            {
                compress = "max";

                Operation = "Экспорт с сильным сжатием. ";
            }
            if (Convert.Maximum)
            {
                compress = "recovery";

                Operation = "Экспорт с максимальным сжатием. Операция требует много ресурсов, по-этому компьютер может работать не стабильно. ";
            }

            Task.Run(() =>
            {
                treatment.UseCommand($"/export-image /sourceimagefile:{Convert.Image} /sourceindex:{Index} /destinationimagefile:{Convert.MountDir} /compress:{compress}", "Dism.exe");
            });

            Application.Current.Dispatcher.Invoke(() => OperationLog.Add(Operation));
        });

        public ICommand MinimizeProgramm => new RelayCommand<object>(obj =>
        {
            foreach (Window t in Application.Current.Windows)
            {
                if (t.Name.Contains("MainTreatmentWindow"))
                {
                    WindowBehavior.MinimizedWindow(t);
                }
            }
        });

        public ICommand CloseProgramm => new RelayCommand<object>(obj =>
        {
            if (!TabEnable)
            {
                var temp = GetActualProcess();

                if (temp != null)
                {
                    MessageBoxResult reult = MessageBox.Show("Идет работа над образом. Вы уверены, что хотите закрыть приложение и прервать работу?", "Внимание", MessageBoxButton.YesNo);

                    if (reult == MessageBoxResult.Yes)
                    {
                        AbortMethod(temp);
                        Application.Current.Shutdown();
                    }
                    else { }
                }
            }
            else
            {
                Application.Current.Shutdown();
            }
        });

        public MainViewModel()
        {
            Treatment.OutputEvent += Treatment_OutputEvent;
            Treatment.OscdimgEvent += Treatment_OscdimgEvent;
        }

        /// <summary>
        /// Метод отмены текущего действия
        /// </summary>
        /// <param name="result"></param>
        private void AbortMethod(Process killedProcess)
        {
            killedProcess.Kill();
            killedProcess.WaitForExit();

            if (killedProcess.ProcessName.Contains("oscdimg") && File.Exists(Iso.To))
            {
                File.Delete(Iso.To);
            }
            else if (killedProcess.ProcessName.Contains("Dism") && File.Exists(CaptureMount.Image))
            {
                File.Delete(CaptureMount.Image);
            }
            else if (killedProcess.ProcessName.Contains("Dism") && File.Exists(Convert.MountDir))
            {
                File.Delete(Convert.MountDir);
            }

            Output = $"{Operation}. Операция отменена";

            Application.Current.Dispatcher.Invoke(() => OperationLog.Add(Output));

            ProgBarStart = false;
            ProgBarValue = 0;
        }

        private Process GetActualProcess()
        {
            Process[] proc = Process.GetProcesses();

            Process temp = null;

            foreach (var t in proc)
            {
                if (t.ProcessName.Contains("Dism") || t.ProcessName.Contains("oscdimg"))
                {
                    temp = t;
                    break;
                }
                else
                {
                    temp = null;
                }
            }

            return temp;
        }

        private void Treatment_OscdimgEvent(string output, bool start, bool tabEnable)
        {
            Output = $"{Operation} {output}";
            ProgBarStart = start;
            TabEnable = tabEnable;
        }

        private void Treatment_OutputEvent(string output, bool start, bool tabEnable)
        {
            Output = $"{Operation}. {output}";
            ProgBarStart = start;
            TabEnable = tabEnable;

            if (!ProgBarStart && Output.Contains("["))
            {
                Output = $@"{Operation}. Операция выполняется.";
                ProgBarValue = output.Replace(" ", "").Length;
            }
        }

        private string UnmountMode(string obj)
        {
            string mode;

            if (obj == "True")
            {
                mode = "/discard";

                Vhd.MountDir = string.Empty;
            }
            else
            {
                if (!Discard)
                {
                    mode = "/commit";
                }
                else
                {
                    mode = "/discard";
                }

                CaptureMount.MountDir = string.Empty;
            }

            return mode;
        }
    }
}
