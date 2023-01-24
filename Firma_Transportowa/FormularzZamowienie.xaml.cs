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
    /// Interaction logic for FormularzZamowienie.xaml
    /// </summary>
    public partial class FormularzZamowienie : Page
    {
        public FormularzZamowienie()
        {
            InitializeComponent();
        }
        int idKonta;
        int idModelu;
        const string Godzina = @"^[0-1][0-9]|(2[0-4])$";
        const string Minuta = @"^[0-5][0-9]$";
        public FormularzZamowienie(int id, int idm)
        {
            InitializeComponent();
            idKonta = id;
            idModelu = idm;
            DPData.DisplayDateStart = DateTime.Now.AddDays(1);
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
        bool CzyZamowic = true;
        string data;
        private void btnZatwierdz_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CzyZamowic = true;
            Wyczysc();
            using (MySqlConnection con = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;"))
            {
                con.Open();
                if (CzyJestPusty(txtPoczatekPanstwo) || CzyJestPusty(txtPoczatekMiasto) || CzyJestPusty(txtPoczatekUlica) || CzyJestPusty(txtPoczatekNrDomu) || CzyJestPusty(txtPoczatekKodPoczt) || CzyJestPusty(txtKoniecPanstwo) || CzyJestPusty(txtKoniecMiasto) || CzyJestPusty(txtKoniecUlica) || CzyJestPusty(txtKoniecNrDomu) || CzyJestPusty(txtKoniecKodPoczt) || CzyJestPusty(TxtGodzina) || CzyJestPusty(TxtMinuta) || CzyJestPusty(DPData))
                {
                    lblPuste.Content = "Pola nie mogą być puste!";
                    CzyZamowic = false;
                }
                else
                {
                    try
                    {
                        SzczegolyZamowienia sz = new SzczegolyZamowienia(txtPoczatekPanstwo.Text, txtPoczatekMiasto.Text, txtKoniecPanstwo.Text, txtKoniecMiasto.Text, txtPoczatekUlica.Text, txtKoniecUlica.Text, txtPoczatekNrDomu.Text, txtKoniecNrDomu.Text, txtPoczatekKodPoczt.Text, txtKoniecKodPoczt.Text);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        lblKPanstw.Content = "Podana nazwa państwa ma nieprawidłowy format!";
                        CzyZamowic = false;
                    }
                    catch (InvalidOperationException)
                    {
                        lblPMiasto.Content = "Podana nazwa miasta ma nieprawidłowy format!";
                        CzyZamowic = false;
                    }
                    catch (ArgumentNullException)
                    {
                        lblPPanstw.Content = "Podana nazwa państwa ma nieprawidłowy format!";
                        CzyZamowic = false;
                    }
                    catch (ArgumentException)
                    {
                        lblKMiasto.Content = "Podana nazwa miasta ma nieprawidłowy format!";
                        CzyZamowic = false;
                    }
                    catch (OverflowException)
                    {
                        lblPUl.Content = "Podana nazwa ulicy ma nieprawidłowy format!";
                        CzyZamowic = false;
                    }
                    catch (OutOfMemoryException)
                    {
                        lblKUl.Content = "Podana nazwa ulicy ma nieprawidłowy format!";
                        CzyZamowic = false;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        lblPKpoczt.Content = "Podany kod pocztowy ma nieprawidłowy format!";
                        CzyZamowic = false;
                    }
                    catch (TimeoutException)
                    {
                        lblKKpoczt.Content = "Podany kod pocztowy ma nieprawidłowy format!";
                        CzyZamowic = false;
                    }
                    catch (FormatException)
                    {
                        lblPKpoczt.Content = "Podany kod pocztowy ma nieprawidłowy format!";
                        CzyZamowic = false;
                    }
                    catch (DivideByZeroException)
                    {
                        lblKNrD.Content = "Podany numer domu ma nieprawidłowy format!";
                        CzyZamowic = false;
                    }
                    catch (ArithmeticException)
                    {
                        lblPNrD.Content = "Podany numer domu ma nieprawidłowy format!";
                        CzyZamowic = false;
                    }

                    if (!Regex.IsMatch(TxtGodzina.Text, Godzina) || !Regex.IsMatch(TxtMinuta.Text, Minuta))
                    {
                        lblczas.Content = "godzina ma niewłaściwy format";
                        CzyZamowic = false;
                    }
                    else
                    {
                        string dzień = Convert.ToString(DPData.SelectedDate).Substring(0, 2);
                        string miesiąc = Convert.ToString(DPData.SelectedDate).Substring(3, 2);
                        string rok = Convert.ToString(DPData.SelectedDate).Substring(6, 4);
                        string godzina = TxtGodzina.Text;
                        string minuta = TxtMinuta.Text;
                        data = rok + "-" + miesiąc + "-" + dzień + " " + godzina + ":" + minuta + ":00";
                        if (Convert.ToDateTime(data) < DateTime.Now.AddDays(1))
                        {
                            lblczas.Content = "Mniej niż 24 godziny do zamowienia";
                            CzyZamowic = false;
                        }
                    }

                    
                }
                con.Close();
            }
            MySqlConnection con2 = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;");
            MySqlConnection con3 = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;");
            con2.Open();
            con3.Open();
            
            int idpojazdu = 1;
            bool czyWolne = true;
            if (CzyZamowic) {
                List<int> listaPojazdow = new List<int>();
                MySqlCommand pojazdy = new MySqlCommand($"SELECT idPojazdy FROM pojazdy where idModel={idModelu} and status_pojazdu like 'Wolny';", con2);
                MySqlDataReader czytnikKtoreAuta = pojazdy.ExecuteReader();
                MySqlCommand IlePojazdow = new MySqlCommand($"SELECT count(IdPojazdy) FROM pojazdy WHERE idModel={idModelu};", con3);
                //Convert.ToInt32(IlePojazdow.ExecuteScalar());
                while (czytnikKtoreAuta.Read())
                {
                    listaPojazdow.Add(Convert.ToInt32(czytnikKtoreAuta["idPojazdy"]));
                }
                czytnikKtoreAuta.Close();
                int y = listaPojazdow.Count();
                idpojazdu = 1;
                List<DateTime> listaDat = new List<DateTime>();
                czyWolne = true;
                for (int j = 0; j < y; j++)
                {

                    MySqlCommand kontrolData = new MySqlCommand($"SELECT zamowiona_data_realizacji FROM zamowienia where idPojazdy ={listaPojazdow[j]} ;", con3);
                    MySqlDataReader daty = kontrolData.ExecuteReader();
                    while (daty.Read())
                    {
                        listaDat.Add(Convert.ToDateTime(daty["zamowiona_data_realizacji"]));
                    }
                    daty.Close();
                    int x = listaDat.Count();
                    for (int i = 0; i < x; i++)
                    {

                        if (Convert.ToDateTime(data).Date == listaDat[i].Date)
                        {
                            czyWolne = false;
                            break;
                        }


                    }
                    if (czyWolne == true)
                    {
                        idpojazdu = listaPojazdow[j];
                    }
                }
                con2.Close();

                if (!czyWolne)
                {
                    lblPuste.Content = "Niestety żaden z pojazdów z wybranego modelu nie jest dostępny w wybranej dacie";
                }
            }



            if (czyWolne && CzyZamowic)
            {
                
                szczegoly_zamowienia sz = new szczegoly_zamowienia();
                mydbEntities baza = new mydbEntities();
                sz.poczatek_panstwo = txtPoczatekPanstwo.Text;
                sz.poczatek_miasto = txtPoczatekMiasto.Text;
                sz.poczatek_ulica = txtPoczatekUlica.Text;
                sz.poczatek_nr_domu = txtPoczatekNrDomu.Text;
                sz.poczatek_kod_pocztowy = txtPoczatekKodPoczt.Text;
                sz.koniec_panstwo = txtKoniecPanstwo.Text;
                sz.koniec_miasto = txtKoniecMiasto.Text;
                sz.koniec_ulica = txtKoniecUlica.Text;
                sz.koniec_nr_domu = txtKoniecNrDomu.Text;
                sz.koniec_kod_pocztowy = txtKoniecKodPoczt.Text;
                sz.cena = null;
                sz.ilosc_km = null;
                baza.szczegoly_zamowienia.Add(sz);
                baza.SaveChanges();
                int idKlienta = baza.klienci.First(t => t.idKonta == idKonta).idKlienci;
                int lastId = baza.szczegoly_zamowienia.OrderByDescending(t => t.idSzczegoly_zamowienia).Take(1).Select(t => t.idSzczegoly_zamowienia).FirstOrDefault();
              
                zamowienia z = new zamowienia();
                z.idKlienci = idKlienta;
                z.data_zlozenia_zamowienia = DateTime.Now;
                z.zamowiona_data_realizacji = Convert.ToDateTime(data);
                z.status_zamowienia = "Oczekuje";
                z.idKierowcy = 1;
                z.idModel = idModelu;
                z.idPojazdy = idpojazdu;
                z.idKonta = idKonta;
                z.idAdministrator = 1;
                z.idSzczegoly_zamowienia = lastId;
                baza.zamowienia.Add(z);
                baza.SaveChanges();

                grdFormularz.Visibility = Visibility.Hidden;
                grdPodsumowanie.Visibility = Visibility.Visible;

                double cena = Convert.ToDouble(sz.cena);
                string kod_pocztowy = $"{sz.poczatek_kod_pocztowy}";
                lblPodsumPocz.Content = $"{sz.poczatek_ulica} {sz.poczatek_nr_domu}\n" +
                                        $"{sz.poczatek_kod_pocztowy} {sz.poczatek_miasto}\n" +
                                        $"{sz.poczatek_panstwo}";
                kod_pocztowy = $"{sz.koniec_kod_pocztowy}";
                lblPodsumKon.Content = $"{sz.koniec_ulica} {sz.koniec_nr_domu}\n" +
                                       $"{sz.koniec_kod_pocztowy} {sz.koniec_miasto}\n" +
                                       $"{sz.koniec_panstwo}";
               // lblCena.Content = $"{sz.ilosc_km}";
                lblRazem.Content = $"Zamówienie czeka na zatwierdzenie i wycenę";
            }
        }

        private void btnZatwierdz_MouseEnter(object sender, MouseEventArgs e)
        {
            btnZatwierdz.Source = new BitmapImage(new Uri("zdjecia/buy_y.png", UriKind.Relative));
            lblZamow.Visibility = Visibility.Visible;
        }

        private void btnZatwierdz_MouseLeave(object sender, MouseEventArgs e)
        {
            btnZatwierdz.Source = new BitmapImage(new Uri("zdjecia/buy_w.png", UriKind.Relative));
            lblZamow.Visibility = Visibility.Hidden;
        }
    }
}
