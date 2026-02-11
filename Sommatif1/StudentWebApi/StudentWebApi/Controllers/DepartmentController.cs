using Mappers;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO;
using Services;

namespace StudentWebApi.Controllers
{
    [ApiController]
    [Route("Department")]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        private readonly IDepartmentMapper _departmentMapper;

        public DepartmentController(IDepartmentService departmentService, IDepartmentMapper departmentMapper)
        {
            _departmentService = departmentService;
            _departmentMapper = departmentMapper;
        }

        /// <summary>
        /// get department list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDepartments()
        {
            List<DepartmentDTO> result = _departmentService.GetAll();
            return Ok(result);
        }

        /// <summary>
        /// get department by id
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        [HttpGet("{departmentId}")]
        public ActionResult GetDepartmentById([FromRoute] int departmentId)
        {
            DepartmentDTO? department = _departmentService.GetById(departmentId);
            return department == null ? NotFound() : Ok(department);
        }

        /// <summary>to Db
        /// add department 
        /// </summary>
        /// <param name="departmentDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddDepartment([FromBody] DepartmentDTO departmentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please Add Department");
            }
            else
            {
                try
                {
                    Department AddDepartment = _departmentService.AddDepartment(departmentDTO);
                    _departmentService.SaveChanges();
                    DepartmentDTO departmentDTOResult = _departmentMapper.MapToDepartmentDTO(AddDepartment);
                    return Ok(departmentDTOResult);
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
            }
        }

        /// <summary>
        /// edit department data in db
        /// </summary>
        /// <param name="departmentId"></param>
        /// <param name="departmentDTO"></param>
        /// <returns></returns>
        [HttpPut("{departmentId}")]
        public ActionResult EditDepartment([FromRoute] int departmentId, [FromBody] DepartmentDTO departmentDTO)
        {
            // 1. Vérification si le body est vide ou mal formé
            if (departmentDTO == null)
            {
                return BadRequest("Le corps de la requête ne peut pas être vide.");
            }

            // 2. Vérification automatique via les Data Annotations (ex: [Required])
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // 3. Appel au service pour la logique de modification
                bool isEdited = _departmentService.EditDepartment(departmentId, departmentDTO);

                if (!isEdited)
                {
                    return NotFound($"Le département avec l'ID {departmentId} n'a pas été trouvé.");
                }

                // 4. Persistance des données
                _departmentService.SaveChanges();

                return Ok(departmentDTO);
            }
            catch (ArgumentException ex)
            {
                // Capture les erreurs de logique (ex: ID dans l'URL vs ID dans le DTO)
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // 5. Gestion des erreurs imprévues (évite de renvoyer l'objet Exception complet)
                // Utile pour éviter l'erreur de sérialisation 'System.Type' vue précédemment
                return StatusCode(500, new
                {
                    message = "Une erreur interne est survenue lors de la mise à jour.",
                    details = ex.Message
                });
            }
        }

        /// <summary>
        /// delete department from db
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>

        [HttpDelete("{departmentId}")]
        public ActionResult DeleteDepartment([FromRoute] int departmentId)
        {
            bool isDeleted = _departmentService.DeleteDepartment(departmentId);
            if (!isDeleted)
            {
                return NotFound("Department Not Found!!!!!!");
            }
            _departmentService.SaveChanges();
            return Ok("Department Deleted Successfully");
        }
    }
}
