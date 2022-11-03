using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Katas.RangeCSharp
{
    public sealed class Endpoint<T>
    {
        private readonly T value;
        private readonly bool isClosed;

        private Endpoint(T value, bool isClosed)
        {
            this.value = value;
            this.isClosed = isClosed;
        }

        internal static Endpoint<T> Closed(T value)
        {
            return new Endpoint<T>(value, true);
        }

        internal static Endpoint<T> Open(T value)
        {
            return new Endpoint<T>(value, false);
        }

        public TResult Match<TResult>(
            Func<T, TResult> whenClosed,
            Func<T, TResult> whenOpen)
        {
            if (isClosed)
                return whenClosed(value);
            else
                return whenOpen(value);
        }

        public Endpoint<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            return Match(
                whenClosed: x => Endpoint.Closed(selector(x)),
                whenOpen: x => Endpoint.Open(selector(x)));
        }

        public override bool Equals(object? obj)
        {
            return obj is Endpoint<T> endpoint &&
                   EqualityComparer<T>.Default.Equals(value, endpoint.value) &&
                   isClosed == endpoint.isClosed;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(value, isClosed);
        }
    }

    public static class Endpoint
    {
        public static Endpoint<T> Closed<T>(T value)
        {
            return Endpoint<T>.Closed(value);
        }

        public static Endpoint<T> Open<T>(T value)
        {
            return Endpoint<T>.Open(value);
        }
    }
}
