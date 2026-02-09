using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AdministracionHospital.Models
{
    public class HospitalesModel
    {
        [Display(Name = "Identificador")]
        public int      ID_Hospital { get; set; }
        [Display(Name = "Centro Hospitalario")]
        public string   Nombre_Hospital { get; set; }
        [Display(Name = "Direccion")]
        public string   Direccion_Hospital { get; set; }
        [Display(Name = "Contacto")]
        public string   Telefono_Hospital { get; set; }
    }
}