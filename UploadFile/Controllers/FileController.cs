using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using UploadFile.Helpers;

namespace UploadFile.Controllers
{
    public class FileController : Controller
    {
        private IHostingEnvironment _environment;

        public FileController(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadAsync(List<IFormFile> files)
        {
 
            //改用 _config 設定圖片路徑
            //var filePath = Path.Combine(_config["StoredFilesPath"], Path.GetRandomFileName());
 
            var size = files.Sum(f => f.Length);

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    if (!file.IsImage())
                    {
                        return Content("Not Image File");
                    } 
   
                    int id = 0;
                    string wwwRootPath = _environment.WebRootPath;
                    string extension = Path.GetExtension(file.FileName);
                    string fileName = $"{id.ToString()}_{DateTime.Now.ToString("yyyyMMddHHmmss")}{extension}";      
                    string path =  Path.Combine(wwwRootPath + "/img/outertransfer/", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }

            return Ok(new { count = files.Count, size });
        }
    }
}
