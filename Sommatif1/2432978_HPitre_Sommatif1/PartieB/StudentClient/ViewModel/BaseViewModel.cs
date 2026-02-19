 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentClient.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
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
    }
}
