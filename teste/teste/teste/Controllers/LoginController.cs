using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Security.Claims;

namespace teste.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Json(new { Msg = "Usuário já logado!" });
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logar(string username, string senha)
        {
            MySqlConnection mySqlConnection = new MySqlConnection("server=localhost;database=usuario;uid=root;password=67578900");
            await mySqlConnection.OpenAsync();
            

            MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
            mySqlCommand.CommandText = $"select * from usuarios where username = '{username}' and senha = '{senha}'";
            
            MySqlDataReader reader = mySqlCommand.ExecuteReader();

           
           if(await reader.ReadAsync())
            {
                int usuarioid = reader.GetInt32(0);
                string nome = reader.GetString(1);

                List<Claim> direitosAcesso = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, usuarioid.ToString()),
                    new Claim(ClaimTypes.Name, nome),
                };

                var identity = new ClaimsIdentity(direitosAcesso, "Identity.Login");
                var userPrincipal = new ClaimsPrincipal(new[] { identity});

                await HttpContext.SignInAsync(userPrincipal,
                    new AuthenticationProperties
                {
                    IsPersistent = false,
                    ExpiresUtc = DateTime.Now.AddHours(1)
                });
                    
                    return Json(new { Msg = "Usuário Logado com sucesso!"});
            }
            
            return Json(new { Msg = "Usuário não encontrado!" });



        }
        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync();
            }
            return RedirectToAction("Index", "Login");
        }
    }

}
