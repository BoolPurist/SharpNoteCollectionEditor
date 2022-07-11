using Moq;
using NoteCollectionEditor.Services;
using Splat;

namespace UnitTestsForApp.TestsForServices;

public class TestLogException
{
  [Fact]
  public async Task TestIfMessageAndExceptionIsLogged()
  {
    var mocker = new Mock<ILogger>();
    const string expectedErrorMessage = "Error";
    var toTest = new LogException(mocker.Object);

    try
    {
      await WillThrow();
    }
    catch (Exception ex)
    {
      var toString = ex.ToString();
      toTest.HandleExceptionAsError(ex, expectedErrorMessage);
      mocker.Verify(logger => logger.Write(expectedErrorMessage, LogLevel.Fatal));
      mocker.Verify(logger => logger.Write(toString, LogLevel.Fatal));
    }
  }

#pragma warning disable CS1998
  public async Task WillThrow()
#pragma warning restore CS1998
  {
    throw new Exception();
  }
}