namespace Infrastructure.ExternalServices
{
    public interface IVerificationCodeService
    {
        string GenerateCode(int userId);
        bool ValidateCode(int userId, string code);
        DateTime? GetCodeExpiration(int userId);
        void RemoveCode(int userId);
    }
}
