using LH.Results.Errors;

namespace LH.Initialization.UnitTests.Fakes;

public static class FakeError
{
    public static Error Error => new("Error.code", "Error.message");
}