namespace StudentClient.Models
{
    public class Student
    {
        public int StudentID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public int GenderID { get; set; }

        public override string ToString() => $"{StudentID} - {FullName} | ";
        public string GetGender() //TODO: Lier le genre de la personne à la table dans la Base de données
        {
            string r;
            switch (GenderID)
            {
                case 1:
                    r = "Masculin";
                    break;
                case 2:
                    r = "Féminin";
                    break;
                default:
                    r = "Non-Binaire";
                    break;
            }
            return r;
        }
    }
}

