using Contracts.User.Responses;
using Application.Abstraction;
using Domain.Entities;
using Contracts.User.Requests;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService (IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public List<UserResponse> GetAll()
        {
            var usersList = _userRepository
                .GetAll()
                .Select(user => new UserResponse
                {
                    Id = user.Id,
                    Name = user.Name,
                    LastName = user.LastName,
                    DNI = user.DNI,
                    Email = user.Email,
                    HealthInsurance = user.HealthInsurance,
                    HealthInsurancePlan = user.HealthInsurancePlan
                }).ToList();

            return usersList;
        }
        public UserResponse? GetById(int id)
        {
            return _userRepository
                .GetById(id) is User user
                ? new UserResponse
                {
                    Id = user.Id,
                    Name = user.Name,
                    LastName = user.LastName,
                    DNI = user.DNI,
                    Email = user.Email,
                    HealthInsurance = user.HealthInsurance,
                    HealthInsurancePlan = user.HealthInsurancePlan
                }
                : null;
        }
        public UserResponse? GetByDNI(string dni)
        {
            return _userRepository
                .GetByDNI(dni) is User user
                ? new UserResponse
                {
                    Id = user.Id,
                    Name = user.Name,
                    LastName = user.LastName,
                    DNI = user.DNI,
                    Email = user.Email,
                    HealthInsurance = user.HealthInsurance,
                    HealthInsurancePlan = user.HealthInsurancePlan
                }
                : null;
        }
        public bool Create(CreateUserRequest user) 
        {
            var existingUser = _userRepository.GetByDNI(user.DNI);
            if (existingUser != null)
            {
                return false; // Usuario con el mismo DNI ya existe
            }
            var newUser = new User
            {
                Name = user.Name,
                LastName = user.LastName,
                DNI = user.DNI,
                Password = user.Password,
                Email = user.Email,
                City = user.City,
                Address = user.Address,
                Phone = user.Phone, 
                HealthInsurance = user.HealthInsurance,
                HealthInsurancePlan = user.HealthInsurancePlan
            };
            _userRepository.Create(newUser);
            return true;
        }
    }
}
