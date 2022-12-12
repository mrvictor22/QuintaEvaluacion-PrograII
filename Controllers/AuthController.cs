using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuintaEvaluacion.Models;

namespace QuintaEvaluacion.Controllers
{
    public class AuthController : Controller
    {
        private adventure_works_db_Entities db = new adventure_works_db_Entities();

        // GET: Auth/SignIn
        public ActionResult SignIn()
        {
            return View();
        }

        // GET: Auth/SignUp
        public ActionResult SignUp()
        {
            return View();
        }

        // GET: Auth/SignOut
        public ActionResult SignOut()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // GET: Auth/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Auth/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "FirstName,LastName,UserName,Email,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                string encriptedPassword = EncodePasswordToBase64(user.Password);
                user.Password = encriptedPassword;
                user.CreatedAt = DateTime.Now;
                db.Users.Add(user);
                await db.SaveChangesAsync();
                Session["isAuth"] = true;
                Session["user_id"] = user.Id;
                Session["username"] = user.UserName;
                Session["first_name"] = user.FirstName;
                Session["last_name"] = user.LastName;

                return RedirectToAction("Index", "Home");
            }

            return View(user);
        }

        // POST: Auth/LogIn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LogIn(string username, string password)
        {
            var decodedPassword = DecodeFrom64(password);
            if (password == null || username == null)
            {
                return View("~/Views/Auth/SignIn.cshtml");
            }
            var user = await db.Users.FirstAsync(u => u.Email == username || u.UserName == username && u.Password == decodedPassword);
            if(user.Id <= 0)
            {
                return View("~/Views/Auth/SignIn.cshtml");
            }


            if(password != decodedPassword)
            {
                return View("~/Views/Auth/SignIn.cshtml");
            }

            Session["isAuth"] = true;
            Session["user_id"] = user.Id;
            Session["username"] = user.UserName;
            Session["first_name"] = user.FirstName;
            Session["last_name"] = user.LastName;

            return RedirectToAction("Index", "Home");
        }

        //this function Convert to Encord your Password
        private string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }
        //this function Convert to Decord your Password
        private string DecodeFrom64(string encodedData)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encodedData);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
