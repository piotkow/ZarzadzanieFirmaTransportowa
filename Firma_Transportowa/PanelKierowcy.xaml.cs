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
using MySql.Data.MySqlClient;


namespace Firma_Transportowa
{
    /// <summary>
    /// Interaction logic for Kierowca.xaml
    /// </summary>
    /// 
    public partial class PanelKierowcy : Page
    {
        public PanelKierowcy()
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

        int idKierowcy;
        int idZamowienia;
        int idPojazdu;
        int idModelu;
        bool CzyZmianaCeny = false;
        int lastIdSczegolow;
        int id_zamowienia;
        List<int> listaZamowien = new List<int>();

        public PanelKierowcy(int id)
        {
            MySqlConnection con = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;");
            MySqlConnection con2 = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;");
            con.Open();
            con2.Open();
            MySqlCommand commKierowca = new MySqlCommand($"SELECT idKierowcy FROM kierowcy WHERE idKonta={id}", con);
            idKierowcy = Convert.ToInt32(commKierowca.ExecuteScalar());
            InitializeComponent();
            if (!CzyJestPusty(Convert.ToString(commKierowca.ExecuteScalar())))
            {
                MySqlCommand commKtoreZlecenia = new MySqlCommand($"SELECT idZamowienia FROM zamowienia WHERE idKierowcy={Convert.ToString(commKierowca.ExecuteScalar())}", con2);
                MySqlDataReader czytnikKtoreZlecenia = commKtoreZlecenia.ExecuteReader();
                while (czytnikKtoreZlecenia.Read())
                {
                    listaZamowien.Add(Convert.ToInt32(czytnikKtoreZlecenia["idZamowienia"]));
                }
                czytnikKtoreZlecenia.Close();

                OdswiezListeZamowien();
                MySqlCommand commIdKierowcy = new MySqlCommand($"SELECT idKierowcy FROM kierowcy WHERE idKonta={id}", con);
                MySqlCommand commPensje = new MySqlCommand($"SELECT * FROM pensja WHERE idKierowcy={Convert.ToInt32(commIdKierowcy.ExecuteScalar())}", con);
                MySqlDataReader czytnikPensji = commPensje.ExecuteReader();
                while (czytnikPensji.Read())
                {
                    pensja p = new pensja();
                    p.idPensja = Convert.ToInt32(czytnikPensji["idPensja"]);
                    p.miesiac = Convert.ToDateTime(czytnikPensji["miesiac"]);
                    lbxListaPensji.Items.Add(p);
                }
                czytnikPensji.Close();
                con.Close();
                con2.Close();
            }
        }

        void OdswiezListeZamowien()
        {
            lbxListaZlecen.Items.Clear();
            lbxListaZlecenH.Items.Clear();
            for (int i = 0; i < listaZamowien.Count(); i++)
            {
                zamowienia zam = new zamowienia();
                MySqlConnection con = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;");
                con.Open();
                MySqlCommand commZlecenie = new MySqlCommand($"SELECT * FROM zamowienia WHERE idZamowienia={listaZamowien[i]} AND status_zamowienia != @status;", con);
                commZlecenie.Parameters.AddWithValue("@status", "Anulowany");
                zam.idZamowienia = listaZamowien[i];
                MySqlDataReader czytnikZlecenia = commZlecenie.ExecuteReader();
                while (czytnikZlecenia.Read())
                {
                    zam.data_zlozenia_zamowienia = Convert.ToDateTime(czytnikZlecenia["data_zlozenia_zamowienia"]);
                    zam.idPojazdy = Convert.ToInt32(czytnikZlecenia["idPojazdy"]);
                    switch (Convert.ToString(czytnikZlecenia["status_zamowienia"]))
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
                        case "Zatwierdzony":
                            zam.status_zamowienia = "Zatwierdzony";
                            break;
                        case "Anulowany":
                            zam.status_zamowienia = "Anulowany";
                            break;
                    }
                    if (zam.status_zamowienia == "Realizowany" || zam.status_zamowienia == "Zakonczony")
                    {
                        zam.data_poczatku = Convert.ToDateTime(czytnikZlecenia["data_poczatku"]);
                    }
                    if (zam.status_zamowienia == "Zakonczony")
                    {
                        zam.data_konca = DateTime.Now;
                        lbxListaZlecenH.Items.Add(zam);
                    }
                    else
                    {
                        lbxListaZlecen.Items.Add(zam);
                    }

                }
                czytnikZlecenia.Close();
                con.Close();
            }
        }

        private void SelectionChanged(ListBox lbx, Label lblNapisAdresP, Label lblNapisAdresK, Label lblAdresPocz, Label lblAdresKon, Label lblOdlegloscSuma, ComboBox cmb)
        {
            if (lbx.SelectedItem != null)
            {
                zamowienia zam = new zamowienia();
                zam = lbx.SelectedItem as zamowienia;
                MySqlConnection con = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;");
                con.Open();
                Console.WriteLine(zam.idZamowienia);
                MySqlCommand commIdSzczegolow = new MySqlCommand($"SELECT idSzczegoly_zamowienia FROM zamowienia WHERE idZamowienia={zam.idZamowienia}", con);
                MySqlCommand idModel = new MySqlCommand($"SELECT idModel FROM zamowienia WHERE idZamowienia={zam.idZamowienia}", con);
                idModelu = Convert.ToInt32(idModel.ExecuteScalar());
                int pom_id = Convert.ToInt32(commIdSzczegolow.ExecuteScalar());
                lastIdSczegolow = pom_id;
                MySqlCommand commSzczegoly = new MySqlCommand($"SELECT * FROM szczegoly_zamowienia WHERE idSzczegoly_zamowienia={pom_id}", con);
                MySqlDataReader czytnik = commSzczegoly.ExecuteReader();
                while (czytnik.Read())
                {
                    double cena = 0;
                    string kod_pocztowy = $"{czytnik["poczatek_kod_pocztowy"]}";
                    if (!DBNull.Value.Equals(czytnik["cena"]))
                    {
                        cena = Convert.ToDouble(czytnik["cena"]);
                    }
                    lblNapisAdresP.Content = "Adres początkowy:";
                    lblAdresPocz.Content = $"{czytnik["poczatek_ulica"]} {czytnik["poczatek_nr_domu"]}\n" +
                                           $"{kod_pocztowy} {czytnik["poczatek_miasto"]}\n" +
                                           $"{czytnik["poczatek_panstwo"]}";
                    kod_pocztowy = $"{czytnik["koniec_kod_pocztowy"]}";
                    lblNapisAdresK.Content = "Adres docelowy:";
                    lblAdresKon.Content = $"{czytnik["koniec_ulica"]} {czytnik["koniec_nr_domu"]}\n" +
                                           $"{kod_pocztowy} {czytnik["koniec_miasto"]}\n" +
                                           $"{czytnik["koniec_panstwo"]}";
                    if (cena == 0)
                    {
                        lblOdlegloscSuma.Content = "Zamówienie czeka na wycenę";
                    }
                    else
                    {
                        lblOdlegloscSuma.Content = $"Ilość kilometrów: {czytnik["ilosc_km"]}km  Razem: {cena.ToString("C")}";
                    }
                    switch (zam.status_zamowienia)
                    {
                        case "Oczekuje":
                            cmb.SelectedIndex = 0; break;
                        case "Realizowany":
                            cmb.SelectedIndex = 1; break;
                        case "Zakonczony":
                            cmb.SelectedIndex = 2; break;
                        case "Zatwierdzony":
                            cmb.SelectedIndex = 3;break;
                    }
                }
                czytnik.Close();
                con.Close();
            }
            else if (CzyZmianaCeny == true)
            {
                MySqlConnection con = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;");
                con.Open();
                MySqlCommand commSzczegoly = new MySqlCommand($"SELECT * FROM szczegoly_zamowienia WHERE idSzczegoly_zamowienia={lastIdSczegolow}", con);
                MySqlDataReader czytnik = commSzczegoly.ExecuteReader();
                while (czytnik.Read())
                {
                    lblOdlegloscSuma.Content = $"Razem: {Convert.ToDouble(czytnik["cena"]).ToString("C")}";
                }
                czytnik.Close();
                con.Close();
            }
        }

        private void lbxListaZlecen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectionChanged(lbxListaZlecen, lblNapisAdresP, lblNapisAdresK, lblAdresPoczatkowy, lblAdresKoncowy, lblOdlegloscSuma, cmbStatusZamowienia);
            mydbEntities baza = new mydbEntities();
            MySqlConnection con = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;");
            con.Open();
            var pojazdy = baza.pojazdy.Where(t => t.idModel == idModelu);
            lbxPojazdy.Items.Clear();
            foreach (var a in pojazdy)
            {
                if (a.dostepnosc == 1)
                {
                    lbxPojazdy.Items.Add(a);
                }
            }
        }


        private void lbxListaPensji_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            pensja p = new pensja();
            p = lbxListaPensji.SelectedItem as pensja;
            mydbEntities baza = new mydbEntities();
            int liczba_godzin=0;
            var zamowienia_z_bazy = baza.zamowienia.Where(t => t.idKierowcy == idKierowcy);
            foreach(var zamowienie in zamowienia_z_bazy)
            {
                liczba_godzin += Convert.ToInt32(zamowienie.przewidywany_czas);
            }
            var stawka = baza.pensja.First(t => t.idKierowcy == idKierowcy && t.miesiac == p.miesiac);
            double stawka_godz = Convert.ToDouble(stawka.stawka_godz);
            lblLiczbaGodzin.Content = "Liczba godzin: " + liczba_godzin;
            lblStawka.Content = "Stawka: " + stawka_godz.ToString("C");
            lblSuma.Content = "Suma: " + (stawka_godz * liczba_godzin).ToString("C");
        }

        private void btnZatwierdzZmiany_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                mydbEntities baza = new mydbEntities();
                zamowienia zam = new zamowienia();
                string status = null;
                zam = lbxListaZlecen.SelectedItem as zamowienia;
                if (lbxListaZlecen.SelectedItem != null)
                {
                    switch (cmbStatusZamowienia.SelectedIndex)
                    {
                        case 0:
                            status = "Oczekuje";
                            if (zam.status_zamowienia == "Realizowany" || zam.status_zamowienia == "Zakonczony" || zam.status_zamowienia == "Zatwierdzony")
                            {
                                throw new InvalidCastException();
                            }
                            break;
                        case 1:
                            status = "Realizowany";
                            if (zam.status_zamowienia == "Oczekuje")
                            {
                                throw new InvalidCastException();
                            }
                            break;
                        case 2:
                            status = "Zakonczony";
                            if (zam.status_zamowienia == "Oczekuje" || zam.status_zamowienia=="Zatwierdzony")
                            {
                                throw new InvalidCastException();
                            }
                            break;
                        case 3:
                            status = "Zatwierdzony";
                            {
                                throw new InvalidCastException();
                            }
                            break;
                    }
                    var zamowienie = baza.zamowienia.First(t => t.idZamowienia == zam.idZamowienia);
                    var typ = baza.model_samochodu.First(t => t.idModel == idModelu);
                    var sz_zamowienie = baza.szczegoly_zamowienia.First(t => t.idSzczegoly_zamowienia == zamowienie.idSzczegoly_zamowienia);
                    if (!CzyJestPusty(txtIloscKm.Text))
                    {
                        sz_zamowienie.ilosc_km = Convert.ToDouble(txtIloscKm.Text);
                        sz_zamowienie.cena = sz_zamowienie.ilosc_km * typ.cena_za_km;
                        CzyZmianaCeny = true;
                    }
                    zamowienie.status_zamowienia = status;
                    if (status == "Realizowany")
                    {
                        zamowienie.data_poczatku = DateTime.Now;
                        var pojazd = baza.pojazdy.First(t => t.idPojazdy == zamowienie.idPojazdy);
                        pojazd.status_pojazdu = "Zajety";
                    }
                    if (status == "Zakonczony")
                    {
                        zamowienie.data_konca = DateTime.Now;
                        var pojazd = baza.pojazdy.First(t => t.idPojazdy == zamowienie.idPojazdy);
                        pojazd.status_pojazdu = "Wolny";
                    }
                    baza.SaveChanges();
                    OdswiezListeZamowien();
                    SelectionChanged(lbxListaZlecen, lblNapisAdresP, lblNapisAdresK, lblAdresPoczatkowy, lblAdresKoncowy, lblOdlegloscSuma, cmbStatusZamowienia);
                }
            }
            catch (InvalidCastException)
            {
                MessageBox.Show("Error");
            }
            OdswiezListeZamowien();
            txtIloscKm.Text = "";
        }

        private void btnZlecenia_Click(object sender, RoutedEventArgs e)
        {
            Color zolty = new Color();
            zolty.A = 255;
            zolty.R = 241;
            zolty.G = 166;
            zolty.B = 13;
            grdPanelHistorii.Visibility = Visibility.Hidden;
            grdPanelZlecen.Visibility = Visibility.Visible;
            btnHistoriaZlecen.Foreground = new SolidColorBrush(zolty);
            btnHistoriaZlecen.Background = Brushes.Transparent;
            btnHistoriaZlecen.BorderBrush = new SolidColorBrush(zolty);
            btnZlecenia.Background = new SolidColorBrush(zolty);
            btnZlecenia.Foreground = Brushes.Black;
            btnZlecenia.BorderBrush = Brushes.Black;
        }

        private void btnHistoriaZlecen_Click(object sender, RoutedEventArgs e)
        {
            Color zolty = new Color();
            zolty.A = 255;
            zolty.R = 241;
            zolty.G = 166;
            zolty.B = 13;
            grdPanelHistorii.Visibility = Visibility.Visible;
            grdPanelZlecen.Visibility = Visibility.Hidden;
            cmbStatusZamowieniaH.Visibility = Visibility.Hidden;
            btnZlecenia.Background = Brushes.Transparent;
            btnZlecenia.Foreground = new SolidColorBrush(zolty);
            btnZlecenia.BorderBrush = new SolidColorBrush(zolty);
            btnHistoriaZlecen.Foreground = Brushes.Black;
            btnHistoriaZlecen.Background = new SolidColorBrush(zolty);
            btnHistoriaZlecen.BorderBrush = Brushes.Black;
        }

        private void lbxListaZlecenH_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectionChanged(lbxListaZlecenH, lblNapisAdresPH, lblNapisAdresKH, lblAdresPoczatkowyH, lblAdresKoncowyH, lblOdlegloscSumaH, cmbStatusZamowieniaH);
            lblNapisSumaH.Visibility = Visibility.Visible;
        }

        private void lbxPojazdy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnPrzypiszPojazd_Click(object sender, RoutedEventArgs e)
        {
            if (lbxListaZlecen.SelectedItem != null && lbxPojazdy.SelectedItem!=null)
            {
                mydbEntities baza = new mydbEntities();
                zamowienia zam = new zamowienia();
                zam = lbxListaZlecen.SelectedItem as zamowienia;
                pojazdy pojazd = new pojazdy();
                pojazd = lbxPojazdy.SelectedItem as pojazdy;
                MySqlConnection con = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;");
                con.Open();
                MySqlCommand Zmien = new MySqlCommand($"UPDATE zamowienia SET idPojazdy={pojazd.idPojazdy} WHERE idZamowienia={zam.idZamowienia}", con);
                Zmien.ExecuteScalar();
                con.Close();
                OdswiezListeZamowien();
            }
        }
    }
}
