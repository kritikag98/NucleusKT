using NucleusComparison;
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
            pairs.Add(new KeyValuePair<string, string>("accessoryassemblyshuttertype", "AASTID"));
            pairs.Add(new KeyValuePair<string, string>("acchardwares", "AHID"));
            pairs.Add(new KeyValuePair<string, string>("accpanels", "APID"));
            pairs.Add(new KeyValuePair<string, string>("addonaccessories", "AOAID"));
            pairs.Add(new KeyValuePair<string, string>("ambiencecategories", "Category"));
            pairs.Add(new KeyValuePair<string, string>("ambienceitems", "Name"));
            pairs.Add(new KeyValuePair<string, string>("ambiencerooms", "AMRID"));
            pairs.Add(new KeyValuePair<string, string>("appliancecategories", "Category"));
            pairs.Add(new KeyValuePair<string, string>("applianceitems", "Name"));
            pairs.Add(new KeyValuePair<string, string>("appliancerooms", "APRID"));
            pairs.Add(new KeyValuePair<string, string>("cabinetconstructiontypeassemblies", "Component"));
            pairs.Add(new KeyValuePair<string, string>("cabinetconstructiontypehardwares", "CCTID"));
            pairs.Add(new KeyValuePair<string, string>("cabinetconstructiontypes", "TypeName"));
            pairs.Add(new KeyValuePair<string, string>("cabinetfinishtypes", "CFTID"));
            pairs.Add(new KeyValuePair<string, string>("cabinetshuttertypes", "CSTID"));
            pairs.Add(new KeyValuePair<string, string>("cabinetsubtypes", "Name"));
            pairs.Add(new KeyValuePair<string, string>("cabinetsubtypesizes", "CSTWID"));
            pairs.Add(new KeyValuePair<string, string>("cabinettypes", "Name"));
            pairs.Add(new KeyValuePair<string, string>("cities", "CityName"));
            pairs.Add(new KeyValuePair<string, string>("citycores", "CCMRID"));
            pairs.Add(new KeyValuePair<string, string>("cityglasses", "CityID"));
            pairs.Add(new KeyValuePair<string, string>("citymodules", "CityID"));
            pairs.Add(new KeyValuePair<string, string>("cityshades", "CFTSSID"));
            pairs.Add(new KeyValuePair<string, string>("corematerialfinishes", "CFID"));
            pairs.Add(new KeyValuePair<string, string>("corematerialrates", "Color"));
            pairs.Add(new KeyValuePair<string, string>("corematerials", "CMName"));
            pairs.Add(new KeyValuePair<string, string>("corematerialshuttertypes", "CMSTID"));
            pairs.Add(new KeyValuePair<string, string>("designcorethickness", "CMRID"));
            pairs.Add(new KeyValuePair<string, string>("designfamilies", "DFID"));
            pairs.Add(new KeyValuePair<string, string>("designrooms", "DRID"));
            pairs.Add(new KeyValuePair<string, string>("designs", "Name"));
            pairs.Add(new KeyValuePair<string, string>("designshuttertypes", "DSTID"));
            pairs.Add(new KeyValuePair<string, string>("designvariants", "DVID"));
            pairs.Add(new KeyValuePair<string, string>("edgebandings", "EBName"));
            pairs.Add(new KeyValuePair<string, string>("finishtypes", "Name"));
            pairs.Add(new KeyValuePair<string, string>("finishtypeseries", "Name"));
            pairs.Add(new KeyValuePair<string, string>("finishtypeseriesshades", "Name"));
            pairs.Add(new KeyValuePair<string, string>("glasscategories", "Category"));
            pairs.Add(new KeyValuePair<string, string>("glasses", "Name"));
            pairs.Add(new KeyValuePair<string, string>("glassfamilies", "gfid"));
            pairs.Add(new KeyValuePair<string, string>("glassproducts", "GPID"));
            pairs.Add(new KeyValuePair<string, string>("handlecategories", "HCName"));
            pairs.Add(new KeyValuePair<string, string>("handlecategoryhandles", "HCHID"));
            pairs.Add(new KeyValuePair<string, string>("handlecategoryproducts", "HCPID"));
            pairs.Add(new KeyValuePair<string, string>("handlepositions", "Description"));
            pairs.Add(new KeyValuePair<string, string>("handles", "Name"));
            pairs.Add(new KeyValuePair<string, string>("hardwares", "Name"));
            pairs.Add(new KeyValuePair<string, string>("legproducts", "LPRID"));
            pairs.Add(new KeyValuePair<string, string>("legrooms", "LegRID"));
            pairs.Add(new KeyValuePair<string, string>("legs", "Name"));
            pairs.Add(new KeyValuePair<string, string>("panelcoreselection", "PanelColour"));
            pairs.Add(new KeyValuePair<string, string>("panels", "Name"));
            pairs.Add(new KeyValuePair<string, string>("productcabinets", "PCID"));
            pairs.Add(new KeyValuePair<string, string>("productcores", "PCID"));
            pairs.Add(new KeyValuePair<string, string>("products", "Name"));
            pairs.Add(new KeyValuePair<string, string>("publishpoints", "PPName"));
            pairs.Add(new KeyValuePair<string, string>("reports", "Reportname"));
            pairs.Add(new KeyValuePair<string, string>("reportsunused", "Reportname"));
            pairs.Add(new KeyValuePair<string, string>("roomcabinets", "RCID"));
            pairs.Add(new KeyValuePair<string, string>("rooms", "Name"));
            pairs.Add(new KeyValuePair<string, string>("shadeedgebandmap", "SEBID"));
            pairs.Add(new KeyValuePair<string, string>("shuttercombinations", "Name"));
            pairs.Add(new KeyValuePair<string, string>("shutterhandlepositions", "SHPID"));
            pairs.Add(new KeyValuePair<string, string>("shutterrates", "SRID"));
            pairs.Add(new KeyValuePair<string, string>("shuttersubtypedims", "Description"));
            pairs.Add(new KeyValuePair<string, string>("shuttersubtypehardware", "STSID"));
            pairs.Add(new KeyValuePair<string, string>("shuttersubtypes", "Name"));
            pairs.Add(new KeyValuePair<string, string>("shuttertypes", "Name"));
            pairs.Add(new KeyValuePair<string, string>("stdcabinetaccessory", "SCAID"));
            pairs.Add(new KeyValuePair<string, string>("wardrobeaccessories", "Name"));
            pairs.Add(new KeyValuePair<string, string>("wardrobeaccessorycabinets", "WACID"));
            pairs.Add(new KeyValuePair<string, string>("wardrobeaccessorypanels", "PanelName"));
            pairs.Add(new KeyValuePair<string, string>("wardrobeacchardwares", "WAHID"));
            pairs.Add(new KeyValuePair<string, string>("wardrobeinternalsetaccessories", "WISACCID"));
            pairs.Add(new KeyValuePair<string, string>("wardrobeinternalsetassemblies", "Component"));
            pairs.Add(new KeyValuePair<string, string>("wardrobeinternalsetcabinets", "WISID"));
            pairs.Add(new KeyValuePair<string, string>("wardrobeinternalsethardware", "WISHID"));
            pairs.Add(new KeyValuePair<string, string>("wardrobeinternalsets", "WISName"));
            pairs.Add(new KeyValuePair<string, string>("wardrobemultiassemblies", "Description"));
            pairs.Add(new KeyValuePair<string, string>("wardrobemulticabinets", "WMSCID"));
            pairs.Add(new KeyValuePair<string, string>("wardrobemultihardware", "WMSHID"));
            pairs.Add(new KeyValuePair<string, string>("wardrobemultishutters", "WMSName"));

            db.fileDelete();

            foreach (var element in pairs)
            {
                Console.WriteLine("For the table {0}, the generated report is", element.Key);
                db.comparison(element.Key, element.Value);
            }
            //db.comparison("cabinetsubtypesizes", "CSTWID");
            Environment.Exit(0);
        }
    }
}
