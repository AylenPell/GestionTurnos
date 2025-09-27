namespace Contracts.User.Responses
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string DNI { get; set; }
        public string Email { get; set; }
        public string HealthInsurance { get; set; }
        public string HealthInsurancePlan { get; set; }
    }
}
