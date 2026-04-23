using Abstractions;

namespace Services
{
    public class PlainTextPasswordService : IPasswordService
    {
        public string Protect(string plainTextPassword)
        {
            return plainTextPassword ?? string.Empty;
        }
    }
}
