using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
   public class ClubInfo
    {
       private Int64 _ID = 0;
       private string _Name = "";
       private string _Description = "";
       private bool _IsActive = false;
       private string _CreationDate = "";

        public ClubInfo()
        { }

        public ClubInfo(Int64 _ID,string _Name,string _Description,bool _IsActive,string _CreationDate)
        {
            ID = _ID;
            Name = _Name;
            Description = _Description;
            IsActive = _IsActive;
            CreationDate = _CreationDate;
        }


        public Int64 ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        public bool IsActive
        {
            get { return _IsActive; }
            set { _IsActive = value; }
        }

        public string CreationDate
        {
            get { return _CreationDate; }
            set { _CreationDate = value; }
        }
    }
}
