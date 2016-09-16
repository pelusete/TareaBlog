using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcBlog2.Models
{
    public class mPost
    {
        //----------- Atributos Privados de la clase ---------------
        private string p_Imagen = "";

        //----------- Atributos publicos de la clase ---------------
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Imagen
        {
            get
            {
                if (this.p_Imagen == "") p_Imagen = "~/Content/img/sinFoto.jpg";

                return this.p_Imagen;
            }
            set
            {
                this.p_Imagen = value;
            }
        }
        public string[] Tags { get; set; }
        public string NombreAutor { get; set; }

        //----------- Constructores del Comentario -----------------
        public mPost() { }
    }
}