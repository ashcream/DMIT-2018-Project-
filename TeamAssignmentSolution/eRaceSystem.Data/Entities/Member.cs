namespace eRaceSystem.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Member
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Member()
        {
            Cars = new HashSet<Car>();
            RaceDetails = new HashSet<RaceDetail>();
        }

        public int MemberID { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        [StringLength(30, ErrorMessage = "LastName is limited to 30 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "FirstName is required")]
        [StringLength(30, ErrorMessage = "FirstName is limited to 30 characters")]
        public string FirstName { get; set; }

        [StringLength(10, ErrorMessage = "Phone is limited to 10 characters")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(30, ErrorMessage = "Address is limited to 30 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(30, ErrorMessage = "City is limited to 30 characters")]
        public string City { get; set; }

        [Required(ErrorMessage = "PostalCode is required")]
        [StringLength(6, ErrorMessage = "PostalCode is limited to 6 characters")]
        public string PostalCode { get; set; }

        [StringLength(30, ErrorMessage = "EmailAddress is limited to 30 characters")]
        public string EmailAddress { get; set; }

        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "CertificationLevel is required")]
        [StringLength(1, ErrorMessage = "CertificationLevel is limited to 1 characters")]
        public string CertificationLevel { get; set; }

        [StringLength(1)]
        public string Gender { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Car> Cars { get; set; }

        public virtual Certification Certification { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RaceDetail> RaceDetails { get; set; }
    }
}
