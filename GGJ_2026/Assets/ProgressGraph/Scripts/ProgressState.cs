using System.Collections.Generic;

namespace ProgressGraph
{
    internal sealed class ProgressState
    {
        internal readonly Dictionary<string, ProgressFlag> Flags =
            new Dictionary<string, ProgressFlag>();

        internal readonly List<ProgressDependency> Dependencies =
            new List<ProgressDependency>();
    }
}