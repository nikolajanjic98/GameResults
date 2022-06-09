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

namespace sportske_utakmice
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string korisnicko_ime;

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Form su = new Form2(this);
            su.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MySqlConnection con = new MySqlConnection("server = localhost; database = sportske_utakmice; username = root; password =; SSL Mode = None");
            korisnicko_ime = boxKorisnicko.Text;

            try
            {
                con.Open();
                String query = "SELECT * FROM korisnik WHERE korisnicko_ime='" + korisnicko_ime + "'";
                MySqlCommand com = new MySqlCommand(query, con);
                MySqlDataReader dr = com.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    if (dr[0].ToString().Equals(korisnicko_ime))
                    {
                        string ime = dr[1].ToString();
                        string prezime = dr[2].ToString();
                        Form forma = new Main(korisnicko_ime, ime, prezime);
                        forma.Show();
                        this.Hide();
                    }
                    else MessageBox.Show("Nevažeće korisničko ime!");
                }
                else MessageBox.Show("Nevažeće korisničko ime!");
            }
            catch (Exception exception) { MessageBox.Show(exception.Message); }
            finally { con.Close(); }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
