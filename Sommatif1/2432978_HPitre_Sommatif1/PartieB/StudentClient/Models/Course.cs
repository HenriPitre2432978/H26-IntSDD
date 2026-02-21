using StudentClient.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentClient.Models
{
    public class Course : BaseViewModel
    {
        public int CourseID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Credits { get; set; } 
        public int DepartmentID { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
    }
}
