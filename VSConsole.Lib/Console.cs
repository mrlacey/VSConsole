using System.Diagnostics;

namespace VSConsole
{
    public static class Console
    {
        public static void WriteLine(string value)
        {
            System.Console.WriteLine(value);
            Debug.WriteLine($"{nameof(VSConsole)}-WriteLine::{value}");
        }

        public static void Write(string value)
        {
            System.Console.Write(value);
            Debug.WriteLine($"{nameof(VSConsole)}-Write::{value}");
        }

        public static void Clear()
        {
            System.Console.Clear();
            Debug.WriteLine($"{nameof(VSConsole)}-Clear::");
        }
    }
}
