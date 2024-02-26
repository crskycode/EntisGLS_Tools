using CSXToolPlus.Types;

namespace CSXToolPlus
{
    public class ECSExecutionImageCommandRecord
    {
        public CSInstructionCode Code { get; set; }
        public int Addr { get; set; }
        public int Size { get; set; }
        public int NewAddr { get; set; }
    }
}
