using NoteCollectionEditor.Extensions;
using Splat;

namespace UnitTestsForApp;

public class TestExtensionForReadonlyDependencyExtension
{
  [Fact]
  public void TestIfThrowsForNull()
  {
    IMutableDependencyResolver services = Locator.CurrentMutable;
    IReadonlyDependencyResolver resolver = Locator.Current;

    Assert.Throws<InvalidOperationException>(() =>
    {
      resolver.GetRequiredService<TestExtensionForReadonlyDependencyExtension>();
    });
  }
}