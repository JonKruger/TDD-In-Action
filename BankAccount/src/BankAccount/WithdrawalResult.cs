using System;

namespace BankAccount
{
    public class WithdrawalResult
    {
        public bool WithdrawalSuccessful { get; set; }
        public bool WithdrawalFailedBecauseOfInsufficientFunds { get; set; }
        public bool WithdrawalFailedBecauseAccountIsClosed { get; set; }
        public bool WithdrawalFailedBecauseAmountWasNegative { get; set; }
    }
}