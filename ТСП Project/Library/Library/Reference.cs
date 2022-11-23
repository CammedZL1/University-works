using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Library
{
    public partial class Reference : Form
    {
        public Reference()
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

        private void button1_Click(object sender, EventArgs e)
        {
            //fill data grid view with all visitors with return date and book title
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
            Command = new SqlCommand("SELECT * FROM Visitors", Connection);
            SqlDataAdapter adapter = new SqlDataAdapter(Command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
            Connection.Close();

        }

        private void Reference_Load(object sender, EventArgs e)
        {
            //fill combobox 1 with all genres
            comboBox1.DataSource = Genres;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
            Command = new SqlCommand("SELECT * FROM Books WHERE Genre = @genre AND Author = @author AND ReleaseYear = @releaseyear", Connection);
            Command.Parameters.AddWithValue("@genre", comboBox1.Text);
            Command.Parameters.AddWithValue("@author", textBox1.Text);
            Command.Parameters.AddWithValue("@releaseyear", textBox2.Text);
            SqlDataAdapter adapter = new SqlDataAdapter(Command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
            Connection.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //message box with age distributuon of all visitors determined by EGN (first 2 digits) in 5 different age columns (0-10, 18, 30, 65, 65+)
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
            Command = new SqlCommand("SELECT * FROM Visitors", Connection);
            var reader = Command.ExecuteReader();
            int[] ageDistribution = new int[5];
            while (reader.Read())
            {
                int age = DateTime.Now.Year - (1900 + int.Parse(reader["EGN"].ToString().Substring(0, 2)));
                if (age < 10) ageDistribution[0]++;
                else if (age < 18) ageDistribution[1]++;
                else if (age < 30) ageDistribution[2]++;
                else if (age < 65) ageDistribution[3]++;
                else ageDistribution[4]++;
            }

            Connection.Close();

            MessageBox.Show("Age distribution: \n0-10: " + ageDistribution[0] + "\n11-18: " + ageDistribution[1] + "\n19-30: " + ageDistribution[2] + "\n31-65: " + ageDistribution[3] + "\n65+: " + ageDistribution[4]);
            
            
        }
    }
}
