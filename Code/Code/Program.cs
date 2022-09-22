using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            DBConnection db = new DBConnection();

            //List containing table names and column identifiers

            List<KeyValuePair<string, string>> pairs = new List<KeyValuePair<string, string>>();
            //pairs.Add(new KeyValuePair<string, string>("acccabinets", "CSubTID"));
            pairs.Add(new KeyValuePair<string, string>("accessories", "Name"));
            pairs.Add(new KeyValuePair<string, string>("accessoryassemblyshuttertype", "AID"));
            pairs.Add(new KeyValuePair<string, string>("acchardwares", "HID"));
            pairs.Add(new KeyValuePair<string, string>("accpanels", "PID"));
            pairs.Add(new KeyValuePair<string, string>("addonaccessories", "MainAID"));
            pairs.Add(new KeyValuePair<string, string>("ambiencecategories", "Category"));
            pairs.Add(new KeyValuePair<string, string>("ambienceitems", "Name"));
            pairs.Add(new KeyValuePair<string, string>("ambiencerooms", "RID"));
            pairs.Add(new KeyValuePair<string, string>("appliancecategories", "Category"));
            pairs.Add(new KeyValuePair<string, string>("applianceitems", "Name"));
            pairs.Add(new KeyValuePair<string, string>("appliancerooms", "RID"));
            pairs.Add(new KeyValuePair<string, string>("cabinetconstructiontypeassemblies", "Component"));
            pairs.Add(new KeyValuePair<string, string>("cabinetconstructiontypehardwares", "HID"));
            pairs.Add(new KeyValuePair<string, string>("cabinetconstructiontypes", "TypeName"));
            pairs.Add(new KeyValuePair<string, string>("cabinetfinishtypes", "FTID"));
            pairs.Add(new KeyValuePair<string, string>("cabinetshuttertypes", "SCID"));
            pairs.Add(new KeyValuePair<string, string>("cabinetsubtypes", "Name"));
            pairs.Add(new KeyValuePair<string, string>("cabinetsubtypesizes", "CSubTID"));
            pairs.Add(new KeyValuePair<string, string>("cabinettypes", "Name"));
            pairs.Add(new KeyValuePair<string, string>("cities", "CityName"));
            pairs.Add(new KeyValuePair<string, string>("citycores", "CMRID"));
            pairs.Add(new KeyValuePair<string, string>("cityglasses", "GID"));
            pairs.Add(new KeyValuePair<string, string>("citymodules", "CSubTID"));
            pairs.Add(new KeyValuePair<string, string>("cityshades", "FTSSID"));
            pairs.Add(new KeyValuePair<string, string>("corematerialfinishes", "FTID"));
            pairs.Add(new KeyValuePair<string, string>("corematerialrates", "Color"));
            pairs.Add(new KeyValuePair<string, string>("corematerials", "CMName"));
            pairs.Add(new KeyValuePair<string, string>("corematerialshuttertypes", "CMID"));
            pairs.Add(new KeyValuePair<string, string>("designcorethickness", "DID"));
            pairs.Add(new KeyValuePair<string, string>("designfamilies", "FTID"));
            pairs.Add(new KeyValuePair<string, string>("designrooms", "RID"));
            pairs.Add(new KeyValuePair<string, string>("designs", "Name"));
            pairs.Add(new KeyValuePair<string, string>("designshuttertypes", "RFT"));
            pairs.Add(new KeyValuePair<string, string>("designvariants", "FTSID"));
            pairs.Add(new KeyValuePair<string, string>("edgebandings", "EBName"));
            pairs.Add(new KeyValuePair<string, string>("finishtypes", "Name"));
            pairs.Add(new KeyValuePair<string, string>("finishtypeseries", "Name"));
            pairs.Add(new KeyValuePair<string, string>("finishtypeseriesshades", "Name"));
            pairs.Add(new KeyValuePair<string, string>("glasscategories", "Category"));
            pairs.Add(new KeyValuePair<string, string>("glasses", "Name"));
            pairs.Add(new KeyValuePair<string, string>("glassfamilies", "ftid"));
            pairs.Add(new KeyValuePair<string, string>("glassproducts", "PRID"));
            pairs.Add(new KeyValuePair<string, string>("handlecategories", "HCName"));
            pairs.Add(new KeyValuePair<string, string>("handlecategoryhandles", "HandleID"));
            pairs.Add(new KeyValuePair<string, string>("handlecategoryproducts", "PRID"));
            pairs.Add(new KeyValuePair<string, string>("handlepositions", "Description"));
            pairs.Add(new KeyValuePair<string, string>("handles", "Name"));
            pairs.Add(new KeyValuePair<string, string>("hardwares", "Name"));
            pairs.Add(new KeyValuePair<string, string>("legproducts", "PRID"));
            pairs.Add(new KeyValuePair<string, string>("legrooms", "RID"));
            pairs.Add(new KeyValuePair<string, string>("legs", "Name"));
            pairs.Add(new KeyValuePair<string, string>("panelcoreselection", "PanelColour"));
            pairs.Add(new KeyValuePair<string, string>("panels", "Name"));
            pairs.Add(new KeyValuePair<string, string>("productcabinets", "PRID"));
            pairs.Add(new KeyValuePair<string, string>("productcores", "CMID"));
            pairs.Add(new KeyValuePair<string, string>("products", "Name"));
            //pairs.Add(new KeyValuePair<string, string>("publishpoints", "PPName"));
            pairs.Add(new KeyValuePair<string, string>("reports", "Reportname"));
            pairs.Add(new KeyValuePair<string, string>("reportsunused", "Reportname"));
            pairs.Add(new KeyValuePair<string, string>("roomcabinets", "CTID"));
            pairs.Add(new KeyValuePair<string, string>("rooms", "Name"));
            pairs.Add(new KeyValuePair<string, string>("shadeedgebandmap", "EBID"));
            pairs.Add(new KeyValuePair<string, string>("shuttercombinations", "Name"));
            pairs.Add(new KeyValuePair<string, string>("shutterhandlepositions", "HPID"));
            pairs.Add(new KeyValuePair<string, string>("shutterrates", "Rate"));
            pairs.Add(new KeyValuePair<string, string>("shuttersubtypedims", "Description"));
            pairs.Add(new KeyValuePair<string, string>("shuttersubtypehardware", "HID"));
            pairs.Add(new KeyValuePair<string, string>("shuttersubtypes", "Name"));
            pairs.Add(new KeyValuePair<string, string>("shuttertypes", "Name"));
            pairs.Add(new KeyValuePair<string, string>("stdcabinetaccessory", "MainAid"));
            pairs.Add(new KeyValuePair<string, string>("wardrobeaccessories", "Name"));
            pairs.Add(new KeyValuePair<string, string>("wardrobeaccessorycabinets", "CSubTID"));
            pairs.Add(new KeyValuePair<string, string>("wardrobeaccessorypanels", "PanelName"));
            pairs.Add(new KeyValuePair<string, string>("wardrobeacchardwares", "HID"));
            pairs.Add(new KeyValuePair<string, string>("wardrobeinternalsetaccessories", "AID"));
            pairs.Add(new KeyValuePair<string, string>("wardrobeinternalsetassemblies", "Component"));
            pairs.Add(new KeyValuePair<string, string>("wardrobeinternalsetcabinets", "CSubTID"));
            pairs.Add(new KeyValuePair<string, string>("wardrobeinternalsethardware", "HID"));
            pairs.Add(new KeyValuePair<string, string>("wardrobeinternalsets", "WISName"));
            pairs.Add(new KeyValuePair<string, string>("wardrobemultiassemblies", "Description"));
            pairs.Add(new KeyValuePair<string, string>("wardrobemulticabinets", "CsubTID"));
            pairs.Add(new KeyValuePair<string, string>("wardrobemultihardware", "HID"));
            pairs.Add(new KeyValuePair<string, string>("wardrobemultishutters", "WMSName"));

            db.FileDelete();

            foreach (var element in pairs)
             {
                 Console.WriteLine("For the table {0}, the generated report is", element.Key);
                 db.Comparison(element.Key, element.Value);
                 db.RowComparison(element.Key, element.Value);
             }
            //db.rowComparison("cabinetsubtypesizes", "CSubTId");
            Environment.Exit(0);
        }
    }
}
