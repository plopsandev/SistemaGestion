using System;
using System.ComponentModel.DataAnnotations;

namespace AdministracionHospital.Models
{
    public class InventarioModel
    {
        [Display(Name = "Hospital")]
        public string Hospital { get; set; }
        [Display(Name = "Medicamento")]
        public string Medicamento { get; set; }
        [Display(Name = "Cantidad de Prescripciones")]
        public int Cantidad_Prescripciones { get; set; }
        [Display(Name = "Inventario Actual")]
        public int Inventario_Actual { get; set; }
    }
}