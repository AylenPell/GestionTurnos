using Contracts.Appointment.Requests;
using Contracts.Appointment.Responses;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstraction
{
    public interface IAppointmentRepository : IBaseRepository<Appointment>
    {
        IEnumerable<Appointment> GetByUserId(int userId);
        IEnumerable<Appointment> GetAll();
        //bool CreateWithReferences(Appointment item, int userId, int? professionalId, int? studyId);
        Appointment? GetByIdWithRelations(int id);
    }
}
