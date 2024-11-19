using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{

        public class Student
        {
            public int STT { get; set; }
            public int ID { get; set; }
            public string NameStudent { get; set; }
            public string Class { get; set; }
            public string Parents { get; set; }
            public string ParentEmail { get; set; }
        }
        public class ClassStudent
        {
            public string Class { get; set; }
        }
    public class studentRolClall {
        public int STT { get; set; }
        public int ID { get; set; }
        public string Class { get; set; }
        public string NameStudent { get; set; }
        public DateTime Date { get; set; }
        public bool status {  get; set; }
    }
    
}
