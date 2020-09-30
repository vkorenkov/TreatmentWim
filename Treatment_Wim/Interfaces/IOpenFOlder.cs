using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Treatment_Wim
{
    interface IOpenFOlder
    {
        string OpenFolderPath { get; set; }

        string OpenPath();

        string OpenFile(params string[] format);
    }
}
