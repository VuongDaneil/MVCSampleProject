using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCSampleProject.common
{
    [Serializable]
    public class UserLogin
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public int? isBlock { get; set; }
    }
}