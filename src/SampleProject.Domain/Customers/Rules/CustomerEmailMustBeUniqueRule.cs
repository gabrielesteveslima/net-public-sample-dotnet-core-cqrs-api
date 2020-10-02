﻿namespace SampleProject.Domain.Customers.Rules
{
    using SeedWork;

    public class CustomerEmailMustBeUniqueRule : IBusinessRule
    {
        private readonly ICustomerUniquenessChecker _customerUniquenessChecker;

        private readonly string _email;

        public CustomerEmailMustBeUniqueRule(
            ICustomerUniquenessChecker customerUniquenessChecker,
            string email)
        {
            _customerUniquenessChecker = customerUniquenessChecker;
            _email = email;
        }

        public bool IsBroken()
        {
            return !_customerUniquenessChecker.IsUnique(_email);
        }

        public string Message => "Customer with this email already exists.";
    }
}