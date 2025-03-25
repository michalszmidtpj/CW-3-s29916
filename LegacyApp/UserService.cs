using System;

namespace LegacyApp
{
    public class UserService
    {
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (!NameOrLastnameIsNotNull(firstName, lastName)) 
                return false;
            if (!EmailIsValid(email)) 
                return false;

            var now = DateTime.Now;
            var age = now.Year - dateOfBirth.Year;

            age = CorrectAgeEdgeCases(dateOfBirth, now, age);

            if (!IsAdult(age)) 
                return false;

            var clientRepository = new ClientRepository();
            var client = clientRepository.GetById(clientId);

            var user = new User(client, dateOfBirth, email, firstName, lastName, client.Type);

            if (!HasProperLimit(user)) 
                return false;

            UserDataAccess.AddUser(user);
            return true;
        }

        private static bool HasProperLimit(User user) =>
            !user.HasCreditLimit || user.CreditLimit >= 500;


        private static int CorrectAgeEdgeCases(DateTime dateOfBirth, DateTime now, int age)
        {
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)) age--;
            return age;
        }

        private static bool IsAdult(int age) =>
            age >= 21;

        private static bool EmailIsValid(string email) =>
            email.Contains('@') || email.Contains('.');

        private static bool NameOrLastnameIsNotNull(string firstName, string lastName) =>
            !string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName);
    }
}