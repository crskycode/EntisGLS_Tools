using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSXTool.ECS.Enums
{
    public enum CSInstructionCode
    {
        csicNew = 0,
        csicFree = 1,
        csicLoad = 2,
        csicStore = 3,
        csicEnter = 4,
        csicLeave = 5,
        csicJump = 6,
        csicCJump = 7,
        csicCall = 8,
        csicReturn = 9,
        csicElement = 10,
        csicElementIndirect = 11,
        csicOperate = 12,
        csicUniOperate = 13,
        csicCompare = 14,
    }
}
