using System;

namespace LegacyApp
{
    public class User
    {
        public object Client { get; internal set; }
        public DateTime DateOfBirth { get; internal set; }
        public string EmailAddress { get; internal set; }
        public string FirstName { get; internal set; }
        public string LastName { get; internal set; }
        public bool HasCreditLimit { get; internal set; }
        public int CreditLimit { get; internal set; }

        public User(object client, DateTime dateOfBirth, string emailAddress, string firstName, string lastName,
            string clientType)
        {
            Client = client;
            DateOfBirth = dateOfBirth;
            EmailAddress = emailAddress;
            FirstName = firstName;
            LastName = lastName;

            SectClientType(clientType);
        }

        private void SectClientType(string clientType)
        {
            switch (clientType)
            {
                case "VeryImportantClient":
                    HasCreditLimit = false;
                    break;
                case "ImportantClient":
                {
                    CalculateLimit(true);
                    break;
                }
                default:
                {
                    HasCreditLimit = true;
                    CalculateLimit(false);
                    break;
                }
            }
        }

        private void CalculateLimit(bool isImportant)
        {
            using var userCreditService = new UserCreditService();
            var creditLimit = userCreditService.GetCreditLimit(LastName, DateOfBirth);
            if (isImportant)
                creditLimit *= 2;
            CreditLimit = creditLimit;
        }

        public bool HasProperLimit() =>
            !HasCreditLimit || CreditLimit >= 500;
    }
}