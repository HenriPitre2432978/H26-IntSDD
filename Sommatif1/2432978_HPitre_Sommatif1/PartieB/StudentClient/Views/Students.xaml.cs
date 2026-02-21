using StudentClient.Consumables;
using StudentClient.Models;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace StudentClient.Views
{
    /// <summary>
    /// Logique d'interaction pour students.xaml
    /// </summary>
    public partial class Students : Window
    {

        #region Implémentation INotifyPropertyChanged (Can't use BaseViewModel dans Window)
        /// <summary>
        /// Se produit lorsque la valeur d'une propriété change.
        /// </summary>
        /// <remarks>
        /// Cet événement est généralement déclenché par l'implémentation de l'interface 
        /// <see cref="INotifyPropertyChanged"/> qui notifie les abonnés qu'une valeur de 
        /// propriété a été mise à jour. Utilisez cet événement pour surveiller les 
        /// changements de propriétés dans la classe implémentante.
        /// </remarks>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Lève l'événement <see cref="PropertyChanged"/> pour notifier les abonnés 
        /// qu'une valeur de propriété a changé.
        /// </summary>
        /// <remarks>
        /// Cette méthode doit être appelée chaque fois qu'une valeur de propriété est 
        /// mise à jour afin de s'assurer que les liaisons de données et autres écouteurs 
        /// sont informés du changement.
        /// <param name="name">Nom de la propriété qui a changé.</param>
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        #endregion

        public Students(Handler stdListData)
        {
            DataContext = stdListData;
            InitializeComponent();
        }

        //Dans un contexte de MVVM parfait, il faudrait utiliser des commandes ainsi qu'un ViewModel fort.
        //Étant donné que la note n'est pas basée sur l'implémentation de MVVM, j'utilise des Click pour simplifier la gestion des évènements,
        //et utilise un Handler comme DataContext au lieu de ViewModels dédiés
        private void EditCourse_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is Course course)
                if (DataContext is Handler handler)
                    Handler.EditCourse(course);
        }
        private void DeleteCourse_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is Course course)
                if (DataContext is Handler handler)
                    handler.DeleteCourse(course);
        }

        private void MettreAJour_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is Handler handler)
                handler.MettreAJourStudent();
        }
        private void InscrireCours_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is Handler handler)
                handler.InscrireCours();
        }
    }
}
