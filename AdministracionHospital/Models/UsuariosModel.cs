using System;
using System.ComponentModel.DataAnnotations;

namespace AdministracionHospital.Models
{
    public class UsuariosModel
    {
        [Display(Name = "ID Usuario")]
        public int ID_Usuario { get; set; }
        [Display(Name = "Nombre Usuario")]
        public string Nombre_Usuario { get; set; }
        [Display(Name = "Cedula del Paciente")]
        public string ID_Paciente { get; set; }
        [Display(Name = "Cedula del Medico")]
        public string ID_Medico { get; set; }
        [Display(Name = "Rol")]
        public string Rol { get; set; }
        [Display(Name = "ID_Rol")]
        public int ID_Rol { get; set; }

        //Relacion con la tabla Pacientes
        public virtual PacientesModel paciente {get;set;}
        //Relacion con la tabla Medicos
        public virtual MedicosModel medicos { get; set; }
     

    }
}