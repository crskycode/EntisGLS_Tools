using CSXTool.ECS.Enums;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CSXTool.ECS.Stuff
{
    public class ECSExecutionImageAssembly : IReadOnlyCollection<ECSExecutionImageCommandRecord>
    {
        private readonly LinkedList<ECSExecutionImageCommandRecord> _commandList;

        public ECSExecutionImageAssembly()
        {
            _commandList = new LinkedList<ECSExecutionImageCommandRecord>();
        }

        public void Add(CSInstructionCode code, int addr, int size)
        {
            var rec = new ECSExecutionImageCommandRecord();

            rec.Code = code;
            rec.Addr = addr;
            rec.Size = size;
            rec.NewAddr = addr;

            _commandList.AddLast(rec);
        }

        public IEnumerator<ECSExecutionImageCommandRecord> GetEnumerator()
        {
            return _commandList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count
        {
            get => _commandList.Count;
        }

        public int TotalCodeSize
        {
            get => _commandList.Sum(x => x.Size);
        }
    }
}
