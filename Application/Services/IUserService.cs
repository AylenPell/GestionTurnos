using Contracts.User.Requests;
using Contracts.User.Responses;

namespace Application.Services
{
    public interface IUserService
    {
        List<UserResponse> GetAll();
        UserResponse? GetById(int id);
        UserResponse? GetByDNI(string dni);
        bool Create(CreateUserRequest user);
    }
}
