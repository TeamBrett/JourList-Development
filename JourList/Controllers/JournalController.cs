using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JourList.Models;
using JourList.Tools;

namespace JourList.Controllers
{
    public class JournalController : Controller
    {
        //
        // GET: /Entry/

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated == false)
                return JsonError("You are not authenticated");

            JournalModel model = new JournalModel(User.Identity.Name,DateTime.Today);

            return View(model);
        }

        [HttpPost]
        public JsonResult GetEntry(string date = "", int id = -1)
        {
            if (User.Identity.IsAuthenticated == false)
                return JsonError("You are not authenticated");

            //JourListDMContainer dm = new JourListDMContainer();
            JournalModel model;
            if (date != string.Empty)
                model = new JournalModel(User.Identity.Name, DateTime.Parse(date));
            else if (id != -1)
                model = new JournalModel(User.Identity.Name, id);
            else
                model = new JournalModel(User.Identity.Name, DateTime.Today);

            return JsonOk(model);
        }

        #region Goals

        ////
        //// GET: /DataManagement/GoalDetails/5

        //public ActionResult GoalDetails(int jourId)
        //{
        //    return View();
        //}

        ////
        //// POST: /DataManagement/GoalList
        //[HttpPost]
        //public JsonResult GoalList()
        //{
        //    if (!User.Identity.IsAuthenticated)
        //    {
        //        return Json(new
        //        {
        //            Result = "ERROR",
        //            Message = "Please log in to perform this action! "
        //        });
        //    }

        //    try
        //    {
        //        JourListDMContainer dm = new JourListDMContainer();

        //        List<GoalModel> goals = new List<GoalModel>();
        //        if (dm.Members.Count() == 0)
        //        {
        //            return Json(new { Result = "OK", Records = goals });
        //        }

        //        foreach (var g in dm.Members.Single(z => z.Name == User.Identity.Name).Goals.Where(z => z.Active == true))
        //        {
        //            GoalModel gtemp = new GoalModel();
        //            gtemp.Id = g.Id;
        //            gtemp.ActivityId = g.Activity.Id;
        //            gtemp.BeginDate = g.DateBegin;
        //            gtemp.EndDate = g.DateEnd;
        //            gtemp.ReccurenceId = g.RecurrenceInterval.Id;
        //            gtemp.Active = g.Active;
        //            goals.Add(gtemp);
        //        }

        //        return Json(new { Result = "OK", Records = goals });
        //    }
        //    catch (Exception e)
        //    {
        //        return Json(new { Result = "ERROR", Message = e.Message });
        //    }

        //}

        ////
        //// POST: /DataManagement/CreateGoal
        //[HttpPost]
        //public JsonResult CreateGoal(GoalModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Json(new
        //        {
        //            Result = "ERROR",
        //            Message = "Form is not valid! " +
        //            "Please correct it and try again."
        //        });
        //    }

        //    if (!User.Identity.IsAuthenticated)
        //    {
        //        return Json(new
        //        {
        //            Result = "ERROR",
        //            Message = "Please log in to perform this action! "
        //        });
        //    }

        //    try
        //    {
        //        JourListDMContainer dm = new JourListDMContainer();

        //        Goal newGoal = new Goal();
        //        newGoal.Activity = dm.Activities.Single(z => z.Id == model.ActivityId);
        //        newGoal.RecurrenceInterval = dm.RecurrenceIntervals.Single(ri => ri.Id == model.ReccurenceId);
        //        newGoal.DateBegin = model.BeginDate;
        //        newGoal.DateEnd = model.EndDate;
        //        newGoal.Member = dm.Members.Single(z => z.Name == User.Identity.Name);
        //        dm.AddToGoals(newGoal);
        //        dm.SaveChanges();

        //        model.Id = newGoal.Id;

        //        return Json(new { Result = "OK", Record = model });
        //    }
        //    catch (Exception e)
        //    {
        //        return Json(new { Result = "ERROR", Message = e.Message });
        //    }
        //}

        ////
        //// POST: /DataManagement/UpdateGoal
        //[HttpPost]
        //public JsonResult UpdateGoal(GoalModel model)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return Json(new
        //            {
        //                Result = "ERROR",
        //                Message = "Form is not valid! " +
        //                  "Please correct it and try again."
        //            });
        //        }

        //        if (!User.Identity.IsAuthenticated)
        //        {
        //            return Json(new
        //            {
        //                Result = "ERROR",
        //                Message = "Please log in to perform this action! "
        //            });
        //        }

        //        JourListDMContainer dm = new JourListDMContainer();

        //        Goal goal = dm.Members.Single(z => z.Name == User.Identity.Name).Goals.Single(z => z.Id == model.Id);
        //        goal.Activity = dm.Activities.Single(z => z.Id == model.ActivityId);
        //        goal.Active = model.Active;
        //        goal.DateBegin = model.BeginDate;
        //        goal.DateEnd = model.EndDate;
        //        goal.RecurrenceInterval = dm.RecurrenceIntervals.Single(ri => ri.Id == model.ReccurenceId);

        //        dm.SaveChanges();

        //        return Json(new { Result = "OK" });
        //    }
        //    catch (Exception e)
        //    {
        //        return Json(new { Result = "ERROR", Message = e.Message });
        //    }
        //}

        ////
        //// POST: /DataManagement/DeleteGoal
        //[HttpPost]
        //public JsonResult DeleteGoal(int jourId)
        //{
        //    if (!User.Identity.IsAuthenticated)
        //    {
        //        return Json(new
        //        {
        //            Result = "ERROR",
        //            Message = "Please log in to perform this action! "
        //        });
        //    }

        //    try
        //    {
        //        JourListDMContainer dm = new JourListDMContainer();

        //        // Grab the goal
        //        Goal g = dm.Members.Single(z => z.Name == User.Identity.Name).Goals.Single(z => z.Id == jourId);

        //        // Delete the object only if there are no references
        //        if (g.Accomplishments.Count < 1)
        //            dm.Goals.DeleteObject(g);

        //        // Otherwise just deactivate so we don't break any historical records
        //        else
        //            g.Active = false;

        //        // Save the changes
        //        dm.SaveChanges();

        //        return Json(new { Result = "OK" });
        //    }
        //    catch (Exception e)
        //    {
        //        return Json(new { Result = "ERROR", Message = e.Message });
        //    }
        //}

        ////
        //// POST: /DataManagement/GetRecurrenceList
        //[HttpPost]
        //public JsonResult GetRecurrenceList()
        //{
        //    try
        //    {
        //        JourListDMContainer dm = new JourListDMContainer();
        //        var intervals = dm.RecurrenceIntervals.Select(z => new { DisplayText = z.Description, Value = z.Id });
        //        return Json(new { Result = "OK", Options = intervals });
        //    }
        //    catch (Exception e)
        //    {
        //        return Json(new { Result = "ERROR", Message = e.Message });
        //    }
        //}

        ////
        //// POST: /DataManagement/GetActivityOptions
        //[HttpPost]
        //public JsonResult GetGoalOptions()
        //{
        //    try
        //    {
        //        JourListDMContainer dm = new JourListDMContainer();
        //        var list = dm.Goals.Select(z => new { DisplayText = z.Activity.Description, Value = z.Id });
        //        return Json(new { Result = "OK", Options = list });
        //    }
        //    catch (Exception e)
        //    {
        //        return Json(new { Result = "ERROR", Message = e.Message });
        //    }
        //}

        #endregion

        public JsonResult GetActivity(long ActId)
        {
            if (User.Identity.IsAuthenticated == false)
                return JsonError("You are not authenticated");
            
            return JsonOk(this.RenderViewToString("ActivityLog", new ActivityLogModel(ActId)));
        }

        public JsonResult GetActivityLogs(string Day)
        {
            if (User.Identity.IsAuthenticated == false)
                return JsonError("You are not authenticated");
            DateTime date;
            if (Day == null)
                date = DateTime.Today;
            else
                date = DateTime.Parse(Day);

            var dm = new JourListDMContainer();
            var list = new List<ActivityLogModel>();
            var member = dm.Members.SingleOrDefault(z => z.Name == User.Identity.Name);
            
            if (member == null)
                return JsonError("You don't exist");
            
            var journal = member.Journals.SingleOrDefault(z => z.EntryDate == date);

            if (journal == null)
                return JsonOk("");

            foreach (var log in journal.ActivityLogs)
                list.Add(new ActivityLogModel(log.Id, User.Identity.Name));

            return JsonOk(list);
        }

        public JsonResult CreateActivityLog(ActivityLogModel model)
        {
            var dm = new JourListDMContainer();
            var log = new ActivityLog();
            var member = dm.Members.SingleOrDefault(z => z.Name == User.Identity.Name);
            if (member == null)
                return JsonError("You don't exist");
            
            var journal = member.Journals.Single(z => z.Id == model.JourId);
            if (journal == null)
                return JsonError("This journal doesn't belong to you");

            log.Activity = dm.Activities.Single(z => z.Id == model.ActId);
            log.Quantity = model.Quantity;
            // TODO: Hyperlink & Notes should be updated to default to an
            //       empty string in the databaseif the value are null, 
            //       the EM has been updated, but the database needs to 
            //       be updated to reflect the default value of "".
            log.Notes = model.Notes;
            log.Hyperlink = model.Hyperlink;
            log.Unit = dm.Units.Single(z => z.Id == model.UnitId);
            log.Journal = journal;
            dm.ActivityLogs.AddObject(log);
            dm.SaveChanges();
//            model.Points = log.Activity.Points;
//            model.TotalPoints = (int)Math.Round(log.Activity.Points * log.Quantity);
            model.LogId = log.Id;
//            return Json(model);
            return Json(new { Result = "OK", Record = model });
//            return JsonError("Not Done");
        }

        public JsonResult PopularActivities(string Day, short MaxResults = 20)
        {
            JourListDMContainer dm = new JourListDMContainer();
            List<ActivityModel> list = new List<ActivityModel>();
            List<ActivityLog> omit = new List<ActivityLog>(0);
            
            var member = dm.Members.SingleOrDefault(z => z.Name == User.Identity.Name);
            if (User.Identity.IsAuthenticated == false || member == null)
                return JsonError("You do not exist.  No activities for you!");

            var date = Day == null ? DateTime.Today : DateTime.Parse(Day);

            // Get a journal entry for this day if it exists
            var jour = member.Journals.SingleOrDefault(z => z.EntryDate == date);

            // Get the exisitng activity logs if the journal did exist
            if (jour != null)
                omit = jour.ActivityLogs.ToList();

            // Get the member's most popular activities
            var logs = from al in dm.ActivityLogs
                       where al.Journal.Member.Name == member.Name
                       group al by al.Activity.Id into j
                       orderby j.GroupBy(z => z.Activity.Id).Count() descending
                       select j;

            // If they haven't completed enough activities, get common popular activities
            if (logs.Count() - omit.Count() < MaxResults)
                logs.Concat(from al in dm.ActivityLogs
                            group al by al.Activity.Id into j
                            orderby j.GroupBy(z => z.Activity.Id).Count() descending
                            select j);
            
            // Add everything to the list            
            foreach (var group in logs)
            {
                var act = group.First().Activity;
                if (omit.Count(z => z.Activity.Id == act.Id) == 0)
                    list.Add(new ActivityModel(act));
                if (list.Count >= MaxResults)
                    break;
            }

            // Fill up any empty space with random activities
            foreach (var act in dm.Activities)
            {
                if (omit.Count(z => z.Activity.Id == act.Id) == 0)
                    list.Add(new ActivityModel(act));
                if (list.Count >= MaxResults)
                    break;
            }

            // Return
            return JsonOk(list.Distinct(new Tools.Tools.KeyEqualityComparer<ActivityModel>(z => z.Id)));
        }

        //
        // POST: /Entry/Save/5

        [HttpPost]
        public JsonResult Save(JournalModel model)
        {
            if (User.Identity.IsAuthenticated == false)
                return JsonError("You are not authenticated");
            JourListDMContainer dm = new JourListDMContainer();
            
            var member = dm.Members.SingleOrDefault(z => z.Name == User.Identity.Name);
            if(member == null)    
                return JsonError("You are not registered in the system");

            var journal = member.Journals.SingleOrDefault(z => z.Id == model.JourId);
            if (journal == null)
                return JsonError("You can't save a journal that doesn't exist. Or at least, technically you could by creating a new one... but that should have already been done.  If it hasn't already been done then you're working outside the system.");
            
            journal.Weight = model.Weight;
            journal.HeartRate = model.HeartRate;
            journal.Story = model.Story;
            journal.Encrypted = model.Encrypted;

            dm.SaveChanges();
            model.JourId = journal.Id;
            return JsonOk(model);
        }
        
        [HttpPost]
        public JsonResult UpdateActivityLog(ActivityLogModel model)
        {
            JourListDMContainer dm = new JourListDMContainer();
            if (User.Identity.IsAuthenticated != true)
                return JsonError("Please log in");

            var member = dm.Members.SingleOrDefault(z => z.Name == User.Identity.Name);
            if ( member == null)
                return JsonError("You do not exist");

            var log = dm.ActivityLogs.SingleOrDefault(z => z.Id == model.LogId);
            if ( log == null )
                return JsonError("That log does not exist");

            if (log.Journal.Member != member)
                return JsonError("This log does not belong to you");

            var unit = dm.Units.SingleOrDefault(z => z.Id == model.UnitId);
            if (unit == null)
                return JsonError("A unit by that ID does not exist");
            if (unit.UnitType != log.Activity.Unit.UnitType)
                return JsonError("Your unit is not the same type as the activity's");

            log.Unit = unit;
            log.Hyperlink = model.Hyperlink;
            log.Notes = model.Notes;
            log.Quantity = model.Quantity;

            dm.SaveChanges();

            return JsonOk(model);
        }

        [HttpPost]
        public JsonResult DeleteActivityLog(long LogId)
        {
            JourListDMContainer dm = new JourListDMContainer();

            if (User.Identity.IsAuthenticated != true)
                return JsonError("Please log in");

            var member = dm.Members.SingleOrDefault(z => z.Name == User.Identity.Name);
            if (member == null)
                return JsonError("You do not exist");

            var log = dm.ActivityLogs.SingleOrDefault(z => z.Id == LogId);
            if (log == null)
                return JsonError("That log does not exist");

            if (log.Journal.Member != member)
                return JsonError("This log does not belong to you");

            dm.ActivityLogs.DeleteObject(log);

            dm.SaveChanges();

            return JsonOk("Deleted");
        }


        private JsonResult JsonError(string message)
        {
            return Json(new
            {
                Result = "ERROR",
                Message = message
            });
        }

        private JsonResult JsonOk(object records)
        {
            return Json(new { Result = "OK", Records = records });
        }

    }
}
