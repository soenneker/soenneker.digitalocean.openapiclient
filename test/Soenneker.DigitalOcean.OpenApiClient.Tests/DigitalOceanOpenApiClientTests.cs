using Soenneker.Tests.HostedUnit;

namespace Soenneker.DigitalOcean.OpenApiClient.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class DigitalOceanOpenApiClientTests : HostedUnitTest
{
    public DigitalOceanOpenApiClientTests(Host host) : base(host)
    {
    }

    [Test]
    public void Default()
    {

    }
}
