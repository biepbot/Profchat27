using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Profchat27
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            string[] arguments = Environment.GetCommandLineArgs();

            int id = 0;

            foreach (string a in arguments)
            {
                if (IsDigitsOnly(a))
                {
                    int.TryParse(a, out id);
                }
            }
            if (id != 0)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Chat(id));
            }
            else
            {
                Console.WriteLine("Application can not be run with these settings, it must be run through the main app!");
            }
        }

        private static bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }
    }
}
