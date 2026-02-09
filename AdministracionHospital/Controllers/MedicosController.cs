using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using AdministracionHospital.Models;

namespace AdministracionHospital.Controllers
{
    public class MedicosController : Controller
    {
        private readonly string _cnnStr = ConfigurationManager.ConnectionStrings["cnnString"].ConnectionString;

        //Metodo para registrar nuevos medicos
        [HttpPost]
        public ActionResult Create(MedicosModel medico)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection _cnnS = new SqlConnection(_cnnStr))
                {
                    string query = @"insert into Medicos 
                                    (ID_Medico, Nombre_Medico, Telefono_Medico, Correo_electronico, ID_Hospital, ID_Especialidad)
                                    values (@ID_Medico, @Nombre_Medico, @Telefono_Medico, @Correo_electronico, @ID_Hospital, @ID_Especialidad)";

                    using (SqlCommand cmd = new SqlCommand(query, _cnnS))
                    {
                        cmd.Parameters.AddWithValue("@ID_Medico",medico.ID_Medico);
                        cmd.Parameters.AddWithValue("@Nombre_Medico",medico.Nombre_Medico);
                        cmd.Parameters.AddWithValue("@Telefono_Medico",medico.Telefono_Medico);
                        cmd.Parameters.AddWithValue("@Correo_Electronico",medico.Correo_Electronico);
                        cmd.Parameters.AddWithValue("@ID_Hospital", medico.ID_Hospital);
                        cmd.Parameters.AddWithValue("@ID_Especialidad", medico.ID_Especialidad);

                        _cnnS.Open();
                        cmd.ExecuteNonQuery();
                    }
                    TempData["Registrado"] = "Se ha registrado correctamente";
                    return RedirectToAction("Create");
                }//Fin del string connection
            }
            LoadEspecialidadesDropdown();
            LoadHospitalesDropdown();
            return View(medico);
        }//Fin del Create

        private void LoadHospitalesDropdown()
        {
            List<SelectListItem> hospitalesList = new List<SelectListItem>();
            using (SqlConnection _cnnS = new SqlConnection(_cnnStr))
            {
                string query = "Select ID_Hospital, Nombre_Hospital from Hospitales ORDER BY Nombre_Hospital";
                SqlCommand cmd = new SqlCommand(query, _cnnS);
                _cnnS.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        hospitalesList.Add(new SelectListItem
                        {
                            Value = reader["ID_Hospital"].ToString(),
                            Text = reader["Nombre_Hospital"].ToString()
                        });
                    }
                }
            }
            ViewBag.ID_Hospital = hospitalesList;
        }//Fin del LoadHospitalesDropDown

        [HttpGet]
        public ActionResult Create()
        {
            LoadHospitalesDropdown();
            LoadEspecialidadesDropdown();
            return View(new MedicosModel());
        }//Fin del create con HttpGet

        private void LoadEspecialidadesDropdown()
        {
            List<SelectListItem> EspecialidadesList = new List<SelectListItem>();
            using (SqlConnection _cnnS = new SqlConnection(_cnnStr))
            {
                string query = "Select ID_Especialidad, Nombre_Especialidad from Especialidades_Medicos ORDER BY Nombre_Especialidad";
                SqlCommand cmd = new SqlCommand(query, _cnnS);
                _cnnS.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        EspecialidadesList.Add(new SelectListItem
                        {
                            Value = reader["ID_Especialidad"].ToString(),
                            Text = reader["Nombre_Especialidad"].ToString()
                        });
                    }
                }
            }
            ViewBag.ID_Especialidad = EspecialidadesList;
        }//Fin del LoadEspecialidadesDropDown

        public ActionResult Details(bool? cargardatos)
        {
            var Medicos = new List<MedicosModel>();

            if(cargardatos == true)
            {
                using (SqlConnection _cnnS = new SqlConnection(_cnnStr))
                {
                    string query = @"Select ID_Medico, 
                                    Nombre_Medico, 
                                    Telefono_Medico, 
                                    Correo_electronico, 
                                    Nombre_Hospital, 
                                    Nombre_Especialidad from Medicos m 
                                    inner join Hospitales h on m.ID_Hospital = h.ID_Hospital 
                                    inner join Especialidades_Medicos e on m.ID_Especialidad = e.ID_Especialidad";

                    SqlCommand cmd = new SqlCommand(query, _cnnS);
                    _cnnS.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            var medico = new MedicosModel();
                            medico.Hospital = new HospitalesModel();
                            medico.Especialidad = new EspecialidadesModel();

                            medico.ID_Medico = reader.GetString(0);
                            medico.Nombre_Medico = reader.GetString(1);
                            medico.Telefono_Medico = reader.GetString(2);
                            medico.Correo_Electronico = reader.GetString(3);
                            medico.Hospital.Nombre_Hospital = reader.GetString(4);
                            medico.Especialidad.Nombre_Especialidad = reader.GetString(5);

                            Medicos.Add(medico);
                        }
                    }
                }
            }
            return View(Medicos);
        }//Fin del details
    }
}