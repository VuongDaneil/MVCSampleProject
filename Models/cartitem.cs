using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCSampleProject.Models
{
    [Serializable]
    public class cartitem
    {
        public Product Product { set; get; }
        public int Quantity { set; get; }
    }
}