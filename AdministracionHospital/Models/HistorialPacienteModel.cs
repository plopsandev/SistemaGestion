using System;
using System.ComponentModel.DataAnnotations;
namespace AdministracionHospital.Models
{
    public class HistorialPacienteModel
    {
        [Display(Name = "Fecha de Cita")]
        public DateTime Fecha_Cita { get; set; }
        [Display(Name = "Cedula")]
        public string Cedula_Paciente { get; set; }
        [Display(Name = "Paciente")]
        public string Paciente { get; set; }
        [Display(Name = "Diagnostico")]
        public string Diagnostico { get; set; }
        [Display(Name = "Medicamento")]
        public string Medicamento { get; set; }
        [Display(Name = "Cantidad de Medicamentos")]
        public int Cantidad_Medicamentos { get; set; }
        [Display(Name = "Dosis")]
        public string Dosis { get; set; }
        [Display(Name = "Indicaciones")]
        public string Indicaciones { get; set; }
        [Display(Name = "Costo del Tratamiento")]
        public decimal Costo_Total_Tratamiento { get; set; }
        [Display(Name = "Hospital")]
        public string Hospital { get; set; }
        [Display(Name = "Atendido por el Medico")]
        public string Atendido_Por { get; set; }
    }
}