namespace Ploeh.Katas.RangeCSharp
{
    public sealed class Range<T> where T : IComparable<T>
    {
        public Range(ClosedEndpoint<T> min, ClosedEndpoint<T> max)
        {
            Min = min;
            Max = max;
        }

        public ClosedEndpoint<T> Min { get; }
        public ClosedEndpoint<T> Max { get; }

        public bool Contains(IEnumerable<T> candidates)
        {
            return candidates.All(c => Min.Value.CompareTo(c) <= 0 && c.CompareTo(Max.Value) <= 0);
        }
    }
}