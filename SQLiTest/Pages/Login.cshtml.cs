using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Data;

namespace SQLiTest.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public required string Login { get; set; }

        [BindProperty]
        public required string Haslo { get; set; }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrWhiteSpace(Login))
            {
                ModelState.AddModelError("Login", "Podaj login !");
                return Page();
            }


            if (string.IsNullOrWhiteSpace(Haslo))
            {
                ModelState.AddModelError("Haslo", "Podaj hasło !");
                return Page();
            }

            if (CzyDobryLoginHaslo(Login, Haslo))
            {
                return RedirectToPage("./Welcome", new { username = Login });
            }
            else
            {
                ModelState.AddModelError("Haslo", "Zły login lub hasło !");
                return Page();
            }
        }

        private bool CzyDobryLoginHaslo(string login, string haslo)
        {
            try
            {
                SqlConnection cnUsers = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Users;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

                string sSQL = "SELECT * FROM Uzytkownicy WHERE Login='" + login + "' AND Haslo='" + haslo + "'";
                SqlDataAdapter daUsers = new SqlDataAdapter(sSQL, cnUsers);
                DataSet dsUsers = new DataSet();
                daUsers.Fill(dsUsers);

                return (dsUsers.Tables[0].Rows.Count > 0);
            }
            catch
            {
                return false;
            }
        }

        protected bool CzyDobryLoginHasloParametr(string login, string haslo)
        {
            try
            {
                SqlConnection cnUsers = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Users;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

                SqlParameter parLogin = new SqlParameter("Login", login);
                SqlParameter parHaslo = new SqlParameter("Haslo", haslo);

                SqlCommand sSelect = new SqlCommand("SELECT * FROM Uzytkownicy WHERE Login=@Login AND Haslo=@Haslo", cnUsers);

                sSelect.Parameters.Add(parLogin);
                sSelect.Parameters.Add(parHaslo);

                SqlDataAdapter daUsers = new SqlDataAdapter(sSelect);
                DataSet dsUsers = new DataSet();
                daUsers.Fill(dsUsers);

                return (dsUsers.Tables[0].Rows.Count > 0);
            }
            catch
            {
                return false;
            }

        }

        bool CzyDobryLoginHasloLinq(string sUser, string sHaslo)
        {
            try
            {
                UsersDbContext dbContext = new UsersDbContext();


                var wynik = dbContext.Uzytkownicy.Where(u => u.Login == sUser && u.Haslo == sHaslo);

                //var wynik = from l in dbContext.Uzytkownicy
                //            where (l.Login == sUser) && (l.Haslo == sHaslo)
                //            select l.Login;

                return (wynik.Count() > 0);

            }
            catch
            {
                return false;                       
            }
        }

        bool CzyDobryLoginHasloStored(string sUser, string sHaslo)
        {
            try
            {
                SqlConnection cnUsers = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Users;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
                cnUsers.Open();

                using (SqlCommand cmd = new SqlCommand("SprawdzHaslo"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@log", sUser));
                    cmd.Parameters.Add(new SqlParameter("@pass", sHaslo));

                    SqlParameter ileParameter = new SqlParameter("@ile", 0);
                    ileParameter.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(ileParameter);

                    cmd.Connection = cnUsers;

                    cmd.ExecuteNonQuery();
                    int ile = Int32.Parse(cmd.Parameters["@ile"].Value.ToString());

                    cnUsers.Close();

                    return ile > 0;
                }
            }
            catch
            {
                return false;
            }
        }


    }

}
