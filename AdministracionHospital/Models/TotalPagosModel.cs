using System;
using System.ComponentModel.DataAnnotations;

namespace AdministracionHospital.Models
{
    //Para Stored Procedure de Obtener_Pagos_Cliente
    public class TotalPagosModel
    {
        [Display(Name = "Cliente")]
        public string Cliente { get; set; }
        [Display(Name = "Fecha Inicial")]
        public DateTime Fecha_Inicial { get; set; }
        [Display(Name = "Fecha Final")]
        public DateTime Fecha_Final { get; set; }
        [Display(Name = "Total Pagado")]
        public decimal Total_Pagos_Cliente { get; set; }
        [Display(Name = "Total Pendiente")]
        public decimal Total_Pendiente { get; set; }
        [Display(Name = "Paciente")]
        public virtual PacientesModel Paciente { get; set; }
    }
}