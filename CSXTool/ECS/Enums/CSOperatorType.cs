using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSXTool.ECS.Enums
{
    public enum CSOperatorType
    {
        csotNop = 0xFF,
        csotAdd = 0,
        csotSub = 1,
        csotMul = 2,
        csotDiv = 3,
        csotMod = 4,
        csotAnd = 5,
        csotOr = 6,
        csotXor = 7,
        csotLogicalAnd = 8,
        csoutLogicalOr = 9,
    }
}
