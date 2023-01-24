using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Firma_Transportowa
{
    /// <summary>
    /// Interaction logic for PanelKlienta.xaml
    /// </summary>
    public partial class PanelKlienta : Page
    {
        public PanelKlienta()
        {
            InitializeComponent();
           
        }

    
        public bool CzyJestPusty(String x)
        {
            if (String.IsNullOrEmpty(x))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        List<int> listaZamowien = new List<int>();
        int idKlienta;
        int idKon;
        
        public PanelKlienta(int id)
        {
            MySqlConnection con = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;");
            MySqlConnection con2 = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;");
            con.Open();
            con2.Open();
            MySqlCommand commKlient = new MySqlCommand($"SELECT idKlienci FROM klienci WHERE idKonta={id}", con);
            idKon = id;
            InitializeComponent();
            idKlienta = Convert.ToInt32(commKlient.ExecuteScalar());

            if (!CzyJestPusty(Convert.ToString(commKlient.ExecuteScalar())))
            {
                MySqlCommand commKtoreZamowienia = new MySqlCommand($"SELECT idZamowienia FROM zamowienia WHERE idKlienci={Convert.ToString(commKlient.ExecuteScalar())}", con2);
                MySqlDataReader czytnikKtoreZamowienie = commKtoreZamowienia.ExecuteReader();
                // List<int> listaZamowien = new List<int>();
                while (czytnikKtoreZamowienie.Read())
                {
                    listaZamowien.Add(Convert.ToInt32(czytnikKtoreZamowienie["idZamowienia"]));
                }
                czytnikKtoreZamowienie.Close();

                for (int i = 0; i < listaZamowien.Count(); i++)
                {
                    MySqlCommand commZamowienie = new MySqlCommand($"SELECT * FROM zamowienia WHERE idZamowienia={listaZamowien[i]}", con2);
                    zamowienia zam = new zamowienia();
                    zam.idZamowienia = listaZamowien[i];
                    MySqlDataReader czytnikZamowienie = commZamowienie.ExecuteReader();
                    while (czytnikZamowienie.Read())
                    {
                        zam.data_zlozenia_zamowienia = Convert.ToDateTime(czytnikZamowienie["data_zlozenia_zamowienia"]);
                        zam.zamowiona_data_realizacji = Convert.ToDateTime(czytnikZamowienie["zamowiona_data_realizacji"]);
                        zam.idSzczegoly_zamowienia = Convert.ToInt32(czytnikZamowienie["idSzczegoly_zamowienia"]);
                        zam.idPojazdy = Convert.ToInt32(czytnikZamowienie["idPojazdy"]);
                        zam.idModel = Convert.ToInt32(czytnikZamowienie["idModel"]);
                        switch (Convert.ToString(czytnikZamowienie["status_zamowienia"]))
                        {
                            case "Oczekuje":
                                zam.status_zamowienia = "Oczekuje";
                                break;
                            case "Realizowany":
                                zam.status_zamowienia = "Realizowany";
                                break;
                            case "Zakonczony":
                                zam.status_zamowienia = "Zakonczony";
                                break;
                            case "Anulowany":
                                zam.status_zamowienia = "Anulowany";
                                break;
                            case "Zatwierdzony":
                                zam.status_zamowienia = "Zatwierdzony";
                                break;
                        }
                        zam.komentarz = Convert.ToString(czytnikZamowienie["komentarz"]);

                        lbxListaZamowien.Items.Add(zam);
                        if (zam.status_zamowienia == "Zakonczony")
                        {
                            zam.data_konca = Convert.ToDateTime(czytnikZamowienie["data_konca"]);
                        }
                        else if (zam.status_zamowienia == "Realizowany")
                        {
                            zam.data_poczatku = Convert.ToDateTime(czytnikZamowienie["data_poczatku"]);
                        }

                    }
                    czytnikZamowienie.Close();
                }

            }
        }


        void OdswiezListe()
        {
            //listaZamowien.Clear();
            lbxListaZamowien.Items.Clear();
                MySqlConnection con2 = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;");
                con2.Open();
                
                    for (int i = 0; i < listaZamowien.Count(); i++)
                    {
                        MySqlCommand commZamowienie = new MySqlCommand($"SELECT * FROM zamowienia WHERE idZamowienia={listaZamowien[i]}", con2);
                        zamowienia zam = new zamowienia();
                        zam.idZamowienia = listaZamowien[i];
                        MySqlDataReader czytnikZamowienie = commZamowienie.ExecuteReader();
                        while (czytnikZamowienie.Read())
                        {

                            zam.data_zlozenia_zamowienia = Convert.ToDateTime(czytnikZamowienie["data_zlozenia_zamowienia"]);
                            zam.zamowiona_data_realizacji = Convert.ToDateTime(czytnikZamowienie["zamowiona_data_realizacji"]);
                            zam.idSzczegoly_zamowienia = Convert.ToInt32(czytnikZamowienie["idSzczegoly_zamowienia"]);
                            zam.idPojazdy = Convert.ToInt32(czytnikZamowienie["idPojazdy"]);
                            zam.idModel = Convert.ToInt32(czytnikZamowienie["idModel"]);
                            switch (Convert.ToString(czytnikZamowienie["status_zamowienia"]))
                            {
                                case "Oczekuje":
                                    zam.status_zamowienia = "Oczekuje";
                                    break;
                                case "Realizowany":
                                    zam.status_zamowienia = "Realizowany";
                                    break;
                                case "Zakonczony":
                                    zam.status_zamowienia = "Zakonczony";
                                    break;
                                case "Anulowany":
                                    zam.status_zamowienia = "Anulowany";
                                    break;
                                case "Zatwierdzony":
                                    zam.status_zamowienia = "Zatwierdzony";
                                    break;
                            }
                            zam.komentarz = Convert.ToString(czytnikZamowienie["komentarz"]);

                            lbxListaZamowien.Items.Add(zam);
                            if (zam.status_zamowienia == "Zakonczony")
                            {
                                zam.data_konca = Convert.ToDateTime(czytnikZamowienie["data_konca"]);
                            }
                            else if (zam.status_zamowienia == "Realizowany")
                            {
                                zam.data_poczatku = Convert.ToDateTime(czytnikZamowienie["data_poczatku"]);
                            }

                        }
                        czytnikZamowienie.Close();
                    }
            WyczyscDane();
                }
        void WyczyscDane()
        {
            lblNapisAdresP.Content = "";
            lblAdresPoczatkowy.Content = "";
            lblNapisAdresK.Content = "";
            lblAdresKoncowy.Content = "";

            lblOdleglosc.Content = "";
            lblNapisOdl.Content = "";
            lblSuma.Content = "";
            lblSuma.Content = "";
            lblNapisOdl.Visibility = Visibility.Hidden;
            lblSuma.Content = "";
            lblNapisOdl.Visibility = Visibility.Hidden;
            lblNapisOdl.Visibility = Visibility.Visible;
            lblNapisCena.Content = "";
            lblSuma.Content = "";
            lblKom.Content = "";
            labele.Visibility = Visibility.Hidden;
        }
            int idZam;
        private void lbxListaZamowien_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            edycja.Visibility = Visibility.Hidden;
            labele.Visibility = Visibility.Visible;
            if (lbxListaZamowien.SelectedItem != null)
            {
                double cena = 0;
                zamowienia zam = new zamowienia();
                zam = lbxListaZamowien.SelectedItem as zamowienia;
                MySqlConnection con = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;");
                idZam = zam.idZamowienia;
                con.Open();
                MySqlCommand commIdSzczegolow = new MySqlCommand($"SELECT idSzczegoly_zamowienia FROM zamowienia WHERE idZamowienia={zam.idZamowienia}", con);
                int pom_id = Convert.ToInt32(commIdSzczegolow.ExecuteScalar());
                MySqlCommand commSzczegoly = new MySqlCommand($"SELECT * FROM szczegoly_zamowienia WHERE idSzczegoly_zamowienia={pom_id}", con);
                MySqlDataReader czytnik = commSzczegoly.ExecuteReader();
                while (czytnik.Read())
                {


                    if (!DBNull.Value.Equals(czytnik["cena"]))
                    {
                        cena = Convert.ToDouble(czytnik["cena"]);
                    }
                    lblNapisAdresP.Content = "Adres początkowy:";
                    lblAdresPoczatkowy.Content = $"{czytnik["poczatek_ulica"]} {czytnik["poczatek_nr_domu"]}\n" +
                                                     $"{czytnik["poczatek_kod_pocztowy"]} {czytnik["poczatek_miasto"]}\n" +
                                                     $"{czytnik["poczatek_panstwo"]}";

                    lblNapisAdresK.Content = "Adres docelowy:";
                    lblAdresKoncowy.Content = $"{czytnik["koniec_ulica"]} {czytnik["koniec_nr_domu"]}\n" +
                                                 $"{czytnik["koniec_kod_pocztowy"]} {czytnik["koniec_miasto"]}\n" +
                                                 $"{czytnik["koniec_panstwo"]}";


                    lblOdleglosc.Content = $"{czytnik["ilosc_km"]}";
                    lblNapisOdl.Content = "Odległość:";

                    if (zam.status_zamowienia == "Anulowany")
                    {
                        lblSuma.Content = "Zamówienie zostało anulowane";
                    }
                    else if (zam.status_zamowienia == "Oczekuje")
                    {
                        lblSuma.Content = "Zamówienie czeka na wycenę";
                        lblNapisOdl.Visibility = Visibility.Hidden;
                    }
                    else if (zam.status_zamowienia == "Zatwierdzony" && cena == 0)
                    {
                        lblSuma.Content = "Zamówienie czeka na wycenę";
                        lblNapisOdl.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        lblNapisOdl.Visibility = Visibility.Visible;
                        lblNapisCena.Content = "Cena: ";
                        lblSuma.Content = $"{cena.ToString("C")}";
                    }

                    if (zam.komentarz != null)
                    {
                        lblKom.Content = zam.komentarz;
                    }
                }
                czytnik.Close();
                con.Close();
            }


        }

        private void btnEdytuj_Click(object sender, RoutedEventArgs e)
        {

            DateTime teraz = DateTime.Now;
            zamowienia zam = new zamowienia();
            zam = lbxListaZamowien.SelectedItem as zamowienia;
            MySqlConnection con = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;");

            if (lbxListaZamowien.SelectedItem != null)
            {
                DateTime wybranaData = Convert.ToDateTime(zam.zamowiona_data_realizacji);
                if (wybranaData <= teraz.AddDays(1) || zam.status_zamowienia == "Zakonczony" || zam.status_zamowienia == "Anulowany" || zam.status_zamowienia == "Realizowany")
                {
                    MessageBox.Show("Zamówienie zostało zakończone, anulowane lub czas do zamówienia jest mniejszy niż 24 godziny nie można już go edytować");
                }
                else
                {
                    con.Open();
                    MySqlCommand commIdSzczegolow = new MySqlCommand($"SELECT idSzczegoly_zamowienia FROM zamowienia WHERE idZamowienia={zam.idZamowienia}", con);
                    int pom_id = Convert.ToInt32(commIdSzczegolow.ExecuteScalar());
                    MySqlCommand commSzczegoly = new MySqlCommand($"SELECT * FROM szczegoly_zamowienia WHERE idSzczegoly_zamowienia={pom_id}", con);
                    MySqlDataReader czytnik = commSzczegoly.ExecuteReader();
                    while (czytnik.Read())
                    {
                        labele.Visibility = Visibility.Hidden;
                        edycja.Visibility = Visibility.Visible;
                        txtPPanstwo.Text = $"{czytnik["poczatek_panstwo"]}";
                        txtPMiasto.Text = $"{ czytnik["poczatek_miasto"]}";
                        txtPUlica.Text = $"{czytnik["poczatek_ulica"]}";
                        txtPNrDomu.Text = $"{czytnik["poczatek_nr_domu"]}";
                        txtPKodPoczt.Text = $"{czytnik["poczatek_kod_pocztowy"]}";
                        txtKPanstwo.Text = $"{czytnik["koniec_panstwo"]}";
                        txtKUlica.Text = $"{czytnik["koniec_ulica"]}";
                        txtKNrDomu.Text = $"{czytnik["koniec_nr_domu"]}";
                        txtKKodPoczt.Text = $"{czytnik["koniec_kod_pocztowy"]}";
                        txtKMiasto.Text = $"{czytnik["koniec_miasto"]}";
                        DPZData.SelectedDate = wybranaData.Date;
                        TxtZGodzina.Text = Convert.ToString(wybranaData.Hour);
                        TxtZMinuta.Text = Convert.ToString(wybranaData.Minute);
                    }
                    czytnik.Close();
                    con.Close();
                }
            }
        }
        void Wyczysc()
        {
            lblPPanstw.Content = "";
            lblPMiasto.Content = "";
            lblPUl.Content = "";
            lblPNrD.Content = "";
            lblPKpoczt.Content = "";

            lblKPanstw.Content = "";
            lblKMiasto.Content = "";
            lblKUl.Content = "";
            lblKNrD.Content = "";
            lblKKpoczt.Content = "";

            lblPuste.Content = "";
        }
        public bool CzyJestPusty(TextBox x)
        {
            if (String.IsNullOrEmpty(x.Text))
            {
                return true;
            }
            else
            {

                return false;
            }
        }
        public bool CzyJestPusty(DatePicker y)
        {
            if (String.IsNullOrEmpty(Convert.ToString(y.SelectedDate)))
            {
                return true;
            }
            else
            {

                return false;
            }
        }
        bool CzyZapisac = true;
        const string Godzina = @"^[0-1][0-9]|(2[0-4])$";
        const string Minuta = @"^[0-5][0-9]$";
        private void btnZapisz_Click(object sender, RoutedEventArgs e)
        {
            zamowienia zam = new zamowienia();
            zam = lbxListaZamowien.SelectedItem as zamowienia;
            CzyZapisac = true;
            Wyczysc();
            using (MySqlConnection con = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;"))
            {
                con.Open();
                if (CzyJestPusty(txtPPanstwo) || CzyJestPusty(txtPMiasto) || CzyJestPusty(txtPUlica) || CzyJestPusty(txtPNrDomu) || CzyJestPusty(txtPKodPoczt) || CzyJestPusty(txtKPanstwo) || CzyJestPusty(txtKMiasto) || CzyJestPusty(txtKUlica) || CzyJestPusty(txtKNrDomu) || CzyJestPusty(txtKKodPoczt) || CzyJestPusty(TxtZGodzina) || CzyJestPusty(TxtZMinuta) || CzyJestPusty(DPZData))
                {
                    lblPuste.Content = "Pola nie mogą być puste!";
                    CzyZapisac = false;
                }
                else
                {
                    try
                    {
                        SzczegolyZamowienia sz = new SzczegolyZamowienia(txtPPanstwo.Text, txtPMiasto.Text, txtKPanstwo.Text, txtKMiasto.Text, txtPUlica.Text, txtKUlica.Text, txtPNrDomu.Text, txtKNrDomu.Text, txtPKodPoczt.Text, txtKKodPoczt.Text);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        lblKPanstw.Content = "Podana nazwa państwa ma nieprawidłowy format!";
                        CzyZapisac = false;
                    }
                    catch (InvalidOperationException)
                    {
                        lblPMiasto.Content = "Podana nazwa miasta ma nieprawidłowy format!";
                        CzyZapisac = false;
                    }
                    catch (ArgumentNullException)
                    {
                        lblPPanstw.Content = "Podana nazwa państwa ma nieprawidłowy format!";
                        CzyZapisac = false;
                    }
                    catch (ArgumentException)
                    {
                        lblKMiasto.Content = "Podana nazwa miasta ma nieprawidłowy format!";
                        CzyZapisac = false;
                    }
                    catch (OverflowException)
                    {
                        lblPUl.Content = "Podana nazwa ulicy ma nieprawidłowy format!";
                        CzyZapisac = false;
                    }
                    catch (OutOfMemoryException)
                    {
                        lblKUl.Content = "Podana nazwa ulicy ma nieprawidłowy format!";
                        CzyZapisac = false;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        lblPKpoczt.Content = "Podany kod pocztowy ma nieprawidłowy format!";
                        CzyZapisac = false;
                    }
                    catch (TimeoutException)
                    {
                        lblKKpoczt.Content = "Podany kod pocztowy ma nieprawidłowy format!";
                        CzyZapisac = false;
                    }
                    catch (FormatException)
                    {
                        lblPKpoczt.Content = "Podany kod pocztowy ma nieprawidłowy format!";
                        CzyZapisac = false;
                    }
                    catch (DivideByZeroException)
                    {
                        lblKNrD.Content = "Podany numer domu ma nieprawidłowy format!";
                        CzyZapisac = false;
                    }
                    catch (ArithmeticException)
                    {
                        lblPNrD.Content = "Podany numer domu ma nieprawidłowy format!";
                        CzyZapisac = false;
                    }

                    if (!Regex.IsMatch(TxtZGodzina.Text, Godzina) || !Regex.IsMatch(TxtZMinuta.Text, Minuta))
                    {
                        lblczas.Content = "godzina ma niewłaściwy format";
                        CzyZapisac = false;
                    }
                }
                con.Close();
            }
            MySqlConnection con2 = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;");

            con2.Open();
            string dzień = Convert.ToString(DPZData.SelectedDate).Substring(0, 2);
            string miesiąc = Convert.ToString(DPZData.SelectedDate).Substring(3, 2);
            string rok = Convert.ToString(DPZData.SelectedDate).Substring(6, 4);
            string godzina = TxtZGodzina.Text;
            string minuta = TxtZMinuta.Text;
            string data = rok + "-" + miesiąc + "-" + dzień + " " + godzina + ":" + minuta + ":00";

            if (CzyZapisac)
            {
                MySqlCommand comEdytujSzczegoly = new MySqlCommand($"update szczegoly_zamowienia set poczatek_panstwo=@poczatek_pan,poczatek_miasto=@poczatek_mias , poczatek_ulica=@poczatek_ul, poczatek_nr_domu=@poczatek_nr, poczatek_kod_pocztowy=@poczatek_kod, koniec_panstwo=@koniec_pan, koniec_miasto=@koniec_mias, koniec_ulica=@koniec_ul, koniec_nr_domu=@koniec_nr, koniec_kod_pocztowy=@koniec_kod, cena=@cen,ilosc_km=@il_km  where idSzczegoly_zamowienia={zam.idSzczegoly_zamowienia}", con2);
                comEdytujSzczegoly.Parameters.AddWithValue("@poczatek_pan", txtPPanstwo.Text);
                comEdytujSzczegoly.Parameters.AddWithValue("@poczatek_mias", txtPMiasto.Text);
                comEdytujSzczegoly.Parameters.AddWithValue("@poczatek_ul", txtPUlica.Text);
                comEdytujSzczegoly.Parameters.AddWithValue("@poczatek_nr", txtPNrDomu.Text);
                comEdytujSzczegoly.Parameters.AddWithValue("@poczatek_kod", txtPKodPoczt.Text);
                comEdytujSzczegoly.Parameters.AddWithValue("@koniec_pan", txtKPanstwo.Text);
                comEdytujSzczegoly.Parameters.AddWithValue("@koniec_mias", txtKMiasto.Text);
                comEdytujSzczegoly.Parameters.AddWithValue("@koniec_ul", txtKUlica.Text);
                comEdytujSzczegoly.Parameters.AddWithValue("@koniec_nr", txtKNrDomu.Text);
                comEdytujSzczegoly.Parameters.AddWithValue("@koniec_kod", txtKKodPoczt.Text);
                comEdytujSzczegoly.Parameters.AddWithValue("@cen", null);
                comEdytujSzczegoly.Parameters.AddWithValue("@il_km", null);
                comEdytujSzczegoly.ExecuteScalar();
                MySqlCommand comEdytujZam = new MySqlCommand($"update zamowienia set zamowiona_data_realizacji=@zamowiona_data_real, status_zamowienia='Oczekuje', idKierowcy=1 where idZamowienia={zam.idZamowienia}", con2);
                comEdytujZam.Parameters.AddWithValue("@zamowiona_data_real", data);
                comEdytujZam.ExecuteScalar();
                con2.Close();
                edycja.Visibility = Visibility.Hidden;
                labele.Visibility = Visibility.Visible;
                
            }
            OdswiezListe();
        }

        private void btnAnuluj_Click(object sender, RoutedEventArgs e)
        {

            MySqlConnection con = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;");
            con.Open();
            MySqlCommand anuluj = new MySqlCommand($"UPDATE zamowienia set status_zamowienia='Anulowany' where idZamowienia={idZam}", con);
            anuluj.ExecuteScalar();
            con.Close();
            OdswiezListe();
        }

        private void btnZmowienia_Click(object sender, RoutedEventArgs e)
        {
            zamowienia.Visibility = Visibility.Visible;
            grdProfil.Visibility = Visibility.Hidden;
            Color zolty = new Color();
            zolty.A = 255;
            zolty.R = 241;
            zolty.G = 166;
            zolty.B = 13;
            btnZmowienia.Foreground = Brushes.Black; 
            btnZmowienia.Background = new SolidColorBrush(zolty); 
            btnZmowienia.BorderBrush = Brushes.Black;
            btnProfil.Foreground = new SolidColorBrush(zolty);
            btnProfil.Background = Brushes.Transparent;
            btnProfil.BorderBrush = new SolidColorBrush(zolty);


        }

/////////////////////////////////////////////////////////////////////////////////profil//////////////////////////////////////////////
  
        private void btnProfil_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnection con = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;");
            MySqlConnection con2 = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;");
            con.Open();
            con2.Open();
            MySqlCommand comDane = new MySqlCommand($"SELECT * FROM klienci where idKonta = {idKon}", con);
            MySqlCommand comDane2 = new MySqlCommand($"SELECT * FROM konta where idKonta = {idKon}", con2);
            MySqlDataReader czytnik = comDane.ExecuteReader();
            MySqlDataReader czytnik2 = comDane2.ExecuteReader();
            while (czytnik.Read() && czytnik2.Read())
            {
                txtImie.Text = Convert.ToString(czytnik["imie_klienta"]);
                txtNazwisko.Text = Convert.ToString(czytnik["nazwisko_klienta"]);
                txtMail.Text = Convert.ToString(czytnik2["email_klienta"]);
                txtNrTelefonu.Text = Convert.ToString(czytnik["numer_tel_klienta"]);
                txtLogin.Text = Convert.ToString(czytnik2["login"]);
                txtHaslo.Text = Convert.ToString(czytnik2["haslo"]);
                txtHaslo2.Text = Convert.ToString(czytnik2["haslo"]);
            }
            zamowienia.Visibility = Visibility.Hidden;
            grdProfil.Visibility = Visibility.Visible;
            Color zolty = new Color();
            zolty.A = 255;
            zolty.R = 241;
            zolty.G = 166;
            zolty.B = 13;
           

            btnZmowienia.Foreground = new SolidColorBrush(zolty);
            btnZmowienia.Background = Brushes.Transparent;
            btnZmowienia.BorderBrush = new SolidColorBrush(zolty);
            btnProfil.Foreground = Brushes.Black;
            btnProfil.Background = new SolidColorBrush(zolty);
            btnProfil.BorderBrush = Brushes.Black;

            btnEdytujP.Foreground = Brushes.Black;
            btnEdytujP.Background = new SolidColorBrush(zolty);
            btnEdytujP.BorderBrush = Brushes.Black;

            btnZatwierdz.Visibility = Visibility.Hidden;
            txtImie.IsEnabled = false;
            txtNazwisko.IsEnabled = false;
            txtMail.IsEnabled = false;
            txtNrTelefonu.IsEnabled = false;
            txtLogin.IsEnabled = false;
            txtHaslo.IsEnabled = false;
            txtHaslo2.IsEnabled = false;
            
        }

        private void btnEdytujP_Click(object sender, RoutedEventArgs e)
        {
            btnZatwierdz.Visibility = Visibility.Visible;
            txtImie.IsEnabled = true;
            txtNazwisko.IsEnabled = true;
            txtMail.IsEnabled = true;
            txtNrTelefonu.IsEnabled = true;
            txtLogin.IsEnabled = true;
            txtHaslo.IsEnabled = true;
            txtHaslo2.IsEnabled = true;
            btnZatwierdz.Visibility = Visibility.Visible;
            Color zolty = new Color();
            zolty.A = 255;
            zolty.R = 241;
            zolty.G = 166;
            zolty.B = 13;
            btnEdytujP.Foreground = new SolidColorBrush(zolty);
            btnEdytujP.Background = Brushes.Transparent;
            btnEdytujP.BorderBrush = new SolidColorBrush(zolty);
            
        }

        private void btnZatwierdz_Click(object sender, RoutedEventArgs e)
        {
            WyczyscP();
            MySqlConnection con = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;");
            MySqlConnection con2 = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;");
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

                if (CzyJestPustyP(txtImie) || CzyJestPustyP(txtNazwisko) || CzyJestPustyP(txtLogin) || CzyJestPustyP(txtMail) || CzyJestPustyP(txtHaslo) || CzyJestPustyP(txtNrTelefonu) || CzyJestPustyP(txtHaslo2))
                {
                    lblZleHaslo.Content = "Pola nie mogą być puste!";
                    break;
                }
                else
                {
                    try
                    {
                        Konto k1 = new Konto(txtMail.Text, txtLogin.Text, txtHaslo.Text);
                        try
                        {
                            if (CzyTakieSameKonta(k1, pomKonto, lblMail, lblLogin))
                            {
                                if (txtHaslo.Text == txtHaslo2.Text)
                                {
                                    if (nrTel != pom_nrtel)
                                    {
                                        Klient klient = new Klient(txtImie.Text, txtNazwisko.Text, txtNrTelefonu.Text);
                                        MySqlCommand daneKonta = new MySqlCommand($"UPDATE konta set email_klienta='{txtMail.Text}',login='{txtLogin.Text}' ,haslo='{txtHaslo.Text}' WHERE idKonta={idKon};", con);
                                        MySqlCommand daneKlienta = new MySqlCommand($"UPDATE klienci set nazwisko_klienta ='{txtNazwisko.Text}',imie_klienta ='{txtImie.Text}',numer_tel_klienta='{txtNrTelefonu.Text}', weryfikacja=0 WHERE idKonta={idKon};", con);
                                        daneKlienta.ExecuteScalar();
                                        daneKonta.ExecuteScalar();
                                        con.Close();
                                        con2.Close();
                                        btnZatwierdz.Visibility = Visibility.Hidden;
                                        txtImie.IsEnabled = false;
                                        txtNazwisko.IsEnabled = false;
                                        txtMail.IsEnabled = false;
                                        txtNrTelefonu.IsEnabled = false;
                                        txtLogin.IsEnabled = false;
                                        txtHaslo.IsEnabled = false;
                                        txtHaslo2.IsEnabled = false;
                                        Color zolty = new Color();
                                        zolty.A = 255;
                                        zolty.R = 241;
                                        zolty.G = 166;
                                        zolty.B = 13;
                                        btnEdytujP.Foreground = Brushes.Black;
                                        btnEdytujP.Background = new SolidColorBrush(zolty);
                                        btnEdytujP.BorderBrush = Brushes.Black;
                                        break;
                                    }
                                    else
                                    {
                                        lblZleNrTel.Content = "Konto z takim numerem telefonu już istnieje!";
                                        break;
                                    }
                                }
                                else
                                {
                                    lblZleHaslo.Content = "Podane hasła nie są takie same";
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
                            lblZleNrTel.Content = "Podany numer telefonu ma nieprawidłowy format!";
                            break;
                        }
                        catch (ArgumentNullException)
                        {
                            lblZleImie.Content = "Podane imie ma nieprawidłowy format!";
                            break;
                        }
                        catch (ArgumentException)
                        {
                            lblZleNazwisko.Content = "Podane nazwisko ma nieprawidłowy format!";
                            break;
                        }
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        lblZleHaslo.Content = "Długość hasła jest nieprawidłowa!";
                        break;
                    }
                    catch (ArgumentNullException)
                    {
                        lblZleLogin.Content = "Długość loginu jest nieprawidłowa!";
                        break;
                    }
                    catch (ArgumentException)
                    {
                        lblZleMail.Content = "Podany e-mail jest nieprawidłowy!";
                        break;
                    }
                }

            }

        }
        public bool CzyTakieSameKonta(Konto p1, Konto p2, Label email, Label login)
        {
            bool spr = true;
            if (p1.Email == p2.Email)
            {
                email.Content = "Konto z takim e-mailem już istnieje!";
                spr = false;
            }
            if (p1.Login == p2.Login)
            {
                login.Content = "Taki login już istnieje!";
                spr = false;
            }
            return spr;
        }

        public bool CzyJestPustyP(TextBox x)
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
        void WyczyscP()
        {
            lblZleImie.Content = "";
            lblZleNazwisko.Content = "";
            lblZleHaslo.Content = "";
            lblZleMail.Content = "";
            lblZleLogin.Content = "";
            lblZleNrTel.Content = "";
        }
    }
}
