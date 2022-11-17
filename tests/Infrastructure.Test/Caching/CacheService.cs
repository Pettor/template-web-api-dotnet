using Backend.Application.Common.Caching;
using FluentAssertions;
using Xunit;

namespace Infrastructure.Test.Caching;

public abstract class CacheService<TCacheService>
    where TCacheService : ICacheService
{
    private record TestRecord(Guid Id, string StringValue, DateTime DateTimeValue);

    private const string TestKey = "testkey";
    private const string TestValue = "testvalue";

    protected abstract TCacheService CreateCacheService();

    [Fact]
    public void ThrowsGivenNullKey()
    {
        var sut = CreateCacheService();

        var action = () => { string? result = sut.Get<string>(null!); };

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ReturnsNullGivenNonExistingKey()
    {
        var sut = CreateCacheService();

        string? result = sut.Get<string>(TestKey);

        result.Should().BeNull();
    }

#pragma warning disable RCS1158
    public static IEnumerable<object[]> ValueData => new List<object[]>
#pragma warning restore RCS1158
        {
            new object[] { TestKey, TestValue },
            new object[] { "integer", 1 },
            new object[] { "long", 1L },
            new object[] { "double", 1.0 },
            new object[] { "bool", true },
            new object[] { "date", new DateTime(2022, 1, 1) },
        };

    [Theory]
    [MemberData(nameof(ValueData))]
    public void ReturnsExistingValueGivenExistingKey<T>(string testKey, T testValue)
    {
        var sut = CreateCacheService();

        sut.Set(testKey, testValue);
        var result = sut.Get<T>(testKey);

        result.Should().Be(testValue);
    }

    [Fact]
    public void ReturnsExistingObjectGivenExistingKey()
    {
        var expected = new TestRecord(Guid.NewGuid(), TestValue, DateTime.UtcNow);
        var sut = CreateCacheService();

        sut.Set(TestKey, expected);
        var result = sut.Get<TestRecord>(TestKey);

        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ReturnsNullGivenAnExpiredKey()
    {
        var sut = CreateCacheService();
        sut.Set(TestKey, TestValue, TimeSpan.FromMilliseconds(200));

        string? result = sut.Get<string>(TestKey);
        Assert.Equal(TestValue, result);

        await Task.Delay(250);
        result = sut.Get<string>(TestKey);

        result.Should().BeNull();
    }
}
