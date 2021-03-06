﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCPack.Model
{
    public class Filter
    {
        public int Page { get; set; }
        public int ItemsPerPage { get; set; }
        public string OrderBy { get; set; }
    }

    public class LogEventFilter : Filter
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int UserId { get; set; }
        public int ResourceId { get; set; }
        public int LogType { get; set; }
    }

    public class CodeListFilter : Filter
    {
        public int ItemID { get; set; }
    }

    public class UserFilter : Filter 
    {
        public UserFilter()
        {
            this.ExcludedUsersId = new int[] { };
            this.JobPositionIDs = new HashSet<int>();
            this.UserIDs = new HashSet<int>();
        }

        public ICollection<int> UserIDs { get; set; }
        public ICollection<int> JobPositionIDs { get; set; }
        public string Name { get; set; }
        public int[] ExcludedUsersId { get; set; }
    }



    public class DocumentFilter : Filter
    {
        public int? ForUserID { get; set; }
        public int? DocumentID { get; set; }
        public string Name { get; set; }
        public string AdministratorName { get; set; }
        public string Number { get; set; }
        public int? DocumentTypeID { get; set; }
        public int? ProjectID { get; set; }
        public int? DivisionID { get; set; }
        public int? AppSystemID { get; set; }
        public int? WorkplaceID { get; set; }
        public bool Reset { get; set; }
        public DateTime? EffeciencyDateFrom { get; set; }
        public DateTime? EffeciencyDateTo { get; set; }

        public DateTime? NextReviewDateFrom { get; set; }
        public DateTime? NextReviewDateTo { get; set; }

        public string ReadType { get; set; }
        public int? StateID { get; set; }
        public string StateCode { get; set; }  // 25.10.2017 LF 

        public string Revision { get; set; }

        public string ReviewNecessaryChange { get; set; }

        // LF 7.11.2017 - Main ID ktere je spolecne pro vsechny vydani 
        public int? MainID { get; set; }

        // LF 7.11.2017 kdyz bude false tak se filter nebude ukladat
        // true je default, false nastavujeme pri volani getDocuments ve view\Document\Edit a Detail ve js funkci GetDocuments_edit (gc.js)

        private bool saveFilter=true;
        public bool SaveFilter
        {
            get { return saveFilter; }
            set { saveFilter = value; }
        }


        public int? AdministratorID { get; set; }
        public int? AuthorID { get; set; }
        public int? UserID { get; set; }

    }

    public class ReadConfirmFilter : Filter
    {
        public int? DocumentID { get; set; }

        public ICollection<int> UserIDs { get; set; }

        public ICollection<int> JobPositionIDs { get; set; }

    }

}
