using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using AdministracionHospital.Models;

namespace AdministracionHospital.Controllers
{
    public class CitasController : Controller
    {
         private readonly string _cnnStr = ConfigurationManager.ConnectionStrings["cnnString"].ConnectionString;

        //Metodo para registrar nuevos medicos
        [HttpPost]
        public ActionResult Create(CitasModel citas)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection _cnnS = new SqlConnection(_cnnStr))
                {
                    string query = @"insert into Citas 
                                    (ID_Cita, Fecha, Hora, ID_Medico, ID_Paciente, ID_Hospital, ID_Diagnostico)
                                    values (@ID_Cita, @Fecha, @Hora, @ID_Medico, @ID_Paciente, @ID_Hospital, @ID_Diagnostico)";

                    using (SqlCommand cmd = new SqlCommand(query, _cnnS))
                    {
                        cmd.Parameters.AddWithValue("@ID_Cita",citas.ID_Cita);
                        cmd.Parameters.AddWithValue("@Fecha",citas.Fecha);
                        cmd.Parameters.AddWithValue("@Hora",citas.Hora);
                        cmd.Parameters.AddWithValue("@ID_Medico",citas.ID_Medico);
                        cmd.Parameters.AddWithValue("@ID_Paciente",citas.ID_Paciente);
                        cmd.Parameters.AddWithValue("@ID_Hospital",citas.ID_Hospital);
                        cmd.Parameters.AddWithValue("@ID_Diganostico",citas.ID_Diagnostico);

                        _cnnS.Open();
                        cmd.ExecuteNonQuery();
                    }
                    TempData["Registrado"] = "Se ha registrado correctamente";
                    return RedirectToAction("Create");
                }//Fin del string connection
            }
            LoadMedicosDropdown();
            LoadHospitalesDropdown();
            LoadDiagnosticosDropdown();
            LoadPacientesDropdown();
            return View(citas);
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
            LoadMedicosDropdown();
            LoadHospitalesDropdown();
            LoadDiagnosticosDropdown();
            LoadPacientesDropdown();

            CitasModel model = new CitasModel()
            {
                ID_Cita = GetNextCitaID()
            };

            return View(new CitasModel());
        }//Fin del create con HttpGet

        private void LoadMedicosDropdown()
        {
            List<SelectListItem> MedicosList = new List<SelectListItem>();
            using (SqlConnection _cnnS = new SqlConnection(_cnnStr))
            {
                string query = "Select ID_Medico, Nombre_Medico from Medicos ORDER BY Nombre_Medico";
                SqlCommand cmd = new SqlCommand(query, _cnnS);
                _cnnS.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MedicosList.Add(new SelectListItem
                        {
                            Value = reader["ID_Medico"].ToString(),
                            Text = reader["Nombre_Medico"].ToString()
                        });
                    }
                }
            }
            ViewBag.ID_Medico = MedicosList;
        }//Fin del LoadMedicosDropDown

        private void LoadDiagnosticosDropdown()
        {
            List<SelectListItem> DiagnosticosList = new List<SelectListItem>();
            using (SqlConnection _cnnS = new SqlConnection(_cnnStr))
            {
                string query = "Select ID_Diagnostico, Descripcion from Diagnosticos ORDER BY Descripcion";
                SqlCommand cmd = new SqlCommand(query, _cnnS);
                _cnnS.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DiagnosticosList.Add(new SelectListItem
                        {
                            Value = reader["ID_Diagnostico"].ToString(),
                            Text = reader["Descripcion"].ToString()
                        });
                    }
                }
            }
            ViewBag.ID_Diagnostico = DiagnosticosList;
        }//Fin del LoadDiagnosticosDropDown

        private void LoadPacientesDropdown()
        {
            List<SelectListItem> PacientesList = new List<SelectListItem>();
            using (SqlConnection _cnnS = new SqlConnection(_cnnStr))
            {
                string query = "Select ID_Paciente, Nombre_Paciente from Pacientes ORDER BY Nombre_Paciente";
                SqlCommand cmd = new SqlCommand(query, _cnnS);
                _cnnS.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PacientesList.Add(new SelectListItem
                        {
                            Value = reader["ID_Paciente"].ToString(),
                            Text = reader["Nombre_Paciente"].ToString()
                        });
                    }
                }
            }
            ViewBag.ID_Paciente = PacientesList;
        }//Fin del LoadPacientesDropDown

        //Metodo para obtener el numero de cita actual
        private int GetNextCitaID()
        {
            int nextID = 1;
            using (SqlConnection _cnnS = new SqlConnection(_cnnStr))
            {
                string query = "SELECT COALESCE(MAX(ID_Cita), 0) + 1 FROM Citas";
                SqlCommand cmd = new SqlCommand(query, _cnnS);
                _cnnS.Open();

                object result = cmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    nextID = Convert.ToInt32(result);
                }
            }
            return nextID;
        }

        //Metodo para observar las Agenda de Citas
        public ActionResult Index(bool? cargardatos)
        {
            var citas = new List<CitasModel>();
            if (cargardatos == true)
            {
                using (SqlConnection _cnnS = new SqlConnection(_cnnStr))
                {
                    string query = @"Select 
                                    c.ID_Cita, c.Fecha, c.Hora, m.Nombre_Medico, p.Nombre_Paciente,
                                    h.Nombre_Hospital, d.Descripcion
                                    from Citas c
                                    inner join Medicos m on c.ID_Medico = m.ID_Medico
                                    inner join Pacientes p on c.ID_Paciente = p.ID_Paciente
                                    inner join Hospitales h on c.ID_Hospital = h.ID_Hospital
                                    inner join Diagnosticos d on c.ID_Diagnostico = d.ID_Diagnostico";

                    SqlCommand cmd = new SqlCommand(query, _cnnS);
                    _cnnS.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var cita = new CitasModel();
                            cita.Paciente = new PacientesModel();
                            cita.Hospital = new HospitalesModel();
                            cita.Medico = new MedicosModel();
                            cita.Diagnostico = new DiagnosticosModel();

                            cita.ID_Cita = reader.GetInt32(0);
                            cita.Fecha = reader.GetDateTime(1);
                            cita.Hora = reader.GetTimeSpan(2);
                            cita.Medico.Nombre_Medico = reader.GetString(3);
                            cita.Paciente.Nombre_Paciente = reader.GetString(4);
                            cita.Hospital.Nombre_Hospital = reader.GetString(5);
                            cita.Diagnostico.Descripcion = reader.GetString(6);

                            citas.Add(cita);
                        }
                    }
                }
            }
            return View(citas);
        }//Fin del Index

        [HttpGet]
        public ActionResult ActualizarDisponibilidadCitas(int? ID_Cita, DateTime? Fecha, TimeSpan? Hora, string ID_Medico, string ID_Paciente, int? ID_Hospital, int? ID_Diagnostico)
        {
            ViewBag.ID_MedicoActual = ID_Medico;
            ViewBag.ID_Cita = ID_Cita;

            LoadMedicosDropdown();
            LoadDiagnosticosDropdown();
            LoadPacientesDropdown();
            LoadHospitalesDropdown();

            List<CitasModel> actualizarDisponibilidad = new List<CitasModel>();

            if (!ID_Cita.HasValue)
            {
                return View(actualizarDisponibilidad);
            }

            using (SqlConnection _cnn = new SqlConnection(_cnnStr))
            {
                string query = @"Registrar_Citas_Actualizar_Disponibilidad";

                using (SqlCommand cmd = new SqlCommand(query, _cnn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ID_Cita",ID_Cita);
                    cmd.Parameters.AddWithValue("@Fecha",Fecha);
                    cmd.Parameters.AddWithValue("@Hora",Hora);
                    cmd.Parameters.AddWithValue("@ID_Medico",ID_Medico);
                    cmd.Parameters.AddWithValue("@ID_Paciente",ID_Paciente);
                    cmd.Parameters.AddWithValue("@ID_Hospital",ID_Hospital);
                    cmd.Parameters.AddWithValue("@ID_Diagnostico",ID_Diagnostico);

                    _cnn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            var citas = new CitasModel();
                            citas.ID_Cita = reader.GetInt32(0);
                            citas.Fecha = reader.GetDateTime(1);
                            citas.Hora = reader.GetTimeSpan(2);
                            citas.ID_Medico = reader.GetString(3);
                            citas.ID_Paciente = reader.GetString(4);
                            citas.ID_Hospital = reader.GetInt32(5);
                            citas.ID_Diagnostico = reader.GetInt32(6);

                            actualizarDisponibilidad.Add(citas);
                        }
                    }
                }
            }
        return View(actualizarDisponibilidad);
        }
    }
}