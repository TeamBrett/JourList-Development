using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using JourList.Tools;

namespace JourList.Models
{
    public class JournalModel
    {
        public long JourId { get; set; }
        
        [Display(Name = @"Story")]
        public string Story { get; set; }

        [Display(Name = @"Weight")]
        public short Weight { get; set; }

        [Display(Name = @"Encrypted")]
        public bool Encrypted { get; set; }

        [Display(Name = @"Date")]
        private DateTime entrydate;
        public DateTime dtEntryDate
        {
            get { return entrydate; }
            set { entrydate = value; }
        }
        
        public String EntryDate 
        {
            get { return entrydate.ToString("MM/dd/yyyy"); }
            set { entrydate = DateTime.Parse(value); } 
        }

        [Display(Name = @"HeartRate")]
        public short HeartRate { get; set; }

        private JourListDMContainer dm;
        private Member __member;
        public JournalModel()
        {
            this.dtEntryDate = DateTime.Today;
            this.Story = " ";
        }

        // Journal for a specific day
        public JournalModel(string member, DateTime date) : this()
        {
            setup(member);
            var journal = __member.Journals.SingleOrDefault(z => z.EntryDate == date);
            if (journal == null)
            {
                journal = new Journal();
                journal.EntryDate = date;
                journal.Member = __member;
                dm.Journals.AddObject(journal);
                dm.SaveChanges();
            }
            this.set(journal);
        }

        /// <summary>
        /// Retrieves or creates a journal for the member for "today"
        /// </summary>
        /// <param name="member">Member's Username</param>
        public JournalModel(string member) : this()
        {
            setup(member); 
            var journal = __member.Journals.SingleOrDefault(z => z.EntryDate == DateTime.Today);
            if (journal == null)
            {
                journal = new Journal();
                journal.EntryDate = DateTime.Now;
                journal.Member = __member;
                dm.Journals.AddObject(journal);
                dm.SaveChanges();
            }
            this.set(journal);
        }

        // Create journal by ID
        public JournalModel(string member, long jourId)
        {
            setup(member);
         
            var journal = __member.Journals.SingleOrDefault(z => z.Id == jourId);
            if(journal == null)
                throw new Exception("Could not find a journal by that ID");
            this.set(journal);
        }
        
        // Create repository, find user
        private void setup(string member)
        {
            dm = new JourListDMContainer();

            __member = dm.Members.SingleOrDefault(z => z.Name == member);
            if (__member == null)
                throw new Exception("You are not registered in the system");
        }

        // Do the actual value copying
        private void set(Journal journal)
        {
            this.dtEntryDate = journal.EntryDate;
            this.JourId = journal.Id;
            this.Weight = journal.Weight;
            this.HeartRate = journal.HeartRate;
            this.Story = journal.Story;
            this.Encrypted = journal.Encrypted;
        }
    }

    public class ActivityLogModel
    {
        public long LogId { get; set; }
        public long ActId { get; set; }
        public long JourId { get; set; }
        public string Description { get; set; }
        public double Quantity { get; set; }
        //public int Points { private get; 
        //    set
        //    {
   
        //    } 
        //}
        public int TotalPoints 
        { 
            get 
            {
                try
                {
                    JourListDMContainer dm = new JourListDMContainer();
                    var act = dm.Activities.Single(z => z.Id == this.ActId);
                    
                    var thisUnit = dm.Units.Single(z => z.Id == this.unitid);
                    // Convert activity to standard units
                    var astandard = Tools.Tools.ConvertUnits(act.Quantity, act.Unit, act.Unit.UnitType.DefaultUnit);
                    // Convert log quantity to standard units
                    var lstandard = Tools.Tools.ConvertUnits(this.Quantity, thisUnit, act.Unit.UnitType.DefaultUnit);

                    return (int)Math.Round(lstandard / astandard * act.Points);
                }
                catch (Exception)
                {
                    return 0;
                }
            }  
        }
//        public int TotalPoints { get; set; }
        private int unitid;

        public SelectList UnitList { get; set; }

        public int UnitId 
        { 
            get { return unitid; } 
            set { unitid = value; } 
        }
        public string sUnitId
        {
            get { return unitid.ToString(); }
            set 
            {
                if (int.TryParse(value, out unitid) == false)
                    throw new Exception("Unit ID is not parse-able");
            }
        }

        private string __hyperlink;
        public string Hyperlink
        {
            get { return __hyperlink ?? ""; }
            set { __hyperlink = value; }
        }

        private string __notes;

        public string Notes
        {
            get { return __notes ?? ""; }
            set { __notes = value; }
        }

        public ActivityLogModel() 
        {
        }

        public ActivityLogModel(long LogId, string memberName)
        {
            JourListDMContainer dm = new JourListDMContainer();
            var mem = dm.Members.SingleOrDefault(z => z.Name == memberName);
            var log = dm.ActivityLogs.Single(z => z.Id == LogId);
            if (log.Journal.Member.Name != memberName)
                throw new Exception("Log does not belong to user");
            set(log, log.Activity);
        }

        public ActivityLogModel(long ActId)
        {
            JourListDMContainer dm = new JourListDMContainer();
            var act = dm.Activities.Single(z => z.Id == ActId);

            set(null, act);

            // TODO: Complete member initialization
            this.Quantity = 0;
            this.unitid = act.Unit.Id;
        }
        private void set(ActivityLog log, Activity act)
        {
            if (log != null)
            {
                this.LogId = log.Id;
                this.Quantity = log.Quantity;
                this.unitid = log.Unit.Id;
                this.UnitList = new SelectList(log.Unit.UnitType.Units, "Id", "Description", this.sUnitId);
                this.Hyperlink = log.Hyperlink;
                this.Notes = log.Notes;
                this.Description = log.Activity.Description;
                this.JourId = log.Journal.Id;
 //               this.TotalPoints = (int)(act.Points * log.Quantity);
            }
            else
            {
                this.unitid = act.Unit.Id;
                this.UnitList = new SelectList(act.Unit.UnitType.Units, "Id", "Description", this.sUnitId);
            }
            this.ActId = act.Id;
            this.Description = act.Description;
//            this.Points = act.Points;

        }
    }

    public struct GoalModel
    {
        public long Id { get; set; }

        [Required]
        [Display(Name = @"Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = @"Begin Date")]
        public DateTime BeginDate { get; set; }

        [Required]
        [Display(Name = @"End Date")]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = @"Recurrence")]
        public int ReccurenceId { get; set; }

        [Required]
        [Display(Name = @"Activity")]
        public long ActivityId { get; set; }

        [Display(Name = @"Active?")]
        public bool Active { get; set; }
    }

}