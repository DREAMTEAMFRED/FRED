using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FRED.Utility
{
    public class Controller
    {
        public int UserID { get; set; }
        public bool IsVerified { get; set; }

        public void CheckID(string username, string password)
        {
            UserID = 0;
            using (SqlConnection myConn = new SqlConnection(Program.cs))
            {
                SqlCommand login = new SqlCommand();
                login.Connection = myConn;
                myConn.Open();
                login.Parameters.AddWithValue("@username", username);
                login.CommandText = ("[spLogin]");
                login.CommandType = System.Data.CommandType.StoredProcedure;
                var result = login.ExecuteScalar();

                if (result != null)
                {
                    // correct username check password
                    UserID = Convert.ToInt32(result);
                    SqlCommand verPass = new SqlCommand();
                    verPass.Connection = myConn;
                    verPass.Parameters.AddWithValue("@userID", UserID);
                    verPass.CommandText = ("[spVerPass]");
                    verPass.CommandType = System.Data.CommandType.StoredProcedure;
                    var pass = verPass.ExecuteScalar();
                    string hashPassword = Convert.ToString(pass);

                    if (Program.Password.Verify(password, hashPassword))
                    {
                        IsVerified = true;
                    }
                    else
                    {
                        UserID = 0;
                        IsVerified = false; // wrong password
                    }
                }
                else
                {
                    IsVerified = false;  // wrong username
                }

                myConn.Close();
            }
        }//checkID()


    }
}
