using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Treatment_Wim.ViewModels
{
    class Options : PropertyChangeClass
    {
        public string Where { get; set; }

        private string appAssocPath;
        public string AppAssocPath
        {
            get { return appAssocPath; }
            set
            {
                appAssocPath = value; OnPropertyChange(nameof(AppAssocPath));

                Task.Run(() =>
                {
                    Treatment treatment = new Treatment();

                    if (!string.IsNullOrEmpty(AppAssocPath) && !string.IsNullOrEmpty(Where))
                        treatment.UseCommand($@"/image:""{Where}"" /import-defaultappassociations:""{AppAssocPath}""", "Dism.exe");
                });
            }
        }

        private string layoutPath;
        public string LayoutPath
        {
            get { return layoutPath; }
            set
            {
                layoutPath = value; OnPropertyChange(nameof(LayoutPath));

                Treatment treatment = new Treatment();

                Task.Run(() =>
                {
                    if (!string.IsNullOrEmpty(LayoutPath) && !string.IsNullOrEmpty(Where))
                    {
                        if(File.Exists(Where + @"\Users\Администратор\AppData\Local\Microsoft\Windows\Shell\DefaultLayouts.xml"))
                        {
                            File.Delete(Where + @"\Users\Администратор\AppData\Local\Microsoft\Windows\Shell\DefaultLayouts.xml");
                        }
                        if(File.Exists(Where + @"\Users\Default\AppData\Local\Microsoft\Windows\Shell\DefaultLayouts.xml"))
                        {
                            File.Delete(Where + @"\Users\Default\AppData\Local\Microsoft\Windows\Shell\DefaultLayouts.xml");
                        }

                        treatment.Copy(layoutPath, Where + @"\Users\Администратор\AppData\Local\Microsoft\Windows\Shell\", "LayoutModification.xml");
                        treatment.Copy(layoutPath, Where + @"\Users\Default\AppData\Local\Microsoft\Windows\Shell\", "LayoutModification.xml");
                    }
                });
            }
        }

        private string winRePath;
        public string WinRePath
        {
            get { return winRePath; }
            set
            {
                winRePath = value; OnPropertyChange(nameof(WinRePath));

                Task.Run(() =>
                {
                    if (!string.IsNullOrEmpty(WinRePath) && !string.IsNullOrEmpty(Where))
                    {
                        Treatment treatment = new Treatment();

                        treatment.Copy(WinRePath, Where+@"\Windows\System32\Recovery", "Winre.wim");
                    }
                });
            }
        }

        private string unattendPath;
        public string UnattendPath
        {
            get { return unattendPath; }
            set
            {
                unattendPath = value; OnPropertyChange(nameof(UnattendPath));

                Treatment treatment = new Treatment();

                Task.Run(() =>
                {
                    if (!string.IsNullOrEmpty(UnattendPath) && !string.IsNullOrEmpty(Where))
                        treatment.UseCommand($@"/image:""{Where}"" /apply-unattend:""{UnattendPath}""", "Dism.exe");
                });
            }
        }
    }
}
