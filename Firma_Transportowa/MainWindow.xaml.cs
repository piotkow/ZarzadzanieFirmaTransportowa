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
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Controls.Primitives;
using System.Runtime.InteropServices;

namespace Firma_Transportowa
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            wysAutka();
            MySqlConnection con = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;");
            con.Open();
            MySqlCommand CzyJestAdmin = new MySqlCommand($"SELECT login FROM konta WHERE idKonta=1 And EXISTS (SELECT idKonta FROM konta WHERE idKonta=1)", con);
            string czyJest = Convert.ToString(CzyJestAdmin.ExecuteScalar());
            if (czyJest != "admin")
            {
                MySqlCommand command = new MySqlCommand($"INSERT INTO konta VALUES ('1','administrator@wp.pl','admin','admin','Admin')", con);
                command.ExecuteScalar();
            }
            MySqlCommand CzyJestKierowca = new MySqlCommand($"SELECT login FROM konta WHERE idKonta=2 And EXISTS (SELECT idKonta FROM konta WHERE idKonta=2)", con);
            string czyJestKierowca = Convert.ToString(CzyJestKierowca.ExecuteScalar());
            if (czyJestKierowca != "kierowca")
            {
                MySqlCommand command = new MySqlCommand($"INSERT INTO konta VALUES ('2','kierowca@wp.pl','kierowca','kierowca','Kierowca')", con);
                command.ExecuteScalar();
                MySqlCommand command1 = new MySqlCommand($"INSERT INTO kierowcy VALUES ('1','default','kierowca','000000000','1','2')", con);
                command1.ExecuteScalar();
            }
            con.Close();
        }
        static int currentId = 2;
        static int currentIdL = currentId - 1;
        static int currentIdP = currentId + 1;
        List<string> listaZdjec = new List<string>();
        List<string> listaNazw = new List<string>();
        List<double> listaCen = new List<double>();
        public void wysAutka()
        {

            grdAuta.Visibility = Visibility.Visible;
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

            imgLeft.Source = new BitmapImage(new Uri($"zdjecia/{listaZdjec[currentIdL]}", UriKind.Relative));
            imgCenter.Source = new BitmapImage(new Uri($"zdjecia/{listaZdjec[currentId]}", UriKind.Relative));
            imgRight.Source = new BitmapImage(new Uri($"zdjecia/{listaZdjec[currentIdP]}", UriKind.Relative));

            lblTyp.Content = $"Model pojazdu: {listaNazw[currentId]}, Cena za km: {listaCen[currentId].ToString("C")}";

            con.Close();
        }

        private void btnWybierz_Click(object sender, RoutedEventArgs e)
        {
            if (CzyZalogowany && potwierdzacz)
            {
                MySqlConnection conId = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;");
                conId.Open();
                MySqlCommand Id = new MySqlCommand($"SELECT idModel from model_samochodu WHERE zdj_model='{listaZdjec[currentId]}';", conId);
                id_modelu = Convert.ToInt32(Id.ExecuteScalar());
                imgLogo.Width = 120;
                Main.Content = new FormularzZamowienie(m_Id, id_modelu);
                grdAuta.Visibility = Visibility.Hidden;
                conId.Close();
            }

        }
        private void MyGrid_MouseEnter(object sender, RoutedEventArgs e)
        {
            Color transpszaryciemny = new Color();
            transpszaryciemny.A = 200;
            transpszaryciemny.R = 70;
            transpszaryciemny.G = 70;
            transpszaryciemny.B = 70;
            Grid grd = sender as Grid;
            if (grd.Name != "koks")
            {
                grd.Background = new SolidColorBrush(transpszaryciemny);
                //grd.Height = 95;
                // g_id = grd.Name;
            }
        }
        private void MyGrid_MouseLeave(object sender, RoutedEventArgs e)
        {
            Color transpszaryciemny = new Color();
            transpszaryciemny.A = 160;
            transpszaryciemny.R = 30;
            transpszaryciemny.G = 30;
            transpszaryciemny.B = 30;
            Grid grd = sender as Grid;
            if (grd.Name != "koks")
            {
                grd.Background = new SolidColorBrush(transpszaryciemny);
                // grd.Height = 90;

                // g_id = grd.Name;
            }
        }

        int m_Id;
        int id_modelu;
        bool CzyZalogowany = false;
        bool potwierdzacz = false;
        public void setId(int id)
        {
            m_Id = id;
        }
        public int getId()
        {
            return m_Id;
        }
        private void btnLogowanie_Click(object sender, RoutedEventArgs e)
        {
            Logowanie okno_logowania = new Logowanie();
            okno_logowania.ShowDialog();
            if (okno_logowania.getCzyZalogowac())
            {
                btnLogowanie.Visibility = Visibility.Hidden;
                btnRejestracja.Visibility = Visibility.Hidden;
                btnWyloguj.Visibility = Visibility.Visible;
                switch (okno_logowania.getKonto().Rola)
                {
                    case Role.Klient:
                        btnStronaGlowna.Visibility = Visibility.Visible;
                        btnPanelKlienta.Visibility = Visibility.Visible; break;
                    case Role.Admin:
                        btnAdministrator.Visibility = Visibility.Visible;
                        btnStronaGlowna.Visibility = Visibility.Visible;
                        //btnKierowca.Visibility = Visibility.Visible;
                        break;
                    case Role.Kierowca:
                        btnKierowca.Visibility = Visibility.Visible;
                        btnStronaGlowna.Visibility = Visibility.Visible;
                        btnPanelKlienta.Visibility = Visibility.Visible; break;
                }
            }
            setId(okno_logowania.getKonto().Id);
            if (okno_logowania.getCzyZalogowac())
            {
                CzyZalogowany = true;
                // btnWybierz.IsEnabled = true;
                lblZalAbyZamowic1.Visibility = Visibility.Hidden;
                MySqlConnection con = new MySqlConnection("server=localhost;database=mydb;Uid=root;Pwd=root;");
                con.Open();
                MySqlCommand potwierdzenie = new MySqlCommand($"SELECT weryfikacja FROM Klienci WHERE idKonta = {m_Id};", con);
                int k = Convert.ToInt32(potwierdzenie.ExecuteScalar());
                if (k == 1)
                {
                    potwierdzacz = true;
                    btnWybierz.IsEnabled = true;
                }
            }
        }

        private void btnRejestracja_Click(object sender, RoutedEventArgs e)
        {
            Rejestracja okno_rejestracji = new Rejestracja();
            okno_rejestracji.ShowDialog();
        }
        private void btnAdministrator_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Main.Content = new Administrator(getId());
            grdAuta.Visibility = Visibility.Hidden;
            imgLogo.Width = 120;
        }

        private void btnKierowca_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Main.Content = new PanelKierowcy(getId());
            grdAuta.Visibility = Visibility.Hidden;
            imgLogo.Width = 120;
        }

        private void myFrame_ContentRendered(object sender, EventArgs e)
        {
            Main.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
        }

        private void btnWyloguj_Click(object sender, RoutedEventArgs e)
        {
            btnLogowanie.Visibility = Visibility.Visible;
            btnRejestracja.Visibility = Visibility.Visible;
            btnAdministrator.Visibility = Visibility.Hidden;
            btnStronaGlowna.Visibility = Visibility.Hidden;
            btnKierowca.Visibility = Visibility.Hidden;
            btnWyloguj.Visibility = Visibility.Hidden;
            Main.Content = null;
            //btnFormularzZamowienie.Visibility = Visibility.Hidden;
            wysAutka();
            lblZalAbyZamowic1.Visibility = Visibility.Visible;
            btnPanelKlienta.Visibility = Visibility.Hidden;
            CzyZalogowany = false;
            imgLogo.Width = 288;
            potwierdzacz = false;
            btnWybierz.IsEnabled = false;
        }

        private void btnPanelKlienta_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Main.Content = new PanelKlienta(getId());
            grdAuta.Visibility = Visibility.Hidden;
            imgLogo.Width = 120;
        }

        private void btnStronaGlowna_MouseEnter(object sender, MouseEventArgs e)
        {
            btnStronaGlowna.Source = new BitmapImage(new Uri("zdjecia/home_y.png", UriKind.Relative));
            panelLabeli.Visibility = Visibility.Visible;
            lbl1.Visibility = Visibility.Visible;
        }

        private void btnStronaGlowna_MouseLeave(object sender, MouseEventArgs e)
        {
            btnStronaGlowna.Source = new BitmapImage(new Uri("zdjecia/home_w.png", UriKind.Relative));
            panelLabeli.Visibility = Visibility.Hidden;
            lbl1.Visibility = Visibility.Hidden;
        }

        private void btnStronaGlowna_MouseDown(object sender, MouseButtonEventArgs e)
        {
            listaNazw.Clear();
            listaZdjec.Clear();
            listaCen.Clear();
            Main.Content = null;
            currentId = 2;
            currentIdL = currentId - 1;
            currentIdP = currentId + 1;
            wysAutka();
            imgLogo.Width = 288;

        }

        private void btnPanelKlienta_MouseEnter(object sender, MouseEventArgs e)
        {
            btnPanelKlienta.Source = new BitmapImage(new Uri("zdjecia/customer_y.png", UriKind.Relative));
            panelLabeli.Visibility = Visibility.Visible;
            lbl2.Visibility = Visibility.Visible;
        }

        private void btnPanelKlienta_MouseLeave(object sender, MouseEventArgs e)
        {
            btnPanelKlienta.Source = new BitmapImage(new Uri("zdjecia/customer_w.png", UriKind.Relative));
            panelLabeli.Visibility = Visibility.Hidden;
            lbl2.Visibility = Visibility.Hidden;
        }

        private void btnKierowca_MouseEnter(object sender, MouseEventArgs e)
        {
            btnKierowca.Source = new BitmapImage(new Uri("zdjecia/truck_y.png", UriKind.Relative));
            panelLabeli.Visibility = Visibility.Visible;
            lbl3.Visibility = Visibility.Visible;
        }

        private void btnKierowca_MouseLeave(object sender, MouseEventArgs e)
        {
            btnKierowca.Source = new BitmapImage(new Uri("zdjecia/truck_w.png", UriKind.Relative));
            panelLabeli.Visibility = Visibility.Hidden;
            lbl3.Visibility = Visibility.Hidden;
        }

        private void btnAdministrator_MouseEnter(object sender, MouseEventArgs e)
        {
            btnAdministrator.Source = new BitmapImage(new Uri("zdjecia/admin_y.png", UriKind.Relative));
            panelLabeli.Visibility = Visibility.Visible;
            lbl4.Visibility = Visibility.Visible;
        }

        private void btnAdministrator_MouseLeave(object sender, MouseEventArgs e)
        {
            btnAdministrator.Source = new BitmapImage(new Uri("zdjecia/admin_w.png", UriKind.Relative));
            panelLabeli.Visibility = Visibility.Hidden;
            lbl4.Visibility = Visibility.Hidden;
        }

        private void btnFormularzZamowienie_MouseEnter(object sender, MouseEventArgs e)
        {
            //btnFormularzZamowienie.Source = new BitmapImage(new Uri("order_y.png", UriKind.Relative));
            panelLabeli.Visibility = Visibility.Visible;
            //lbl5.Visibility = Visibility.Visible;
        }

        private void btnFormularzZamowienie_MouseLeave(object sender, MouseEventArgs e)
        {
            //btnFormularzZamowienie.Source = new BitmapImage(new Uri("order_w.png", UriKind.Relative));
            panelLabeli.Visibility = Visibility.Hidden;
            //lbl5.Visibility = Visibility.Hidden;
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnMinimalize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = (WindowState)System.Windows.Forms.FormWindowState.Minimized;
        }

        private void imgLeft_MouseDown(object sender, MouseButtonEventArgs e)
        {
            currentIdP = currentId;
            currentId = currentIdL;
            currentIdL--;

            if (currentId == 0) currentIdL = (listaZdjec.Count() - 1);
            wysAutka();
        }

        private void imgRight_MouseDown(object sender, MouseButtonEventArgs e)
        {
            currentIdL = currentId;
            currentId = currentIdP;
            currentIdP++;
            if (currentId == listaZdjec.Count() - 1) currentIdP = 0;
            wysAutka();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}