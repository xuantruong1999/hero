using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hero.Model
{
    public class CSVLine
    {
        [DisplayName("Solution")]
        public string Solution { get; set; }
        [DisplayName("Solution Number")]
        public string SolutionNumber{ get; set; }
        [DisplayName("Project Name")]
        public string ProjectName { get; set; }
        [DisplayName("Project Number")]
        public string ProjectNumber { get; set; }
        [DisplayName("NameSpace")]
        public string NameSpace { get; set; }
        [DisplayName("Class Name")]
        public string ClassName { get; set; }
        [DisplayName("Class Number")]
        public string ClassNumber { get; set; }
        [DisplayName("Method Name")]
        public string MethodName { get; set; }
        [DisplayName("Method Number")]
        public string MethodNumber { get; set; }
        [DisplayName("Error")]
        public string Error { get; set; }
        [DisplayName("Error Number")]
        public string ErrorNumber { get; set; }
        [DisplayName("Severity Level")]
        public string SeverityLevel { get; set; }
        [DisplayName("Path")]
        public string Path { get; set; }
        

    }
}
