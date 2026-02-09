using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using AdministracionHospital.Models;

namespace AdministracionHospital.Controllers
{
    public class HospitalController : Controller
    {
        private readonly string _cnnStr = ConfigurationManager.ConnectionStrings["cnnString"].ConnectionString;
        
        //Metodo para Mostrar los hospitales
        public ActionResult Index(bool? cargardatos)
        {
            var Hospitales = new List<HospitalesModel>();

            if (cargardatos == true)
            {
                using (SqlConnection _cnnS = new SqlConnection(_cnnStr))
                {
                    string query = @"Select ID_Hospital, Nombre_Hospital, Direccion_Hospital, Telefono_Hospital from Hospitales";

                    SqlCommand cmd = new SqlCommand(query, _cnnS);
                    _cnnS.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Hospitales.Add(new HospitalesModel
                            {
                                ID_Hospital = reader.GetInt32(0),
                                Nombre_Hospital = reader.GetString(1),
                                Direccion_Hospital = reader.GetString(2),
                                Telefono_Hospital = reader.GetString(3)
                            });
                        }
                    }
                }
            }
            return View(Hospitales);
        }

        //Metodo para retornar la vista solamente al personal administrativo
        [HttpGet]
        public ActionResult Create()
        {
            return View(new HospitalesModel());
        }

        //Metodo para registrar un nuevo hospital
        [HttpPost]
        public ActionResult Create(HospitalesModel Hospital)
        {

            if(ModelState.IsValid)
            {
                using (SqlConnection _cnnS = new SqlConnection(_cnnStr))
                {
                    string query = "InsertarHospital";

                    SqlCommand cmd = new SqlCommand(query, _cnnS)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@ID_Hospital", Hospital.ID_Hospital);
                    cmd.Parameters.AddWithValue("@Nombre_Hospital", Hospital.Nombre_Hospital);
                    cmd.Parameters.AddWithValue("@Direccion_Hospital", Hospital.Direccion_Hospital);
                    cmd.Parameters.AddWithValue("@Telefono_Hospital", Hospital.Telefono_Hospital);

                    _cnnS.Open();
                    cmd.ExecuteNonQuery();

                    return RedirectToAction("Index", new { cargardatos = true });
                }                  
            }
            return View(Hospital);
        }

        //Metodo para Obtener Inventario de 30 Dias
        [HttpGet]
        public ActionResult Inventario(string NombreHospital)
        {
            ViewBag.NombreHospitalActual = NombreHospital;

            List<InventarioModel> stockList = new List<InventarioModel>();

            if(string.IsNullOrWhiteSpace(NombreHospital))
            {
                return View(stockList);
            }

            using (SqlConnection _cnnS = new SqlConnection(_cnnStr))
            {
                string query = "ObtenerInventario_30_Dias";

                SqlCommand cmd = new SqlCommand(query, _cnnS)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@Nombre_Hospital",NombreHospital);

                _cnnS.Open();

                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        InventarioModel stock = new InventarioModel();
                        stock.Hospital = reader.GetString(0);
                        stock.Medicamento = reader.GetString(1);
                        stock.Cantidad_Prescripciones = reader.GetInt32(2);
                        stock.Inventario_Actual = reader.GetInt32(3);

                        stockList.Add(stock);
                    }
                }
            };
         return View(stockList);
        }

        //Metodo para registrar medicamentos
       [HttpPost]
       public ActionResult RegistroMedicamentos(MedicamentosModel medicamento)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection _cnnS = new SqlConnection(_cnnStr))
                {
                    string query = @"insert into Medicamentos (ID_Medicamento, Nombre_Medicamento, Descripcion_Medic, Costo_Unidad) 
                    values (@ID_Medicamento, @Nombre_Medicamento, @Descripcion_Medic, @Costo_Unidad)";

                    using (SqlCommand cmd = new SqlCommand(query, _cnnS))
                    {
                        cmd.Parameters.AddWithValue("@ID_Medicamento", medicamento.ID_Medicamento);
                        cmd.Parameters.AddWithValue("@Nombre_Medicamento", medicamento.Nombre_Medicamento);
                        cmd.Parameters.AddWithValue("@Descripcion_Medic", medicamento.Descripcion_Medic);
                        cmd.Parameters.AddWithValue("@Costo_Unidad", medicamento.Costo_Unidad);

                        _cnnS.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            return View(medicamento);
        }//Fin del RegistroMedicamentos

       [HttpGet]
       public ActionResult RegistroMedicamentos()
        {
            return View();
        }
      
       [HttpPost]
       public ActionResult RealizarPrescripciones(PrescripcionesModel prescripciones)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection _cnnS = new SqlConnection(_cnnStr))
                {
                    string query = @"insert into Prescripciones (ID_Prescripcion, ID_Medicamento) 
                    values (@ID_Prescripcion, @ID_Medicamento)";

                    using (SqlCommand cmd = new SqlCommand(query, _cnnS))
                    {
                        cmd.Parameters.AddWithValue("@ID_Prescripcion", prescripciones.ID_Prescripcion);
                        cmd.Parameters.AddWithValue("@ID_Medicamento", prescripciones.ID_Medicamento);

                        _cnnS.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            return View(prescripciones);
        }//Fin del RealizarPrescripciones

       [HttpGet]
       public ActionResult RealizarPrescripciones()
       {
            LoadMedicamentosDropdown();
            return View(new PrescripcionesModel());
       }

        private void LoadMedicamentosDropdown()
        {
            List<SelectListItem> MedicamentosList = new List<SelectListItem>();
            using (SqlConnection _cnnS = new SqlConnection(_cnnStr))
            {
                string query = "Select ID_Medicamento, Nombre_Medicamento from Medicamentos ORDER BY Nombre_Medicamento";
                SqlCommand cmd = new SqlCommand(query, _cnnS);
                _cnnS.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MedicamentosList.Add(new SelectListItem
                        {
                            Value = reader["ID_Medicamento"].ToString(),
                            Text = reader["Nombre_Medicamento"].ToString()
                        });
                    }
                }
            }
            ViewBag.ID_Medicamento = MedicamentosList;
        }//Fin del LoadHospitalesDropDown
    }
}