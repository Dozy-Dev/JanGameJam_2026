
namespace ProgressGraph
{
    public sealed class ProgressFlag
    {
        public string Id { get; }
        public bool IsOneWay { get; }

        internal bool IsSet;

        public ProgressFlag(string id, bool oneWay = true)
        {
            Id = id;
            IsOneWay = oneWay;
        }
    }
}