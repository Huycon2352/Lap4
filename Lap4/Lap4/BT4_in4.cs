using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;

namespace Lap4
{
    public partial class BT4_in4 : Form
    {
        public BT4_in4()
        {
            InitializeComponent();

            // Path to the offline index.html file (from the offline site download)
            string offlineHtmlPath = Path.Combine(Application.StartupPath, "offline_site", "index.html");

            if (File.Exists(offlineHtmlPath))
            {
                // Read HTML content from the offline file
                string htmlContent = File.ReadAllText(offlineHtmlPath, Encoding.UTF8);

                // Display HTML content in RichTextBox (optional for preview)
                richTextBox1.Text = htmlContent;

                // Folder to save local assets (already downloaded in offline_site)
                string localPath = Path.Combine(Application.StartupPath, "offline_site");

                // Display updated HTML in WebBrowser control
                DisplayHtml(offlineHtmlPath);
            }
            else
            {
                MessageBox.Show("The offline HTML file does not exist.");
            }
        }

        private void DisplayHtml(string updatedHtmlPath)
        {
            // Display the offline HTML in the WebBrowser control
            webBrowser1.Navigate(new Uri(updatedHtmlPath));
        }
    }
}
