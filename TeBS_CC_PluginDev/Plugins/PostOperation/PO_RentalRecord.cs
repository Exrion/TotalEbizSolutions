using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System;
using SendGrid;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Tooling.Connector;
using TeBS_CC_PluginDev.Services;
using SendGrid.Helpers.Mail;

namespace TeBS_CC_PluginDev.Plugins.PostOperation
{
    public class PO_RentalRecord : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            // Obtain the tracing service
            ITracingService tracingService =
            (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Obtain the execution context from the service provider.  
            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));

            // The InputParameters collection contains all the data passed in the message request.  
            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.  
                Entity entity = (Entity)context.InputParameters["Target"];

                // Obtain the organization service reference which you will need for  
                // web service calls.  
                IOrganizationServiceFactory serviceFactory =
                    (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                try
                {
                    #region Entity Booking - CREATE RentalRecord
                    if (entity.LogicalName == "cr90d_booking")
                    {
                        //Form Submission Values
                        string name;
                        double cost = 0;
                        DateTime startdate;
                        DateTime enddate;
                        EntityReference userEntRef;
                        EntityReference vehicleEntRef;
                        Entity userEnt = new Entity();
                        Entity vehicleEnt = new Entity();
                        Guid rentalvehicleid = Guid.Empty;
                        try
                        {
                            name = entity.Attributes["cr90d_name"].ToString();
                            startdate = Convert.ToDateTime(entity.Attributes["cr90d_bookingstartdate"]);
                            enddate = Convert.ToDateTime(entity.Attributes["cr90d_bookingenddate"]);
                            userEntRef = (EntityReference)entity.Attributes["cr90d_userid"];
                            vehicleEntRef = (EntityReference)entity.Attributes["cr90d_vehicleid"];
                        }
                        catch(FormatException e)
                        {
                            throw new InvalidPluginExecutionException("An error has occurred: " + e.Message);
                        }

                        /*OptionSet Values - cr90d_billstatus
                        393790000 = Failed
                        393790001 = Succeeded
                        393790002 = Processing*/
                        OptionSetValue billstatus = new OptionSetValue(393790002); //Processing

                        //Find first available vehicle 
                        #region Legacy Query
                        /*QueryExpression query = new QueryExpression("cr90d_rentalvehicle");

                        ConditionExpression condVehId = new ConditionExpression(); //Match Vehicle ID
                        condVehId.AttributeName = "cr90d_rentalvehicleid";
                        condVehId.Operator = ConditionOperator.Equal;
                        condVehId.Values.Add(vehicleEnt.Id);

                        ConditionExpression condIsRented = new ConditionExpression(); //Match that it is not rented
                        condIsRented.AttributeName = "cr90d_isrented";
                        condIsRented.Operator = ConditionOperator.Equal;
                        condIsRented.Values.Add(false);

                        FilterExpression queryFilter = new FilterExpression(); 
                        queryFilter.Conditions.Add(condVehId); 
                        queryFilter.AddFilter(LogicalOperator.And); //Ensure both are true
                        queryFilter.Conditions.Add(condIsRented);

                        query.Criteria.AddFilter(queryFilter); //Add filter to query
                        query.ColumnSet = new ColumnSet(true); //All columns*/
                        #endregion
                        #region Query
                        // Define Condition Values
                        var query_0_cr90d_vehicleid = vehicleEntRef.Id;
                        var query_0_cr90d_isrented = true;

                        // Instantiate QueryExpression query
                        var queryVeh = new QueryExpression("cr90d_rentalvehicle");

                        // Add columns to query.ColumnSet
                        queryVeh.ColumnSet.AddColumns("cr90d_rentalvehicleid", "cr90d_name", "createdon");
                        queryVeh.AddOrder("cr90d_name", OrderType.Ascending);

                        // Define filter query.Criteria
                        var query_Criteria_0 = new FilterExpression();
                        queryVeh.Criteria.AddFilter(query_Criteria_0);

                        // Define filter query_Criteria_0
                        query_Criteria_0.AddCondition("cr90d_vehicleid", ConditionOperator.Equal, query_0_cr90d_vehicleid);
                        query_Criteria_0.AddCondition("cr90d_isrented", ConditionOperator.NotEqual, query_0_cr90d_isrented);

                        EntityCollection res = service.RetrieveMultiple(queryVeh);
                        if (res.Entities.Count != 0)
                        {
                            rentalvehicleid = res.Entities[0].Id; 
                        }
                        else
                        {
                            //TODO: Add vehicle record for no vehicle available
                            billstatus = new OptionSetValue(393790000); //Failed
                        }
                        #endregion

                        //Find Vehicle Type
                        #region Query
                        // Define Condition Values
                        var query_cr90d_vehicleid = vehicleEntRef.Id;

                        // Instantiate QueryExpression query
                        var queryVehType = new QueryExpression("cr90d_vehicle");

                        // Add columns to query.ColumnSet
                        queryVehType.ColumnSet.AddColumns("cr90d_vehicleid", "cr90d_vehiclemodel", "createdon", "cr90d_categoryid");
                        queryVehType.AddOrder("cr90d_vehiclemodel", OrderType.Ascending);

                        // Define filter query.Criteria
                        queryVehType.Criteria.AddCondition("cr90d_vehicleid", ConditionOperator.Equal, query_cr90d_vehicleid);
                        
                        res = service.RetrieveMultiple(queryVehType);
                        if (res.Entities.Count != 0)
                        {
                            vehicleEnt = res.Entities[0];
                        }
                        #endregion

                        //Find vehicle cateogry cost
                        #region Query
                        // Category Entity Ref
                        EntityReference categoryEnt = (EntityReference)vehicleEnt.Attributes["cr90d_categoryid"];

                        // Define Condition Values
                        var query_cr90d_categoryid = categoryEnt.Id;

                        // Instantiate QueryExpression query
                        var queryCat = new QueryExpression("cr90d_category");

                        // Add columns to query.ColumnSet
                        queryCat.ColumnSet.AddColumns("cr90d_categoryid", "cr90d_categoryname", "createdon", "cr90d_categorycost");
                        queryCat.AddOrder("cr90d_categoryname", OrderType.Ascending);

                        // Define filter query.Criteria
                        queryCat.Criteria.AddCondition("cr90d_categoryid", ConditionOperator.Equal, query_cr90d_categoryid);

                        res = service.RetrieveMultiple(queryCat);
                        if (res.Entities.Count != 0)
                        {
                            decimal catCost = ((Money)res.Entities[0].Attributes["cr90d_categorycost"]).Value;
                            cost = double.Parse(catCost.ToString());

                            double dateDiff = Math.Round((enddate - startdate).TotalDays);
                            if (dateDiff > 1)
                            {
                                cost *= dateDiff;
                            }
                            else
                            {
                                cost *= 1;
                            }
                        }
                        else
                        {
                            //TODO: Add vehicle record for no vehicle available
                            billstatus = new OptionSetValue(393790000); //Failed
                        }
                        #endregion

                        //Create Billing Object
                        Entity billEntity = new Entity("cr90d_billing");
                        billEntity.Id = Guid.NewGuid();
                        billEntity["cr90d_billstatus"] = billstatus;

                        //Create Rental Record Object
                        Entity rentalRecordEntity = new Entity("cr90d_rentalrecord");
                        rentalRecordEntity.Id = Guid.NewGuid();
                        rentalRecordEntity["cr90d_name"] = name;
                        rentalRecordEntity["cr90d_cost"] = decimal.Parse(cost.ToString());
                        rentalRecordEntity["cr90d_isapproved"] = false;
                        rentalRecordEntity["cr90d_startdate"] = startdate;
                        rentalRecordEntity["cr90d_enddate"] = enddate;
                        rentalRecordEntity["cr90d_userid"] = userEntRef;
                        rentalRecordEntity["cr90d_rentalvehicleid"] =
                            new EntityReference("cr90d_rentalvehicle", rentalvehicleid);
                        rentalRecordEntity["cr90d_billingid"] =
                            new EntityReference("cr90d_billing", billEntity.Id);

                        //Save Changes
                        var crmBilling = service.Create(billEntity);
                        var crmRentalRecord = service.Create(rentalRecordEntity);

                        #region Email
                        //Retrieve User Info
                        #region Query
                        // Define Condition Values
                        var query_systemuserid = userEntRef.Id;

                        // Instantiate QueryExpression query
                        var queryUser = new QueryExpression("systemuser");

                        // Add columns to query.ColumnSet
                        queryUser.ColumnSet.AddColumns("fullname", "businessunitid", "title", "address1_telephone1", "positionid", "systemuserid", "internalemailaddress");
                        queryUser.AddOrder("fullname", OrderType.Ascending);

                        // Define filter query.Criteria
                        queryUser.Criteria.AddCondition("systemuserid", ConditionOperator.Equal, query_systemuserid);

                        res = service.RetrieveMultiple(queryUser);
                        if (res.Entities.Count != 0)
                        {
                            userEnt = res.Entities[0];
                        }
                        #endregion

                        //Email Admin
                        string RECORD_URL = "https://org7f3624ff.crm5.dynamics.com/main.aspx?appid=9e6787fd-be42-ed11-bba3-0022485adde6&pagetype=entityrecord&etn=cr90d_rentalrecord&id=";
                        string EMAIL_TO = userEnt.Attributes["internalemailaddress"].ToString();
                        string EMAIL_TO_NAME = userEnt.Attributes["fullname"].ToString();

                        /*EmailModel templateData = new EmailModel()
                        {
                            subject = "New Booking - Awaiting Approval",
                            prehead = "A customer has sent a booking request.",
                            body = "Follow the link to approve or deny the booking request.",
                            url = $"{RECORD_URL}{rentalRecordEntity.Id}"
                        };

                        EmailService EmailClient = new EmailService();
                        EmailClient.SendTemplateMail(
                            EMAIL_TO,
                            EMAIL_TO_NAME,
                            "d-112daae2df47449384fbbe02e3aa8bf4",
                            templateData);*/

                        string _apiKey = "";
                        SendGridClient client = new SendGridClient(_apiKey);

                        EmailModel templateData = new EmailModel()
                        {
                            subject = "New Booking - Awaiting Approval",
                            prehead = "A customer has sent a booking request.",
                            body = "Follow the link to approve or deny the booking request.",
                            url = "https://org7f3624ff.crm5.dynamics.com/main.aspx?appid=9e6787fd-be42-ed11-bba3-0022485adde6&pagetype=entityrecord&etn=cr90d_rentalrecord&id=c5a16ffd-f14f-41d4-a627-2cbac62768a1"
                        };

                        var msg = MailHelper.CreateSingleTemplateEmail(
                            new EmailAddress("titus.lim@totalebizsolutions.com", "TeBS_CC"),
                            new EmailAddress("titus.lim@totalebizsolutions.com", "Titus Lim"),
                            "d-112daae2df47449384fbbe02e3aa8bf4",
                            templateData
                            );
                        var result = client.SendEmailAsync(msg);

                        /*Task sendMailTask = EmailClient.SendMail(
                            new List<string> { EMAIL_TO },
                            "New Booking - Awaiting Approval",
                            "d-112daae2df47449384fbbe02e3aa8bf4",
                            templateData
                            );*/
                        #endregion
                    }
                    #endregion
                }
                catch (InvalidPluginExecutionException e)
                {
                    // catch exception
                    throw new InvalidPluginExecutionException("An error has occurred: " + e.Message);
                }
            }
        }
    }
}