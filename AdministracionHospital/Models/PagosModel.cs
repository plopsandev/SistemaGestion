using System;
using System.ComponentModel.DataAnnotations;

namespace AdministracionHospital.Models
{
    public class PagosModel
    {
        [Display(Name ="Identificador de Pago")]
        public int ID_Pago { get; set; }
        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; }
        [Display(Name = "Monto")]
        public decimal Monto { get; set; }
        [Display(Name = "Metodo de Pago")]
        public string Metodo_Pago { get; set; }
        [Display(Name = "Hospital")]
        public int ID_Hospital { get; set; }
        [Display(Name = "Paciente")]
        public string ID_Paciente { get; set; }
        [Display(Name = "Tratamiento")]
        public int ID_Tratamiento { get; set; }
        [Display(Name = "Saldo Pendiente")]
        public decimal Saldo_Pendiente { get; set; }
        public virtual HospitalesModel Hospital { get; set; }
        public virtual PacientesModel Paciente { get; set; }
        public virtual TratamientosModel Tratamiento { get; set; }
        [Display(Name = "Estado del Pago")]
        public string Estado_Pago { get; set; }
        [Display(Name = "Tipo de Pago")]
        public string Tipo_Pago { get; set; }
    }
}