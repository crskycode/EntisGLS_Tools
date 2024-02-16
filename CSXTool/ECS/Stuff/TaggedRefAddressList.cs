using System.Collections.Generic;

namespace CSXTool.ECS.Stuff
{
    public class TaggedRefAddressList
    {
        private readonly List<TaggedRefAddress> _list;

        public TaggedRefAddressList()
        {
            _list = new List<TaggedRefAddress>();
        }

        public TaggedRefAddressList(int capacity)
        {
            _list = new List<TaggedRefAddress>(capacity);
        }

        public TaggedRefAddress this[int index]
        {
            get => _list[index];
        }

        public int Count
        {
            get => _list.Count;
        }

        public void Add(string name, List<int> data)
        {
            _list.Add(new TaggedRefAddress(name, data));
        }
    }
}
