using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.IO;

namespace ImgSorter
{
    class Program
    {
        
        static void Main(string[] args)
        {
            string mainDir;
            string outputDir;
            if (args.Length == 0)
            {
                mainDir = @"C:\Users\Maciej\Pictures";
                outputDir = @"C:\Users\Maciej\Pictures";

            } else
            {
                mainDir = args[0];
                outputDir = @"C:\Users\Maciej\Pictures";
                Console.WriteLine("Dir set to " + mainDir);
            }
            
            string json;
            string[] dirs = Directory.GetFiles(mainDir, "*.jpg");
            Console.WriteLine("The number of files starting with jpg is {0}.", dirs.Length);
            
            json = File.ReadAllText("json1.json");
            Industrial cats = JsonConvert.DeserializeObject<Industrial>(json);
            foreach (Replace rpl in cats.replaces)
            {
                foreach (string dir in dirs)
                {
                    try { 
                    string fll = dir.Replace(rpl.src, rpl.dst);
                    System.IO.File.Move(dir, fll);
                    fll = dir.Replace(rpl.src.ToLower(), rpl.dst);
                    System.IO.File.Move(dir, fll);
                    } catch (Exception ex)
                    {
                        Console.WriteLine("ERROR>>>" + ex.Message);
                    }
                }
            }
            dirs = null;
            dirs = Directory.GetFiles(mainDir, "*.jpg");
            foreach (string el in cats.categories.places) {
                Console.WriteLine(el);
                foreach (string dir in dirs)
                {
                    try { 
                    if (dir.Contains(el) || dir.Contains(el.ToLower())) {
                        string eldir = outputDir + @"\industry\" + cats.categories.name + @"\" + el + @"\";
                        checkDir(eldir);
                        Console.WriteLine(dir);
                        string result = Path.GetFileName(dir);
                        Console.WriteLine("SOURCE >>>" + dir);
                        Console.WriteLine("DEST>>>" + eldir + result);
                        System.IO.File.Move(dir, eldir + result);
                        Console.WriteLine("Done");
                        }
                    } catch (Exception ex)
                    {
                        Console.WriteLine("ERROR>>>" + ex.Message);
                    }
                
                    
                }
            }
        }

        static bool checkDir(string dir)
        {
            
            bool rtn = false;
            if (System.IO.Directory.Exists(dir))
            {
                rtn = true;
            } else
            {
                rtn = false;
                System.IO.Directory.CreateDirectory(dir);
            }

            return rtn;
            
        }
    }



    public class Replace
    {
        public string src { get; set; }
        public string dst { get; set; }
    }

    public class Categories
    {
        public string name { get; set; }
        public IList<string> places { get; set; }
    }

    public class Industrial
    {
        public IList<Replace> replaces { get; set; }
        public Categories categories { get; set; }
    }



}
