using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bd_magaz
{
    public partial class Form2 : Form
    {
        string connString = null;

        public Form2(string connStr)
        {
            InitializeComponent();
            connString = connStr;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaleOfGoods form = new SaleOfGoods(connString);
            form.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Deliveries form = new Deliveries(connString);
            form.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Product form = new Product(connString);
            form.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Supplier form = new Supplier(connString);
            form.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Seller form = new Seller(connString);
            form.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ProductType form = new ProductType(connString);
            form.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            StorageCond form = new StorageCond(connString);
            form.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            CountryOfOrigin form = new CountryOfOrigin(connString);
            form.Show();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
