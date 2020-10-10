using InventoryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;

namespace InventoryManagement.Controllers
{
    public class UserController : Controller
    {
        private readonly InventoryContext _context;

        public UserController(InventoryContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult AddUser([FromBody] User user)
        {
            string cityName = "";
            if (user.CityNames != null)
            {
                if (user.CityNames.Length > 0)
                {
                    cityName = string.Join(",", user.CityNames);
                }
            }

            var userObj = new User
            {
                UserName = user.UserName,
                Password = user.Password,
                CityName = cityName
            };

            if (!string.IsNullOrEmpty(userObj.UserName))
            {
                userObj.UserName = userObj.UserName.Trim();
            }

            if (!string.IsNullOrEmpty(userObj.Password))
            {
                userObj.Password = userObj.Password.Trim();
            }

            bool userExisted = UserExisted(user);

            if (userExisted)
            {
                return Conflict(new { msg = "Account existed!" });
            }

            try
            {
                _context.Add(userObj);
                _context.SaveChanges();
                return Created("", new { msg = "Account created!" });
            }
            catch(Exception e)
            {
                return Conflict(new { msg = e.Message });
            }
        }

        private bool UserExisted(User user)
        {
            var _user = _context.User.Any(u =>
     u.UserName == user.UserName && u.Password == u.Password);
            return _user;
        }

    }
}
