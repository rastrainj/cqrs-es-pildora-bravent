using TrailRunning.Races.Core.Aggregates;

namespace TrailRunning.Races.Management.FunctionalTests;

public class Given
{
    private readonly TestingWebAppFactory _testingWebAppFactory;

    public Given(TestingWebAppFactory testingWebAppFactory) => _testingWebAppFactory = testingWebAppFactory;

    public async Task<T?> GetAsync<T>(Guid id) where T : class, IAggregate
        => await _testingWebAppFactory.ExecuteAsync<T>(async aggregateRepository =>
        {
            var aggregate = await aggregateRepository.FindAsync(id, CancellationToken.None);
            return aggregate;
        });
}
