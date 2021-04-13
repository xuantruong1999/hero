using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hero.Model
{
    public class InforMethod
    {
        public InforMethod()
        {
            ListError = new List<InforError>();
        }
        public string methodName { get; set; }
        public List<InforError> ListError { get; set; }
        public string Number { get; set; }
    }
}
