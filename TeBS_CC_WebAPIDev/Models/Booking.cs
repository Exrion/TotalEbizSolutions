using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeBS_CC_WebAPIDev.Models
{
    public class Booking
    {
        public string Name { get; set; }
        public DateTime BookingStartDate { get; set; }
        public DateTime BookingEndDate { get; set; }
        public string UserEmail { get; set; }
        public Guid VehicleId { get; set; }
    }
}