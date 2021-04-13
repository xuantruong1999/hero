using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Text.RegularExpressions;
using Hero.Model;
using CsvHelper;

namespace Hero.Definition 
{
    public class ReflectionCSharp
    {
        public static List<string> GetAllDirectories(string rootPath)
        {
            try
            {
                if (Directory.Exists(rootPath))
                {
                    List<string> directories = new List<string>();
                    directories.Add(rootPath);
                    directories = Directory.EnumerateDirectories(rootPath).Where(x => Path.GetFileName(x) != "bin" && Path.GetFileName(x) != "obj").ToList();
                    List<string> temps = new List<string>(); 
                    foreach(string item in directories)
                    {
                       temps = temps.Concat(LoadSubDirectories(item)).ToList();
                    }
                    if(temps.Any())
                    {
                        directories = directories.Concat(temps).ToList();
                    }
                    return directories;
                }
                else
                {
                    Console.WriteLine("The directory is not exist");
                    return null;
                }
            }catch(Exception ex)
            {
                throw ex;
            }
           
        }
        private static List<string> LoadSubDirectories(string subPath)
        {
            try
            {
                var directories = Directory.EnumerateDirectories(subPath).ToList();
                foreach(string item in directories) 
                {
                    LoadSubDirectories(item);
                }
                return directories;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<string> GetAllFiles(string pathDiretory)
        {
            try
            {
                if (Directory.Exists(pathDiretory))
                {
                    var files = Directory.EnumerateFiles(pathDiretory, "*.cs", SearchOption.AllDirectories).ToList();
                    return files.ToList();
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public static InforNameSpace CrawlFile(string pathFile)
        {
            try
            {
                //define regex pattern
                Regex partternNameSpace = new Regex(@"namespace\s[\w\.]*");
                Regex comments = new Regex(@"(/\*([^*]|[\r\n]|(\*+([^*/]|[\r\n])))*\*+/)|(//.*)");
                string classNameRegex = @"class\s+?([\w.]+)";
                string methodNameRegex = @"(public|private|protected)\s*(?:static)?\s*[<datatype>\w]*\s+([\w]*)\s*\((\)|([\w\s\,\s*\)])*)";
                Regex blockErrorPattern = new Regex(@"(ReplacementLog|DiLogger.[\w]+).Error\s?\(([^\;]+)\s?\;");
                Regex blockClass = new Regex(@"\b(class)(.+?)(?:(?!(public|private|protected|internal|partial)?\s+class).)*", RegexOptions.Singleline);
                Regex blockMethod = new Regex(@"(public|private|protected)\s*(?:static)?\s*[<datatype>\w]*\s+([\w]*)\s*\((\)|([\w\s\,\s*\)])*).*?(?:(?!\}\s*(public|private|protected)).)*");
                Regex region = new Regex(@"\#.+?(?:(?!\\r\\n).)*");
                //
                string text = System.IO.File.ReadAllText(pathFile);
                string textFilter = text;
                InforNameSpace rowExcel = new InforNameSpace();

                //
                MatchCollection matchComment = comments.Matches(text);
                // remove comment
                if (matchComment.Count != 0)
                {
                    textFilter = Regex.Replace(text, @"(/\*([^*]|[\r\n]|(\*+([^*/]|[\r\n])))*\*+/)|(//.*)", " "); 
                }
                //get namespace
                MatchCollection nameSpaceMatches = partternNameSpace.Matches(textFilter);      
                if (nameSpaceMatches.Count != 0)
                {   
                    var temps = nameSpaceMatches[0];
                    rowExcel.nameSpace = temps.Value.Split(' ')[1];
                }
                else
                {
                    rowExcel.nameSpace = "No NameSpace";
                }
                int classCounting = 1;
                int methodCounting = 1;
                int errorCounting = 1;
                // get class name
                MatchCollection matchesBlockClass = blockClass.Matches(textFilter);
                if (matchesBlockClass.Count != 0)
                {
                    InforClass classInfor = new InforClass();
                    foreach (Match item in matchesBlockClass)
                    {

                        var itemReplacemented = region.Replace(item.Value, " ");
                        itemReplacemented = Regex.Replace(itemReplacemented, @"\[.*\]\s*(?:(?!(public|private|protected|internal|partial)).)*", " ");
                        itemReplacemented = itemReplacemented.Replace("\r\n", " ");
                        classInfor.ClassName = Regex.Match(item.Value, classNameRegex).Value;
                        MatchCollection matchesListMethods = blockMethod.Matches(itemReplacemented);
                        if (matchesListMethods.Count != 0)
                        {
                            foreach(Match m in matchesListMethods)
                            {
                                InforMethod method = new InforMethod();
                                
                                method.methodName = Regex.Match(m.Value, methodNameRegex).Value;
                                MatchCollection ListErrors = blockErrorPattern.Matches(m.Value);
                                if(ListErrors.Count != 0)
                                {
                                    foreach (Match e in ListErrors)
                                    {
                                        InforError error = new InforError();
                                        error.Error = e.Value;
                                        error.Number = (errorCounting++).ToString("\\'000");
                                        method.ListError.Add(error);
                                    }
                                }
                                
                                method.Number = ListErrors.Count != 0 ? (methodCounting++).ToString("\\'000") : string.Empty;
                               
                                errorCounting = 1;
                                classInfor.ListMethod.Add(method);
                            }
                        }
                        methodCounting = 1;
                        classInfor.Number = (classCounting++).ToString("\\'000");
                    }
                    rowExcel.ListClass.Add(classInfor);
                }
                rowExcel.path = pathFile;
                return rowExcel;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public static void ExportCSV(List<InforNameSpace> inforNameSpace, string path)
        {
            //string header = "Solution,SolutionNumber,ProjectName,ProjectNumber,NameSpace,ClassName,ClassNumber,MethodName,MethodNumber,Error,ErrorNumber,Severity Level,Path";
            List<CSVLine> table = new List<CSVLine>();
            foreach (InforNameSpace i in inforNameSpace)
            {
                var line = i.ListClass.SelectMany(c => c.ListMethod.SelectMany(m => m.ListError.Select(e =>
                    new CSVLine()
                    {
                        Solution = "",
                        SolutionNumber = "",
                        ProjectName = "DiAutomotive",
                        ProjectNumber = "Manual fill",
                        NameSpace = i.nameSpace,
                        ClassName = m.Number == "'001" ? c.ClassName : string.Empty ,
                        ClassNumber = c.Number,
                        MethodName = e.Number == "'001" ? m.methodName : string.Empty,
                        MethodNumber = m.Number,
                        Error = e.Error,
                        ErrorNumber = e.Number,
                        SeverityLevel = "4",
                        Path = i.path
                    }
                ))).ToList();
                table.AddRange(line);
            }
            using (var writer = new StreamWriter(path))
            using (var csvWriter = new CsvWriter(writer, System.Globalization.CultureInfo.CurrentCulture, true))
            {
                csvWriter.WriteHeader<CSVLine>();
                csvWriter.WriteRecords(table);
                writer.Flush();
            }
        }
    }
}
