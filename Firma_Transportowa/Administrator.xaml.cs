using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Text.Json.Serialization;
using iTextSharp.text.pdf;

namespace Firma_Transportowa
{
    /// <summary>
    /// Interaction logic for Administrator.xaml
    /// </summary>

    public partial class Administrator : Page
    {
        public Administrator()
        {
            InitializeComponent();
        }
        int idKontaKierowcy;
        int idKierowcy;
        int idKontaKlienta;
        int idKlienta;
        bool czyWybranoKierowce = false;
        bool czyWybranoKlienta = false;
        int weryfikacja = 0;
        void Wyczysc()
        {
            txtImie.Text = "";
            txtHaslo.Text = "";
            txtLogin.Text = "";
            txtMail.Text = "";
            txtNazwisko.Text = "";
            txtNrTelefonu.Text = "";
        }
        void WyczyscLabel()
        {
            lblZleHaslo.Content = "";
            lblZleImie.Content = "";
            lblZleLogin.Content = "";
            lblZleMail.Content = "";
            lblZleNrTel.Content = "";
            lblZleNazwisko.Content = "";
        }
        void UkryjWeryfikacje()
        {
            btnTak.Visibility = Visibility.Hidden;
            btnNie.Visibility = Visibility.Hidden;
            lblWeryfikacja.Visibility = Visibility.Hidden;
            lblOpis.Visibility = Visibility.Hidden;
            WeryfikacjaKlient.Visibility = Visibility.Hidden;
            OpisKlientGrid.Visibility = Visibility.Hidden;
        }
        void PokazWeryfikacje()
        {
            btnTak.Visibility = Visibility.Visible;
            btnNie.Visibility = Visibility.Visible;
            lblWeryfikacja.Visibility = Visibility.Visible;
            lblOpis.Visibility = Visibility.Visible;
            WeryfikacjaKlient.Visibility = Visibility.Visible;
            OpisKlientGrid.Visibility = Visibility.Visible;
        }
        void PokazEdycjeZamowienia()
        {
            btnTakPrzypisanie.Visibility = Visibility.Visible;
            btnNiePrzypisanie.Visibility = Visibility.Visible;
            lblCzyPrzydzielic.Visibility = Visibility.Visible;
            lbxKierowcaZamowienia.Visibility = Visibility.Visible;
            PrzydzielenieKierowcyGrid.Visibility = Visibility.Visible;
        }
        void UkryjEdycjeZamowienia()
        {
            btnTakPrzypisanie.Visibility = Visibility.Hidden;
            btnNiePrzypisanie.Visibility = Visibility.Hidden;
            lblCzyPrzydzielic.Visibility = Visibility.Hidden;
            lbxKierowcaZamowienia.Visibility = Visibility.Hidden;
            PrzydzielenieKierowcyGrid.Visibility = Visibility.Hidden;
        }
        void Inicjalizacja()
        {
            txtGodziny.Text = "";
            txtKomentarz.Text = "";
            lbxKierowcy.Items.Clear();
            lbxKlienci.Items.Clear();
            lbxWeryfikacja.Items.Clear();
            lbxZamowienia.Items.Clear();
            lbxKierowcaZamowienia.Items.Clear();
            lbxZamowieniaEdycja.Items.Clear();
            lbxKierowcyStawkaGodzinowa.Items.Clear();
            UkryjWeryfikacje();
            UkryjWeryfikacjeZamowienia();
            UkryjEdycjeZamowienia();
            weryfikacja = 0;
            MySqlConnection con = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;");
            con.Open();
            MySqlCommand commKierowca = new MySqlCommand($"SELECT * FROM kierowcy where idKonta>2", con);
            MySqlCommand comKlienci = new MySqlCommand($"SELECT * FROM Klienci where weryfikacja=1", con);
            MySqlCommand comKlienci1 = new MySqlCommand($"SELECT * FROM Klienci where weryfikacja=0", con);

            MySqlDataReader czytnik2 = comKlienci1.ExecuteReader();
            while (czytnik2.Read())
            {
                if (czytnik2.HasRows)
                {
                    if (Convert.ToInt32(czytnik2["czy_aktywne"]) == 1)
                    {
                        Klient klient1 = new Klient(Convert.ToString(czytnik2["Imie_Klienta"]), Convert.ToString(czytnik2["nazwisko_Klienta"]), Convert.ToString(czytnik2["numer_tel_klienta"]))
                        {
                            IdKlienci = Convert.ToInt32(czytnik2["IdKlienci"]),
                            Weryfikacja = Convert.ToInt32(czytnik2["weryfikacja"]),
                            IdKonta = Convert.ToInt32(czytnik2["idkonta"])
                        };
                        lbxWeryfikacja.Items.Add(klient1);
                    }
                }
            }
            czytnik2.Close();

            MySqlDataReader czytnik = commKierowca.ExecuteReader();
            while (czytnik.Read())
            {
                if (!DBNull.Value.Equals(czytnik["czy_aktywne"]))
                {
                    if (Convert.ToInt32(czytnik["czy_aktywne"]) == 1)
                    {
                        Kierowca kier1 = new Kierowca(Convert.ToString(czytnik["Imie_prac"]), Convert.ToString(czytnik["nazwisko_prac"]), Convert.ToString(czytnik["numer_tel_kierowcy"])) { IdKierowcy = Convert.ToInt32(czytnik["idkierowcy"]), IdKonta = Convert.ToInt32(czytnik["idkonta"]) };
                        lbxKierowcy.Items.Add(kier1);
                        lbxKierowcaZamowienia.Items.Add(kier1);
                        lbxKierowcyStawkaGodzinowa.Items.Add(kier1);
                    }
                }
            }
            czytnik.Close();

            MySqlDataReader czytnik1 = comKlienci.ExecuteReader();
            while (czytnik1.Read())
            {
                if (czytnik1.HasRows)
                {
                    if (Convert.ToInt32(czytnik1["czy_aktywne"]) == 1)
                    {
                        Klient klient1 = new Klient(Convert.ToString(czytnik1["Imie_Klienta"]), Convert.ToString(czytnik1["nazwisko_Klienta"]), Convert.ToString(czytnik1["numer_tel_klienta"]))
                        {
                            IdKlienci = Convert.ToInt32(czytnik1["IdKlienci"]),
                            Weryfikacja = Convert.ToInt32(czytnik1["weryfikacja"]),
                            IdKonta = Convert.ToInt32(czytnik1["idkonta"])
                        };
                        lbxKlienci.Items.Add(klient1);
                    }
                }
            }
            czytnik1.Close();

            zamowienia zam;
            MySqlCommand commZamowienia = new MySqlCommand($"SELECT * FROM zamowienia", con);

            MySqlDataReader czytnikZlecenia = commZamowienia.ExecuteReader();

            while (czytnikZlecenia.Read())
            {
                if (!DBNull.Value.Equals(czytnikZlecenia["data_poczatku"]) && !DBNull.Value.Equals(czytnikZlecenia["data_konca"]))
                {
                    zam = new zamowienia()
                    {
                        idZamowienia = Convert.ToInt32(czytnikZlecenia["idZamowienia"]),
                        idKierowcy = Convert.ToInt32(czytnikZlecenia["idKierowcy"]),
                        idPojazdy = Convert.ToInt32(czytnikZlecenia["idPojazdy"]),
                        idKlienci = Convert.ToInt32(czytnikZlecenia["idKlienci"]),
                        idKonta = Convert.ToInt32(czytnikZlecenia["idKonta"]),
                        idSzczegoly_zamowienia = Convert.ToInt32(czytnikZlecenia["idSzczegoly_zamowienia"]),
                        data_zlozenia_zamowienia = Convert.ToDateTime(czytnikZlecenia["data_zlozenia_zamowienia"]),
                        status_zamowienia = Convert.ToString(czytnikZlecenia["status_zamowienia"]),
                        data_poczatku = Convert.ToDateTime(czytnikZlecenia["data_poczatku"]),
                        data_konca = Convert.ToDateTime(czytnikZlecenia["data_konca"]),
                        komentarz = Convert.ToString(czytnikZlecenia["komentarz"]),
                    };
                }
                else if (!DBNull.Value.Equals(czytnikZlecenia["data_poczatku"]))
                {
                    zam = new zamowienia()
                    {
                        idZamowienia = Convert.ToInt32(czytnikZlecenia["idZamowienia"]),
                        idKierowcy = Convert.ToInt32(czytnikZlecenia["idKierowcy"]),
                        idPojazdy = Convert.ToInt32(czytnikZlecenia["idPojazdy"]),
                        idKlienci = Convert.ToInt32(czytnikZlecenia["idKlienci"]),
                        idKonta = Convert.ToInt32(czytnikZlecenia["idKonta"]),
                        idSzczegoly_zamowienia = Convert.ToInt32(czytnikZlecenia["idSzczegoly_zamowienia"]),
                        data_zlozenia_zamowienia = Convert.ToDateTime(czytnikZlecenia["data_zlozenia_zamowienia"]),
                        status_zamowienia = Convert.ToString(czytnikZlecenia["status_zamowienia"]),
                        data_poczatku = Convert.ToDateTime(czytnikZlecenia["data_poczatku"]),
                        komentarz = Convert.ToString(czytnikZlecenia["komentarz"]),
                    };
                }
                else if (!DBNull.Value.Equals(czytnikZlecenia["data_konca"]))
                {
                    zam = new zamowienia()
                    {
                        idZamowienia = Convert.ToInt32(czytnikZlecenia["idZamowienia"]),
                        idKierowcy = Convert.ToInt32(czytnikZlecenia["idKierowcy"]),
                        idPojazdy = Convert.ToInt32(czytnikZlecenia["idPojazdy"]),
                        idKlienci = Convert.ToInt32(czytnikZlecenia["idKlienci"]),
                        idKonta = Convert.ToInt32(czytnikZlecenia["idKonta"]),
                        idSzczegoly_zamowienia = Convert.ToInt32(czytnikZlecenia["idSzczegoly_zamowienia"]),
                        data_zlozenia_zamowienia = Convert.ToDateTime(czytnikZlecenia["data_zlozenia_zamowienia"]),
                        status_zamowienia = Convert.ToString(czytnikZlecenia["status_zamowienia"]),
                        data_konca = Convert.ToDateTime(czytnikZlecenia["data_konca"]),
                        komentarz = Convert.ToString(czytnikZlecenia["komentarz"]),
                    };
                }
                else
                {
                    zam = new zamowienia()
                    {
                        idZamowienia = Convert.ToInt32(czytnikZlecenia["idZamowienia"]),
                        idKierowcy = Convert.ToInt32(czytnikZlecenia["idKierowcy"]),
                        idPojazdy = Convert.ToInt32(czytnikZlecenia["idPojazdy"]),
                        idKlienci = Convert.ToInt32(czytnikZlecenia["idKlienci"]),
                        idKonta = Convert.ToInt32(czytnikZlecenia["idKonta"]),
                        idSzczegoly_zamowienia = Convert.ToInt32(czytnikZlecenia["idSzczegoly_zamowienia"]),
                        data_zlozenia_zamowienia = Convert.ToDateTime(czytnikZlecenia["data_zlozenia_zamowienia"]),
                        status_zamowienia = Convert.ToString(czytnikZlecenia["status_zamowienia"]),
                        komentarz = Convert.ToString(czytnikZlecenia["komentarz"]),
                    };
                }
                lbxZamowienia.Items.Add(zam);
            }
            czytnikZlecenia.Close();

            zamowienia zamEdycja;
            MySqlCommand commZamowieniaEdycja = new MySqlCommand($"SELECT * FROM zamowienia where status_zamowienia=@status", con);
            commZamowieniaEdycja.Parameters.AddWithValue("@status", "Zatwierdzony");
            MySqlDataReader czytnikZleceniaEdycja = commZamowieniaEdycja.ExecuteReader();
            while (czytnikZleceniaEdycja.Read())
            {
                zamEdycja = new zamowienia()
                {
                    idZamowienia = Convert.ToInt32(czytnikZleceniaEdycja["idZamowienia"]),
                    idKierowcy = Convert.ToInt32(czytnikZleceniaEdycja["idKierowcy"]),
                    idPojazdy = Convert.ToInt32(czytnikZleceniaEdycja["idPojazdy"]),
                    idKlienci = Convert.ToInt32(czytnikZleceniaEdycja["idKlienci"]),
                    idKonta = Convert.ToInt32(czytnikZleceniaEdycja["idKonta"]),
                    idSzczegoly_zamowienia = Convert.ToInt32(czytnikZleceniaEdycja["idSzczegoly_zamowienia"]),
                    data_zlozenia_zamowienia = Convert.ToDateTime(czytnikZleceniaEdycja["data_zlozenia_zamowienia"]),
                    status_zamowienia = Convert.ToString(czytnikZleceniaEdycja["status_zamowienia"]),
                    zamowiona_data_realizacji = Convert.ToDateTime(czytnikZleceniaEdycja["zamowiona_data_realizacji"]),
                };
                lbxZamowieniaEdycja.Items.Add(zamEdycja);
            }
            czytnikZleceniaEdycja.Close();
            con.Close();
            czyWybranoKierowce = false;
            czyWybranoKlienta = false;
        }

        public Administrator(int id)
        {
            InitializeComponent();
            Inicjalizacja();
            OdswiezWeryfikacje();
        }

        private void btnEdytuj_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnection con2 = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;");
            con2.Open();
            if (czyWybranoKierowce)
            {
                Kierowca kier1 = new Kierowca();
                kier1 = lbxKierowcy.SelectedItem as Kierowca;
                if (kier1 != null)
                {
                    gridEdycji.Visibility = Visibility.Visible;
                    chkWeryfikacja.Visibility = Visibility.Hidden;
                    txtImie.Text = kier1.Imie;
                    txtNazwisko.Text = kier1.Nazwisko;
                    txtNrTelefonu.Text = kier1.NrTelefonu;
                    idKierowcy = kier1.IdKierowcy;
                    cbxRola.SelectedIndex = 1;

                    MySqlCommand comKierowca2 = new MySqlCommand($"SELECT * FROM konta where idKonta={Convert.ToInt32(kier1.IdKonta)}", con2);
                    MySqlDataReader czytnik2 = comKierowca2.ExecuteReader();
                    if (czytnik2.Read())
                    {
                        idKontaKierowcy = Convert.ToInt32(czytnik2["idkonta"]);
                        txtLogin.Text = Convert.ToString(czytnik2["login"]);
                        txtHaslo.Text = Convert.ToString(czytnik2["haslo"]);
                        txtMail.Text = Convert.ToString(czytnik2["email_klienta"]);
                    }
                    czytnik2.Close();
                }
            }
            if (czyWybranoKlienta)
            {
                Klient klient1 = new Klient();
                klient1 = lbxKlienci.SelectedItem as Klient;
                if (klient1 != null)
                {
                    gridEdycji.Visibility = Visibility.Visible;
                    chkWeryfikacja.Visibility = Visibility.Visible;
                    txtImie.Text = klient1.Imie;
                    txtNazwisko.Text = klient1.Nazwisko;
                    txtNrTelefonu.Text = klient1.NrTelefonu;
                    idKlienta = klient1.IdKlienci;
                    cbxRola.SelectedIndex = 0;
                    if (klient1.Weryfikacja == 0)
                    {
                        chkWeryfikacja.IsChecked = false;
                    }
                    else
                    {
                        chkWeryfikacja.IsChecked = true;
                    }
                    MySqlCommand comKierowca2 = new MySqlCommand($"SELECT * FROM konta where idKonta={Convert.ToInt32(klient1.IdKonta)}", con2);
                    MySqlDataReader czytnik2 = comKierowca2.ExecuteReader();
                    if (czytnik2.Read())
                    {
                        idKontaKlienta = Convert.ToInt32(czytnik2["idkonta"]);
                        txtLogin.Text = Convert.ToString(czytnik2["login"]);
                        txtHaslo.Text = Convert.ToString(czytnik2["haslo"]);
                        txtMail.Text = Convert.ToString(czytnik2["email_klienta"]);
                    }
                    czytnik2.Close();
                }
            }
            con2.Close();
        }

        private void lbxKierowcy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            czyWybranoKierowce = true;
            czyWybranoKlienta = false;
            lbxKlienci.UnselectAll();
        }

        private void lbxKlienci_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            czyWybranoKlienta = true;
            czyWybranoKierowce = false;
            lbxKierowcy.UnselectAll();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            WyczyscLabel();
            if (czyWybranoKierowce)
            {
                if (cbxRola.SelectedIndex == 1)
                {
                    try
                    {

                        MySqlConnection con = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;");
                        con.Open();
                        MySqlCommand comDodaj = new MySqlCommand("update kierowcy set nazwisko_prac=@nazwisko, imie_prac=@imie, numer_tel_kierowcy=@nr_tel where idKonta=@idKontaKierowcy", con);
                        comDodaj.Parameters.AddWithValue("@nazwisko", txtNazwisko.Text);
                        comDodaj.Parameters.AddWithValue("@imie", txtImie.Text);
                        comDodaj.Parameters.AddWithValue("@idKontaKierowcy", idKontaKierowcy);
                        comDodaj.Parameters.AddWithValue("@nr_tel", txtNrTelefonu.Text);
                        Kierowca kierowca = new Kierowca(txtImie.Text, txtNazwisko.Text, txtNrTelefonu.Text);
                        comDodaj.ExecuteScalar();
                        MySqlCommand comDodajKonta = new MySqlCommand("update konta set email_klienta=@email, login=@login, haslo=@haslo where idKonta=@idKontaKierowcy", con);
                        comDodajKonta.Parameters.AddWithValue("@email", txtMail.Text);
                        comDodajKonta.Parameters.AddWithValue("@login", txtLogin.Text);
                        comDodajKonta.Parameters.AddWithValue("@haslo", txtHaslo.Text);
                        comDodajKonta.Parameters.AddWithValue("@idKontaKierowcy", idKontaKierowcy);
                        try
                        {

                            Konto konto = new Konto(txtMail.Text, txtLogin.Text, txtHaslo.Text);
                            comDodajKonta.ExecuteScalar();
                            con.Close();
                            MessageBox.Show("Zmiany zapisano");
                            Wyczysc();
                            gridEdycji.Visibility = Visibility.Hidden;
                            Inicjalizacja();
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            lblZleHaslo.Content = "Długość hasła jest nieprawidłowa!";
                        }
                        catch (ArgumentNullException)
                        {
                            lblZleLogin.Content = "Długość loginu jest nieprawidłowa!";
                        }
                        catch (ArgumentException)
                        {
                            lblZleMail.Content = "Podany e-mail jest nieprawidłowy!";
                        }
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        lblZleNrTel.Content = "Podany numer telefonu ma nieprawidłowy format!";
                    }
                    catch (ArgumentNullException)
                    {
                        lblZleImie.Content = "Podane imie ma nieprawidłowy format!";
                    }
                    catch (ArgumentException)
                    {
                        lblZleNazwisko.Content = "Podane nazwisko ma nieprawidłowy format!";
                    }
                }
                else if (cbxRola.SelectedIndex == 0)
                {

                    try
                    {
                        if (chkWeryfikacja.IsChecked == true)
                        {
                            weryfikacja = 1;
                        }
                        else
                        {
                            weryfikacja = 0;
                        }
                        MySqlConnection con = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;");
                        con.Open();
                        MySqlCommand comDodaj = new MySqlCommand("insert into Klienci(nazwisko_Klienta,imie_Klienta,numer_tel_klienta,weryfikacja,idKonta,czy_aktywne) values (@nazwisko, @imie, @numer, @weryfikacja, @idKontaKierowcy,@aktywne)", con);
                        comDodaj.Parameters.AddWithValue("@nazwisko", txtNazwisko.Text);
                        comDodaj.Parameters.AddWithValue("@imie", txtImie.Text);
                        comDodaj.Parameters.AddWithValue("@numer", txtNrTelefonu.Text);
                        comDodaj.Parameters.AddWithValue("@weryfikacja", weryfikacja);
                        comDodaj.Parameters.AddWithValue("@idKontaKierowcy", idKontaKierowcy);
                        comDodaj.Parameters.AddWithValue("@aktywne", 1);
                        Klient klient = new Klient(txtImie.Text, txtNazwisko.Text, txtNrTelefonu.Text);
                        try
                        {
                            Konto konto = new Konto(txtMail.Text, txtLogin.Text, txtHaslo.Text);
                            MySqlCommand comDezaktywacja = new MySqlCommand("update kierowcy set czy_aktywne=@aktywne where idKonta=@idKontaKierowcy", con);
                            comDezaktywacja.Parameters.AddWithValue("@aktywne", 0);
                            comDezaktywacja.Parameters.AddWithValue("@idKontaKierowcy", idKontaKierowcy);
                            comDezaktywacja.ExecuteScalar();
                            MySqlCommand comSpr = new MySqlCommand($"SELECT idKonta FROM klienci WHERE idKonta = {idKontaKierowcy} And EXISTS(SELECT idKonta FROM klienci WHERE idKonta = {idKontaKierowcy})", con);
                            object spr = comSpr.ExecuteScalar();
                            if (spr == null)
                            {
                                comDodaj.ExecuteScalar();
                            }
                            else
                            {
                                MySqlCommand comAktualizuj = new MySqlCommand($"update klienci set czy_aktywne={1}, weryfikacja={weryfikacja} where idKonta={idKontaKierowcy}", con);
                                comAktualizuj.ExecuteScalar();
                            }
                            MySqlCommand comDodajKonta = new MySqlCommand("update konta set email_klienta=@email, login=@login, haslo=@haslo,rola=@rola where idKonta=@idKontaKierowcy", con);
                            comDodajKonta.Parameters.AddWithValue("@email", txtMail.Text);
                            comDodajKonta.Parameters.AddWithValue("@login", txtLogin.Text);
                            comDodajKonta.Parameters.AddWithValue("@haslo", txtHaslo.Text);
                            comDodajKonta.Parameters.AddWithValue("@idKontaKierowcy", idKontaKierowcy);
                            comDodajKonta.Parameters.AddWithValue("@rola", "Klient");
                            comDodajKonta.ExecuteScalar();
                            con.Close();
                            MessageBox.Show("Zmiany zapisano");
                            Wyczysc();
                            gridEdycji.Visibility = Visibility.Hidden;
                            Inicjalizacja();
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            lblZleHaslo.Content = "Długość hasła jest nieprawidłowa!";
                        }
                        catch (ArgumentNullException)
                        {
                            lblZleLogin.Content = "Długość loginu jest nieprawidłowa!";
                        }
                        catch (ArgumentException)
                        {
                            lblZleMail.Content = "Podany e-mail jest nieprawidłowy!";
                        }
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        lblZleNrTel.Content = "Podany numer telefonu ma nieprawidłowy format!";
                    }
                    catch (ArgumentNullException)
                    {
                        lblZleImie.Content = "Podane imie ma nieprawidłowy format!";
                    }
                    catch (ArgumentException)
                    {
                        lblZleNazwisko.Content = "Podane nazwisko ma nieprawidłowy format!";
                    }
                }
            }
            if (czyWybranoKlienta)
            {
                if (cbxRola.SelectedIndex == 0)
                {
                    try
                    {
                        MySqlConnection con = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;");
                        con.Open();
                        if (chkWeryfikacja.IsChecked == true)
                        {
                            weryfikacja = 1;
                        }
                        else
                        {
                            weryfikacja = 0;
                        }
                        MySqlCommand comDodaj = new MySqlCommand("update klienci set nazwisko_klienta=@nazwisko, imie_Klienta=@imie, numer_tel_klienta=@nrTel, weryfikacja=@weryfikacja where idKonta=@idKontaKlienta", con);
                        comDodaj.Parameters.AddWithValue("@nazwisko", txtNazwisko.Text);
                        comDodaj.Parameters.AddWithValue("@imie", txtImie.Text);
                        comDodaj.Parameters.AddWithValue("@nrTel", txtNrTelefonu.Text);
                        comDodaj.Parameters.AddWithValue("@weryfikacja", weryfikacja);
                        comDodaj.Parameters.AddWithValue("@idKontaKlienta", idKontaKlienta);
                        Klient klient = new Klient(txtImie.Text, txtNazwisko.Text, txtNrTelefonu.Text);
                        MySqlCommand comDodajKonta = new MySqlCommand("update konta set email_klienta=@email, login=@login, haslo=@haslo where idKonta=@idKontaKlienta", con);
                        comDodajKonta.Parameters.AddWithValue("@email", txtMail.Text);
                        comDodajKonta.Parameters.AddWithValue("@login", txtLogin.Text);
                        comDodajKonta.Parameters.AddWithValue("@haslo", txtHaslo.Text);
                        comDodajKonta.Parameters.AddWithValue("@idKontaKlienta", idKontaKlienta);
                        try
                        {
                            Konto konto = new Konto(txtMail.Text, txtLogin.Text, txtHaslo.Text);
                            comDodaj.ExecuteScalar();
                            comDodajKonta.ExecuteScalar();
                            con.Close();
                            MessageBox.Show("Zmiany zapisano");
                            Wyczysc();
                            gridEdycji.Visibility = Visibility.Hidden;
                            Inicjalizacja();
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            lblZleHaslo.Content = "Długość hasła jest nieprawidłowa!";
                        }
                        catch (ArgumentNullException)
                        {
                            lblZleLogin.Content = "Długość loginu jest nieprawidłowa!";
                        }
                        catch (ArgumentException)
                        {
                            lblZleMail.Content = "Podany e-mail jest nieprawidłowy!";
                        }
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        lblZleNrTel.Content = "Podany numer telefonu ma nieprawidłowy format!";
                    }
                    catch (ArgumentNullException)
                    {
                        lblZleImie.Content = "Podane imie ma nieprawidłowy format!";
                    }
                    catch (ArgumentException)
                    {
                        lblZleNazwisko.Content = "Podane nazwisko ma nieprawidłowy format!";
                    }
                }
                else if (cbxRola.SelectedIndex == 1)
                {
                    try
                    {
                        MySqlConnection con = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;");
                        con.Open();
                        MySqlCommand comDodaj = new MySqlCommand("insert into kierowcy(nazwisko_prac,imie_prac,numer_tel_kierowcy,idKonta,czy_aktywne) values (@nazwisko, @imie,@nr_tel, @idKontaKlienta,@aktywne)", con);
                        comDodaj.Parameters.AddWithValue("@nazwisko", txtNazwisko.Text);
                        comDodaj.Parameters.AddWithValue("@imie", txtImie.Text);
                        comDodaj.Parameters.AddWithValue("@idKontaKlienta", idKontaKlienta);
                        comDodaj.Parameters.AddWithValue("@nr_tel", txtNrTelefonu.Text);
                        comDodaj.Parameters.AddWithValue("@aktywne", 1);

                        MySqlCommand comDezaktywacja = new MySqlCommand("update klienci set czy_aktywne=@aktywne where idKonta=@idKontaKlienta", con);
                        comDezaktywacja.Parameters.AddWithValue("@aktywne", 0);
                        comDezaktywacja.Parameters.AddWithValue("@idKontaKlienta", idKontaKlienta);

                        Kierowca kierowca = new Kierowca(txtImie.Text, txtNazwisko.Text, txtNrTelefonu.Text);
                        comDezaktywacja.ExecuteScalar();

                        MySqlCommand comSpr = new MySqlCommand($"SELECT idKonta FROM kierowcy WHERE idKonta = {idKontaKlienta} And EXISTS(SELECT idKonta FROM kierowcy WHERE idKonta = {idKontaKlienta})", con);
                        object spr = comSpr.ExecuteScalar();
                        if (spr == null)
                        {
                            comDodaj.ExecuteScalar();
                        }
                        else
                        {
                            MySqlCommand comAktualizuj = new MySqlCommand($"update kierowcy set czy_aktywne={1} where idKonta={idKontaKlienta}", con);
                            comAktualizuj.ExecuteScalar();
                        }
                        MySqlCommand comDodajKonta = new MySqlCommand("update konta set email_klienta=@email, login=@login, haslo=@haslo, rola=@role where idKonta=@idKontaKlienta", con);
                        comDodajKonta.Parameters.AddWithValue("@email", txtMail.Text);
                        comDodajKonta.Parameters.AddWithValue("@login", txtLogin.Text);
                        comDodajKonta.Parameters.AddWithValue("@haslo", txtHaslo.Text);
                        comDodajKonta.Parameters.AddWithValue("@role", "Kierowca");
                        comDodajKonta.Parameters.AddWithValue("@idKontaKlienta", idKontaKlienta);
                        try
                        {
                            Konto konto = new Konto(txtMail.Text, txtLogin.Text, txtHaslo.Text);
                            comDodajKonta.ExecuteScalar();
                            con.Close();
                            MessageBox.Show("Zmiany zapisano");
                            Wyczysc();
                            gridEdycji.Visibility = Visibility.Hidden;
                            Inicjalizacja();
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            lblZleHaslo.Content = "Długość hasła jest nieprawidłowa!";
                        }
                        catch (ArgumentNullException)
                        {
                            lblZleLogin.Content = "Długość loginu jest nieprawidłowa!";
                        }
                        catch (ArgumentException)
                        {
                            lblZleMail.Content = "Podany e-mail jest nieprawidłowy!";
                        }
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        lblZleNrTel.Content = "Podany numer telefonu ma nieprawidłowy format!";
                    }
                    catch (ArgumentNullException)
                    {
                        lblZleImie.Content = "Podane imie ma nieprawidłowy format!";
                    }
                    catch (ArgumentException)
                    {
                        lblZleNazwisko.Content = "Podane nazwisko ma nieprawidłowy format!";
                    }
                }
            }
        }

        private void btnAnuluj_Click(object sender, RoutedEventArgs e)
        {
            gridEdycji.Visibility = Visibility.Hidden;
            lbxKlienci.UnselectAll();
            lbxKierowcy.UnselectAll();
        }

        private void cbxRola_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxRola.SelectedIndex == 0)
            {
                chkWeryfikacja.Visibility = Visibility.Visible;
            }
            else
            {
                chkWeryfikacja.Visibility = Visibility.Hidden;
            }
        }

        private void lbxWeryfikacja_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            lblOpis.Content = "";
            PokazWeryfikacje();
            Klient klient = new Klient();
            klient = lbxWeryfikacja.SelectedItem as Klient;
            MySqlConnection con = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;");
            con.Open();
            if (klient != null)
            {
                MySqlCommand comKierowca2 = new MySqlCommand($"SELECT * FROM konta where idKonta={Convert.ToInt32(klient.IdKonta)}", con);
                MySqlDataReader czytnik2 = comKierowca2.ExecuteReader();
                if (czytnik2.Read())
                {
                    lblOpis.Content = $"Imię i Nazwisko: {klient.Imie} {klient.Nazwisko}\nNumer telefonu: {klient.NrTelefonu} \n" +
                        $"E-mail: {czytnik2["email_klienta"]} \nLogin: {czytnik2["login"]}\nHasło: {czytnik2["haslo"]}";
                }
                czytnik2.Close();
            }
            con.Close();
        }

        private void btnTak_Click(object sender, RoutedEventArgs e)
        {
            Klient klient = new Klient();
            klient = lbxWeryfikacja.SelectedItem as Klient;
            MySqlConnection con = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;");
            con.Open();
            MySqlCommand comDodaj = new MySqlCommand("update klienci set weryfikacja=@weryfikacja where idKonta=@idKontaKlienta", con);
            comDodaj.Parameters.AddWithValue("@weryfikacja", 1);
            comDodaj.Parameters.AddWithValue("@idKontaKlienta", Convert.ToInt32(klient.IdKonta));
            comDodaj.ExecuteScalar();
            con.Close();
            Inicjalizacja();
        }

        private void btnNie_Click(object sender, RoutedEventArgs e)
        {
            Klient klient = new Klient();
            klient = lbxWeryfikacja.SelectedItem as Klient;
            MySqlConnection con = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;");
            con.Open();
            MySqlCommand comDodaj = new MySqlCommand("update klienci set weryfikacja=@weryfikacja, czy_aktywne=@aktywne where idKonta=@idKontaKlienta", con);
            comDodaj.Parameters.AddWithValue("@weryfikacja", 0);
            comDodaj.Parameters.AddWithValue("@aktywne", 0);
            comDodaj.Parameters.AddWithValue("@idKontaKlienta", Convert.ToInt32(klient.IdKonta));
            comDodaj.ExecuteScalar();
            con.Close();
            lbxWeryfikacja.UnselectAll();
            UkryjWeryfikacje();
            Inicjalizacja();
        }

        private void btnWeryfikacja_Click(object sender, RoutedEventArgs e)
        {
            Color zolty = new Color();
            zolty.A = 255;
            zolty.R = 241;
            zolty.G = 166;
            zolty.B = 13;
            OdswiezWeryfikacje();
            btnEdycja.Foreground = new SolidColorBrush(zolty);
            btnEdycja.Background = Brushes.Transparent;
            btnEdycja.BorderBrush = new SolidColorBrush(zolty);
            btnZamowienia.Foreground = new SolidColorBrush(zolty);
            btnZamowienia.Background = Brushes.Transparent;
            btnZamowienia.BorderBrush = new SolidColorBrush(zolty);
            btnEdycjaZamowienia.Foreground = new SolidColorBrush(zolty);
            btnEdycjaZamowienia.Background = Brushes.Transparent;
            btnEdycjaZamowienia.BorderBrush = new SolidColorBrush(zolty);
            btnWeryfikacja.Background = new SolidColorBrush(zolty);
            btnWeryfikacja.Foreground = Brushes.Black;
            btnWeryfikacja.BorderBrush = Brushes.Black;
            btnPojazdy.Foreground = new SolidColorBrush(zolty);
            btnPojazdy.Background = Brushes.Transparent;
            btnPojazdy.BorderBrush = new SolidColorBrush(zolty);
            btnRaport.Foreground = new SolidColorBrush(zolty);
            btnRaport.Background = Brushes.Transparent;
            btnRaport.BorderBrush = new SolidColorBrush(zolty);
            btnStawkaGodzinowa.Foreground = new SolidColorBrush(zolty);
            btnStawkaGodzinowa.Background = Brushes.Transparent;
            btnStawkaGodzinowa.BorderBrush = new SolidColorBrush(zolty);
        }

        private void btnEdycja_Click(object sender, RoutedEventArgs e)
        {
            Color zolty = new Color();
            zolty.A = 255;
            zolty.R = 241;
            zolty.G = 166;
            zolty.B = 13;
            OdswiezEdycjeKlienta();
            btnWeryfikacja.Foreground = new SolidColorBrush(zolty);
            btnWeryfikacja.Background = Brushes.Transparent;
            btnWeryfikacja.BorderBrush = new SolidColorBrush(zolty);
            btnZamowienia.Foreground = new SolidColorBrush(zolty);
            btnZamowienia.Background = Brushes.Transparent;
            btnZamowienia.BorderBrush = new SolidColorBrush(zolty);
            btnEdycjaZamowienia.Foreground = new SolidColorBrush(zolty);
            btnEdycjaZamowienia.Background = Brushes.Transparent;
            btnEdycjaZamowienia.BorderBrush = new SolidColorBrush(zolty);
            btnEdycja.Background = new SolidColorBrush(zolty);
            btnEdycja.Foreground = Brushes.Black;
            btnEdycja.BorderBrush = Brushes.Black;
            btnPojazdy.Foreground = new SolidColorBrush(zolty);
            btnPojazdy.Background = Brushes.Transparent;
            btnPojazdy.BorderBrush = new SolidColorBrush(zolty);
            btnRaport.Foreground = new SolidColorBrush(zolty);
            btnRaport.Background = Brushes.Transparent;
            btnRaport.BorderBrush = new SolidColorBrush(zolty);
            btnStawkaGodzinowa.Foreground = new SolidColorBrush(zolty);
            btnStawkaGodzinowa.Background = Brushes.Transparent;
            btnStawkaGodzinowa.BorderBrush = new SolidColorBrush(zolty);
        }

        private void btnZamowienia_Click(object sender, RoutedEventArgs e)
        {
            Color zolty = new Color();
            zolty.A = 255;
            zolty.R = 241;
            zolty.G = 166;
            zolty.B = 13;
            OdswiezZamowienia();
            btnWeryfikacja.Foreground = new SolidColorBrush(zolty);
            btnWeryfikacja.Background = Brushes.Transparent;
            btnWeryfikacja.BorderBrush = new SolidColorBrush(zolty);
            btnEdycja.Foreground = new SolidColorBrush(zolty);
            btnEdycja.Background = Brushes.Transparent;
            btnEdycja.BorderBrush = new SolidColorBrush(zolty);
            btnEdycjaZamowienia.Foreground = new SolidColorBrush(zolty);
            btnEdycjaZamowienia.Background = Brushes.Transparent;
            btnEdycjaZamowienia.BorderBrush = new SolidColorBrush(zolty);
            btnZamowienia.Background = new SolidColorBrush(zolty);
            btnZamowienia.Foreground = Brushes.Black;
            btnZamowienia.BorderBrush = Brushes.Black;
            btnPojazdy.Foreground = new SolidColorBrush(zolty);
            btnPojazdy.Background = Brushes.Transparent;
            btnPojazdy.BorderBrush = new SolidColorBrush(zolty);
            btnRaport.Foreground = new SolidColorBrush(zolty);
            btnRaport.Background = Brushes.Transparent;
            btnRaport.BorderBrush = new SolidColorBrush(zolty);
            btnStawkaGodzinowa.Foreground = new SolidColorBrush(zolty);
            btnStawkaGodzinowa.Background = Brushes.Transparent;
            btnStawkaGodzinowa.BorderBrush = new SolidColorBrush(zolty);
        }
        private void btnEdycjaZamowienia_Click(object sender, RoutedEventArgs e)
        {
            Color zolty = new Color();
            zolty.A = 255;
            zolty.R = 241;
            zolty.G = 166;
            zolty.B = 13;
            OdswiezEdycjeZamowienia();
            btnWeryfikacja.Foreground = new SolidColorBrush(zolty);
            btnWeryfikacja.Background = Brushes.Transparent;
            btnWeryfikacja.BorderBrush = new SolidColorBrush(zolty);
            btnEdycja.Foreground = new SolidColorBrush(zolty);
            btnEdycja.Background = Brushes.Transparent;
            btnEdycja.BorderBrush = new SolidColorBrush(zolty);
            btnZamowienia.Foreground = new SolidColorBrush(zolty);
            btnZamowienia.Background = Brushes.Transparent;
            btnZamowienia.BorderBrush = new SolidColorBrush(zolty);
            btnEdycjaZamowienia.Background = new SolidColorBrush(zolty);
            btnEdycjaZamowienia.Foreground = Brushes.Black;
            btnEdycjaZamowienia.BorderBrush = Brushes.Black;
            btnPojazdy.Foreground = new SolidColorBrush(zolty);
            btnPojazdy.Background = Brushes.Transparent;
            btnPojazdy.BorderBrush = new SolidColorBrush(zolty);
            btnRaport.Foreground = new SolidColorBrush(zolty);
            btnRaport.Background = Brushes.Transparent;
            btnRaport.BorderBrush = new SolidColorBrush(zolty);
            btnStawkaGodzinowa.Foreground = new SolidColorBrush(zolty);
            btnStawkaGodzinowa.Background = Brushes.Transparent;
            btnStawkaGodzinowa.BorderBrush = new SolidColorBrush(zolty);
        }

        private void btnPojazdy_Click(object sender, RoutedEventArgs e)
        {
            OdswiezPojazdy();
            wysAutka();
            Color zolty = new Color();
            zolty.A = 255;
            zolty.R = 241;
            zolty.G = 166;
            zolty.B = 13;
            btnWeryfikacja.Foreground = new SolidColorBrush(zolty);
            btnWeryfikacja.Background = Brushes.Transparent;
            btnWeryfikacja.BorderBrush = new SolidColorBrush(zolty);
            btnEdycja.Foreground = new SolidColorBrush(zolty);
            btnEdycja.Background = Brushes.Transparent;
            btnEdycja.BorderBrush = new SolidColorBrush(zolty);
            btnZamowienia.Foreground = new SolidColorBrush(zolty);
            btnZamowienia.Background = Brushes.Transparent;
            btnZamowienia.BorderBrush = new SolidColorBrush(zolty);
            btnEdycjaZamowienia.Background = Brushes.Transparent;
            btnEdycjaZamowienia.Foreground = new SolidColorBrush(zolty);
            btnEdycjaZamowienia.BorderBrush = new SolidColorBrush(zolty);
            btnPojazdy.Foreground = Brushes.Black;
            btnPojazdy.Background = new SolidColorBrush(zolty);
            btnPojazdy.BorderBrush = Brushes.Black;
            btnRaport.Foreground = new SolidColorBrush(zolty);
            btnRaport.Background = Brushes.Transparent;
            btnRaport.BorderBrush = new SolidColorBrush(zolty);
            btnStawkaGodzinowa.Foreground = new SolidColorBrush(zolty);
            btnStawkaGodzinowa.Background = Brushes.Transparent;
            btnStawkaGodzinowa.BorderBrush = new SolidColorBrush(zolty);
            btnDodaPojazd0.Visibility = Visibility.Hidden;

        }

        private void btnRaport_Click(object sender, RoutedEventArgs e)
        {
            OdswiezRaport();
            btnZapiszDoPliku.Visibility = Visibility.Hidden;
            Color zolty = new Color();
            zolty.A = 255;
            zolty.R = 241;
            zolty.G = 166;
            zolty.B = 13;
            btnWeryfikacja.Foreground = new SolidColorBrush(zolty);
            btnWeryfikacja.Background = Brushes.Transparent;
            btnWeryfikacja.BorderBrush = new SolidColorBrush(zolty);
            btnEdycja.Foreground = new SolidColorBrush(zolty);
            btnEdycja.Background = Brushes.Transparent;
            btnEdycja.BorderBrush = new SolidColorBrush(zolty);
            btnZamowienia.Foreground = new SolidColorBrush(zolty);
            btnZamowienia.Background = Brushes.Transparent;
            btnZamowienia.BorderBrush = new SolidColorBrush(zolty);
            btnEdycjaZamowienia.Background = Brushes.Transparent;
            btnEdycjaZamowienia.Foreground = new SolidColorBrush(zolty);
            btnEdycjaZamowienia.BorderBrush = new SolidColorBrush(zolty);
            btnPojazdy.Foreground = new SolidColorBrush(zolty); 
            btnPojazdy.Background = Brushes.Transparent;
            btnPojazdy.BorderBrush = new SolidColorBrush(zolty);
            btnRaport.Foreground = Brushes.Black;
            btnRaport.Background = new SolidColorBrush(zolty);
            btnRaport.BorderBrush = Brushes.Black;
            btnStawkaGodzinowa.Foreground = new SolidColorBrush(zolty);
            btnStawkaGodzinowa.Background = Brushes.Transparent;
            btnStawkaGodzinowa.BorderBrush = new SolidColorBrush(zolty);
        }

        private void btnStawkaGodzinowa_Click(object sender, RoutedEventArgs e)
        {
            OdswiezEdycjeStawkiZamowienia();
            grdZapisanieStawkiGodzinowej.Visibility = Visibility.Hidden;
            grdEdycjaStawkiGodzinowej.Visibility = Visibility.Hidden;
            Color zolty = new Color();
            zolty.A = 255;
            zolty.R = 241;
            zolty.G = 166;
            zolty.B = 13;
            btnStawkaGodzinowa.Foreground = Brushes.Black;
            btnStawkaGodzinowa.Background = new SolidColorBrush(zolty);
            btnStawkaGodzinowa.BorderBrush = Brushes.Black;
            btnWeryfikacja.Foreground = new SolidColorBrush(zolty);
            btnWeryfikacja.Background = Brushes.Transparent;
            btnWeryfikacja.BorderBrush = new SolidColorBrush(zolty);
            btnEdycja.Foreground = new SolidColorBrush(zolty);
            btnEdycja.Background = Brushes.Transparent;
            btnEdycja.BorderBrush = new SolidColorBrush(zolty);
            btnZamowienia.Foreground = new SolidColorBrush(zolty);
            btnZamowienia.Background = Brushes.Transparent;
            btnZamowienia.BorderBrush = new SolidColorBrush(zolty);
            btnEdycjaZamowienia.Background = Brushes.Transparent;
            btnEdycjaZamowienia.Foreground = new SolidColorBrush(zolty);
            btnEdycjaZamowienia.BorderBrush = new SolidColorBrush(zolty);
            btnPojazdy.Foreground = new SolidColorBrush(zolty);
            btnPojazdy.Background = Brushes.Transparent;
            btnPojazdy.BorderBrush = new SolidColorBrush(zolty);
            btnRaport.Foreground = new SolidColorBrush(zolty);
            btnRaport.Background = Brushes.Transparent;
            btnRaport.BorderBrush = new SolidColorBrush(zolty);

        }

        void OdswiezWeryfikacje()
        {
            lbxZamowienia.UnselectAll();
            UkryjWeryfikacjeZamowienia();
            lbxWeryfikacja.UnselectAll();
            UkryjWeryfikacje();
            lbxKlienci.UnselectAll();
            lbxKierowcy.UnselectAll();
            grdKlienci.Visibility = Visibility.Hidden;
            gridEdycji.Visibility = Visibility.Hidden;
            grdWeryfikacja.Visibility = Visibility.Visible;
            grdZamowienie.Visibility = Visibility.Hidden;
            grdEdycjaZamowienia.Visibility = Visibility.Hidden;
            grdPojazd.Visibility = Visibility.Hidden;
            grdRaport.Visibility = Visibility.Hidden;
            UkryjEdycjeZamowienia();
            grdStawkaGodzinowa.Visibility = Visibility.Hidden;
            lbxKierowcyStawkaGodzinowa.UnselectAll();
        }
        void OdswiezEdycjeKlienta()
        {
            lbxZamowienia.UnselectAll();
            UkryjWeryfikacjeZamowienia();
            lbxWeryfikacja.UnselectAll();
            UkryjWeryfikacje();
            lbxKlienci.UnselectAll();
            lbxKierowcy.UnselectAll();
            grdKlienci.Visibility = Visibility.Visible;
            gridEdycji.Visibility = Visibility.Hidden;
            grdWeryfikacja.Visibility = Visibility.Hidden;
            grdZamowienie.Visibility = Visibility.Hidden;
            grdEdycjaZamowienia.Visibility = Visibility.Hidden;
            grdPojazd.Visibility = Visibility.Hidden;
            grdRaport.Visibility = Visibility.Hidden;
            UkryjEdycjeZamowienia();
            grdStawkaGodzinowa.Visibility = Visibility.Hidden;
            lbxKierowcyStawkaGodzinowa.UnselectAll();
        }
        void OdswiezPojazdy()
        {
            OdswiezListeModeli();
            lbxZamowienia.UnselectAll();
            UkryjWeryfikacjeZamowienia();
            lbxWeryfikacja.UnselectAll();
            UkryjWeryfikacje();
            UkryjEdycjeZamowienia();
            lbxKlienci.UnselectAll();
            lbxKierowcy.UnselectAll();
            grdKlienci.Visibility = Visibility.Hidden;
            gridEdycji.Visibility = Visibility.Hidden;
            grdWeryfikacja.Visibility = Visibility.Hidden;
            grdZamowienie.Visibility = Visibility.Hidden;
            grdEdycjaZamowienia.Visibility = Visibility.Hidden;
            grdPojazd.Visibility = Visibility.Visible;
            UkryjEdycjeZamowienia();
            grdRaport.Visibility = Visibility.Hidden;
            grdStawkaGodzinowa.Visibility = Visibility.Hidden;
            lbxKierowcyStawkaGodzinowa.UnselectAll();
        }

        void OdswiezRaport()
        {
            lbxZamowienia.UnselectAll();
            UkryjWeryfikacjeZamowienia();
            lbxWeryfikacja.UnselectAll();
            UkryjWeryfikacje();
            UkryjEdycjeZamowienia();
            lbxKlienci.UnselectAll();
            lbxKierowcy.UnselectAll();
            grdKlienci.Visibility = Visibility.Hidden;
            gridEdycji.Visibility = Visibility.Hidden;
            grdWeryfikacja.Visibility = Visibility.Hidden;
            grdZamowienie.Visibility = Visibility.Hidden;
            grdEdycjaZamowienia.Visibility = Visibility.Hidden;
            grdPojazd.Visibility = Visibility.Hidden;
            UkryjEdycjeZamowienia();
            grdRaport.Visibility = Visibility.Visible;
            btnZapiszDoPliku.Visibility = Visibility.Hidden;
            grdStawkaGodzinowa.Visibility = Visibility.Hidden;
            lbxKierowcyStawkaGodzinowa.UnselectAll();
        }

        void OdswiezZamowienia()
        {
            lbxZamowienia.UnselectAll();
            UkryjWeryfikacjeZamowienia();
            lbxWeryfikacja.UnselectAll();
            UkryjWeryfikacje();
            UkryjEdycjeZamowienia();
            lbxKlienci.UnselectAll();
            lbxKierowcy.UnselectAll();
            grdKlienci.Visibility = Visibility.Hidden;
            gridEdycji.Visibility = Visibility.Hidden;
            grdWeryfikacja.Visibility = Visibility.Hidden;
            grdZamowienie.Visibility = Visibility.Visible;
            grdEdycjaZamowienia.Visibility = Visibility.Hidden;
            grdPojazd.Visibility = Visibility.Hidden;
            grdRaport.Visibility = Visibility.Hidden;
            UkryjEdycjeZamowienia();
            grdStawkaGodzinowa.Visibility = Visibility.Hidden;
            lbxKierowcyStawkaGodzinowa.UnselectAll();
        }
        void OdswiezEdycjeZamowienia()
        {
            lbxZamowienia.UnselectAll();
            UkryjWeryfikacjeZamowienia();
            lbxWeryfikacja.UnselectAll();
            UkryjWeryfikacje();
            lbxKlienci.UnselectAll();
            lbxKierowcy.UnselectAll();
            grdKlienci.Visibility = Visibility.Hidden;
            gridEdycji.Visibility = Visibility.Hidden;
            grdWeryfikacja.Visibility = Visibility.Hidden;
            grdZamowienie.Visibility = Visibility.Hidden;
            grdEdycjaZamowienia.Visibility = Visibility.Visible;
            grdPojazd.Visibility = Visibility.Hidden;
            grdRaport.Visibility = Visibility.Hidden;
            UkryjEdycjeZamowienia();
            grdStawkaGodzinowa.Visibility = Visibility.Hidden;
            lbxKierowcyStawkaGodzinowa.UnselectAll();
        }

        private void lbxZamowienia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lblBladGodzina.Visibility = Visibility.Hidden;
            txtGodziny.Text = "";
            lblOpisZamowienia.Content = "";
            txtKomentarz.Text = "";
            PokazWeryfikacjeZamowienia();
            zamowienia zamowienie = new zamowienia();
            zamowienie = lbxZamowienia.SelectedItem as zamowienia;

            if (zamowienie != null)
            {
                if (zamowienie.status_zamowienia == "Oczekuje")
                {
                    lblIle.Visibility = Visibility.Visible;
                    txtGodziny.Visibility = Visibility.Visible;
                    txtKomentarz.Visibility = Visibility.Visible;
                    lblKomentarzNapis.Visibility = Visibility.Visible;
                    CzyZwerGrid.Visibility = Visibility.Visible;
                }
                else
                {
                    lblIle.Visibility = Visibility.Hidden;
                    txtGodziny.Visibility = Visibility.Hidden;
                    txtKomentarz.Visibility = Visibility.Hidden;
                    lblKomentarzNapis.Visibility = Visibility.Hidden;
                    CzyZwerGrid.Visibility = Visibility.Hidden;
                }
            }
            MySqlConnection con = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;");
            con.Open();
            if (zamowienie != null)
            {
                MySqlCommand comKierowcy = new MySqlCommand($"SELECT * FROM kierowcy where idKierowcy={Convert.ToInt32(zamowienie.idKierowcy)}", con);
                MySqlCommand comPojazdy = new MySqlCommand($"SELECT * FROM pojazdy where idPojazdy={Convert.ToInt32(zamowienie.idPojazdy)}", con);
                MySqlCommand comKlienci = new MySqlCommand($"SELECT * FROM klienci where idKlienci={Convert.ToInt32(zamowienie.idKlienci)}", con);
                MySqlCommand comKonta = new MySqlCommand($"SELECT * FROM konta where idKonta={Convert.ToInt32(zamowienie.idKonta)}", con);
                MySqlCommand comSzczegoly = new MySqlCommand($"SELECT * FROM szczegoly_zamowienia where idSzczegoly_zamowienia={Convert.ToInt32(zamowienie.idSzczegoly_zamowienia)}", con);
                MySqlDataReader czytnikSzczegoly = comSzczegoly.ExecuteReader();
                string cena;
                string km;
                if (czytnikSzczegoly.Read())
                {
                    if (DBNull.Value.Equals(czytnikSzczegoly["cena"]))
                    {
                        cena = "Brak";
                    }
                    else
                    {
                        cena = Convert.ToString(czytnikSzczegoly["cena"]);
                    }
                    if (DBNull.Value.Equals(czytnikSzczegoly["ilosc_km"]))
                    {
                        km = "Brak";
                    }
                    else
                    {
                        km = Convert.ToString(czytnikSzczegoly["ilosc_km"]);
                    }
                    lblOpisZamowienia.Content = $"Data złożenia zamówienia: {zamowienie.data_zlozenia_zamowienia} \nData początku: {zamowienie.data_poczatku} \n" +
                        $"Data Końca: {zamowienie.data_konca}\nStatus zamówienia: {zamowienie.status_zamowienia}\n\nAdres początkowy:\nPaństwo: {czytnikSzczegoly["poczatek_panstwo"]}" +
                        $"\nMiasto: {czytnikSzczegoly["poczatek_miasto"]}\nUlica: {czytnikSzczegoly["poczatek_ulica"]}\nNr domu: {czytnikSzczegoly["poczatek_nr_domu"]}" +
                        $"\nKod pocztowy: {czytnikSzczegoly["poczatek_kod_pocztowy"]}\n\nAdres docelowy:\nPaństwo: {czytnikSzczegoly["koniec_panstwo"]}\nMiasto: {czytnikSzczegoly["koniec_miasto"]}\n" +
                        $"Ulica: {czytnikSzczegoly["koniec_ulica"]}\nNr domu: {czytnikSzczegoly["koniec_nr_domu"]}\nKod pocztowy: {czytnikSzczegoly["koniec_kod_pocztowy"]}\n\nIlość kilometrów: {km}\n" +
                        $"Cena: {cena}\nKomentarz: {zamowienie.komentarz}\n";
                }
                czytnikSzczegoly.Close();
                MySqlDataReader czytnikKlienci = comKlienci.ExecuteReader();
                if (czytnikKlienci.Read())
                {
                    lblOpisZamowieniaKlient.Content = $"Imię i Nazwisko: {czytnikKlienci["imie_klienta"]} {czytnikKlienci["nazwisko_klienta"]}\nNumer telefonu: {czytnikKlienci["numer_tel_klienta"]}\n";
                }
                czytnikKlienci.Close();
                MySqlDataReader czytnikKonta = comKonta.ExecuteReader();
                if (czytnikKonta.Read())
                {
                    lblOpisZamowieniaKonto.Content = $"E-mail: {czytnikKonta["email_klienta"]} \nLogin: {czytnikKonta["login"]}\nHasło: {czytnikKonta["haslo"]}";
                }
                czytnikKonta.Close();
                MySqlDataReader czytnikKierowcy = comKierowcy.ExecuteReader();
                if (czytnikKierowcy.Read())
                {
                    lblOpisZamowieniaKierowca.Content = $"Kierowca: \nImię i Nazwisko: {czytnikKierowcy["imie_prac"]} {czytnikKierowcy["nazwisko_prac"]}\nNumer telefonu: {czytnikKierowcy["numer_tel_kierowcy"]}\n";
                }
                czytnikKierowcy.Close();
                MySqlDataReader czytnikPojazdy = comPojazdy.ExecuteReader();
                if (czytnikPojazdy.Read())
                {
                    lblOpisZamowieniaPojazd.Content = $"Pojazd:\nNr rejestracji: {czytnikPojazdy["nr_rejestracji"]}\nStatus pojazdu: {czytnikPojazdy["dostepnosc"]}\n";
                }
                czytnikPojazdy.Close();
            }
            con.Close();
        }
        void PokazWeryfikacjeZamowienia()
        {
            btnTakZamowienia.Visibility = Visibility.Visible;
            btnNieZamowienia.Visibility = Visibility.Visible;
            lblWeryfikacjaZamowienia.Visibility = Visibility.Visible;
            lblOpisZamowienia.Visibility = Visibility.Visible;
            lblOpisZamowieniaKlient.Visibility = Visibility.Visible;
            lblOpisZamowieniaKonto.Visibility = Visibility.Visible;
            lblOpisZamowieniaKierowca.Visibility = Visibility.Visible;
            lblOpisZamowieniaPojazd.Visibility = Visibility.Visible;
            txtKomentarz.Visibility = Visibility.Visible;
            lblKomentarzNapis.Visibility = Visibility.Visible;
            lblIle.Visibility = Visibility.Visible;
            txtGodziny.Visibility = Visibility.Visible;
            CzyZwerGrid.Visibility = Visibility.Visible;
            OpisGrid.Visibility = Visibility.Visible;
        }
        void UkryjWeryfikacjeZamowienia()
        {
            btnTakZamowienia.Visibility = Visibility.Hidden;
            btnNieZamowienia.Visibility = Visibility.Hidden;
            lblWeryfikacjaZamowienia.Visibility = Visibility.Hidden;
            lblOpisZamowienia.Visibility = Visibility.Hidden;
            lblOpisZamowieniaKlient.Visibility = Visibility.Hidden;
            lblOpisZamowieniaKonto.Visibility = Visibility.Hidden;
            lblOpisZamowieniaKierowca.Visibility = Visibility.Hidden;
            lblOpisZamowieniaPojazd.Visibility = Visibility.Hidden;
            txtKomentarz.Visibility = Visibility.Hidden;
            lblKomentarzNapis.Visibility = Visibility.Hidden;
            lblIle.Visibility = Visibility.Hidden;
            txtGodziny.Visibility = Visibility.Hidden;
            lblBladGodzina.Visibility = Visibility.Hidden;
            CzyZwerGrid.Visibility = Visibility.Hidden;
            OpisGrid.Visibility = Visibility.Hidden;
        }
        const string godzinki = @"^[0-9]*$";
        private void btnTakZamowienia_Click(object sender, RoutedEventArgs e)
        {
            int czas = 0;
            zamowienia zamowienie = new zamowienia();
            zamowienie = lbxZamowienia.SelectedItem as zamowienia;
            MySqlConnection con = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;");
            con.Open();
            MySqlCommand comDodaj = new MySqlCommand("update zamowienia set status_zamowienia=@status, komentarz=@komentarz, przewidywany_czas=@godzina where idZamowienia=@idZamowienia", con);
            if (zamowienie.status_zamowienia == "Oczekuje")
            {
                if (Regex.IsMatch(txtGodziny.Text, godzinki) && !String.IsNullOrEmpty(txtGodziny.Text)) czas += Convert.ToInt32(txtGodziny.Text);
                else lblBladGodzina.Visibility = Visibility.Visible;
                if (czas > 0)
                {
                    comDodaj.Parameters.AddWithValue("@status", "Zatwierdzony");
                    comDodaj.Parameters.AddWithValue("@idZamowienia", Convert.ToInt32(zamowienie.idZamowienia));
                    comDodaj.Parameters.AddWithValue("@komentarz", txtKomentarz.Text);
                    comDodaj.Parameters.AddWithValue("@godzina", czas);
                    comDodaj.ExecuteScalar();
                    con.Close();
                    Inicjalizacja();
                }

            }
            
        }


        private void btnNieZamowienia_Click(object sender, RoutedEventArgs e)
        {
            zamowienia zamowienie = new zamowienia();
            zamowienie = lbxZamowienia.SelectedItem as zamowienia;
            MySqlConnection con = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;");
            con.Open();
            MySqlCommand comDodaj = new MySqlCommand("update zamowienia set status_zamowienia=@status, komentarz=@komentarz where idZamowienia=@idZamowienia", con);
            if (zamowienie.status_zamowienia == "Oczekuje")
            {
                comDodaj.Parameters.AddWithValue("@status", "Anulowany");
                comDodaj.Parameters.AddWithValue("@idZamowienia", Convert.ToInt32(zamowienie.idZamowienia));
                comDodaj.Parameters.AddWithValue("@komentarz", Convert.ToString(txtKomentarz.Text));
                comDodaj.ExecuteScalar();
            }
            con.Close();
            Inicjalizacja();
        }
        int idZam = 1;
        int idKier = 1;
        private void lbxZamowieniaEdycja_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lbxKierowcaZamowienia.Visibility = Visibility.Visible;
            zamowienia zamowienie = new zamowienia();
            zamowienie = lbxZamowieniaEdycja.SelectedItem as zamowienia;
            if (zamowienie != null)
            {
                idZam = zamowienie.idZamowienia;

                DateTime data = Convert.ToDateTime(zamowienie.zamowiona_data_realizacji);
                mydbEntities baza = new mydbEntities();
                List<kierowcy> lista_kierowcow = new List<kierowcy>();
                var kierowcy = baza.kierowcy.Where(t => t.idKierowcy != 1 && t.idKierowcy != zamowienie.idKierowcy);
                foreach (var x in kierowcy)
                {
                    lista_kierowcow.Add(x);
                }
                List<int> ktorzy_kierowcy = new List<int>();
                for (int i = 0; i < lista_kierowcow.Count(); i++)
                {
                    List<zamowienia> lista_zamowien = new List<zamowienia>();
                    int id = lista_kierowcow[i].idKierowcy;
                    var zamowienia = baza.zamowienia.Where(t => t.idKierowcy == id);
                    foreach (zamowienia x in zamowienia)
                    {
                        lista_zamowien.Add(x);
                    }
                    for (int j = 0; j < lista_zamowien.Count(); j++)
                    {
                        DateTime data_pom = Convert.ToDateTime(lista_zamowien[j].zamowiona_data_realizacji);
                        if (data_pom.Date == data.Date)
                        {
                            ktorzy_kierowcy.Add(lista_kierowcow[i].idKierowcy);
                        }
                    }
                }
                for (int i = 0; i < ktorzy_kierowcy.Count(); i++)
                {
                    lista_kierowcow.Remove(lista_kierowcow.SingleOrDefault(x => x.idKierowcy == ktorzy_kierowcy[i]));
                }
                lbxKierowcaZamowienia.Items.Clear();
                foreach (kierowcy k in lista_kierowcow)
                {
                    lbxKierowcaZamowienia.Items.Add(k);
                }
            }

        }

        private void lbxKierowcaZamowienia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PokazEdycjeZamowienia();
            kierowcy kierowca = new kierowcy();
            kierowca = lbxKierowcaZamowienia.SelectedItem as kierowcy;
            if (kierowca != null)
            {
                idKier = kierowca.idKierowcy;
            }
        }

        private void btnTakPrzypisanie_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnection con = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;");
            con.Open();
            MySqlCommand comDodaj = new MySqlCommand("update zamowienia set idKierowcy=@idKier where idZamowienia=@idZamowienia", con);
            comDodaj.Parameters.AddWithValue("@idKier", idKier);
            comDodaj.Parameters.AddWithValue("@idZamowienia", idZam);
            comDodaj.ExecuteScalar();
            con.Close();
            Inicjalizacja();
        }

        private void btnNiePrzypisanie_Click(object sender, RoutedEventArgs e)
        {
            lbxKierowcaZamowienia.UnselectAll();
            lbxZamowieniaEdycja.UnselectAll();
            UkryjEdycjeZamowienia();
        }

        void OdswiezListeModeli()
        {
            lbxListaModeli.Items.Clear();
            mydbEntities baza = new mydbEntities();
            var wszystkie_modele = baza.model_samochodu;
            foreach (var model in wszystkie_modele)
            {
                lbxListaModeli.Items.Add(model);
            }
        }

        int currentId = 2;
        List<string> listaZdjec = new List<string>();
        List<string> listaNazw = new List<string>();
        List<double> listaCen = new List<double>();
        public void wysAutka()
        {
            
            MySqlConnection con = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;");
            con.Open();
            model_samochodu samochod = new model_samochodu();
            MySqlCommand ZdjAutka = new MySqlCommand($"SELECT zdj_model, cena_za_km, nazwa_model from model_samochodu m join pojazdy p on m.idModel=p.idModel where p.dostepnosc = 1 group by m.nazwa_model;", con);
            MySqlDataReader czytnikZdjec = ZdjAutka.ExecuteReader();
            while (czytnikZdjec.Read())
            {
                listaZdjec.Add(Convert.ToString(czytnikZdjec["zdj_model"]));
                listaNazw.Add(Convert.ToString(czytnikZdjec["nazwa_model"]));
                listaCen.Add(Convert.ToDouble(czytnikZdjec["cena_za_km"]));
            }

            imgLeft.Source = new BitmapImage(new Uri($"zdjecia/{listaZdjec[currentId - 1]}", UriKind.Relative));
            imgCenter.Source = new BitmapImage(new Uri($"zdjecia/{listaZdjec[currentId]}", UriKind.Relative));
            imgRight.Source = new BitmapImage(new Uri($"zdjecia/{listaZdjec[currentId + 1]}", UriKind.Relative));

            lblTyp.Content = $"Model pojazdu: {listaNazw[currentId]}, Cena za km: {listaCen[currentId].ToString("C")}";

            con.Close();
        }


        private void imgLeft_MouseDown(object sender, MouseButtonEventArgs e)
        {
            currentId--;
            if ((currentId - 1) < 0) currentId = (listaZdjec.Count() - 1);
            wysAutka();
        }

        private void imgRight_MouseDown(object sender, MouseButtonEventArgs e)
        {
            currentId++;
            if ((currentId + 1) > listaZdjec.Count()) currentId = 0;
            wysAutka();
        }



        model_samochodu model = new model_samochodu();

        private void lbxListaModeli_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            grdSzczegolyPojazdu.Visibility = Visibility.Visible;
            grdEdycjaPojazdu.Visibility = Visibility.Hidden;
            lblNrRejestracji.Content = null;
            lblStatusPojazdu.Content = null;
            txtNrRejestracji.Visibility = Visibility.Hidden;
            grdEdycjaPojazdu.Visibility = Visibility.Hidden;
            grdDodaniePojazdu.Visibility = Visibility.Hidden;
            grdDodanieModelu.Visibility = Visibility.Hidden;
            if (lbxListaModeli.SelectedItem != null)
            {
                lbxListaPojazdow.Items.Clear();
                mydbEntities baza = new mydbEntities();
                model = lbxListaModeli.SelectedItem as model_samochodu;
                var wszystkie_pojazdy = baza.pojazdy.Where(t => t.idModel == model.idModel);
                foreach (var pojazd in wszystkie_pojazdy)
                {
                    if (pojazd.dostepnosc == 1)
                    {
                        lbxListaPojazdow.Items.Add(pojazd);
                    }
                }
                if (lbxListaPojazdow.Items.Count < 1)
                {
                    btnDodaPojazd0.Visibility = Visibility.Visible;
                }
                else
                {
                    btnDodaPojazd0.Visibility = Visibility.Hidden;
                }
                lblNazwaModelu.Content = "Model: " + model.nazwa_model;
                imgPojazd.Source = new BitmapImage(new Uri($"zdjecia/{model.zdj_model}", UriKind.Relative));
                lblCena.Content = "Cena za km: " + model.cena_za_km + "zł";
               
            }
        }

        private void lbxListaPojazdow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            grdSzczegolyPojazdu.Visibility = Visibility.Visible;
            grdEdycjaPojazdu.Visibility = Visibility.Hidden;
            grdDodaniePojazdu.Visibility = Visibility.Hidden;
            grdDodanieModelu.Visibility = Visibility.Hidden;
            if (lbxListaPojazdow.SelectedItem != null)
            {
                lbxListaModeli.UnselectAll();
                pojazdy pojazd = new pojazdy();
                pojazd = lbxListaPojazdow.SelectedItem as pojazdy;
                lblNrRejestracji.Content = "Nr. Rejestracji: " + pojazd.nr_rejestracji;
                lblStatusPojazdu.Content = "Status: " + pojazd.status_pojazdu;
                lblNrRejestracji.Content = "Nr. Rejestracji: " + pojazd.nr_rejestracji;
                lblStatusPojazdu.Content = "Status: " + pojazd.status_pojazdu;
            }
        }

        private void btnEdytujPojazd_Click(object sender, RoutedEventArgs e)
        {
            if (lbxListaModeli.SelectedItem != null)
            {
                lblBladCenaE.Content = null;
                btnDodaPojazd0.Visibility = Visibility.Hidden;
                btnAnuluj.Visibility = Visibility.Visible;
                btnZapisz.Visibility = Visibility.Visible;
                btnDodajZdjecie.Visibility = Visibility.Visible;
                grdSzczegolyPojazdu.Visibility = Visibility.Hidden;
                grdEdycjaPojazdu.Visibility = Visibility.Visible;
                grdDodaniePojazdu.Visibility = Visibility.Hidden;
                grdDodanieModelu.Visibility = Visibility.Hidden;
                txtCena.Text = model.cena_za_km.ToString();
                txtNazwaModelu.Text = model.nazwa_model;
                imgPojazdEdycja.Source = new BitmapImage(new Uri($"zdjecia/{model.zdj_model}", UriKind.Relative));
            }
            else if (lbxListaPojazdow.SelectedItem != null)
            {
                lblBladCenaE.Content = null;
                btnDodaPojazd0.Visibility = Visibility.Hidden;
                btnAnuluj.Visibility = Visibility.Visible;
                btnZapisz.Visibility = Visibility.Visible;
                btnDodajZdjecie.Visibility = Visibility.Visible;
                grdSzczegolyPojazdu.Visibility = Visibility.Hidden;
                grdEdycjaPojazdu.Visibility = Visibility.Visible;
                grdDodaniePojazdu.Visibility = Visibility.Hidden;
                grdDodanieModelu.Visibility = Visibility.Hidden;
                txtCena.Text = model.cena_za_km.ToString();
                txtNazwaModelu.Text = model.nazwa_model;
                imgPojazdEdycja.Source = new BitmapImage(new Uri($"zdjecia/{model.zdj_model}", UriKind.Relative));
                txtNrRejestracji.Visibility = Visibility.Visible;
                pojazdy pojazd = new pojazdy();
                pojazd = lbxListaPojazdow.SelectedItem as pojazdy;
                txtNrRejestracji.Text = pojazd.nr_rejestracji.ToString();
            }
        }

        //Sciezka do zdjecia zeby nie kombinowac duzo z przekazaniem jego nazwy
        string image_path;


        private void btnDodajZdjecie_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png;)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
            if (open.ShowDialog() == true)
            {
                imgPojazdEdycja.Source = new BitmapImage(new Uri($"zdjecia/{open.SafeFileName}", UriKind.Relative));
                imgPojazdDodanie.Source = new BitmapImage(new Uri($"zdjecia/{open.SafeFileName}", UriKind.Relative));
                image_path = open.SafeFileName;
            }
        }

        const string Rejestracja = @"^[A-Z0-9]+$";
        const string Cena = @"^[0-9]*(\,[0-9]{0,2})?$";

        private void btnZapisz_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lblBladCenaE.Content = null;
                if (Regex.IsMatch(txtCena.Text, Cena))
                {
                    mydbEntities baza = new mydbEntities();
                    if (lbxListaPojazdow.SelectedItem != null)
                    {
                        if (Regex.IsMatch(txtNrRejestracji.Text, Rejestracja))
                        {
                            pojazdy pojazd = new pojazdy();
                            pojazd = lbxListaPojazdow.SelectedItem as pojazdy;
                            var edytowany_pojazd = baza.pojazdy.First(t => t.idPojazdy == pojazd.idPojazdy);
                            edytowany_pojazd.nr_rejestracji = txtNrRejestracji.Text;
                        }
                        else
                        {
                            lblBladRejestracjaPojazduE.Content = "Niepoprawna Rejestracja";
                        }
                    }
                    var edytowany_model = baza.model_samochodu.First(t => t.idModel == model.idModel);
                    edytowany_model.nazwa_model = txtNazwaModelu.Text;


                    edytowany_model.cena_za_km = Convert.ToDouble(txtCena.Text);
                    if (image_path != null)
                    {
                        edytowany_model.zdj_model = image_path;
                    }
                    if(String.IsNullOrEmpty(txtNazwaModelu.Text) || String.IsNullOrEmpty(txtCena.Text) || String.IsNullOrEmpty(image_path))
                    {
                        throw new ArgumentNullException();
                    }
                    baza.SaveChanges();
                    OdswiezPojazdy();
                    btnDodajZdjecie.Visibility = Visibility.Hidden;
                    txtNrRejestracji.Visibility = Visibility.Hidden;
                    grdEdycjaPojazdu.Visibility = Visibility.Hidden;
                    listaNazw.Clear();
                    listaZdjec.Clear();
                    listaCen.Clear();
                    currentId = 2;
                    wysAutka();
                }
                else
                {
                    lblBladCenaE.Content = "Niepoprawna Cena";
                }
            }
            catch(ArgumentNullException) {
                MessageBox.Show("Pola nie mogą być puste!");
            }
            OdswiezPojazdy();
        }

        private void btnDodajPojazd_Click(object sender, RoutedEventArgs e)
        {
            lblBladCenaM.Content = null;
            grdSzczegolyPojazdu.Visibility = Visibility.Hidden;
            grdEdycjaPojazdu.Visibility = Visibility.Hidden;
            btnZapisz.Visibility = Visibility.Hidden;
            if (lbxListaModeli.SelectedItem != null && lbxListaPojazdow.SelectedItem == null)
            {
                grdDodanieModelu.Visibility = Visibility.Visible;
            }
            if (lbxListaModeli.SelectedItem == null && lbxListaPojazdow.SelectedItem != null)
            {
                grdDodaniePojazdu.Visibility = Visibility.Visible;
            }
            btnDodaPojazd0.Visibility = Visibility.Hidden;
        }

        private void btnZapiszP_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lblBladRejestracjaP.Content = null;
                if (Regex.IsMatch(txtNrRejestracjiDodanieP.Text, Rejestracja))
                {
                    mydbEntities baza = new mydbEntities();
                    pojazdy pojazd = new pojazdy();
                    pojazd.dostepnosc = 1;
                    pojazd.idModel = model.idModel;
                    pojazd.nr_rejestracji = txtNrRejestracjiDodanieP.Text;
                    pojazd.status_pojazdu = "Wolny";
                    baza.pojazdy.Add(pojazd);
                    baza.SaveChanges();
                    grdDodaniePojazdu.Visibility = Visibility.Hidden;
                    btnDodaPojazd0.Visibility = Visibility.Hidden;
                    if (String.IsNullOrEmpty(txtNrRejestracjiDodanieP.Text))
                    {
                        throw new ArgumentNullException();
                    }
                }
                else
                {
                    lblBladRejestracjaP.Content = "Niepoprawna Rejestracja";
                }
                OdswiezPojazdy();
            }
            catch (ArgumentException)
            {
                lblBladRejestracjaP.Content = "Pola nie moga byc puste";
            }
        }

        private void btnZapiszM_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lblBladCenaM.Content = null;
                lblBladRejestracjaPojazduM.Content = null;
                if (Regex.IsMatch(txtNrRejestracjiDodanieM.Text, Rejestracja))
                {
                    if (Regex.IsMatch(txtCenaDodanie.Text, Cena))
                    {
                        mydbEntities baza = new mydbEntities();
                        model_samochodu model = new model_samochodu();
                        model.nazwa_model = txtNazwaModeluDodanie.Text;
                        model.zdj_model = image_path;
                        model.cena_za_km = Convert.ToDouble(txtCenaDodanie.Text);
                        grdDodanieModelu.Visibility = Visibility.Hidden;
                        baza.model_samochodu.Add(model);

                        pojazdy pojazd = new pojazdy();
                        pojazd.dostepnosc = 1;
                        pojazd.idModel = model.idModel;
                        pojazd.nr_rejestracji = txtNrRejestracjiDodanieM.Text;
                        pojazd.status_pojazdu = "Wolny";
                        if (String.IsNullOrEmpty(txtNrRejestracjiDodanieM.Text) || String.IsNullOrEmpty(image_path) || String.IsNullOrEmpty(txtNazwaModeluDodanie.Text) || String.IsNullOrEmpty(txtCenaDodanie.Text))
                        {
                            throw new ArgumentNullException();
                        }
                        else
                        {
                            baza.pojazdy.Add(pojazd);
                            baza.SaveChanges();
                            OdswiezPojazdy();
                        }
                    }
                    else
                    {
                        lblBladCenaM.Content = "Niepoprawna Cena";
                    }
                    OdswiezPojazdy();
                }
                else
                {
                    lblBladRejestracjaPojazduM.Content = "Niepoprawna Rejestracja";
                }
                listaNazw.Clear();
                listaZdjec.Clear();
                listaCen.Clear();
                currentId = 2;
                wysAutka();
            }
            catch (ArgumentException)
            {
                lblBladRejestracjaPojazduM.Content="Pola nie moga byc puste";
            }
        }

        private void btnUsunPojazd_Click(object sender, RoutedEventArgs e)
        {
            if (lbxListaPojazdow.SelectedItem != null)
            {
                mydbEntities baza = new mydbEntities();
                pojazdy p = new pojazdy();
                pojazdy p_z_bazy = new pojazdy();
                p = lbxListaPojazdow.SelectedItem as pojazdy;
                p_z_bazy = baza.pojazdy.First(t => t.idPojazdy == p.idPojazdy);
                p_z_bazy.dostepnosc = 0;
                baza.SaveChanges();
                lbxListaPojazdow.UnselectAll();
                lbxListaPojazdow.Items.Clear();
                grdSzczegolyPojazdu.Visibility = Visibility.Hidden;
                OdswiezPojazdy();
            }
            btnDodaPojazd0.Visibility = Visibility.Hidden;
        }

        private void btnGenerujRaport_Click(object sender, RoutedEventArgs e)
        {
            mydbEntities baza = new mydbEntities();
            kierowcy kierowca = new kierowcy();
            int ilosc_zrealizowanych_zamowien = baza.zamowienia.Count(t => t.data_konca >= dtpOdKiedy.SelectedDate && t.data_konca <= dtpDoKiedy.SelectedDate);
            var zamowienia_z_zakresu = baza.zamowienia.Where(t => t.data_konca >= dtpOdKiedy.SelectedDate && t.data_konca <= dtpDoKiedy.SelectedDate);
            double laczna_kwota = 0;
            List<int> zamowienia_z_zakresu_tab = new List<int>();
            List<int> kierowcy_z_zakresu_tab = new List<int>();
            foreach (var zamowienie in zamowienia_z_zakresu)
            {
                zamowienia_z_zakresu_tab.Add(zamowienie.idSzczegoly_zamowienia);
            }

            for (int i = 0; i < zamowienia_z_zakresu_tab.Count(); i++)
            {
                int id = zamowienia_z_zakresu_tab[i];
                var szczegoly_zamowien = baza.szczegoly_zamowienia.First(t => t.idSzczegoly_zamowienia == id);
                laczna_kwota += Convert.ToDouble(szczegoly_zamowien.cena);
            }
            string tresc_raportu = $"RAPORT\n„DOJAZD” sp. z o.o.\n\nOd {dtpOdKiedy} do {dtpDoKiedy}\nIlosc zrealizowanych zamowien: {ilosc_zrealizowanych_zamowien}\nLaczna kwota: {laczna_kwota} zł\n\nLista kierowcow:";
            var wszyscy_kierowcy = baza.kierowcy.Where(t => t.czy_aktywne == 1 && t.idKierowcy > 1);
            List<kierowcy> kierowcyALL = new List<kierowcy>();
            foreach (var kierowcy in wszyscy_kierowcy)
            {
                kierowcyALL.Add(kierowcy);
            }
            for (int i = 0; i < kierowcyALL.Count(); i++)
            {
                int id = kierowcyALL[i].idKierowcy;
                var zamowienia = baza.zamowienia.Where(t => t.idKierowcy == id && t.data_konca >= dtpOdKiedy.SelectedDate && t.data_konca <= dtpDoKiedy.SelectedDate);
                List<zamowienia> zamowienia1 = new List<zamowienia>();
                foreach (var zamowienie in zamowienia)
                {
                    zamowienia1.Add(zamowienie);
                }

                var pensje = baza.pensja.Where(t => t.idKierowcy == id && t.miesiac >= dtpOdKiedy.SelectedDate && t.miesiac <= dtpDoKiedy.SelectedDate);
                List<pensja> pensja1 = new List<pensja>();
                foreach (var pensja in pensje)
                {
                    pensja1.Add(pensja);
                }
                tresc_raportu += $"\n\nImie i Nazwisko: {kierowcyALL[i].imie_prac} {kierowcyALL[i].nazwisko_prac}";
                int godziny = 0;
                for (int j = 0; j < zamowienia1.Count(); j++)
                {
                    if (zamowienia1[j].przewidywany_czas != null) godziny += Convert.ToInt32(zamowienia1[j].przewidywany_czas);
                }
                tresc_raportu += $"\nIlosc przepracowanych godzin: {godziny} h";
                double sumaPensja = 0;
                for (int j = 0; j < zamowienia1.Count(); j++)
                {
                    for (int k = 0; k < pensja1.Count(); k++)
                    {
                        if (Convert.ToDateTime(zamowienia1[j].data_konca).Month == Convert.ToDateTime(pensja1[k].miesiac).Month)
                        {
                            if (pensja1[k].stawka_godz != null && zamowienia1[j] != null) sumaPensja += godziny * Convert.ToInt32(pensja1[k].stawka_godz);
                        }
                    }

                }
                tresc_raportu += $"\nLaczna ilosc zamowien: {zamowienia1.Count()}\nLaczna kwota: {sumaPensja} zl";
            }
            txtPodglad.Text = tresc_raportu;
            btnZapiszDoPliku.Visibility = Visibility.Visible;
            Color zolty = new Color();
            zolty.A = 255;
            zolty.R = 241;
            zolty.G = 166;
            zolty.B = 13;
            btnGenerujRaport.Foreground = new SolidColorBrush(zolty);
            btnGenerujRaport.Background = Brushes.Transparent;
            btnGenerujRaport.BorderBrush = new SolidColorBrush(zolty);
        }

       

        private void btnZapiszDoPliku_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            System.Windows.Forms.DialogResult messageResult = System.Windows.Forms.MessageBox.Show("Zapisać plik jako PDF?", "Plik PDF", System.Windows.Forms.MessageBoxButtons.OKCancel);

            if (messageResult == System.Windows.Forms.DialogResult.Cancel)
            {
                System.Windows.Forms.MessageBox.Show("Operacja przerwana przez użytkownika", "Plik PDF");
            }
            else
            {
                sfd.Title = "Zapisz jako PDF";
                sfd.Filter = "(*.pdf)|*.pdf";
                sfd.InitialDirectory = @"C:\";


                if (sfd.ShowDialog() == Convert.ToBoolean(System.Windows.Forms.DialogResult.OK))
                {
                    if (txtPodglad.Text != "")
                    {
                        iTextSharp.text.Document doc = new iTextSharp.text.Document();
                        PdfWriter.GetInstance(doc, new FileStream(sfd.FileName, FileMode.Create));
                        doc.Open();
                        doc.Add(new iTextSharp.text.Paragraph(txtPodglad.Text));
                        doc.Close();
                        txtPodglad.Text = "";
                        System.Windows.Forms.MessageBox.Show("Plik zapisany pomyślnie", "Plik PDF");

                        Color zolty = new Color();
                        zolty.A = 255;
                        zolty.R = 241;
                        zolty.G = 166;
                        zolty.B = 13;
                        btnGenerujRaport.Foreground = Brushes.Black;
                        btnGenerujRaport.Background = new SolidColorBrush(zolty);
                        btnGenerujRaport.BorderBrush = Brushes.Black;
                        btnZapiszDoPliku.Visibility = Visibility.Hidden;

                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Nie ma nic do zapisania", "Plik PDF",
                        System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Asterisk);
                    }

                }

            }
        }
        void OdswiezEdycjeStawkiZamowienia()
        {
            lbxZamowienia.UnselectAll();
            UkryjWeryfikacjeZamowienia();
            lbxWeryfikacja.UnselectAll();
            UkryjWeryfikacje();
            lbxKlienci.UnselectAll();
            lbxKierowcy.UnselectAll();
            grdKlienci.Visibility = Visibility.Hidden;
            gridEdycji.Visibility = Visibility.Hidden;
            grdWeryfikacja.Visibility = Visibility.Hidden;
            grdZamowienie.Visibility = Visibility.Hidden;
            grdEdycjaZamowienia.Visibility = Visibility.Hidden;
            grdPojazd.Visibility = Visibility.Hidden;
            grdRaport.Visibility = Visibility.Hidden;
            UkryjEdycjeZamowienia();
            grdStawkaGodzinowa.Visibility = Visibility.Visible;
            lbxKierowcyStawkaGodzinowa.UnselectAll();
            
        }

        
        Kierowca kierowcaStawkaGodzinowa = new Kierowca();
        List<pensja> pensje = new List<pensja>();
        private void lbxKierowcyStawkaGodzinowa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtStawka.Text = "";
            cbxMiesiac.Items.Clear();
            cbxRok.Items.Clear();
            cbxMiesiac.SelectedIndex = 0;
            cbxRok.SelectedIndex = 0;
            grdEdycjaStawkiGodzinowej.Visibility = Visibility.Visible;
            grdZapisanieStawkiGodzinowej.Visibility = Visibility.Visible;
            lblZłaStawkaGodzinowa.Visibility = Visibility.Hidden;
            if (lbxKierowcyStawkaGodzinowa.SelectedItem != null) { 
                mydbEntities baza = new mydbEntities();
                kierowcaStawkaGodzinowa = lbxKierowcyStawkaGodzinowa.SelectedItem as Kierowca;
                var stawki = baza.pensja.Where(t => t.idKierowcy == kierowcaStawkaGodzinowa.IdKierowcy);
                    foreach (pensja stawka in stawki)
                    {
                        pensje.Add(stawka);
                    }
            }
            for (int j = 1; j <= 12; j++)
            {
                cbxMiesiac.Items.Add(j);
            }
            for (int j = 2022; j >= 2000; j--)
            {
                cbxRok.Items.Add(j);
            }
        }
        const string stawkaGodzinowa = @"^[0-9]*(\,[0-9]{0,2})?$";
        private void btnZapiszStawkeGodzninowa_Click(object sender, RoutedEventArgs e)
        {
            mydbEntities baza = new mydbEntities();
            if (pensje.Count() > 0 && kierowcaStawkaGodzinowa != null)
            {
                for (int i = 0; i < pensje.Count(); i++)
                {
                    if (Convert.ToDateTime(pensje[i].miesiac).Month == Convert.ToInt32(cbxMiesiac.SelectedItem) && Convert.ToDateTime(pensje[i].miesiac).Year == Convert.ToInt32(cbxRok.SelectedItem))
                    {
                        if (Regex.IsMatch(txtStawka.Text,stawkaGodzinowa) && !String.IsNullOrEmpty(txtStawka.Text))
                        {
                            string dateString = $"{cbxRok.SelectedItem}-{cbxMiesiac.SelectedItem}-01";
                            DateTime dataTime = Convert.ToDateTime(dateString);
                            var pensja = baza.pensja.First(t => t.miesiac == dataTime && t.idKierowcy == kierowcaStawkaGodzinowa.IdKierowcy);
                            pensja.stawka_godz = Convert.ToDouble(txtStawka.Text);
                            baza.SaveChanges();
                            Inicjalizacja();
                            grdZapisanieStawkiGodzinowej.Visibility = Visibility.Hidden;
                            grdEdycjaStawkiGodzinowej.Visibility = Visibility.Hidden;
                            break;
                        }
                        else
                        {
                            lblZłaStawkaGodzinowa.Visibility = Visibility.Visible;
                        }
                    }
                    else if (i + 1 == pensje.Count())
                    {
                        if (Regex.IsMatch(txtStawka.Text, stawkaGodzinowa) && !String.IsNullOrEmpty(txtStawka.Text))
                        {
                            string dateString = $"{cbxRok.SelectedItem}-{cbxMiesiac.SelectedItem}-01";
                        pensja nowaPensja = new pensja()
                        {
                            stawka_godz = Convert.ToDouble(txtStawka.Text),
                            miesiac = Convert.ToDateTime(dateString).Date,
                            idKierowcy = kierowcaStawkaGodzinowa.IdKierowcy
                        };
                            baza.pensja.Add(nowaPensja);
                            baza.SaveChanges();
                            Inicjalizacja();
                            grdZapisanieStawkiGodzinowej.Visibility = Visibility.Hidden;
                            grdEdycjaStawkiGodzinowej.Visibility = Visibility.Hidden;
                        }
                        else
                        {
                            lblZłaStawkaGodzinowa.Visibility = Visibility.Visible;
                        }
                    }
                }
            }else if(pensje.Count() == 0 && kierowcaStawkaGodzinowa != null)
            {
                if (Regex.IsMatch(txtStawka.Text, stawkaGodzinowa) && !String.IsNullOrEmpty(txtStawka.Text))
                {
                    string dateString = $"{cbxRok.SelectedItem}-{cbxMiesiac.SelectedItem}-01";
                    pensja nowaPensja = new pensja()
                    {
                        stawka_godz = Convert.ToDouble(txtStawka.Text),
                        miesiac = Convert.ToDateTime(dateString).Date,
                        idKierowcy = kierowcaStawkaGodzinowa.IdKierowcy
                    };
                    baza.pensja.Add(nowaPensja);
                    baza.SaveChanges();
                    Inicjalizacja();
                    grdZapisanieStawkiGodzinowej.Visibility = Visibility.Hidden;
                    grdEdycjaStawkiGodzinowej.Visibility = Visibility.Hidden;
                }
                else
                {
                    lblZłaStawkaGodzinowa.Visibility = Visibility.Visible;
                }

            }
            
        }

        private void btnDodaPojazd0_Click(object sender, RoutedEventArgs e)
        {
            lblBladCenaM.Content = null;
            grdSzczegolyPojazdu.Visibility = Visibility.Hidden;
            grdEdycjaPojazdu.Visibility = Visibility.Hidden;
            btnZapisz.Visibility = Visibility.Hidden;
            if (lbxListaModeli.SelectedItem == null && lbxListaPojazdow.SelectedItem != null)
            {
                grdDodaniePojazdu.Visibility = Visibility.Visible;
            }
        }
    }
}
