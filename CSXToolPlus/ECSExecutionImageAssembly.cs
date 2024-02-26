using CSXToolPlus.Types;
using System.Collections.Generic;
using System.Linq;

namespace CSXToolPlus
{
    public class ECSExecutionImageAssembly
    {
        private readonly LinkedList<ECSExecutionImageCommandRecord> _commands;

        public ECSExecutionImageAssembly()
        {
            _commands = new LinkedList<ECSExecutionImageCommandRecord>();
        }

        public void Add(CSInstructionCode code, int addr, int size)
        {
            var rec = new ECSExecutionImageCommandRecord();

            rec.Code = code;
            rec.Addr = addr;
            rec.Size = size;
            rec.NewAddr = addr;

            _commands.AddLast(rec);
        }

        public IEnumerator<ECSExecutionImageCommandRecord> GetEnumerator()
        {
            return _commands.GetEnumerator();
        }

        public int Count
        {
            get => _commands.Count;
        }

        public int TotalCodeSize
        {
            get => _commands.Sum(x => x.Size);
        }
    }
}
