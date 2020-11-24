using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using eRaceSystem.Data.POCOs.PurchasingPOCO;

namespace eRaceSystem.Data.DTOs.PurchasingDTO
{
    public class InventoryListDTO
    {
        public string Description { get; set; }
        public List<ProductPOCO> ProductList { get; set; }
    }
}
