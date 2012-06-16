using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using mod = JourList.Models;
using System.Web.Security;

namespace JourList
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            LoadDefaultData();
        }

        protected void LoadDefaultData()
        {
            JourList.Models.JourListDMContainer dm = new Models.JourListDMContainer();
            MembershipUser muc;

            if (   dm.Members.SingleOrDefault(z => z.Name == "TeamBrett") == null
                && (muc = Membership.GetUser("TeamBrett"))                != null)
            {

                mod.Member m = new mod.Member();
                m.UserId = (Guid)muc.ProviderUserKey;
                m.Name = muc.UserName;
                dm.AddToMembers(m);
                dm.SaveChanges();
            }

            // Recurrence Intervals
            if (dm.RecurrenceIntervals.Count() == 0)
            {
                mod.RecurrenceInterval rid = new Models.RecurrenceInterval();
                rid.Description = "Daily";
                dm.AddToRecurrenceIntervals(rid);

                mod.RecurrenceInterval riw = new Models.RecurrenceInterval();
                riw.Description = "Weekly";
                dm.AddToRecurrenceIntervals(riw);

                mod.RecurrenceInterval rim = new Models.RecurrenceInterval();
                rim.Description = "Monthly";
                dm.AddToRecurrenceIntervals(rim);

                mod.RecurrenceInterval riy = new Models.RecurrenceInterval();
                riy.Description = "Yearly";
                dm.AddToRecurrenceIntervals(riy);

                mod.RecurrenceInterval rin = new Models.RecurrenceInterval();
                rin.Description = "Never";
                dm.AddToRecurrenceIntervals(rin);


                dm.SaveChanges();

            }

            // Unit Types
            if (dm.UnitTypes.Count() == 0)
            {
                string[] icDescriptions = new string[] { "Other", "Volume", "Distance", "Weight", "Time", "Temperature", "None" };

                // Can't just insert the same object with changes over and over, so we need a list of unique item categories
                List<mod.UnitType> ic = new List<mod.UnitType>(icDescriptions.Length);

                // Populate the item category descriptions with our list
                for (int i = 0; i < icDescriptions.Length; i++)
                {
                    ic.Add(new mod.UnitType());
                    ic[i].Description = icDescriptions[i];
                    dm.AddToUnitTypes(ic[i]);
                }
                dm.SaveChanges();
            }


            // Inventory Actions
            if (dm.InventoryActions.Count() == 0)
            {
                string[] icDescriptions = Enum.GetNames(typeof(mod.StockUpdateEnum)); ;

                // Can't just insert the same object with changes over and over, so we need a list of unique item categories
                List<mod.InventoryAction> ic = new List<mod.InventoryAction>(icDescriptions.Length);

                // Populate the item category descriptions with our list
                for (int i = 0; i < icDescriptions.Length; i++)
                {
                    ic.Add(new mod.InventoryAction());
                    ic[i].Description = icDescriptions[i];
                    dm.AddToInventoryActions(ic[i]);
                }
                dm.SaveChanges();
            }


            // Recurrence Interval
            if (dm.RecurrenceIntervals.Count() == 0)
            {
                string[] Descriptions = new string[] { "Minute", "Hour", "Day", "Week", "Month", "Year" };
                long[]   Minutes =      new long[]   { 1,        60,     1440,  10080,  0,       525600 };

                // Can't just insert the same object with changes over and over, so we need a list of unique item categories
                List<mod.RecurrenceInterval> ic = new List<mod.RecurrenceInterval>(Descriptions.Length);

                // Populate the item category descriptions with our list
                for (int i = 0; i < Descriptions.Length; i++)
                {
                    ic.Add(new mod.RecurrenceInterval());
                    
                    ic[i].Description = Descriptions[i];
                    ic[i].Minutes = Minutes[i];
                    
                    dm.AddToRecurrenceIntervals(ic[i]);
                }
                dm.SaveChanges();
            }

            // Units
            if (dm.Units.Count() == 0)
            {
                object[,] values = {
                                         {"Gram",       "gm",   1.0,           "Weight"}
                                        ,{"Pound",      "lb",   453.59237,     "Weight"}
                                        ,{"Ounces",     "oz",   28.3495231,    "Weight"}

                                        ,{"Liter",      "l" ,   1.0,           "Volume"}
                                        ,{"Fluid Once", "fl oz",0.02395735296, "Volume"}
                                        ,{"Gallon",     "gal",  3.78541178,    "Volume"}
                                        ,{"Cubic Feet", "ft3",  28.3168466,    "Volume"}
                                        ,{"Cubic Meter","m3",   1000.0,        "Volume"}
                                        ,{"Cubic Inch", "in3",  0.016387064,   "Volume"}
                                        ,{"Cubic Yards","yd3",  28.3495231,    "Volume"}
                                        ,{"Ounces",     "oz",   764.554858,    "Weight"}

                                        ,{"Mile",       "m",    1609.344,    "Distance"}
                                        ,{"Inch",       "in",   0.0254,      "Distance"}
                                        ,{"Yard",       "yd",   0.9144,      "Distance"}
                                        ,{"Foot",       "ft",   0.3048,      "Distance"}
                                        ,{"Kilometer",  "km",   1000.0,      "Distance"}
                                        ,{"Meter",      "m",    1.0,         "Distance"}
                                        ,{"Centimeter", "cm",   .01,         "Distance"}

                                        ,{"Second",     "s",    0.016666666666,  "Time"}
                                        ,{"Hour",       "h",    60.0,            "Time"}
                                        ,{"Minute",     "m",    1.0,             "Time"}
                                        ,{"Day",        "d",    1440.0,          "Time"}
                                        
                                        ,{"Celsius",    "C",    1.0,      "Temperature"}

                                        ,{"Unit",       "u",    1.0,            "Other"}
                                    };

                for (int i = 0; i < values.Length/4; i++)
			    {
                    int j = 0;
                    var u = new mod.Unit();
                    u.Description = (string)values[i, j++];
                    u.Abbreviation = (string)values[i, j++];
                    u.ConversionFactor = (double)values[i, j++];
                    string utDesc = (string)values[i, j];
                    u.UnitType = dm.UnitTypes.Single(z => z.Description == utDesc);
                    dm.AddToUnits(u);
			    }

                foreach (var type in dm.UnitTypes)
                    type.DefaultUnit = type.Units.FirstOrDefault(z => z.ConversionFactor == 1);

                dm.SaveChanges();
            }
            foreach (var type in dm.UnitTypes)
                type.DefaultUnit = type.Units.FirstOrDefault(z => z.ConversionFactor == 1);
            dm.SaveChanges();
            // Item Categories
            if (dm.ItemCategories.Count() == 0)
            {
                string[] icDescriptions = new string[] {"Food","Clothes","Appliances","Bath","Electronics"};

                // Can't just insert the same object with changes over and over, so we need a list of unique item categories
                List<mod.ItemCategory> ic = new List<mod.ItemCategory>(icDescriptions.Length);

                // Populate the item category descriptions with our list
                for (int i = 0; i < icDescriptions.Length; i++)
                {
                    ic.Add(new mod.ItemCategory());
                    ic[i].Description = icDescriptions[i];
                    dm.AddToItemCategories(ic[i]);
                }
                dm.SaveChanges();
            }
            dm.Dispose();
        }
    }
}