using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSQL
{
    public class TransactionModel
    {
        public int ID { get; set; }
        public int customerID { get; set; }
        public int productID { get; set; }
        public int amount { get; set; }
        public int totalPrice { get; set; }
        public string productName { get; set;}
    }
}
