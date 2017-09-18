﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace GCPack.Model
{
    public enum DocumentAdminType
    {
        FromDocumentType,
        User
    }

    // LF doplneno kvuli moznosti filtrace  dokumentu ozncenych priznakem Archived
    public enum DocumentArchived
    {
        Archived = 1
    }

    public class FileItem
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public string Size { get; set; }
        public byte[] Data { get; set; }
    }

    public class DocumentModel
    {
        public DocumentModel()
        {
            FileItems = new HashSet<FileItem>();
        }

        public int ID { get; set; }
        [Required(ErrorMessage = "Musí být vyplněn název dokumentu.")]
        public string Title { get; set; }
        public string Annotation { get; set; }
        public DocumentAdminType DocumentAdminType { get; set; }
        public bool CanEdit { get; set; }
        public bool CanRevision { get; set; }
        public bool CanConfirmRead { get; set; }
        public ICollection<int> SelectedUsers { get; set; }
        public int DocumentTypeID { get; set; }
        public string DocumentNumber { get; set; }
        public string DocumentAdministrator { get; set; }
        public string DocumentStateName { get; set; }
        public string DocumentStateCode { get; set; }
        public string Revision { get; set; }
        public int StateID { get; set; }
        public int AdministratorID { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> EffeciencyDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> ReviewDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> NextReviewDate { get; set; }
        public ICollection<FileItem> FileItems { get; set; }
        public string DeleteFileItems { get; set; } // soubory ktere smazeme
        public ICollection<UserModel> Users { get; set; }
        public DateTime? ReadDate { get; set; } // datum seznameni s dokumentem
        public ICollection<int> JobPositionIDs { get; set; }
        public ICollection<int> SelectedProjectsID { get; set; } // všechny vybrané projekty
        public ICollection<int> SelectedDivisionsID { get; set; } // všechny vybrané střediska
        public ICollection<int> SelectedAppSystemsID { get; set; } // všechny vybrané systémy
        public ICollection<int> SelectedWorkplacesID { get; set; } // všechny vybrané pracovní místa

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> EndDate { get; set; } // opravil na nullabale 11.9.2017 LF
        public bool Archived { get; set; } // 0 - neni v archivu, 1 je v archivu
        public UsersInDocument UsersInDocument { get; set; } // seznam vsech uzivatelu prirazenych k dokumentu a seznam vsech co 
        // si dokument precetli
        public int UsersRead { get; set; }

        public int AllUsers { get; set; }

        public int IssueNumber { get; set; } // číslo vydání
        public int ParentID { get; set; }   // odkaz na předchozího 

        public bool ReviewNecessaryChange { get; set; } // 1 je nutna zmena
    }








}
