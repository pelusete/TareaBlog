using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcBlog2.Models
{
    public class mBlog
    {
        BloggerBDEntidades db = new BloggerBDEntidades();

        public int blogId { get; set; }
        public string blogNombre { get; set; }
        public List<mPost> blogListaPost { get; set; }

        public mBlog()
        {
            blogListaPost = new List<mPost>();
        }

    }
}