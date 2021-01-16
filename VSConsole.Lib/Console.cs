using System.Diagnostics;
using System.Linq;

namespace VSConsole
{
    public static class Console
    {
        public static void Clear()
        {
            System.Console.Clear();
            Debug.WriteLine($"{nameof(VSConsole)}-Clear::");
        }

        public static void Write(ulong value)
        {
            Write(value.ToString());
        }

        public static void Write(bool value)
        {
            Write(value.ToString());
        }

        public static void Write(char value)
        {
            Write(value.ToString());
        }

        public static void Write(char[] buffer)
        {
            Write(new string(buffer));
        }

        public static void Write(char[] buffer, int index, int count)
        {
            Write(new string(buffer.Skip(index).Take(count).ToArray()));
        }

        public static void Write(double value)
        {
            Write(value.ToString());
        }

        public static void Write(long value)
        {
            Write(value.ToString());
        }

        public static void Write(object value)
        {
            Write(value.ToString());
        }

        public static void Write(float value)
        {
            Write(value.ToString());
        }

        public static void Write(string value)
        {
            System.Console.Write(value);
            Debug.WriteLine($"{nameof(VSConsole)}-Write::{value}");
        }

        public static void Write(string format, object arg0)
        {
            Write(string.Format(format, arg0));
        }

        public static void Write(string format, object arg0, object arg1)
        {
            Write(string.Format(format, arg0, arg1));
        }

        public static void Write(string format, object arg0, object arg1, object arg2)
        {
            Write(string.Format(format, arg0, arg1, arg2));
        }

        public static void Write(string format, params object[] arg)
        {
            Write(string.Format(format, arg));
        }

        public static void Write(uint value)
        {
            Write(value.ToString());
        }

        public static void Write(decimal value)
        {
            Write(value.ToString());
        }

        public static void Write(int value)
        {
            Write(value.ToString());
        }

        public static void WriteLine(ulong value)
        {
            WriteLine(value.ToString());
        }

        public static void WriteLine(string value, params object[] arg)
        {
            WriteLine(string.Format(value, arg));
        }

        public static void WriteLine()
        {
            WriteLine(string.Empty);
        }

        public static void WriteLine(bool value)
        {
            WriteLine(value.ToString());
        }

        public static void WriteLine(char[] buffer)
        {
            WriteLine(new string(buffer));
        }

        public static void WriteLine(char[] buffer, int index, int count)
        {
            WriteLine(new string(buffer.Skip(index).Take(count).ToArray()));
        }

        public static void WriteLine(decimal value)
        {
            WriteLine(value.ToString());
        }

        public static void WriteLine(double value)
        {
            WriteLine(value.ToString());
        }

        public static void WriteLine(uint value)
        {
            WriteLine(value.ToString());
        }

        public static void WriteLine(int value)
        {
            WriteLine(value.ToString());
        }

        public static void WriteLine(object value)
        {
            WriteLine(value.ToString());
        }

        public static void WriteLine(float value)
        {
            WriteLine(value.ToString());
        }

        public static void WriteLine(string value)
        {
            System.Console.WriteLine(value);
            Debug.WriteLine($"{nameof(VSConsole)}-WriteLine::{value}");
        }

        public static void WriteLine(string format, object arg0)
        {
            WriteLine(string.Format(format, arg0));
        }

        public static void WriteLine(string format, object arg0, object arg1)
        {
            WriteLine(string.Format(format, arg0, arg1));
        }

        public static void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            WriteLine(string.Format(format, arg0, arg1, arg2));
        }

        public static void WriteLine(long value)
        {
            WriteLine(value.ToString());
        }

        public static void WriteLine(char value)
        {
            WriteLine(value.ToString());
        }
    }
}
