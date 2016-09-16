using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MvcBlog2.Models;      //-------- Referencia a los Modelos
using PagedList;            //-------- Librería de paginación de ASP.NET

namespace MvcBlog2.Controllers
{
    public class BlogController : Controller
    {
        BloggerBDEntidades db = new BloggerBDEntidades();   //-----Mapeado de las tablas de la BD en el objeto
        
        //
        // GET: /Blog/
        /*
         Funcion principal del controlador que permite el filtrado y paginación de datos
         */
        public ViewResult Index(string txt_SearchTag, string currentFilter, int? page)
        {
            if (txt_SearchTag != null)
            {
                page = 1;
            }
            else
            {
                txt_SearchTag = currentFilter;
            }

            ViewBag.CurrentFilter = txt_SearchTag;

            //---------------Se consulta por todos los post
            var posteos = from c in db.POSTs where c.BLOG_ID == 1 select c;
            var count = posteos.Count();    //---Se cuentan los post

            //Si se ha buscado por algun tag, se modifica la consulta
            if (!string.IsNullOrEmpty(txt_SearchTag))
            {
                posteos = posteos.Where(m => m.POST_TAGS.Contains(txt_SearchTag));
            }

            posteos = posteos.OrderByDescending(x => x.POST_FECHA); //---La consulta se ordena de forma descendente

            //--------Se comprueba el tamaño de la paginación
            int pageSize = count;
            if (count > pageSize) { pageSize = 10; }

            int pageNumber = (page ?? 1);
            return View(posteos.ToPagedList(pageNumber, pageSize));//----- Se genera la vista de los post consultados mediante la paginación
        }

        /// <summary>
        /// Procedimiento para subir la fotografía del Post al servidor
        /// </summary>
        /// <param name="file">Archivo a subir</param>
        /// <returns>Retorna la ruta de la fotografía, si es vacío retorna</returns>
        public string SubirFotografia(HttpPostedFileBase file)
        {
            if (file != null && file.FileName != "")
            {
                string rutaImagenesPost = "~/Content/img/imagenesPost";     //-----Ruta fisica donde se almacenarán las imágenes de los post

                string pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath(rutaImagenesPost), pic);
                
                file.SaveAs(path);  //----Se guarda fisicamente la imagen

                return rutaImagenesPost + "/" + pic;    //----Retorna la ruta de la imagen
            }
            else return "";
        }

        // GET: /Blog/Details/5
        public ActionResult Details(int id = 0)
        {
            POST postDetalle = db.POSTs.Find(id);   //----De todos los post, Se encuentra un POST según su ID
            return View(postDetalle);
        }

        //
        // GET: /Blog/Create
        public ActionResult Create()
        {
            mPost pos = new mPost();

            //-----Pagina de creacion de un post para el blog
            return View(pos);
        }

        //
        // POST: /Blog/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)   //--------Postback que se activa unicamente al crear un nuevo post
        {
            mPost postCreate = new mPost();
            
            if (ModelState.IsValid)
            {
                //--------Se reciben todos los datos enviados
                POST postBD = new POST();
                postBD.BLOG_ID = 1;
                postBD.POST_TITULO = collection["Titulo"];
                postBD.POST_DESCRIPCION = collection["Descripcion"];
                postBD.POST_TAGS = collection["Tags"];
                postBD.POST_NOMBREAUTOR = collection["NombreAutor"];
                
                HttpPostedFileBase file = Request.Files["post_Imagen"];
                //------Si se envió una imagen, se procesa
                if (file.FileName != "")
                {
                    postBD.POST_IMAGEN = SubirFotografia(file);
                }
                postBD.POST_FECHA = DateTime.Now;    //---Fecha de creación

                db.POSTs.Add(postBD);       //----Se agrega el registro a la base de datos
                db.SaveChanges();           //----Se guardan los cambios

                return RedirectToAction("Index");   //---Se retorna a la pagina principal del blog
            }

            return View(postCreate);    //----Retorna una vista en la que se utilizará el objeto
        }

        //
        // GET: /Blog/Edit/5

        /*
         - Método que se incia al ingresar a la pagina de edición.
         - Captura todos los datos del post desde la BD mediante su ID
         */
        public ActionResult Edit(int id)
        {
            POST post = db.POSTs.Find(id);   //----De todos los post, Se encuentra un POST según su ID

            mPost postED = new mPost();
            postED.Id = post.POST_ID;
            postED.Fecha = Convert.ToDateTime(post.POST_FECHA);
            postED.Titulo = post.POST_TITULO;
            postED.Descripcion = post.POST_DESCRIPCION;
            postED.Imagen = post.POST_IMAGEN;
            postED.Tags = post.POST_TAGS.Split(',');
            postED.NombreAutor = post.POST_NOMBREAUTOR;

            return View(postED);    //----Retorno del objeto para ser trabajado en la vista.
        }

        //
        // POST: /Blog/Edit/5
        //------------------ Opera en la pagina de edición con el método POST unicamente.
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                POST postBD = db.POSTs.Find(id);   //----De todos los post, Se encuentra un POST según su ID
                
                //----------Se modifican todos los datos del post con los parámetros enviados (menos su ID)
                postBD.POST_TITULO = collection["Titulo"];
                postBD.POST_DESCRIPCION = collection["Descripcion"];
                postBD.POST_TAGS = collection["Tags"];
                postBD.POST_NOMBREAUTOR = collection["NombreAutor"];

                //-----Si se ha subido imagen, se procesa
                HttpPostedFileBase file = Request.Files["post_Imagen"];
                if (file.FileName != "")
                {
                    postBD.POST_IMAGEN = SubirFotografia(file);
                }

                postBD.POST_FECHA = DateTime.Now;    //---Fecha de modificación

                db.SaveChanges();           //-----Se guardan los cambios en la BD
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Blog/Delete/5
        /*
         - Método que se incia al ingresar a la pagina de eliminación.
         - Captura todos los datos del post desde la BD para mostrar informacion del post a eliminar.
         */
        public ActionResult Delete(int id)
        {
            POST post = db.POSTs.Find(id);   //----De todos los post, Se encuentra un POST según su ID

            mPost postELM = new mPost();
            postELM.Id = post.POST_ID;
            postELM.Fecha = Convert.ToDateTime(post.POST_FECHA);
            postELM.Titulo = post.POST_TITULO;
            postELM.Descripcion = post.POST_DESCRIPCION;
            postELM.Imagen = post.POST_IMAGEN;
            postELM.Tags = post.POST_TAGS.Split(',');
            postELM.NombreAutor = post.POST_NOMBREAUTOR;
            
            return View(postELM);       //-----Se trabaja el objeto en la vista.
        }

        //
        // POST: /Blog/Delete/5
        //------------------ Opera en la pagina de eliminación con el método POST unicamente.
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                POST postELM = db.POSTs.Find(id);   //----De todos los post, Se encuentra un POST según su ID

                //-----Se borra la imagen fisica del directorio de imagenes del post
                string rutaImagenPost = postELM.POST_IMAGEN;
                if (System.IO.File.Exists(rutaImagenPost)) System.IO.File.Delete(rutaImagenPost);

                db.POSTs.Remove(postELM);           //----Se remueve el post del registro
                db.SaveChanges();                   //----Se guardan los cambios

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

    }
}
