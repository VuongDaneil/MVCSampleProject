using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MVCSampleProject.Identity
{
    public class AppUser : IdentityUser
    {
        public string Role { get; set; }
        public int isBlook { get; set; }
    }
}