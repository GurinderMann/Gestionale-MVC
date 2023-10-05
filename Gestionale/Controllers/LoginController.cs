using Gestionale.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.Security;

namespace Gestionale.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(User u)
        {
            string conn = ConfigurationManager.ConnectionStrings["GestionaleDB"].ConnectionString;
            SqlConnection sqlConnection = new SqlConnection(conn);
            try
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("Select * FROM [User] WHERE Username=@Username And Password=@Password", sqlConnection);
                sqlCommand.Parameters.AddWithValue("Username", u.Username);
                sqlCommand.Parameters.AddWithValue("Password", u.Password);

                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    FormsAuthentication.SetAuthCookie(u.Username, false);
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ViewBag.AuthError = "Autenticazione non riuscita";
                    return View();
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

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User u) 
        {
            string role = "User";
            string conn = ConfigurationManager.ConnectionStrings["GestionaleDB"].ConnectionString;
            SqlConnection sqlConnection = new SqlConnection(conn);
            try
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("INSERT INTO [User] (Username, Password, Role) VALUES(@Username, @Password, @Role)", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Username", u.Username);
                sqlCommand.Parameters.AddWithValue("@Password", u.Password);
                sqlCommand.Parameters.AddWithValue("@Role", role);

                sqlCommand.ExecuteReader();
                
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());

            }
            finally
            {
                sqlConnection.Close();
            }

            return RedirectToAction("Index", "Home");
        }
    }
}