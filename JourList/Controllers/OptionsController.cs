using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JourList.Models;

namespace JourList.Controllers
{
    public class OptionsController : Controller
    {
        //
        // POST: /Options/Items
        [HttpPost]
        public JsonResult StandardItems()
        {
            try
            {
                JourListDMContainer dm = new JourListDMContainer();
                var list = dm.Items.OfType<PersonalItem>().Select(z => new { DisplayText = z.Description, Value = z.Id })
                                   .OrderByDescending(z => z.DisplayText);
                return Json(new { Result = "OK", Options = list });
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = e.Message });
            }
        }

        //
        // POST: /Options/Items
        [HttpPost]
        public JsonResult InventoryItems()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new
                {
                    Result = "ERROR",
                    Message = "You need to log in to see your items."
                });
            }

            try
            {
                JourListDMContainer dm = new JourListDMContainer();
                var list = dm.Members.SingleOrDefault(z=>z.Name == User.Identity.Name)
                                     .Inventories.Select(z => new { DisplayText = z.Item.Description, Value = z.Item.Id })
                                                 .OrderByDescending(z => z.DisplayText);
                return Json(new { Result = "OK", Options = list });
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = e.Message });
            }
        }

        //
        // POST: /Options/Activities
        [HttpPost]
        public JsonResult Activities()
        {
            try
            {
                JourListDMContainer dm = new JourListDMContainer();
                var list = dm.Activities.Select(z => new { DisplayText = z.Description, Value = z.Id });
                return Json(new { Result = "OK", Options = list });
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = e.Message });
            }
        }

        //
        // POST: /Options/Units
        [HttpPost]
        public JsonResult Units()
        {
            try
            {
                JourListDMContainer dm = new JourListDMContainer();
                var list = dm.Units.Select(z => new { DisplayText = z.Description, Value = z.Id });
                return Json(new { Result = "OK", Options = list });
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = e.Message });
            }
        }

        //
        // POST: /Options/UnitTypes
        [HttpPost]
        public JsonResult UnitTypes()
        {
            try
            {
                JourListDMContainer dm = new JourListDMContainer();
                var list = dm.UnitTypes.Select(z => new { DisplayText = z.Description, Value = z.Id });
                return Json(new { Result = "OK", Options = list });
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = e.Message });
            }
        }

        //
        // POST: /Options/ItemCategories
        [HttpPost]
        public JsonResult ItemCategories()
        {
            try
            {
                JourListDMContainer dm = new JourListDMContainer();
                var list = dm.ItemCategories.Select(z => new { DisplayText = z.Description, Value = z.Id });
                return Json(new { Result = "OK", Options = list });
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = e.Message });
            }
        }

        //
        // POST: /Options/RecurrenceIntervals
        [HttpPost]
        public JsonResult RecurrenceIntervals()
        {
            try
            {
                JourListDMContainer dm = new JourListDMContainer();
                var list = dm.RecurrenceIntervals.Select(z => new { DisplayText = z.Description, Value = z.Id });
                return Json(new { Result = "OK", Options = list });
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = e.Message });
            }
        }

        //
        // POST: /Options/InventoryActions
        [HttpPost]
        public JsonResult InventoryActions()
        {
            try
            {
                JourListDMContainer dm = new JourListDMContainer();
                var list = dm.InventoryActions.Select(z => new { DisplayText = z.Description, Value = z.Id });
                return Json(new { Result = "OK", Options = list });
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = e.Message });
            }
        }
        
    }
}
