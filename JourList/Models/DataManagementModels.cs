using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace JourList.Models
{
    public class ActivityModel
    {
        public long Id { get; set; }

        [Required]
        [Display(Name = @"Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = @"Points")]
        public int Points { get; set; }

        [Required]
        [Display(Name = @"Unit Type")]
        public int UnitId { get; set; }

        [Required]
        [Display(Name = @"Quantity")]
        public double Quantity { get; set; }

        [Display(Name = @"Active?")]
        public bool Active { get; set; }
        public ActivityModel()
        {
        }
        public ActivityModel(Activity activity)
        {
            // TODO: Complete member initialization
            this.Id = activity.Id;
            this.Description = activity.Description;
            this.Points = activity.Points;
            this.UnitId = activity.Unit.Id;
            this.Active = activity.Active;
            this.Quantity = activity.Quantity;
        }
    }

    public struct UnitModel
    {
        public long Id { get; set; }
        
        [Required]
        [Display(Name = @"Description")]
        public string Description { get; set; }
        
        [Required]
        [Display(Name = @"Conversion Factor")]
        public double ConversionFactor { get; set; }
        
        [Required]
        [Display(Name = @"Abbreviation")]
        public string Abbreviation { get; set; }

        [Display(Name = @"Active?")]
        public bool Active { get; set; }

        [Required]
        [Display(Name = @"Unit Type")]
        public int UnitTypeId { get; set; }
    }

    public struct RecurrenceIntervalModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = @"Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = @"Minutes")]
        public long Minutes { get; set; }

        [Display(Name = @"Active?")]
        public bool Active { get; set; }
    }

    public struct UnitTypeModel
    {
        public long Id { get; set; }
        
        [Required]
        [Display(Name = @"Description")]
        public string Description { get; set; }

        [Display(Name = @"Active?")]
        public bool Active { get; set; }
    }

    public struct ItemCategoryModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = @"Description")]
        public string Description { get; set; }

        [Display(Name = @"Active?")]
        public bool Active { get; set; }
    }

    public struct InventoryActionModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = @"Description")]
        public string Description { get; set; }

        [Display(Name = @"Active?")]
        public bool Active { get; set; }
    }

}
