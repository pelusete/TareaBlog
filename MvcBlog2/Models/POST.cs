//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MvcBlog2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class POST
    {
        public POST()
        {
            this.COMENTARIOs = new HashSet<COMENTARIO>();
        }
    
        public int POST_ID { get; set; }
        public int BLOG_ID { get; set; }
        public Nullable<System.DateTime> POST_FECHA { get; set; }
        public string POST_TITULO { get; set; }
        public string POST_DESCRIPCION { get; set; }
        public string POST_IMAGEN { get; set; }
        public string POST_TAGS { get; set; }
        public string POST_NOMBREAUTOR { get; set; }
    
        public virtual BLOG BLOG { get; set; }
        public virtual ICollection<COMENTARIO> COMENTARIOs { get; set; }
    }
}
