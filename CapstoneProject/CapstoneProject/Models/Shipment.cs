//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CapstoneProject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Shipment
    {
        public int ShipmentID { get; set; }
        public System.DateTime ShipmentDate { get; set; }
        public string ShippingServiceName { get; set; }
        public string TrackingNumber { get; set; }
        public int OrderID { get; set; }
        public string ShippingClerkID { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual Order Order { get; set; }
    }
}
