using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Models;

namespace Repositories
{
    public class DepartmentRepository : BaseRepository, IDepartmentRepository
    {
        private StudentDB _StudentDB;
        public DepartmentRepository(StudentDB studentDB) : base(studentDB)
        {
            _StudentDB = studentDB;
        }

        /// <summary>
        /// get all departments
        /// </summary>
        /// <returns></returns>
        public List<Department> GetAll()
        {
            return _StudentDB.Departments.ToList();
        }

        /// <summary>
        /// get department by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Department? GetById(int id)
        {
            return _StudentDB.Departments.FirstOrDefault(g => g.DepartmentID == id);
        }
        /// <summary>
        /// delete department
        /// </summary>
        /// <param name="department"></param>
        public void DeleteDepartment(Department department)
        {
            _StudentDB.Remove(department);
        }

        /// <summary>
        /// edit department data
        /// </summary>
        /// <param name="departmentId"></param>
        /// <param name="department"></param>
        public bool EditDepartment(int departmentId, Department department)
        {
            // 1. Validation : Vérifier si l'objet envoyé n'est pas nul
            if (department == null)
            {
                throw new ArgumentNullException(nameof(department), "Les données du département sont manquantes.");
            }

            // 2. Validation : Vérifier la cohérence des IDs
            // Si l'ID dans le body est différent de l'ID dans l'URL
            if (department.DepartmentID != 0 && department.DepartmentID != departmentId)
            {
                throw new ArgumentException("L'ID du département dans le corps de la requête ne correspond pas à l'ID de l'URL.");
            }

            // 3. Recherche de l'entité existante
            Department? existingEntity = _StudentDB.Departments.Find(departmentId);
            if (existingEntity == null)
            {
                return false; // Sera géré comme un 404 par le contrôleur
            }

            try
            {
                // Détacher l'entité existante pour éviter les conflits de suivi (tracking)
                _StudentDB.Entry(existingEntity).State = EntityState.Detached;

                // On s'assure que l'ID est bien positionné sur l'objet à modifier
                department.DepartmentID = departmentId;

                _StudentDB.Attach(department);
                _StudentDB.Entry(department).State = EntityState.Modified;

                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                // Gère les accès concurrents si l'entrée a été supprimée entre-temps
                throw new Exception("Le département a été modifié ou supprimé par un autre utilisateur.");
            }
            catch (Exception ex)
            {
                // Capture toute autre erreur (problème de DB, etc.)
                throw new Exception("Une erreur est survenue lors de la mise à jour : " + ex.Message);
            }
        }

        /// <summary>
        /// add department to database
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        public Department AddDepartment(Department department)
        {
            EntityEntry<Department> x = _StudentDB.Departments.Add(department);
            return x.Entity;

        }
    }
}
