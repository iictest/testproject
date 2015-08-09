using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.BL
{
    class SetCashPayment_Request
    {
        public string terminalID { set; get; }
        public string transactionId { set; get; }
        public string pan { set; get; }
        public string merchant { set; get; }
        public string amount { set; get; }
    }

    class SetCashPayment_Response
    {
        public string terminalID { set; get; }
        public string transactionId { set; get; }
        public string pan { set; get; }
        public string merchant { set; get; }
        public string amount { set; get; }
    }
}
