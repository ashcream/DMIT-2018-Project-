namespace eRaceSystem.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Car
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Car()
        {
            RaceDetails = new HashSet<RaceDetail>();
        }

        public int CarID { get; set; }

        [Required(ErrorMessage = "Serial Number is required")]
        [StringLength(15, ErrorMessage = "Serial Number is limited to 15 characters")]
        public string SerialNumber { get; set; }

        [Required(ErrorMessage = "Ownership is required")]
        [StringLength(6, ErrorMessage = "Ownership is limited to 6 characters")]
        public string Ownership { get; set; }

        public int CarClassID { get; set; }

        [Required(ErrorMessage = "State is required")]
        [StringLength(10, ErrorMessage = "State is limited to 10 characters")]
        public string State { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(255, ErrorMessage = "Description is limited to 255 characters")]
        public string Description { get; set; }

        public int? MemberID { get; set; }

        public virtual CarClass CarClass { get; set; }

        public virtual Member Member { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RaceDetail> RaceDetails { get; set; }
    }
}
