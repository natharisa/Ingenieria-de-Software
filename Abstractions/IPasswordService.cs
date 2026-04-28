namespace Abstractions
{
    public interface IPasswordService
    {
        string Protect(string plainTextPassword);
    }
}
