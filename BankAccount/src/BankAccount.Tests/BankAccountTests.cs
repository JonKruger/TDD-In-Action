using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Should.Extensions.AssertExtensions;

namespace BankAccount.Tests
{
    [TestFixture]
    public class When_depositing_money_into_an_account : Specification
    {
        private Account _account;

        protected override void Establish_context()
        {
            _account = new Account();
        }

        protected override void Because_of()
        {
            _account.Deposit(1.23m);
        }

        [Test]
        public void Should_add_the_specified_money_into_the_account()
        {
            _account.Balance.ShouldEqual(1.23m);
        }
    }

    [TestFixture]
    public class When_withdrawing_money_from_an_account_and_there_is_enough_money_in_the_account_to_satisfy_the_withdrawal : Specification
    {
        private Account _account;
        private WithdrawalResult _result;

        protected override void Establish_context()
        {
            _account = new Account();
            _account.Deposit(20);
        }

        protected override void Because_of()
        {
            _result = _account.Withdraw(1.50m);
        }

        [Test]
        public void should_withdraw_the_specified_amount_from_the_account()
        {
            _account.Balance.ShouldEqual(18.50m);
        }

        [Test]
        public void should_specify_that_the_withdrawal_was_successful()
        {
            _result.WithdrawalSuccessful.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class When_withdrawing_money_from_an_account_and_there_is_not_enough_money_in_the_account_to_satisfy_the_withdrawal : Specification
    {
        private Account _account;
        private WithdrawalResult _result;

        protected override void Establish_context()
        {
            _account = new Account();
            _account.Deposit(20);
        }

        protected override void Because_of()
        {
            _result = _account.Withdraw(30m);
        }

        [Test]
        public void should_not_withdraw_any_money_from_the_account()
        {
            _account.Balance.ShouldEqual(20);
        }

        [Test]
        public void should_specify_that_the_withdrawal_was_unsuccessful_because_there_was_insufficient_funds()
        {
            _result.WithdrawalSuccessful.ShouldBeFalse();
            _result.WithdrawalFailedBecauseOfInsufficientFunds.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class When_depositing_annual_interest_into_the_account : Specification
    {
        private Account _account;

        protected override void Establish_context()
        {
            _account = new Account();
            _account.Deposit(20m);
        }

        protected override void Because_of()
        {
            _account.DepositAnnualInterest(.07327m);
        }

        [Test]
        public void should_multiply_the_bank_account_by_the_annual_percentage_rate_and_add_that_amount_into_the_account()
        {
            _account.Balance.ShouldEqual(21.47m); // 20 + (20 * .07327)
        }
    }

    [TestFixture]
    public class When_attempting_to_close_an_account_and_the_account_balance_is_zero : Specification
    {
        private Account _account;
        private CloseAccountResult _result;

        protected override void Establish_context()
        {
            _account = new Account();
        }

        protected override void Because_of()
        {
            _result = _account.Close();
        }

        [Test]
        public void should_mark_the_account_as_closed()
        {
            _account.IsClosed.ShouldBeTrue();
        }

        [Test]
        public void should_notify_the_user_that_the_account_was_successfully_closed()
        {
            _result.AccountClosed.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class When_attempting_to_close_an_account_and_the_account_balance_is_not_zero : Specification
    {
        private Account _account;
        private CloseAccountResult _result;

        protected override void Establish_context()
        {
            _account = new Account();
            _account.Deposit(20m);
        }

        protected override void Because_of()
        {
            _result = _account.Close();
        }

        [Test]
        public void Should_not_mark_the_account_as_closed()
        {
            _account.IsClosed.ShouldBeFalse();
        }
        
        [Test]
        public void should_notify_the_user_that_the_account_could_not_be_closed_because_the_account_balance_was_not_zero()
        {
            _result.AccountNotClosedBecauseAccountIsNotEmpty.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class When_the_user_attempts_to_deposit_money_into_a_closed_account : Specification
    {
        private Account _account;
        private DepositResult _result;

        protected override void Establish_context()
        {
            base.Establish_context();

            _account = new Account();
            _account.Close();
        }

        protected override void Because_of()
        {
            _result = _account.Deposit(3m);
        }

        [Test]
        public void should_not_deposit_anything_into_the_account()
        {
            _account.Balance.ShouldEqual(0);
        }

        [Test]
        public void should_notify_the_user_that_the_they_could_not_deposit_into_a_closed_account()
        {
            _result.DepositFailedBecauseAccountIsClosed.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class When_the_user_attempts_to_withdraw_money_from_a_closed_account : Specification
    {
        private Account _account;
        private WithdrawalResult _result;

        protected override void Establish_context()
        {
            base.Establish_context();

            _account = new Account();
            _account.Close();
        }

        protected override void Because_of()
        {
            _result = _account.Withdraw(3m);
        }

        [Test]
        public void should_not_withdraw_anything_into_the_account()
        {
            _account.Balance.ShouldEqual(0);
        }

        [Test]
        public void should_notify_the_user_that_the_they_could_not_deposit_into_a_closed_account()
        {
            _result.WithdrawalFailedBecauseAccountIsClosed.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class When_attempting_to_deposit_a_negative_amount : Specification
    {
        private Account _account;
        private DepositResult _result;

        protected override void Establish_context()
        {
            base.Establish_context();

            _account = new Account();
            _account.Deposit(40m);
        }

        protected override void Because_of()
        {
            _result = _account.Deposit(-2m);
        }

        [Test]
        public void should_not_change_the_balance()
        {
            _account.Balance.ShouldEqual(40m);
        }

        [Test]
        public void should_notify_the_user_that_you_cannot_deposit_a_negative_amount()
        {
            _result.DepositFailedBecauseAmountWasNegative.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class When_attempting_to_withdraw_a_negative_amount : Specification
    {
        private Account _account;
        private WithdrawalResult _result;

        protected override void Establish_context()
        {
            base.Establish_context();

            _account = new Account();
            _account.Deposit(40m);
        }

        protected override void Because_of()
        {
            _result = _account.Withdraw(-2m);
        }

        [Test]
        public void should_not_change_the_balance()
        {
            _account.Balance.ShouldEqual(40m);
        }

        [Test]
        public void should_notify_the_user_that_you_cannot_withdraw_a_negative_amount()
        {
            _result.WithdrawalFailedBecauseAmountWasNegative.ShouldBeTrue();
        }
    }

}
