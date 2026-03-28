using System.Collections.Concurrent;

namespace Infrastructure.ExternalServices
{
    public class VerificationCodeService : IVerificationCodeService
    {
        private readonly ConcurrentDictionary<int, VerificationCodeData> _codes = new();
        private readonly TimeSpan _expirationTime = TimeSpan.FromMinutes(5);

        public string GenerateCode(int userId)
        {
            var random = new Random();
            var code = random.Next(100000, 999999).ToString(); // 6 dígitos

            var codeData = new VerificationCodeData
            {
                Code = code,
                CreatedAt = DateTime.Now,
                ExpiresAt = DateTime.Now.Add(_expirationTime)
            };

            _codes.AddOrUpdate(userId, codeData, (_, _) => codeData);

            return code;
        }

        public bool ValidateCode(int userId, string code)
        {
            if (!_codes.TryGetValue(userId, out var codeData))
                return false;

            if (DateTime.Now > codeData.ExpiresAt)
            {
                _codes.TryRemove(userId, out _);
                return false;
            }

            if (codeData.Code != code)
                return false;

            // Código válido, lo eliminamos para que no se pueda reutilizar
            _codes.TryRemove(userId, out _);
            return true;
        }

        public DateTime? GetCodeExpiration(int userId)
        {
            if (_codes.TryGetValue(userId, out var codeData))
                return codeData.ExpiresAt;

            return null;
        }

        public void RemoveCode(int userId)
        {
            _codes.TryRemove(userId, out _);
        }

        private class VerificationCodeData
        {
            public string Code { get; set; } = default!;
            public DateTime CreatedAt { get; set; }
            public DateTime ExpiresAt { get; set; }
        }
    }
}
