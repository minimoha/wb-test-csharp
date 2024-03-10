namespace TestApi.Utilities;

    public class SimpleValidator
    {
    public static bool IsValidPhoneNumber(string phoneNumber)
    {
        //string pattern = @"^(?:\+?234|0)?\d{10,13}$";
        string pattern = @"^\d{6,16}$";

        Regex regex = new Regex(pattern);

        return regex.IsMatch(phoneNumber);
    }

    public static bool IsValidEmail(string email)
    {
        string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

        Regex regex = new Regex(pattern);

        return regex.IsMatch(email);
    }

    public static bool IsValidPassword(string password)
    {
        string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$";

        Regex regex = new Regex(pattern);

        return regex.IsMatch(password);
    }
}

