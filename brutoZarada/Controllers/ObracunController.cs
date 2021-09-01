using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using brutoZarada.Models;
using Baza;
using System.Data;

namespace brutoZarada.Controllers
{
    public class ObracunController : Controller
    {
        // GET: Obracun
        public ActionResult PrikaziObracunZarada(RadnikIZarada radnikIZarada)
        {

            /*Obracun :*/
            double neto = double.Parse(radnikIZarada.Neto.ToString());
            double bruto1 = neto * Konstante_Za_Obracun.koef_bruto2;
              radnikIZarada.Bruto1 = bruto1.ToString();

            double osnovica_za_porez = bruto1 - Konstante_Za_Obracun.umanjenje_poreza;
            double porez_na_zaradu = osnovica_za_porez * Konstante_Za_Obracun.koef_za_porez;
              radnikIZarada.Porez_na_dohodak = porez_na_zaradu.ToString();

            double doprinos_za_pio_zaposlenog = bruto1 * Konstante_Za_Obracun.koef_za_doprinos_pio__zaposlenog;
              radnikIZarada.Doprinos_pio_zaposleni = doprinos_za_pio_zaposlenog.ToString();
            double doprinos_za_zdravstvo_zaposlenog = bruto1 * Konstante_Za_Obracun.koef_za_doprinos_zdravstva_zaposlenog;
              radnikIZarada.Doprinos_zdravstvo_zaposleni = doprinos_za_zdravstvo_zaposlenog.ToString();
            double doprinos_za_nezaposlenost_zaposlenog = bruto1 * Konstante_Za_Obracun.koef_za_doprinos_nezaposlenost_zaposlenog;
              radnikIZarada.Doprinos_nezaposlenost_zaposleni = doprinos_za_nezaposlenost_zaposlenog.ToString();            
            double ukupan_doprinos_zaposlenog = doprinos_za_pio_zaposlenog + doprinos_za_zdravstvo_zaposlenog + doprinos_za_nezaposlenost_zaposlenog;
              radnikIZarada.Ukupan_doprinos_zaposleni = ukupan_doprinos_zaposlenog.ToString();

            double dopirnos_za_pio_poslodavca = bruto1 * Konstante_Za_Obracun.koef_za_doprinos_pio_poslodavca;
              radnikIZarada.Doprinos_pio_poslodavac = dopirnos_za_pio_poslodavca.ToString();
            double doprinos_za_zdravstvo_poslodavca = bruto1 * Konstante_Za_Obracun.koef_za_doprinos_zdravstva_poslodavca;
              radnikIZarada.Doprinos_zdravstvo_poslodavac = doprinos_za_zdravstvo_poslodavca.ToString();            
            double doprinos_za_nezaposlesnost_poslodavca = bruto1 * Konstante_Za_Obracun.koef_za_doprinos_nezapos_poslodavca;
              radnikIZarada.Doprinos_nezaposlenost_poslodavac = doprinos_za_nezaposlesnost_poslodavca.ToString();
            double ukupan_doprinos_poslodavca = dopirnos_za_pio_poslodavca + doprinos_za_zdravstvo_poslodavca + doprinos_za_nezaposlesnost_poslodavca;
              radnikIZarada.Ukupan_doprinos_poslodavac = ukupan_doprinos_poslodavca.ToString();

            double ukupno_svi_doprinosi = ukupan_doprinos_zaposlenog + ukupan_doprinos_poslodavca;
              radnikIZarada.Ukupno_svi_doprinosi = ukupno_svi_doprinosi.ToString();

            double bruto2 = bruto1 + ukupan_doprinos_poslodavca;
              radnikIZarada.Bruto2 = bruto2.ToString();

            double procenat_dazbina_neto_plate = Math.Round(((porez_na_zaradu+ukupan_doprinos_zaposlenog+ukupan_doprinos_poslodavca) / neto)*100,2);
              radnikIZarada.Procenat_dazbina_neto_plate = procenat_dazbina_neto_plate.ToString();

            /*kraj obracuna*/

            /* snimanje u bazu ObracunZarade */
            CatalogAccess.UnesiNovuZaradu(radnikIZarada);
            List<RadnikIZarada> listaZarada = CatalogAccess.PrikazObracunaZarada();

            return View(listaZarada);
        }
    }
}