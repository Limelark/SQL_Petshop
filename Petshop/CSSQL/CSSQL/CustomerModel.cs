using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CSSQL
{
    public class CustomerModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Zipcode { get; set; }
        public string Password { get; set; }
    }
}
