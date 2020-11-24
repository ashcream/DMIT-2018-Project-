namespace eRaceSystem.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class StoreRefund
    {
        [Key]
        public int RefundID { get; set; }

        public int InvoiceID { get; set; }

        public int ProductID { get; set; }

        public int OriginalInvoiceID { get; set; }

        [Required(ErrorMessage = "Reason is required")]
        [StringLength(150, ErrorMessage = "Reason is limited to 150 characters")]
        public string Reason { get; set; }

        public virtual Invoice RefundInvoice { get; set; }

        public virtual Invoice OriginalInvoice { get; set; }

        public virtual Product Product { get; set; }
    }
}
