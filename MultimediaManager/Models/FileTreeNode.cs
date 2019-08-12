using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MultimediaManager.Models
{
    public class FileTreeNode
    {
        public int id = -1;
        public FileTreeNode[] nodes;

        public String text;

        public String href;

        public FileTreeNode()
        {
            
            this.href = null;
            
            this.nodes = null;
            
            this.text = null;
        }

        public FileTreeNode(FileTreeNode[] children, string name)
        {
            
            this.href = "";
          
            this.nodes = children;
            
            this.text = name;
        }

        public FileTreeNode(/*FileTreeNode root, FileTreeNode parent, */FileTreeNode[] children, string name, string file)
        {
            
            this.href = file;
            
            this.nodes = children;
            
            this.text = name;
        }


    }
}