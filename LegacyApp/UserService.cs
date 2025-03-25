using System;

namespace LegacyApp
{
    public class UserService
    {
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (!DoPreChecks(firstName, lastName, email, dateOfBirth))
                return false;
            var user = ObtainUser(firstName, lastName, email, dateOfBirth, clientId);
            if (!user.HasProperLimit())
                return false;
            UserDataAccess.AddUser(user);
            return true;
        }

        private static bool DoPreChecks(string firstName, string lastName, string email, DateTime dateOfBirth) =>
            NameOrLastnameIsNotNull(firstName, lastName) && EmailIsValid(email) && CheckAge(dateOfBirth);

        private static User ObtainUser(string firstName, string lastName, string email, DateTime dateOfBirth,
            int clientId)
        {
            var clientRepository = new ClientRepository();
            var client = clientRepository.GetById(clientId);

            var user = new User(client, dateOfBirth, email, firstName, lastName, client.Type);
            return user;
        }

        private static bool CheckAge(DateTime dateOfBirth)
        {
            var now = DateTime.Now;
            var age = now.Year - dateOfBirth.Year;

            age = CorrectAgeEdgeCases(dateOfBirth, now, age);

            if (!IsAdult(age))
                return false;
            return true;
        }


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