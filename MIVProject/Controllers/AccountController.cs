using Resources;
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


        public int SendEmail(string email, string body, string subject)
        {
            try
            {
                var fromEmailAddress = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();
                var fromEmailDisplayName = ConfigurationManager.AppSettings["FromEmailDisplayName"].ToString();
                var fromEmailPassword = ConfigurationManager.AppSettings["FromEmailPassword"].ToString();
                var smtpHost = ConfigurationManager.AppSettings["SMTPHost"].ToString();
                var smtpPort = ConfigurationManager.AppSettings["SMTPPort"].ToString();


                MailMessage message = new MailMessage(new MailAddress(fromEmailAddress, fromEmailDisplayName), new MailAddress(email, ""));
                message.Subject = subject;
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
            catch
            {
                return 0;
            }
        }

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
        public ActionResult Unauthorized()
        {
            return View();
        }

        // GET: Accounts
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(mivUser mivUser)
        {
            string username = mivUser.username;
            DateTime date = DateTime.Now;

            mivUser.password = GetMD5HashData(mivUser.password.ToString());
            int count = mivEntities.mivUser.Where(x => x.username == mivUser.username && x.password == mivUser.password).Count();
            if (count == 0)
            {
                string msg = "Login attempt failed";
                mivEntities.Database.ExecuteSqlCommand("Insert into logs values(0, @p0, @p1, @p2 )", username, msg, date);
                ViewBag.Msg = Local.InvalidData;
                return View();
            }
            else
            {
                //FormsAuthentication.SetAuthCookie(mivUser.username, false);
                var u = mivEntities.Database.SqlQuery<mivUser>("Select * from mivUser where username = @p0", mivUser.username);
                Session["username"] = mivUser.username;
                Session["userID"] = u.First().userID;
                userType userType = mivEntities.userType.Find(u.First().type);
                if (userType != null)
                {
                    if (userType.type == "administrator" || userType.type == "admin") Session["type"] = "administrator";
                    else if (userType.type == "dobavljač" || userType.type == "dobavljac" || userType.type == "supplier") Session["type"] = "dobavljac";
                    else Session["type"] = "referent";

                }
                else
                {
                    Session["type"] = null;
                }

                string msg = "User loged in";
                int userID = (int)Session["userID"];
                mivEntities.Database.ExecuteSqlCommand("Insert into logs values(@p0, @p1, @p2, @p3 )", userID, username, msg, date);
                return Redirect(FormsAuthentication.GetRedirectUrl(Session["username"].ToString(), false));
                //return RedirectToAction("Index", "Home");
            }

        }

        public ActionResult Logout()
        {
            //FormsAuthentication.SignOut();
            string username = Session["username"].ToString();
            DateTime date = DateTime.Now;
            string msg = "User loged out";
            int userID = (int)Session["userID"];
            mivEntities.Database.ExecuteSqlCommand("Insert into logs values(@p0, @p1, @p2, @p3 )", userID, username, msg, date);
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
                try
                {
                    mivEntities.SaveChanges();
                }
                catch
                {
                    ViewBag.Msg = Local.InvalidData;
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
                        ViewBag.Msg = Local.InvalidData;
                        return View();
                    }
                }
                else
                {
                    mivEntities.mivUser.Remove(mivUser);
                    mivEntities.SaveChanges();
                    ViewBag.Msg = Local.InvalidData;
                    return View();
                }


                string body = Local.NewSupplierRegistered + ": " + supplier.name;
                string subject = Local.Registration + " " + supplier.name;

                SendEmail("ljdetic@gmail.com", body, subject);
                return RedirectToAction("Registered");


            }

            else
            {
                ViewBag.Msg = Local.InvalidData;
                return View();
            }



        }

        public ActionResult Registered()
        {
            return View();
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



        [HttpGet]
        public ActionResult GeneratePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GeneratePassword(mivUser mivUser)
        {
            string plainPassword = CreateRandomPassword(7);
            string newPassword = GetMD5HashData(plainPassword);

            mivEntities.Database.ExecuteSqlCommand("Update mivUser set password = @p0 where username = @p1", newPassword, Request.Form["username"]);
            string username = Request.Form["username"].ToString();
            int userID = (int)Session["userID"];
            var user = mivEntities.mivUser.Where(x => x.username == username);

            string body = Local.PasswordIsChanged + ". " + Local.PasswordGenerated + ": " + plainPassword;
            string subject = Local.PasswordIsChanged;

            DateTime date = DateTime.Now;

            if (user != null)
            {

                if (SendEmail(user.First().email.ToString(), body, subject) != 0)
                {


                    string msg = "New password generated, and email sent";
                    mivEntities.Database.ExecuteSqlCommand("Insert into logs values(@p0, @p1, @p2, @p3 )", userID, username, msg, date);
                    return RedirectToAction("PasswordGenerated");

                }
                else
                {
                    string msg = "An error occured when sending new password mail";
                    mivEntities.Database.ExecuteSqlCommand("Insert into logs values(@p0, @p1, @p2, @p3 )", userID, username, msg, date);

                    ViewBag.Msg = Local.SendingMailProblem;
                    return View();
                }


            }

            else
            {
                ViewBag.Msg = Local.InvalidData;
                return View();
            }


        }


        [HttpGet]
        public ActionResult PasswordGenerated()
        {
            return View();
        }


        [CustomAuthorize(Roles = "administrator,referent,dobavljac")]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(int a = 1)
        {

            string pass = Request.Form["password"];
            string confPass = Request.Form["confirmedPassword"];

            if (pass == confPass)
            {
                string username = Session["username"].ToString();
                DateTime date = DateTime.Now;
                string msg = "Password changed";
                int userID = (int)Session["userID"];
                mivEntities.Database.ExecuteSqlCommand("Insert into logs values(@p0, @p1, @p2, @p3 )", userID, username, msg, date);
                string newPassword = GetMD5HashData(pass);

                mivEntities.Database.ExecuteSqlCommand("Update mivUser set password = @p0 where userID = @p1", newPassword, userID);
                return RedirectToAction("PasswordIsChanged");
            }
            else
            {
                ViewBag.Msg = Local.PassDoesntMatch;
                return View();
            }

        }
        public ActionResult PasswordIsChanged()
        {
            return View();
        }
    }
}