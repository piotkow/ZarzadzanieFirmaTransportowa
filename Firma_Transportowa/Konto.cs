using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Firma_Transportowa
{
    public enum Role
    {
        Kierowca,
        Klient,
        Admin
    }
    
    public class Konto
    {
        private string haslo;
        private string email;
        private string login;
        public Konto()
        {

        }
        const string mail = @"^\s*[\w\-\+_']+(\.[\w\-\+_']+)*\@[A-Za-z0-9]([\w\.-]*[A-Za-z0-9])?\.[A-Za-z][A-Za-z\.]*[A-Za-z]$";
        public Konto(string email, string login, string haslo)
        {
            this.Email = email;
            this.Login = login;
            this.Haslo = haslo;
        }

        public string Email
        {
            get { return email; }
            set 
            {
                if (Regex.IsMatch(value, mail)) email = value;
                else throw new ArgumentException();
            }
        }

        public string Login
        {
            get { return login; }
            set
            {
                if (value.Length < 5 || value.Length > 25) throw new ArgumentNullException();
                else login = value;
            }
        }

        public string Haslo
        {
            get{ return haslo; }
            set{
                if (value.Length < 5 || value.Length > 25) throw new ArgumentOutOfRangeException();
                else haslo = value;
            }
        }

        public Role Rola
        {
            get;
            set;
        }
        public int Id
        {
            get; set;
        }
    }
}
