using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using MySql.Data.MySqlClient;

namespace Bd_magaz
{
    public partial class Form1 : Form
    {
        string connString = null;
        Thread thread;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            connString = $"server={textBox1.Text};port={textBox2.Text};username={textBox3.Text};password={textBox4.Text};database=magazin_ov";
            MySqlConnection connection = new MySqlConnection(connString);
            try
            {
                connection.Open();
                this.Close();
                thread = new Thread(open);
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start(); 
            }
            catch (Exception)
            {
                MessageBox.Show($"Не удалось подключиться к БД.\nПроверьте конфигурацию и повторите попытку", "Ошибка подключения", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }
        
        public void open(object obj)
        {
            Application.Run(new Form2(connString));
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
