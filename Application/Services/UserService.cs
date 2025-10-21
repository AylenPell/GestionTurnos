using Application.Abstraction;
using Contracts.User.Requests;
using Contracts.User.Responses;
using Domain.Entities;
using Application.Services.Helpers;

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
                    BirthDate = user.BirthDate,
                    Email = user.Email,
                    City = user.City,
                    Address = user.Address,
                    Phone = user.Phone,
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
                    BirthDate = user.BirthDate,
                    Email = user.Email,
                    City = user.City,
                    Address = user.Address,
                    Phone = user.Phone,
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
                    BirthDate = user.BirthDate,
                    Email = user.Email,
                    City = user.City,
                    Address = user.Address,
                    Phone = user.Phone,
                    HealthInsurance = user.HealthInsurance,
                    HealthInsurancePlan = user.HealthInsurancePlan
                }
                : null;
        }
        public bool Create(CreateUserRequest user, out string message, out int createdId)
        {
            message = "";
            createdId = 0;

            var existingUser = _userRepository.GetByDNI(user.DNI);
            if (existingUser != null)
            {
                message = "El usuario ya existe.";
                return false;
            }

            if (!ValidationHelper.DNIValidator(user.DNI))
            {
                message = "El DNI ingresado no es válido.";
                return false;
            }

            if (!ValidationHelper.EmailValidator(user.Email))
            {
                message = "El email ingresado no es válido.";
                return false;
            }

            if (!ValidationHelper.PhoneNumberValidator(user.Phone))
            {
                message = "El número de teléfono ingresado no es válido.";
                return false;
            }
            if (!ValidationHelper.BirthDateValidator(user.BirthDate))
            {
                message = "La fecha de nacimiento no es válida.";
                return false;
            }
            if (!ValidationHelper.PasswordValidator(user.Password))
            {
                message = "La contraseña no es válida.";
                return false;
            }

            var newUser = new User
            {
                Name = user.Name,
                LastName = user.LastName,
                DNI = user.DNI,
                BirthDate = user.BirthDate,
                Password = user.Password,
                Email = user.Email,
                City = user.City,
                Address = user.Address,
                Phone = user.Phone,
                HealthInsurance = user.HealthInsurance,
                HealthInsurancePlan = user.HealthInsurancePlan
            };

            _userRepository.Create(newUser);

            message = "Usuario creado correctamente.";
            createdId = newUser.Id;
            return true;
        }


        public bool Update(int id, UpdateUserRequest user, out string message)
        {
            message = "";

            var existingUser = _userRepository.GetById(id);
            if (existingUser == null)
            {
                message = "El usuario no existe.";
                return false;
            }

            if (user.DNI != null && !ValidationHelper.DNIValidator(user.DNI))
            {
                message = "El DNI ingresado no es válido.";
                return false;
            }

            if (user.Email != null && !ValidationHelper.EmailValidator(user.Email))
            {
                message = "El email ingresado no es válido.";
                return false;
            }

            if (user.Phone != null && !ValidationHelper.PhoneNumberValidator(user.Phone))
            {
                message = "El número de teléfono ingresado no es válido.";
                return false;
            }

            if (user.BirthDate.HasValue && !ValidationHelper.BirthDateValidator(user.BirthDate.Value))
            {
                message = "La fecha de nacimiento no es válida.";
                return false;
            }
            if (user.Password != null &&  !ValidationHelper.PasswordValidator(user.Password))
            {
                message = "La contraseña no es válida.";
                return false;
            }

            existingUser.Name = user.Name ?? existingUser.Name;
            existingUser.LastName = user.LastName ?? existingUser.LastName;
            existingUser.DNI = user.DNI ?? existingUser.DNI;

            if (user.BirthDate.HasValue)
                existingUser.BirthDate = user.BirthDate.Value;

            existingUser.Email = user.Email ?? existingUser.Email;
            existingUser.Address = user.Address ?? existingUser.Address;
            existingUser.City = user.City ?? existingUser.City;
            existingUser.Phone = user.Phone ?? existingUser.Phone;
            existingUser.Password = user.Password ?? existingUser.Password;
            existingUser.HealthInsurance = user.HealthInsurance ?? existingUser.HealthInsurance;
            existingUser.HealthInsurancePlan = user.HealthInsurancePlan ?? existingUser.HealthInsurancePlan;

            var updated = _userRepository.Update(existingUser);

            if (!updated)
            {
                message = "No se pudo actualizar el usuario en la base de datos.";
                return false;
            }

            message = "Usuario actualizado correctamente.";
            return true;
        }


        //public bool Delete(int id)
        //{
        //    var deletedUser = _userRepository.GetById(id);
        //    if (deletedUser == null)
        //        return false;

        //    return _userRepository.Delete(deletedUser);
        //}
        public bool Delete(int id, out string message)
        {
            message = "";
            var deletedUser = _userRepository.GetById(id);
            if (deletedUser == null)
            {
                message = "El usuario no existe.";
                return false;
            }
            if (!deletedUser.IsActive)
            {
                message = "El usuario ya se encuentra inactivo.";
                return false;
            }
            _userRepository.Delete(deletedUser);
            message = "Usuario eliminado exitosamente.";
            return true;
        }

    }
    
}
