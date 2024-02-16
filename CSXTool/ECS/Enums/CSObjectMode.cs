using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSXTool.ECS.Enums
{
    public enum CSObjectMode
    {
        csomImmediate = 0,
        csomStack = 1,
        csomThis = 2,
        csomGlobal = 3,
        csomData = 4,
        csomAuto = 5,
    }
}
