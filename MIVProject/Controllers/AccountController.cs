using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;

namespace MIVProject.Controllers
{
    public class AccountController : Controller
    {
        mivEntities mivEntities = new mivEntities();
       
        private string GetMD5HashData(string data)
        {
            //create new instance of md5
            MD5 md5 = MD5.Create();

            //convert the input text to array of bytes
            byte[] hashData = md5.ComputeHash(Encoding.Default.GetBytes(data));

            //create new instance of StringBuilder to save hashed data
            StringBuilder returnValue = new StringBuilder();

            //loop for each byte and add it to StringBuilder
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }

            // return hexadecimal string
            return returnValue.ToString();

        }

       
        // GET: Accounts
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(mivUser mivUser)
        {
            
            mivUser.password = GetMD5HashData(mivUser.password.ToString());
            int count = mivEntities.mivUser.Where(x => x.username == mivUser.username && x.password == mivUser.password).Count();
            if (count == 0) {
                ViewBag.Msg = "Invalid username or password";
                return View();
            }
            else {
                //FormsAuthentication.SetAuthCookie(mivUser.username, false);
                var u = mivEntities.Database.SqlQuery<mivUser>("Select * from mivUser where username = @p0", mivUser.username);                         
                Session["username"] = mivUser.username;
                userType userType = mivEntities.userType.Find(u.First().type);
                if (userType != null)
                {
                    Session["type"] = userType.type;
                }
                else
                {
                    Session["type"] = mivUser.type;
                }
                return RedirectToAction("Index", "Home");
            }
            
        }

        public ActionResult Logout()
        {
            //FormsAuthentication.SignOut();
            Session.RemoveAll();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(int a = 0)
        {
            
            mivUser mivUser = new mivUser();
            supplier supplier = new supplier();
            mivUser.username = Request.Form["username"];
            mivUser.password = GetMD5HashData(Request.Form["password"].ToString());
            mivUser.email = Request.Form["email"];
            if (mivUser.username.CompareTo("") != 0 && mivUser.password.CompareTo("") != 0 && mivUser.email.CompareTo("") != 0) 
            { 
                mivEntities.mivUser.Add(mivUser);         
                try{
                    mivEntities.SaveChanges();
                }
                catch
                {
                     ViewBag.Msg = "Neispravni podaci";
                     return View();
                }

                int user = mivUser.userID;

                    supplier.mivUser = user;
                    supplier.name = Request.Form["name"];                   
                    supplier.phone = Request.Form["phone"];
                    supplier.OIB = Request.Form["oib"];

                    if (supplier.name.CompareTo("") != 0 && supplier.phone.CompareTo("") != 0 && supplier.OIB.CompareTo("") != 0)
                    {
                        mivEntities.supplier.Add(supplier);
                        try
                        {
                            mivEntities.SaveChanges();
                        }
                        catch
                        {
                            mivEntities.mivUser.Remove(mivUser);
                            mivEntities.SaveChanges();
                            ViewBag.Msg = "Neispravni podaci";
                            return View();
                        }
                    }
                    else
                    {
                        mivEntities.mivUser.Remove(mivUser);
                        mivEntities.SaveChanges();
                        ViewBag.Msg = "Neispravni podaci";
                        return RedirectToAction("Register");
                    }
                    return RedirectToAction("Index", "Home");
                }

            else
            {
                ViewBag.Msg = "Neispravni podaci";
                return View();
            }
            
            

        }

        public static string CreateRandomPassword(int PasswordLength)
        {

            string _allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789-";
            Random randNum = new Random();
            char[] chars = new char[PasswordLength];
            int allowedCharCount = _allowedChars.Length;

            for (int i = 0; i < PasswordLength; i++)
            {
                chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
            }

            return new string(chars);
        }


        public int SendEmail(string email, string password)
        {
            try
            {
                var fromEmailAddress = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();
                var fromEmailDisplayName = ConfigurationManager.AppSettings["FromEmailDisplayName"].ToString();
                var fromEmailPassword = ConfigurationManager.AppSettings["FromEmailPassword"].ToString();
                var smtpHost = ConfigurationManager.AppSettings["SMTPHost"].ToString();
                var smtpPort = ConfigurationManager.AppSettings["SMTPPort"].ToString();

                string body = "Vaša nova lozinka: / Your new password: "+ password;
                MailMessage message = new MailMessage(new MailAddress(fromEmailAddress, fromEmailDisplayName), new MailAddress(email, ""));
                message.Subject = "Vaša nova lozinka / Your new password";
                message.IsBodyHtml = true;
                message.Body = body;

                var client = new SmtpClient();
                client.Credentials = new NetworkCredential(fromEmailAddress, fromEmailPassword);
                client.Host = smtpHost;
                client.EnableSsl = true;
                client.Port = !string.IsNullOrEmpty(smtpPort) ? Convert.ToInt32(smtpPort) : 0;
                client.Send(message);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        [HttpGet]
        public ActionResult GeneratePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GeneratePassword(mivUser mivUser)
        {
            string newPassword = GetMD5HashData(CreateRandomPassword(7));

            mivEntities.Database.ExecuteSqlCommand("Update mivUser set password = @p0 where username = @p1", newPassword, Request.Form["username"]);
            string email = mivEntities.Database.ExecuteSqlCommand("Select email from mivUser where username = @p0", Request.Form["username"]).ToString();
        
            if (email.CompareTo("")!=0)
                {
                    
                            if (SendEmail(mivUser.email, newPassword) != 0)
                            {
                                ViewBag.Msg = "Nova lozinka poslana je na vašu e-mail adresu";

                            }
                            else
                            {
                                ViewBag.Msg = "Problem sa slanjem emaila";
                                return View();
                            }
                                         
                    
                }

            else
            {
                ViewBag.Msg = "Ovo korisničko ime ne postoji.";
               
            }

            return View();
        }
    }
}