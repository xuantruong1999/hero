using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hero.Model
{
    public class InforNameSpace
    {
        public InforNameSpace()
        {
            ListClass = new List<InforClass>();
        }
        public string nameSpace { get; set; }
        public List<InforClass> ListClass { get; set;}
        public string path { get; set; }
    }
}
