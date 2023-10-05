using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Security;

namespace Gestionale.Models
{
    public class SiteRole : RoleProvider
    {
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string Username)
        {
            string conn = ConfigurationManager.ConnectionStrings["GestionaleDB"].ConnectionString;
            SqlConnection sqlConnection = new SqlConnection(conn);

            sqlConnection.Open();

            SqlCommand sqlCommand = new SqlCommand("Select Role FROM [User] WHERE Username=@Username", sqlConnection);
            sqlCommand.Parameters.AddWithValue("Username", Username);

            SqlDataReader reader = sqlCommand.ExecuteReader();

            List<string> roles = new List<string>();

            while (reader.Read())
            {
                string role = reader["Role"].ToString();
                roles.Add(role);
            }

            return roles.ToArray();
        }


        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}