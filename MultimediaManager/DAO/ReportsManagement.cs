using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MultimediaManager.Models;
using System.Data;

namespace MultimediaManager.DAO
{
    public class ReportsManagement
    {
        List<AllFile> max_size_files = new List<AllFile>(10);
        static List<AllFile> new_files = new List<AllFile>();
        static List<AllFile> deleted_files = new List<AllFile>();
        static List<AllFile> prev_files = new List<AllFile>();
        static List<AllFile> curr_files = new List<AllFile>();


        //**********************************StateReport
        public static void AddNewFile(AllFile new_file)
        {
            new_files.Add(new_file);
        }

        public void AddDeletedFile(AllFile deleted_file)
        {
            deleted_files.Add(deleted_file);
        }

        public void SavePrevFiles()
        {
            using (MultimediaEntities db = new MultimediaEntities())
            {
                //prev_files = db.AllFiles.Where(x => x.FileIsOnDrive == true).ToList<AllFile>();
            }
        }

        public void GetCurrFiles()
        {
            using (MultimediaEntities db = new MultimediaEntities())
            {
                curr_files = db.AllFiles.Where(x => x.FileIsOnDrive == true).ToList();
            }
        }
        public ulong GetTotalFilesSize()
        {
            ulong size = 0;
            foreach (AllFile file in curr_files)
            {
                size += (ulong)file.FileSize;
            }
            return size;
        }

        public int GetTotalFilesCount()
        {
            return curr_files.Count;
        }

        public int GetTotalImagesCount()
        {
            int[] image_extensions;
            int count = 0;
            using (MultimediaEntities db = new MultimediaEntities())
            {
                image_extensions = db.FileExtensions.Where(x => x.FileTypeID == 1).Select(x => x.FileExtensionID).ToArray<int>();
            }
            foreach (AllFile file in curr_files)
            {
                if (image_extensions.Contains(file.FileExtensionID))
                {
                    count++;
                }
            }
            return count;
        }
        public int GetTotalSoundsCount()
        {
            int[] sound_extensions;
            int count = 0;
            using (MultimediaEntities db = new MultimediaEntities())
            {
                sound_extensions = db.FileExtensions.Where(x => x.FileTypeID == 2).Select(x => x.FileExtensionID).ToArray<int>();
            }
            foreach (AllFile file in curr_files)
            {
                if (sound_extensions.Contains(file.FileExtensionID))
                {
                    count++;
                }
            }
            return count;
        }
        public int GetTotalVideosCount()
        {
            int[] video_extensions;
            int count = 0;
            using (MultimediaEntities db = new MultimediaEntities())
            {
                video_extensions = db.FileExtensions.Where(x => x.FileTypeID == 3).Select(x => x.FileExtensionID).ToArray<int>();
            }
            foreach (AllFile file in curr_files)
            {
                if (video_extensions.Contains(file.FileExtensionID))
                {
                    count++;
                }
            }
            return count;
        }

        public ulong GetTotalImagesSize()
        {
            int[] image_extensions;
            ulong size = 0;
            using (MultimediaEntities db = new MultimediaEntities())
            {
                image_extensions = db.FileExtensions.Where(x => x.FileTypeID == 1).Select(x => x.FileExtensionID).ToArray<int>();
            }
            foreach (AllFile file in curr_files)
            {
                if (image_extensions.Contains(file.FileExtensionID))
                {
                    size += (ulong)file.FileSize;
                }
            }
            return size;
        }
        public ulong GetTotalSoundsSize()
        {
            int[] sound_extensions;
            ulong size = 0;
            using (MultimediaEntities db = new MultimediaEntities())
            {
                sound_extensions = db.FileExtensions.Where(x => x.FileTypeID == 2).Select(x => x.FileExtensionID).ToArray<int>();
            }
            foreach (AllFile file in curr_files)
            {
                if (sound_extensions.Contains(file.FileExtensionID))
                {
                    size += (ulong)file.FileSize;
                }
            }
            return size;
        }
        public ulong GetTotalVideosSize()
        {
            int[] video_extensions;
            ulong size = 0;
            using (MultimediaEntities db = new MultimediaEntities())
            {
                video_extensions = db.FileExtensions.Where(x => x.FileTypeID == 3).Select(x => x.FileExtensionID).ToArray<int>();
            }
            foreach (AllFile file in curr_files)
            {
                if (video_extensions.Contains(file.FileExtensionID))
                {
                    size += (ulong)file.FileSize;
                }
            }
            return size;
        }

        public List<AllFile> GetTopSizedFiles()
        {
            List<AllFile> top_size = new List<AllFile>(10);
            using (MultimediaEntities db = new MultimediaEntities())
            {
                top_size = db.AllFiles.OrderByDescending(x => x.FileSize).Take(10).ToList();
            }
            return top_size;
        }

        public List<AllFile> GetNewFiles()
        {
            return new_files;
        }

        public List<AllFile> GetDeletedFiles()
        {
            int[] all_deleted_files;
            using (MultimediaEntities db = new MultimediaEntities())
            {
                all_deleted_files = db.AllFiles.Where(x => x.FileIsOnDrive == false).Select(x => x.FileID).ToArray<int>();
            }
            foreach (AllFile file in prev_files)
            {
                if (all_deleted_files.Contains(file.FileID))
                {
                    deleted_files.Add(file);
                }
            }
            return deleted_files;
        }

        //*************************************UsageReport

        public int GetFilesDBTotalCount()
        {
            int count;
            using (MultimediaEntities db = new MultimediaEntities())
            {
                count = db.AllFiles.Count();
            }
            return count;
        }
        public int GetImagesDBTotalCount()
        {
            int count;
            using (MultimediaEntities db = new MultimediaEntities())
            {
                count = db.Images.Count();
            }
            return count;
        }
        public int GetSoundsDBTotalCount()
        {
            int count;
            using (MultimediaEntities db = new MultimediaEntities())
            {
                count = db.Sounds.Count();
            }
            return count;
        }
        public int GetVideosDBTotalCount()
        {
            int count;
            using (MultimediaEntities db = new MultimediaEntities())
            {
                count = db.Videos.Count();
            }
            return count;
        }

        public int GetTotalImagesViews()
        {
            int views = 0;
            using (MultimediaEntities db = new MultimediaEntities())
            {
                foreach (Image file in db.Images.ToList())
                {
                    views += file.ImageViews;
                }
            }
            return views;
        }
        public int GetTotalSoundsViews()
        {
            int views = 0;
            using (MultimediaEntities db = new MultimediaEntities())
            {
                foreach (Sound file in db.Sounds.ToList())
                {
                    views += file.SoundViews;
                }
            }
            return views;
        }
        public int GetTotalVideosViews()
        {
            int views = 0;
            using (MultimediaEntities db = new MultimediaEntities())
            {
                foreach (Video file in db.Videos.ToList())
                {
                    views += file.VideoViews;
                }
            }
            return views;
        }

        public List<AllFile> GetTopViewedImages()
        {
            List<AllFile> top_images = new List<AllFile>(10);
            using (MultimediaEntities db = new MultimediaEntities())
            {
                List<Image> topimages = db.Images.OrderBy(x => x.ImageViews).Take(10).ToList();
                foreach (Image image in topimages)
                {
                    top_images.Add(db.AllFiles.Find(image.FileID));
                }
            }
            return top_images;
        }
        public List<AllFile> GetTopViewedSounds()
        {
            List<AllFile> top_sounds = new List<AllFile>(10);
            using (MultimediaEntities db = new MultimediaEntities())
            {
                List<Sound> topsounds = db.Sounds.OrderBy(x => x.SoundViews).Take(10).ToList();
                foreach (Sound sound in topsounds)
                {
                    top_sounds.Add(db.AllFiles.Find(sound.FileID));
                }
            }
            return top_sounds;
        }
        public List<AllFile> GetTopViewedVideos()
        {
            List<AllFile> top_videos = new List<AllFile>(10);
            using (MultimediaEntities db = new MultimediaEntities())
            {
                List<Video> topvideos = db.Videos.OrderBy(x => x.VideoViews).Take(10).ToList();
                foreach (Video image in topvideos)
                {
                    top_videos.Add(db.AllFiles.Find(image.FileID));
                }
            }
            return top_videos;
        }

        public int GetSoundInProcessCount()
        {
            int count = 0;
            using (MultimediaEntities db = new MultimediaEntities())
            {
                TimeSpan ts = new TimeSpan(0, 0, 0);
                count = db.Sounds.Where(x => x.SoundCurrentPosition > ts).Count();
            }
            return count;
        }
        public int GetVideoInProcessCount()
        {
            int count = 0;
            using (MultimediaEntities db = new MultimediaEntities())
            {
                TimeSpan ts = new TimeSpan(0, 0, 0);
                count = db.Videos.Where(x => x.VideoCurrentPosition > ts).Count();
            }
            return count;
        }

        public string GetTotalSoundProcessed()
        {
            TimeSpan ts = new TimeSpan(0, 0, 0);
            using (MultimediaEntities db = new MultimediaEntities())
            {
                foreach(Sound sound in db.Sounds.ToList())
                {
                    for (int i = 0; i < sound.SoundViews; i++)
                    {
                        ts.Add(sound.SoundDuration);
                    }
                    ts.Add(sound.SoundCurrentPosition);
                }
            }
            return ts.ToString();
        }

        public string GetTotalVideoProcessed()
        {
            TimeSpan ts = new TimeSpan(0, 0, 0);
            using (MultimediaEntities db = new MultimediaEntities())
            {
                foreach (Video video in db.Videos.ToList())
                {
                    for (int i = 0; i < video.VideoViews; i++)
                    {
                        ts.Add(video.VideoDuration);
                    }
                    ts.Add(video.VideoCurrentPosition);
                }
            }
            return ts.ToString();
        }
    }
}