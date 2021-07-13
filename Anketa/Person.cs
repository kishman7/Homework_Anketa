using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anketa
{
    public class Person
    {
        public DateTime Age { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        //public string Country { get; set; }
        public int  Country { set; get; }
        public string Gender { get; set; }
        public string OtherHobby { get; set; }

        public List<string> hobby = new List<string>();

    }
}
