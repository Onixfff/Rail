using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rail
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new zero_silo());
#if Debug || OPER
            Application.Run(new general());
            //Application.Run(new report());
#endif
#if user
            Application.Run(new balance()); 
#endif
        }
    }
}
