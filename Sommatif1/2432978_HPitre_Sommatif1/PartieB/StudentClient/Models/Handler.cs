using Microsoft.VisualBasic;
using StudentClient.Consumables;
using StudentClient.ViewModel;
using StudentClient.Views;
using System.CodeDom;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Xml.Linq;

namespace StudentClient.Models
{
    public class Handler : BaseViewModel, INotifyPropertyChanged
    {

        #region Propriétés

        //L'équivalent du contenu d'une ListView mais dynamqiue et avec IPropertyChanegd. Utilisée pour la main list d'élèves
        private ICollectionView _studentsView;
        public ICollectionView StudentsView
        {
            get => _studentsView;
            set
            {
                _studentsView = value;
                OnPropertyChanged(nameof(StudentsView));
            }
        }

        private ObservableCollection<Student> _students = new();
        public ObservableCollection<Student> Students
        {
            get => _students;
            set
            {
                _students = value;
                OnPropertyChanged(nameof(Students));
            }
        }

        public ObservableCollection<Gender> _genders = new();
        public ObservableCollection<Gender> Genders
        {
            get => _genders;
            set
            {
                _genders = value;
                OnPropertyChanged(nameof(Genders));
            }
        }

        public ObservableCollection<Course> _courses = new();
        public ObservableCollection<Course> Courses
        {
            get => _courses;
            set
            {
                _courses = value;
                OnPropertyChanged(nameof(Courses));
            }
        }

        #region Propriétés de séléction actuelle (Current object selected)

        private Course? _courseSelectionne;
        public Course? CourseSelectionne
        {
            get => _courseSelectionne;
            set
            {
                _courseSelectionne = value;
                OnPropertyChanged(nameof(CourseSelectionne));
            }
        }

        private Student? _studentSelectionne = null;
        public Student? StudentSelectionne
        {
            get => _studentSelectionne;
            set
            {
                if (_studentSelectionne != value)
                {
                    _studentSelectionne = value;
                    OnPropertyChanged(nameof(StudentSelectionne));

                    if (value != null)
                        _ = LoadCoursesByStudentAsync(value.StudentID);
                }
            }
        }
        #endregion

        #endregion

        #region Propriétés de Recherche et Logique de Filtrage

        private string? _recherche;
        public string? Recherche
        {
            get => _recherche;
            set
            {
                if (_recherche != value)
                {
                    _recherche = value;
                    OnPropertyChanged(nameof(Recherche));
                    StudentsView?.Refresh();
                }
            }
        }
        /// <summary>
        /// Filtrer les étudiants par leur genre, nom ou ID, en ignorant les accents et la casse
        /// </summary>
        /// <param name="obj">Filter d'une ICollection (Ici, d'un StudentView)</param>
        /// <returns>True si réussite</returns>
        private bool FilterStudents(object obj)
        {
            Student s = (Student)obj;

            if (string.IsNullOrWhiteSpace(Recherche))
                return true;

            string r = Recherche.ToLower();

            //CultureInfo et CompareOption pour rechercher en ignorant la Culture (accents)
            return

                //Search by name
                CultureInfo.CurrentCulture.CompareInfo.IndexOf(
                    s.FullName, Recherche,
                    CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0 ||

                //Search by gender
                CultureInfo.CurrentCulture.CompareInfo.IndexOf(
                    Genders.FirstOrDefault(g => g.GenderID == s.GenderID)?.Name.ToString() ?? "", Recherche,
                    CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0 ||

                //Search by ID
                s.StudentID.ToString().Contains(Recherche);
        }

        #endregion

        /// <summary>
        /// NOTE CORRECTION:
        /// La gestion des cours n'est pas optimisée car elle refait une reqêute GetAllCourses à chaque fois qu'on change d'étudiant selectionné. 
        /// Elle fonctionne tout de même et je suis au courant du problème, mais je manque de temps pour faire mieux.
        /// </summary>
        #region Gestion des Cours

        /// <summary>
        /// Charger tous les cours assignées à un élève spécifié
        /// </summary>
        /// <param name="studentId">Id de l'élève spéciifé</param>
        /// <returns></returns>
        private async Task LoadCoursesByStudentAsync(int studentId)
        {
            var allCourses = await CourseHelper.GetAllAsync();
            if (allCourses == null) return;

            Courses.Clear();

            foreach (var course in allCourses)
            {
                var students = await CourseHelper.GetStudentsByCourseAsync(course.CourseID);

                course.IsSelected =
                    students != null &&
                    students.Any(s => s.StudentID == studentId);

                Courses.Add(course);
            }
        }

        public static async void EditCourse(Course course)
        {
            //Sert littéralement à rien, elle change les propriétés du cours par la même chose. On peut parcontre voir qu'elle marche puisqu'on voit passer la requête dans la console et retourne le code 200
            if (course == null) return;

            bool success = await CourseHelper.PutAsync(course.CourseID, course);  //Remplacer les données du cours avec id de "course" par les données de "course" (boucle inutile)

            MessageBox.Show(success ? "Cours modifié !" : "!ERREUR! Modification impossible du cours.");
        }

        public async void DeleteCourse(Course course)
        {
            if (course == null) return;

            bool success = await CourseHelper.DeleteAsync(course.CourseID);

            if (success)
                Courses.Remove(course);
        }


        /// <summary>
        /// Au clic du bouton "Inscrire aux cours", on vérifie pour chaque cours si il est sélectionné et si l'étudiant est déjà inscrit. Si oui/non, on inscrit/désinsscrit
        /// </summary>
        public async void InscrireCours()
        {
            if (StudentSelectionne == null)
            {
                MessageBox.Show("Sélectionnez un étudiant avant d'inscrire !" );
                return;
            }

            foreach (var course in Courses)
            {
                var students = await CourseHelper.GetStudentsByCourseAsync(course.CourseID);
                bool isInscrit =
                    students != null &&
                    students.Any(s => s.StudentID == StudentSelectionne.StudentID);

                //Si le cours est selefctionné mais l'étudiant n'est pas inscrit, on l'inscrit
                if (course.IsSelected && !isInscrit)
                {
                    await CourseHelper.PostRegisterStdCourseAsync(
                        course.CourseID,
                        StudentSelectionne.StudentID);
                }
                //Si le cours n'est plus selectionné mais que l'étudiant était inscrit, on le désinscrit
                else if (!course.IsSelected && isInscrit) 
                {
                    await CourseHelper.DeleteStudentCourseAsync(
                        course.CourseID,
                        StudentSelectionne.StudentID);
                }
            }

            MessageBox.Show("Mise à jour des inscriptions effectuée !");
        }

        #endregion


        #region Gestion des étudiants/Genders

        #region Gestion d'étudiant sélectionné (individuel)

        /// <summary>
        /// Au clic du bouton Mettre à jour, on envoie une requête Put pour mettre à jour les données de l'étudiant sélectionné avec les données actuellement écrites dans les champs de texte (binding).(Copilot)
        /// </summary>
        public async void MettreAJourStudent()
        {
            if (StudentSelectionne == null)
                return;

            bool success = await StudentHelper.PutAsync(
                StudentSelectionne.StudentID,
                StudentSelectionne
            );

            MessageBox.Show(success ? "Mise à jour réussie !" : "!ERREUR! Modifications d'étudiant impossible.");
            StudentsView.Refresh();
        }

        #endregion

        private async Task LoadGendersAsync()
        {
            List<Gender> list = await GenderHelper.GetAllAsync();

            if (list == null)
            {
                MessageBox.Show("!ERREUR! Get GENDERS impossible.");
                return;
            }

            Genders.Clear();
            foreach (Gender g in list)
                Genders.Add(g);
        }

        private async Task LoadStudentsAsync()
        {
            List<Student> list = await StudentHelper.GetAllAsync();

            if (list == null)
            {
                MessageBox.Show("!ERREUR! Get STUDENTS impossible.");
                return;
            }

            Students.Clear();
            foreach (Student student in list)
            {
                student.Gender = Genders.FirstOrDefault(g => g.GenderID == student.GenderID)?.Name ?? ""; //Get gender corresponding to id of student's genre

                Students.Add(student);
            }
        }

        #endregion

        // Constructeur
        public Handler()
        {
            StudentsView = CollectionViewSource.GetDefaultView(Students);
            StudentsView.Filter = FilterStudents;

            _ = LoadStudentsAsync(); // NOTE: _ permet de contourner le await impossible dans une méthode pas async, en mettant une variable dumped par après
            _ = LoadGendersAsync(); // NOTE: _ permet de contourner le await impossible dans une méthode pas async, en mettant une variable dumped par après
        }
    }
}