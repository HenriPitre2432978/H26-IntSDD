using Microsoft.VisualBasic;
using StudentClient.Consumables;
using StudentClient.Models;
using System.Windows;

namespace StudentClient
{
    /// <summary>
    /// Logique d'interaction pour CourseView.xaml
    /// 
    /// Ceci est une copie modifiée de DepartmentView qui a été créée au préalable, 
    /// avant même que l'on ait les instructions d'ajouter la liste des cours et la possibilité d'inscrire un étudiant.
    /// </summary>
    public partial class CourseView : Window
    {
        public CourseView() { InitializeComponent(); }

        private async void LoadCourses_Click(object sender, RoutedEventArgs e)
            => lvCourses.ItemsSource = await ChargerCourses();

        private static async Task<List<Course>> ChargerCourses()
        {
            List<Course> crsList = await CourseHelper.GetAllAsync();
            if (crsList == null)
            {
                MessageBox.Show("!ERREUR! Get impossible.");
                return [];
            }
            return crsList;
        }

        private async void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            //Demander les IDs du cours et de l’étudiant à inscrire, puis appeler la méthode d’inscription et afficher un message de succès ou d’erreur.
            string idCrsStr = Interaction.InputBox("ID du cours :", "Inscription d’un étudiant");
            string idStdStr = Interaction.InputBox("ID de l’étudiant :", "Inscription d’un étudiant");

            if (!int.TryParse(idCrsStr, out int courseId) || !int.TryParse(idStdStr, out int studentId))
            {
                MessageBox.Show("Les identifiants doivent être des nombres valides.");
                return;
            }

            bool success = await CourseHelper.PostRegisterStdCourseAsync(courseId, studentId);

            MessageBox.Show(success ? "Ajout réussi !" : "!ERREUR! Ajout impossible.");

            lvCourses.ItemsSource = await ChargerCourses(); // Rafraîchir la liste
            if (lvCourses.SelectedItem is Course selectedCourse)
                lvStudentsByCourse.ItemsSource = await CourseHelper.GetStudentsByCourseAsync(selectedCourse.CourseID); //rafraîchir la liste des étudiants 
        }


        /// <summary>
        /// Changer les étudiants affichés dans lvStudentsByCourse en fonction du cours sélectionné dans lvCourses.
        /// </summary>
        private async void lvCourses_SelectionChangedAsync(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (lvCourses.SelectedItem is not Course selectedCourse)
                return;

            lvStudentsByCourse.ItemsSource = null;
            lvStudentsByCourse.ItemsSource = await CourseHelper.GetStudentsByCourseAsync(selectedCourse.CourseID);
        }
    }
}
