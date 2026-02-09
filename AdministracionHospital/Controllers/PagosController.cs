using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using AdministracionHospital.Models;

namespace AdministracionHospital.Controllers
{
    public class PagosController : Controller
    {
        private readonly string _cnnStr = ConfigurationManager.ConnectionStrings["cnnString"].ConnectionString;
        //Metodo para registrar Pagos
        [HttpPost]
        public ActionResult Create(PagosModel pago)
        {
            if(ModelState.IsValid)
            {
                using(SqlConnection _cnnS = new SqlConnection(_cnnStr))
                {
                    string query = @"insert into Pagos
                                    (ID_Pago, Fecha, Monto, Metodo_Pago, ID_Hospital, ID_Paciente, ID_Tratamiento)
                                    values (@ID_Pago, @Fecha, @Monto, @Metodo_Pago, @ID_Hospital, @ID_Paciente, @ID_Tratamiento)";

                    using (SqlCommand cmd = new SqlCommand(query, _cnnS))
                    {
                        cmd.Parameters.AddWithValue("@Id_Pago",pago.ID_Pago);
                        cmd.Parameters.AddWithValue("@Fecha",pago.Fecha);
                        cmd.Parameters.AddWithValue("@Monto",pago.Monto);
                        cmd.Parameters.AddWithValue("@Metodo_Pago",pago.Metodo_Pago);
                        cmd.Parameters.AddWithValue("@ID_Hospital",pago.ID_Hospital);
                        cmd.Parameters.AddWithValue("@ID_Paciente",pago.ID_Paciente);
                        cmd.Parameters.AddWithValue("@ID_Tratamiento",pago.ID_Tratamiento);

                        _cnnS.Open();
                        cmd.ExecuteNonQuery();
                    }
                    return RedirectToAction("Index");
                }//Fin del String de conexion
            }
            return View(pago);
        }//Fin del Create

        [HttpGet]
        public ActionResult Create()
        {
            LoadHospitalesDropdown();
            LoadPacientesDropdown();
            return View(new PagosModel());
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

        //Metodo para obtener los pagos pendientes
        [HttpGet]
        public ActionResult Details(string ID_Paciente)
        {
            ViewBag.ID_PacienteActual = ID_Paciente;

            if (string.IsNullOrWhiteSpace(ID_Paciente))
            {
                return View(new List<PagosModel>());
            }

            var Pendientes = new List<PagosModel>();

            using (SqlConnection _cnnS = new SqlConnection(_cnnStr))
            {
                string query = "Obtener_Pagos_Pendientes";

                SqlCommand cmd = new SqlCommand(query, _cnnS)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@ID_Paciente", ID_Paciente);

                _cnnS.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var pagosPendientes = new PagosModel();
                        pagosPendientes.Paciente = new PacientesModel();
                        pagosPendientes.Tratamiento = new TratamientosModel();

                        pagosPendientes.Fecha = reader.GetDateTime(0);
                        pagosPendientes.Paciente.Nombre_Paciente = reader.GetString(1);
                        pagosPendientes.Tratamiento.Costo_Tratamiento = reader.GetDecimal(2);
                        pagosPendientes.Monto = reader.GetDecimal(3);
                        pagosPendientes.Saldo_Pendiente = reader.GetDecimal(4);
                        pagosPendientes.Estado_Pago = reader.GetString(5);
                        pagosPendientes.Tipo_Pago = reader.GetString(6);

                        Pendientes.Add(pagosPendientes);
                    }
                };
            }
            return View(Pendientes);
        }//Fin del Details

        //Metodo para obtener los pagos pendientes
        [HttpGet]
        public ActionResult TotalPagosCliente(string ID_Paciente, DateTime? Fecha_Inicio, DateTime? Fecha_Final)
        {
            ViewBag.ID_PacienteActual = ID_Paciente;
            TotalPagosModel Pagos = new TotalPagosModel();

            if (string.IsNullOrWhiteSpace(ID_Paciente) || !Fecha_Inicio.HasValue|| !Fecha_Final.HasValue)
            {
                return View(Pagos);
            }

            using (SqlConnection _cnnS = new SqlConnection(_cnnStr))
            {
                string query = "Total_Pagos_Cliente";

                SqlCommand cmd = new SqlCommand(query, _cnnS)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@ID_Paciente", ID_Paciente);
                cmd.Parameters.AddWithValue("@Fecha_Inicio", Fecha_Inicio.Value);
                cmd.Parameters.AddWithValue("@Fecha_Final", Fecha_Final.Value);

                _cnnS.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Pagos.Cliente = reader.GetString(0);
                        Pagos.Fecha_Inicial = reader.GetDateTime(1);
                        Pagos.Fecha_Final = reader.GetDateTime(2);
                        Pagos.Total_Pagos_Cliente = reader.GetDecimal(3);
                    }
                };
            }
            return View(Pagos);
        }//Fin del TotalPagosCliente

        [HttpGet]
        public ActionResult CalcularPagosPacientes(string ID_Paciente)
        {
            ViewBag.ID_PacienteActual = ID_Paciente;

            List<TotalPagosModel> listaPendientes = new List<TotalPagosModel>();

            if (string.IsNullOrWhiteSpace(ID_Paciente))
            {
                return View(listaPendientes);
            }

            using (SqlConnection _cnn = new SqlConnection(_cnnStr))
            {
                string query = @"Calcular_Pagos_Pendientes_Pacientes";

                using (SqlCommand cmd = new SqlCommand(query, _cnn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_Paciente", ID_Paciente);

                    _cnn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var pendientes = new TotalPagosModel();
                            pendientes.Paciente = new PacientesModel();

                            pendientes.Paciente.ID_Paciente = reader.GetString(0);
                            pendientes.Cliente = reader.GetString(1);
                            pendientes.Total_Pendiente = reader.GetDecimal(2);

                            listaPendientes.Add(pendientes);
                        }
                    }
                }
            }
            return View(listaPendientes);
        }//Fin del CalcularPagosPacientes*****AJUSTAR*******

    }//Fin de la clase
}