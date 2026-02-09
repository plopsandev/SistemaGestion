using System;
using System.ComponentModel.DataAnnotations;

namespace AdministracionHospital.Models
{
    public class PacientesModel
    {
        [Display(Name = "Cedula")]
        public string ID_Paciente { get; set; }
        [Display(Name = "Nombre")]
        public string Nombre_Paciente { get; set; }
        [Display(Name = "Apellidos")]
        public string Apellidos { get; set; }
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime Fecha_Nacimiento { get; set; }
        [Display(Name = "Genero")]
        public Char Genero { get; set; }
        [Display(Name = "Direccion")]
        public string Direccion_Paciente { get; set; }
        [Display(Name = "Telefono")]
        public string Telefono_Paciente { get; set; }
        [Display(Name = "Resgistrado en")]
        public int ID_Hospital { get; set; }
        //Establecer la relacion con tabla Hospitales
        public virtual HospitalesModel Hospitales { get; set; }
    }
}