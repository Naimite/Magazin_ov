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
    public partial class Supplier : Form
    {
        string connString = null;
        DataTable table;
        int selectedID;
        string[] obj = new string[6];

        public Supplier(string connStr)
        {
            InitializeComponent();
            connString = connStr;
        }

        public DataTable loadInTable()
        {
            MySqlConnection connection = new MySqlConnection(connString);
            connection.Open();
            string sqlcmdString = "SELECT The_supplier.id_supplier AS 'ID', The_supplier.firm AS 'Фирма', The_supplier.telephone AS 'Телефон', The_supplier.surname AS 'Фамилия', The_supplier.name AS 'Имя', The_supplier.patronymic AS 'Отчество' FROM The_supplier ORDER BY The_supplier.firm";
            MySqlDataAdapter adapter = new MySqlDataAdapter(sqlcmdString, connection);

            table = new DataTable();
            table.Clear();
            adapter.Fill(table);
            connection.Close();

            for (int i = 0; i < 6; i++)
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
                    string sqlcmdString = $"SELECT The_supplier.id_supplier AS 'ID', The_supplier.firm AS 'Фирма', The_supplier.telephone AS 'Телефон', The_supplier.surname AS 'Фамилия', The_supplier.name AS 'Имя', The_supplier.patronymic AS 'Отчество' FROM The_supplier WHERE LOCATE('{query}', CONCAT_WS(' ', The_supplier.firm, The_supplier.telephone, The_supplier.surname, The_supplier.name, The_supplier.patronymic)) ORDER BY The_supplier.firm";
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
                    string sqlcmdString = $"SELECT * FROM The_supplier WHERE The_supplier.id_supplier = {selectedID}";
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
                            obj[5] = reader.GetString(5);
                        }
                    }
                    connection.Close();
                }

                for (int i = 0; i < 6; i++)
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
                for (int i = 0; i < 6; i++)
                {
                    obj[i] = this.Controls.Find($"textBox{i + 1}", true)[0].Text;
                    obj[i] = Regex.Replace(obj[i], @"\s+", " ");
                    obj[i] = obj[i].Trim();
                }

                MySqlConnection connection = new MySqlConnection(connString);
                connection.Open();
                string sqlcmdString = $"INSERT INTO The_supplier (id_supplier, firm, telephone, surname, name, patronymic) VALUES (NULL, '{obj[1]}', '{obj[2]}', '{obj[3]}', '{obj[4]}', '{obj[5]}')";
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
                for (int i = 0; i < 6; i++)
                {
                    obj[i] = this.Controls.Find($"textBox{i + 1}", true)[0].Text;
                    obj[i] = Regex.Replace(obj[i], @"\s+", " ");
                    obj[i] = obj[i].Trim();
                }

                MySqlConnection connection = new MySqlConnection(connString);
                connection.Open();
                string sqlcmdString = $"UPDATE The_supplier SET id_supplier = '{obj[0]}', firm = '{obj[1]}', telephone = '{obj[2]}', surname = '{obj[3]}', name = '{obj[4]}', patronymic = '{obj[5]}' WHERE The_supplier.id_supplier = {selectedID}";
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
                string sqlcmdString = $"DELETE FROM The_supplier WHERE The_supplier.id_supplier = {selectedID}";
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
    }
}
