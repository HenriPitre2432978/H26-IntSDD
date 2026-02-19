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
        public ObservableCollection<Gender> _genders =new();
        public ObservableCollection<Gender> Genders
        {
            get => _genders;
            set
            {
                _genders = value;
                OnPropertyChanged(nameof(Genders));
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
                    StudentsView?.Refresh();
                }
            }
        }
        #endregion

        #region Instanciation des commandes

        #endregion

        // Constructeur
        public Handler()
        {
            StudentsView = CollectionViewSource.GetDefaultView(Students);
            StudentsView.Filter = FilterStudents;

            _ = LoadStudentsAsync(); // NOTE: _ permet de contourner le await impossible dans une méthode pas async, en mettant une variable dumped par après
            _ = LoadGendersAsync(); // NOTE: _ permet de contourner le await impossible dans une méthode pas async, en mettant une variable dumped par après
        }

        #region Méthodes relatives aux élèves
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