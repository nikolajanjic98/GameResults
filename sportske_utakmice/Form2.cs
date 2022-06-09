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
    public partial class Form2 : Form
    {
        Form parrent;
        string korisnicko_ime, ime, prezime;
        public Form2(Form parrent)
        {
            InitializeComponent();
            this.parrent = parrent;
        }

        public void checkInputs(String korisnicko_ime, String ime, String prezime)
        {
            if (korisnicko_ime.Trim().Equals("") || ime.Trim().Equals("") || prezime.Trim().Equals("")) throw new AllFieldsRequired("popunite sva polja");
        }


        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            this.parrent.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MySqlConnection con = new MySqlConnection("server = localhost; database = sportske_utakmice; username = root; password =;SSL Mode = None ");
            korisnicko_ime = boxKorisnicko.Text;
            ime = boxIme.Text;
            prezime = boxPrezime.Text;

            try
            {
                checkInputs(korisnicko_ime, ime, prezime);
                con.Open();
                MySqlTransaction transaction = con.BeginTransaction();
                String query = "SELECT * FROM korisnik WHERE korisnicko_ime ='" + korisnicko_ime + "'";
                MySqlCommand com1 = new MySqlCommand(query, con);
                MySqlDataReader dr = com1.ExecuteReader();
                if (dr.HasRows) MessageBox.Show("Korisnik već postoji!");
                else
                {
                    dr.Close();
                    String ins = "INSERT INTO korisnik (korisnicko_ime, ime, prezime) VALUES ('" + korisnicko_ime + "', '" + ime + "', '" + prezime + "')";
                    MySqlCommand com2 = new MySqlCommand();
                    com2.Connection = con;
                    com2.Transaction = transaction;
                    com2.CommandText = ins;
                    com2.ExecuteNonQuery();
                    transaction.Commit();
                    this.parrent.Show();
                    this.Close();
                }
            }
            catch (AllFieldsRequired exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally { con.Close(); }
        }

        public class AllFieldsRequired : Exception
        {
            public AllFieldsRequired(String error) : base(error) { }
        }
    }
}
