using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace brutoZarada.Models
{
    public class RadnikIZarada
    {
        public String Ime { set; get; }
        public String Prezime { set; get; }
        public String Adresa { set; get; }
        public String Neto { set; get; }
        public String Bruto1 { set; get; }
        public String Porez_na_dohodak { set; get; }
        public String Doprinos_pio_zaposleni { set; get; }
        public String Doprinos_zdravstvo_zaposleni { set; get; }
        public String Doprinos_nezaposlenost_zaposleni { set; get; }
        public String Ukupan_doprinos_zaposleni { set; get; }
        public String Doprinos_pio_poslodavac { set; get; }
        public String Doprinos_zdravstvo_poslodavac { set; get; }
        public String Doprinos_nezaposlenost_poslodavac { set; get; }
        public String Ukupan_doprinos_poslodavac { set; get; }
        public String Ukupno_svi_doprinosi { set; get; }
        public String Bruto2 { set; get; }
        public String Procenat_dazbina_neto_plate { set; get; }

    }
}