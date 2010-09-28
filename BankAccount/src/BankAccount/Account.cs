using System;

namespace BankAccount
{
    public class Account
    {
        public const string CannotDepositNegativeAmountErrorMessage = 
            "You cannot deposit a negative amount.";
        public const string CannotWithdrawNegativeAmountErrorMessage = 
            "You cannot withdraw a negative amount.";
        public const string NotEnoughMoneyForWithdrawalErrorMessage =
            "There is not enough money in the account for the withdrawal.";
        public const string CannotCloseAccountThatHasMoneyInItErrorMessage =
            "You cannot close an account that has money in it.";
        public const string CannotDepositMoneyIntoClosedAccountErrorMessage =
            "You cannot deposit money into a closed account.";
        public const string CannotWithdrawMoneyFromClosedAccountErrorMessage =
            "You cannot withdraw money from a closed account.";

        public decimal Balance { get; private set; }
        public bool IsClosed { get; private set; }
        
        public void Deposit(decimal amount)
        {
            if (IsClosed)
                throw new InvalidOperationException(CannotDepositMoneyIntoClosedAccountErrorMessage);
            if (amount < 0)
                throw new InvalidOperationException(CannotDepositNegativeAmountErrorMessage);
            Balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            if (IsClosed)
                throw new InvalidOperationException(CannotWithdrawMoneyFromClosedAccountErrorMessage);
            if (amount < 0)
                throw new InvalidOperationException(CannotWithdrawNegativeAmountErrorMessage);
            if (amount > Balance)
                throw new InvalidOperationException(NotEnoughMoneyForWithdrawalErrorMessage);
            Balance -= amount;
        }

        public void Close()
        {
            if (Balance > 0)
                throw new InvalidOperationException(CannotCloseAccountThatHasMoneyInItErrorMessage);
            IsClosed = true;
        }
    }
}