using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcBlog2.Models
{
    public class mComentario
    {
        //----------- Atributos publicos de la clase ---------------
        public int Id { get; set; }
        public int IdPost { get; set; }
        public DateTime Fecha { get; set; }
        public string NombreAutor { get; set; }
        public string Detalle { get; set; }

        //----------- Constructores de la clase ---------------
        public mComentario() { }
    }
}