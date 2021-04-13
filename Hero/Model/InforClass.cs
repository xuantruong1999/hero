using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hero.Model
{
    public class InforClass
    {
        public InforClass()
        {
            ListMethod = new List<InforMethod>();
        }
        public string ClassName { get; set; }
        public List<InforMethod>ListMethod { get; set; }
        public string Number { get; set; }
    }
}
