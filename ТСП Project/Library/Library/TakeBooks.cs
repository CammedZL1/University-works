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
    public partial class TakeBooks : Form
    {
        public TakeBooks()
        {
            InitializeComponent();
        }

        public string ConnectionString =
            "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\chavd\\Documents\\Projects\\VSProjects\\Library\\Library\\LibraryDB.mdf;Integrated Security=True";

        public SqlConnection Connection;
        public SqlCommand Command;

        private void TakeBooks_Load(object sender, EventArgs e)
        {
            UpdateGrid();
            FillComboBox1();
            FillComboBox4();
            FillComboBox2();
            FillComboBox5();
            FillComboBox3();
        }

        private void FillComboBox3()
        {
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
            Command = new SqlCommand("SELECT * FROM Books", Connection);
            var reader = Command.ExecuteReader();
            while (reader.Read()) comboBox3.Items.Add(reader["InventoryNumber"].ToString());
            Connection.Close();
        }

        private void FillComboBox2()
        {
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
            Command = new SqlCommand("SELECT * FROM Books", Connection);
            var reader = Command.ExecuteReader();
            while (reader.Read()) comboBox2.Items.Add(reader["InventoryNumber"].ToString());
            Connection.Close();
        }

        private void FillComboBox4()
        {
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
            Command = new SqlCommand("SELECT * FROM Visitors", Connection);
            var reader = Command.ExecuteReader();
            while (reader.Read()) comboBox4.Items.Add(reader["Barcode"].ToString());
            Connection.Close();
        }

        private void FillComboBox5()
        {
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
            Command = new SqlCommand("SELECT * FROM Visitors", Connection);
            var reader = Command.ExecuteReader();
            while (reader.Read()) comboBox5.Items.Add(reader["Barcode"].ToString());
            Connection.Close();
        }

        private void FillComboBox1()
        {
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
            Command = new SqlCommand("SELECT * FROM Books", Connection);
            var reader = Command.ExecuteReader();
            while (reader.Read()) comboBox1.Items.Add(reader["InventoryNumber"].ToString());
            Connection.Close();
        }


        /*
    CREATE TABLE [dbo].[TakenBooks] (
    [BookId]     INT  NOT NULL,
    [VisitorId]  INT  NOT NULL,
    [TakenDate]  DATE NOT NULL,
    [ReturnDate] DATE NOT NULL,
    PRIMARY KEY CLUSTERED ([BookId] ASC)
        );*/

        private void UpdateGrid()
        {
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
            Command = new SqlCommand("SELECT * FROM TakenBooks", Connection);
            var adapter = new SqlDataAdapter(Command);
            var table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
            Connection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
            Command = new SqlCommand("INSERT INTO TakenBooks VALUES (@BookId, @VisitorId, @TakenDate, @ReturnDate)",
                Connection);
            Command.Parameters.AddWithValue("@BookId", comboBox1.Text);
            Command.Parameters.AddWithValue("@VisitorId", comboBox4.Text);
            Command.Parameters.AddWithValue("@TakenDate", textBox1.Text);
            Command.Parameters.AddWithValue("@ReturnDate", textBox2.Text);
            Command.ExecuteNonQuery();
            Connection.Close();

            UpdateGrid();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //get values from textboxes and comboboxes and update the database
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
            Command = new SqlCommand(
                "UPDATE TakenBooks SET BookId = @BookId, VisitorId = @VisitorId, TakenDate = @TakenDate, ReturnDate = @ReturnDate WHERE BookId = @BookId",
                Connection);
            Command.Parameters.AddWithValue("@BookId", int.Parse(comboBox2.Text));
            Command.Parameters.AddWithValue("@VisitorId", int.Parse(comboBox5.Text));
            Command.Parameters.AddWithValue("@TakenDate", textBox3.Text);
            Command.Parameters.AddWithValue("@ReturnDate", textBox4.Text);
            Command.ExecuteNonQuery();
            Connection.Close();

            UpdateGrid();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //get values from db and fill textboxes and combo boxes
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
            Command = new SqlCommand("SELECT * FROM TakenBooks WHERE BookId = @BookId", Connection);
            Command.Parameters.AddWithValue("@BookId", comboBox2.Text);
            var reader = Command.ExecuteReader();
            while (reader.Read())
            {
                comboBox5.Text = reader["VisitorId"].ToString();
                textBox3.Text = reader["TakenDate"].ToString();
                textBox4.Text = reader["ReturnDate"].ToString();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //delete record from database
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
            Command = new SqlCommand("DELETE FROM TakenBooks WHERE BookId = @BookId", Connection);
            Command.Parameters.AddWithValue("@BookId", int.Parse(comboBox3.Text));
            Command.ExecuteNonQuery();
            Connection.Close();

            //clear all comboboxes
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            comboBox4.Items.Clear();
            comboBox5.Items.Clear();

            UpdateGrid();
            FillComboBox1();
            FillComboBox4();
            FillComboBox2();
            FillComboBox5();
            FillComboBox3();
        }
    }
}