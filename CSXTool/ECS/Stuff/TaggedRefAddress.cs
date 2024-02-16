using System.Collections.Generic;

namespace CSXTool.ECS.Stuff
{
    public record TaggedRefAddress(string Tag, List<int> Refs)
    {
        public string Tag { get; set; } = Tag;
        public List<int> Refs { get; set; } = Refs;
    }
}
