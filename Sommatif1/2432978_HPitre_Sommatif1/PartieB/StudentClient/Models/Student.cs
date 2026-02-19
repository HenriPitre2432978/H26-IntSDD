using StudentClient.Consumables;

namespace StudentClient.Models
{
    public class Student
    {
        public int StudentID { get; set; }
        public string FullName { get; set; }
        public string FullNameFormatted { get => FullName.Split(' ')[1] + ", " + FullName.Split(' ')[0]; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public int GenderID { get; set; }

        public string Gender {get;set;}

    }
}

