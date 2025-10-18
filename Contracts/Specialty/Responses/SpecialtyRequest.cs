namespace Contracts.Specialty.Responses
{
    public class SpecialtyResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }   
        public bool IsActive { get; set; }
        public int ProfessionalsCount { get; set; } 
    }
}