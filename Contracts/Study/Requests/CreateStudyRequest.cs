namespace Contracts.Study.Requests
{
    public class CreateStudyRequest
    {
        public string Name { get; set; }
        public bool? IsActive { get; set; } = true;
    }
}