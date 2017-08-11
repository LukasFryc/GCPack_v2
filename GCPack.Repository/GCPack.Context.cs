﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GCPack.Repository
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class GCPackContainer : DbContext
    {
        public GCPackContainer()
            : base("name=GCPackContainer")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AppSystem> AppSystems { get; set; }
        public virtual DbSet<Division> Divisions { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<JobPosition> JobPositions { get; set; }
        public virtual DbSet<LogEvent> LogEvents { get; set; }
        public virtual DbSet<ReadConfirmation> ReadConfirmations { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Signature> Signatures { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<Login> Logins { get; set; }
        public virtual DbSet<File> Files { get; set; }
        public virtual DbSet<JobPositionUser> JobPositionUsers { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<JobPositionDocument> JobPositionDocuments { get; set; }
        public virtual DbSet<UserDocument> UserDocuments { get; set; }
        public virtual DbSet<DocumentType> DocumentTypes { get; set; }
        public virtual DbSet<DivisionDocument> DivisionDocuments { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectDocument> ProjectDocuments { get; set; }
        public virtual DbSet<SystemDocument> SystemDocuments { get; set; }
        public virtual DbSet<DocumentState> DocumentStates { get; set; }
    
        public virtual int GetDocuments(Nullable<int> forUserID, Nullable<int> documentID, string name, string number, string administrator, string orderBy, Nullable<int> documentTypeID, Nullable<int> page, Nullable<int> itemsPerPage)
        {
            var forUserIDParameter = forUserID.HasValue ?
                new ObjectParameter("forUserID", forUserID) :
                new ObjectParameter("forUserID", typeof(int));
    
            var documentIDParameter = documentID.HasValue ?
                new ObjectParameter("documentID", documentID) :
                new ObjectParameter("documentID", typeof(int));
    
            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));
    
            var numberParameter = number != null ?
                new ObjectParameter("Number", number) :
                new ObjectParameter("Number", typeof(string));
    
            var administratorParameter = administrator != null ?
                new ObjectParameter("Administrator", administrator) :
                new ObjectParameter("Administrator", typeof(string));
    
            var orderByParameter = orderBy != null ?
                new ObjectParameter("OrderBy", orderBy) :
                new ObjectParameter("OrderBy", typeof(string));
    
            var documentTypeIDParameter = documentTypeID.HasValue ?
                new ObjectParameter("DocumentTypeID", documentTypeID) :
                new ObjectParameter("DocumentTypeID", typeof(int));
    
            var pageParameter = page.HasValue ?
                new ObjectParameter("Page", page) :
                new ObjectParameter("Page", typeof(int));
    
            var itemsPerPageParameter = itemsPerPage.HasValue ?
                new ObjectParameter("ItemsPerPage", itemsPerPage) :
                new ObjectParameter("ItemsPerPage", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("GetDocuments", forUserIDParameter, documentIDParameter, nameParameter, numberParameter, administratorParameter, orderByParameter, documentTypeIDParameter, pageParameter, itemsPerPageParameter);
        }
    
        public virtual ObjectResult<GetDocuments1_Result> GetDocuments1(Nullable<int> forUserID, Nullable<int> documentID, string name, string number, string administrator, string orderBy, Nullable<int> documentTypeID, Nullable<int> page, Nullable<int> itemsPerPage)
        {
            var forUserIDParameter = forUserID.HasValue ?
                new ObjectParameter("forUserID", forUserID) :
                new ObjectParameter("forUserID", typeof(int));
    
            var documentIDParameter = documentID.HasValue ?
                new ObjectParameter("documentID", documentID) :
                new ObjectParameter("documentID", typeof(int));
    
            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));
    
            var numberParameter = number != null ?
                new ObjectParameter("Number", number) :
                new ObjectParameter("Number", typeof(string));
    
            var administratorParameter = administrator != null ?
                new ObjectParameter("Administrator", administrator) :
                new ObjectParameter("Administrator", typeof(string));
    
            var orderByParameter = orderBy != null ?
                new ObjectParameter("OrderBy", orderBy) :
                new ObjectParameter("OrderBy", typeof(string));
    
            var documentTypeIDParameter = documentTypeID.HasValue ?
                new ObjectParameter("DocumentTypeID", documentTypeID) :
                new ObjectParameter("DocumentTypeID", typeof(int));
    
            var pageParameter = page.HasValue ?
                new ObjectParameter("Page", page) :
                new ObjectParameter("Page", typeof(int));
    
            var itemsPerPageParameter = itemsPerPage.HasValue ?
                new ObjectParameter("ItemsPerPage", itemsPerPage) :
                new ObjectParameter("ItemsPerPage", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetDocuments1_Result>("GetDocuments1", forUserIDParameter, documentIDParameter, nameParameter, numberParameter, administratorParameter, orderByParameter, documentTypeIDParameter, pageParameter, itemsPerPageParameter);
        }
    }
}
