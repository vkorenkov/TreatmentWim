using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Treatment_Wim.Interfaces;

namespace Treatment_Wim.ViewModels
{
    class ConvertBuild : PropertyChangeClass, IMount
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
            set { mountDir = value; OnPropertyChange(nameof(MountDir)); }
        }

        public bool NoCompress { get; set; }

        public bool Normal { get; set; }

        public bool Strong { get; set; }

        public bool Maximum { get; set; }

        public void RunCommand(string whatCheck, string command)
        {

        }
    }
}
