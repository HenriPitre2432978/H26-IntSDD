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
using System.Xml.Linq;

namespace StudentClient.Models
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

    public class StudentList : BaseViewModel, INotifyPropertyChanged
    {
        #region Propriétés
        private static List<Student> _students => ChargerStd().Result;
        
        public ObservableCollection<Student> Students
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Recherche))
                    return new ObservableCollection<Student>(_students);
                else
                {
                    ObservableCollection<Student> result = [];
                    string recherche = Recherche.ToLower();

                   foreach (Student s in (IEnumerable<Student>)_students)
                    {
                        if (s.FullName.Contains(recherche) || s.Phone.Contains(recherche) || s.StudentID.ToString().Contains(recherche))
                            result.Add(s);
                    }
                    return result;
                }
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
                }
            }
        }
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
                    OnPropertyChanged(nameof(Students));
                }
            }
        }
        #endregion

        #region Instanciation des commandes

        #endregion

        // Constructeur
        public StudentList()
        {
            //Assignation des commandes

        }

        #region Méthodes relatives aux élèves

        //private async void LoadStudents_Click(object sender, RoutedEventArgs e)
        //    => lvStudents.ItemsSource = await ChargerStudents();

        private static async Task<List<Student>?> ChargerStd()
        {
            List<Student> stdList = await StudentHelper.GetAllAsync(); // CECI NE FONCTIONNE PAS ET EMPÈCHE LE FONCTIONNEMENT ENTIER DU PROGRAMME
            if (stdList == null)
            {
                MessageBox.Show("!ERREUR! Get impossible.");
                return [];
            }
            return stdList;
        }

        private async void AjouterStd(Student s)
        {
            bool success = await StudentHelper.PostAsync(s);

            MessageBox.Show(success ? "Ajout réussi !" : "!ERREUR! Ajout impossible.");
        }

        private async void EffacerStd(object sender, RoutedEventArgs e)
        {
            //if (lvStudents.SelectedItem is Student selected)
            //{
            //    bool success = await StudentApiHelper.DeleteAsync(selected.StudentID);
            //    MessageBox.Show(success ? "Supprimé !" : "!ERREUR! Suppression impossible.");
            //    lvStudents.ItemsSource = await ChargerStudents(); //refetch pour voir le changement dans la listeview
            //}
        }

        //#region Bouton Ajouter
        //public bool AjouterContact(object nouveauContactObj)
        //{
        //    // Vérifier que le paramètre n'est pas null et est de type Contact
        //    if (nouveauContactObj is null || nouveauContactObj is not Contact)
        //        throw new ArgumentNullException();

        //    Contact nouveauContact = (Contact)nouveauContactObj;
        //    // Vérifier que la liste ne contient un contact avec les mêmes nom et prénom
        //    if (_contacts.Any(c => c.EstLeMeme(nouveauContact)))
        //        return false;
        //    else
        //        _contacts.Add((Contact)nouveauContact);
        //    return true;
        //}

        //private void OuvrirFenetreAjouter()
        //{
        //    Contact nouveau = new Contact();

        //    EditionContact fenetre = new EditionContact(nouveau);

        //    //Ouvrir Fenetre Edition Contact
        //    if (fenetre.ShowDialog() == true)
        //    {
        //        if (!AjouterContact(nouveau)) //if ajoutcontact failed
        //            MessageBox.Show($"Le contact {nouveau.Nom}, {nouveau.Prenom} existe déjà ! ", "ERREUR - Carnet de Contacts", MessageBoxButton.OK, MessageBoxImage.Error);
        //        OnPropertyChanged(nameof(Contacts));
        //    }
        //}

        //#endregion

        //#region Bouton Modifier
        //private void OuvrirFenetreModifier(object original)
        //{
        //    if (original is not Contact)
        //        return;

        //    Contact copie = (Contact)original;

        //    EditionContact fenetre = new EditionContact(copie);

        //    if (fenetre.ShowDialog() == true) //If closed avec Accepter
        //    {
        //        ModifierContactSelectionne(copie);
        //        OnPropertyChanged(nameof(Contacts));
        //    }
        //}
        //public void ModifierContactSelectionne(object contactModifie)
        //{
        //    if (ContactSelectionne is null)
        //        throw new ArgumentException("Aucun contact sélectionné.");
        //    if (contactModifie is null)
        //        throw new ArgumentException("Aucune modification appliquée.");
        //    if (_contacts.Contains(ContactSelectionne) == false)
        //        throw new ArgumentException($"Le contact sélectionné n'est pas dans la liste.");
        //    if (ContactSemblableExistant(contactModifie))
        //        throw new ArgumentException($"Le contact {((Contact)contactModifie).Nom}, {((Contact)contactModifie).Prenom} existe déjà.");

        //    ContactSelectionne.Nom = ((Contact)contactModifie).Nom;
        //    ContactSelectionne.Prenom = ((Contact)contactModifie).Prenom;
        //    ContactSelectionne.Telephone = ((Contact)contactModifie).Telephone;
        //    ContactSelectionne.Email = ((Contact)contactModifie).Email;
        //    ContactSelectionne.Adresse = ((Contact)contactModifie).Adresse;
        //    ContactSelectionne.CodePostal = ((Contact)contactModifie).CodePostal;
        //    ContactSelectionne.Ville = ((Contact)contactModifie).Ville;
        //}

        //#endregion

        //#region Bouton Supprimer
        //private void SupprimerContactSelectionne()
        //{
        //    if (PeutSupprimerContactSelectionne() && ContactSelectionne is not null)
        //    {
        //        _contacts.Remove(ContactSelectionne);
        //        ContactSelectionne = null;
        //        OnPropertyChanged(nameof(Contacts));
        //    }
        //}

        //private bool PeutSupprimerContactSelectionne()
        //{
        //    if (ContactSelectionne is null) return false;
        //    if (_contacts.Contains(ContactSelectionne) == false) return false;

        //    return true;
        //}

        //#endregion

        //private bool ContactSemblableExistant(object contactModifie)
        //{
        //    // Vérifier que la modification ne crée pas de doublon
        //    List<Contact> contacts = _contacts.Where(c => c.Nom == ((Contact)contactModifie).Nom && c.Prenom == ((Contact)contactModifie).Prenom).ToList();
        //    foreach (Contact contact in contacts)
        //        if (contact != ContactSelectionne) return true;

        //    return false;
        //}



        #endregion

    }
}