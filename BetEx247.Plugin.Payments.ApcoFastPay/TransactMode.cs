using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BetEx247.Plugin.Payments.ApcoFastPay
{
    public enum TransactMode : byte
    {       
        Purchase=1,
        Credit=2,
        Void=3,
        Authorization=4,
        Capture=5,
        VoidCredit=6,
        VoidCapture=7,
        VoidAuthorization=9,
        Verify=10,
        RepeatPurchase=11,
        PartialRefund=12,
        OriginalCredit=13,
        RepeatAuthorization=14
    }
}
