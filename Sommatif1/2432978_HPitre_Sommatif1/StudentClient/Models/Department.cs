namespace StudentClient.Models
{
    public class Department
    {
        public int DepartmentID { get; set; }
        public string Name { get; set; }

        public override string ToString() => $"{DepartmentID} | {Name}";
    }
}
