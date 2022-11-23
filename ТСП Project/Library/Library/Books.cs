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
    public partial class Books : Form
    {
        public Books()
        {
            InitializeComponent();
        }

        public string ConnectionString =
            "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\chavd\\Documents\\Projects\\VSProjects\\Library\\Library\\LibraryDB.mdf;Integrated Security=True";

        public SqlConnection Connection;
        public SqlCommand Command;

        public List<string> Genres = new List<string>()
        {
            "Fiction",
            "Non-Fiction",
            "Fantasy",
            "Science Fiction",
            "Romance",
            "Mystery",
            "Horror",
            "Thriller"
        };

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateDataGrid();
            comboBox3.DataSource = Genres;
            comboBox4.DataSource = Genres;

            FillComboBox1();
            FillComboBox2();
        }

        private void FillComboBox1()
        {
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
            Command = new SqlCommand("SELECT * FROM Books", Connection);
            var reader = Command.ExecuteReader();
            while (reader.Read()) comboBox1.Items.Add(reader["InventoryNumber"]);

            Connection.Close();
        }

        private void FillComboBox2()
        {
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
            Command = new SqlCommand("SELECT * FROM Books", Connection);
            var reader = Command.ExecuteReader();
            while (reader.Read()) comboBox2.Items.Add(reader["InventoryNumber"]);

            Connection.Close();

            //foccus on the first item
            comboBox2.SelectedIndex = 0;
        }

        private void UpdateDataGrid()
        {
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
            Command = new SqlCommand("SELECT * FROM Books", Connection);
            var adapter = new SqlDataAdapter(Command);
            var table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
            Connection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //on button click, insert data into database
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
            Command = new SqlCommand(
                "INSERT INTO Books (InventoryNumber, Title, Author, ReleaseYear, Genre) VALUES (@inventoryNumber, @title, @author, @releaseYear, @genre)",
                Connection);
            Command.Parameters.AddWithValue("@inventoryNumber", int.Parse(textBox1.Text));
            Command.Parameters.AddWithValue("@title", textBox2.Text);
            Command.Parameters.AddWithValue("@author", textBox3.Text);
            Command.Parameters.AddWithValue("@releaseYear", textBox4.Text);
            Command.Parameters.AddWithValue("@genre", comboBox3.Text);
            Command.ExecuteNonQuery();
            Connection.Close();
            //clear textboxes
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();

            UpdateDataGrid();
            FillComboBox1();
            FillComboBox2();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //delete item from database by inventory number selected in combobox
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
            Command = new SqlCommand("DELETE FROM Books WHERE InventoryNumber = @inventoryNumber", Connection);
            Command.Parameters.AddWithValue("@inventoryNumber", int.Parse(comboBox2.Text));
            Command.ExecuteNonQuery();
            Connection.Close();

            //clear combobox and refill it
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();

            UpdateDataGrid();
            FillComboBox1();
            FillComboBox2();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //on combobox selection change, fill textboxes with data from database
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
            Command = new SqlCommand("SELECT * FROM Books WHERE InventoryNumber = @inventoryNumber", Connection);
            Command.Parameters.AddWithValue("@inventoryNumber", int.Parse(comboBox1.Text));
            var reader = Command.ExecuteReader();
            while (reader.Read())
            {
                textBox9.Text = reader["Title"].ToString();
                textBox8.Text = reader["Author"].ToString();
                textBox7.Text = reader["ReleaseYear"].ToString();
                comboBox4.Text = reader["Genre"].ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //on button click, update data in database
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
            Command = new SqlCommand(
                "UPDATE Books SET Title = @title, Author = @author, ReleaseYear = @releaseYear, Genre = @genre WHERE InventoryNumber = @inventoryNumber",
                Connection);
            Command.Parameters.AddWithValue("@inventoryNumber", int.Parse(comboBox1.Text));
            Command.Parameters.AddWithValue("@title", textBox9.Text);
            Command.Parameters.AddWithValue("@author", textBox8.Text);
            Command.Parameters.AddWithValue("@releaseYear", textBox7.Text);
            Command.Parameters.AddWithValue("@genre", comboBox4.Text);

            Command.ExecuteNonQuery();
            Connection.Close();

            UpdateDataGrid();
        }

        private void addVisitorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //on add visitor menu item click, open add visitor form
            var addVisitor = new Visitors();
            addVisitor.Show();
        }

        private void takeBookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var takeBook = new TakeBooks();
            takeBook.Show();
        }

        private void refrenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var reference = new Reference();
            reference.Show();
        }
    }
}