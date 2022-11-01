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
                    new ClosedEndpoint<short>(t.min),
                    new ClosedEndpoint<short>(t.max));

                var actual = sut.Contains(t.xs);

                Assert.True(actual, $"Expected {t.xs} to be contained in {sut}.");
            });
        }
    }
}
