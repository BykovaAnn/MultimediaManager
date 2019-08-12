using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MultimediaManager.Models;
using System.Data;

namespace MultimediaManager.DAO
{
    public class VideosManagement
    {
        int[] video_extensions;
        static List<Video> videos = new List<Video>();
        static List<AllFile> video_files = new List<AllFile>();

        static public List<string> v_names = new List<string>();
        static public List<string> v_paths = new List<string>();
        static public List<int> v_ids = new List<int>();

        void BuildVideoList()
        {
            using (MultimediaEntities db = new MultimediaEntities())
            {
                video_files = db.AllFiles.Where(x => x.FileIsOnDrive == true).OrderBy(x => x.FilePath).ThenBy(x => x.FileName).ToList<AllFile>();

                foreach (AllFile file in video_files.ToList())
                {
                    if (!video_extensions.Contains(file.FileExtensionID))
                    {
                        video_files.Remove(file);
                    }
                }

                foreach (AllFile file in video_files)
                {
                    videos.Add(db.Videos.Where(x => x.FileID == file.FileID).First());
                }
            }
        }

        void BuildVideosView()
        {
            foreach (AllFile file in video_files)
            {
                v_ids.Add(file.FileID);
                v_paths.Add(file.FilePath);
                v_names.Add(file.FileName);
            }
        }

        public List<string> GetVideoInfo(int video_id)
        {
            List<string> video_info = new List<string>();
            Video f_video;
            AllFile f_file;
            using (MultimediaEntities db = new MultimediaEntities())
            {
                f_video = db.Videos.Where(x => x.FileID == video_id).First();
                f_file = db.AllFiles.Where(x => x.FileID == video_id).First();
            }
            video_info.Add(f_file.FileName);
            video_info.Add(f_file.FilePath);
            video_info.Add(f_file.FileSize.ToString());
            video_info.Add(f_video.VideoDuration.ToString());
            video_info.Add(f_video.VideoCurrentPosition.ToString());
            return video_info;
        }

        public void AddView(int video_id)
        {
            using (MultimediaEntities db = new MultimediaEntities())
            {
                Video f_video = db.Videos.Where(x => x.FileID == video_id).First();
                f_video.VideoViews++;
                db.Entry(f_video).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        void GetVideoExtensions()
        {
            using (MultimediaEntities db = new MultimediaEntities())
            {
                video_extensions = db.FileExtensions.Where(x => x.FileTypeID == 3).Select(x => x.FileExtensionID).ToArray<int>();
            }
        }

        public void AddFinding(int video_id, int finding_type)
        {
            using (MultimediaEntities db = new MultimediaEntities())
            {
                Finding finding = new Finding { FileID = video_id, FindingTypeID = finding_type };
                db.Findings.Add(finding);
                db.SaveChanges();
            }
        }

        public void UpdateVideoList()
        {
            videos = new List<Video>();
            video_files = new List<AllFile>();
            v_ids = new List<int>();
            v_names = new List<string>();
            v_paths = new List<string>();
            GetVideoExtensions();
            BuildVideoList();
            BuildVideosView();
        }

        public void UpdateVideoCurrentPosition(int video_id, string curr_pos)
        {
            using (MultimediaEntities db = new MultimediaEntities())
            {
                Video f_video = db.Videos.Where(x => x.FileID == video_id).First();
                TimeSpan t_s;
                if (!TimeSpan.TryParse(curr_pos, out t_s))
                {
                    f_video.VideoCurrentPosition = new TimeSpan(0, 0, 0);
                }
                else
                {
                    f_video.VideoCurrentPosition = t_s;
                }
                db.Entry(f_video).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}