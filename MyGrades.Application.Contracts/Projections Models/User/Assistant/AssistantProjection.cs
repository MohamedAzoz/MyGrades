using MyGrades.Domain.Entities;

namespace MyGrades.Application.Contracts.Projections_Models.User.Assistants
{
    public class AssistantProjection
    {
        public Assistant Assistant { get; set; }

        public string NationalId { get; set; }
        public string FullName { get; set; }
    }

}
