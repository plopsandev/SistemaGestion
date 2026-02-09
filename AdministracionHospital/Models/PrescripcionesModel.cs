using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdministracionHospital.Models
{
    public class PrescripcionesModel
    {
        public int ID_Prescripcion { get; set; }
        public int ID_Medicamento { get; set;}
        public virtual MedicosModel Medicamento { get; set; }
    }
}