using System;
using System.Windows.Forms;

namespace ProgrammingAdvent2016
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        public static bool ReadInputFile(out string text, string path)
        {
            try
            {
                text = System.IO.File.ReadAllText(path);
                return true;
            }
            catch
            {
                text = "ERROR";
                return false;
            }
        }

        public static string[] ToLines(this string input)
        {
            return input.TrimEnd(new[] { '\r', '\n' }).Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        }
    }
}
