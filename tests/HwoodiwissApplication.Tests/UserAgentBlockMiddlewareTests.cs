using System.Net;

namespace HwoodiwissApplication.Tests;

public sealed class UserAgentBlockMiddlewareTests
{
    [Test]
    public async Task UserAgentBlockMiddleware_WhenRequestIsReceivedFromABlockedUserAgent_ShouldReturnForbidden()
    {
        // Arrange
        await using var fixture = new HwoodiwissApplicationFixture();

        // Act
        var response = await fixture.GetForecasts(client => { client.DefaultRequestHeaders.UserAgent.ParseAdd("BadBot"); });

        // Assert
        Assert.Equals(response.StatusCode, HttpStatusCode.Forbidden);
    }
}