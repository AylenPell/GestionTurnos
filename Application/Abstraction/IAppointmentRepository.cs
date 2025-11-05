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
        IQueryable<Appointment> GetByUserId(int userId);
        //bool CreateWithReferences(Appointment item, int userId, int? professionalId, int? studyId);
    }
}
