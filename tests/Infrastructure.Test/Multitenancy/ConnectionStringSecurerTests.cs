﻿using Backend.Application.Common.Persistence;
using Infrastructure.Test.Multitenancy.Fixtures;
using Xunit;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Infrastructure.Test.Multitenancy;

public class ConnectionStringSecurerTests : TestBed<TestFixture>
{
    private const string Mssql =
        "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=backendTemplateDb;MultipleActiveResultSets=True;";
    private const string Mysql = "server=127.0.0.1;database=test";
    private readonly IConnectionStringSecurer? _makeSecureConnectionString;

    public ConnectionStringSecurerTests(ITestOutputHelper testOutputHelper, TestFixture fixture)
        : base(testOutputHelper, fixture)
    {
        _makeSecureConnectionString = _fixture.GetService<IConnectionStringSecurer>(
            _testOutputHelper
        );
    }

    [Theory]
    [InlineData(
        Mssql + ";Integrated Security=True;",
        "mssql",
        "MSSQL: CASE 1 - Integrated Security"
    )]
    [InlineData(Mssql + ";user id=sa;password=pass;", "mssql", "MSSQL: CASE 2 - Credentials")]
    [InlineData(Mysql + ";uid=root;pwd=12345;", "mysql", "MYSQL: CASE 3 - Credentials")]
    public void MakeSecureTest(string connectionString, string dbProvider, string name)
    {
        var res1 = _makeSecureConnectionString?.MakeSecure(connectionString, dbProvider);
        var check1 = _makeSecureConnectionString?.MakeSecure(res1, dbProvider);

        Assert.True(check1?.Equals(res1, StringComparison.InvariantCultureIgnoreCase), name);
    }

    protected override void Clear() { }

    protected override ValueTask DisposeAsyncCore() => new();
}
