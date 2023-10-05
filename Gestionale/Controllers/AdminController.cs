using Gestionale.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace Gestionale.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        List<Spedizione> spedizioni = new List<Spedizione>();
        List<Spedizione> Attesa = new List<Spedizione>();


        //Home page admin
        public ActionResult Index()
        {
            string conn = ConfigurationManager.ConnectionStrings["GestionaleDB"].ConnectionString;
            SqlConnection sqlConnection = new SqlConnection(conn);

            try
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM SPEDIZIONI ", sqlConnection);
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

                    spedizioni.Add(spedizione);
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
            finally { sqlConnection.Close(); }

            Spedizione.SpedizioneList = spedizioni;
            return View(Spedizione.SpedizioneList);
        }


        //Codice per modificare la spedizione
        public ActionResult Modifica(int idSpedizione)
        {
            Spedizione spedizione = new Spedizione();
            foreach (Spedizione s in Spedizione.SpedizioneList)
            {
                if (s.id == idSpedizione)
                {
                    spedizione = s; break;
                }
            }
            return View(spedizione);
        }

        [HttpPost]

        public ActionResult Modifica(Spedizione s)
        {
            if (ModelState.IsValid)
            {
                string conn = ConfigurationManager.ConnectionStrings["GestionaleDB"].ConnectionString;
                SqlConnection sqlConnection = new SqlConnection(conn);
                List<Spedizione> spedizioni = new List<Spedizione>();
                try
                {
                    sqlConnection.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE  SPEDIZIONI SET DataSpedizione = @DataSpedizione, Peso = @Peso, Destinazione = @Destinazione, Indirizzo = @indirizzo, Nome = @Nome, Costo=@Costo", sqlConnection);
                    cmd.Parameters.AddWithValue("@DataSpedizione", s.DataSpedizione);
                    cmd.Parameters.AddWithValue("@Peso", s.Peso);
                    cmd.Parameters.AddWithValue("@Destinazione", s.Destinazione);
                    cmd.Parameters.AddWithValue("@Indirizzo", s.indirizzo);
                    cmd.Parameters.AddWithValue("@Nome", s.Nome);
                    cmd.Parameters.AddWithValue("@Costo", s.Costo);


                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        ViewBag.MessaggioConferma = "Salvataggio effettuato";
                    }
                    else
                    {
                        ViewBag.MessaggioConferma = "Nessuna modifica effettuata";
                    }

                }
                catch (Exception ex)
                {
                    Response.Write(ex.ToString());
                }
                finally { sqlConnection.Close(); }


            }
            return RedirectToAction("Index", "Admin");
        }

        //Codice per eliminare la spedizione
        public ActionResult Elimina(int id)
        {

            Spedizione spedizione = new Spedizione();
            foreach (Spedizione s in Spedizione.SpedizioneList)
            {
                if (s.id == id)
                {
                    spedizione = s; break;
                }
            }
            return View(spedizione);
        }

        [HttpPost]
        public ActionResult Elimina(Spedizione s)
        {

            string conn = ConfigurationManager.ConnectionStrings["GestionaleDB"].ConnectionString;
            SqlConnection sqlConnection = new SqlConnection(conn);
            try
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM SPEDIZIONI WHERE IdSpedizione = @id", sqlConnection);
                cmd.Parameters.AddWithValue("@id", s.id);

                int rowsAffected = cmd.ExecuteNonQuery();


                cmd = new SqlCommand("DELETE FROM STATO WHERE IdSpedizione = @id", sqlConnection);

                cmd.Parameters.AddWithValue("@id", s.id);

                rowsAffected += cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    ViewBag.MessaggioConferma = "Salvataggio effettuato";
                }
                else
                {
                    ViewBag.MessaggioConferma = "Nessuna modifica effettuata";
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
            return RedirectToAction("Index");
        }


        public ActionResult NewCliente()
        {
            return View();

        }

        [HttpPost]
        public ActionResult NewCliente(Cliente cliente)
        {

            if (ModelState.IsValid)
            {
                string conn = ConfigurationManager.ConnectionStrings["GestionaleDB"].ConnectionString;
                SqlConnection sqlConnection = new SqlConnection(conn);

                try
                {
                    sqlConnection.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO ANAGRAFE (Nome, [CF O P.IVA]) VALUES (@Nome, @CFpIVA )", sqlConnection);

                    cmd.Parameters.AddWithValue("@Nome", cliente.Nome);
                    cmd.Parameters.AddWithValue("@CFpIVA", cliente.CFoPIVA);


                    cmd.ExecuteNonQuery();



                }
                catch (Exception ex)
                {
                    Response.Write(ex.ToString());
                }
                finally { sqlConnection.Close(); }
            }

            return RedirectToAction("Index", "Admin");
        }


        //Codice per gestire lo stato delle spedizioni
        public ActionResult Stato(int idSpedizione)
        {
            return View();
        }

        [HttpPost]

        public ActionResult Stato(int idSpedizione, Stato st)
        {

            string conn = ConfigurationManager.ConnectionStrings["GestionaleDB"].ConnectionString;
            SqlConnection sqlConnection = new SqlConnection(conn);

            try
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO STATO (DataConsegna, StatoSpedizione, DataAggiornamento, InConsegna, IdSpedizione) VALUES (@DataConsegna, @StatoSpedizione, @DataAggiornamento, @InConsegna, @IdSpedizione)", sqlConnection);

                cmd.Parameters.AddWithValue("@DataConsegna", st.DataConsegna);
                cmd.Parameters.AddWithValue("@StatoSpedizione", st.StatoSpedizione);
                cmd.Parameters.AddWithValue("@DataAggiornamento", st.DataAggiornamento);
                cmd.Parameters.AddWithValue("@InConsegna", st.InConsegna);
                cmd.Parameters.AddWithValue("@IdSpedizione", idSpedizione);



                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    ViewBag.MessaggioConferma = "Salvataggio effettuato";
                }
                else
                {
                    ViewBag.MessaggioConferma = "Nessuna modifica effettuata";
                }

            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
            finally { sqlConnection.Close(); }

            return RedirectToAction("Index", "Admin");


        }


        //Codice per creare una nuova spedizione    
        public ActionResult Nuova()
        {
            ViewBag.Clienti = new List<Cliente>();

            string conn = ConfigurationManager.ConnectionStrings["GestionaleDB"].ConnectionString;
            SqlConnection sqlConnection = new SqlConnection(conn);
            try
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM ANAGRAFE", sqlConnection);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    ViewBag.Clienti.Add(new Cliente()
                    {
                        Value = Convert.ToInt32(reader["IdCliente"].ToString()),
                        Text = reader["Nome"].ToString(),
                    });

            }
            catch { }
            finally { sqlConnection.Close(); }

            return View();
        }

        [HttpPost]
        public ActionResult Nuova(Spedizione sp)
        {
            ViewBag.Clienti = new List<Cliente>();
            string conn = ConfigurationManager.ConnectionStrings["GestionaleDB"].ConnectionString;
            SqlConnection sqlConnection = new SqlConnection(conn);

            try
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO SPEDIZIONI (DataSpedizione, Peso, Destinazione, Indirizzo, Nome, Costo, IdCliente) VALUES (@DataSpedizione, @Peso, @Destinazione, @Indirizzo, @Nome, @Costo, @IdCliente)", sqlConnection);

                cmd.Parameters.AddWithValue("@DataSpedizione", sp.DataSpedizione);
                cmd.Parameters.AddWithValue("@Peso", sp.Peso);
                cmd.Parameters.AddWithValue("@Destinazione", sp.Destinazione);
                cmd.Parameters.AddWithValue("@indirizzo", sp.indirizzo);
                cmd.Parameters.AddWithValue("@Nome", sp.Nome);
                cmd.Parameters.AddWithValue("@Costo", sp.Costo);
                cmd.Parameters.AddWithValue("@IdCliente", sp.IdCliente);

                cmd.ExecuteNonQuery();




                cmd = new SqlCommand("SELECT * FROM ANAGRAFE", sqlConnection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                    ViewBag.Clienti.Add(new Cliente()
                    {
                        Value = Convert.ToInt32(reader["IdCliente"].ToString()),
                        Text = reader["Nome"].ToString(),
                    });





            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
            finally { sqlConnection.Close(); }

            return RedirectToAction("Index", "Admin");
        }


    
       



        //I json Results sono nel controller Corrirere

        //View per la chiamata ajax per le spedizioni in consegna oggi
        public ActionResult InConsegnaOggi()
        {
            return View();
        }

        //View per la chiamata ajax per le consegne in attesa
        public ActionResult ConsegneInAttesa()
        {
            return View();
        }

        //View per la chiamata ajax che raggruppa le consegne per città
        public ActionResult Citta()
        {
            return View();
        }








    }

}





