namespace Ploeh.Katas.RangeCSharp
{
    public sealed class Range<T> where T : IComparable<T>
    {
        private readonly Endpoint<T> min;
        private readonly Endpoint<T> max;

        public Range(Endpoint<T> min, Endpoint<T> max)
        {
            this.min = min;
            this.max = max;
        }

        public bool Contains(IEnumerable<T> candidates)
        {
            return candidates.All(IsInRange);
        }

        private bool IsInRange(T candidate)
        {
            return min.Match(
                whenClosed: l => max.Match(
                    whenClosed: h => l.CompareTo(candidate) <= 0 && candidate.CompareTo(h) <= 0,
                    whenOpen: h => l.CompareTo(candidate) <= 0 && candidate.CompareTo(h) < 0),
                whenOpen: l => max.Match(
                    whenClosed: h => l.CompareTo(candidate) < 0 && candidate.CompareTo(h) <= 0,
                    whenOpen: h => l.CompareTo(candidate) < 0 && candidate.CompareTo(h) < 0));
        }

        public Range<TResult> Select<TResult>(Func<T, TResult> selector)
            where TResult : IComparable<TResult>
        {
            return new Range<TResult>(min.Select(selector), max.Select(selector));
        }

        public override bool Equals(object? obj)
        {
            return obj is Range<T> range &&
                   EqualityComparer<Endpoint<T>>.Default.Equals(min, range.min) &&
                   EqualityComparer<Endpoint<T>>.Default.Equals(max, range.max);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(min, max);
        }
    }
}