using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExitGateReportPanel.Models
{
    public class ProductModel
    {
        public List<Product> products { get; set; }

        ///<summary>
        /// Gets or sets CurrentPageIndex.
        ///</summary>
        public int CurrentPageIndex { get; set; }

        ///<summary>
        /// Gets or sets PageCount.
        ///</summary>
        public int PageCount { get; set; }
    }
}
