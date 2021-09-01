using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System;
using System.Data.OleDb;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using brutoZarada.Models;

namespace Baza
{
    public static class EagleConfiguration
    {
        // Caches the connection string 
        private static string dbConnectionString;
        // Caches the data provider name 
        private static string dbProviderName;
        static EagleConfiguration()
        {
            dbConnectionString = ConfigurationManager.ConnectionStrings["ObracunZaradeKonekcija"].ConnectionString;
            dbProviderName = ConfigurationManager.ConnectionStrings["ObracunZaradeKonekcija"].ProviderName;
        }
        // Vraca konekcioni string za bazu ObracunZarade

        public static string DbConnectionString
        {
            get
            {
                return dbConnectionString;
            }
        }
        public static string DbProviderName
        {
            get { return dbProviderName; }
        }

    }
    public static class GenericDataAccess
    {
        // static constructor 
        static GenericDataAccess()
        {
            // TODO: Add constructor logic here 
        }
        public static DbCommand SQLNaredba()
        {
            // Obtain the database provider name 
            string dataProviderName = EagleConfiguration.DbProviderName;
            // Obtain the database connection string 
            string connectionString = EagleConfiguration.DbConnectionString;
            // Create a new data provider factory 
            DbProviderFactory factory = DbProviderFactories.GetFactory(dataProviderName);
            // Obtain a database-specific connection object 
            DbConnection conn = factory.CreateConnection();
            // Set the connection string 
            conn.ConnectionString = connectionString;
            // Create a database-specific command object 
            DbCommand comm = conn.CreateCommand();
            comm.CommandType = CommandType.Text;
            return comm;
        }
        // creates and prepares a new DbCommand object on a new connection 
        public static DbCommand CreateCommand()
        {
            // Obtain the database provider name 
            string dataProviderName = EagleConfiguration.DbProviderName;
            // Obtain the database connection string 
            string connectionString = EagleConfiguration.DbConnectionString;
            // Create a new data provider factory 
            DbProviderFactory factory = DbProviderFactories.GetFactory(dataProviderName);
            // Obtain a database-specific connection object 
            DbConnection conn = factory.CreateConnection();
            // Set the connection string 
            conn.ConnectionString = connectionString;
            // Create a database-specific command object 
            DbCommand comm = conn.CreateCommand();
            // Set the command type to stored procedure 
            comm.CommandType = CommandType.StoredProcedure;
            // Return the initialized command object 
            return comm;
        }
        public static DbCommand CreateTable()
        {
            // Obtain the database provider name 
            string dataProviderName = EagleConfiguration.DbProviderName;
            // Obtain the database connection string 
            string connectionString = EagleConfiguration.DbConnectionString;
            // Create a new data provider factory 
            DbProviderFactory factory = DbProviderFactories.GetFactory(dataProviderName);
            // Obtain a database-specific connection object 
            DbConnection conn = factory.CreateConnection();
            // Set the connection string 
            conn.ConnectionString = connectionString;
            // Create a database-specific command object 
            DbCommand comm = conn.CreateCommand();
            // Set the command type to stored procedure 
            comm.CommandType = CommandType.TableDirect;
            // Return the initialized command object 
            return comm;
        }



        // executes a command and returns the results as a DataTable object
        public static DataTable ExecuteSelectCommand(DbCommand command)
        {
            // The DataTable to be returned 
            DataTable table = new DataTable();
            // Execute the command making sure the connection gets closed in the end
            try
            {
                // Open the data connection 
                command.Connection.Open();
                // Execute the command and save the results in a DataTable
                DbDataReader reader = command.ExecuteReader();
                table.Load(reader);

                // Close the reader 
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Exception caught", ex);

                //Utilitiess.LogError(ex);                
                throw;
            }
            finally
            {
                // Close the connection
                command.Connection.Close();
            }
            return table;
        }

        // execute an update, delete, or insert command 
        // and return the number of affected rows
        public static int ExecuteNonQuery(DbCommand command)
        {
            // The number of affected rows 
            int affectedRows = -1;
            // Execute the command making sure the connection gets closed in the end
            // Open the connection of the command
            command.Connection.Open();
            // Execute the command and get the number of affected rows
            affectedRows = command.ExecuteNonQuery();
            /**5**/
            // return the number of affected rows
            return affectedRows;
        }

        // execute a select command and return a single result as a string
        public static string ExecuteScalar(DbCommand command)
        {
            // The value to be returned 
            string value = "";
            // Open the connection of the command
            command.Connection.Open();
            // Execute the command and get the number of affected rows
            value = command.ExecuteScalar().ToString();
            command.Connection.Close();
            // return the result
            return value;
        }
    }
    public static class CatalogAccess
    {
        static CatalogAccess()
        {
            // // TODO: Add constructor logic here // 
        }

        public static List<RadnikIZarada> PrikazObracunaZarada()
        {           
            DbCommand comm = GenericDataAccess.SQLNaredba();
            comm.CommandText = "select * from RadniciIZarade";
            DataTable dt = GenericDataAccess.ExecuteSelectCommand(comm);

            List<RadnikIZarada> listaZarada = new List<RadnikIZarada>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];

                    RadnikIZarada zarada = new RadnikIZarada();
                    
                    zarada.Ime = dr["Ime"].ToString();
                    zarada.Prezime = dr["Prezime"].ToString();
                    zarada.Adresa = dr["Adresa"].ToString();

                    zarada.Neto = dr["Neto"].ToString();
                    zarada.Bruto1 = dr["Bruto1"].ToString();
                    zarada.Porez_na_dohodak = dr["Porez_na_dohodak"].ToString();

                    zarada.Doprinos_pio_zaposleni = dr["Doprinos_pio_zaposleni"].ToString();
                    zarada.Doprinos_zdravstvo_zaposleni = dr["Doprinos_zdravstvo_zaposleni"].ToString();
                    zarada.Doprinos_nezaposlenost_zaposleni = dr["Doprinos_nezaposlenost_zaposleni"].ToString();
                    zarada.Ukupan_doprinos_zaposleni = dr["Ukupan_doprinos_zaposleni"].ToString();

                    zarada.Doprinos_pio_poslodavac = dr["Doprinos_pio_poslodavac"].ToString();
                    zarada.Doprinos_zdravstvo_poslodavac = dr["Doprinos_zdravstvo_poslodavac"].ToString();
                    zarada.Doprinos_nezaposlenost_poslodavac = dr["Doprinos_nezaposlenost_poslodavac"].ToString();
                    zarada.Ukupan_doprinos_poslodavac = dr["Ukupan_doprinos_poslodavac"].ToString();

                    zarada.Ukupno_svi_doprinosi = dr["Ukupno_svi_doprinosi"].ToString();

                    zarada.Bruto2 = dr["Bruto2"].ToString();

                    zarada.Procenat_dazbina_neto_plate = dr["Procenat_dazbina_neto_plata"].ToString();

                    listaZarada.Add(zarada);
                }
            }
            return listaZarada;
        }

        private static int radnik_Je_U_bazi(RadnikIZarada radnik_i_zarada)
        {
            DbCommand comm = GenericDataAccess.SQLNaredba();
            comm.CommandText = "select count(*) as Radnik_je_u_bazi from RadniciIZarade where Ime='" + radnik_i_zarada.Ime + "' and Prezime='" + radnik_i_zarada.Prezime + "' and Adresa='" + radnik_i_zarada.Adresa + "'";
            int postoji = int.Parse(GenericDataAccess.ExecuteScalar(comm));
            return postoji;
        }

        public static int Id_Radnika(string ime, string prezime, string adresa)
        {
            DbCommand comm = GenericDataAccess.SQLNaredba();
            comm.CommandText = "select Id from Radnici where Ime='" + ime + "' and Prezime='" + prezime + "' and Adresa='" + adresa + "'";
            int id = int.Parse(GenericDataAccess.ExecuteScalar(comm));
            return id;
        }

        private static void Izmeni_Zaradu(RadnikIZarada radnik_i_zarada)
        {
            
            DbCommand comm = GenericDataAccess.SQLNaredba();
            comm.CommandText = "update RadniciIZarade set neto = '"+radnik_i_zarada.Neto+"', Bruto1 = '"+radnik_i_zarada.Bruto1+"', Porez_na_dohodak = '"+radnik_i_zarada.Porez_na_dohodak+"', Doprinos_pio_zaposleni = '"+radnik_i_zarada.Doprinos_pio_zaposleni+"', Doprinos_zdravstvo_zaposleni = '"+radnik_i_zarada.Doprinos_zdravstvo_zaposleni+"', Doprinos_nezaposlenost_zaposleni = '"+radnik_i_zarada.Doprinos_nezaposlenost_zaposleni+"', Ukupan_doprinos_zaposleni = '"+radnik_i_zarada.Ukupan_doprinos_zaposleni+"', Doprinos_pio_poslodavac = '"+radnik_i_zarada.Doprinos_pio_poslodavac+"', Doprinos_zdravstvo_poslodavac = '"+radnik_i_zarada.Doprinos_zdravstvo_poslodavac+"', Doprinos_nezaposlenost_poslodavac = '"+radnik_i_zarada.Doprinos_nezaposlenost_poslodavac+"', Ukupan_doprinos_poslodavac = '"+radnik_i_zarada.Ukupan_doprinos_poslodavac+"', Ukupno_svi_doprinosi = '"+radnik_i_zarada.Ukupno_svi_doprinosi+"', Bruto2 = '"+radnik_i_zarada.Bruto2+"', Procenat_dazbina_neto_plata = '"+radnik_i_zarada.Procenat_dazbina_neto_plate+"' where Ime = '"+radnik_i_zarada.Ime+"' and Prezime = '"+radnik_i_zarada.Prezime+"' and Adresa = '"+radnik_i_zarada.Adresa+"'";
            GenericDataAccess.ExecuteNonQuery(comm);

        }

        public static int broj_Radnika()
        {
            DbCommand comm = GenericDataAccess.SQLNaredba();
            comm.CommandText = "select count(*) from Radnici";
            int brojRadnika = int.Parse(GenericDataAccess.ExecuteScalar(comm));
            return brojRadnika;
        }

        public static void UnesiNovogRadnika(int red_br_radnika, string ime, string prezime, string adresa)
        {
            DbCommand comm = GenericDataAccess.SQLNaredba();
            comm.CommandText = "insert into Radnici values (" + red_br_radnika + ",'" + ime + "','" + prezime + "','" + adresa + "')";
            GenericDataAccess.ExecuteNonQuery(comm);
        }

        public static int broj_Zarada()
        {
            DbCommand comm = GenericDataAccess.SQLNaredba();
            comm.CommandText = "select count(*) from Zarade";
            int brojZarada = int.Parse(GenericDataAccess.ExecuteScalar(comm));
            return brojZarada;
        }

        public static void UnosNoveZarade(RadnikIZarada radnik_i_zarada)
        {
            DbCommand comm = GenericDataAccess.SQLNaredba();
            comm.CommandText = "insert into RadniciIZarade values('"+radnik_i_zarada.Ime+"', '"+radnik_i_zarada.Prezime+"', '"+radnik_i_zarada.Adresa+"','"+radnik_i_zarada.Neto+"', '"+radnik_i_zarada.Bruto1+"', '"+radnik_i_zarada.Porez_na_dohodak+"', '"+radnik_i_zarada.Doprinos_pio_zaposleni+"', '"+radnik_i_zarada.Doprinos_zdravstvo_zaposleni+"', '"+radnik_i_zarada.Doprinos_nezaposlenost_zaposleni+"', '"+radnik_i_zarada.Ukupan_doprinos_zaposleni+"', '"+radnik_i_zarada.Doprinos_pio_poslodavac+"', '"+radnik_i_zarada.Doprinos_zdravstvo_poslodavac+"', '"+radnik_i_zarada.Doprinos_nezaposlenost_poslodavac+"', '"+radnik_i_zarada.Ukupan_doprinos_poslodavac+"', '"+radnik_i_zarada.Ukupno_svi_doprinosi+"', '"+radnik_i_zarada.Bruto2+"', '"+radnik_i_zarada.Procenat_dazbina_neto_plate+"')";
            GenericDataAccess.ExecuteNonQuery(comm);
        }

        public static void UnesiNovuZaradu(RadnikIZarada radnik_i_zarada)
        {
            if (radnik_Je_U_bazi(radnik_i_zarada) == 1)
            {
                //int id_radnika = Id_Radnika(ime, prezime, adresa);
                Izmeni_Zaradu(radnik_i_zarada);
            }
            else
            {
                UnosNoveZarade(radnik_i_zarada);
            }
        }
        /*1
         * 
         * **/

    }
    public static class Utility
    {
        private static DataTable dodavanjeUTabelu(DataTable dt, string[] rows)
        {
            if (rows.Length > 1)
            {
                DataRow dr = dt.NewRow();
                for (int i = 0; i < rows.Length; i++)
                {
                    dr[i] = rows[i].Trim();
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                if (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    for (int i = 0; i < rows.Length; i++)
                    {
                        dt.Columns.Add("ccc" + i);
                    }
                    dt = dodavanjeUTabelu(dt, rows);

                    while (!sr.EndOfStream)
                    {
                        rows = sr.ReadLine().Split(',');
                        dt = dodavanjeUTabelu(dt, rows);
                    }
                }
            }

            return dt;
        }

        public static List<string> ConvertTXTtoList(string strFilePath)
        {
            List<string> cardNo = new List<string>();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    cardNo.Add(rows[3]);
                }
            }
            return cardNo;
        }

        public static DataTable ConvertXSLXtoDataTable(string strFilePath, string connString)
        {
            OleDbConnection oledbConn = new OleDbConnection(connString);
            DataTable dt = new DataTable();
            try
            {
                oledbConn.Open();
                OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn);
                OleDbDataAdapter oleda = new OleDbDataAdapter();
                oleda.SelectCommand = cmd;
                DataSet ds = new DataSet();
                oleda.Fill(ds);

                dt = ds.Tables[0];
            }
            catch
            {
            }
            finally
            {

                oledbConn.Close();
            }

            return dt;

        }
        public static List<string> ConvertXSLXtoList(string strFilePath, string connString)
        {
            OleDbConnection oledbConn = new OleDbConnection(connString);
            DataTable dt = new DataTable();
            List<string> list = new List<string>();
            try
            {

                oledbConn.Open();
                OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn);
                OleDbDataAdapter oleda = new OleDbDataAdapter();
                oleda.SelectCommand = cmd;
                DataSet ds = new DataSet();
                oleda.Fill(ds);

                dt = ds.Tables[0];

            }
            catch
            {
            }
            finally
            {
                oledbConn.Close();
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                list.Add(dt.Rows[i][3].ToString());
            }

            return list;

        }
    }
    public static class Autentifikacija
    {
        //public static bool ulogovan = false;
        public static bool postavljenAdmin = false;
        public static int ulogaID = -1;
    }
    public static class Korisnik
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public static string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public static string Password { get; set; }

        [Display(Name = "Remember me?")]
        public static bool RememberMe { get; set; }
    }


    public static class Inicijalizacija
    {
        public static DateTime od, Do;
        public static DataTable brojPristupaPoInstitutima;
        public static DataTable brojPristupaZaKorisnika;
        public static DataTable pregledPristupa;
        //public static List<Department> departmani = CatalogAccess.PrikazDepartmana();
        public static List<List<SelectListItem>> listaDropDownListi = init();
        public static string ime;
        public static int brojPristupa;

        public static void init_brojPristupaPoInstitutima()
        {
            brojPristupaPoInstitutima = new DataTable();
            brojPristupaPoInstitutima.Columns.Add("Institut", typeof(string));
            brojPristupaPoInstitutima.Columns.Add("Broj pristupa", typeof(string));
        }
        public static void init_brojPristupaZaKorisnika()
        {
            brojPristupaZaKorisnika = new DataTable();
            brojPristupaZaKorisnika.Columns.Add("vremedatum", typeof(string));
            brojPristupaZaKorisnika.Columns.Add("CardNo", typeof(string));
            brojPristupaZaKorisnika.Columns.Add("CardName", typeof(string));
            brojPristupaZaKorisnika.Columns.Add("FirstName", typeof(string));
            brojPristupaZaKorisnika.Columns.Add("LastName", typeof(string));

        }
        public static void init_PregledPristupa()
        {
            pregledPristupa = new DataTable();
            pregledPristupa.Columns.Add("vremedatum", typeof(string));
            pregledPristupa.Columns.Add("CardNo", typeof(string));
            pregledPristupa.Columns.Add("CardName", typeof(string));
            pregledPristupa.Columns.Add("FirstName", typeof(string));
            pregledPristupa.Columns.Add("LastName", typeof(string));
        }
        public static List<List<SelectListItem>> init()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            //List<Department> departLista = departmani; //System.Diagnostics.Debug.WriteLine("ab ab : 1");
            items.Add(new SelectListItem { Text = "---", Value = "-1" });
            /*foreach (var departman in departLista)
            {
                items.Add(new SelectListItem { Text = departman.Name, Value = departman.DepartmentID.ToString() });
            }*/

            //ViewBag.DepartmanTip = items;

            List<SelectListItem> listItems = new List<SelectListItem>(); //System.Diagnostics.Debug.WriteLine("ab ab : 2");
            //List<Employee> studentiLista = CatalogAccess.prikaziStudente(); //System.Diagnostics.Debug.WriteLine("ab ab : 3");
            //studentiLista.Sort();
            listItems.Add(new SelectListItem
            {
                Text = "---",
                Value = "-1"
            });
            /*
            for (int i = 0; i < studentiLista.Count; i++)
            {
                listItems.Add(new SelectListItem
                {
                    Text = studentiLista[i].ispisDropDown(),
                    Value = studentiLista[i].EmployeeID.ToString()
                });
            }
            */
            //ViewBag.StudentiTip = listItems;
            //System.Diagnostics.Debug.WriteLine("ab ab : 4");
            List<List<SelectListItem>> l = new List<List<SelectListItem>>();
            l.Add(items); l.Add(listItems);
            //System.Diagnostics.Debug.WriteLine("ab ab : 5");
            return l;
        }
    }
 /**4**/
}
