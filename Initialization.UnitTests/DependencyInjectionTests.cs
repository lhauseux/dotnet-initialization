using LH.Initialization;
using LH.Initialization.UnitTests.Fakes;
using Microsoft.Extensions.DependencyInjection;

namespace Initialization.UnitTests;

public class DependencyInjectionTests
{
    [Fact]
    public void AddInitializables_Ok()
    {
        var services = new ServiceCollection();
        services.AddInitializables(typeof(FakeInitializable));

        var initializable = services.BuildServiceProvider().GetService<IInitializable>();

        Assert.NotNull(initializable);
    }


    [Fact]
    public void AddInitializables_Nothing_NotOk()
    {
        var services = new ServiceCollection();

        var initializable = services.BuildServiceProvider().GetService<IInitializable>();

        Assert.Null(initializable);
    }

    [Fact]
    public void AddInitializables_Multiple_Ok()
    {
        var services = new ServiceCollection();
        services.AddInitializables(typeof(FakeInitializable), typeof(FakeInitializable), typeof(FakeInitializable));

        var initializables = services.BuildServiceProvider().GetRequiredService<IEnumerable<IInitializable>>()
            .Cast<FakeInitializable>()
            .ToList();

        Assert.NotNull(initializables);

        initializables[0].CurrentCount = 0;
        initializables[1].CurrentCount = 1;
        initializables[2].CurrentCount = 2;

        var currentCount = 0;
        foreach (var initializable in initializables)
        {
            Assert.Equal(currentCount, initializable.CurrentCount);
            currentCount++;
        }
    }


    [Fact]
    public void AddInitializables_MultipleNothing_NotOk()
    {
        var services = new ServiceCollection();

        var initializables = services.BuildServiceProvider().GetRequiredService<IEnumerable<IInitializable>>();

        Assert.Empty(initializables);
    }
}
