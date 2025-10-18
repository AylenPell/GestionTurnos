using Contracts.User.Requests;
using Contracts.User.Responses;

namespace Application.Services
{
    public interface IUserService
    {
        List<UserResponse> GetAll();
        UserResponse? GetById(int id);
        UserResponse? GetByDNI(string dni);
        bool Create(CreateUserRequest user, out string message, out int createdId);
        bool Update(int id, UpdateUserRequest user, out string message);
        bool Delete(int id);
    }
}
