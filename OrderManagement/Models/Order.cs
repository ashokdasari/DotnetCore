using System;
using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Models
{
    public class Order
    {
        public int OrderID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string FirstName { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string LastName { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string City { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string State { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date of Birth")]
        public DateTime DOB { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

    }
}
