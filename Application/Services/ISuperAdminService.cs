using Contracts.Authentication;
using Contracts.SuperAdmin.Requests;
using Contracts.SuperAdmin.Responses;

namespace Application.Services
{
    public interface ISuperAdminService
    {
        List<SuperAdminUserResponse> GetAll();
        SuperAdminUserResponse? GetById(int id);
        SuperAdminUserResponse? GetByDNI(string dni);
        bool Create(SuperAdminCreateUserRequest user, out string message, out int createdId);
        bool Update(int id, SuperAdminUpdateUserRequest user, out string message);
        bool Delete(int id, out string message);
        bool Reactivate(int id, out string message);
    }
}
