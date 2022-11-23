using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library
{
    public partial class Visitors : Form
    {
        public Visitors()
        {
            InitializeComponent();
        }

        public string ConnectionString =
            "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\chavd\\Documents\\Projects\\VSProjects\\Library\\Library\\LibraryDB.mdf;Integrated Security=True";

        public SqlConnection Connection;
        public SqlCommand Command;

        private void Visitors_Load(object sender, EventArgs e)
        {
            //disable textBox1
            textBox1.Enabled = false;
            UpdateGrid();
            UpdateComboBox1();
            UpdateComboBox2();
        }

        private void UpdateComboBox2()
        {
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
            Command = new SqlCommand("SELECT * FROM Visitors", Connection);
            var reader = Command.ExecuteReader();
            while (reader.Read()) comboBox2.Items.Add(reader["Barcode"]);
            Connection.Close();
            //focus first
            comboBox2.SelectedIndex = 0;
        }

        private void UpdateComboBox1()
        {
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
            Command = new SqlCommand("SELECT * FROM Visitors", Connection);
            var reader = Command.ExecuteReader();
            while (reader.Read()) comboBox1.Items.Add(reader["Barcode"]);
            Connection.Close();
        }

        private void UpdateGrid()
        {
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
            Command = new SqlCommand("SELECT * FROM Visitors", Connection);
            var reader = Command.ExecuteReader();
            var table = new DataTable();
            table.Load(reader);
            dataGridView1.DataSource = table;
            Connection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //get values and insert into database
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
            Command = new SqlCommand("INSERT INTO Visitors (Barcode, Egn, Names) VALUES (@Barcode, @Egn, @Names)",
                Connection);
            Command.Parameters.AddWithValue("@Barcode", int.Parse(textBox1.Text));
            Command.Parameters.AddWithValue("@Egn", int.Parse(textBox2.Text));
            Command.Parameters.AddWithValue("@Names", textBox3.Text);
            Command.ExecuteNonQuery();
            Connection.Close();

            //update grid
            UpdateGrid();
            //clear combobox2
            comboBox2.Items.Clear();
            //update combobox2
            UpdateComboBox2();
            //clear combobox1
            comboBox1.Items.Clear();
            //update combobox1
            UpdateComboBox1();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // on button press generate a new barcode with Random
            var random = new Random();
            var barcode = random.Next(100000000, 999999999);
            textBox1.Text = barcode.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //on button press delete selected from combobox2
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
            Command = new SqlCommand("DELETE FROM Visitors WHERE Barcode = @Barcode", Connection);
            Command.Parameters.AddWithValue("@Barcode", int.Parse(comboBox2.Text));
            Command.ExecuteNonQuery();
            Connection.Close();

            //update grid
            UpdateGrid();
            //clear combobox2
            comboBox2.Items.Clear();
            //update combobox2
            UpdateComboBox2();
            //clear combobox1
            comboBox1.Items.Clear();
            //update combobox1
            UpdateComboBox1();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
            Command = new SqlCommand("UPDATE Visitors SET Egn = @Egn, Names = @Names WHERE Barcode = @Barcode",
                Connection);
            Command.Parameters.AddWithValue("@Barcode", int.Parse(comboBox1.Text));
            Command.Parameters.AddWithValue("@Egn", int.Parse(textBox5.Text));
            Command.Parameters.AddWithValue("@Names", textBox6.Text);
            Command.ExecuteNonQuery();
            Connection.Close();

            //update grid
            UpdateGrid();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
            Command = new SqlCommand("SELECT * FROM Visitors WHERE Barcode = @Barcode", Connection);
            Command.Parameters.AddWithValue("@Barcode", int.Parse(comboBox1.Text));
            var reader = Command.ExecuteReader();
            while (reader.Read())
            {
                textBox5.Text = reader["Egn"].ToString();
                textBox6.Text = reader["Names"].ToString();
            }

            Connection.Close();
        }
    }
}