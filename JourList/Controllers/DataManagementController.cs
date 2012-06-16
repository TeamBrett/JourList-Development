using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JourList.Models;

namespace JourList.Controllers
{
    public class DataManagementController : Controller
    {
        //
        // GET: /DataManagement/

        public ActionResult Index()
        {
            return View();
        }

        #region Activities

        //
        // GET: /DataManagement/ActivityDetails/5
        public ActionResult ActivityDetails(int id)
        {
            return View();
        }
        
        //
        // POST: /DataManagement/ActivityList
        [HttpPost]
        public JsonResult ActivityList()
        {
            try
            {
                JourListDMContainer dm = new JourListDMContainer();
                List<ActivityModel> list = new List<ActivityModel>();

                foreach (var a in dm.Activities)//.Where(z => z.Active == true))
                {
                    ActivityModel atemp = new ActivityModel();
                    atemp.Id = a.Id;
                    atemp.Description = a.Description;
                    atemp.Points = a.Points;
                    atemp.Active = a.Active;
                    atemp.UnitId = a.Unit.Id;
                    atemp.Quantity = a.Quantity;
                    list.Add(atemp);
                }
                return Json(new { Result = "OK", Records = list });
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = e.Message });
            }
        }

        //
        // POST: /DataManagement/CreateActivity
        [HttpPost]
        public JsonResult CreateActivity(ActivityModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    Result = "ERROR",
                    Message = "Form is not valid! " +
                    "Please correct it and try again."
                });
            }

            try
            {
                JourListDMContainer dm = new JourListDMContainer();

                Activity newActivity = new Activity();
                newActivity.Points = model.Points;
                newActivity.Description = model.Description;
                newActivity.Unit = dm.Units.Single(z => z.Id == model.UnitId);
                newActivity.Quantity = model.Quantity;
                dm.AddToActivities(newActivity);
                dm.SaveChanges();

                model.Id = newActivity.Id;

                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = e.Message });
            }
        }
        
        //
        // POST: /DataManagement/UpdateActivity
        [HttpPost]
        public JsonResult UpdateActivity(ActivityModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "Form is not valid! " +
                          "Please correct it and try again."
                    });
                }

                JourListDMContainer dm = new JourListDMContainer();

                Activity activity = dm.Activities.Single(z => z.Id == model.Id);
                activity.Description = model.Description;
                activity.Points = model.Points;
                activity.Unit = dm.Units.Single(z => z.Id == model.UnitId);
                activity.Quantity = model.Quantity;
                dm.SaveChanges();

                return Json(new { Result = "OK" });
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = e.Message });
            }
        }

        //
        // POST: /DataManagement/DeleteActivity
        [HttpPost]
        public JsonResult DeleteActivity(int id)
        {
            try
            {
                JourListDMContainer dm = new JourListDMContainer();

                // Grab the goal
                Activity a = dm.Activities.Single(z => z.Id == id);

                // Delete the object only if there are no references
                if (a.ActivityLogs.Count < 1)
                    dm.Activities.DeleteObject(a);

                // Otherwise just deactivate so we don't break any historical records
                else
                    a.Active = false;

                // Save the changes
                dm.SaveChanges();

                return Json(new { Result = "OK" });
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = e.Message });
            }
        }

        ////
        //// POST: /DataManagement/GetActivityOptions
        //[HttpPost]
        //public JsonResult GetActivityOptions()
        //{
        //    try
        //    {
        //        JourListDMContainer dm = new JourListDMContainer();
        //        var list = dm.Activities.Select(z => new { DisplayText = z.Description, Value = z.Id });
        //        return Json(new { Result = "OK", Options = list });
        //    }
        //    catch (Exception e)
        //    {
        //        return Json(new { Result = "ERROR", Message = e.Message });
        //    }
        //}
    
        #endregion

        #region Unit

        // POST: /DataManagement/UnitList
        public JsonResult UnitList()
        {
            try
            {
                JourListDMContainer dm = new JourListDMContainer();
                List<UnitModel> list = new List<UnitModel>();

                foreach (var a in dm.Units.Where(z => z.Active == true))
                {
                    UnitModel item = new UnitModel();
                    item.Id = a.Id;
                    item.Description = a.Description;
                    item.ConversionFactor = a.ConversionFactor;
                    item.UnitTypeId = a.UnitType.Id;
                    item.Abbreviation = a.Abbreviation;
                    item.Active = a.Active;
                    list.Add(item);
                }

                return Json(new { Result = "OK", Records = list });
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = e.Message });
            }
        }
        //
        // POST: /DataManagement/CreateUnit
        [HttpPost]
        public JsonResult CreateUnit(UnitModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    Result = "ERROR",
                    Message = "Form is not valid! " +
                    "Please correct it and try again."
                });
            }

            if (!User.IsInRole("Officer"))
            {
                return Json(new
                {
                    Result = "ERROR",
                    Message = "You do not have the authority to create a new unit."
                });
            }

            try
            {
                JourListDMContainer dm = new JourListDMContainer();

                Unit unit = new Unit();
                
                unit.UnitType = dm.UnitTypes.Single(z => z.Id == model.UnitTypeId);
                unit.Description = model.Description;
                unit.ConversionFactor = model.ConversionFactor;
                unit.Abbreviation = model.Abbreviation;

                if (model.ConversionFactor == 1)
                {
                    if (unit.UnitType.DefaultUnit != null && unit.UnitType.DefaultUnit.Active)
                        return Json(new
                        {
                            Result = "ERROR",
                            Message = "A default unit with conversion factor of 1 already exists."
                        });

                    dm.AddToUnits(unit);
                    dm.SaveChanges();
                    
                    unit.UnitType.DefaultUnit = unit;
                }
                else
                {
                    dm.AddToUnits(unit);
                }
                
                dm.SaveChanges();

                model.Id = unit.Id;

                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = e.Message });
            }
        }

        //
        // POST: /DataManagement/UpdateUnit
        [HttpPost]
        public JsonResult UpdateUnit(UnitModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "Form is not valid! " +
                          "Please correct it and try again."
                    });
                }

                if (!User.IsInRole("Officer"))
                {
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "You do not have the authority to update this unit."
                    });
                }

                JourListDMContainer dm = new JourListDMContainer();

                Unit unit = dm.Units.Single(z => z.Id == model.Id);
                unit.Description = model.Description;
                unit.Abbreviation = model.Abbreviation;
                unit.ConversionFactor = model.ConversionFactor;
                unit.UnitType = dm.UnitTypes.Single(z => z.Id == model.UnitTypeId);

                if (model.ConversionFactor == 1 && unit.UnitType.DefaultUnit != null && unit.UnitType.DefaultUnit.Active)
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "An active default unit with conversion factor of 1 already exists."
                    });
                else if ( model.ConversionFactor == 1)
                    unit.UnitType.DefaultUnit = unit;

                unit.Active = model.Active;
                
                dm.SaveChanges();

                return Json(new { Result = "OK" });
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = e.Message });
            }
        }

        //
        // POST: /DataManagement/DeleteUnit
        [HttpPost]
        public JsonResult DeleteUnit(int id)
        {
            if (!User.IsInRole("Officer"))
            {
                return Json(new
                {
                    Result = "ERROR",
                    Message = "You do not have the authority to delete this unit."
                });
            }

            try
            {
                JourListDMContainer dm = new JourListDMContainer();

                // Grab the goal
                Unit u = dm.Units.Single(z => z.Id == id);

                if (u.UnitType.DefaultUnit == u)
                {
                    u.UnitType.DefaultUnit = null;
                }

                // Delete the object only if there are no references
                if (u.ActivityLogs.Count < 1 &&  u.UnitType.DefaultUnit != u && u.Inventories.Count < 1)
                    dm.Units.DeleteObject(u);

                // Otherwise just deactivate so we don't break any historical records
                else
                    u.Active = false;

                // Save the changes
                dm.SaveChanges();

                return Json(new { Result = "OK" });
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = e.Message });
            }
        }

        ////
        //// POST: /DataManagement/GetUnitOptions
        //[HttpPost]
        //public JsonResult GetUnitOptions()
        //{
        //    try
        //    {
        //        JourListDMContainer dm = new JourListDMContainer();
        //        var list = dm.Units.Select(z => new { DisplayText = z.Description, Value = z.Id });
        //        return Json(new { Result = "OK", Options = list });
        //    }
        //    catch (Exception e)
        //    {
        //        return Json(new { Result = "ERROR", Message = e.Message });
        //    }
        //}
        
        #endregion

        #region UnitType

        // POST: /DataManagement/UnitTypeList
        public JsonResult UnitTypeList()
        {
            try
            {
                JourListDMContainer dm = new JourListDMContainer();
                List<UnitTypeModel> list = new List<UnitTypeModel>();

                foreach (var a in dm.UnitTypes.Where(z => z.Active == true))
                {
                    UnitTypeModel item = new UnitTypeModel();
                    item.Id = a.Id;
                    item.Description = a.Description;
                    item.Active = a.Active;
                    list.Add(item);
                }

                return Json(new { Result = "OK", Records = list });
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = e.Message });
            }
        }

        //
        // POST: /DataManagement/CreateTypeUnit
        [HttpPost]
        public JsonResult CreateUnitType(UnitTypeModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    Result = "ERROR",
                    Message = "Form is not valid! " +
                    "Please correct it and try again."
                });
            }

            if (!User.IsInRole("Officer"))
            {
                return Json(new
                {
                    Result = "ERROR",
                    Message = "You do not have the authority to create a new unit type."
                });
            }

            try
            {
                JourListDMContainer dm = new JourListDMContainer();

                UnitType newUnitType = new UnitType();
                newUnitType.Description = model.Description;
                dm.AddToUnitTypes(newUnitType);
                dm.SaveChanges();

                model.Id = newUnitType.Id;

                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = e.Message });
            }
        }

        //
        // POST: /DataManagement/UpdateUnitType
        [HttpPost]
        public JsonResult UpdateUnitType(UnitTypeModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "Form is not valid! " +
                          "Please correct it and try again."
                    });
                }

                if (!User.IsInRole("Officer"))
                {
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "You do not have the authority to update this unit type."
                    });
                }

                JourListDMContainer dm = new JourListDMContainer();

                UnitType item = dm.UnitTypes.Single(z => z.Id == model.Id);
                item.Description = model.Description;
                item.Active = model.Active;
                dm.SaveChanges();

                return Json(new { Result = "OK" });
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = e.Message });
            }
        }

        //
        // POST: /DataManagement/DeleteUnitType
        [HttpPost]
        public JsonResult DeleteTypeUnit(int id)
        {
            if (!User.IsInRole("Officer"))
            {
                return Json(new
                {
                    Result = "ERROR",
                    Message = "You do not have the authority to delete this unit type."
                });
            }

            try
            {
                JourListDMContainer dm = new JourListDMContainer();

                // Grab the unit object
                UnitType t = dm.UnitTypes.Single(z => z.Id == id);

                // Delete the object only if there are no references
                if (t.Units.Count < 1 )
                    dm.UnitTypes.DeleteObject(t);

                // Otherwise just deactivate so we don't break any historical records
                else
                    t.Active = false;

                // Save the changes
                dm.SaveChanges();

                return Json(new { Result = "OK" });
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = e.Message });
            }
        }

        ////
        //// POST: /DataManagement/GetUnitTypeOptions
        //[HttpPost]
        //public JsonResult GetUnitTypeOptions()
        //{
        //    try
        //    {
        //        JourListDMContainer dm = new JourListDMContainer();
        //        var list = dm.UnitTypes.Select(z => new { DisplayText = z.Description, Value = z.Id });
        //        return Json(new { Result = "OK", Options = list });
        //    }
        //    catch (Exception e)
        //    {
        //        return Json(new { Result = "ERROR", Message = e.Message });
        //    }
        //}
        
        #endregion

        #region ItemCategory

        // POST: /DataManagement/ItemCategoryList
        public JsonResult ItemCategoryList()
        {
            try
            {
                JourListDMContainer dm = new JourListDMContainer();
                List<ItemCategoryModel> list = new List<ItemCategoryModel>();

                foreach (var a in dm.ItemCategories.Where(z => z.Active == true))
                {
                    ItemCategoryModel item = new ItemCategoryModel();
                    item.Id = a.Id;
                    item.Description = a.Description;
                    item.Active = a.Active;
                    list.Add(item);
                }

                return Json(new { Result = "OK", Records = list });
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = e.Message });
            }
        }

        //
        // POST: /DataManagement/CreateItemCategory
        [HttpPost]
        public JsonResult CreateItemCategory(ItemCategoryModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    Result = "ERROR",
                    Message = "Form is not valid! " +
                    "Please correct it and try again."
                });
            }

            if (!User.IsInRole("Officer"))
            {
                return Json(new
                {
                    Result = "ERROR",
                    Message = "You do not have the authority to create a new item category."
                });
            }

            try
            {
                JourListDMContainer dm = new JourListDMContainer();

                ItemCategory item = new ItemCategory();
                item.Description = model.Description;
                dm.AddToItemCategories(item);
                dm.SaveChanges();

                model.Id = item.Id;

                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = e.Message });
            }
        }

        //
        // POST: /DataManagement/UpdateItemCategory
        [HttpPost]
        public JsonResult UpdateItemCategory(ItemCategoryModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "Form is not valid! " +
                          "Please correct it and try again."
                    });
                }

                if (!User.IsInRole("Officer"))
                {
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "You do not have the authority to update this item category."
                    });
                }

                JourListDMContainer dm = new JourListDMContainer();

                ItemCategory item = dm.ItemCategories.Single(z => z.Id == model.Id);
                item.Description = model.Description;
                item.Active = model.Active;
                dm.SaveChanges();

                return Json(new { Result = "OK" });
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = e.Message });
            }
        }

        //
        // POST: /DataManagement/DeleteItemCategory
        [HttpPost]
        public JsonResult DeleteItemCategory(int id)
        {
            if (!User.IsInRole("Officer"))
            {
                return Json(new
                {
                    Result = "ERROR",
                    Message = "You do not have the authority delete this item category."
                });
            }

            try
            {
                JourListDMContainer dm = new JourListDMContainer();

                // Grab the item
                ItemCategory cat = dm.ItemCategories.Single(z => z.Id == id);

                // Delete the object only if there are no references
                if (cat.Items.Count < 1)
                    dm.ItemCategories.DeleteObject(cat);

                // Otherwise just deactivate so we don't break any historical records
                else
                    cat.Active = false;

                // Save the changes
                dm.SaveChanges();

                return Json(new { Result = "OK" });
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = e.Message });
            }
        }

        ////
        //// POST: /DataManagement/GetItemCategoryOptions
        //[HttpPost]
        //public JsonResult GetItemCategoryOptions()
        //{
        //    try
        //    {
        //        JourListDMContainer dm = new JourListDMContainer();
        //        var list = dm.ItemCategories.Select(z => new { DisplayText = z.Description, Value = z.Id });
        //        return Json(new { Result = "OK", Options = list });
        //    }
        //    catch (Exception e)
        //    {
        //        return Json(new { Result = "ERROR", Message = e.Message });
        //    }
        //}

        #endregion

        #region ReccurenceIntervals

        // POST: /DataManagement/RecurrenceIntervalList
        public JsonResult RecurrenceIntervalList()
        {
            try
            {
                JourListDMContainer dm = new JourListDMContainer();
                List<RecurrenceIntervalModel> list = new List<RecurrenceIntervalModel>();

                foreach (var a in dm.RecurrenceIntervals.Where(z => z.Active == true))
                {
                    RecurrenceIntervalModel item = new RecurrenceIntervalModel();
                    item.Id = a.Id;
                    item.Description = a.Description;
                    item.Minutes = a.Minutes;
                    item.Active = a.Active;
                    list.Add(item);
                }

                return Json(new { Result = "OK", Records = list });
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = e.Message });
            }
        }

        //
        // POST: /DataManagement/CreateRecurrenceInterval
        [HttpPost]
        public JsonResult CreateReccurenceInterval(RecurrenceIntervalModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    Result = "ERROR",
                    Message = "Form is not valid! " +
                    "Please correct it and try again."
                });
            }

            if (!User.IsInRole("Officer"))
            {
                return Json(new
                {
                    Result = "ERROR",
                    Message = "You do not have the authority to create a new interval."
                });
            }

            try
            {
                JourListDMContainer dm = new JourListDMContainer();

                RecurrenceInterval item = new RecurrenceInterval();
                item.Description = model.Description;
                item.Minutes = model.Minutes;
                dm.AddToRecurrenceIntervals(item);
                dm.SaveChanges();

                model.Id = item.Id;

                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = e.Message });
            }
        }

        //
        // POST: /DataManagement/UpdateRecurrenceInterval
        [HttpPost]
        public JsonResult UpdateRecurrenceInterval(RecurrenceIntervalModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "Form is not valid! " +
                          "Please correct it and try again."
                    });
                }

                if (!User.IsInRole("Officer"))
                {
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "You do not have the authority to update this interval."
                    });
                }

                JourListDMContainer dm = new JourListDMContainer();

                RecurrenceInterval item = dm.RecurrenceIntervals.Single(z => z.Id == model.Id);
                item.Description = model.Description;
                item.Minutes = model.Minutes;
                item.Active = model.Active;
                dm.SaveChanges();

                return Json(new { Result = "OK" });
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = e.Message });
            }
        }

        //
        // POST: /DataManagement/DeleteRecurrenceInterval
        [HttpPost]
        public JsonResult DeleteRecurrenceInterval(int id)
        {
            if (!User.IsInRole("Officer"))
            {
                return Json(new
                {
                    Result = "ERROR",
                    Message = "You do not have the authority delete this interval."
                });
            }

            try
            {
                JourListDMContainer dm = new JourListDMContainer();

                // Grab the item
                RecurrenceInterval item = dm.RecurrenceIntervals.Single(z => z.Id == id);

                // Delete the object only if there are no references
                if (item.Goals.Count < 1)
                    dm.RecurrenceIntervals.DeleteObject(item);

                // Otherwise just deactivate so we don't break any historical records
                else
                    item.Active = false;

                // Save the changes
                dm.SaveChanges();

                return Json(new { Result = "OK" });
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = e.Message });
            }
        }

        ////
        //// POST: /DataManagement/RecurrenceIntervalOptions
        //[HttpPost]
        //public JsonResult GetRecurrenceIntervalOptions()
        //{
        //    try
        //    {
        //        JourListDMContainer dm = new JourListDMContainer();
        //        var list = dm.RecurrenceIntervals.Select(z => new { DisplayText = z.Description, Value = z.Id });
        //        return Json(new { Result = "OK", Options = list });
        //    }
        //    catch (Exception e)
        //    {
        //        return Json(new { Result = "ERROR", Message = e.Message });
        //    }
        //}

        #endregion

        #region Inventory Actions

        // POST: /DataManagement/InventoryActionList
        public JsonResult InventoryActionList()
        {
            try
            {
                JourListDMContainer dm = new JourListDMContainer();
                List<InventoryActionModel> list = new List<InventoryActionModel>();

                foreach (var a in dm.InventoryActions)
                {
                    InventoryActionModel item = new InventoryActionModel();
                    item.Id = a.Id;
                    item.Description = a.Description;
                    list.Add(item);
                }

                return Json(new { Result = "OK", Records = list });
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = e.Message });
            }
        }

        //
        // POST: /DataManagement/CreateInventoryAction
        [HttpPost]
        public JsonResult CreateInventoryAction(InventoryActionModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    Result = "ERROR",
                    Message = "Form is not valid! " +
                    "Please correct it and try again."
                });
            }

            if (!User.IsInRole("Officer"))
            {
                return Json(new
                {
                    Result = "ERROR",
                    Message = "You do not have the authority to create a new inventory action."
                });
            }

            try
            {
                JourListDMContainer dm = new JourListDMContainer();

                InventoryAction item = new InventoryAction();
                item.Description = model.Description;
                dm.AddToInventoryActions(item);
                dm.SaveChanges();

                model.Id = item.Id;

                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = e.Message });
            }
        }

        //
        // POST: /DataManagement/UpdateInventoryAction
        [HttpPost]
        public JsonResult UpdateInventoryAction(InventoryActionModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "Form is not valid! " +
                          "Please correct it and try again."
                    });
                }

                if (!User.IsInRole("Officer"))
                {
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "You do not have the authority to update this inventory action."
                    });
                }
            
                JourListDMContainer dm = new JourListDMContainer();

                InventoryAction item = dm.InventoryActions.Single(z => z.Id == model.Id);
                item.Description = model.Description;
                dm.SaveChanges();

                return Json(new { Result = "OK" });
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = e.Message });
            }
        }

        //
        // POST: /DataManagement/DeleteInventoryAction
        [HttpPost]
        public JsonResult DeleteInventoryAction(int id)
        {
            if (!User.IsInRole("Officer"))
            {
                return Json(new
                {
                    Result = "ERROR",
                    Message = "You do not have the authority delete this inventory action."
                });
            }

            try
            {
                JourListDMContainer dm = new JourListDMContainer();

                // Grab the item
                InventoryAction item = dm.InventoryActions.Single(z => z.Id == id);

                // Delete the object only if there are no references
                if (item.InventoryLogs.Count < 1)
                    dm.InventoryActions.DeleteObject(item);

                // Otherwise just deactivate so we don't break any historical records
                else return Json(new
                     {
                         Result = "ERROR",
                         Message = "Please eliminate references to delete."
                     });

                // Save the changes
                dm.SaveChanges();

                return Json(new { Result = "OK" });
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = e.Message });
            }
        }

        ////
        //// POST: /DataManagement/GetInventoryActionOptions
        //[HttpPost]
        //public JsonResult GetInventoryActionOptions()
        //{
        //    try
        //    {
        //        JourListDMContainer dm = new JourListDMContainer();
        //        var list = dm.InventoryActions.Select(z => new { DisplayText = z.Description, Value = z.Id });
        //        return Json(new { Result = "OK", Options = list });
        //    }
        //    catch (Exception e)
        //    {
        //        return Json(new { Result = "ERROR", Message = e.Message });
        //    }
        //}

        #endregion

        #region Items

        // POST: /DataManagement/ItemList
        public JsonResult ItemList(FormCollection collection)
        {
            try
            {
                JourListDMContainer dm = new JourListDMContainer();
                List<ItemModel> list = new List<ItemModel>();

                foreach (var a in dm.Items.OfType<StandardItem>().Where(z => z.Active == true))
                {
                    ItemModel item = new ItemModel();
                    item.Id = a.Id;
                    item.Description = a.Description;
                    //item.Hyperlink = a.Hyperlink;
                    //item.Barcode = a.Barcode;
                    item.CategoryId = a.ItemCategory.Id;
                    item.UnitTypeId = a.UnitType.Id;
                    item.Active = a.Active;
                    list.Add(item);
                }

                return Json(new { Result = "OK", Records = list });
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = e.Message });
            }
        }

        //
        // POST: /DataManagement/CreateItem
        [HttpPost]
        public JsonResult CreateItem(ItemModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    Result = "ERROR",
                    Message = "Form is not valid! " +
                    "Please correct it and try again."
                });
            }

            if (!User.Identity.IsAuthenticated)
            {
                return Json(new
                {
                    Result = "ERROR",
                    Message = "You need to log in to add an item."
                });
            }

            if (!User.IsInRole("Officer"))
            {
                return Json(new
                {
                    Result = "ERROR",
                    Message = "You do not have the authority to create a new inventory item."
                });
            }
            try
            {
                JourListDMContainer dm = new JourListDMContainer();


                StandardItem item = new StandardItem();

                item.Description = model.Description;
                //if (model.Hyperlink != null) item.Hyperlink = model.Hyperlink;
                //if (model.Barcode != null) item.Barcode = model.Barcode;
                item.ItemCategory = dm.ItemCategories.Single(z => z.Id == model.CategoryId);
                item.UnitType = dm.UnitTypes.Single(z => z.Id == model.UnitTypeId);

                dm.AddToItems(item);
                dm.SaveChanges();

                model.Id = item.Id;

                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = e.Message });
            }
        }

        //
        // POST: /DataManagement/UpdateItem
        [HttpPost]
        public JsonResult UpdateItem(ItemModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "Form is not valid! " +
                          "Please correct it and try again."
                    });
                }

                if (!User.Identity.IsAuthenticated)
                {
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "You need to log in to add update an item."
                    });
                }

                if (!User.IsInRole("Officer"))
                {
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "You do not have the authority to create a new inventory item."
                    });
                }

                JourListDMContainer dm = new JourListDMContainer();

                Item item = dm.Items.OfType<StandardItem>().Single(z => z.Id == model.Id);

                if (item == null)
                {
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "The item was not modified since it does not exist."
                    });
                }

                item.Description = model.Description;
                //if (model.Hyperlink != null) item.Hyperlink = model.Hyperlink;
                //if (model.Barcode != null) item.Barcode = model.Barcode;
                item.ItemCategory = dm.ItemCategories.Single(z => z.Id == model.CategoryId);
                item.UnitType = dm.UnitTypes.Single(z => z.Id == model.UnitTypeId);
                item.Active = model.Active;

                dm.SaveChanges(); dm.SaveChanges();

                return Json(new { Result = "OK" });
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = e.Message });
            }
        }

        //
        // POST: /DataManagement/DeleteItem
        [HttpPost]
        public JsonResult DeleteItem(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new
                {
                    Result = "ERROR",
                    Message = "You need to log in to delete an item."
                });
            }

            if (!User.IsInRole("Officer"))
            {
                return Json(new
                {
                    Result = "ERROR",
                    Message = "You do not have the authority to delete an item."
                });
            }

            try
            {
                JourListDMContainer dm = new JourListDMContainer();

                // Grab the item
                Item item = dm.Items.OfType<StandardItem>().Single(z => z.Id == id);

                if (item == null)
                {
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "The item for deletion does not exist!"
                    });
                }

                // Delete the object only if there are no references
                if (item.Inventories.Count < 1)
                    dm.Items.DeleteObject(item);

                // Otherwise just deactivate so we don't break any historical records
                else
                    item.Active = false;

                // Save the changes
                dm.SaveChanges();

                return Json(new { Result = "OK" });
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = e.Message });
            }
        }
    #endregion

    }
}
