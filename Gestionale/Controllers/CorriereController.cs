using Gestionale.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace Gestionale.Controllers
{
    public class CorriereController : Controller
    {

        public ActionResult Index()
        {


            return View();

        }


        //Codice per verificare le spedizioni in attesa di consegna
        public JsonResult AttesaConsegna()
        {
            string conn = ConfigurationManager.ConnectionStrings["GestionaleDB"].ConnectionString;
            SqlConnection sqlConnection = new SqlConnection(conn);

            List<Spedizione> spedizioniNonInConsegna = new List<Spedizione>();

            try
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand("SELECT s.* " +
                                                "FROM SPEDIZIONI s " +
                                                "INNER JOIN STATO st ON s.IdSpedizione = st.IdSpedizione " +
                                                "WHERE st.DataAggiornamento = (SELECT MAX(DataAggiornamento) FROM STATO WHERE IdSpedizione = s.IdSpedizione) " +
                                                "AND st.InConsegna = 0 " +
                                                "AND s.IdSpedizione NOT IN (SELECT IdSpedizione FROM STATO WHERE InConsegna = 1)", sqlConnection);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int Id = Convert.ToInt32(reader["IdSpedizione"]);
                    DateTime datasp = Convert.ToDateTime(reader["DataSpedizione"].ToString());
                    int peso = Convert.ToInt32(reader["Peso"].ToString());
                    string destinazione = reader["Destinazione"].ToString();
                    string indirizzo = reader["Indirizzo"].ToString();
                    string nome = reader["Nome"].ToString();
                    decimal costo = Convert.ToDecimal(reader["Costo"].ToString());

                    Spedizione spedizione = new Spedizione
                    {
                        id = Id,
                        DataSpedizione = datasp,
                        Peso = peso,
                        Destinazione = destinazione,
                        indirizzo = indirizzo,
                        Nome = nome,
                        Costo = costo
                    };

                    spedizioniNonInConsegna.Add(spedizione);
                }

                // Return the list as JSON
                return Json(spedizioniNonInConsegna, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
                return Json(new { error = "An error occurred while fetching data." }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        //Codice per le consegne raggruppate per Città
        public JsonResult Citta()
        {
            string conn = ConfigurationManager.ConnectionStrings["GestionaleDB"].ConnectionString;
            SqlConnection sqlConnection = new SqlConnection(conn);

            List<Spedizione> cittaSpedizioni = new List<Spedizione>();

            try
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT Destinazione, COUNT(IdSpedizione) AS NumeroSpedizioni " +
                    "FROM SPEDIZIONI " +
                    "GROUP BY Destinazione",
                    sqlConnection);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string destinazione = reader["Destinazione"].ToString();
                    int numeroSpedizioni = Convert.ToInt32(reader["NumeroSpedizioni"]);

                    Spedizione citta = new Spedizione
                    {
                        Destinazione = destinazione,
                        NumeroSpedizioni = numeroSpedizioni
                    };

                    cittaSpedizioni.Add(citta);
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
            }


            return Json(cittaSpedizioni, JsonRequestBehavior.AllowGet);
        }


        //Codice per consegne in data odierna
        public JsonResult SpedizioniInConsegnaOggi()
        {
            string conn = ConfigurationManager.ConnectionStrings["GestionaleDB"].ConnectionString;
            SqlConnection sqlConnection = new SqlConnection(conn);

            List<Spedizione> spedizioniInConsegnaOggi = new List<Spedizione>();

            try
            {
                sqlConnection.Open();
                // Ottieni la data odierna
                DateTime dataOdierna = DateTime.Today;

                SqlCommand cmd = new SqlCommand(
                    "SELECT s.* " +
                    "FROM SPEDIZIONI s " +
                    "INNER JOIN STATO st ON s.IdSpedizione = st.IdSpedizione " +
                    "WHERE st.DataAggiornamento = @DataOdierna AND st.InConsegna = 1",
                    sqlConnection);

                // Imposta il parametro per la data odierna
                cmd.Parameters.AddWithValue("@DataOdierna", dataOdierna);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int Id = Convert.ToInt32(reader["IdSpedizione"]);
                    DateTime datasp = Convert.ToDateTime(reader["DataSpedizione"].ToString());
                    int peso = Convert.ToInt32(reader["Peso"].ToString());
                    string destinazione = reader["Destinazione"].ToString();
                    string indirizzo = reader["Indirizzo"].ToString();
                    string nome = reader["Nome"].ToString();
                    decimal costo = Convert.ToDecimal(reader["Costo"].ToString());

                    Spedizione spedizione = new Spedizione
                    {
                        id = Id,
                        DataSpedizione = datasp,
                        Peso = peso,
                        Destinazione = destinazione,
                        indirizzo = indirizzo,
                        Nome = nome,
                        Costo = costo
                    };

                    spedizioniInConsegnaOggi.Add(spedizione);
                }

                // Restituisci la lista come JSON
                return Json(spedizioniInConsegnaOggi, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
                return Json(new { error = "An error occurred while fetching data." }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

    }
}