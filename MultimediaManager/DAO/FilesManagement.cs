using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MultimediaManager.Models;
using System.IO;
using System.Data;
using System.Drawing;
using WMPLib;

namespace MultimediaManager.DAO
{
    /// <summary>
    /// Class for working with current files on drives
    /// </summary>
    public class FilesManagement
    {
        //Tree structure with current files
        static List<FileTreeNode> files_tree = new List<FileTreeNode>();
        //List of all extensions
        static string[] extensions;
        /// <summary>
        /// Lists for viewing files on drives
        /// </summary>
        static public List<int> MMlevels = new List<int>();
        static public List<string> MMnames = new List<string>();
        static public List<string> MMhrefs = new List<string>();
        static public List<int> MMids = new List<int>();

        //Building tree structure with files
        public void BuildFileTree()
        {
            GetExtentions();
            FileTreeNode drive;
            //foreach (DriveInfo d in DriveInfo.GetDrives())
            //{
                
                    drive = MMSearch("D:\\");
                    if (drive != null)
                    {
                        files_tree.Add(drive);
                        buildMMView(0, drive);
                    }
                
            //}
        }
        //Checking multimedia files existence
        bool HasMMFiles(string sDir)
        {
            string curr_extention;

            foreach (string f in Directory.GetFiles(sDir))
            {
                curr_extention = f.Remove(0, f.LastIndexOf('.'));
                curr_extention = curr_extention.ToUpper();
                if (extensions.Contains(curr_extention))
                {
                    return true;
                }
            }
            return false;
        }
        //Recurse seaching in folders on drives
        FileTreeNode MMSearch(string sDir)
        {
            FileTreeNode curr = new FileTreeNode();
            string l = sDir.Remove(0, sDir.LastIndexOf('\\') + 1);
            if (l == "")
            {
                l = sDir;
            }
            try
            {
                if (Directory.GetDirectories(sDir).Count() == 0)
                {
                    if (HasMMFiles(sDir))
                    {
                        curr = new FileTreeNode(GetFileNodes(sDir).ToArray<FileTreeNode>(), l);
                        return curr;
                    }
                    else
                    {
                        return null;
                    }
                }
                if (Directory.GetDirectories(sDir).Count() > 0)
                {
                    List<FileTreeNode> children = new List<FileTreeNode>();
                    FileTreeNode child;
                    foreach (string d in Directory.GetDirectories(sDir))
                    {
                        child = MMSearch(d);
                        if (child != null)
                        {
                            children.Add(child);
                        }
                    }
                    if (HasMMFiles(sDir))
                    {
                        foreach (FileTreeNode f in GetFileNodes(sDir))
                        {
                            children.Add(f);
                        }
                    }
                    if (children.Count != 0)
                    {
                        curr = new FileTreeNode(children.ToArray<FileTreeNode>(), l);
                        return curr;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }
        //Getting multimedia files in current folder
        List<FileTreeNode> GetFileNodes(string sDir)
        {
            List<FileTreeNode> files = new List<FileTreeNode>();
            string curr_extention;
            FileTreeNode l;
            foreach (string f in Directory.GetFiles(sDir))
            {
                curr_extention = f.Remove(0, f.LastIndexOf('.'));
                curr_extention = curr_extention.ToUpper();
                if (extensions.Contains(curr_extention))
                {
                    l = new FileTreeNode(null, f.Remove(0, f.LastIndexOf('\\') + 1), f);
                    l.id = CheckDBFile(l);
                    files.Add(l);
                }
            }
            return files;
        }
        //Getting extensions from database
        void GetExtentions()
        {
            MultimediaEntities db = new MultimediaEntities();
            extensions = db.FileExtensions.Select(x => x.FileExtensionName).ToArray<string>();
            for (int i = 0; i < extensions.Count(); i++)
            {
                extensions[i] = extensions[i].ToUpper();
            }
        }
        //Conversion file information to database file table format
        AllFile BuilsNewMMFile(FileTreeNode f, string sDir)
        {
            AllFile mm_file = new AllFile();
            FileInfo fi = new FileInfo(f.href);
            mm_file.FileName = f.text;
            mm_file.FileIsOnDrive = true;
            mm_file.FilePath = sDir;
            mm_file.FileExtensionID = GetExtention(f.text.Substring(f.text.IndexOf('.'))).FileExtensionID;
            mm_file.FileSize = (int)fi.Length / 1024;
            mm_file.FileAdded = DateTime.Now;
            mm_file.FileChanged = fi.LastWriteTime;
            mm_file.FileCreated = fi.CreationTime;
            return mm_file;
        }
        //Getting extension element from database due to string extension
        FileExtension GetExtention(string extension)
        {
            FileExtension fe;
            using (MultimediaEntities db = new MultimediaEntities())
            {
                fe = db.FileExtensions.Where(x => x.FileExtensionName.ToUpper() == extension.ToUpper()).First();
            }
            return fe;
        }
        //Creating structures for view
        void buildMMView(int level, FileTreeNode node)
        {
            MMlevels.Add(level);
            MMnames.Add(node.text);
            MMhrefs.Add(node.href);
            MMids.Add(node.id);
            if (node.nodes != null)
            {
                foreach (FileTreeNode c in node.nodes)
                {
                    buildMMView(level + 1, c);
                }
            }
        }
        //Reseting file existion in database
        public void ResetFilesDB()
        {
            using (MultimediaEntities db = new MultimediaEntities())
            {
                foreach(AllFile file in db.AllFiles)
                {
                    file.FileIsOnDrive = false;
                    db.Entry(file).State = EntityState.Modified;
                }
                db.SaveChanges();
            }
        }
        //Input file into database
        int CheckDBFile (FileTreeNode file)
        {
            string file_path = file.href.Remove(file.href.LastIndexOf('\\') + 1);
            FileInfo fi = new FileInfo(file.href);
            AllFile f_file;
            using (MultimediaEntities db = new MultimediaEntities())
            {
                try
                {
                    f_file = db.AllFiles.Where(x => x.FileName == file.text && x.FilePath == file_path && x.FileSize == (int)fi.Length / 1024).First();
                    if (f_file != null)
                    {
                        f_file.FileIsOnDrive = true;
                        db.Entry(f_file).State = EntityState.Modified;
                    }
                    else
                    {
                        f_file = BuilsNewMMFile(file, file_path);
                        ReportsManagement.AddNewFile(f_file);
                        db.AllFiles.Add(f_file);
                    }
                }
                catch(Exception ex)
                {
                    f_file = BuilsNewMMFile(file, file_path);
                    ReportsManagement.AddNewFile(f_file);
                    db.AllFiles.Add(f_file);
                }
                db.SaveChanges();
                f_file = db.AllFiles.Where(x => x.FileName == file.text && x.FilePath == file_path && x.FileSize == (int)fi.Length / 1024).First();
                switch (db.FileExtensions.Find(f_file.FileExtensionID).FileTypeID)
                {
                    case 1:
                        {
                            if (!db.Images.Select(x => x.FileID).Contains(f_file.FileID))
                            {
                                AddToImages(file, f_file);
                            }
                            break;
                        }
                    case 2:
                        {
                            if (!db.Sounds.Select(x => x.FileID).Contains(f_file.FileID))
                            {
                                AddToSounds(file, f_file);
                            }
                            break;
                        }
                    case 3:
                        {
                            if (!db.Videos.Select(x => x.FileID).Contains(f_file.FileID))
                            {
                                AddToVideos(file, f_file);
                            }
                            break;
                        }
                }
            }
            return f_file.FileID;
        }
        //Adding new image from file
        void AddToImages(FileTreeNode file, AllFile db_file)
        {
            Models.Image image_db = new Models.Image();
            System.Drawing.Image image_info = System.Drawing.Image.FromFile(file.href);
            image_db.FileID = db_file.FileID;
            image_db.ImageSize = image_info.Size.Width.ToString() + "x" + image_info.Size.Height.ToString();
            image_db.ImageViews = 0;
            using (MultimediaEntities db = new MultimediaEntities())
            {
                db.Images.Add(image_db);
                db.SaveChanges();
            }
        }
        //Adding new sound from file
        void AddToSounds(FileTreeNode file, AllFile db_file)
        {
            Sound sound_db = new Sound();
            sound_db.FileID = db_file.FileID;
            TagLib.File sound_tags = TagLib.File.Create(file.href);
            try
            {
                sound_db.SoundAlbum = sound_tags.Tag.Album;
            }
            catch
            {
                sound_db.SoundAlbum = "Unknown";
            }
            try
            {
                sound_db.SoundArtist = sound_tags.Tag.AlbumArtists[0];
            }
            catch
            {
                sound_db.SoundArtist = "Unknown";
            }
            try
            {
                sound_db.SoundDuration = sound_tags.Properties.Duration.Duration();
            }
            catch
            {
                sound_db.SoundDuration = new TimeSpan(0, 3, 0);
            }
            try
            {
                sound_db.SoundName = sound_tags.Tag.Title;
            }
            catch
            {
                sound_db.SoundName = "Unknown";
            }
            sound_db.SoundCurrentPosition = new TimeSpan(0, 0, 0);
            sound_db.SoundViews = 0;

            using (MultimediaEntities db = new MultimediaEntities())
            {
                db.Sounds.Add(sound_db);
                db.SaveChanges();
            }
        }
        //Adding new video from file
        void AddToVideos(FileTreeNode file, AllFile db_file)
        {
            Video video_db = new Video();
            video_db.FileID = db_file.FileID;
            video_db.VideoCurrentPosition = new TimeSpan(0, 0, 0);
            try
            {
                WindowsMediaPlayer wmp = new WindowsMediaPlayer();
                IWMPMedia mediainfo = wmp.newMedia(file.href);
                string[] time = mediainfo.durationString.Split(':');
                video_db.VideoDuration = new TimeSpan(Int32.Parse(time[0]), Int32.Parse(time[1]), Int32.Parse(time[2]));
            }
            catch
            {
                video_db.VideoDuration = new TimeSpan(0, 0, 32);
            }
            video_db.VideoViews = 0;
            using (MultimediaEntities db = new MultimediaEntities())
            {
                db.Videos.Add(video_db);
                db.SaveChanges();
            }
        }
    }

}