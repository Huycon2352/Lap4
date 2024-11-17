using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Xml.Linq;

namespace Lap4
{
    public partial class BT3_ : Form
    {
        public BT3_()
        {
            InitializeComponent();
        }
        private static HttpClient client = new HttpClient();

        class Mem_NT106_P11
        {
            public int id { get; set; }
            public string name { get; set; }
            public string role { get; set; }
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            textBox2.Enabled = false;
            textBox3.Enabled = false;

            try
            {
                string id = textBox1.Text;
                var response = await client.GetStringAsync($"http://localhost:5000/api/Members/{id}");
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var member = JsonSerializer.Deserialize<Mem_NT106_P11>(response,options);
                
                if ( member != null)
                {
                    //listView1.Items.Clear();
                    ListViewItem item = new ListViewItem(id);
                    item.SubItems.Add(member.name);
                    item.SubItems.Add(member.role);
                    listView1.Items.Add(item);
                }

            }
            catch
            {
                MessageBox.Show("Error, Please try again later !!!");
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Enabled is false || textBox3.Enabled is false)
            {
                textBox2.Enabled = true;
                textBox3.Enabled = true;
                return;
            }
            try
            {
                string ID = textBox1.Text;
                if (string.IsNullOrEmpty(ID))
                {
                    MessageBox.Show("Please enter a valid ID.");
                    return;
                }
                // Đầu vào ID từ người dùng
                var member = new
                {
                    id = Int32.Parse(ID),
                    Name = textBox2.Text,  // Lấy tên từ text box
                    Role = textBox3.Text   // Lấy role từ text box
                };

                string json = JsonSerializer.Serialize(member);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Thực hiện PUT request
                var response = await client.PutAsync($"http://localhost:5000/api/members/{ID}", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Member updated successfully!");
                }
                else
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Failed to update member! Response Code: {response.StatusCode}, Message: {responseContent}");
                }
            }
            catch (HttpRequestException ex)
            {

                MessageBox.Show($"HttpRequestException: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"General Error: {ex.Message}");
            }
            


        }

        private async void button4_Click(object sender, EventArgs e)
        {
            if ( textBox2.Enabled is false || textBox3.Enabled is false)
            {
                textBox2.Enabled = true;
                textBox3.Enabled = true;
                return;
            }

            try
            {
                string ID = textBox1.Text;
                if (string.IsNullOrEmpty(ID))
                {
                    MessageBox.Show("Please enter a valid ID.");
                    return;
                }

                // Đầu vào ID và các thông tin từ người dùng
                var member = new
                {
                    id = Int32.Parse(ID),
                    Name = textBox2.Text,  // Lấy tên từ text box
                    Role = textBox3.Text   // Lấy role từ text box
                };

                string json = JsonSerializer.Serialize(member);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Gửi yêu cầu POST
                var response = await client.PostAsync("http://localhost:5000/api/members", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Member created successfully!");
                }
                else
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Failed to create member! Response Code: {response.StatusCode}, Message: {responseContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"HttpRequestException: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"General Error: {ex.Message}");
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            if (textBox2.Enabled is true || textBox3.Enabled is true)
            {
                textBox2.Enabled = false;
                textBox3.Enabled = false;
                return;
            }
            try
            {
                string ID = textBox1.Text;
                if (string.IsNullOrEmpty(ID))
                {
                    MessageBox.Show("Please enter a valid ID.");
                    return;
                }

                // Gửi yêu cầu DELETE
                var response = await client.DeleteAsync($"http://localhost:5000/api/members/{ID}");

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Member deleted successfully!");
                }
                else
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Failed to delete member! Response Code: {response.StatusCode}, Message: {responseContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"HttpRequestException: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"General Error: {ex.Message}");
            }
        }
    }
}
