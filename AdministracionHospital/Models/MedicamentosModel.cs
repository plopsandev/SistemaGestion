using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdministracionHospital.Models
{
    public class MedicamentosModel
    {
        public int ID_Medicamento { get; set; }
        public string Nombre_Medicamento { get; set; }
        public string Descripcion_Medic { get; set; }
        public decimal Costo_Unidad { get; set; }
    }
}