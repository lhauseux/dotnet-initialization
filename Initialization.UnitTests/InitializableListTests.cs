using LH.Initialization;
using LH.Initialization.UnitTests.Fakes;
using LH.Results;
using LH.Results.Errors;
using Microsoft.Extensions.DependencyInjection;

namespace Initialization.UnitTests;

public class InitializableListTests
{
    [Fact]
    public async Task Initialize_ResultSuccess()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddInitializables(typeof(FakeInitializable));

        var initializableList = services.BuildServiceProvider().GetRequiredService<IInitializableList>();

        var result = await initializableList.Initialize();

        Assert.True(result.IsSuccess);
    }


    [Fact]
    public async Task Initialize_WithFailure_ResultFailure()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddInitializables(typeof(FakeInitializable));

        var serviceProvider = services.BuildServiceProvider();

        var initializables = serviceProvider.GetRequiredService<IEnumerable<IInitializable>>();
        var initializableList = serviceProvider.GetRequiredService<IInitializableList>();

        (initializables.ToList()[0] as FakeInitializable)!.Result = Result.Failure(FakeError.Error);


        var result = await initializableList.Initialize();

        Assert.True(result.IsFailure);
    }


    [Fact]
    public async Task Initialize_WithFailure_EnsureNextNotInitialized()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddInitializables(typeof(FakeInitializable), typeof(FakeInitializable));

        var serviceProvider = services.BuildServiceProvider();

        var initializablesList = serviceProvider.GetRequiredService<IEnumerable<IInitializable>>()
            .Cast<FakeInitializable>()
            .ToList();
        var initializableList = serviceProvider.GetRequiredService<IInitializableList>();
        initializablesList[0].Result = Result.Failure(FakeError.Error);

        var result = await initializableList.Initialize();

        Assert.True(result.IsFailure);
        Assert.False(initializablesList[1].IsInitializedCalled);
    }


    [Fact]
    public async Task Initialize_WithException_NoThrowAndResultIsFailure()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddInitializables(typeof(FakeInitializable), typeof(FakeInitializable));

        var serviceProvider = services.BuildServiceProvider();

        var initializablesList = serviceProvider.GetRequiredService<IEnumerable<IInitializable>>()
            .Cast<FakeInitializable>()
            .ToList();
        var initializableList = serviceProvider.GetRequiredService<IInitializableList>();
        initializablesList[0].ThrowException = true;

        var result = await Record.ExceptionAsync(initializableList.Initialize);

        Assert.Null(result);
        Assert.False(initializablesList[1].IsInitializedCalled);
    }


    [Fact]
    public async Task Initialize_WithException_ResultIsUnhandledException()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddInitializables(typeof(FakeInitializable), typeof(FakeInitializable));

        var serviceProvider = services.BuildServiceProvider();

        var initializablesList = serviceProvider.GetRequiredService<IEnumerable<IInitializable>>()
            .Cast<FakeInitializable>()
            .ToList();
        var initializableList = serviceProvider.GetRequiredService<IInitializableList>();
        initializablesList[0].ThrowException = true;

        var result = await initializableList.Initialize();

        Assert.Equal(ExceptionErrors.Unhandled("Unhandled exception").Code, result.Error.Code);
        Assert.False(initializablesList[1].IsInitializedCalled);
    }
}
