namespace eRaceSystem.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Employee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            Invoices = new HashSet<Invoice>();
            Orders = new HashSet<Order>();
            ReceiveOrders = new HashSet<ReceiveOrder>();
            SalesCartItems = new HashSet<SalesCartItem>();
        }

        public int EmployeeID { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        [StringLength(30, ErrorMessage = "LastName is limited to 30 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "FirstName is required")]
        [StringLength(30, ErrorMessage = "FirstName is limited to 30 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(30, ErrorMessage = "Address is limited to 30 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(30, ErrorMessage = "City is limited to 30 characters")]
        public string City { get; set; }

        [Required(ErrorMessage = "PostalCode is required")]
        [StringLength(6, ErrorMessage = "PostalCode is limited to 6 characters")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [StringLength(10, ErrorMessage = "Phone is limited to 10 characters")]
        public string Phone { get; set; }

        public int PositionID { get; set; }

        [StringLength(50)]
        public string LoginId { get; set; }

        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "SocialInsuranceNumber is required")]
        [StringLength(9, ErrorMessage = "SocialInsuranceNumber is limited to 9 characters")]
        public string SocialInsuranceNumber { get; set; }

        public virtual Position Position { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invoice> Invoices { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReceiveOrder> ReceiveOrders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesCartItem> SalesCartItems { get; set; }
    }
}
