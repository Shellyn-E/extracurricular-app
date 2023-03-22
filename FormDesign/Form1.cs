using MySql.Data.MySqlClient;

namespace FormDesign
{
    public partial class Form1 : Form
    {
        MySqlConnection connection = ConnectionService.GetConnection();

        public Form1()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(login(textBox1.Text, textBox2.Text))
            {
                Form2 fm = new Form2();
                fm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Tidak Berhasil");
            }
        }

        private Boolean login(String username, String password)
        {
            string SQL = "SELECT username, password FROM loginn";
            connection.Open();
            MySqlCommand cmd =new MySqlCommand(SQL, connection);
            MySqlDataReader reader= cmd.ExecuteReader();
            while (reader.Read())
            {
                if((username == reader.GetString(0) && (password == reader.GetString(1))))
                {
                    connection.Close();
                    return true;
                }
            }
            connection.Close();
            return false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}