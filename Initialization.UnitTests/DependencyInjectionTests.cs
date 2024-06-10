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

        var initializables = services.BuildServiceProvider().GetRequiredService<IEnumerable<IInitializable>>();

        Assert.NotNull(initializables);

        var currentCount = 0;
        foreach (var initializable in initializables)
        {
            var fakeInitializable = initializable as FakeInitializable;
            Assert.Equal(currentCount, fakeInitializable?.CurrentCount);
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
