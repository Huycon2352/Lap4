using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack; // Để sử dụng HtmlAgilityPack.HtmlDocument
using HtmlDoc = HtmlAgilityPack.HtmlDocument; // Alias cho HtmlAgilityPack.HtmlDocument

namespace Lap4
{
    public partial class BT4 : Form
    {
        public BT4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //1279, 627
                this.Width = 965;
                this.Height = 540;

                webBrowser2.Navigate(textBox1.Text);
                webBrowser2.Width = 900;
                webBrowser2.Height = 400;
                button2.Enabled = true;
            }
            catch
            {
                MessageBox.Show("URL not available !!!");
                return;
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            string url = textBox1.Text; // Thay bằng URL của bạn
            string outputDirectory = "offline_site";

            // Tạo thư mục lưu trữ
            Directory.CreateDirectory(outputDirectory);

            // Bước 1: Tải trang HTML một cách bất đồng bộ
            HttpClient client = new HttpClient();
            string htmlContent = await client.GetStringAsync(url);

            // Ghi nội dung HTML vào tệp trong một Task.Run() để tránh làm chậm giao diện
            string indexPath = Path.Combine(outputDirectory, "index.html");
            await Task.Run(() => File.WriteAllText(indexPath, htmlContent));

            // Bước 2: Phân tích và tìm các tài nguyên
            HtmlDoc doc = new HtmlDoc();
            doc.LoadHtml(htmlContent);

            // Bước 3: Tải tài nguyên và thay đổi đường dẫn
            await Task.WhenAll(
                DownloadResources(doc, client, url, "img", "src", Path.Combine(outputDirectory, "img")),
                DownloadResources(doc, client, url, "link", "href", Path.Combine(outputDirectory, "css")),
                DownloadResources(doc, client, url, "script", "src", Path.Combine(outputDirectory, "js"))
            );

            // Bước 4: Lưu lại file HTML với đường dẫn tài nguyên tương đối
            await Task.Run(() => doc.Save(indexPath));

            // Mở form BT4_in4 sau khi hoàn thành tất cả các tác vụ bất đồng bộ
            Invoke(new Action(() =>
            {
                BT4_in4 form = new BT4_in4();
                form.Show();
            }));
        }




        static async Task DownloadResources(HtmlDoc doc, HttpClient client, string baseUrl, string tag, string attribute, string outputFolder)
        {
            Directory.CreateDirectory(outputFolder);

            var nodes = doc.DocumentNode.SelectNodes($"//{tag}[@{attribute}]");
            if (nodes == null) return;

            foreach (var node in nodes)
            {
                string resourceUrl = node.GetAttributeValue(attribute, null);
                if (string.IsNullOrEmpty(resourceUrl)) continue;

                // Tạo URL tuyệt đối từ URL cơ bản và tài nguyên URL
                Uri resourceUri = new Uri(new Uri(baseUrl), resourceUrl);

                // Lấy tên file và đường dẫn lưu tài nguyên
                string fileName = Path.GetFileName(resourceUri.LocalPath);
                string localPath = Path.Combine(outputFolder, fileName);

                try
                {
                    // Tải tài nguyên (hình ảnh, CSS, JS, v.v.)
                    byte[] resourceData = await client.GetByteArrayAsync(resourceUri);

                    // Lưu tài nguyên vào tệp
                    using (FileStream fs = new FileStream(localPath, FileMode.Create, FileAccess.Write))
                    {
                        await fs.WriteAsync(resourceData, 0, resourceData.Length);
                    }

                    // Cập nhật đường dẫn trong HTML từ tuyệt đối thành tương đối
                    string relativePath = Path.Combine(Path.GetFileName(outputFolder), fileName).Replace("\\", "/");
                    node.SetAttributeValue(attribute, relativePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to download {resourceUri}: {ex.Message}");
                }
            }
        }
    }
}
