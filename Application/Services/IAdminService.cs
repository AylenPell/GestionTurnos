using Contracts.Authentication;
using Contracts.Admin.Requests;
using Contracts.Admin.Responses;

namespace Application.Services
{
    public interface IAdminService
    {
        List<AdminUserResponse> GetAll();
        AdminUserResponse? GetById(int id);
        AdminUserResponse? GetByDNI(string dni);
        bool Create(AdminCreateUserRequest user, out string message, out int createdId);
        bool Update(int id, AdminUpdateUserRequest user, out string message);
        bool Delete(int id, out string message);
    }
}
