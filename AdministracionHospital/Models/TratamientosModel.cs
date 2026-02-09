using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdministracionHospital.Models
{
    public class TratamientosModel
    {
        public decimal Costo_Tratamiento { get; set; }
        public virtual CitasModel Citas { get; set; }
        public virtual PrescripcionesModel Prescripciones { get; set; }
        public string Dosis { get; set; }
        public string Indicaciones { get; set; }
        public int ID_Tratamiento { get; set; }
    }
}