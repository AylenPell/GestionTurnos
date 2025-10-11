using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Specialty : BaseEntity
    {
        public Specialties SpecialtyName { get; set; }
    }
    public enum Specialties
    {
        Mastologia = 1,
        Genetica = 2,
        Oncologia = 3,
        Ginecologia = 4,
        Fertilidad = 5,
        Obstetricia = 6,
        Urologia = 7,
        Clinica = 8,
        Gastroenterologia = 9,
        Kinesiologia = 10,
        Nutricion = 11,
        Cardiologia = 12,
        Reumatologia = 13,
        Neumonologia = 14,
        Alergologia = 15,
        CirugiaPlastica = 16,
        Uroginecologia = 17,
        Urodinamia = 18
    }
}
