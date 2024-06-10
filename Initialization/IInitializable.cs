using LH.Results;

namespace LH.Initialization;

public interface IInitializable
{
    Task<Result> Initialize();
}