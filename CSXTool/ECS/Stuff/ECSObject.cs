using System.Collections.Generic;

namespace CSXTool.ECS.Stuff
{
    public class ECSObject
    {
    }

    public class ECSInteger : ECSObject
    {
        public uint Value { get; set; }

        public ECSInteger(uint value)
        {
            Value = value;
        }
    }

    public class ECSReal : ECSObject
    {
        public double Value { get; set; }

        public ECSReal(double value)
        {
            Value = value;
        }
    }

    public class ECSString : ECSObject
    {
        public string Value { get; set; }

        public ECSString(string value)
        {
            Value = value;
        }
    }

    public class ECSArray : ECSObject
    {
        private readonly List<ECSObject> _array;

        public ECSArray()
        {
            _array = new List<ECSObject>();
        }

        public ECSArray(int capacity)
        {
            _array = new List<ECSObject>(capacity);
        }

        public ECSObject this[int index]
        {
            get => _array[index];
            set => _array[index] = value;
        }

        public int Count
        {
            get => _array.Count;
        }

        public void Add(ECSObject obj)
        {
            _array.Add(obj);
        }
    }

    public class ECSClassInfoObject : ECSObject
    {
        public string ClassName { get; set; }

        public ECSClassInfoObject(string className)
        {
            ClassName = className;
        }
    }

    public class ECSHash : ECSObject
    {
    }

    public class ECSReference : ECSObject
    {
    }

    public class ECSGlobal : ECSObject
    {
        private readonly List<ECSObjectItem> _list;

        public ECSGlobal()
        {
            _list = new List<ECSObjectItem>();
        }

        public ECSGlobal(int capacity)
        {
            _list = new List<ECSObjectItem>(capacity);
        }

        public ECSObjectItem this[int index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        public int Count
        {
            get => _list.Count;
        }

        public void Add(string name, ECSObject obj)
        {
            _list.Add(new ECSObjectItem(name, obj));
        }
    }
}
