using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;


namespace JourList.Models
{

    public class ItemModel
    {
        [Display(Name = @"Item Id")]
        public long Id { get; set; }

        [Display(Name = @"Inventory Id")]
        public long InvId { get; set; }
        
        [Required]
        [Display(Name = @"Description")]
        public string Description { get; set; }

        [Display(Name = @"Hyperlink")]
        public string Hyperlink { get; set; }

        [Required]
        [Display(Name = @"Category")]
        public long CategoryId { get; set; }

        [Required]
        [Display(Name = @"Unit Type")]
        public int UnitTypeId { get; set; }

        [Required]
        [Display(Name = @"Units")]
        public int UnitId { get; set; }

        [Required]
        [Display(Name = @"Required Quantity")]
        public double RequiredQuantity { get; set; }

        [Required]
        [Display(Name = @"On Hand")]
        public double OnHand { get; set; }

        [Required]
        [Display(Name = @"Restock Threshold")]
        public double RestockThreshold { get; set; }

        [Display(Name = @"Active?")]
        public bool Active { get; set; }

        [Display(Name = @"Barcode")]
        public string Barcode { get; set; }

        public bool IsPersonal { get; set; }
    }

    public class ShoppingListModel
    {
        [Display(Name = @"Inventory Id")]
        public long InvId { get; set; }

        [Required]
        [Display(Name = @"Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = @"Units")]
        public string Units { get; set; }

        [Required]
        [Display(Name = @"Quantity")]
        public double Quantity { get; set; }

        [Required]
        [Display(Name = @"Size")]
        public double Size { get; set; }

        [Required]
        [Display(Name = @"In Cart?")]
        public bool InCart { get; set; }

        public ShoppingListModel() { }

        public ShoppingListModel(ShoppingList list)
        {
            this.InvId = list.Inventory.Id;
            this.Description = list.Inventory.Item.Description;
            this.InCart = list.InCart;
            this.Units = list.Inventory.Unit.Description;
            this.Quantity = list.QuantityNeeded;
            this.Size = list.SizeNeeded;
        }
    }

    public enum StockUpdateEnum { ADD = 1, REMOVE = 2, ADJUST = 3 }

    public class TransactionModel
    {
        [Display(Name = @"Item Id")]
        public long InvId { get; set; }

        [Display(Name = @"Item Id")]
        public long TransId { get; set; }

        [Display(Name = @"Description")]
        public string Description { get; set; }

        //[Required]
        [Display(Name = @"Quantity")]
        public double Quantity { get; set; }

        //[Required]
        [Display(Name = @"Size")]
        public double Size { get; set; }

        //[Required]
        [Display(Name = @"Units")]
        int unitid;
        public int UnitId 
        { 
            get { return unitid; } 
            set { unitid = value; this.sunitid = value.ToString(); } 
        }

        //[Required]
        [Display(Name = @"Cost")]
        public double Cost { get; set; }

        //[Required]
        [Display(Name = @"Action")]
        public int ActionId { get; set; }

        //[Required]
        [Display(Name = @"Date/Time")]
        public DateTime TimeStamp { get; set; }

        //[Required]
        [Display(Name = @"Unitz")]
        string sunitid;
        public string sUnitId 
        { 
            get { return sunitid; } 
            set 
            {
                if(String.IsNullOrEmpty(value))
                    unitid = 0;
                else if (int.TryParse(value, out unitid) == false)
                        throw new Exception("Not a number");
            } 
        }

        //[Required]
        public SelectList UnitOptions { get; set; }

        public TransactionModel() : this(-1, null, -1)
        {
            
        }
        public TransactionModel(InventoryLog log)
        {
            this.InvId = log.Inventory.Id;
            this.TransId = log.Id;
            this.Quantity = log.Quantity;
            this.Size = log.Size;
            this.UnitId = log.Unit.Id;
            this.ActionId = log.InventoryAction.Id;
            this.Cost = log.Cost;
            this.Description = log.Inventory.Item.Description;
            this.TimeStamp = log.TransactionDate;
        }
        public TransactionModel(int actionId = -1, string user = null, long invid = -1, bool allunits=false)
        {
            JourListDMContainer dm = new JourListDMContainer();
            TransactionModel model = this;
            Inventory inv = null;
            IEnumerable<Unit> units = dm.Units;
            // Verify the member
            if (user != null)
            {
                Member member = dm.Members.SingleOrDefault(z => z.Name == user);
                if (member == null) throw new Exception("User not authenticated");

                // Get the item if the jourId is specified
                inv = member.Inventories.FirstOrDefault(z => z.Id == invid);
                
                if (inv == null) inv = member.Inventories.FirstOrDefault(z => z.ShoppingList != null);

                // If that didn't work just grab the first item from the inventory
                if (inv == null) inv = member.Inventories.FirstOrDefault();

                // If that didn't work then they get nothing
                if (inv != null)
                {
                    model.InvId = inv.Id;
                    model.Description = inv.Item.Description;
                    model.UnitId = inv.Unit.Id;

                    InventoryLog log;
                    if ((log = inv.InventoryLogs.FirstOrDefault()) != null)
                        model.Cost = log.Cost;
                    else model.Cost = 0;
                    
                    if(allunits == false)
                        units = inv.Unit.UnitType.Units;
                    
                    if (inv.ShoppingList != null)
                    {
                        model.Quantity = inv.ShoppingList.QuantityNeeded;
                        model.Size = inv.ShoppingList.SizeNeeded;
                    }
                    else
                    {
                        model.Quantity = 0;
                        model.Size = 0;
                    }
                }

            }
            if(inv == null)
            {
                model.InvId = -1;
                model.Description = "";
                model.Quantity = 0;
                model.Size = 0;
                model.sUnitId = "";
                model.Cost = 0;
            }

            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in units)
            {
                SelectListItem li = new SelectListItem();
                li.Text = item.Description;
                li.Value = item.Id.ToString();
                if (inv != null && item.Id == inv.Unit.Id)
                    li.Selected = true;
                list.Add(li);
            }

            model.UnitOptions = new SelectList(list, "Value", "Text", model.sUnitId);

            if (actionId != -1)
                model.ActionId = actionId;
        }
    }
}