using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MultimediaManager.Models;
using System.Data;

namespace MultimediaManager.DAO
{
    public class SoundsManagement
    {
        int[] sound_extensions;
        static List<Sound> sounds = new List<Sound>();
        static List<AllFile> sound_files = new List<AllFile>();
        static public List<string> s_names = new List<string>();
        static public List<string> s_paths = new List<string>();
        static public List<int> s_ids = new List<int>();

        void BuildSoundList()
        {
            using (MultimediaEntities db = new MultimediaEntities())
            {
                sound_files = db.AllFiles.Where(x => x.FileIsOnDrive == true).OrderBy(x => x.FilePath).ThenBy(x => x.FileName).ToList<AllFile>();

                foreach (AllFile file in sound_files.ToList())
                {
                    if (!sound_extensions.Contains(file.FileExtensionID))
                    {
                        sound_files.Remove(file);
                    }
                }

                foreach (AllFile file in sound_files)
                {
                    sounds.Add(db.Sounds.Where(x => x.FileID == file.FileID).First());
                }
            }
        }

        void BuildSoundsView()
        {
            foreach (AllFile file in sound_files)
            {
                s_ids.Add(file.FileID);
                s_paths.Add(file.FilePath);
                s_names.Add(file.FileName);
            }
        }


        public List<string> GetSoundInfo(int sound_id)
        {
            List<string> sound_info = new List<string>();
            Sound f_sound;
            AllFile f_file;
            using (MultimediaEntities db = new MultimediaEntities())
            {
                f_sound = db.Sounds.Where(x => x.FileID == sound_id).First();
                f_file = db.AllFiles.Where(x => x.FileID == sound_id).First();
            }
            sound_info.Add(f_file.FileName);
            sound_info.Add(f_file.FilePath);
            sound_info.Add(f_file.FileSize.ToString());
            sound_info.Add(f_sound.SoundDuration.ToString());
            sound_info.Add(f_sound.SoundCurrentPosition.ToString());
            sound_info.Add(f_sound.SoundName.ToString());
            sound_info.Add(f_sound.SoundArtist.ToString());
            sound_info.Add(f_sound.SoundAlbum.ToString());
            return sound_info;
        }

        public void AddView(int sound_id)
        {
            using (MultimediaEntities db = new MultimediaEntities())
            {
                Sound f_sound = db.Sounds.Where(x => x.FileID == sound_id).First();
                f_sound.SoundViews++;
                db.Entry(f_sound).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        void GetSoundExtensions()
        {
            using (MultimediaEntities db = new MultimediaEntities())
            {
                sound_extensions = db.FileExtensions.Where(x => x.FileTypeID == 2).Select(x => x.FileExtensionID).ToArray<int>();
            }
        }

        public void AddFinding(int sound_id, int finding_type)
        {
            using (MultimediaEntities db = new MultimediaEntities())
            {
                Finding finding = new Finding { FileID = sound_id, FindingTypeID = finding_type };
                db.Findings.Add(finding);
                db.SaveChanges();
            }
        }

        public void UpdateSoundList()
        {
            sounds = new List<Sound>();
            sound_files = new List<AllFile>();
            s_ids = new List<int>();
            s_names = new List<string>();
            s_paths = new List<string>();
            GetSoundExtensions();
            BuildSoundList();
            BuildSoundsView();
        }

        public void UpdateSoundCurrentPosition(int sound_id, string curr_pos)
        {
            using (MultimediaEntities db = new MultimediaEntities())
            {
                Sound f_sound = db.Sounds.Where(x => x.FileID == sound_id).First();
                TimeSpan t_s;
                if (!TimeSpan.TryParse(curr_pos, out t_s))
                {
                    f_sound.SoundCurrentPosition = new TimeSpan(0, 0, 0);
                }
                else
                {
                    f_sound.SoundCurrentPosition = t_s;
                }
                db.Entry(f_sound).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}