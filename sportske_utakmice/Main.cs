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
    public partial class Main : Form
    {
        string ime, prezime, korisnicko_ime;
        int keyU,keyT, keyI; //za ID utakmice, timova, igraca

        public Main(string korisnicko_ime, string ime, string prezime)
        {
            InitializeComponent();
            this.ime = ime;
            this.prezime = prezime;
            this.korisnicko_ime = korisnicko_ime;
            ImeAdminaLbl.Text = ime + " " + prezime;
//tab - profil
            utakmiceProfilTabela.DataSource = loadU(); //za ucitavanje tabele
            utakmiceProfilTabela.AutoResizeColumns(); //da podesi lepo sirinu kolona
//tab - utakmice
            utakmiceTabela.DataSource = loadU(); 
            utakmiceTabela.AutoResizeColumns(); 
//TAB - tim
            timTabela.DataSource = loadT(); 
            timTabela.AutoResizeColumns(); 
//tab - igraci
            igraciTabela.DataSource = loadI(); 
            igraciTabela.AutoResizeColumns(); 
        }
        MySqlConnection con = new MySqlConnection("server = localhost; database = sportske_utakmice; username = root; password =; SSL Mode = None");



//----------------------------------------------------------------------------- TAB PROFIL

        //tabela
        private BindingSource loadU()
        {
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand cmdd;
            DataSet ds = new DataSet();
            BindingSource bs = new BindingSource();
            string sql = "SELECT id_utakmice as ID, naziv as sport,l.id_lige, naziv_lige as liga,datum, id_tima1, t1.naziv_tima as TIM1, rezultat_tima1 as 'rez1' , rezultat_tima2 as 'rez2 ' , t2.naziv_tima as TIM2, id_tima2, mesto from utakmica as u inner join liga as l on u.id_lige=l.id_lige inner join sport as s on l.id_sporta=s.id_sporta inner join (select naziv_tima, id_tima from tim) t1 on t1.id_tima=id_tima1 inner join (select naziv_tima, id_tima from tim) t2 on t2.id_tima=id_tima2";


            //za pretragu (ukoliko je nepopunjeno svakako stampa sve utakmice)
            if (!(boxTim.Text.Trim().Equals(""))) //naziv tima
            {
                sql += " AND (t1.naziv_tima LIKE '%" + boxTim.Text.Trim() + "%' OR t2.naziv_tima LIKE '%" + boxTim.Text.Trim()+"%')" ;
            }

            if (!(boxDatum.Text.Trim().Equals(""))) //datum
            {
                sql += " AND datum='" + boxDatum.Text.Trim() + "'";
            }
            if (comboBoxSport.SelectedItem != null)
            {
                sql += " AND naziv='" + comboBoxSport.SelectedItem.ToString() + "'";
            }
            if (!(boxMesto.Text.Trim().Equals(""))) //mesto
            {
                sql += " AND mesto='" + boxMesto.Text.Trim() + "'";
            }




            cmdd = new MySqlCommand(sql, con);

            adapter.SelectCommand = cmdd;
            adapter.Fill(ds);

            bs.DataSource = ds.Tables[0];
            return bs;

        }

        private void odjaviSe_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
//DUGME PRETRAZI 
        private void button16_Click(object sender, EventArgs e)
        {
            utakmiceProfilTabela.DataSource = loadU();
        }



//----------------------------------------------------------------------------- TAB UTAKMICA


//DUGME DODAJ --- >  tab utakmica
        private void dodajU_Click(object sender, EventArgs e)
        {
            if (ProveriPoljaU() == true)
            {
                MessageBox.Show("Nisu uneti svi podaci!", "Dodavanje utakmice neuspešno!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand();
                MySqlTransaction transaction;
                transaction = con.BeginTransaction();
                cmd.Connection = con;
                cmd.Transaction = transaction;

                string datum = boxDatumU.Text;
                string tim1 = boxTim1U.Text;
                string mesto = boxMestoU.Text;
                string tim2 = boxTim2U.Text;
                string rez1 = boxRez1U.Text;
                string rez2 = boxRez2U.Text;
                string liga =boxIdLige.Text;

                try
                {
                    String r = "SELECT * from utakmica";
                    MySqlCommand com = new MySqlCommand(r, con);
                    MySqlDataReader dr = com.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        dr.Close();
                        cmd.CommandText = "INSERT INTO utakmica ( datum, id_tima1, rezultat_tima1, id_tima2, rezultat_tima2, mesto, id_lige) values('" + datum + "' ,'" + tim1+ "' ,'" + rez1 + "' ,'" + tim2+ "' ,'" + rez2 + "' ,'" + mesto + "' ,'"+liga+ "')";
                        cmd.ExecuteNonQuery();
                        transaction.Commit();
                        MessageBox.Show("Izvršeno!");
                        ResetUtakmice();
                        utakmiceTabela.DataSource = loadU();
                    }
                    else MessageBox.Show("Nevažeća utakmica!");
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    MessageBox.Show(exception.Message);
                }
                finally { con.Close(); }
            }
        }

 //POPUNJAVANJE SVIH POLJA --- provera da li su sva polja popunjena
        bool ProveriPoljaU()
        {
            if (boxDatumU.Text == "" || boxTim1U.Text == "" || boxMestoU.Text==""
                || boxTim2U.Text == "" || boxRez1U.Text == "" || boxRez2U.Text=="" || boxIdLige.Text=="")
            {
                return true;
            }
            else { return false; }
        }

// funkcija koja prazni polja na utakmice panelu
        private void ResetUtakmice()
        {
            keyU = 0;
            boxDatumU.Text = "";
            boxTim1U.Text = "";
            boxMestoU.Text = "";
            boxTim2U.Text = "";
            boxRez1U.Text = "";
            boxRez2U.Text = "";
            boxIdLige.Text ="";
        }

// fja koja omugaca da duplim klikom na neko polje iz tabele, popunimo textBoxove
        private void utakmiceTabela_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            boxDatumU.Text = utakmiceTabela.SelectedRows[0].Cells[4].Value.ToString();
            boxTim1U.Text = utakmiceTabela.SelectedRows[0].Cells[5].Value.ToString();
            boxMestoU.Text = utakmiceTabela.SelectedRows[0].Cells[11].Value.ToString();
            boxTim2U.Text = utakmiceTabela.SelectedRows[0].Cells[10].Value.ToString();
            boxRez1U.Text = utakmiceTabela.SelectedRows[0].Cells[7].Value.ToString();
            boxRez2U.Text = utakmiceTabela.SelectedRows[0].Cells[8].Value.ToString();
            boxIdLige.Text = utakmiceTabela.SelectedRows[0].Cells[2].Value.ToString();

            if (boxDatumU.Text == "") {keyU = 0;}
            else { keyU = Convert.ToInt32(utakmiceTabela.SelectedRows[0].Cells[0].Value.ToString());}
        }

        private void otkaziU_Click(object sender, EventArgs e)
        {
            ResetUtakmice();
        }
//DUGME IZMENI
        private void izmeniU_Click(object sender, EventArgs e)
        {
            if (ProveriPoljaU() == true)
            {
                MessageBox.Show("Nisu uneti svi podaci!", "Izmena utakmice neuspešna!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand();
                MySqlTransaction transaction;
                transaction = con.BeginTransaction();
                cmd.Connection = con;
                cmd.Transaction = transaction;

                string datum = boxDatumU.Text;
                string tim1 = boxTim1U.Text;
                string mesto = boxMestoU.Text;
                string tim2 = boxTim2U.Text;
                string rez1 = boxRez1U.Text;
                string rez2 = boxRez2U.Text;
                string liga = boxIdLige.Text;

                try
                {
                    String r = "SELECT * from utakmica";
                    MySqlCommand com = new MySqlCommand(r, con);
                    MySqlDataReader dr = com.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        dr.Close();
                        cmd.CommandText = "UPDATE utakmica set datum='" + datum + "', id_tima1='"+tim1 +"',rezultat_tima1='" + rez1 + "', id_tima2='" + tim2+ "', rezultat_tima2='" + rez2 + "', mesto='" + mesto + "', id_lige='" + liga+ "' where id_utakmice=" + keyU + "";
                        cmd.ExecuteNonQuery();
                        transaction.Commit();
                        MessageBox.Show("Izvršeno!");
                        utakmiceTabela.DataSource = loadU();
                    }
                    else MessageBox.Show("Nevažeća utakmica!");
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    MessageBox.Show(exception.Message);
                }
                finally { con.Close(); }
            }
        }

        private void obrisiU_Click(object sender, EventArgs e)
        {
            if (ProveriPoljaU() == true)
            {
                MessageBox.Show("Nisu uneti svi podaci!", "Brisanje utakmice neuspešno!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand();
                MySqlTransaction transaction;
                transaction = con.BeginTransaction();
                cmd.Connection = con;
                cmd.Transaction = transaction;

                try
                {
                    String r = "SELECT * from utakmica";
                    MySqlCommand com = new MySqlCommand(r, con);
                    MySqlDataReader dr = com.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        dr.Close();
                        cmd.CommandText = "DELETE from utakmica where id_utakmice=" + keyU + "";
                        cmd.ExecuteNonQuery();
                        transaction.Commit();
                        MessageBox.Show("Izvršeno!");
                        utakmiceTabela.DataSource = loadU();
                    }
                    else MessageBox.Show("Nevažeća utakmica!");
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    MessageBox.Show(exception.Message);
                }
                finally { con.Close(); }
            }
        }

        private void ImeAdminaLbl_Click(object sender, EventArgs e)
        {

        }


        private void Main_Load(object sender, EventArgs e)
        {

        }




//---------------------------------------------------------------------------TAB TIM

        private BindingSource loadT()
        {
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand cmdd;
            DataSet ds = new DataSet();
            BindingSource bs = new BindingSource();
            string sql = "SELECT id_tima as ID, naziv_tima as naziv, godina_osnivanja, drzava, trener from tim where 1=1";


            //za pretragu (ukoliko je nepopunjeno svakako stampa sve timove)
            if (!(boxNazivT.Text.Trim().Equals(""))) //naziv tima
            {
                sql += " AND naziv_tima LIKE'%" + boxNazivT.Text.Trim() + "%'";
            }

            if (!(boxGodT.Text.Trim().Equals(""))) //godina osnivanja
            {
                sql += " AND godina_osnivanja='" + boxGodT.Text.Trim() + "'";
            }

            if (!(boxTrener.Text.Trim().Equals(""))) //trener
            {
                sql += " AND trener LIKE'%" + boxTrener.Text.Trim() + "%'";
            }

            if (!(boxDrzava.Text.Trim().Equals(""))) //drzava
            {
                sql += " AND drzava LIKE '%" + boxDrzava.Text.Trim() + "%'";
            }

            cmdd = new MySqlCommand(sql, con);

            adapter.SelectCommand = cmdd;
            adapter.Fill(ds);

            bs.DataSource = ds.Tables[0];
            return bs;

        }

        private void pretraziTim_Click(object sender, EventArgs e)
        {
            timTabela.DataSource = loadT();
        }


        // funkcija koja prazni polja na tim panelu
        private void ResetTim()
        {
            keyT = 0;
            boxNazivT.Text = "";
            boxGodT.Text = "";
            boxTrener.Text = "";
            boxDrzava.Text = "";
        }

//dugme otkazi
        private void button12_Click(object sender, EventArgs e)
        {
            ResetTim();
        }

        private void izmeniTim_Click(object sender, EventArgs e)
        {
            if (ProveriPoljaT() == true)
            {
                MessageBox.Show("Nisu uneti svi podaci!", "Izmena tima neuspešna!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand();
                MySqlTransaction transaction;
                transaction = con.BeginTransaction();
                cmd.Connection = con;
                cmd.Transaction = transaction;

                string naziv = boxNazivT.Text;
                string godina = boxGodT.Text;
                string trener = boxTrener.Text;
                string drzava = boxDrzava.Text;

                try
                {
                    String r = "SELECT * from tim";
                    MySqlCommand com = new MySqlCommand(r, con);
                    MySqlDataReader dr = com.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        dr.Close();
                        cmd.CommandText = "UPDATE tim set naziv_tima='" + naziv + "', godina_osnivanja='" + godina + "', trener='" + trener + "', drzava='" + drzava + "' where id_tima=" + keyT + "";
                        cmd.ExecuteNonQuery();
                        transaction.Commit();
                        MessageBox.Show("Izvršeno!");
                        timTabela.DataSource = loadT();
                    }
                    else MessageBox.Show("Nevažeći tim!");
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    MessageBox.Show(exception.Message);
                }
                finally { con.Close(); }
            }
        }

        private void obrisiTim_Click(object sender, EventArgs e)
        {
            if (ProveriPoljaT() == true)
            {
                MessageBox.Show("Nisu uneti svi podaci!", "Brisanje tima neuspešno!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand();
                MySqlTransaction transaction;
                transaction = con.BeginTransaction();
                cmd.Connection = con;
                cmd.Transaction = transaction;

                try
                {
                    String r = "SELECT * from tim";
                    MySqlCommand com = new MySqlCommand(r, con);
                    MySqlDataReader dr = com.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        dr.Close();
                        cmd.CommandText = " DELETE from tim where id_tima=" + keyT + "";
                        cmd.ExecuteNonQuery();
                        transaction.Commit();
                        MessageBox.Show("Izvršeno!");
                        timTabela.DataSource = loadT();
                    }
                    else MessageBox.Show("Nevažeći tim!");
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    MessageBox.Show(exception.Message);
                }
                finally { con.Close(); }
            }
         }


        //POPUNJAVANJE SVIH POLJA --- provera da li su sva polja popunjena
        bool ProveriPoljaT()
        {
            if (boxNazivT.Text == "" || boxGodT.Text == "" || boxTrener.Text == "" || boxDrzava.Text == "")
            {
                return true;
            }
            else { return false; }
        }

        private void dodajTim_Click(object sender, EventArgs e)
        {
            if (ProveriPoljaT() == true)
            {
                MessageBox.Show("Nisu uneti svi podaci!", "Dodavanje tima neuspešno!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand();
                MySqlTransaction transaction;
                transaction = con.BeginTransaction();
                cmd.Connection = con;
                cmd.Transaction = transaction;

                string naziv = boxNazivT.Text;
                string godina = boxGodT.Text;
                string trener = boxTrener.Text;
                string drzava = boxDrzava.Text;

                try
                {
                    String r = "SELECT * from tim";
                    MySqlCommand com = new MySqlCommand(r, con);
                    MySqlDataReader dr = com.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        dr.Close();
                        cmd.CommandText = "INSERT INTO tim ( naziv_tima, godina_osnivanja, trener, drzava) values('" + naziv + "' ,'" + godina + "' ,'" + trener + "' ,'" + drzava + "')";
                        cmd.ExecuteNonQuery();
                        transaction.Commit();
                        MessageBox.Show("Izvršeno!");
                        
                        timTabela.DataSource = loadT();
                    }
                    else MessageBox.Show("Nevažeći tim!");
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    MessageBox.Show(exception.Message);
                }
                finally { con.Close(); }
            }
        }

        // fja koja omugaca da duplim klikom na neko polje iz tabele, popunimo textBoxove
        private void timTabela_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            boxNazivT.Text = timTabela.SelectedRows[0].Cells[1].Value.ToString();
            boxGodT.Text = timTabela.SelectedRows[0].Cells[2].Value.ToString();
            boxDrzava.Text = timTabela.SelectedRows[0].Cells[3].Value.ToString();
            boxTrener.Text = timTabela.SelectedRows[0].Cells[4].Value.ToString();

            if (boxGodT.Text == "") { keyT = 0; }
            else { keyT = Convert.ToInt32(timTabela.SelectedRows[0].Cells[0].Value.ToString()); }
        }


        //------------------------------------------------------------------------------- TAB IGRAC

        private BindingSource loadI()
        {
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand cmdd;
            DataSet ds = new DataSet();
            BindingSource bs = new BindingSource();
            string sql = "SELECT id_igraca as ID, ime, prezime, godina_rodjenja, t.id_tima, naziv_tima, pozicija, broj_dresa from igraci inner join tim t on igraci.id_tima=t.id_tima where 1=1";


            //za pretragu (ukoliko je nepopunjeno svakako stampa sve timove)
            if (!(boxImeIgraca.Text.Trim().Equals(""))) //ime
            {
                sql += " AND ime LIKE'%" + boxImeIgraca.Text.Trim() + "%'";
            }

            if (!(boxPrezimeIgraca.Text.Trim().Equals(""))) //prezime
            {
                sql += " AND prezime LIKE '%" + boxPrezimeIgraca.Text.Trim() + "%'";
            }

            if (!(boxGRIgraca.Text.Trim().Equals(""))) //godina rodjenja
            {
                sql += " AND godina_rodjenja='" + boxGRIgraca.Text.Trim() + "'";
            }

            if (!(boxDres.Text.Trim().Equals(""))) //dres
            {
                sql += " AND broj_dresa LIKE '%" + boxDres.Text.Trim() + "%'";
            }

            if (!(boxPozicija.Text.Trim().Equals(""))) //pozicija
            {
                sql += " AND pozicija LIKE '%" + boxPozicija.Text.Trim() + "%'";
            }

            if (!(boxTima.Text.Trim().Equals(""))) //tim
            {
                sql += " AND t.id_tima LIKE '%" + boxTima.Text.Trim() + "%'";
            }

            cmdd = new MySqlCommand(sql, con);

            adapter.SelectCommand = cmdd;
            adapter.Fill(ds);

            bs.DataSource = ds.Tables[0];
            return bs;

        }

        private void pretraziIgraca_Click(object sender, EventArgs e)
        {
            igraciTabela.DataSource = loadI();
        }

        private void otkaziIgraca_Click(object sender, EventArgs e)
        {
            ResetIgraci();
        }

        // funkcija koja prazni polja na igraci panelu
        private void ResetIgraci()
        {
            keyI = 0;
            boxImeIgraca.Text = "";
            boxPrezimeIgraca.Text = "";
            boxGRIgraca.Text = "";
            boxDres.Text = "";
            boxPozicija.Text = "";
            boxTima.Text = "";
        }


        private void igraciTabela_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            boxImeIgraca.Text = igraciTabela.SelectedRows[0].Cells[1].Value.ToString();
            boxPrezimeIgraca.Text = igraciTabela.SelectedRows[0].Cells[2].Value.ToString();
            boxGRIgraca.Text = igraciTabela.SelectedRows[0].Cells[3].Value.ToString();
            boxTima.Text = igraciTabela.SelectedRows[0].Cells[4].Value.ToString();
            boxPozicija.Text = igraciTabela.SelectedRows[0].Cells[6].Value.ToString();
            boxDres.Text = igraciTabela.SelectedRows[0].Cells[7].Value.ToString();

            if (boxImeIgraca.Text == "") { keyI = 0; }
            else { keyI = Convert.ToInt32(igraciTabela.SelectedRows[0].Cells[0].Value.ToString()); }
        }


        //POPUNJAVANJE SVIH POLJA --- provera da li su sva polja popunjena
        bool ProveriPoljaI()
        {
            if (boxImeIgraca.Text == "" || boxPrezimeIgraca.Text == "" || boxGRIgraca.Text == ""
                || boxTima.Text == "" || boxPozicija.Text == "" || boxDres.Text == "")
            {
                return true;
            }
            else { return false; }
        }


        private void DodajIgraca_Click(object sender, EventArgs e)
        {
            if (ProveriPoljaI() == true)
            {
                MessageBox.Show("Nisu uneti svi podaci!", "Dodavanje igrača neuspešno!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand();
                MySqlTransaction transaction;
                transaction = con.BeginTransaction();
                cmd.Connection = con;
                cmd.Transaction = transaction;

                string imeI=boxImeIgraca.Text;
                string prezimeI=boxPrezimeIgraca.Text;
                string godinaR=boxGRIgraca.Text;
                string dres=boxDres.Text;
                string pozicija=boxPozicija.Text;
                string tim=boxTima.Text;

                try
                {
                    String r = "SELECT * from igraci";
                    MySqlCommand com = new MySqlCommand(r, con);
                    MySqlDataReader dr = com.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        dr.Close();
                        cmd.CommandText = "INSERT INTO igraci ( ime, prezime, godina_rodjenja, id_tima, pozicija, broj_dresa) values('" + imeI + "' ,'" + prezimeI + "' ,'" + godinaR + "' ,'" + tim + "','"+ pozicija + "' ,'" + dres + "')";
                        cmd.ExecuteNonQuery();
                        transaction.Commit();
                        MessageBox.Show("Izvršeno!");
                        ResetIgraci();
                        igraciTabela.DataSource = loadI();
                    }
                    else MessageBox.Show("Nevažeći igrač!");
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    MessageBox.Show(exception.Message);
                }
                finally { con.Close(); }
            }
        }


        private void izmeniIgraca_Click(object sender, EventArgs e)
        {
            if (ProveriPoljaI() == true)
            {
                MessageBox.Show("Nisu uneti svi podaci!", "Izmena igrača neuspešna!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand();
                MySqlTransaction transaction;
                transaction = con.BeginTransaction();
                cmd.Connection = con;
                cmd.Transaction = transaction;

                string imeI=boxImeIgraca.Text;
                string prezimeI=boxPrezimeIgraca.Text;
                string godinaR=boxGRIgraca.Text;
                string dres=boxDres.Text;
                string pozicija=boxPozicija.Text;
                string tim=boxTima.Text;

                try
                {
                    String r = "SELECT * from igraci";
                    MySqlCommand com = new MySqlCommand(r, con);
                    MySqlDataReader dr = com.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        dr.Close();
                        cmd.CommandText = "UPDATE igraci set ime='" + imeI + "', prezime='" + prezimeI + "', godina_rodjenja='" + godinaR + "', pozicija='" + pozicija + "', id_tima='" + tim + "', broj_dresa='" + dres + "' where id_igraca=" + keyI + "";
                        cmd.ExecuteNonQuery();
                        transaction.Commit();
                        MessageBox.Show("Izvršeno!");
                        ResetIgraci();
                        igraciTabela.DataSource = loadI();
                    }
                    else MessageBox.Show("Nevažeći igrač!");
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    MessageBox.Show(exception.Message);
                }
                finally { con.Close(); }
            }
        }

        private void obrisiIgraca_Click(object sender, EventArgs e)
        {
            if (ProveriPoljaI() == true)
            {
                MessageBox.Show("Nisu uneti svi podaci!", "Brisanje igrača neuspešno!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand();
                MySqlTransaction transaction;
                transaction = con.BeginTransaction();
                cmd.Connection = con;
                cmd.Transaction = transaction;

                try
                {
                    String r = "SELECT * from igraci";
                    MySqlCommand com = new MySqlCommand(r, con);
                    MySqlDataReader dr = com.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        dr.Close();
                        cmd.CommandText = " DELETE from igraci where id_igraca=" + keyI + "";
                        cmd.ExecuteNonQuery();
                        transaction.Commit();
                        MessageBox.Show("Izvršeno!");
                        ResetIgraci();
                        igraciTabela.DataSource = loadI();
                    }
                    else MessageBox.Show("Nevažeći igrač!");
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    MessageBox.Show(exception.Message);
                }
                finally { con.Close(); }
            }
        }

    }
}
