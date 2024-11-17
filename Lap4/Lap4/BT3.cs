using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Lap4.API;



namespace Lap4
{
    public partial class BT3 : Form
    {
        public BT3()
        {
            InitializeComponent();
        }

       

        private static HttpClient client = new HttpClient();
        private async Task<List<Photo>> GetPhotosAsync()
        {
            var response = await client.GetStringAsync("https://jsonplaceholder.typicode.com/photos");
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var photos = JsonSerializer.Deserialize<List<Photo>>(response,options);
            return photos.GetRange(0, 100); // Lấy 100 ảnh đầu tiên
        }

        private async Task<List<Comment>> GetCommentsAsync()
        {
            var response = await client.GetStringAsync("https://jsonplaceholder.typicode.com/comments");
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var comments = JsonSerializer.Deserialize<List<Comment>>(response,options);
            return comments.GetRange(0, 100); // Lấy 100 bình luận đầu tiên
        }

        private async Task<List<User>> GetUsersAsync()
        {
            // link to URL
            var response = await client.GetStringAsync("https://jsonplaceholder.typicode.com/users");

            // serialize : gom manh
            // deserialize : phan manh 

            // option để khi map gán dữ liệu sẽ tìm đúng atribute mà không phải xét in hoa hay in thường 
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var users = JsonSerializer.Deserialize<List<User>>(response, options);
            try
            {
                return users.GetRange(0, Convert.ToInt32(comboBox1.Text));
            }
            catch { MessageBox.Show("There only under 10 Person");
                return null;
            }
        }

        

        private async void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                int temp = Int32.Parse(comboBox1.Text);
            }
            catch { MessageBox.Show("Please enter number");
                return;
            }
            if ( string.IsNullOrWhiteSpace(comboBox1.Text) && (comboBox1.SelectedIndex == -1 )|| comboBox2.SelectedIndex == -1 ) {
                MessageBox.Show("Pleas ensure there no empty blank");
                return;
            }

            var us = await GetUsersAsync();
            if (us != null)
            {
                
                if (comboBox2.SelectedIndex == 1)
                {
                    
                    var case1 = us.Select(User => $"Name : {User.Name} and Email: {User.Email}").ToList();
                    ls.DataSource = case1;
                    columnHeader1.Width = 200;
                    listView1.Items.Clear();
                    foreach (var user in us)
                    {
                        ListViewItem item = new ListViewItem(user.Name);
                        item.SubItems.Add(user.Email);
                        listView1.Items.Add(item);
                    }

                }
                else
                {
                    columnHeader1.Width = 900;
                    ls.DataSource = us;
                    ls.DisplayMember = "Name";

                    listView1.Items.Clear();
                    foreach (var user in us)
                    {
                        ListViewItem item = new ListViewItem(user.Name);
                        listView1.Items.Add(item);
                    }
                }

            }
                
                
                

            }

        private void button2_Click(object sender, EventArgs e)
        {
            BT3_ add_form = new BT3_();
            add_form.Show();
        }
        /*
         * //ls.DataSource = us;
        //ls.DisplayMember = "email"; 
        foreach (var user in us)
        {
            ls.Items.Add($"{user.Name} ({user.Username}) - {user.Email}");
        }
        */
    }
}

