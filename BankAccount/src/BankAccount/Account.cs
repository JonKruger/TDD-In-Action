using System;

namespace BankAccount
{
    public class Account
    {
        public const string CannotDepositNegativeAmountErrorMessage =
            "You cannot deposit a negative amount into an account.";
        public const string NotEnoughMoneyToCoverTheWithdrawalErrorMessage = 
            "There is not enough money to cover the withdrawal.";
        public const string CannotWithdrawNegativeAmountErrorMessage = 
            "You cannot withdraw a negative amount from an account.";
        public const string CannotCloseAccountThatHasMoneyInItErrorMessage =
            "The account cannot be closed because there is money in the account.";
        public const string CannotDepositMoneyIntoClosedAccountErrorMessage =
            "You cannot deposit money into a closed account.";
        public const string CannotWithdrawMoneyFromClosedAccountErrorMessage =
            "You cannot withdraw money from a closed account.";

        private decimal _balance;
        private bool _isClosed;

        public decimal Balance
        {
            get { return _balance; }
        }

        public bool IsClosed
        {
            get { return _isClosed; }
        }

        public void Deposit(decimal amount)
        {
            if (_isClosed)
                throw new InvalidOperationException(CannotDepositMoneyIntoClosedAccountErrorMessage);
            if (amount < 0)
                throw new InvalidOperationException(CannotDepositNegativeAmountErrorMessage);

            amount = Math.Round(amount, 2);

            _balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            if (_isClosed)
                throw new InvalidOperationException(CannotWithdrawMoneyFromClosedAccountErrorMessage);
            if (amount < 0)
                throw new InvalidOperationException(CannotWithdrawNegativeAmountErrorMessage);
            if (amount > _balance)
                throw new InvalidOperationException(NotEnoughMoneyToCoverTheWithdrawalErrorMessage);

            amount = Math.Round(amount, 2);

            _balance -= amount;
        }

        public void Close()
        {
            if (_balance > 0)
                throw new InvalidOperationException(CannotCloseAccountThatHasMoneyInItErrorMessage);
         
            _isClosed = true;
        }
    }
}