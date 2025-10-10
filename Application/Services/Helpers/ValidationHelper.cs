using System;
using System.Net;
using System.Text.RegularExpressions;

namespace Application.Services.Helpers
{
    public static class ValidationHelper
    {
        // si llegamos dsp hacer un campo para area y uno para numero
        public static bool PhoneNumberValidator(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return true; 

            phone = phone
                .Replace("+54", "")
                .Replace("+549", "")
                .Replace("-", "")
                .Replace("_", "")
                .Replace(".", "")
                .Replace("(", "")
                .Replace(")", "")
                .Replace(" ", "")
                .Trim();

            if (!Regex.IsMatch(phone, @"^\d+$"))
                return false;

            foreach (var code in PhoneRules.AreaCodes)
            {
                string areaCode = code.Key;
                int expectedLength = code.Value;

                if (phone.StartsWith(areaCode))
                {
                    var localNumber = phone.Substring(areaCode.Length);
                    return localNumber.Length == expectedLength;
                }
            }
            return false;
        }
        public static bool EmailValidator(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static bool DNIValidator(string dni)
        {

            if (string.IsNullOrWhiteSpace(dni))
                return false;

            dni = Regex.Replace(dni, @"[.\s-]", "");

            if (!Regex.IsMatch(dni, @"^\d+$") || dni.Length < 7 || dni.Length > 8)
                return false;

            int valor = int.Parse(dni);
            if (valor < 1000000 || valor > 99999999)
                return false;

            return true;
        }

        public static bool BirthDateValidator(DateOnly? birthDate)
        {
            if (birthDate is null)
                return true;

            DateOnly today = DateOnly.FromDateTime(DateTime.Today);

            if (birthDate > today)
                return false;

            int age = today.Year - birthDate.Value.Year;

            if (birthDate > today.AddYears(-age))
                age--;

            if (age > 116) // Ethel Caterham (UK), la persona más longeva registrada viva
                return false;

            return true;
        }

        public static bool PasswordValidator(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 9)
                return false;

            if (!Regex.IsMatch(password, @"[A-Z]"))
                return false;

            if (!Regex.IsMatch(password, @"[a-z]"))
                return false;

            if (!Regex.IsMatch(password, @"\d"))
                return false;

            if (!Regex.IsMatch(password, @"[!@#$%^&*()_+\-=\[\]{}|;:',.<>/?]"))
                return false;

            return true;
        }


}
}
