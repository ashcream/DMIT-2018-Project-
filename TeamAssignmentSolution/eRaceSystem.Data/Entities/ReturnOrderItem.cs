namespace eRaceSystem.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ReturnOrderItem
    {
        public int ReturnOrderItemID { get; set; }

        public int ReceiveOrderID { get; set; }

        public int? OrderDetailID { get; set; }

        
        [StringLength(50, ErrorMessage = "UnOrderedItem is limited to 50 characters")]
        public string UnOrderedItem { get; set; }

        public int ItemQuantity { get; set; }

        [StringLength(100, ErrorMessage = "Comment is limited to 100 characters")]
        public string Comment { get; set; }

        [StringLength(25, ErrorMessage = "VendorProductID is limited to 25 characters")]
        public string VendorProductID { get; set; }

        public virtual OrderDetail OrderDetail { get; set; }

        public virtual ReceiveOrder ReceiveOrder { get; set; }
    }
}
