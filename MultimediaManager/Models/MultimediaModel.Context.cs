﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MultimediaManager.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MultimediaEntities : DbContext
    {
        public MultimediaEntities()
            : base("name=MultimediaEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<AllFile> AllFiles { get; set; }
        public DbSet<FileExtension> FileExtensions { get; set; }
        public DbSet<FileType> FileTypes { get; set; }
        public DbSet<Finding> Findings { get; set; }
        public DbSet<FindingType> FindingTypes { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Sound> Sounds { get; set; }
        public DbSet<sysdiagram> sysdiagrams { get; set; }
        public DbSet<Video> Videos { get; set; }
    }
}