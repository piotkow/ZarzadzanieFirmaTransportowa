using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

enum statusy
{
    Oczekuje,
    Realizacja,
    Zakonczone
}
namespace Firma_Transportowa
{
    class Zamowienie
    {
        public override string ToString()
        {
            return $"Data początku: {Data_poczatku}  Data końca: {Data_konca}  Status zamówienia: {Status}";
        }

        public int Id
        {
            get;
            set;
        }
        public DateTime Data_poczatku
        {
            get;
            set;
        }
        public DateTime Data_konca
        {
            get;
            set;
        }

        public statusy Status
        {
            get;
            set;
        }
    }
}
