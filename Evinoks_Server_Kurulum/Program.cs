using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Evinoks_Server_Kurulum
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        internal static ApplicationContext ac = new ApplicationContext();
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                ac.MainForm = new Login();
                Application.Run(ac);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
