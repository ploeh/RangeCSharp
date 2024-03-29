﻿using CsCheck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ploeh.Katas.RangeCSharp
{
    public sealed class RangeTests
    {
        [Fact]
        public void ClosedRangeContainsList()
        {
            (from xs in Gen.Short.Enumerable.Nonempty
             let min = xs.Min()
             let max = xs.Max()
             select (xs, min, max))
            .Sample(t =>
            {
                var sut = new Range<short>(
                    Endpoint.Closed(t.min),
                    Endpoint.Closed(t.max));

                var actual = sut.Contains(t.xs);

                Assert.True(actual, $"Expected {t.xs} to be contained in {sut}.");
            });
        }

        [Fact]
        public void ClosedRangeDoesNotContainElementsOutsideRange()
        {
            (from min in Gen.Int
             from size in Gen.Int[1, 99]
             select (min, size))
            .Sample(t =>
            {
                var max = t.min + t.size;
                var sut = new Range<int>(
                    Endpoint.Closed(t.min),
                    Endpoint.Closed(max));

                var outside = new[] { t.min - 1, max + 1 };
                var actual = sut.Contains(outside);

                Assert.False(actual, $"Expected {sut} to not contain {outside}.");
            });
        }

        [Fact]
        public void OpenRangeContainsList()
        {
            (from xs in Gen.Long.Enumerable.Nonempty
             let min = xs.Min()
             let max = xs.Max()
             select (xs, min, max))
            .Sample(t =>
            {
                var sut = new Range<long>(
                    Endpoint.Open(t.min - 1),
                    Endpoint.Open(t.max + 1));

                var actual = sut.Contains(t.xs);

                Assert.True(actual, $"Expected {t.xs} to be contained in {sut}.");
            });
        }

        [Fact]
        public void OpenRangeDoesNotContainEndpoints()
        {
            (from min in Gen.Int
             from size in Gen.Int[1, 99]
             select (min, size))
            .Sample(t =>
            {
                var max = t.min + t.size;
                var sut = new Range<int>(
                    Endpoint.Open(t.min),
                    Endpoint.Open(max));

                var outside = new[] { t.min, max };
                var actual = sut.Contains(outside);

                Assert.False(actual, $"Expected {sut} to not contain {outside}.");
            });
        }

        [Fact]
        public void OpenClosedRangeContainsList()
        {
            (from xs in Gen.Long.Enumerable.Nonempty
             let min = xs.Min()
             let max = xs.Max()
             select (xs, min, max))
            .Sample(t =>
            {
                var sut = new Range<long>(
                    Endpoint.Open(t.min - 1),
                    Endpoint.Closed(t.max));

                var actual = sut.Contains(t.xs);

                Assert.True(actual, $"Expected {t.xs} to be contained in {sut}.");
            });
        }

        [Fact]
        public void OpenClosedRangeDoesNotContainEndpoints()
        {
            (from min in Gen.Int
             from size in Gen.Int[1, 99]
             select (min, size))
            .Sample(t =>
            {
                var max = t.min + t.size;
                var sut = new Range<int>(
                    Endpoint.Open(t.min),
                    Endpoint.Closed(max));

                var outside = new[] { t.min, max + 1 };
                var actual = sut.Contains(outside);

                Assert.False(actual, $"Expected {sut} to not contain {outside}.");
            });
        }

        [Fact]
        public void ClosedOpenRangeContainsList()
        {
            (from xs in Gen.Long.Enumerable.Nonempty
             let min = xs.Min()
             let max = xs.Max()
             select (xs, min, max))
            .Sample(t =>
            {
                var sut = new Range<long>(
                    Endpoint.Closed(t.min),
                    Endpoint.Open(t.max + 1));

                var actual = sut.Contains(t.xs);

                Assert.True(actual, $"Expected {t.xs} to be contained in {sut}.");
            });
        }

        [Fact]
        public void ClosedOpenRangeDoesNotContainEndpoints()
        {
            (from min in Gen.Int
             from size in Gen.Int[1, 99]
             select (min, size))
            .Sample(t =>
            {
                var max = t.min + t.size;
                var sut = new Range<int>(
                    Endpoint.Closed(t.min),
                    Endpoint.Open(max));

                var outside = new[] { t.min - 1, max };
                var actual = sut.Contains(outside);

                Assert.False(actual, $"Expected {sut} to not contain {outside}.");
            });
        }

        [Fact]
        public void FirstFunctorLaw()
        {
            var genEndpoint = Gen.OneOf(
                Gen.Int.Select(Endpoint.Closed),
                Gen.Int.Select(Endpoint.Open));
            genEndpoint.SelectMany(min => genEndpoint
                .Select(max => new Range<int>(min, max)))
            .Sample(sut =>
            {
                var actual = sut.Select(x => x);

                Assert.Equal(sut, actual);
            });
        }

        [Fact]
        public void SecondFunctorLaw()
        {
            var genEndpoint = Gen.OneOf(
                Gen.Int.Select(Endpoint.Closed),
                Gen.Int.Select(Endpoint.Open));
            (from min in genEndpoint
             from max in genEndpoint
             from f in Gen.OneOfConst<Func<int, int>>(x => x, x => x + 1, x => x * 2)
             from g in Gen.OneOfConst<Func<int, int>>(x => x, x => x + 1, x => x * 2)
             select (sut : new Range<int>(min, max), f, g))
            .Sample(t =>
            {
                var actual = t.sut.Select(x => t.g(t.f(x)));

                Assert.Equal(
                    t.sut.Select(t.f).Select(t.g),
                    actual);
            });
        }
    }
}
