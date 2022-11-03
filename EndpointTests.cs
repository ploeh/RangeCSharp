using CsCheck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ploeh.Katas.RangeCSharp
{
    public sealed class EndpointTests
    {
        [Fact]
        public void FirstFunctorLaw()
        {
            Gen.OneOf(
                Gen.Int.Select(Endpoint.Open),
                Gen.Int.Select(Endpoint.Closed))
            .Sample(expected =>
            {
                var actual = expected.Select(x => x);

                Assert.Equal(expected, actual);
            });
        }

        [Fact]
        public void ScondFunctorLaw()
        {
            (from endpoint in Gen.OneOf(
                Gen.Int.Select(Endpoint.Open),
                Gen.Int.Select(Endpoint.Closed))
             from f in Gen.OneOfConst<Func<int, int>>(x => x, x => x + 1, x => x * 2)
             from g in Gen.OneOfConst<Func<int, int>>(x => x, x => x + 1, x => x * 2)
             select (endpoint, f, g))
            .Sample(t =>
            {
                var actual = t.endpoint.Select(x => t.g(t.f(x)));

                Assert.Equal(
                    t.endpoint.Select(t.f).Select(t.g),
                    actual);
            });
        }
    }
}
