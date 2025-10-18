namespace Contracts.Specialty.Requests
{
    public class CreateSpecialtyRequest
    {   
        public int? Id { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; } = true;
    }
}