using System;
using System.Collections.Generic;
using System.Text;

namespace CETAutomation.Export
{
    public class ExportAttribute:Attribute
    {
        /// <summary>
        /// Define property is exported into excel or not
        /// </summary>
        public bool IsAllowExport { get; set; }
    }
}
