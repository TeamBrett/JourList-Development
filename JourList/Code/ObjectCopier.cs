using System;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.Mvc.Html;
using System.Web.Mvc;
using System.Text;
using System.Collections.Generic;
using JourList.Models;
namespace JourList.Tools
{
    public static class ObjectCopier
    {
        /// <summary>
        /// Perform a deep Copy of the object.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T Clone<T>(this T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }
    public static class Tools
    {
        /// <summary>Renders a view to string.</summary> 
        public static string RenderPartialToString(this HtmlHelper html, string viewName, object viewData)
        {
            return RenderViewToString(html.ViewContext.Controller.ControllerContext, viewName, viewData);
        }
        /// <summary>Renders a view to string.</summary> 
        public static string RenderViewToString(this Controller controller, string viewName, object viewData)
        {
            return RenderViewToString(controller.ControllerContext, viewName, viewData);
        }

        public static string RenderViewToString(ControllerContext context, string viewName, object viewData)
        {
            //Create memory writer 
            var sb = new StringBuilder();
            var memWriter = new StringWriter(sb);

            //Create fake http context to render the view 
            var fakeResponse = new HttpResponse(memWriter);
            var fakeContext = new HttpContext(HttpContext.Current.Request, fakeResponse);
            var fakeControllerContext = new ControllerContext(
                new HttpContextWrapper(fakeContext),
                context.RouteData, context.Controller);

            var oldContext = HttpContext.Current;
            HttpContext.Current = fakeContext;

            //Use HtmlHelper to render partial view to fake context 
            var html = new HtmlHelper(new ViewContext(fakeControllerContext,
                new FakeView(), new ViewDataDictionary(), new TempDataDictionary(), memWriter),
                new ViewPage());
            html.RenderPartial(viewName, viewData);

            //Restore context 
            HttpContext.Current = oldContext;

            //Flush memory and return output 
            memWriter.Flush();
            return sb.ToString();
        }

        /// <summary>Fake IView implementation, only used to instantiate an HtmlHelper.</summary> 
        public class FakeView : IView
        {
            #region IView Members
            public void Render(ViewContext viewContext, System.IO.TextWriter writer)
            {
                throw new NotImplementedException();
            }
            #endregion
        }
        /// <summary>
        /// Converts a quantity from one unit type to another
        /// </summary>
        /// <param name="quantity">Quantity to convert to</param>
        /// <param name="from">Unit to convert from</param>
        /// <param name="to">Unit to convert to</param>
        /// <returns>Converted value</returns>
        public static double ConvertUnits(double quantity, Unit from, Unit to)
        {
            if (from.UnitType != to.UnitType)
                throw new Exception("Unit types do not match for conversion");
            // Perform conversions
            if (from != to)
            {
                // To the standard
                quantity = quantity * from.ConversionFactor;

                // To item's preferred units
                quantity = quantity / to.ConversionFactor;
            }
            return quantity;
        }

        public class KeyEqualityComparer<T> : IEqualityComparer<T>
        {
            private readonly Func<T, object> keyExtractor;

            public KeyEqualityComparer(Func<T, object> keyExtractor)
            {
                this.keyExtractor = keyExtractor;
            }

            public bool Equals(T x, T y)
            {
                return this.keyExtractor(x).Equals(this.keyExtractor(y));
            }

            public int GetHashCode(T obj)
            {
                return this.keyExtractor(obj).GetHashCode();
            }
        }
    }

}