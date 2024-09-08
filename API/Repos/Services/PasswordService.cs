using API.Repos.Interfaces;

public class PasswordGenerator : IPasswordGenerator
{
    private const string AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public string GeneratePassword(int length)
    {
        Random random = new Random();
        var passwordChars = new char[length];

        for (int i = 0; i < length; i++)
        {
            int index = random.Next(AllowedChars.Length);
            passwordChars[i] = AllowedChars[index];
        }

        return new string(passwordChars);
    }
}
