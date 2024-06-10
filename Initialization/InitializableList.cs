
using LH.Results;
using LH.Results.Errors;
using Microsoft.Extensions.Logging;

namespace LH.Initialization;

public class InitializableList(ILogger<InitializableList> _logger,
    IEnumerable<IInitializable> _initializables) : IInitializableList
{
    public async Task<Result> Initialize()
    {
        _logger.LogInformation("Initializing {@Count} initializables", _initializables.Count());

        foreach (var initializable in _initializables)
        {
            _logger.LogDebug("Initializing {@Initializable}", initializable.GetType().Name);
            try
            {
                var result = await initializable.Initialize();
                if (result.IsSuccess)
                {
                    _logger.LogDebug("Initialized {@Initializable}", initializable.GetType().Name);
                }
                else
                {
                    _logger.LogError("Failed to initialize {@Initializable}: {@Error}", initializable.GetType().Name, result.Error);
                    return result;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An unhandled exception occurred in {@Initializable}", initializable.GetType().Name);
                return Result.Failure(ExceptionErrors.Unhandled(e.Message));
            }
        }

        _logger.LogInformation("Initialized");
        return Result.Success();
    }
}