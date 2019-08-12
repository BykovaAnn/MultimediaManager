using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using MultimediaManager.Models;
using System.Data;

namespace MultimediaManager.DAO
{
    public class ImagesManagement
    {
        int[] image_extensions;
        public static List<Models.Image> images = new List<Models.Image>();
        public static List<AllFile> image_files = new List<AllFile>();
        static public List<string> i_names = new List<string>();
        static public List<string> i_paths = new List<string>();
        static public List<int> i_ids = new List<int>();

        void BuildImageList()
        {
            using (MultimediaEntities db = new MultimediaEntities())
            {
                image_files = db.AllFiles.Where(x => x.FileIsOnDrive == true).OrderBy(x => x.FilePath).ThenBy(x => x.FileName).ToList<AllFile>();

                foreach (AllFile file in image_files.ToList())
                {
                    if (!image_extensions.Contains(file.FileExtensionID))
                    {
                        image_files.Remove(file);
                    }
                }

                foreach (AllFile file in image_files)
                {
                    images.Add(db.Images.Where(x => x.FileID == file.FileID).First());
                }
            }
        }

        void BuildImagesView()
        {
            foreach (AllFile file in image_files)
            {
                i_ids.Add(file.FileID);
                i_paths.Add(file.FilePath);
                i_names.Add(file.FileName);
            }
        }
        public List<string> GetImageInfo(int image_id)
        {
            List<string> image_info = new List<string>();
            Models.Image f_image;
            AllFile f_file;
            using (MultimediaEntities db = new MultimediaEntities())
            {
                f_image = db.Images.Where(x => x.FileID == image_id).First();
                f_file = db.AllFiles.Where(x => x.FileID == image_id).First();
            }
            image_info.Add(f_file.FileName);
            image_info.Add(f_file.FilePath);
            image_info.Add(f_file.FileSize.ToString());
            image_info.Add(f_image.ImageSize);
            return image_info;
        }

        public void AddView(int image_id)
        {
            using (MultimediaEntities db = new MultimediaEntities())
            {
                Models.Image f_image = db.Images.Where(x => x.FileID == image_id).First();
                f_image.ImageViews++;
                db.Entry(f_image).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        void GetImageExtensions()
        {
            using (MultimediaEntities db = new MultimediaEntities())
            {
                image_extensions = db.FileExtensions.Where(x => x.FileTypeID == 1).Select(x => x.FileExtensionID).ToArray<int>();
            }
        }

        public void AddFinding(int image_id, int finding_type)
        {
            using (MultimediaEntities db = new MultimediaEntities())
            {
                Finding finding = new Finding { FileID = image_id, FindingTypeID = finding_type };
                db.Findings.Add(finding);
                db.SaveChanges();
            }
        }

        public void UpdateImageList()
        {
            images = new List<Models.Image>();
            image_files = new List<AllFile>();
            i_ids = new List<int>();
            i_names = new List<string>();
            i_paths = new List<string>();
            GetImageExtensions();
            BuildImageList();
            BuildImagesView();
        }

        
    }
}