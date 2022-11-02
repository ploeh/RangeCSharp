namespace Ploeh.Katas.RangeCSharp
{
    public sealed class ClosedEndpoint<T>
    {
        public ClosedEndpoint(T value)
        {
            Value = value;
        }

        public T Value { get; }
    }
}