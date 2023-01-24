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
using System.Data.Entity.Core;

namespace Firma_Transportowa
{
    public partial class Rejestracja : Form
    {
        private bool dragging = false;
        private Point startPoint = new Point(0, 0);
        public Rejestracja()
        {
            InitializeComponent();
        }
        public bool CzyJestPusty(TextBox x)
        {
            if (String.IsNullOrEmpty(x.Text) || x.Text == "Imię" || x.Text == "Nazwisko" || x.Text == "E-mail" || x.Text == "Numer Telefonu" || x.Text == "Login" || x.Text == "Hasło" || x.Text == "Powtórz Hasło")
            {
                return true;
            }
            else
            {

                return false;
            }
        }
        void Wyczysc()
        {
            lblZleImie.Text = "";
            lblZleNazwisko.Text = "";
            lblZleHaslo.Text = "";
            lblZlyEmail.Text = "";
            lblZlyLogin.Text = "";
            lblZlyNumer.Text = "";
        }
        public bool CzyTakieSameKonta(Konto p1, Konto p2, Label email, Label login)
        {
            bool spr = true;
            if (p1.Email == p2.Email)
            {
                email.Text = "Konto z takim e-mailem już istnieje!";
                spr = false;
            }
            if (p1.Login == p2.Login)
            {
                login.Text = "Taki login już istnieje!";
                spr = false;
            }
            return spr;
        }

        private void btnRejestracja_Click(object sender, EventArgs e)
        {
            Wyczysc();
            MySqlConnection con = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;");
            con.Open();
            string nrTel = txtNrTelefonu.Text;
            MySqlCommand c = new MySqlCommand("SELECT MAX(idKonta) FROM konta", con);
            int pom = Convert.ToInt32(c.ExecuteScalar());

            for (int i = 1; i <= pom; i++)
            {
                MySqlCommand command = new MySqlCommand($"SELECT email_klienta FROM konta WHERE idKonta={i}", con);
                MySqlCommand command2 = new MySqlCommand($"SELECT login FROM konta WHERE idKonta={i}", con);
                MySqlCommand command3 = new MySqlCommand($"SELECT numer_tel_klienta FROM klienci WHERE idKonta={i}", con);
                MySqlCommand command4 = new MySqlCommand($"SELECT haslo FROM konta WHERE idKonta={i}", con);
                string pom_nrtel = Convert.ToString(command3.ExecuteScalar());
                Konto pomKonto = new Konto(Convert.ToString(command.ExecuteScalar()), Convert.ToString(command2.ExecuteScalar()), Convert.ToString(command4.ExecuteScalar()));

                if (CzyJestPusty(txtImie) || CzyJestPusty(txtNazwisko) || CzyJestPusty(txtLogin) || CzyJestPusty(txtEmail) || CzyJestPusty(txtHaslo) || CzyJestPusty(txtNrTelefonu) || CzyJestPusty(txtPowtorzHaslo))
                {
                    lblZleHaslo.Text = "Pola nie mogą być puste!";
                    break;
                }
                else
                {
                    try
                    {
                        Konto k1 = new Konto(txtEmail.Text, txtLogin.Text, txtHaslo.Text);
                        try
                        {
                            if (CzyTakieSameKonta(k1, pomKonto, lblZlyEmail, lblZlyLogin))
                            {
                                if (txtHaslo.Text == txtPowtorzHaslo.Text)
                                {
                                    if (nrTel != pom_nrtel)
                                    {
                                        Klient klient = new Klient(txtImie.Text, txtNazwisko.Text, txtNrTelefonu.Text);
                                        MySqlCommand daneKonta = new MySqlCommand($"INSERT INTO konta (email_klienta,login,haslo,rola) VALUES ('{txtEmail.Text}','{txtLogin.Text}','{txtHaslo.Text}','Klient')", con);
                                        MySqlCommand idKonta = new MySqlCommand("SELECT MAX(idKonta) FROM konta", con);
                                        daneKonta.ExecuteScalar();
                                        int pom_id = Convert.ToInt32(idKonta.ExecuteScalar());
                                        MySqlCommand daneKlienta = new MySqlCommand($"INSERT INTO klienci (nazwisko_klienta,imie_klienta,numer_tel_klienta,weryfikacja,idKonta,czy_aktywne) VALUES ('{txtNazwisko.Text}','{txtImie.Text}','{txtNrTelefonu.Text}','0','{pom_id}',1)", con);
                                        daneKlienta.ExecuteScalar();
                                        this.Close();
                                        break;
                                    }
                                    else
                                    {
                                        lblZlyNumer.Text = "Konto z takim numerem telefonu już istnieje!";
                                        break;
                                    }
                                }
                                else
                                {
                                    lblZleHaslo.Text = "Podane hasła nie są takie same";
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            lblZlyNumer.Text = "Podany numer telefonu ma nieprawidłowy format!";
                            break;
                        }
                        catch (ArgumentNullException)
                        {
                            lblZleImie.Text = "Podane imie ma nieprawidłowy format!";
                            break;
                        }
                        catch (ArgumentException)
                        {
                            lblZleNazwisko.Text = "Podane nazwisko ma nieprawidłowy format!";
                            break;
                        }
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        lblZleHaslo.Text = "Długość hasła jest nieprawidłowa!";
                        break;
                    }
                    catch (ArgumentNullException)
                    {
                        lblZlyLogin.Text = "Długość loginu jest nieprawidłowa!";
                        break;
                    }
                    catch (ArgumentException)
                    {
                        lblZlyEmail.Text = "Podany e-mail jest nieprawidłowy!";
                        break;
                    }
                }

            }

        }

        private void txtImie_Click(object sender, EventArgs e)
        {
            if(txtImie.Text == "Imię")txtImie.Clear();
            panel1.BackColor = Color.FromArgb(241, 166, 13);
            txtImie.ForeColor = Color.FromArgb(241, 166, 13);
            
            panel2.BackColor = Color.WhiteSmoke;
            txtNazwisko.ForeColor = Color.WhiteSmoke;
            panel3.BackColor = Color.WhiteSmoke;
            txtNrTelefonu.ForeColor = Color.WhiteSmoke;
            panel4.BackColor = Color.WhiteSmoke;
            txtEmail.ForeColor = Color.WhiteSmoke;
            panel5.BackColor = Color.WhiteSmoke;
            txtLogin.ForeColor = Color.WhiteSmoke;
            panel6.BackColor = Color.WhiteSmoke;
            txtHaslo.ForeColor = Color.WhiteSmoke;
            panel7.BackColor = Color.WhiteSmoke;
            txtPowtorzHaslo.ForeColor = Color.WhiteSmoke;

            if (CzyJestPusty(txtNazwisko)) txtNazwisko.Text = "Nazwisko";
            if (CzyJestPusty(txtNrTelefonu)) txtNrTelefonu.Text = "Numer Telefonu";
            if (CzyJestPusty(txtEmail)) txtEmail.Text = "E-mail";
            if (CzyJestPusty(txtLogin)) txtLogin.Text = "Login";
            if (CzyJestPusty(txtHaslo)) { txtHaslo.PasswordChar = '\0'; txtHaslo.Text = "Hasło"; }
            if (CzyJestPusty(txtPowtorzHaslo)) { txtPowtorzHaslo.PasswordChar = '\0'; txtPowtorzHaslo.Text = "Powtórz Hasło"; }

        }

        private void txtNazwisko_Click(object sender, EventArgs e)
        {
            if (txtNazwisko.Text == "Nazwisko") txtNazwisko.Clear();
            panel2.BackColor = Color.FromArgb(241, 166, 13);
            txtNazwisko.ForeColor = Color.FromArgb(241, 166, 13);

            panel1.BackColor = Color.WhiteSmoke;
            txtImie.ForeColor = Color.WhiteSmoke;
            panel3.BackColor = Color.WhiteSmoke;
            txtNrTelefonu.ForeColor = Color.WhiteSmoke;
            panel4.BackColor = Color.WhiteSmoke;
            txtEmail.ForeColor = Color.WhiteSmoke;
            panel5.BackColor = Color.WhiteSmoke;
            txtLogin.ForeColor = Color.WhiteSmoke;
            panel6.BackColor = Color.WhiteSmoke;
            txtHaslo.ForeColor = Color.WhiteSmoke;
            panel7.BackColor = Color.WhiteSmoke;
            txtPowtorzHaslo.ForeColor = Color.WhiteSmoke;

            if (CzyJestPusty(txtImie)) txtImie.Text = "Imię";
            if (CzyJestPusty(txtNrTelefonu)) txtNrTelefonu.Text = "Numer Telefonu";
            if (CzyJestPusty(txtEmail)) txtEmail.Text = "E-mail";
            if (CzyJestPusty(txtLogin)) txtLogin.Text = "Login";
            if (CzyJestPusty(txtHaslo)) { txtHaslo.PasswordChar = '\0'; txtHaslo.Text = "Hasło"; }
            if (CzyJestPusty(txtPowtorzHaslo)) { txtPowtorzHaslo.PasswordChar = '\0'; txtPowtorzHaslo.Text = "Powtórz Hasło"; }

        }

        private void txtNrTelefonu_Click(object sender, EventArgs e)
        {
            if (txtNrTelefonu.Text == "Numer Telefonu") txtNrTelefonu.Clear();
            panel3.BackColor = Color.FromArgb(241, 166, 13);
            txtNrTelefonu.ForeColor = Color.FromArgb(241, 166, 13);

            panel1.BackColor = Color.WhiteSmoke;
            txtImie.ForeColor = Color.WhiteSmoke;
            panel2.BackColor = Color.WhiteSmoke;
            txtNazwisko.ForeColor = Color.WhiteSmoke;
            panel4.BackColor = Color.WhiteSmoke;
            txtEmail.ForeColor = Color.WhiteSmoke;
            panel5.BackColor = Color.WhiteSmoke;
            txtLogin.ForeColor = Color.WhiteSmoke;
            panel6.BackColor = Color.WhiteSmoke;
            txtHaslo.ForeColor = Color.WhiteSmoke;
            panel7.BackColor = Color.WhiteSmoke;
            txtPowtorzHaslo.ForeColor = Color.WhiteSmoke;

            if(CzyJestPusty(txtImie)) txtImie.Text = "Imię";
            if(CzyJestPusty(txtNazwisko)) txtNazwisko.Text = "Nazwisko";
            if(CzyJestPusty(txtEmail)) txtEmail.Text = "E-mail";
            if(CzyJestPusty(txtLogin)) txtLogin.Text = "Login";
            if (CzyJestPusty(txtHaslo)) { txtHaslo.PasswordChar = '\0'; txtHaslo.Text = "Hasło"; }
            if (CzyJestPusty(txtPowtorzHaslo)) { txtPowtorzHaslo.PasswordChar = '\0'; txtPowtorzHaslo.Text = "Powtórz Hasło"; }

        }

        private void txtEmail_Click(object sender, EventArgs e)
        {
            if (txtEmail.Text == "E-mail") txtEmail.Clear();
            panel4.BackColor = Color.FromArgb(241, 166, 13);
            txtEmail.ForeColor = Color.FromArgb(241, 166, 13);

            panel1.BackColor = Color.WhiteSmoke;
            txtImie.ForeColor = Color.WhiteSmoke;
            panel2.BackColor = Color.WhiteSmoke;
            txtNazwisko.ForeColor = Color.WhiteSmoke;
            panel3.BackColor = Color.WhiteSmoke;
            txtNrTelefonu.ForeColor = Color.WhiteSmoke;
            panel5.BackColor = Color.WhiteSmoke;
            txtLogin.ForeColor = Color.WhiteSmoke;
            panel6.BackColor = Color.WhiteSmoke;
            txtHaslo.ForeColor = Color.WhiteSmoke;
            panel7.BackColor = Color.WhiteSmoke;
            txtPowtorzHaslo.ForeColor = Color.WhiteSmoke;

            if (CzyJestPusty(txtImie)) txtImie.Text = "Imię";
            if (CzyJestPusty(txtNazwisko)) txtNazwisko.Text = "Nazwisko";
            if (CzyJestPusty(txtNrTelefonu)) txtNrTelefonu.Text = "Numer Telefonu";
            if (CzyJestPusty(txtLogin)) txtLogin.Text = "Login";
            if (CzyJestPusty(txtHaslo)) { txtHaslo.PasswordChar = '\0'; txtHaslo.Text = "Hasło"; }
            if (CzyJestPusty(txtPowtorzHaslo)) { txtPowtorzHaslo.PasswordChar = '\0'; txtPowtorzHaslo.Text = "Powtórz Hasło"; }

        }

        private void txtLogin_Click(object sender, EventArgs e)
        {
            if (txtLogin.Text == "Login") txtLogin.Clear();
            panel5.BackColor = Color.FromArgb(241, 166, 13);
            txtLogin.ForeColor = Color.FromArgb(241, 166, 13);

            panel1.BackColor = Color.WhiteSmoke;
            txtImie.ForeColor = Color.WhiteSmoke;
            panel2.BackColor = Color.WhiteSmoke;
            txtNazwisko.ForeColor = Color.WhiteSmoke;
            panel3.BackColor = Color.WhiteSmoke;
            txtNrTelefonu.ForeColor = Color.WhiteSmoke;
            panel4.BackColor = Color.WhiteSmoke;
            txtEmail.ForeColor = Color.WhiteSmoke;
            panel6.BackColor = Color.WhiteSmoke;
            txtHaslo.ForeColor = Color.WhiteSmoke;
            panel7.BackColor = Color.WhiteSmoke;
            txtPowtorzHaslo.ForeColor = Color.WhiteSmoke;

            if (CzyJestPusty(txtImie)) txtImie.Text = "Imię";
            if (CzyJestPusty(txtNazwisko)) txtNazwisko.Text = "Nazwisko";
            if (CzyJestPusty(txtNrTelefonu)) txtNrTelefonu.Text = "Numer Telefonu";
            if (CzyJestPusty(txtEmail)) txtEmail.Text = "E-mail";
            if (CzyJestPusty(txtHaslo)) { txtHaslo.PasswordChar = '\0'; txtHaslo.Text = "Hasło"; }
            if (CzyJestPusty(txtPowtorzHaslo)) { txtPowtorzHaslo.PasswordChar = '\0'; txtPowtorzHaslo.Text = "Powtórz Hasło"; }
        }

        private void txtHaslo_Click(object sender, EventArgs e)
        {
            if (txtHaslo.Text == "Hasło") txtHaslo.Clear();
            txtHaslo.PasswordChar = '*';
            panel6.BackColor = Color.FromArgb(241, 166, 13);
            txtHaslo.ForeColor = Color.FromArgb(241, 166, 13);

            panel1.BackColor = Color.WhiteSmoke;
            txtImie.ForeColor = Color.WhiteSmoke;
            panel2.BackColor = Color.WhiteSmoke;
            txtNazwisko.ForeColor = Color.WhiteSmoke;
            panel3.BackColor = Color.WhiteSmoke;
            txtNrTelefonu.ForeColor = Color.WhiteSmoke;
            panel4.BackColor = Color.WhiteSmoke;
            txtEmail.ForeColor = Color.WhiteSmoke;
            panel5.BackColor = Color.WhiteSmoke;
            txtLogin.ForeColor = Color.WhiteSmoke;
            panel7.BackColor = Color.WhiteSmoke;
            txtHaslo.ForeColor = Color.WhiteSmoke;

            if (CzyJestPusty(txtImie)) txtImie.Text = "Imię";
            if (CzyJestPusty(txtNazwisko)) txtNazwisko.Text = "Nazwisko";
            if (CzyJestPusty(txtNrTelefonu)) txtNrTelefonu.Text = "Numer Telefonu";
            if (CzyJestPusty(txtEmail)) txtEmail.Text = "E-mail";
            if (CzyJestPusty(txtLogin)) txtLogin.Text = "Login";
            if (CzyJestPusty(txtPowtorzHaslo)) { txtPowtorzHaslo.PasswordChar = '\0'; txtPowtorzHaslo.Text = "Powtórz Hasło"; }

        }

        private void txtPowtorzHaslo_Click(object sender, EventArgs e)
        {
            if (txtPowtorzHaslo.Text == "Powtórz Hasło") txtPowtorzHaslo.Clear();
            txtPowtorzHaslo.PasswordChar = '*';
            panel7.BackColor = Color.FromArgb(241, 166, 13);
            txtPowtorzHaslo.ForeColor = Color.FromArgb(241, 166, 13);

            panel1.BackColor = Color.WhiteSmoke;
            txtImie.ForeColor = Color.WhiteSmoke;
            panel2.BackColor = Color.WhiteSmoke;
            txtNazwisko.ForeColor = Color.WhiteSmoke;
            panel3.BackColor = Color.WhiteSmoke;
            txtNrTelefonu.ForeColor = Color.WhiteSmoke;
            panel4.BackColor = Color.WhiteSmoke;
            txtEmail.ForeColor = Color.WhiteSmoke;
            panel5.BackColor = Color.WhiteSmoke;
            txtLogin.ForeColor = Color.WhiteSmoke;
            panel6.BackColor = Color.WhiteSmoke;
            txtPowtorzHaslo.ForeColor = Color.WhiteSmoke;

            if (CzyJestPusty(txtImie)) txtImie.Text = "Imię";
            if (CzyJestPusty(txtNazwisko)) txtNazwisko.Text = "Nazwisko";
            if (CzyJestPusty(txtNrTelefonu)) txtNrTelefonu.Text = "Numer Telefonu";
            if (CzyJestPusty(txtEmail)) txtEmail.Text = "E-mail";
            if (CzyJestPusty(txtLogin)) txtLogin.Text = "Login";
            if (CzyJestPusty(txtHaslo)) { txtHaslo.PasswordChar = '\0'; txtHaslo.Text = "Hasło"; }
        }

        private void txtHaslo_TextChanged(object sender, EventArgs e)
        {
           // txtHaslo.PasswordChar = '*';
        }

        private void txtPowtorzHaslo_TextChanged(object sender, EventArgs e)
        {
          //  txtPowtorzHaslo.PasswordChar = '*';
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Rejestracja_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            startPoint = new Point(e.X, e.Y);
        }

        private void Rejestracja_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void Rejestracja_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - this.startPoint.X, p.Y - this.startPoint.Y);
            }
        }

    }
}
