using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Treatment_Wim.Interfaces
{
    interface IMount
    {
        string Image { get; set; }

        string MountDir { get; set; }

        void RunCommand(string whatCheck, string command);
    }
}
