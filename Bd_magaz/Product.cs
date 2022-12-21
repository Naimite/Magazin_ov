using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace Bd_magaz
{
    public partial class Product : Form
    {
        string connString = null;
        DataTable table;
        int selectedID;
        string[] obj = new string[5];

        public Product(string connStr)
        {
            InitializeComponent();
            connString = connStr;
        }

        public DataTable loadInTable()
        {
            MySqlConnection connection = new MySqlConnection(connString);
            connection.Open();
            string sqlcmdString = "CALL Product_read()";
            MySqlDataAdapter adapter = new MySqlDataAdapter(sqlcmdString, connection);

            table = new DataTable();
            table.Clear();
            adapter.Fill(table);
            connection.Close();

            for (int i = 0; i < 5; i++)
            {
                this.Controls.Find($"textBox{i + 1}", true)[0].Text = null;
            }

            return table;
        }

        private void loadBTN_Click(object sender, EventArgs e)
        {
            loadBTN.Text = "Загрузить";

            try
            {
                dataGridView1.DataSource = loadInTable();
            }
            catch (MySqlException)
            {
                MessageBox.Show($"Не удалось обработать запрос", "Ошибка загрузки таблицы", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void searchTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                loadBTN.Text = "Сбросить";

                try
                {
                    string query = searchTextBox.Text;
                    query = Regex.Replace(query, @"\s+", " ");
                    query = query.Trim();

                    MySqlConnection connection = new MySqlConnection(connString);
                    connection.Open();
                    string sqlcmdString = $"CALL Product_search('{query}')";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(sqlcmdString, connection);

                    table = new DataTable();
                    table.Clear();
                    adapter.Fill(table);
                    connection.Close();
                }
                catch (MySqlException)
                {
                    MessageBox.Show($"Не удалось обработать запрос", "Ошибка загрузки таблицы", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                dataGridView1.DataSource = table;
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                int selectedRowCount = dataGridView1.Rows.GetRowCount(DataGridViewElementStates.Selected);

                if (selectedRowCount > 0)
                {
                    selectedID = Convert.ToInt32(dataGridView1.Rows[dataGridView1.SelectedRows[0].Index].Cells[0].Value);
                    MySqlConnection connection = new MySqlConnection(connString);
                    connection.Open();
                    string sqlcmdString = $"CALL Product_get_by_id({selectedID})";
                    MySqlCommand sqlcmd = new MySqlCommand(sqlcmdString, connection);
                    using (MySqlDataReader reader = sqlcmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            obj[0] = reader.GetInt32(0).ToString();
                            obj[1] = reader.GetString(1);
                            obj[2] = reader.GetString(2);
                            obj[3] = reader.GetString(3);
                            obj[4] = reader.GetString(4);
                        }
                    }
                    connection.Close();
                }

                for (int i = 0; i < 5; i++)
                {
                    this.Controls.Find($"textBox{i + 1}", true)[0].Text = obj[i];
                }

            }
            catch (MySqlException)
            {
                MessageBox.Show($"Не удалось обработать запрос", "Ошибка выборки строки", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void createBTN_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < 5; i++)
                {
                    obj[i] = this.Controls.Find($"textBox{i + 1}", true)[0].Text;
                    obj[i] = Regex.Replace(obj[i], @"\s+", " ");
                    obj[i] = obj[i].Trim();
                }

                MySqlConnection connection = new MySqlConnection(connString);
                connection.Open();
                string sqlcmdString = $"CALL Product_create('{obj[1]}', '{obj[2]}', '{obj[3]}', '{obj[4]}');";
                MySqlCommand sqlcmd = new MySqlCommand(sqlcmdString, connection);
                sqlcmd.ExecuteNonQuery();
                connection.Close();

                MessageBox.Show("Запись добвлена", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);

                dataGridView1.DataSource = loadInTable();
            }
            catch (MySqlException)
            {
                MessageBox.Show($"Не удалось обработать запрос", "Ошибка добавления", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chBTN_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < 5; i++)
                {
                    obj[i] = this.Controls.Find($"textBox{i + 1}", true)[0].Text;
                    obj[i] = Regex.Replace(obj[i], @"\s+", " ");
                    obj[i] = obj[i].Trim();
                }

                MySqlConnection connection = new MySqlConnection(connString);
                connection.Open();
                string sqlcmdString = $"CALL Product_update('{selectedID}', '{obj[0]}', '{obj[1]}', '{obj[2]}', '{obj[3]}', '{obj[4]}');";
                MySqlCommand sqlcmd = new MySqlCommand(sqlcmdString, connection);
                sqlcmd.ExecuteNonQuery();
                connection.Close();

                dataGridView1.DataSource = loadInTable();
                MessageBox.Show("Запись изменена", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (MySqlException)
            {
                MessageBox.Show($"Не удалось обработать запрос", "Ошибка изменения", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void delBTN_Click(object sender, EventArgs e)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(connString);
                connection.Open();
                string sqlcmdString = $"DELETE FROM Product WHERE Product.id_product = {selectedID}";
                MySqlCommand sqlcmd = new MySqlCommand(sqlcmdString, connection);
                sqlcmd.ExecuteNonQuery();
                connection.Close();

                dataGridView1.DataSource = loadInTable();

                MessageBox.Show($"Запись No.{selectedID} удалена", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (MySqlException)
            {
                MessageBox.Show($"Не удалось обработать запрос", "Ошибка удаления", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void typeBTN_Click(object sender, EventArgs e)
        {
            ProductType form = new ProductType(connString);
            form.Show();
        }

        private void condBTN_Click(object sender, EventArgs e)
        {
            StorageCond form = new StorageCond(connString);
            form.Show();
        }

        private void originBTN_Click(object sender, EventArgs e)
        {
            CountryOfOrigin form = new CountryOfOrigin(connString);
            form.Show();
        }
    }
}
