namespace Ploeh.Katas.RangeCSharp
{
    public sealed class Range<T>
    {
        public Range(ClosedEndpoint<T> min, ClosedEndpoint<T> max)
        {
        }

        public bool Contains(IEnumerable<T> candidates)
        {
            return true;
        }
    }
}