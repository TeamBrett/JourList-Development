using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JourList.Models;
using System.Net;
using JourList.Tools;

namespace JourList.Controllers
{
    public class InventoryController : Controller
    {
        //
        // Get: /Inventory/
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        #region Item
                
        //
        // POST: /Inventory/GetStandardItems
        public JsonResult GetStandardItems(FormCollection collection)
        {
            if (!User.Identity.IsAuthenticated)
                return JsonError("You need to log in to add personal items.");

            try
            {
                JourListDMContainer dm = new JourListDMContainer();
                // if convsconversion fails, the out value is false
                bool _showinactive;
                string showinactive = collection["ShowInactive"];
                bool.TryParse(showinactive, out _showinactive);
                

                //// Get all of the member's standard items
                //var sitems = dm.Items.OfType<StandardItem>();
                //foreach (var s in sitems)
                //{
                //    var witems = s.Inventories.Where(y => y.Member.Name == User.Identity.Name && y.Active == true);
                //}

                var pitems = (IEnumerable<Item>)dm.Members.First(z => z.Name == User.Identity.Name).PersonalItems
                    .Where(z => z.Inventories.FirstOrDefault(y => y.Member.Name == User.Identity.Name && y.Active == true) == null);

                var sitems = (IEnumerable<Item>)dm.Items.OfType<StandardItem>()
                              // Where the item is not already in the member's inventory
                              .Where(z => z.Inventories.FirstOrDefault(y => y.Member.Name == User.Identity.Name
                                  // And make sure to grab inactivated inventories
                                       && y.Active == true) == null );

                var items = pitems.Union(sitems);

                if (!_showinactive)
                    items = items.Where(z => true == z.Active);

                return CreateItemModelList(items);
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = e.Message });
            }
        }

        //
        // POST: /Inventory/GetInventory
        public JsonResult GetInventory(FormCollection collection)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new
                {
                    Result = "ERROR",
                    Message = "You need to log in to add personal items."
                });
            }

            try
            {
                JourListDMContainer dm = new JourListDMContainer();
                List<Item> items = new List<Item>();

                // if convsconversion fails, the out value is false
                bool _showinactive;
                string showinactive = collection["ShowInactive"];
                bool.TryParse(showinactive, out _showinactive);

                // Get only the active items if we don't want to show the inactive ones

                foreach (var inv in dm.Members.Single(z => z.Name == User.Identity.Name).Inventories)
                    if (!_showinactive && !inv.Active)
                        continue;
                    else items.Add(inv.Item);

                return CreateItemModelList(items);
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = e.Message });
            }
        }
        
        // Get Generic Items
        private JsonResult CreateItemModelList( IEnumerable<Item> items)
        {
            try
            {
                List<ItemModel> list = new List<ItemModel>();
                JourListDMContainer dm = new JourListDMContainer();
                var member = dm.Members.SingleOrDefault(z => z.Name == User.Identity.Name);

                foreach (var a in items)
                {
                    ItemModel item = new ItemModel();
                    item.Id = a.Id;
                    item.Description = a.Description;
                    //item.Hyperlink = a.Hyperlink;
                    //item.Barcode = a.Barcode;
                    item.CategoryId = a.ItemCategory.Id;
                    item.UnitTypeId = a.UnitType.Id;
                    item.Active = member.Inventories.First(z => z.Item.Id == a.Id).Active;//a.Inventories.First(.Active;
                    item.IsPersonal = a is PersonalItem;
                    Inventory inv;
                    if (member != null && (inv = member.Inventories.FirstOrDefault(z => z.Item.Id == a.Id)) != null)
                    {
                        item.InvId = inv.Id;
                        item.UnitId = inv.Unit.Id;
                        item.OnHand = inv.OnHand;
                        item.RequiredQuantity = inv.RequiredQuantity;
                        item.RestockThreshold = inv.RestockThreshold;
                    }
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
        // POST: /Inventory/AddStandardItemToInventory
        [HttpPost]
        public JsonResult AddStandardItemToInventory(FormCollection collection)
        {
            if (!User.Identity.IsAuthenticated)
                return JsonError("You need to log in to add personal items.");
            
            long itemID = -1;

            if ( long.TryParse(collection["ItemId"], out itemID) == false)
                return JsonError("Invalid ID data.");

            JourListDMContainer dm = new JourListDMContainer();

            try
            {
                // We don't want the member to access other people's items so limit
                // their access to standard items and their own.
                // Check standard items first
                Item item = dm.Items.OfType<StandardItem>().FirstOrDefault(z => z.Id == itemID);

                if (item == null)
                    // then check the member's personal items
                    item = dm.Members.First(z => z.Name == User.Identity.Name).PersonalItems.FirstOrDefault(z => z.Id == itemID);
                if (item == null)
                {
                    // So we have a more specific error message, either way we're ending this attempt.
                    item = dm.Items.OfType<PersonalItem>().FirstOrDefault(z => z.Member.Name != User.Identity.Name);
                    if (item != null)
                        return JsonError("That's not your item to add to your inventory.");
                    else return JsonError("Could not find the item.");
                }
                ItemModel model = new ItemModel();
                model.Id = item.Id;
                model.IsPersonal = item is PersonalItem;
                model.Description = item.Description;
                model.UnitTypeId = item.UnitType.Id;
                model.CategoryId = item.ItemCategory.Id;

                Unit dUnit;
                if ((dUnit = item.UnitType.DefaultUnit) != null)
                    model.UnitId = item.UnitType.DefaultUnit.Id;
                else
                    model.UnitId = dm.Units.First(z => z.UnitType.Id == item.UnitType.Id).Id;

                Inventory inv;
                if ( (inv = AddToInventory(model)) == null)
                        return JsonError("Could not add this item to your inventory");
                
                model.OnHand = inv.OnHand;
                model.RestockThreshold = inv.RestockThreshold;
                model.RequiredQuantity = inv.RequiredQuantity;
                model.Active = inv.Active;

                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception e)
            {
                return Json(new
                {
                    Result = "ERROR",
                    Message = e.Message
                });
            }
        }


        //
        // POST: /DataManagement/CreateItem
        [HttpPost]
        public JsonResult CreateItem(ItemModel model)
        {
            if (!ModelState.IsValid)
                return Json(new
                {
                    Result = "ERROR",
                    Message = "Form is not valid! " +
                    "Please correct it and try again."
                });
            
            if (!User.Identity.IsAuthenticated)
                return Json(new
                {
                    Result = "ERROR",
                    Message = "You need to log in to add personal items."
                });
            
            try
            {
                JourListDMContainer dm = new JourListDMContainer();
                var member = dm.Members.SingleOrDefault(z => z.Name == User.Identity.Name);

                // If the item exists but is inactive, just reactivate it.
                PersonalItem item;
                if ( (item = member.PersonalItems.SingleOrDefault(z => z.Description == model.Description)) != null)
                {
                    if (item.Active == false)
                        item.Active = true;
                    else
                        return Json(new
                        {
                            Result = "ERROR",
                            Message = "An active item by this description already exists"
                        });
                }
                // Otherwie create a new personal item
                else
                {
                    item = new PersonalItem();
                    item.Description = model.Description;
                    //if (model.Hyperlink != null) item.Hyperlink = model.Hyperlink;
                    //if (model.Barcode != null) item.Barcode = model.Barcode;
                    item.ItemCategory = dm.ItemCategories.Single(z => z.Id == model.CategoryId);
                    item.UnitType = dm.UnitTypes.Single(z => z.Id == model.UnitTypeId);
                    item.Member = member;
                    dm.AddToItems(item);
                    dm.SaveChanges();
                    // Hookup our model with the new jourId number
                    model.Id = item.Id;
                }

                Inventory inv;
                if ((inv = this.AddToInventory(model)) == null)
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "An active inventory for this item already exists"
                    });

                // Relevant inventory for jTable's purposes
//                model.Id = item.Id;
                model.IsPersonal = item is PersonalItem;

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
                if (!ModelState.IsValid) return JsonError("Form is not valid! Please correct it and try again.");

                if (!User.Identity.IsAuthenticated) return JsonError("You need to log in to add modify personal items.");
                
                JourListDMContainer dm = new JourListDMContainer();

                Inventory inv = dm.Members.SingleOrDefault(z=>z.Name == User.Identity.Name)
                                  .Inventories.FirstOrDefault(z => z.Item.Id == model.Id);

                if (inv == null) return JsonError("The item was not modified since it does not exist in your inventory.");

                // Allow the update of fundamental properties if it's personal
                if (inv.Item is PersonalItem)
                {
                    inv.Item.Description = model.Description;
                    //if (model.Hyperlink != null) inv.Item.Hyperlink = model.Hyperlink;
                    //if (model.Barcode != null) inv.Item.Barcode = model.Barcode;
                    inv.Item.ItemCategory = dm.ItemCategories.Single(z => z.Id == model.CategoryId);
                    inv.Item.UnitType = dm.UnitTypes.Single(z => z.Id == model.UnitTypeId);
                    inv.Active = model.Active;
                }
                // Otherwise fix the model that will be resent and displayed
                else
                {
                    // TODO: Provide some message to the user that they can't update thse
                    model.Description = inv.Item.Description;
                    model.CategoryId = inv.Item.ItemCategory.Id;
                    model.UnitTypeId = inv.Item.UnitType.Id;
                    model.Active = inv.Active;
                }

                // TODO: Create unit test to ensure unit manipulation happens after we've confirmed Unit Type changes
                Unit unit = dm.Units.Single(z => z.Id == model.UnitId);

                // Ensure the new unit is of the correct type
                if (unit.UnitType.Id != inv.Unit.UnitType.Id)
                    return JsonError("The new unit must be of type " + inv.Unit.UnitType.Description + "!");
                
                // If the standard unit is changing, convert the stock level values.
                if (inv.Unit.Id != model.UnitId)
                {
                    try
                    {
                        inv.OnHand = Tools.Tools.ConvertUnits(inv.OnHand, inv.Unit, unit);
                        inv.RestockThreshold = Tools.Tools.ConvertUnits(inv.RestockThreshold, inv.Unit, unit);
                        inv.RequiredQuantity = Tools.Tools.ConvertUnits(inv.RequiredQuantity, inv.Unit, unit);
                    }
                    catch (Exception e)
                    {
                        return JsonError(e.Message);
                    }
                }

                // Assign converted values
                inv.RestockThreshold = model.RestockThreshold;
                inv.RequiredQuantity = model.RequiredQuantity;
                inv.Unit = unit;
                
                dm.SaveChanges();

                TransactionModel cm = new TransactionModel();
                cm.InvId = inv.Id;
                cm.ActionId = dm.InventoryActions.Single(z => z.Description == "ADJUST").Id;
                cm.UnitId = unit.Id;
                cm.Quantity = 1;
                cm.Size = model.OnHand;
                cm.Cost = 0;
//                cm.IsTotalCost = true;
                
                if (UpdateStock(cm) == null)
                    return JsonError("Your inventory could not be updated.");
                
                return Json(new { Result = "OK" });
            }
            catch (Exception e)
            {
                return JsonError(e.Message);
            }
        }

        //
        // POST: /DataManagement/DeleteItem
        [HttpPost]
        public JsonResult DeleteItem(int id)
        {
            if (!User.Identity.IsAuthenticated) 
                return JsonError("You need to log in to add modify personal items.");
                
            try
            {
                JourListDMContainer dm = new JourListDMContainer();
                var member = dm.Members.SingleOrDefault(z => z.Name == User.Identity.Name);
                
                // Grab item's inventory
                var inv = member.Inventories.SingleOrDefault(z => z.Item.Id == id);

                Item item;

                // If the inventory doesn't exist
                if (inv == null)
                {
                    // and if the item is a personal item of the user
                    if ((item = member.PersonalItems.SingleOrDefault(z => z.Id == id)) == null)
                        return JsonError("The item for deletion is not yours to delete!");

                    // If it is, and they had no inventory of it, delete the item entirely
                    else
                    {
                        foreach (var info in item.ItemInfoes)
                            dm.ItemInfoes.DeleteObject(info);

                        dm.Items.DeleteObject(item);
                    }
                }

                // If there is a current inventory of the item being deleted
                else
                {
                    // Delete the object only if there are no references on the log
                    if (inv.InventoryLogs.Count < 1)
                    {

                        // Get associated item information
                        if (inv.Item is PersonalItem)
                            foreach (var info in inv.Item.ItemInfoes)
                                dm.ItemInfoes.DeleteObject(info);
                            
                            // Get the actual item
                            dm.Items.DeleteObject(inv.Item);

                        // Get the inventory entries
                        dm.Inventories.DeleteObject(inv);

                    }
                    // Otherwise just deactivate so we don't break any historical records
                    else
                    {
                        // Deactivate the item if it's personally owned
                        if (inv.Item is PersonalItem)
                            inv.Item.Active = false;

                        // Always deactivate the item's inventory
                        inv.Active = false;
                    }
                }

                // Save the changes
                dm.SaveChanges();

                return Json(new { Result = "OK" });
            }
            catch (Exception e)
            {
                return JsonError(e.Message);
            }
        }

        #endregion

        #region Shopping List

        //
        // Get: /Inventory/ShoppingList
        [HttpPost]
        public JsonResult JsonShoppingList()
        {
            if (!User.Identity.IsAuthenticated) return JsonError("You need to log in to add modify personal items.");

            try
            {
                JourListDMContainer dm = new JourListDMContainer();
                var member = dm.Members.SingleOrDefault(z => z.Name == User.Identity.Name);
                if (member == null) return JsonError("You don't exist!");

                return JsonOk(GenerateShoppingList(member));
            }
            catch (Exception e)
            {
                return JsonError(e.Message);                
            }
        }

        //
        // Get: /Inventory/ShoppingList
        [HttpGet]
        public ActionResult ShoppingList()
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("LogOn", "Account");
            
            JourListDMContainer dm = new JourListDMContainer();
            var member = dm.Members.SingleOrDefault(z => z.Name == User.Identity.Name);
            if (member == null) return RedirectToAction("LogOn", "Account");

            return View(GenerateShoppingList(member));
        }

        private List<ShoppingListModel> GenerateShoppingList(Member member)
        {
            List<ShoppingListModel> list = new List<ShoppingListModel>();

            foreach (var inv in member.Inventories.Where(z => z.Active && z.ShoppingList != null))
            {
                var model = new ShoppingListModel(inv.ShoppingList);

                //model.InvId = inv.Id;
                //model.Description = inv.Item.Description;
                //model.InCart = inv.ShoppingList.InCart;
                //model.Units = inv.Unit.Description;
                //model.Quantity = inv.ShoppingList.QuantityNeeded;
                //model.Size = inv.ShoppingList.SizeNeeded;

                list.Add(model);
            }

            return list;
        }

        //
        // POST: /Inventory/UpdateCart
        [HttpPost]
        public JsonResult UpdateCart(FormCollection collection)
        {
            JourListDMContainer dm = new JourListDMContainer();

            // Get the ID of the item we're handling
            long id;
            if (!long.TryParse(collection["InvId"], out id))
                return JsonError("Malformed ID.");

            // Verify the member
            Member member = dm.Members.SingleOrDefault(z => z.Name == User.Identity.Name);
            if (!User.Identity.IsAuthenticated || member == null)
                return JsonError("You need to log in to use the shopping cart.");

            try
            {
                // Get the shopping list item
                var list = member.Inventories.SingleOrDefault(z => id == z.Id).ShoppingList;

                // Toggle the switch
                list.InCart = !list.InCart;

                dm.SaveChanges();

                return Json(new { Result = "OK" });
            }
            catch (Exception e)
            {
                return JsonError(e.Message);
            }
        }

        /// <summary>
        /// Updates the inventory entry for the item
        /// Calls to update shopping list
        /// Creates a transaction log entry
        /// </summary>
        /// <param name="action">ADD/REMOVE/CURRENT</param>
        /// <param name="quantity">Count Multiplier</param>
        /// <param name="size">Size of each Item</param>
        /// <param name="itemId">ID of the item we're updating</param>
        /// <param name="units">Units of quantity coming in</param>
        /// <returns></returns>
        private InventoryLog UpdateStock(TransactionModel model)//(StockUpdateEnum action, double quantity, double size, long itemId, Unit units, double cost)
        {
            #region Setup objects

            JourListDMContainer dm = new JourListDMContainer();
            InventoryLog log = new InventoryLog();

            var member = dm.Members.SingleOrDefault(z => z.Name == User.Identity.Name);
            if (member == null) return null;

            var inv = member.Inventories.FirstOrDefault(z => z.Id == model.InvId);
            if (inv == null) return null;

            // Update the inventory numbers
            var action = dm.InventoryActions.Single(z => z.Id == model.ActionId).Description;
            var uaction = (StockUpdateEnum)Enum.Parse(typeof(StockUpdateEnum), action);
            
            #endregion

            #region Update inventory values
            var units = dm.Units.Single(z => z.Id == model.UnitId);
            double size;
            try
            {
                size = Tools.Tools.ConvertUnits(model.Size, units, inv.Unit);
            }
            catch (Exception e)
            {
                throw e;
            }

            double total = size * model.Quantity;

            switch (uaction)
            {
                case StockUpdateEnum.ADD:
                    inv.OnHand += total;
                    log.Cost =  model.Cost;
                    break;
                case StockUpdateEnum.REMOVE:
                    model.Quantity *= -1;
                    inv.OnHand -= total;
                    model.Cost = 0;
                    break;
                case StockUpdateEnum.ADJUST:
                    if (inv.OnHand > total)
                    {
                        model.Quantity *= -1;
                        model.Size = -1 * (inv.OnHand - total);
                    }
                    else // if inv.onhand < total
                        model.Size = total - inv.OnHand;
                    inv.OnHand = total;
                    model.Cost = 0;
                    break;
                default:
                    break;
            }
            #endregion

            dm.SaveChanges();

            #region Make a transaction log

            // Create an inventory transaction
            log.Quantity = model.Quantity;
            log.Inventory = inv;
            log.TransactionDate = DateTime.Now;
            log.InventoryAction = dm.InventoryActions.Single(z => z.Id == model.ActionId);
            log.Size = model.Size;
            log.Quantity = model.Quantity;
            log.Unit = dm.Units.Single(z => z.Id == model.UnitId);
            log.Cost = model.Cost;
            dm.SaveChanges();

            model.TransId = log.Id;

            #endregion

            // With any inventory update, it's possible the item need to be included/excluded from the shopping list.  
            UpdateShoppingList(inv);

            // Success
            return log;
        }

        private void UpdateShoppingList(Inventory inventory)
        {
            JourListDMContainer dm = new JourListDMContainer();
            var inv = dm.Inventories.Single(z=>z.Id == inventory.Id);
            bool needmore, needlist;
            // deal with shopping list if necessary
            needmore = inv.OnHand < inv.RestockThreshold;
            needlist = inv.ShoppingList == null;
            if (needmore)
            {
                if (needlist)
                {
                    // Createlist
                    ShoppingList list = new ShoppingList();

                    do { list.Id = Guid.NewGuid(); }
                    while (dm.ShoppingLists.Count(z => z.Id == list.Id) > 0);
                    dm.AddToShoppingLists(list);
                    inv.ShoppingList = list;
                }
                var log = inv.InventoryLogs.Where(z => z.UnitId == inv.Unit.Id);
                if (log.Count() > 0)
                {
                    double sze = (from a in log group a by a.Size into s orderby s.Count() descending select s).First().First().Size;
                    inv.ShoppingList.SizeNeeded = sze;
                    inv.ShoppingList.QuantityNeeded = Math.Round((inv.RequiredQuantity - inv.OnHand) / sze, MidpointRounding.AwayFromZero);
                }
                else
                {
                    inv.ShoppingList.SizeNeeded = (inv.RequiredQuantity - inv.OnHand);
                    inv.ShoppingList.QuantityNeeded = 1;
                }
            }
            else if (!needlist)
                // Remove the item from the shoppinglist
                dm.ShoppingLists.DeleteObject(dm.Inventories.First(z => z.ShoppingList.Id == inv.ShoppingList.Id).ShoppingList);

            dm.SaveChanges();
        }

        /// <summary>
        /// Add an inventory item given an item model if it doesn't exist
        /// Reactivates it if deactivated
        /// </summary>
        /// <param name="model"></param>
        /// <returns>The manipulated/created inventory entry</returns>
        private Inventory AddToInventory(ItemModel model)
        {
            JourListDMContainer dm = new JourListDMContainer();
            var member = dm.Members.SingleOrDefault(z => z.Name == User.Identity.Name);

            if (member == null) return null;

            // If inventory already exists but is inactive, reactivate
            // If it exists and is active, then just return the inventory item, do nothing
            Inventory inv;
            if ((inv = member.Inventories.FirstOrDefault(z=>z.Item.Id == model.Id)) != null)
            {
                if (inv.Active == false)
                    inv.Active = true;
                else return inv;
            }

            // Add an inventory listing if it doesn't exist.  
            else
            {
                inv = new Inventory();
                inv.Item = dm.Items.Single(z=>z.Id == model.Id);
                inv.RestockThreshold = model.RestockThreshold;
                inv.OnHand = model.OnHand;
                inv.RequiredQuantity = model.RequiredQuantity;
                inv.Unit = dm.Units.Single(z => z.Id == model.UnitId);
                inv.Member = member;
                dm.AddToInventories(inv);
            }

            dm.SaveChanges();
            
            return inv;
        }

        #endregion
        public ActionResult Use()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("LogOn", "Home");

            try
            {
                return View(new TransactionModel((int)StockUpdateEnum.REMOVE, User.Identity.Name,-1,true));
            }
            catch (Exception)
            {
                return View(new TransactionModel((int)StockUpdateEnum.REMOVE));
            }
        }

        #region
        [HttpGet]
        public ActionResult Add()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("LogOn", "Home");

            try
            {
                return View(new TransactionModel((int)StockUpdateEnum.ADD, User.Identity.Name,-1,true));
            }
            catch (Exception)
            {
                return View(new TransactionModel((int)StockUpdateEnum.ADD));
            }

        }

        public JsonResult Transact(TransactionModel model)
        {
            JourListDMContainer dm = new JourListDMContainer();

            Member member = dm.Members.SingleOrDefault(z => z.Name == User.Identity.Name);
            if (!User.Identity.IsAuthenticated || member == null)
                return JsonError("You need to log in to use the shopping cart.");

            try
            {
                var log = UpdateStock(model);
                model.TransId = log.Id;
                model.TimeStamp = log.TransactionDate;
            }
            catch (Exception e)
            {
                return JsonError(e.Message);
                throw;
            }

            TransactionModel nmodel = new TransactionModel(model.ActionId, member.Name);

            return JsonOk(new { NextItem = nmodel, Transaction = model });

        }

        [HttpPost]
        public JsonResult Add(TransactionModel model)
        {
            //JourListDMContainer dm = new JourListDMContainer();

            //// Verify the member
            //Member member = dm.Members.SingleOrDefault(z => z.Name == User.Identity.Name);
            //if (!User.Identity.IsAuthenticated || member == null)
            return JsonError("This function has been deprecated");

            //model.ActionId = dm.InventoryActions.Single(z => z.Description == "ADD").Id;

            //try
            //{
            //    model.TransId = UpdateStock(model).Id;
            //}
            //catch (Exception e)
            //{
            //    return JsonError(e.Message);
            //    throw;
            //}


            //// get next shopping list item or default item model
            //var i = member.Inventories.FirstOrDefault(z => z.ShoppingList != null);
            //TransactionModel nmodel = new TransactionModel();
            //nmodel.InvId = i == null ? 0 : i.Id;
            //nmodel.Description = i == null ? "" : i.Item.Description;
            //nmodel.Quantity = i == null ? 0 : i.ShoppingList.QuantityNeeded;
            //nmodel.Size = i == null ? 0 : i.ShoppingList.SizeNeeded;
            //nmodel.sUnitId = i == null ? "-1" : i.Unit.Id.ToString();
            //nmodel.Cost = i == null ? 0 :
            //    i.InventoryLogs == null ? 0 : i.InventoryLogs.Last().Cost;

            //return JsonOk(new { NextItem = nmodel, Transaction = tmodel });
        }

        [HttpPost]
        public JsonResult SearchInventory(string term)
        {
            JourListDMContainer dm = new JourListDMContainer();

            var items = from c in dm.Members.Single(z=>z.Name == User.Identity.Name).Inventories
                        where c.Item.Description.ToUpper().Contains(term.ToUpper())
                        select new { value = c.Id, label = c.Item.Description };

            return this.Json(items);
        }

        [HttpPost]
        public JsonResult NewTransaction(string InvId, string ActionId)
        {
            JourListDMContainer dm = new JourListDMContainer();

            long id;
            if (!long.TryParse(InvId, out id))
                return JsonError("Invalid item jourId");

            int aid;
            if (!int.TryParse(ActionId, out aid))
                return JsonError("Invalid action jourId");

            var member = dm.Members.SingleOrDefault(z => z.Name == User.Identity.Name);
            if (member == null)
                return JsonError("You don't exist");

            return JsonOk(new TransactionModel(aid, User.Identity.Name, id));
        }
//        [HttpPost]
//        public JsonResult GetItemForCheckin(string InvId)
//        {
//            JourListDMContainer dm = new JourListDMContainer();

//            long jourId;
//            if (!long.TryParse(InvId, out jourId))
//                return JsonError("Invalid item jourId");

//            var member = dm.Members.SingleOrDefault(z => z.Name == User.Identity.Name);
//            if ( member == null)
//                return JsonError("You don't exist");

////            var trans = new TransactionModel((int)StockUpdateEnum.ADD, User.Identity.Name, jourId);

//            //var inv = member.Inventories.First(z => z.Id == jourId);
//            //var log = inv.InventoryLogs.LastOrDefault();

//            //CheckInModel cm = new CheckInModel();
//            //cm.InvId = inv.Id;
//            //cm.Description = inv.Item.Description;
//            //cm.Quantity = log == null ? 0 : inv.InventoryLogs.LastOrDefault().Quantity; ;
//            //cm.Size = log == null ? 0 : inv.InventoryLogs.LastOrDefault().Size; ;
//            //cm.sUnitId = inv.Unit.Id.ToString();
//            //cm.Cost = log == null ? 0 : inv.InventoryLogs.LastOrDefault().Cost;

//            return JsonOk(new TransactionModel((int)StockUpdateEnum.ADD, User.Identity.Name, jourId));
//        }

        [HttpPost]
        public JsonResult GetTodayTransactions(FormCollection collection)
        {
            List<TransactionModel> ltm = new List<TransactionModel>();
            JourListDMContainer dm = new JourListDMContainer();

            DateTime morn;
            
            if (collection["Day"] != null)
                morn = DateTime.Parse(collection["Day"]);
            else
                morn = DateTime.Today.Date;

            DateTime eve = morn.AddDays(1);
                
            var trans = dm.InventoryLogs.Where(z => z.Inventory.Member.Name == User.Identity.Name 
                                                 && z.TransactionDate <= eve
                                                 && z.TransactionDate >= morn);

            foreach (var t in trans)
            {
//                var tt = new TransactionModel(t);
                //TransactionModel tm = new TransactionModel();
                //tm.InvId = t.Inventory.Id;
                //tm.TransId = t.Id;
                //tm.Quantity = t.Quantity;
                //tm.Size = t.Size;
                //tm.UnitId = t.Unit.Id;
                //tm.ActionId = t.InventoryAction.Id;
                //tm.Cost = t.Cost;
                //tm.Description = t.Inventory.Item.Description;
                //tm.TimeStamp = t.TransactionDate;
                ltm.Add(new TransactionModel(t));
            }
            // TODO: Implement
            return JsonOk(ltm);
        }

        #endregion

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
