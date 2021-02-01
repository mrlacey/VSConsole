using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace VSConsole
{
    public static class Console
    {
        public static ConsoleColor ForegroundColor
        {
            get
            {
                return System.Console.ForegroundColor;
            }

            set
            {
                Debug.WriteLine($"{nameof(VSConsole)}-{nameof(ForegroundColor)}::{value}");
                System.Console.ForegroundColor = value;
            }
        }

        public static ConsoleColor BackgroundColor
        {
            get
            {
                return System.Console.BackgroundColor;
            }

            set
            {
                Debug.WriteLine($"{nameof(VSConsole)}-{nameof(BackgroundColor)}::{value}");
                System.Console.BackgroundColor = value;
            }
        }

        public static void Beep()
        {
            // VSConsole does nothing here
            System.Console.Beep();
        }

        public static void Beep(int frequency, int duration)
        {
            // VSConsole does nothing here
            System.Console.Beep(frequency, duration);
        }

        public static void Clear()
        {
            System.Console.Clear();
            Debug.WriteLine($"{nameof(VSConsole)}-{nameof(Clear)}::");
        }

        public static void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop)
        {
            // VSConsole does nothing here
            System.Console.MoveBufferArea(sourceLeft, sourceTop, sourceWidth, sourceHeight, targetLeft, targetTop);
        }

        public static Stream OpenStandardError(int bufferSize)
        {
            // VSConsole does nothing here
            return System.Console.OpenStandardError(bufferSize);
        }

        public static Stream OpenStandardError()
        {
            // VSConsole does nothing here
            return System.Console.OpenStandardError();
        }

        public static Stream OpenStandardInput(int bufferSize)
        {
            // VSConsole does nothing here
            return System.Console.OpenStandardInput(bufferSize);
        }

        public static Stream OpenStandardInput()
        {
            // VSConsole does nothing here
            return System.Console.OpenStandardInput();
        }

        public static Stream OpenStandardOutput(int bufferSize)
        {
            // VSConsole does nothing here
            return System.Console.OpenStandardOutput(bufferSize);
        }

        public static Stream OpenStandardOutput()
        {
            // VSConsole does nothing here
            return System.Console.OpenStandardOutput();
        }

        public static int Read()
        {
            // VSConsole does nothing here
            return System.Console.Read();
        }

        public static ConsoleKeyInfo ReadKey(bool intercept)
        {
            // VSConsole does nothing here
            return System.Console.ReadKey(intercept);
        }

        public static ConsoleKeyInfo ReadKey()
        {
            // VSConsole does nothing here
            return System.Console.ReadKey();
        }

        public static string ReadLine()
        {
            // VSConsole does nothing here
            return System.Console.ReadLine();
        }

        public static void ResetColor()
        {
            Debug.WriteLine($"{nameof(VSConsole)}-ResetColor::");
            System.Console.ResetColor();
        }

        public static void SetBufferSize(int width, int height)
        {
            // VSConsole does nothing here
            System.Console.SetBufferSize(width, height);
        }

        public static void SetCursorPosition(int left, int top)
        {
            // VSConsole does nothing here
            System.Console.SetCursorPosition(left, top);
        }

        public static void SetError(TextWriter newError)
        {
            // VSConsole does nothing here
            System.Console.SetError(newError);
        }

        public static void SetIn(TextReader newIn)
        {
            // VSConsole does nothing here
            System.Console.SetIn(newIn);
        }

        public static void SetOut(TextWriter newOut)
        {
            // VSConsole does nothing here
            System.Console.SetOut(newOut);
        }

        public static void SetWindowPosition(int left, int top)
        {
            // VSConsole does nothing here
            System.Console.SetWindowPosition(left, top);
        }

        public static void SetWindowSize(int width, int height)
        {
            // VSConsole does nothing here
            System.Console.SetWindowSize(width, height);
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
