using LH.Results;

namespace LH.Initialization.UnitTests.Fakes
{
    public class FakeInitializable : IInitializable
    {
        public static int InitializeCallCount { get; private set; } = 0;

        public FakeInitializable()
        {
            CurrentCount = InitializeCallCount++;
        }

        public int CurrentCount { get; }

        public Task<Result> Initialize()
        {
            return Task.FromResult(Result.Success());
        }
    }
}