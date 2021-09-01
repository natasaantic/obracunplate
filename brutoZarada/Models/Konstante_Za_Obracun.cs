using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace brutoZarada.Models
{
    public class Konstante_Za_Obracun
    {
        public const double koef_bruto2 = 1.67;

        public const double umanjenje_poreza = 18300;
        public const double koef_za_porez = 0.1;

        public const double koef_za_doprinos_pio__zaposlenog = 0.14;
        public const double koef_za_doprinos_zdravstva_zaposlenog = 0.0515;
        public const double koef_za_doprinos_nezaposlenost_zaposlenog = 0.0075;

        public const double koef_za_doprinos_pio_poslodavca = 0.115;
        public const double koef_za_doprinos_zdravstva_poslodavca = 0.0515;
        public const double koef_za_doprinos_nezapos_poslodavca = 0;
    }
}