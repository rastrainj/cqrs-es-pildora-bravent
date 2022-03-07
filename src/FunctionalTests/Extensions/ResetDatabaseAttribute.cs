using System.Reflection;
using Xunit.Sdk;

namespace TrailRunning.Races.Management.FunctionalTests.Extensions;

public class ResetDatabaseAttribute
    : BeforeAfterTestAttribute
{
    public override void Before(MethodInfo methodUnderTest)
    {
        TestingWebAppFactory.ResetDatabase();
    }
}
