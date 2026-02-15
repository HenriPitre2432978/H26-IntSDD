using StudentClient.Models;
using System.ComponentModel;
using System.Windows;

namespace StudentClient.Views
{
    /// <summary>
    /// Logique d'interaction pour students.xaml
    /// </summary>
    public partial class Students : Window
    {
        /*
  NOTE M. EMMANUEL:

        J'ai eu un problème avec ma requête GetAsync (voir le fichier StudentHelper.cs) et même avec votre aide, nous n'avons pas réussi à régler le problème.

 Au cours de ce sommatif, je n'ai pas eu le temps de déceler le problème exact et de le régler, alors rien ne s'affiche dans le ListView et au démarrage de mon projet. 
 Pour tenter de gagner des points, j'ai tout de même continué de coder la logique en faisant comme si tout fonctionnait, 
 mais je n'ai pas eu le temps de finir car j'ai passé une bonne partie du temps à essayer de régler le problème source.

 Alternativement, si vous estimez que vous ne pouvez pas évaluer grand chose de ce sommatif, 
 vous pouvez regarder mon Sommatif1 Partie A où je fais la consommation d'API sans problème pour la section département.
 Je l'avais fait à l'avance dans le but de mieux comprendre et me préparer pour aujourd'hui, mais il semblerait que le GetAsync m'en a empêché. Bonne journée.

  */

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

        public Students()
        {
            this.DataContext = new StudentList();
            InitializeComponent();
        }
    }
}
