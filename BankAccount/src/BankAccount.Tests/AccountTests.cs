using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Should;
using Should.Extensions.AssertExtensions;

namespace BankAccount.Tests
{
    public abstract class AccountSpecification : Specification
    {
        protected Account Given_a_bank_account_that_has_money_in_it()
        {
            var account = new Account();
            account.Deposit(20m);
            return account;
        }

        protected Account Given_an_account_where_the_balance_is_0()
        {
            return new Account();
        }

        protected Account Given_a_closed_account()
        {
            var account = new Account();
            account.Close();
            return account;
        }
    }

    [TestFixture]
    public class When_the_user_deposits_money_into_an_account : Specification
    {
        private Account _account;

        protected override void Establish_context()
        {
            base.Establish_context();

            _account = new Account();
        }

        protected override void Because_of()
        {
            _account.Deposit(2m);
        }

        [Test]
        public void Should_add_the_amount_to_the_existing_balance()
        {
            _account.Balance.ShouldEqual(2m);
        }
    }

    [TestFixture]
    public class When_the_user_attemps_to_deposit_a_negative_amount_of_money_into_an_account : Specification
    {
        private Account _account;
        private Exception _exception;

        protected override void Establish_context()
        {
            base.Establish_context();

            _account = new Account();
        }

        protected override void Because_of()
        {
            _exception = Record.Exception(() => _account.Deposit(-2m));
        }

        [Test]
        public void Should_not_change_the_balance()
        {
            _account.Balance.ShouldEqual(0);
        }

        [Test]
        public void Should_return_an_error_that_says___You_cannot_deposit_a_negative_amount()
        {
            _exception.Message.ShouldEqual(Account.CannotDepositNegativeAmountErrorMessage);
        }
    }

    [TestFixture]
    public class When_the_user_withdraws_money_from_the_account_and_there_is_enough_money_to_cover_the_withdrawal : AccountSpecification
    {
        private Account _account;

        protected override void Establish_context()
        {
            _account = Given_a_bank_account_that_has_money_in_it();
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
    public class When_the_user_withdraws_money_from_the_account_and_there_is_not_enough_money_to_cover_the_withdrawal : AccountSpecification
    {
        private Account _account;
        private Exception _exception;

        protected override void Establish_context()
        {
            _account = Given_a_bank_account_that_has_money_in_it();
        }

        protected override void Because_of()
        {
            _exception = Record.Exception(() => _account.Withdraw(100m));
        }

        [Test]
        public void Should_not_change_the_balance()
        {
            _account.Balance.ShouldEqual(20m);
        }

        [Test]
        public void Should_return_an_error_that_says___There_is_not_enough_money_in_the_account_for_the_withdrawal()
        {
            _exception.Message.ShouldEqual(Account.NotEnoughMoneyForWithdrawalErrorMessage);
        }
    }

    [TestFixture]
    public class When_the_user_attemps_to_withdraw_a_negative_amount_of_money_from_an_account : AccountSpecification
    {
        private Account _account;
        private Exception _exception;

        protected override void Establish_context()
        {
            _account = Given_a_bank_account_that_has_money_in_it();
        }

        protected override void Because_of()
        {
            _exception = Record.Exception(() => _account.Withdraw(-3m));
        }

        [Test]
        public void Should_not_change_the_balance()
        {
            _account.Balance.ShouldEqual(20m);
        }

        [Test]
        public void Should_return_an_error_that_says___You_cannot_withdraw_a_negative_amount__()
        {
            _exception.Message.ShouldEqual(Account.CannotWithdrawNegativeAmountErrorMessage);
        }
    }

    [TestFixture]
    public class When_the_user_closes_the_account : AccountSpecification
    {
        private Account _account;

        protected override void Establish_context()
        {
            _account = Given_an_account_where_the_balance_is_0();
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
    public class When_the_user_attempts_to_close_the_account : AccountSpecification
    {
        private Account _account;
        private Exception _exception;

        protected override void Establish_context()
        {
            _account = Given_a_bank_account_that_has_money_in_it();
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
        public void Should_not_change_the_balance()
        {
            _account.Balance.ShouldEqual(20m);
        }

        [Test]
        public void Should_return_error_that_says___You_cannot_close_an_account_that_has_money_in_it()
        {
            _exception.Message.ShouldEqual(Account.CannotCloseAccountThatHasMoneyInItErrorMessage);
        }
    }

    [TestFixture]
    public class When_the_user_attempts_to_deposit_money_into_a_closed_account : AccountSpecification
    {
        private Account _account;
        private Exception _exception;

        protected override void Establish_context()
        {
            _account = Given_a_closed_account();
        }

        protected override void Because_of()
        {
            _exception = Record.Exception(() => _account.Deposit(3m));
        }

        [Test]
        public void Should_not_change_the_balance()
        {
            _account.Balance.ShouldEqual(0m);
        }

        [Test]
        public void Should_return_error_that_says___You_cannot_deposit_money_into_a_closed_account()
        {
            _exception.Message.ShouldEqual(Account.CannotDepositMoneyIntoClosedAccountErrorMessage);
        }
    }

    [TestFixture]
    public class When_the_user_attempts_to_withdraw_money_from_a_closed_account : AccountSpecification
    {
        private Account _account;
        private Exception _exception;

        protected override void Establish_context()
        {
            _account = Given_a_closed_account();
        }

        protected override void Because_of()
        {
            _exception = Record.Exception(() => _account.Withdraw(2m));
        }

        [Test]
        public void Should_not_change_the_balance()
        {
            _account.Balance.ShouldEqual(0m);
        }

        [Test]
        public void Should_return_error_that_says___You_cannot_withdraw_money_from_a_closed_account()
        {
            _exception.Message.ShouldEqual(Account.CannotWithdrawMoneyFromClosedAccountErrorMessage);
        }
    }
}
