using Mappers;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO;
using Services;

namespace StudentWebApi.Controllers
{
    [ApiController]
    [Route("Course")]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IStudentCourseService _studentCourseService;
        private readonly ICourseMapper _courseMapper;

        public CourseController(ICourseService courseService, IStudentCourseService studentCourseService, ICourseMapper courseMapper)
        {
            _studentCourseService = studentCourseService;
            _courseService = courseService;
            _courseMapper = courseMapper;
        }

        /// <summary>
        /// get course list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetCourses()
        {
            List<CourseDTO> result = _courseService.GetAll();
            return Ok(result);
        }

        /// <summary>
        /// get course by id
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        [HttpGet("{courseId}")]
        public ActionResult GetCourseById([FromRoute] int courseId)
        {
            CourseDTO? course = _courseService.GetById(courseId);
            return course == null ? NotFound() : Ok(course);
        }

        /// <summary>to Db
        /// add course 
        /// </summary>
        /// <param name="courseDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddCourse([FromBody] CourseDTO courseDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please Add Course");
            }

            try
            {
                // On tente l'ajout
                Course AddCourse = _courseService.AddCourse(courseDTO);
                _courseService.SaveChanges();

                CourseDTO courseDTOResult = _courseMapper.MapToCourseDTO(AddCourse);
                return Ok(courseDTOResult);
            }
            catch (Exception e)
            {
                // On vérifie si l'erreur vient d'une violation de clé étrangère (DepartmentID)
                // L'exception interne contient souvent le message de MySQL
                if (e.InnerException != null && e.InnerException.Message.Contains("FOREIGN KEY"))
                {
                    return StatusCode(400, new
                    {
                        message = $"L'ajout a échoué : Le département avec l'ID {courseDTO.DepartmentID} n'existe pas.",
                        error = "Invalid DepartmentID"
                    });
                }

                // On renvoie un message simple au lieu de l'objet Exception 'e' complet
                return StatusCode(500, new
                {
                    message = "Une erreur interne est survenue lors de la création du cours.",
                    details = e.Message // On ne renvoie que le message, pas l'objet Type
                });
            }
        }

        /// <summary>
        /// edit course data in db
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="courseDTO"></param>
        /// <returns></returns>
        [HttpPut("{courseId}")]
        public ActionResult EditCourse([FromRoute] int courseId, [FromBody] CourseDTO courseDTO)
        {
            if (courseDTO == null) return BadRequest("Données invalides.");

            try
            {
                bool isEdited = _courseService.EditCourse(courseId, courseDTO);
                if (!isEdited)
                {
                    return NotFound($"Le cours avec l'ID {courseId} n'existe pas.");
                }

                _courseService.SaveChanges();
                return Ok(courseDTO);
            }
            catch (Exception e)
            {
                // Gestion de l'erreur si le DepartmentID fourni n'existe pas
                if (e.InnerException?.Message.Contains("FOREIGN KEY") == true)
                {
                    return BadRequest(new { message = $"Le département ID {courseDTO.DepartmentID} est invalide." });
                }
                return StatusCode(500, new { message = "Erreur lors de la modification", details = e.Message });
            }
        }

        /// <summary>
        /// delete course from db
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>

        [HttpDelete("{courseId}")]
        public ActionResult DeleteCourse([FromRoute] int courseId)
        {
            bool isDeleted = _courseService.DeleteCourse(courseId);
            if (!isDeleted)
            {
                return NotFound("Course Not Found!!!!!!");
            }
            _courseService.SaveChanges();
            return Ok("Course Deleted Successfully");
        }

        [HttpPost("registerStudentCourse/{courseId}/{studentId}")]
        public ActionResult RegisterStudentCourse([FromRoute] int courseId, [FromRoute] int studentId)
        {
            try
            {
                _studentCourseService.AddStudentCourse(courseId, studentId);
                _studentCourseService.SaveChanges();
                return Ok(new { message = "Étudiant inscrit au cours avec succès." });
            }
            catch (Exception e)
            {
                // Capture si l'étudiant ou le cours n'existe pas (FK) ou si déjà inscrit (Duplicate)
                return BadRequest(new { message = "Inscription impossible. Vérifiez si l'étudiant/cours existe ou si l'étudiant est déjà inscrit.", details = e.Message });
            }
        }


        [HttpDelete("deleteStudentCourse/{courseId}/{studentId}")]
        public ActionResult DeleteStudentCourse([FromRoute] int courseId, [FromRoute] int studentId)
        {
            try
            {
                // CORRECTION : Appel de la méthode Delete (assurez-vous qu'elle existe dans votre service)
                bool isDeleted = _studentCourseService.DeleteStudentCourse(courseId, studentId);

                if (!isDeleted) return NotFound("L'inscription spécifiée n'existe pas.");

                _studentCourseService.SaveChanges();
                return Ok(new { message = "Étudiant retiré du cours avec succès." });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = "Erreur lors de la désinscription", details = e.Message });
            }
        }

        /// <summary>
        /// get all students in a course
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        [HttpGet("StudentsByCourse/{courseId}")]
        public ActionResult StudentsByCourseId([FromRoute] int courseId)
        {
            List<StudentDTO> students = _courseService.GetStudentsByCourseId(courseId);
            return Ok(students);
        }
    }
}
