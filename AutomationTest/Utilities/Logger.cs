namespace AutomationTest.Utilities
{
    public class Logger
    {
        public static log4net.ILog log { get; set; }

        public static void SetLogger()
        {
            log = log4net.LogManager.GetLogger(NUnit.Framework.TestContext.CurrentContext.Test.FullName);
        }
    }
}
