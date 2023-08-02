using IMageCurd.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace IMageCurd.Controllers
{
    public class HomeController : Controller
    {
        //Added Entity model object 

        ExampleDBEntities db = new ExampleDBEntities();
        public ActionResult Index()
        {
            var data = db.employees.ToList();
            return View(data);
        }

        // Create Method View
        public ActionResult Create()
        {
            
            return View();
        }

        // Create Method (Post) View
        [HttpPost]
        public ActionResult Create(employee e)
        {
            if (ModelState.IsValid == false)
            {
                string fileName = Path.GetFileNameWithoutExtension(e.ImageFile.FileName);
                string extension = Path.GetExtension(e.ImageFile.FileName);
                HttpPostedFileBase postedFile = e.ImageFile;
                int length = postedFile.ContentLength;
                if (extension.ToLower() ==".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                {
                    // inbyte = i mb
                    if (length<=1000000)
                    {
                        fileName = fileName + extension;
                        e.image_path = "~/images/" +fileName;
                        fileName = Path.Combine(Server.MapPath("~/images/"),fileName);
                        e.ImageFile.SaveAs(fileName);
                        db.employees.Add(e);
                        int a = db.SaveChanges();
                        if (a > 0)
                        {
                            TempData["UpdateMessage"] = "<script>alert(alert('Data Update Successfully'))</script>";
                            ModelState.Clear();
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            TempData["CreateMessage"] = "<script>alert(alert('Data not Inserted '))</script>";
                           
                        }
                    }
                    else
                    {
                        TempData["SizeMessage"] = "<script>alert('Image Size should be less the 1 MB')</script>";
                    }
                }
                else
                {
                    TempData["ExtensionMessage"] = "<script>alert('Format not supported')</script>";
                }
            }
            return View();
        }

        // Edit MEthod

        public ActionResult Edit(int id)
        {
            var EmployeeRow = db.employees.Where(model => model.id == id).FirstOrDefault();
            Session["Image"] = EmployeeRow.image_path;
            //ViewBag.Message = "Your application description page.";

            return View(EmployeeRow);
        }

        // Edit Post method
        [HttpPost]
        public ActionResult Edit(employee e)
        {
            
                if (e.ImageFile !=null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(e.ImageFile.FileName);
                    string extension = Path.GetExtension(e.ImageFile.FileName);
                    HttpPostedFileBase postedFile = e.ImageFile;
                    int length = postedFile.ContentLength;
                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                    {
                        // inbyte = i mb
                        if (length <= 1000000)
                        {
                            fileName = fileName + extension;
                            e.image_path = "~/images/" + fileName;
                            fileName = Path.Combine(Server.MapPath("~/images/"), fileName);
                            e.ImageFile.SaveAs(fileName);
                            db.Entry(e).State = EntityState.Modified;
                            int a = db.SaveChanges();
                            if (a > 0)
                            {
                                TempData["CreateMessage"] = "<script>alert(alert('Data Update Successfully'))</script>";
                                ModelState.Clear();
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                TempData["CreateMessage"] = "<script>alert(alert('Data not Update '))</script>";

                            }
                        }
                        else
                        {
                            TempData["SizeMessage"] = "<script>alert('Image Size should be less the 1 MB')</script>";
                        }
                    }
                    else
                    {
                        TempData["ExtensionMessage"] = "<script>alert('Format not supported')</script>";
                    }
                }
            else
            {
                e.image_path = Session["Image"].ToString();
                db.Entry(e).State = EntityState.Modified;
                int a = db.SaveChanges();
                if (a > 0)
                {
                    TempData["CreateMessage"] = "<script>alert(alert('Data Update Successfully'))</script>";
                    ModelState.Clear();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["CreateMessage"] = "<script>alert(alert('Data not Update '))</script>";

                }

            }

            
            return View();
        }

        // GET: Post/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            employee post = db.employees.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            employee post = db.employees.Find(id);
            db.employees.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            employee post = db.employees.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}