using System;
using System.ComponentModel.DataAnnotations;

namespace AdministracionHospital.Models
{
    public class PagosPendientesModel
    {
        [Display(Name = "Cedula Paciente")]
        public string ID_Paciente { get; set; }
        [Display(Name = "Paciente")]
        public string Paciente { get; set; }
        [Display(Name = "Total por pagar")]
        public decimal Total_Pendiente { get; set; }
    }
}