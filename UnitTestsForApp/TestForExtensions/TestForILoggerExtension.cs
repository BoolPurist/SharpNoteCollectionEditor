using Moq;
using NoteCollectionEditor.Extensions;
using Splat;

namespace UnitTestsForApp.TestForExtensions;

public class TestForILoggerExtension
{
  [Fact]
  public void TestLogError()
  {
    AssertForOneLevel(LogLevel.Error, (logger, message) => logger.LogError(message));
  }
  
  [Fact]
  public void TestLogFatal()
  {
    AssertForOneLevel(LogLevel.Fatal, (logger, message) => logger.LogFatal(message));
  }
  
  [Fact]
  public void TestLogWarning()
  {
    AssertForOneLevel(LogLevel.Warn, (logger, message) => logger.LogWarning(message));
  }
  
  [Fact]
  public void TestLogInfo()
  {
    AssertForOneLevel(LogLevel.Info, (logger, message) => logger.LogInfo(message));
  }
  
  [Fact]
  public void TestLogDebug()
  {
    AssertForOneLevel(LogLevel.Debug, (logger, message) => logger.LogDebug(message));
  }

  private static void AssertForOneLevel(LogLevel toAssertWith, Action<ILogger, string> act)
  {
    var logger = Mock.Of<ILogger>();
    const string message = "Message";
    act(logger, message);
    Mock.Get(logger).Verify(toVerify => toVerify.Write(message, toAssertWith));
  }
}