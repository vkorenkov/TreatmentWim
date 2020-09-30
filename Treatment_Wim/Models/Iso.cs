using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Treatment_Wim.ViewModels
{
    class Iso : PropertyChangeClass
    {
        private string installWim;
        public string InstallWim
        {
            get { return installWim; }
            set { installWim = value; OnPropertyChange(nameof(InstallWim)); }
        }

        private string bootWim;
        public string BootWim
        {
            get { return bootWim; }
            set { bootWim = value; OnPropertyChange(nameof(BootWim)); }
        }

        private string whereFrom;
        public string WhereFrom
        {
            get { return whereFrom; }
            set { whereFrom = value; OnPropertyChange(nameof(WhereFrom)); }
        }
       
        private string to;
        public string To
        {
            get { return to; }
            set { to = value; OnPropertyChange(nameof(To)); }
        }
    }
}
