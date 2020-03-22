using System;
using System.IO;
using System.Net;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace bingImg
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string path = Application.StartupPath + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".jpg";
            if (!File.Exists(path))
            {
                try
                {
                    WebRequest req = WebRequest.Create("https://api.sunweihu.com/api/bing1/api.php");
                    Image img = Image.FromStream(req.GetResponse().GetResponseStream());
                    img.Save(path);                
                    SystemParametersInfo(20, 0, path, 0x2);     
                }
                catch (Exception ex)
                {
                    MessageBox.Show("bingImg", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                SystemParametersInfo(20, 0, path, 0x2);
            }
            Application.Exit();
        }

        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        public static extern int SystemParametersInfo(
            int uAction,
            int uParam,
            string lpvParam,
            int fuWinIni
        );
    }
}
