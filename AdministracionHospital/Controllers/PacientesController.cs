using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using AdministracionHospital.Models;


namespace AdministracionHospital.Controllers
{
    public class PacientesController : Controller
    {
        private readonly string _cnnStr = ConfigurationManager.ConnectionStrings["cnnString"].ConnectionString;

        //Metodo para mostrar pacientes registrados
        public ActionResult Index(bool? cargardatos)
        {
            var pacientes = new List<PacientesModel>();

            if(cargardatos == true)
            {
                using (SqlConnection _cnnS = new SqlConnection(_cnnStr))
                {
                    string query = @"Select ID_Paciente,
                                Nombre_Paciente,
                                Apellidos,
                                Fecha_Nacimiento,
                                Genero,
                                Direccion_Paciente,
                                Telefono_Paciente,
                                H.Nombre_Hospital
                                from Pacientes inner join Hospitales h on Pacientes.ID_Hospital = H.ID_Hospital";

                    SqlCommand cmd = new SqlCommand(query, _cnnS);
                    _cnnS.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var paciente = new PacientesModel();
                            paciente.Hospitales = new HospitalesModel();

                            paciente.ID_Paciente = reader.GetString(0);
                            paciente.Nombre_Paciente = reader.GetString(1);
                            paciente.Apellidos = reader.GetString(2);
                            paciente.Fecha_Nacimiento = reader.GetDateTime(3);
                            paciente.Genero = reader.GetString(4)[0];
                            paciente.Direccion_Paciente = reader.GetString(5);
                            paciente.Telefono_Paciente = reader.GetString(6);
                            paciente.Hospitales.Nombre_Hospital = reader.GetString(7);

                            pacientes.Add(paciente);
                        }
                    }
                }
            }
            return View(pacientes);
        }//Fin del index

        [HttpPost]
        public ActionResult Create(PacientesModel paciente)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection _cnnS = new SqlConnection(_cnnStr))
                {
                    string query = @"insert into Pacientes 
                                    (ID_Paciente, Nombre_Paciente, Apellidos, Fecha_Nacimiento, Genero, Direccion_Paciente, Telefono_Paciente, ID_Hospital)
                                    values (@ID_Paciente, @Nombre_Paciente, @Apellidos, @Fecha_Nacimiento, @Genero, @Direccion_Paciente, @Telefono_Paciente, @ID_Hospital)";

                    using (SqlCommand cmd = new SqlCommand(query, _cnnS))
                    {
                        cmd.Parameters.AddWithValue("@ID_Paciente",paciente.ID_Paciente);
                        cmd.Parameters.AddWithValue("@Nombre_Paciente",paciente.Nombre_Paciente);
                        cmd.Parameters.AddWithValue("@Apellidos",paciente.Apellidos);
                        cmd.Parameters.AddWithValue("@Fecha_Nacimiento",paciente.Fecha_Nacimiento);
                        cmd.Parameters.AddWithValue("@Genero",paciente.Genero);
                        cmd.Parameters.AddWithValue("@Direccion_Paciente",paciente.Direccion_Paciente);
                        cmd.Parameters.AddWithValue("@Telefono_Paciente",paciente.Telefono_Paciente);
                        cmd.Parameters.AddWithValue("@ID_Hospital",paciente.ID_Hospital);

                        _cnnS.Open();
                        cmd.ExecuteNonQuery();
                    }
                    return RedirectToAction("Index");
                }//Fin del string connection
            }
            return View(paciente);
        }//Fin del Create

        [HttpGet]
        public ActionResult Create()
        {
            LoadHospitalesDropdown();
            return View(new PacientesModel());
        }//Fin del create con HttpGet

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

        //Metodo para obtener historial del paciente
        [HttpGet]
        public ActionResult Details(string ID_Paciente)
        {
            ViewBag.ID_PacienteActual = ID_Paciente;

            if(string.IsNullOrWhiteSpace(ID_Paciente))
            {
                return View(new List<HistorialPacienteModel>());
            }

            var historialPaciente = new List<HistorialPacienteModel>();

            using(SqlConnection _cnnS = new SqlConnection(_cnnStr))
            {
                string query = "Historial_Medico_Paciente";

                SqlCommand cmd = new SqlCommand(query, _cnnS)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@ID_Paciente", ID_Paciente);

                _cnnS.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                   while(reader.Read())
                    {
                        var historial = new HistorialPacienteModel();

                        historial.Fecha_Cita = reader.GetDateTime(0);
                        historial.Cedula_Paciente = reader.GetString(1);
                        historial.Paciente = reader.GetString(2);
                        historial.Diagnostico = reader.GetString(3);
                        historial.Medicamento = reader.GetString(4);
                        historial.Cantidad_Medicamentos = reader.GetInt32(5);
                        historial.Dosis = reader.GetString(6);
                        historial.Indicaciones = reader.GetString(7);
                        historial.Costo_Total_Tratamiento = reader.GetDecimal(8);
                        historial.Hospital = reader.GetString(9);
                        historial.Atendido_Por = reader.GetString(10);

                        historialPaciente.Add(historial);
                    }
                };
            }
            return View(historialPaciente);
        }//Fin del Details
    }
}