using LH.Results;

namespace LH.Initialization.UnitTests.Fakes
{
    public class FakeInitializable(Result _result) : IInitializable
    {
        public Task<Result> Initialize()
        {
            return Task.FromResult(_result);
        }
    }
}