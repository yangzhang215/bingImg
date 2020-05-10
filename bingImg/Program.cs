using System;
using System.IO;
using System.Net;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text;

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
                    string htmlApi = "https://cn.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1&mkt=zh-CN";
                    string reqString = getHtml(htmlApi);
                    string htmlImg = getImgHtml(reqString);
                    Image img = getImg(htmlImg);
                    img.Save(path);
                    SystemParametersInfo(20, 0, path, 1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message,"bingImg",  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                SystemParametersInfo(20, 0, path, 1);
            }
            Application.Exit();
        }

        public static string getHtml(string html)
        {
            WebRequest req = WebRequest.Create(html);
            Stream reqStream = req.GetResponse().GetResponseStream();
            StreamReader reqStreamReader = new StreamReader(reqStream, Encoding.UTF8);
            return reqStreamReader.ReadToEnd(); 
        }

        public static string getImgHtml(string html)
        {
            int index = html.IndexOf("url") + 6;
            html = "https://cn.bing.com" + html.Substring(index);
            index = html.IndexOf(".jpg") + 4;
            html = html.Substring(0, index);
            return html; 
        }

        public static Image getImg(string html)
        {
            WebRequest req = WebRequest.Create(html);
            return Image.FromStream(req.GetResponse().GetResponseStream());

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
