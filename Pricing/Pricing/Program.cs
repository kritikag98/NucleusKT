using Org.BouncyCastle.X509.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Pricing
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DBConnection db = new DBConnection();
            List<string> vals = new List<string>();
            vals.Add("corematerialmarkups");
            vals.Add("corematerialprices");
            vals.Add("edgebandprices");
            vals.Add("glassprices");
            vals.Add("handleprices");
            vals.Add("hardwareprices");
            vals.Add("legprices");
            vals.Add("priceversions");
            vals.Add("variantmarkups");
            vals.Add("variantprices");
            vals.Add("wismarkups");
            db.FileDelete();
            db.Add();
            foreach (var element in vals)
            {
                db.Comparison(element);
            }
            //db.Comparison("handleprices", "NucleusID");
            Environment.Exit(0);

        }
    }
}
