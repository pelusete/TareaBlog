using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Newtonsoft.Json;      //------- Tratamiento de JSON

using MvcBlog2.Models;      //-------- Referencia a los Modelos

namespace MvcBlog2.Controllers
{
    public class AjaxController : Controller
    {
        BloggerBDEntidades db = new BloggerBDEntidades();   //-----Mapeado de las tablas de la BD en el objeto

        //
        // GET: /Ajax/

        /*
         Función inicial del controlador que devuelve una sentencia JSON en base a las acciones que vaya recibiendo el controlador
         */
        public JsonResult Index(FormCollection collection)
        {
            string acc = collection["acc"]; //---- Acción a ejecutar por el controlador

            if (acc == "POST_CARGAR_COMENTARIOS")
            {
                int idPost = Convert.ToInt32(collection["idPost"]);

                var comentariosPost = from c in db.COMENTARIOs where c.POST_ID == idPost select c;

                //-----Se genera una lista de objetos Comentario del post
                List<mComentario> lista_pst = new List<mComentario>();
                foreach (var row in comentariosPost)
                {
                    mComentario com_ = new mComentario();
                    com_.Id = row.COM_ID;
                    com_.IdPost = row.POST_ID;
                    com_.Fecha = Convert.ToDateTime(row.COM_FECHA);
                    com_.NombreAutor = row.COM_NOMBREAUTOR;
                    com_.Detalle = row.COM_DETALLE;

                    lista_pst.Add(com_);
                    com_ = null;
                }

                //Se convierte la lista generada a JSON para su tratamiento en la vista.
                var logJson = JsonConvert.SerializeObject(lista_pst);

                return Json(logJson);
            }
            else if (acc == "POST_COMENTAR")
            {
                //-------- Se reciben los parametros del comentario a través de la colección
                COMENTARIO comBD = new COMENTARIO();
                comBD.COM_ID = 1;  //--- Se asigna un valor para su registro (es incremental en la BD)
                comBD.COM_FECHA = DateTime.Now;
                comBD.COM_NOMBREAUTOR = collection["nombreAutor"];
                comBD.COM_DETALLE = collection["comentarioUsuario"];
                comBD.POST_ID = Convert.ToInt32(collection["idPost"]);

                db.COMENTARIOs.Add(comBD);  //---Se agrega el comentario nuevo a la BD
                db.SaveChanges();           //---Se guardan los cambios

                var comentariosPost = from c in db.COMENTARIOs where c.POST_ID == comBD.POST_ID select c;

                //-----Se genera una lista de objetos Comentario del post
                List<mComentario> lista_pst = new List<mComentario>();
                foreach (var row in comentariosPost)
                {
                    mComentario com_ = new mComentario();
                    com_.Id = row.COM_ID;
                    com_.IdPost = row.POST_ID;
                    com_.Fecha = Convert.ToDateTime(row.COM_FECHA);
                    com_.NombreAutor = row.COM_NOMBREAUTOR;
                    com_.Detalle = row.COM_DETALLE;

                    lista_pst.Add(com_);
                    com_ = null;
                }

                //Se convierte la lista generada a JSON para su tratamiento en la vista.
                var logJson = JsonConvert.SerializeObject(lista_pst);

                return Json(logJson);       //----Se retorna la lista json serializada a la vista
            }
            else
            {
                return Json("");    //---- No se retorna nada si hay error
            }
        }

    }
}
