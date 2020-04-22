using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace FoodPort_Payment
{
    [ServiceContract]
    interface IBankService
    {
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        int MakePayment(Transaction trans);
    }
}
