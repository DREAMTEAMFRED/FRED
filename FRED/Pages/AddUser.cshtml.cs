using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FRED.Pages
{
    public class AddUserModel : PageModel
    {
        public void OnGet()
        {

        }

        public void OnPostNewUser(string username, string password)
        {
            string hashPassword = Program.Password.Hash(password);

            using (SqlConnection myConn = new SqlConnection(Program.cs))
            {
                SqlCommand UserExists = new SqlCommand();
                UserExists.Connection = myConn;
                myConn.Open();
                UserExists.Parameters.AddWithValue("@username", username);
                UserExists.CommandText = ("[spUserExists]");
                UserExists.CommandType = System.Data.CommandType.StoredProcedure;
                var exists = UserExists.ExecuteScalar();

                if(username == Convert.ToString(exists))
                {
                    // username exists
                    Program.Controller.UserID = 0;
                    Response.Redirect("./Error");
                }
                else
                {
                    // username does not exists
                    int userID = 0;
                    SqlCommand addUser = new SqlCommand();
                    addUser.Connection = myConn;

                    addUser.Parameters.AddWithValue("@username", username);
                    addUser.Parameters.AddWithValue("@password", hashPassword);

                    addUser.CommandText = ("[spAddUser]");
                    addUser.CommandType = System.Data.CommandType.StoredProcedure;

                    var result = addUser.ExecuteScalar();
                    if (result != null)
                    {
                        userID = Convert.ToInt16(result);
                        Program.Controller.UserID = userID;
                    }

                    // add Log table
                    SqlCommand addLog = new SqlCommand();
                    addLog.Connection = myConn;

                    addLog.Parameters.AddWithValue("@UserID", userID);

                    addLog.CommandText = ("[spAddLog]");
                    addLog.CommandType = System.Data.CommandType.StoredProcedure;

                    addLog.ExecuteNonQuery();

                    myConn.Close();
                    Response.Redirect("./UserControls");
                }
                
            }
            //OnPostLogin(username, password);
            
        }

        public void OnPostLogin(string username, string password)
        {
            Program.Controller.CheckID(username, password);
            if (Program.Controller.IsVerified)
            {
                Response.Redirect("./UserControls");
            }
            else
            {
                Response.Redirect("./Error");
            }

        }
    }
}