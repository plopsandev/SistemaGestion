using System;
using System.ComponentModel.DataAnnotations;

namespace AdministracionHospital.Models
{
    public class MedicosModel
    {
        [Display(Name = "Cedula")]
        public string ID_Medico { get; set; }
        [Display(Name = "Medico")]
        public string Nombre_Medico { get; set; }
        [Display(Name = "Telefono")]
        public string Telefono_Medico { get; set; }
        [Display(Name = "Correo Electronico")]
        public string Correo_Electronico { get; set; }
        [Display(Name = "Hospital")]
        public int ID_Hospital { get; set; }
        [Display(Name = "Especialidad")]
        public int ID_Especialidad { get; set; }
        //Relaciones con Tabla Hospital y Especialidades
        public virtual HospitalesModel Hospital { get; set; }
        public virtual EspecialidadesModel Especialidad { get; set; }
    }
}