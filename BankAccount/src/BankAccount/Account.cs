using System;

namespace BankAccount
{
    public class Account
    {
        public decimal Balance { get; private set; }
        public bool IsClosed { get; private set; }

        public DepositResult Deposit(decimal amount)
        {
            if (IsClosed)
                return new DepositResult { DepositFailedBecauseAccountIsClosed = true };
            if (amount < 0)
                return new DepositResult {DepositFailedBecauseAmountWasNegative = true};

            Balance += amount;
            return new DepositResult { DepositSucceeded = true };
        }

        public WithdrawalResult Withdraw(decimal amount)
        {
            if (IsClosed)
                return new WithdrawalResult { WithdrawalFailedBecauseAccountIsClosed = true };
            if (amount < 0)
                return new WithdrawalResult {WithdrawalFailedBecauseAmountWasNegative = true};
            if (Balance < amount)
                return new WithdrawalResult { WithdrawalFailedBecauseOfInsufficientFunds = true };
            Balance -= amount;
            return new WithdrawalResult { WithdrawalSuccessful = true };
        }

        public void DepositAnnualInterest(decimal annualPercentageRate)
        {
            Balance += Math.Round(Balance * annualPercentageRate, 2);
        }

        public CloseAccountResult Close()
        {
            if (Balance == 0)
            {
                IsClosed = true;
                return new CloseAccountResult { AccountClosed = true };
            }
            else
                return new CloseAccountResult { AccountNotClosedBecauseAccountIsNotEmpty = true };
        }
    }

    public class DepositResult
    {
        public bool DepositSucceeded { get; set; }
        public bool DepositFailedBecauseAccountIsClosed { get; set; }
        public bool DepositFailedBecauseAmountWasNegative { get; set; }
    }

    public class CloseAccountResult
    {
        public bool AccountClosed { get; set; }
        public bool AccountNotClosedBecauseAccountIsNotEmpty { get; set; }
    }
}