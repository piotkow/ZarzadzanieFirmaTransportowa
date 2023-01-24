using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Firma_Transportowa
{
    public class SzczegolyZamowienia
    {
        public SzczegolyZamowienia(string pPanstwo,string pMiasto, string kPanstwo, string kMiasto, string pUlica, string kUlica, string pNrDomu, string kNrDomu, string pKod, string kKod)
        {
            this.Poczatek_Panstwo = pPanstwo;
            this.Poczatek_Miato = pMiasto;
            this.Koniec_Panstwo = kPanstwo;
            this.Koniec_Miasto = kMiasto;
            this.Poczatek_Ulica = pUlica;
            this.Koniec_Ulica = kUlica;
            this.Poczatek_NrDomu = pNrDomu;
            this.Koniec_NrDomu = kNrDomu;
            this.Poczatek_KodPocztowy = pKod;
            this.Koniec_KodPocztowy = kKod;
        }

        const string motif = @"^([0-9]{2}-[0-9]{3})$";
        const string name = @"^[a-zA-ZąćęłńóśźżĄĘŁŃÓŚŹŻ]+(?:[\s][a-zA-ZąćęłńóśźżĄĘŁŃÓŚŹŻ]+)*$";
        const string nameMiasto = @"^[a-zA-ZąćęłńóśźżĄĘŁŃÓŚŹŻ]+(?:[\s-][a-zA-ZąćęłńóśźżĄĘŁŃÓŚŹŻ]+)*$";
        const string nameUlica = @"^[A-Za-ząćęłńóśźżĄĘŁŃÓŚŹŻ0-9]+(?:\s[A-Za-ząćęłńóśźżĄĘŁŃÓŚŹŻ0-9'_-]+)*$";
        const string nameNrdomu = @"^\d+(\s|-)?\w*$";
       // const string motifkp = @"^(\d{2})(-\d{4})?$";
        private string p_panstwo;
        private string p_miasto;
        private string k_panstwo;
        private string k_miasto;
        private string p_ulica;
        private string k_ulica;
        private string p_kod;
        private string k_kod;
        private string p_nrdomu;
        private string k_nrdomu;

        public int Id
        {
            get;
            set;
        }

        public string Poczatek_Panstwo
        {
            get { return p_panstwo; }
            set
            {
                if (Regex.IsMatch(value, name)) p_panstwo = value;
                else throw new ArgumentNullException();
            }
        }
        public string Poczatek_Miato
        {
            get { return p_miasto; }
            set
            {
                if (Regex.IsMatch(value, nameMiasto)) p_miasto = value;
                else throw new InvalidOperationException();
            }
        }
        public string Poczatek_Ulica
        {
            get { return p_ulica; }
            set
            {
                if (Regex.IsMatch(value, nameUlica)) p_ulica = value;
                else throw new OverflowException();
            }
        }
        public string Poczatek_NrDomu
        {
            get { return p_nrdomu; }
            set
            {
                if (Regex.IsMatch(value, nameNrdomu)) p_nrdomu = value;
                else throw new ArithmeticException();
            }
        }
        public string Poczatek_KodPocztowy
        {
            get { return p_kod; }
            set
            {
                int a = Convert.ToString(value).Length;
                if (Regex.IsMatch(Convert.ToString(value), motif)) p_kod = value;
                else throw new IndexOutOfRangeException();
            }
        }
        public string Koniec_Panstwo
        {
            get { return k_panstwo; }
            set
            {
                if (Regex.IsMatch(value, name)) k_panstwo = value;
                else throw new ArgumentOutOfRangeException();
            }
        }
        public string Koniec_Miasto
        {
            get { return k_miasto; }
            set
            {
                if (Regex.IsMatch(value, nameMiasto)) k_miasto = value;
                else throw new ArgumentException();
            }
        }
        public string Koniec_Ulica
        {
            get { return k_ulica; }
            set
            {
                if (Regex.IsMatch(value, nameUlica)) k_ulica = value;
                else throw new OutOfMemoryException();
            }
        }
        public string Koniec_NrDomu
        {
            get { return k_nrdomu; }
            set
            {
                if (Regex.IsMatch(value, nameNrdomu)) k_nrdomu = value;
                else throw new DivideByZeroException();
            }
        }
        public string Koniec_KodPocztowy
        {
            get { return k_kod; }
            set
            {
                int a = Convert.ToString(value).Length;
                if (Regex.IsMatch(Convert.ToString(value), motif)) k_kod = value;
                else throw new TimeoutException();
            }
        }
    }
}
