using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace FormDesign
{
    public partial class Form2 : Form
    {
        MySqlConnection conn = ConnectionService.GetConnection();
        DataTable dataTable = new DataTable();
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            filldataTable();
        }

        public DataTable getDataSiswa()
        {
            dataTable.Reset();
            dataTable = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM data_ekskul", conn))
            {
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                dataTable.Load(reader);
            }
            return dataTable;

        }

        public void filldataTable()
        {
            dgv_data.DataSource = getDataSiswa();

            DataGridViewButtonColumn colEdit = new DataGridViewButtonColumn();
            colEdit.UseColumnTextForButtonValue = true;
            colEdit.Text = "Edit";
            colEdit.Name = "";
            dgv_data.Columns.Add(colEdit);

            DataGridViewButtonColumn colDelete = new DataGridViewButtonColumn();
            colDelete.UseColumnTextForButtonValue = true;
            colDelete.Text = "Delete";
            colDelete.Name = "";
            dgv_data.Columns.Add(colDelete);

            /*resetIncrement();*/
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            MySqlCommand cmd;

            try
            {
                cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE data_ekskul SET nama = @Nama, ekskul = @Ekskul, kelas = @Kelas WHERE nis = @NIS";
                cmd.Parameters.AddWithValue("@NIS", tb_nis.Text);
                cmd.Parameters.AddWithValue("@Nama", tb_nama.Text);
                cmd.Parameters.AddWithValue("@Ekskul", tb_ekskul.Text);
                cmd.Parameters.AddWithValue("@Kelas", tb_kelas.Text);
                cmd.ExecuteNonQuery();
                conn.Close();

                dgv_data.Columns.Clear();
                dataTable.Clear();
                filldataTable();
            }
            catch (Exception ex) { }
        }

        private void dgv_data_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4)
            {
                int id = Convert.ToInt32(dgv_data.CurrentCell.RowIndex.ToString());
                tb_nis.Text = dgv_data.Rows[id].Cells[0].Value.ToString();
                tb_nama.Text = dgv_data.Rows[id].Cells[1].Value.ToString();
                tb_ekskul.Text = dgv_data.Rows[id].Cells[2].Value.ToString();
                tb_kelas.Text = dgv_data.Rows[id].Cells[3].Value.ToString();

            }

            if (e.ColumnIndex == 5)
            {
                int id = Convert.ToInt32(dgv_data.CurrentCell.RowIndex.ToString());

                MySqlCommand cmd;

                try
                {
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "DELETE FROM data_ekskul WHERE nis = @NIS";
                    cmd.Parameters.AddWithValue("@NIS", dgv_data.Rows[id].Cells[0].Value.ToString());

                    cmd.ExecuteNonQuery();
                    conn.Close();

                    dgv_data.Columns.Clear();
                    dataTable.Clear();
                    filldataTable();
                }
                catch (Exception ex) { }

            }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            MySqlCommand cmd;

            try
            {
                cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO data_ekskul(nama,ekskul,kelas) VALUE(@Nama, @Ekskul, @Kelas)";
                cmd.Parameters.AddWithValue("@Nama", tb_nama.Text);
                cmd.Parameters.AddWithValue("@Ekskul", tb_ekskul.Text);
                cmd.Parameters.AddWithValue("@Kelas", tb_kelas.Text);
                cmd.ExecuteNonQuery();
                conn.Close();

                dgv_data.Columns.Clear();
                dataTable.Clear();
                filldataTable();
            }
            catch (Exception ex) { }
        }

        public void searchData (String ValueToFind)
        {
            string searchQuery = "SELECT * FROM data_ekskul WHERE CONCAT(nama, kelas, ekskul) LIKE '%" + ValueToFind + "%'";
            MySqlDataAdapter adapter = new MySqlDataAdapter(searchQuery, conn);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dgv_data.DataSource = table;
        }

        private void tb_search_TextChanged(object sender, EventArgs e)
        {
            searchData(tb_search.Text);
        }

        private void resetIncrement()
        {
            int id = 1;
            int maxInc = int.Parse(dataTable.Rows.Count.ToString());
            for (int i = 0; i < maxInc; i++)
            {
                string idinc = $"UPDATE data_ekskul SET id = '{id}' WHERE id = '{dgv_data.Rows[i].Cells[0].Value}'";
                MySqlCommand cmd = new MySqlCommand(idinc, conn);
                cmd.ExecuteNonQuery();
                id++;
            }
            maxIncrement();
            conn.Close();
            dgv_data.Columns.Clear();
            dataTable.Clear();
            dgv_data.DataSource = getDataSiswa();
            dgv_data.Columns[0].Width = 50;
        }

        private void maxIncrement()
        {
            int maxInc = int.Parse(dataTable.Rows.Count.ToString());
            string resetMax = $"ALTER TABLE data_ekskul AUTO_INCREMENT = {maxInc}";
            MySqlCommand cmd = new MySqlCommand(resetMax, conn);
            cmd.ExecuteNonQuery();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            tb_nis.Clear();
            tb_nama.Clear();
            tb_ekskul.Clear();
            tb_kelas.Clear();
        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.PrintPreviewControl.Zoom= 1;
            printPreviewDialog1.ShowDialog();
        }

        private void printDocumen1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Bitmap imagebmp = new Bitmap(dgv_data.Width, dgv_data.Height);
            dgv_data.DrawToBitmap(imagebmp, new Rectangle(0, 0, dgv_data.Width, dgv_data.Height));
            e.Graphics.DrawImage(imagebmp, 120, 20);
        }
    }
}
