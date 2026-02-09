using System;
using System.ComponentModel.DataAnnotations;

namespace AdministracionHospital.Models
{
    public class CitasModel
    {
        [Display(Name = "Cita")]
        public int ID_Cita { get; set; }
        [Display(Name = "Fecha")]
        public DateTime?  Fecha { get; set; }
        [Display(Name = "Hora")]
        public TimeSpan? Hora { get; set; }
        [Display(Name = "Medico")]
        public string ID_Medico { get; set; }
        [Display(Name = "Paciente")]
        public string ID_Paciente { get; set; }
        [Display(Name = "Hospital")]
        public int? ID_Hospital { get; set; }
        [Display(Name = "Diagnostico")]
        public int? ID_Diagnostico { get; set; }
        //Relaciones con tablas MEdicos, Pacientes, Hospitales y Diagnosticos
        public virtual MedicosModel Medico { get; set; }
        public virtual PacientesModel Paciente { get; set; }
        public virtual HospitalesModel Hospital { get; set; }
        public virtual DiagnosticosModel Diagnostico { get; set; }
    }
}