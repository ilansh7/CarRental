using System.Linq;
using System.Web.Security;

#pragma warning disable 618

namespace SunshineRentals
{
    public class UsersLogic : BaseLogic
    {
        /// <summary>
        /// Register new user.
        /// </summary>
        /// <param name="username">The user's username.</param>
        /// <param name="password">The user's password.</param>
        public void Register(string id, string fname, string lname, string username, string password, string email)
        {
            // Need to add reference to: System.Web (Framework 4.0)
            string encryptPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "sha1");
            User user = new User { FirstName = fname, LastName = lname, IdNumber = id, UserName = username, Password = encryptPassword, eMail = email };

            DB.Roles.FirstOrDefault(r => r.RoleID == 3).Users.Add(user); // Set this user the "User" Role (RoleID = 3).
            DB.Users.Add(user);
            DB.SaveChanges();
        }

        /// <summary>
        /// Check if username already taken.
        /// </summary>
        /// <param name="username">The username to check.</param>
        /// <returns>Returns true if username already taken.</returns>
        public bool IsUsernameTaken(string username)
        {
            return true;
            //return DB.Users.Any(u => u.Username == username);
        }

        /// <summary>
        /// Check if user exists.
        /// </summary>
        /// <param name="username">The user's username.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>Returns true if user exist.</returns>
        public bool IsUserExist(string username, string password)
        {
            string encryptPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "sha1");
            return DB.Users.Any(u => u.UserName == username && u.Password == encryptPassword);
        }

        /// <summary>
        /// Get all users.
        /// </summary>
        /// <returns>Returns all usernames.</returns>
        public string[] GetAllUsers()
        {
            return DB.Users.Select(u => u.UserName).ToArray();
        }
        
        /// <summary>
        /// Get all users.
        /// </summary>
        /// <returns>Returns all usernames.</returns>
        public int GetUserId(string username)
        {
            if (string.IsNullOrEmpty(username))
                return 0;
            //var user = DB.Users.Where(u => u.UserName == username).Select(m => m.UserId).ToString();
            var user = DB.Users.Where(x => x.UserName == username).Select(x => x.UserId).FirstOrDefault().ToString();
            return int.Parse(user);
        }

        /// <summary>
        /// Get all users.
        /// </summary>
        /// <returns>Returns all usernames.</returns>
        public string GetUserFullName(int userid)
        {
            int _user = 0;
            if (!int.TryParse(userid.ToString(), out _user))
                return string.Empty;
            if (_user <= 0)
                return string.Empty;
            //var user = DB.Users.Where(u => u.UserName == username).Select(m => m.UserId).ToString();
            var userName = DB.Users.Where(x => x.UserId == _user).Select(x => new { Name = x.FirstName + " " + x.LastName }).FirstOrDefault().ToString();
            return userName;
        }

        public string IsUserOccupied(string userName)
        {
            using (RolesLogic logic = new RolesLogic())
            {
                var roles = logic.GetRolesForUser(userName);
                foreach (var roll in roles)
                {
                    if (roll == "Admin")
                        return "Admin";
                    if (roll == "Manager")
                        return "Manager";
                }
                int userId = GetUserId(userName);
                if (DB.Rentals.Any(r => r.UserId == userId))
                    return "User have orders.";
            }
            return string.Empty;
        }
    }
}
