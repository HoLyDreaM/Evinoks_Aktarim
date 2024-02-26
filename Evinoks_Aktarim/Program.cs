using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Evinoks_Aktarim
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
                DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                //var _licenseHelper = new LicenseHelper();
                //if(_licenseHelper.Check(false))
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
