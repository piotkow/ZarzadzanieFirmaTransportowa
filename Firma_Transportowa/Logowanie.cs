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
using MySql.Data.MySqlClient;


namespace Firma_Transportowa
{
    public partial class Logowanie : Form
    {
        private bool dragging = false;
        private Point startPoint = new Point(0, 0);

        public Logowanie()
        {
            InitializeComponent();
        }
        
        Konto k1 = new Konto();
        bool CzyZalogowac = false;
        public bool getCzyZalogowac()
        {
            return CzyZalogowac;
        }
        public Konto getKonto()
        {
            return k1;
        }
        public bool CzyJestPusty(TextBox x)
        {
            if (String.IsNullOrEmpty(x.Text) || x.Text == "Login" || x.Text == "Hasło")
            {
                return true;
            }
            else
            {

                return false;
            }
        }
        private void btnZaloguj_Click(object sender, EventArgs e)
        {
            int pom_aktywne = 1;
            MySqlConnection con = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;");
            con.Open();
            using (var cmd = new SqlCommand("SELECT * FROM konta"))
            {
                MySqlCommand c = new MySqlCommand("SELECT MAX(idKonta) FROM konta", con);
                int pom = Convert.ToInt32(c.ExecuteScalar());
                for (int i = 1; i <= pom; i++)
                {
                    MySqlCommand command = new MySqlCommand($"SELECT login FROM konta WHERE idKonta={i}", con);
                    MySqlCommand command2 = new MySqlCommand($"SELECT haslo FROM konta WHERE idKonta={i}", con);
                    MySqlCommand command3 = new MySqlCommand($"SELECT rola FROM konta WHERE idKonta={i}", con);
                    string pom_login = Convert.ToString(command.ExecuteScalar());
                    string pom_haslo = Convert.ToString(command2.ExecuteScalar());
                    string pom_rola = Convert.ToString(command3.ExecuteScalar());

                    if (pom_rola == "Kierowca" && pom_login != "kierowca")
                    {
                        MySqlCommand czyAktywne = new MySqlCommand($"SELECT czy_aktywne FROM kierowcy WHERE idKonta={i}", con);
                        pom_aktywne = Convert.ToInt32(czyAktywne.ExecuteScalar());
                    }
                    else if (pom_rola == "Klient")
                    {
                        MySqlCommand czyAktywne = new MySqlCommand($"SELECT czy_aktywne FROM klienci WHERE idKonta={i}", con);
                        pom_aktywne = Convert.ToInt32(czyAktywne.ExecuteScalar());
                    }

                    if (txtLogin.Text == pom_login && txtHaslo.Text == pom_haslo && pom_aktywne == 1 && !CzyJestPusty(txtLogin) &&!CzyJestPusty(txtHaslo))
                    {
                        k1.Login = txtLogin.Text;
                        k1.Haslo = txtHaslo.Text;
                        k1.Id = i;
                        CzyZalogowac = true;
                        MySqlCommand commandRola = new MySqlCommand($"SELECT rola FROM konta WHERE login LIKE '{k1.Login}'", con);
                        string rola = Convert.ToString(commandRola.ExecuteScalar());
                        switch (rola)
                        {
                            case "Admin":
                                k1.Rola = Role.Admin;
                                break;
                            case "Kierowca":
                                k1.Rola = Role.Kierowca;
                                break;
                            case "Klient":
                                k1.Rola = Role.Klient;
                                break;
                        }
                        break;
                    }
                    else
                    {
                        CzyZalogowac = false;
                    }

                }
                if (!CzyZalogowac)
                {
                    lblWynikLogowania.Text = "Błędny login lub hasło";
                }
                else
                {
                    con.Close();
                    this.Close();
                }
            }

        }

        private void txtLogin_Click(object sender, EventArgs e)
        {
            if (txtLogin.Text == "Login") txtLogin.Clear();
            pictureBox1.BackgroundImage = Properties.Resources.user;
            panelLogin.BackColor = Color.FromArgb(241, 166, 13);
            txtLogin.ForeColor = Color.FromArgb(241, 166, 13);

            pictureBox2.BackgroundImage = Properties.Resources.hasloW;
            panelHaslo.BackColor = Color.WhiteSmoke;
            txtHaslo.ForeColor = Color.WhiteSmoke;
            if (CzyJestPusty(txtHaslo)) { txtHaslo.PasswordChar = '\0'; txtHaslo.Text = "Hasło"; }
        }

        private void txtHaslo_TextChanged(object sender, EventArgs e)
        {
           // txtHaslo.PasswordChar = '*';
        }

        private void txtHaslo_Click_1(object sender, EventArgs e)
        {
            if (txtHaslo.Text == "Hasło") txtHaslo.Clear();
            txtHaslo.PasswordChar = '*';
            pictureBox2.BackgroundImage = Properties.Resources.hasloY2;
            panelHaslo.BackColor = Color.FromArgb(241, 166, 13);
            txtHaslo.ForeColor = Color.FromArgb(241, 166, 13);

            pictureBox1.BackgroundImage = Properties.Resources.userW;
            panelLogin.BackColor = Color.WhiteSmoke;
            txtLogin.ForeColor = Color.WhiteSmoke;
            if (CzyJestPusty(txtLogin)) txtLogin.Text = "Login";
        }

        private void exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Zarejestruj_Click(object sender, EventArgs e)
        {
            this.Close();
            Rejestracja okno_rejestracji = new Rejestracja();
            okno_rejestracji.ShowDialog();
        }

        private void Logowanie_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            startPoint = new Point(e.X, e.Y);
        }

        private void Logowanie_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void Logowanie_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - this.startPoint.X, p.Y - this.startPoint.Y);
            }
        }

        private void Logowanie_KeyDown(object sender, KeyEventArgs e)
        {
            int pom_aktywne = 1;
            MySqlConnection con = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;");
            con.Open();
            using (var cmd = new SqlCommand("SELECT * FROM konta"))
            {
                MySqlCommand c = new MySqlCommand("SELECT MAX(idKonta) FROM konta", con);
                int pom = Convert.ToInt32(c.ExecuteScalar());
                for (int i = 1; i <= pom; i++)
                {
                    MySqlCommand command = new MySqlCommand($"SELECT login FROM konta WHERE idKonta={i}", con);
                    MySqlCommand command2 = new MySqlCommand($"SELECT haslo FROM konta WHERE idKonta={i}", con);
                    MySqlCommand command3 = new MySqlCommand($"SELECT rola FROM konta WHERE idKonta={i}", con);
                    string pom_login = Convert.ToString(command.ExecuteScalar());
                    string pom_haslo = Convert.ToString(command2.ExecuteScalar());
                    string pom_rola = Convert.ToString(command3.ExecuteScalar());

                    if (pom_rola == "Kierowca" && pom_login != "kierowca")
                    {
                        MySqlCommand czyAktywne = new MySqlCommand($"SELECT czy_aktywne FROM kierowcy WHERE idKonta={i}", con);
                        pom_aktywne = Convert.ToInt32(czyAktywne.ExecuteScalar());
                    }
                    else if (pom_rola == "Klient")
                    {
                        MySqlCommand czyAktywne = new MySqlCommand($"SELECT czy_aktywne FROM klienci WHERE idKonta={i}", con);
                        pom_aktywne = Convert.ToInt32(czyAktywne.ExecuteScalar());
                    }

                    if (txtLogin.Text == pom_login && txtHaslo.Text == pom_haslo && pom_aktywne == 1 && !CzyJestPusty(txtLogin) && !CzyJestPusty(txtHaslo))
                    {
                        k1.Login = txtLogin.Text;
                        k1.Haslo = txtHaslo.Text;
                        k1.Id = i;
                        CzyZalogowac = true;
                        MySqlCommand commandRola = new MySqlCommand($"SELECT rola FROM konta WHERE login LIKE '{k1.Login}'", con);
                        string rola = Convert.ToString(commandRola.ExecuteScalar());
                        switch (rola)
                        {
                            case "Admin":
                                k1.Rola = Role.Admin;
                                break;
                            case "Kierowca":
                                k1.Rola = Role.Kierowca;
                                break;
                            case "Klient":
                                k1.Rola = Role.Klient;
                                break;
                        }
                        break;
                    }
                    else
                    {
                        CzyZalogowac = false;
                    }

                }
                if (!CzyZalogowac)
                {
                    lblWynikLogowania.Text = "Błędny login lub hasło";
                }
                else
                {
                    con.Close();
                    this.Close();
                }
            }
        }
    }
}

