using System.Collections.Generic;

namespace CSXTool.ECS.Stuff
{
    public record TaggedRefAddress(string Tag, List<int> Refs);
}
