using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace ShoppingCarts.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {

            //宣告回傳商品列表陣列result
            List<Models.Product> result = new List<Models.Product>();
            ViewBag.ResultMessage = TempData["ResultMessage"];

            //使用CartsEntities類別，名稱為db
            using (Models.ShoppingCartsEntities db = new Models.ShoppingCartsEntities())
            {
                //使用LinQ語法抓取目前products資料庫中的資料
                result = (from s in db.Products select s).ToList();

                //將result傳送給檢視
                return View(result);
            }
                
        }
        //建立商品頁面
        public ActionResult Create()
        {
            return View(); 
        }
        //建立商品頁面 - 資料傳回處理
        [HttpPost] //指定只有POST方法才可進入
        public ActionResult Create(Models.Product postback)
        {
            using (Models.ShoppingCartsEntities db = new Models.ShoppingCartsEntities())
            {
                //將回傳資料postback加入至Products
                db.Products.Add(postback);

                //儲存異動資料
                db.SaveChanges();
            }
            return View();
        }
        //編輯商品頁面
        public ActionResult Edit (int id)
        {

            using (Models.ShoppingCartsEntities db = new Models.ShoppingCartsEntities())
            {
                //抓取Product.Id等於輸入id的資料
                var result = (from s in db.Products where s.Id == id select s).FirstOrDefault();
                if (result != default(Models.Product)) //判斷此id是否有資料
                {
                    return View(result); //如果有id回傳編輯商品頁面
                }
                else
                {
                    //如果沒有資料則顯示錯誤訊息並導回Index頁面
                    TempData["ResultMessage"] = "資料有誤，請重新操作";
                    return RedirectToAction("Index");
                }

            }

        }
        //編輯商品頁面 - 資料傳回處理
        [HttpPost]
        public ActionResult Edit(Models.Product postback)
        {
            if(this.ModelState.IsValid) //判斷使用者輸入資料是否正確
            {
                using (Models.ShoppingCartsEntities db = new Models.ShoppingCartsEntities())
                {
                    //抓取Product,ID等於回傳postback.Id的資料
                    var result = (from s in db.Products where s.Id == postback.Id select s).FirstOrDefault();

                    //儲存使用者變更資料
                    result.Name = postback.Name; result.Price = postback.Price;
                    result.PublishDate = postback.PublishDate; result.Quantity = postback.Quantity;
                    result.Status = postback.Status; result.CategoryID = postback.CategoryID;
                    result.DefaultImageId = postback.DefaultImageId; result.Description = postback.Description;
                    result.DefaultImageURL = postback.DefaultImageURL;

                    //儲存所有變更
                    db.SaveChanges();

                    //設定成功訊息並導回Index頁面
                    TempData["ResultMessage"] = string.Format("商品{0}成功編輯", postback.Name);
                    return RedirectToAction("Index");
                }

            }
            else //如果資料不正確則導向自己(Edit頁面)
            {
                return View(postback);
            }
        }
        //刪除商品
        public ActionResult Delete(int id)
        {
            using (Models.ShoppingCartsEntities db = new Models.ShoppingCartsEntities())
            {
                //抓取Product.Id等於輸入id的資料
                var result = (from s in db.Products where s.Id == id select s).FirstOrDefault();
                if( result != default(Models.Product)) //判斷此id是否有資料
                {
                    db.Products.Remove(result);
                    //儲存所有變更
                    db.SaveChanges();

                    //設定成功訊息並導回index頁面
                    TempData["ResultMessage"] = String.Format("商品:{0}成功刪除", result.Name);
                    return RedirectToAction("Index");

                }
                else
                { //如果沒有資料則顯示錯誤訊息並導回Index頁面
                    TempData["ResultMessage"] = "指定資料不存在，無法刪除，請重新操作";
                    return RedirectToAction("Index");
                                
                }
            }
        }

        private string fileSavedPath = WebConfigurationManager.AppSettings["UploadPath"];

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            JObject jo = new JObject();
            string result = string.Empty;

            if (file == null)
            {
                jo.Add("Result", false);
                jo.Add("Msg", "請上傳檔案!");
                result = JsonConvert.SerializeObject(jo);
                return Content(result, "application/json");
            }
            if (file.ContentLength <= 0)
            {
                jo.Add("Result", false);
                jo.Add("Msg", "請上傳正確的檔案.");
                result = JsonConvert.SerializeObject(jo);
                return Content(result, "application/json");
            }

            string fileExtName = Path.GetExtension(file.FileName).ToLower();

            if (!fileExtName.Equals(".xls", StringComparison.OrdinalIgnoreCase)
                &&
                !fileExtName.Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                jo.Add("Result", false);
                jo.Add("Msg", "請上傳 .xls 或 .xlsx 格式的檔案");
                result = JsonConvert.SerializeObject(jo);
                return Content(result, "application/json");
            }

            try
            {
                var uploadResult = this.FileUploadHandler(file);

                jo.Add("Result", !string.IsNullOrWhiteSpace(uploadResult));
                jo.Add("Msg", !string.IsNullOrWhiteSpace(uploadResult) ? uploadResult : "");

                result = JsonConvert.SerializeObject(jo);
            }
            catch (Exception ex)
            {
                jo.Add("Result", false);
                jo.Add("Msg", ex.Message);
                result = JsonConvert.SerializeObject(jo);
            }
            return Content(result, "application/json");
        }

        /// <summary>
        /// Files the upload handler.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">file;上傳失敗：沒有檔案！</exception>
        /// <exception cref="System.InvalidOperationException">上傳失敗：檔案沒有內容！</exception>
        private string FileUploadHandler(HttpPostedFileBase file)
        {
            string result;

            if (file == null)
            {
                throw new ArgumentNullException("file", "上傳失敗：沒有檔案！");
            }
            if (file.ContentLength <= 0)
            {
                throw new InvalidOperationException("上傳失敗：檔案沒有內容！");
            }

            try
            {
                string virtualBaseFilePath = Url.Content(fileSavedPath);
                string filePath = HttpContext.Server.MapPath(virtualBaseFilePath);

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                string newFileName = string.Concat(
                    DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                    Path.GetExtension(file.FileName).ToLower());

                string fullFilePath = Path.Combine(Server.MapPath(fileSavedPath), newFileName);
                file.SaveAs(fullFilePath);

                result = newFileName;
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }
    }
}