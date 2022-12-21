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
    public partial class StorageCond : Form
    {
        string connString = null;
        DataTable table;
        int selectedID;
        string[] obj = new string[2];

        public StorageCond(string connStr)
        {
            InitializeComponent();
            connString = connStr;
        }

        public DataTable loadInTable()
        {
            MySqlConnection connection = new MySqlConnection(connString);
            connection.Open();
            string sqlcmdString = "SELECT storage_conditions.id_storage_conditions AS 'ID', storage_conditions.condition AS 'Условия' FROM storage_conditions ORDER BY storage_conditions.id_storage_conditions";
            MySqlDataAdapter adapter = new MySqlDataAdapter(sqlcmdString, connection);

            table = new DataTable();
            table.Clear();
            adapter.Fill(table);
            connection.Close();

            for (int i = 0; i < 2; i++)
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
                    string sqlcmdString = $"SELECT storage_conditions.id_storage_conditions AS 'ID', storage_conditions.condition AS 'Условия' FROM storage_conditions WHERE LOCATE('{query}', storage_conditions.condition) ORDER BY storage_conditions.id_storage_conditions";
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
                    string sqlcmdString = $"SELECT * FROM storage_conditions WHERE storage_conditions.id_storage_conditions = {selectedID}";
                    MySqlCommand sqlcmd = new MySqlCommand(sqlcmdString, connection);
                    using (MySqlDataReader reader = sqlcmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            obj[0] = reader.GetInt32(0).ToString();
                            obj[1] = reader.GetString(1);
                        }
                        connection.Close();
                    }
                }

                for (int i = 0; i < 2; i++)
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
                for (int i = 0; i < 2; i++)
                {
                    obj[i] = this.Controls.Find($"textBox{i + 1}", true)[0].Text;
                    obj[i] = Regex.Replace(obj[i], @"\s+", " ");
                    obj[i] = obj[i].Trim();
                }

                MySqlConnection connection = new MySqlConnection(connString);
                connection.Open();
                string sqlcmdString = $"INSERT INTO storage_conditions (id_storage_conditions, storage_conditions.condition) VALUES (NULL, '{obj[1]}')";
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
                for (int i = 0; i < 2; i++)
                {
                    obj[i] = this.Controls.Find($"textBox{i + 1}", true)[0].Text;
                    obj[i] = Regex.Replace(obj[i], @"\s+", " ");
                    obj[i] = obj[i].Trim();
                }

                MySqlConnection connection = new MySqlConnection(connString);
                connection.Open();
                string sqlcmdString = $"UPDATE storage_conditions SET id_storage_conditions = '{obj[0]}', storage_conditions.condition = '{obj[1]}' WHERE storage_conditions.id_storage_conditions = {selectedID}";
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
                string sqlcmdString = $"DELETE FROM storage_conditions WHERE storage_conditions.id_storage_conditions = {selectedID}";
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
