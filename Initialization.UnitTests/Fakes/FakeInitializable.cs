using LH.Results;

namespace LH.Initialization.UnitTests.Fakes
{
    public class FakeInitializable : IInitializable
    {

        public Result Result { get; set; } = Result.Success();
        public bool ThrowException { get; set; } = false;
        public int CurrentCount { get; set; }
        public bool IsInitializedCalled { get; private set; } = false;

        public Task<Result> Initialize()
        {
            IsInitializedCalled = true;
            if (ThrowException)
            {
                throw new Exception();
            }
            return Task.FromResult(Result);
        }
    }
}