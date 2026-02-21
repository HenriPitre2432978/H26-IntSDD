using StudentClient.Consumables;

namespace StudentClient.Models
{
    public class Student
    {
        public int StudentID { get; set; }
        public string FullName { get; set; }
        public string FullNameFormatted { get => LastName + ", " + FirstName; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public int GenderID { get; set; }

        public string Gender {get;set;}

        public string FirstName
        {
            get => FullName?.Split(' ')[0] ?? "";
            set => FullName = $"{value} {LastName}"; //Quand on change le prénom avec btn Mettre A joru, on reconstruit le fullname 
        }

        public string LastName
        {
            get => FullName?.Split(' ').Length > 1
                ? FullName.Split(' ')[1]
                : "";
            set => FullName = $"{FirstName} {value}"; //Quand on change le prénom avec btn Mettre A joru, on reconstruit le fullname 
        }

    }
}

