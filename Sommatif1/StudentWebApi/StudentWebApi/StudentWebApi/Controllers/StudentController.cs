using Mappers;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO;
using Services;
using System.Web.Http.ModelBinding;
using Microsoft.AspNetCore.JsonPatch;



namespace StudentWebApi.Controllers
{
    [ApiController]
    [Route("Student")]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IStudentMapper _studentMapper;

        public StudentController(IStudentService studentService, IStudentMapper studentMapper)
        {
            _studentService = studentService;
            _studentMapper = studentMapper;
        }

        /// <summary>
        /// get student list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetStudents()
        {
            List<StudentDTO> result = _studentService.GetAll();
            return Ok(result);
        }

        /// <summary>
        /// get student by id
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        [HttpGet("{studentId}")]
        public ActionResult GetStudentById([FromRoute] int studentId)
        {
            StudentDTO? student = _studentService.GetById(studentId);
            return student == null ? NotFound() : Ok(student);
        }

        /// <summary>to Db
        /// add student 
        /// </summary>
        /// <param name="studentDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddStudent([FromBody] StudentDTO studentDTO)
        {
            // Validation du Genre obligatoire
            if (studentDTO.GenderID <= 0)
            {
                return BadRequest(new { message = "Le champ 'GenderID' est obligatoire et doit être valide." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Student addedStudent = _studentService.AddStudent(studentDTO);
                _studentService.SaveChanges();

                StudentDTO result = _studentMapper.MapToStudentDTO(addedStudent);
                return CreatedAtAction(nameof(GetStudentById), new { studentId = result.StudentID }, result);
            }
            catch (Exception e)
            {
                // Erreur de clé étrangère si le GenderID n'existe pas en base
                if (e.InnerException?.Message.Contains("FOREIGN KEY") == true)
                {
                    return BadRequest(new { message = "Le GenderID fourni n'existe pas dans la base de données." });
                }
                // Jamais de 'return BadRequest(e)' pour éviter l'erreur System.Type
                return StatusCode(500, new { message = "Erreur interne", details = e.Message });
            }
        }

        /// <summary>
        /// edit student data in db
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="studentDTO"></param>
        /// <returns></returns>
        [HttpPut("{studentId}")]
        public ActionResult EditStudent([FromRoute] int studentId, [FromBody] StudentDTO studentDTO)
        {
            if (studentDTO == null) return BadRequest("Données manquantes.");

            try
            {
                bool isEdited = _studentService.EditStudent(studentId, studentDTO);
                if (!isEdited)
                {
                    return NotFound(new { message = $"Étudiant avec l'ID {studentId} introuvable." });
                }

                _studentService.SaveChanges();
                return Ok(studentDTO);
            }
            catch (Exception e)
            {
                if (e.InnerException?.Message.Contains("FOREIGN KEY") == true)
                {
                    return BadRequest(new { message = "Modification impossible : GenderID invalide." });
                }
                return StatusCode(500, new { message = "Erreur lors de la mise à jour", details = e.Message });
            }
        }





        // ...

        [HttpPatch("{studentId}")]
        public ActionResult PatchStudent([FromRoute] int studentId, [FromBody] JsonPatchDocument<StudentDTO> patchDoc)
        {
            // 1. Vérification si le document de patch est présent
            if (patchDoc == null) return BadRequest("Données de modification manquantes.");

            try
            {
                // 2. Récupérer les données actuelles de l'étudiant
                var studentDTO = _studentService.GetById(studentId);
                if (studentDTO == null)
                {
                    return NotFound(new { message = $"Étudiant avec l'ID {studentId} introuvable." });
                }

                // 3. Appliquer les modifications partielles au DTO récupéré
                patchDoc.ApplyTo(studentDTO);

                // Vous pouvez ensuite valider manuellement votre DTO si nécessaire
                if (!TryValidateModel(studentDTO))
                {
                    return BadRequest(ModelState);
                }
                // 4. Valider le résultat après l'application du patch
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // 5. Utiliser ta logique existante d'édition
                bool isEdited = _studentService.EditStudent(studentId, studentDTO);
                if (!isEdited)
                {
                    return NotFound(new { message = $"Erreur lors de l'application du patch : Étudiant {studentId} introuvable." });
                }

                _studentService.SaveChanges();
                return Ok(studentDTO);
            }
            catch (Exception e)
            {
                // Gestion cohérente avec ton modèle PUT pour les clés étrangères (ex: GenderID)
                if (e.InnerException?.Message.Contains("FOREIGN KEY") == true)
                {
                    return BadRequest(new { message = "Modification partielle impossible : Données liées invalides (ex: GenderID)." });
                }

                return StatusCode(500, new { message = "Erreur lors de la mise à jour partielle (PATCH)", details = e.Message });
            }
        }

        /// <summary>
        /// delete student from db
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>

        [HttpDelete("{studentId}")]
        public ActionResult DeleteStudent([FromRoute] int studentId)
        {
            bool isDeleted = _studentService.DeleteStudent(studentId);
            if (!isDeleted)
            {
                return NotFound("Student Not Found!!!!!!");
            }
            _studentService.SaveChanges();
            return Ok("Student Deleted Successfully");
        }

        /// <summary>
        /// get all courses for a student
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        [HttpGet("StudentsByCourse/{courseId}")]
        public ActionResult CoursesByCourseId([FromRoute] int studentId)
        {
            List<CourseDTO> courses = _studentService.GetCoursesByStudentId(studentId);
            return Ok(courses);
        }
    }
}
