using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdministracionHospital.Models
{
    public class StockMedicamentosModel
    {
        public int ID_Stock { get; set; }
        public virtual MedicamentosModel Medicamento { get; set; }
        public virtual HospitalesModel Hospital { get; set; }
        public int Cantidad_Dispo { get; set; }
    }
}