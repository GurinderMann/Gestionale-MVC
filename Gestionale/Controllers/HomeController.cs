using Gestionale.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;



namespace Gestionale.Controllers
{
    public class HomeController : Controller
    {
        List<Spedizione> spedizioni = new List<Spedizione>();

        //Home page
        public ActionResult Index()
        {
            return View();
        }


        //Codice per la ricerca della spedizione
        public ActionResult Spedizioni()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Spedizioni(Spedizione s)
        {
            string conn = ConfigurationManager.ConnectionStrings["GestionaleDB"].ConnectionString;
            SqlConnection sqlConnection = new SqlConnection(conn);
            List<Spedizione> spedizioni = new List<Spedizione>();
            try
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("Select * FROM ANAGRAFE a INNER JOIN Spedizioni s ON a.IdCliente = s.IdCliente WHERE a.[CF o P.IVA] = @CodiceFiscalePiva AND s.IdSpedizione = @NumeroSpedizione;", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@CodiceFiscalePiva", s.CF);
                sqlCommand.Parameters.AddWithValue("@NumeroSpedizione", s.id);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["IdSpedizione"].ToString());
                    DateTime dataSpedizione = Convert.ToDateTime(reader["DataSpedizione"].ToString());
                    int peso = Convert.ToInt32(reader["Peso"].ToString());
                    string destinazione = reader["Destinazione"].ToString();
                    string indirizzo = reader["Indirizzo"].ToString();
                    string nome = reader["Nome"].ToString();
                    decimal costo = Convert.ToDecimal(reader["Costo"].ToString());


                    Spedizione spedizione = new Spedizione
                    {
                        id = id,
                        DataSpedizione = dataSpedizione,
                        Destinazione = destinazione,
                        Peso = peso,
                        indirizzo = indirizzo,
                        Nome = nome,
                        Costo = costo,

                    };
                    spedizioni.Add(spedizione);

                }
                Session["Spedizioni"] = spedizioni;

            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());

            }
            finally
            {

                sqlConnection.Close();
            }

            return RedirectToAction("Lista", "Home");

        }


        //Coidce della lista basata sulla spedizione cercata
        public ActionResult Lista()
        {

            List<Spedizione> spedizioni = Session["Spedizioni"] as List<Spedizione>;


            if (spedizioni != null)
            {
                return View(spedizioni);
            }


            return RedirectToAction("Spedizioni");
        }



        //Codice per ottenere lo stato e i detttagli della spedizione
        public ActionResult Stato(Stato st, int idSpedizione)
        {
            string conn = ConfigurationManager.ConnectionStrings["GestionaleDB"].ConnectionString;
            SqlConnection sqlConnection = new SqlConnection(conn);
            List<Stato> statolist = new List<Stato>();
            Spedizione spedizioneDettaglio = null;

            try
            {
                sqlConnection.Open();

                // Query per ottenere lo stato della spedizione
                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Stato WHERE IdSpedizione = @id ORDER BY DataAggiornamento DESC", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@id", idSpedizione);
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    DateTime dataAggiornamento = Convert.ToDateTime(reader["DataAggiornamento"].ToString());
                    string statoSpedizione = reader["StatoSpedizione"].ToString();

                    Stato stato = new Stato
                    {
                        DataAggiornamento = dataAggiornamento,
                        StatoSpedizione = statoSpedizione
                    };
                    statolist.Add(stato);
                }

                reader.Close();

                // Query per ottenere i dettagli della spedizione
                sqlCommand = new SqlCommand("SELECT * FROM Spedizioni WHERE IdSpedizione = @id", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@id", idSpedizione);
                reader = sqlCommand.ExecuteReader();

                if (reader.Read())
                {
                    int id = Convert.ToInt32(reader["IdSpedizione"].ToString());
                    DateTime dataSpedizione = Convert.ToDateTime(reader["DataSpedizione"].ToString());
                    int peso = Convert.ToInt32(reader["Peso"].ToString());
                    string destinazione = reader["Destinazione"].ToString();
                    string indirizzo = reader["Indirizzo"].ToString();
                    string nome = reader["Nome"].ToString();
                    decimal costo = Convert.ToDecimal(reader["Costo"].ToString());

                    spedizioneDettaglio = new Spedizione
                    {
                        id = id,
                        DataSpedizione = dataSpedizione,
                        Destinazione = destinazione,
                        Peso = peso,
                        indirizzo = indirizzo,
                        Nome = nome,
                        Costo = costo
                    };
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

            // Inserisci i dettagli della spedizione nella ViewBag
            ViewBag.SpedizioneDettaglio = spedizioneDettaglio;

            return View(statolist);
        }




    }
}