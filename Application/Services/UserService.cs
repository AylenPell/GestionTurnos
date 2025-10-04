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

        public bool Update( int id, UpdateUserRequest user)
        {
            var existingUser = _userRepository.GetById(id);

            if (existingUser == null)
                return false;

            existingUser.Name = user.Name ?? existingUser.Name;
            existingUser.LastName = user.LastName ?? existingUser.LastName;
            existingUser.DNI = user.DNI ?? existingUser.DNI;
            existingUser.Email = user.Email ?? existingUser.Email;
            existingUser.Address = user.Address ?? existingUser.Address;
            existingUser.City = user.City ?? existingUser.City;
            existingUser.Phone = user.Phone ?? existingUser.Phone;
            existingUser.Password = user.Password ?? existingUser.Password;
            existingUser.HealthInsurance = user.HealthInsurance ?? existingUser.HealthInsurance;
            existingUser.HealthInsurancePlan = user.HealthInsurancePlan ?? existingUser.HealthInsurancePlan;

            return _userRepository.Update(existingUser);
        }

        public bool Delete(int id)
        {
            var deletedUser = _userRepository.GetById(id);
            if (deletedUser == null)
                return false;

            return _userRepository.Delete(deletedUser);
        }

    }
    
}
