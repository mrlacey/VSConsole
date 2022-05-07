using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace VSDebugConsole
{
    public static class Console
    {
        private static ConsoleColor _foregroundColor = ConsoleColor.White;

        public static ConsoleColor ForegroundColor
        {
            get
            {
                return _foregroundColor;
            }

            set
            {
                Debug.WriteLine($"{nameof(VSConsole)}-{nameof(ForegroundColor)}::{value}");
                _foregroundColor = value;
            }
        }

        private static ConsoleColor _backgroundColor = ConsoleColor.Black;

        public static ConsoleColor BackgroundColor
        {
            get
            {
                return _backgroundColor;
            }

            set
            {
                Debug.WriteLine($"{nameof(VSConsole)}-{nameof(BackgroundColor)}::{value}");
                _backgroundColor = value;
            }
        }

        public static void Beep()
        {
            // VSConsole does nothing here
        }

        public static void Beep(int frequency, int duration)
        {
            // VSConsole does nothing here
        }

        public static void Clear()
        {
            Debug.WriteLine($"{nameof(VSConsole)}-{nameof(Clear)}::");
        }

        public static void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop)
        {
            // VSConsole does nothing here
        }

        public static Stream OpenStandardError(int bufferSize)
        {
            // VSConsole does nothing here
            throw new NotSupportedException();
        }

        public static Stream OpenStandardError()
        {
            // VSConsole does nothing here
            throw new NotSupportedException();
        }

        public static Stream OpenStandardInput(int bufferSize)
        {
            // VSConsole does nothing here
            throw new NotSupportedException();
        }

        public static Stream OpenStandardInput()
        {
            // VSConsole does nothing here
            throw new NotSupportedException();
        }

        public static Stream OpenStandardOutput(int bufferSize)
        {
            // VSConsole does nothing here
            throw new NotSupportedException();
        }

        public static Stream OpenStandardOutput()
        {
            // VSConsole does nothing here
            throw new NotSupportedException();
        }

        public static int Read()
        {
            // VSConsole does nothing here
            throw new NotSupportedException();
        }

        public static ConsoleKeyInfo ReadKey(bool intercept)
        {
            // VSConsole does nothing here
            throw new NotSupportedException();
        }

        public static ConsoleKeyInfo ReadKey()
        {
            // VSConsole does nothing here
            throw new NotSupportedException();
        }

        public static string ReadLine()
        {
            // VSConsole does nothing here
            throw new NotSupportedException();
        }

        public static void ResetColor()
        {
            Debug.WriteLine($"{nameof(VSConsole)}-ResetColor::");
        }

        public static void SetBufferSize(int width, int height)
        {
            // VSConsole does nothing here
        }

        public static void SetCursorPosition(int left, int top)
        {
            // VSConsole does nothing here
        }

        public static void SetError(TextWriter newError)
        {
            // VSConsole does nothing here
        }

        public static void SetIn(TextReader newIn)
        {
            // VSConsole does nothing here
        }

        public static void SetOut(TextWriter newOut)
        {
            // VSConsole does nothing here
        }

        public static void SetWindowPosition(int left, int top)
        {
            // VSConsole does nothing here
        }

        public static void SetWindowSize(int width, int height)
        {
            // VSConsole does nothing here
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
