using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Extensions;
using Microsoft.Xrm.Sdk.Query;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Services.Description;
using TeBS_CC_WebAPIDev.Models;
using TeBS_CC_WebAPIDev.Services;

namespace TeBS_CC_WebAPIDev.Controllers
{
    [RoutePrefix("api/rental")]
    public class RentalController : ApiController
    {
        private static readonly CrmContext crmCtx = new CrmContext();
        private readonly IOrganizationService _crmCtx = crmCtx.GetCrmContext();

        [HttpGet]
        [Route("vehicles")]
        public IHttpActionResult GetVehicles()
        {
            //Get Vehicles
            #region Query
            // Define Condition Values
            var queryCr90dFuelCapacity = 0;

            // Instantiate QueryExpression query
            var query = new QueryExpression("cr90d_vehicle");

            // Add columns to query.ColumnSet
            query.ColumnSet.AddColumns("cr90d_vehicleid", "cr90d_vehiclemodel", "createdon", "cr90d_vehiclemanufacturer", "statuscode", "statecode", "overriddencreatedon", "cr90d_passengercapacity", "owningbusinessunit", "ownerid", "modifiedon", "modifiedonbehalfby", "modifiedby", "cr90d_fueleffeciency", "cr90d_fuelcapacity", "createdonbehalfby", "createdby", "cr90d_categoryid");
            query.AddOrder("cr90d_vehiclemodel", OrderType.Ascending);

            // Define filter query.Criteria
            query.Criteria.AddCondition("cr90d_fuelcapacity", ConditionOperator.NotEqual, queryCr90dFuelCapacity);
            #endregion

            var res = _crmCtx.RetrieveMultiple(query);
            if (res.Entities.Count != 0)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest("No vehicles found");
            }
        }

        [HttpPost]
        [Route("booking")]
        public IHttpActionResult PostBooking(Booking bookingStr)
        {
            //Data
            EntityReference userEntRef = new EntityReference();
            EntityReference vehEntRef = new EntityReference();

            //Retrieve UserId Entity Reference
            #region Query
            // Define Condition Values
            var queryInternalEmailAddress = bookingStr.UserEmail;

            // Instantiate QueryExpression query
            var queryUser = new QueryExpression("systemuser");

            // Add columns to query.ColumnSet
            queryUser.ColumnSet.AddColumns("fullname", "businessunitid", "title", "address1_telephone1", "positionid", "systemuserid");
            queryUser.AddOrder("fullname", OrderType.Ascending);

            // Define filter query.Criteria
            queryUser.Criteria.AddCondition("internalemailaddress", ConditionOperator.Equal, queryInternalEmailAddress);
            #endregion

            var res = _crmCtx.RetrieveMultiple(queryUser);
            if (res.Entities.Count != 0)
            {
                userEntRef = (EntityReference)res.Entities[0].ToEntityReference();
            }
            else
            {
                return BadRequest("User Email not found");
            }

            //Retrieve VehicleId Entity Reference
            #region Query
            // Define Condition Values
            var queryCr90dVehicleId = bookingStr.VehicleId;

            // Instantiate QueryExpression query
            var queryVehicle = new QueryExpression("cr90d_vehicle");

            // Add columns to query.ColumnSet
            queryVehicle.ColumnSet.AddColumns("cr90d_vehicleid", "cr90d_vehiclemodel", "createdon");
            queryUser.AddOrder("cr90d_vehiclemodel", OrderType.Ascending);

            // Define filter query.Criteria
            queryVehicle.Criteria.AddCondition("cr90d_vehicleid", ConditionOperator.Equal, queryCr90dVehicleId);
            #endregion

            res = _crmCtx.RetrieveMultiple(queryVehicle);
            if (res.Entities.Count != 0)
            {
                vehEntRef = (EntityReference)res.Entities[0].ToEntityReference();
            }
            else
            {
                return BadRequest("Vehicle not found");
            }

            //Create Booking Record
            Entity bookingEnt = new Entity("cr90d_booking");
            bookingEnt.Id = Guid.NewGuid();
            bookingEnt.Attributes["cr90d_name"] = bookingStr.Name;
            bookingEnt.Attributes["cr90d_bookingstartdate"] = bookingStr.BookingStartDate;
            bookingEnt.Attributes["cr90d_bookingenddate"] = bookingStr.BookingEndDate;
            bookingEnt.Attributes["cr90d_userid"] = userEntRef;
            bookingEnt.Attributes["cr90d_vehicleid"] = vehEntRef;

            //Save Changes
            var crmBooking = _crmCtx.Create(bookingEnt);
            return Ok("Successfully sent booking request");
        }
    }
}
