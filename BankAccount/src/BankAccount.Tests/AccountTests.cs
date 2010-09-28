using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Should;
using Should.Extensions.AssertExtensions;

namespace BankAccount.Tests
{
    [TestFixture]
    public class When_depositing_money_into_a_bank_account : Specification
    {
        private Account _account;

        protected override void Establish_context()
        {
            base.Establish_context();

            _account = new Account();
        }

        protected override void Because_of()
        {
            _account.Deposit(1.23m);
        }

        [Test]
        public void Should_increase_the_balance_by_the_amount_of_the_deposit()
        {
            _account.Balance.ShouldEqual(1.23m);
        }
    }

    [TestFixture]
    public class When_the_user_attempts_to_deposit_a_negative_amount_into_the_account : Specification
    {
        private Account _account;

        protected override void Establish_context()
        {
            base.Establish_context();

            _account = new Account();
        }

        [Test]
        public void Should_return_an_error_that_says_You_cannot_deposit_a_negative_amount_into_an_account()
        {
            Record.Exception(() => _account.Deposit(-1m)).Message.ShouldEqual("You cannot deposit a negative amount into an account.");
        }
    }

    [TestFixture]
    public class When_the_user_attempts_to_deposit_an_amount_and_more_than_two_decimal_places_are_entered : Specification
    {
        private Account _account;

        protected override void Establish_context()
        {
            base.Establish_context();

            _account = new Account();
        }

        protected override void Because_of()
        {
            _account.Deposit(1.235m);
        }

        [Test]
        public void Should_round_the_deposit_amount_to_the_nearest_penny_before_depositing()
        {
            _account.Balance.ShouldEqual(1.24m);
        }
    }

    public abstract class When_withdrawing_money_from_an_account : Specification
    {
        protected Account _account;
        protected void Given_a_bank_account_that_has_money_in_it()
        {
            _account = new Account();
            _account.Deposit(20m);
        }
    }

    [TestFixture]
    public class When_withdrawing_money_from_the_account_and_there_is_enough_money_in_the_account_to_cover_the_withdrawal : When_withdrawing_money_from_an_account
    {
        protected override void Establish_context()
        {
            Given_a_bank_account_that_has_money_in_it();
        }

        protected override void Because_of()
        {
            _account.Withdraw(1.25m);
        }

        [Test]
        public void Should_reduce_the_balance_by_the_amount_of_the_withdrawal()
        {
            _account.Balance.ShouldEqual(18.75m);
        }
    }

    [TestFixture]
    public class When_withdrawing_money_from_the_account_and_there_is_not_enough_money_in_the_account_to_cover_the_withdrawal 
        : When_withdrawing_money_from_an_account
    {
        private Exception _exception;

        protected override void Establish_context()
        {
            Given_a_bank_account_that_has_money_in_it();
        }

        protected override void Because_of()
        {
            _exception = Record.Exception(() => _account.Withdraw(1111.25m));
        }

        [Test]
        public void Should_not_change_the_balance()
        {
            _account.Balance.ShouldEqual(20m);
        }

        [Test]
        public void Should_return_error_that_says_There_is_not_enough_money_to_cover_the_withdrawal()
        {
            _exception.Message.ShouldEqual(Account.NotEnoughMoneyToCoverTheWithdrawalErrorMessage);
        }
    }

    [TestFixture]
    public class When_the_user_attempts_to_withdraw_a_negative_amount_from_the_account 
        : When_withdrawing_money_from_an_account
    {
        private Exception _exception;

        protected override void Establish_context()
        {
            Given_a_bank_account_that_has_money_in_it();
        }

        protected override void Because_of()
        {
            _exception = Record.Exception(() => _account.Withdraw(-9m));
        }

        [Test]
        public void Should_not_change_the_balance()
        {
            _account.Balance.ShouldEqual(20m);
        }

        [Test]
        public void Should_return_an_error_that_says___You_cannot_withdraw_a_negative_amount_from_an_account()
        {
            _exception.Message.ShouldEqual(Account.CannotWithdrawNegativeAmountErrorMessage);
        }
    }

    [TestFixture]
    public class When_the_user_attempts_to_withdraw_an_amount_and_more_than_two_decimal_places_are_entered 
        : When_withdrawing_money_from_an_account
    {
        protected override void Establish_context()
        {
            Given_a_bank_account_that_has_money_in_it();
        }

        protected override void Because_of()
        {
            _account.Withdraw(1.995m);
        }

        [Test]
        public void Should_round_the_withdrawal_amount_to_the_nearest_penny_before_withdrawing()
        {
            _account.Balance.ShouldEqual(18m);
        }
    }

    [TestFixture]
    public class When_closing_an_account_and_the_balance_is_0 : Specification
    {
        private Account _account;

        protected override void Establish_context()
        {
            base.Establish_context();

            _account = new Account();
        }

        protected override void Because_of()
        {
            _account.Close();
        }

        [Test]
        public void Should_mark_the_account_as_closed()
        {
            _account.IsClosed.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class When_closing_an_account_and_the_balance_is_not_0 : Specification
    {
        private Account _account;
        private Exception _exception;

        protected override void Establish_context()
        {
            base.Establish_context();

            _account = new Account();
            _account.Deposit(1m);
        }

        protected override void Because_of()
        {
            _exception = Record.Exception(() => _account.Close());
        }

        [Test]
        public void Should_not_close_the_account()
        {
            _account.IsClosed.ShouldBeFalse();
        }

        [Test]
        public void Should_return_an_error_that_says___The_account_cannot_be_closed_because_there_is_money_in_the_account()
        {
            _exception.Message.ShouldEqual(Account.CannotCloseAccountThatHasMoneyInItErrorMessage);
        }
    }

    [TestFixture]
    public class When_the_user_attempts_to_deposit_money_into_a_closed_account : Specification
    {
        private Account _account;
        private Exception _exception;

        protected override void Establish_context()
        {
            base.Establish_context();

            _account = new Account();
            _account.Close();
        }

        protected override void Because_of()
        {
            _exception = Record.Exception(() => _account.Deposit(3m));
        }

        [Test]
        public void Should_return_an_error_that_says___You_cannot_deposit_money_into_a_closed_account__()
        {
            _exception.Message.ShouldEqual(Account.CannotDepositMoneyIntoClosedAccountErrorMessage);
        }
    }

    [TestFixture]
    public class When_the_user_attempts_to_withdraw_money_from_a_closed_account : Specification
    {
        private Account _account;
        private Exception _exception;

        protected override void Establish_context()
        {
            base.Establish_context();

            _account = new Account();
            _account.Close();
        }

        protected override void Because_of()
        {
            _exception = Record.Exception(() => _account.Withdraw(3m));
        }

        [Test]
        public void Should_return_an_error_that_says___You_cannot_withdraw_money_from_a_closed_account__()
        {
            _exception.Message.ShouldEqual(Account.CannotWithdrawMoneyFromClosedAccountErrorMessage);
        }
    }
}
