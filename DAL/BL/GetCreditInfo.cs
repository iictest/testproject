using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.BL
{
   public class GetCreditInfo_Request
    {
        public string terminalID { set; get; }
        public string merchantID { set; get; }
        public string pan { set; get; }
        public string traceID { set; get; }
    }

   public class GetCreditInfo_Response
   {
       public string transactionId { set; get; }
       public string name { set; get; }
       public string family { set; get; }
       public string validation { set; get; }
   }
}
