using CsCheck;
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
    }
}
