using Senku.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Senku.Data
{
    public class UsuarioData : Connection
    {
        public void AddUser(Usuario usuario)
        {
            Connect();

            try
            {
                SqlCommand command = new SqlCommand("insert_user", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@id", usuario.Id);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                Disconnect();
            }
        }
    }
}