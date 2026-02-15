using Microsoft.VisualBasic;
using StudentClient.Consumables;
using StudentClient.Models;
using System.Windows;

namespace StudentClient
{
    /// <summary>
    /// Logique d'interaction pour Home.xaml
    /// </summary>
    public partial class DepartmentView: Window
    {
        public DepartmentView() { InitializeComponent(); }

        private async void LoadDepartments_Click(object sender, RoutedEventArgs e)
            => lvDepartments.ItemsSource = await ChargerDepartments();

        private static async Task<List<Department>> ChargerDepartments()
        {
            List<Department> dptList = await DepartmentHelper.GetAllAsync();
            if (dptList == null)
            {
                MessageBox.Show("!ERREUR! Get impossible.");
                return [];
            }
            return dptList;
        }

        private async void AddDepartment_Click(object sender, RoutedEventArgs e)
        {
            string name = Interaction.InputBox("Nom du département :", "Ajouter un département", ""); //TODO: Idéalement un vrai formulaire en View
            if (string.IsNullOrWhiteSpace(name)) return;

            Department dept = new() { Name = name };
            bool success = await DepartmentHelper.PostAsync(dept);

            MessageBox.Show(success ? "Ajout réussi !" : "!ERREUR! Ajout impossible.");
            lvDepartments.ItemsSource = await ChargerDepartments(); //refetch pour voir le changement dans la listeview
        }

        private async void DeleteDepartment_Click(object sender, RoutedEventArgs e)
        {
            if (lvDepartments.SelectedItem is Department selected)
            {
                bool success = await DepartmentHelper.DeleteAsync(selected.DepartmentID);
                MessageBox.Show(success ? "Supprimé !" : "!ERREUR! Suppression impossible.");
                lvDepartments.ItemsSource = await ChargerDepartments(); //refetch pour voir le changement dans la listeview
            }
        }
    }
}
