using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hero.Definition;
using Hero.Model;

namespace Hero
{
    class Program
    {
        public static void Main(string[] args)
        {
            string path = @"D:\Source\ReplacementService\DiAutomotiveSolutions\Common";
            string pathOfResult = @"C:\Users\truongnguyen\Documents\CSV\ReplacementService.csv";
            var results = ReflectionCSharp.GetAllDirectories(path); 
            List<string> files = new List<string>();
            List<InforNameSpace> ListNameSapce = new List<InforNameSpace>();
            foreach(string item in results)
            {
                files = files.Concat(ReflectionCSharp.GetAllFiles(item)).ToList();
            }
            if(files.Count != 0)
            {
                //foreach (string file in files)
                //{
                    var inForNameSpace = ReflectionCSharp.CrawlFile(@"D:\Source\ReplacementService\DiAutomotiveSolutions\Common\DiReplacement.Services\AutoSendLogic\AutoSendDocument.cs");
                    ListNameSapce.Add(inForNameSpace);
                //}

                ReflectionCSharp.ExportCSV(ListNameSapce, pathOfResult);
            }
        }
    }
}
