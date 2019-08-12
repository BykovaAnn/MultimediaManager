using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MultimediaManager.DAO;

namespace MultimediaManager.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
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

        public ActionResult FileManager()
        {
            ViewBag.MMTreeViewLevels = FilesManagement.MMlevels;
            ViewBag.MMTreeViewNames = FilesManagement.MMnames;
            ViewBag.MMTreeViewHrefs = FilesManagement.MMhrefs;
            ViewBag.MMTreeViewIds = FilesManagement.MMids;
            ViewBag.MMTreeViewSize = FilesManagement.MMlevels.Count;
            return View();
        }

        public ActionResult ImageManager()
        {
            ImagesManagement im = new ImagesManagement();
            im.UpdateImageList();
            ViewBag.ImageNames = ImagesManagement.i_names;
            ViewBag.ImageIds = ImagesManagement.i_ids;
            ViewBag.ImagePaths = ImagesManagement.i_paths;
            ViewBag.ImageCount = ImagesManagement.image_files.Count;
            return View();
        }

        public ActionResult SoundManager()
        {
            SoundsManagement sm = new SoundsManagement();
            sm.UpdateSoundList();
            ViewBag.SoundNames = SoundsManagement.s_names;
            ViewBag.SoundIds = SoundsManagement.s_ids;
            ViewBag.SoundPaths = SoundsManagement.s_paths;
            ViewBag.SoundCount = SoundsManagement.s_ids.Count;
            return View();
        }

        public ActionResult VideoManager()
        {
            VideosManagement vm = new VideosManagement();
            vm.UpdateVideoList();
            ViewBag.VideoNames = VideosManagement.v_names;
            ViewBag.VideoIds = VideosManagement.v_ids;
            ViewBag.VideoPaths = VideosManagement.v_paths;
            ViewBag.VideoCount = VideosManagement.v_ids.Count;
            return View();
        }

        public ActionResult ReportState()
        {
            ReportsManagement rm = new ReportsManagement();
            rm.GetCurrFiles();
            ViewBag.TotalSize = rm.GetTotalFilesSize();
            ViewBag.TotalFiles = rm.GetTotalFilesCount();
            ViewBag.TotalImages = rm.GetTotalImagesCount();
            ViewBag.TotalSounds = rm.GetTotalSoundsCount();
            ViewBag.TotalVideos = rm.GetTotalVideosCount();
            ViewBag.TotalImagesSize = rm.GetTotalImagesSize();
            ViewBag.TotalSoundsSize = rm.GetTotalSoundsSize();
            ViewBag.TotalVideosSize = rm.GetTotalVideosSize();
            ViewBag.TopSizeFiles = rm.GetTopSizedFiles().ToList();
            ViewBag.NewFiles = rm.GetNewFiles().ToList();
            ViewBag.DeletedFiles = rm.GetDeletedFiles().ToList();
            return View();
        }

        public ActionResult ReportUsage()
        {
            ReportsManagement rm = new ReportsManagement();
            rm.GetCurrFiles();
            ViewBag.TotalFiles = rm.GetFilesDBTotalCount();
            ViewBag.TotalImages = rm.GetImagesDBTotalCount();
            ViewBag.TotalSounds = rm.GetSoundsDBTotalCount();
            ViewBag.TotalVideos = rm.GetVideosDBTotalCount();
            ViewBag.TotalImageViews = rm.GetTotalImagesViews();
            ViewBag.TotalSoundViews = rm.GetTotalSoundsViews();
            ViewBag.TotalVideoViews = rm.GetTotalVideosViews();
            ViewBag.TopImageViews = rm.GetTopViewedImages().ToList();
            ViewBag.TopSoundViews = rm.GetTopViewedSounds().ToList();
            ViewBag.TopVideoViews = rm.GetTopViewedVideos().ToList();
            ViewBag.ProcessingSoundsCount = rm.GetSoundInProcessCount();
            ViewBag.ProcessingVideosCount = rm.GetVideoInProcessCount();
            ViewBag.TimeSound = rm.GetTotalSoundProcessed();
            ViewBag.TimeVideo = rm.GetTotalVideoProcessed();
            return View();
        }

        public JsonResult getFileInfo(int fileID)
        {
            MultimediaManager.Models.AllFile result;
            MultimediaManager.Models.FileExtension extension;
            string type1 = "";
            using (MultimediaManager.Models.MultimediaEntities db = new Models.MultimediaEntities())
            {
                result = db.AllFiles.Find(fileID);
                extension = db.FileExtensions.Find(result.FileExtensionID);
            }
            if (extension.FileTypeID == 1)
            {
                type1 = "Image";
            }
            if (extension.FileTypeID == 2)
            {
                type1 = "Sound";
            }
            if (extension.FileTypeID == 3)
            {
                type1 = "Video";
            }
            var res = new
            {
                name = result.FileName,
                path = result.FilePath,
                size = result.FileSize,
                added = result.FileAdded.ToString(),
                changed = result.FileChanged.ToString(),
                created = result.FileCreated.ToString(),
                extension = extension.FileExtensionName,
                type = type1
            };
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getImageInfo(int fileID)
        {
            ImagesManagement im = new ImagesManagement();
            List<string> result = im.GetImageInfo(fileID);
            var res = new
            {
                name = result[0],
                path = result[1],
                size = result[2],
                resolution = result[3]
            };
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getSoundInfo(int fileID)
        {
            SoundsManagement sm = new SoundsManagement();
            List<string> result = sm.GetSoundInfo(fileID);
            var res = new
            {
                name = result[0],
                path = result[1],
                size = result[2],
                duration = result[3],
                currpos = result[4],
                title = result[5],
                artist = result[6],
                album = result[7]
            };
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getVideoInfo(int fileID)
        {
            VideosManagement vm = new VideosManagement();
            List<string> result = vm.GetVideoInfo(fileID);
            var res = new
            {
                name = result[0],
                path = result[1],
                size = result[2],
                duration = result[3],
                currpos = result[4],
            };
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        //FORDOWNLOaDING
        public FileResult tre()
        {
            return File("E:\\1.jpg", System.Net.Mime.MediaTypeNames.Application.Octet);
        }
    }
}