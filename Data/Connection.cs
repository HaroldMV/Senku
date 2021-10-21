using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Senku.Data
{
    public class Connection
    {
        protected SqlConnection connection;

        protected void Connect()
        {
            try
            {
                connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connection"].ConnectionString);
                connection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        protected void Disconnect()
        {
            try
            {
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}