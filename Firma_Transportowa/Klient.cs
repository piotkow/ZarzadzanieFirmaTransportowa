using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Firma_Transportowa
{

    public class Klient
    {
        public Klient() { }
        public Klient(string imie,string nazwisko,string numertel)
        {
            this.Imie = imie;
            this.Nazwisko = nazwisko;
            this.NrTelefonu = numertel;
        }
        const string motif = @"^(\d{9})$";
        const string name = @"^[^\s][\p{L}]+$";
        private string imie;
        private string nazwisko;
        private string nrTelefonu;

        public string Imie
        {
            get { return imie; }
            set 
            {
                if (Regex.IsMatch(value, name)) imie = value;
                else throw new ArgumentNullException();
            }
        }
        public string Nazwisko
        {
            get { return nazwisko; }
            set
            {
                if (Regex.IsMatch(value, name)) nazwisko = value;
                else throw new ArgumentException();
            }
        }

        public string NrTelefonu
        {
            get { return nrTelefonu; }
            set
            {
                if (IsPhoneNbr(value)) nrTelefonu = value;
                else throw new ArgumentOutOfRangeException();
            }
        }
        
        bool IsPhoneNbr(string number)
        {
            if (number != null) return Regex.IsMatch(number, motif);
            else return false;
        }
        public override string ToString()
        {
            return $"{IdKlienci}. {Imie} {Nazwisko}";
        }
        public int Weryfikacja { get; set; }
        public int IdKlienci { get; set; }
        public int IdKonta { get; set; }
    }
}
