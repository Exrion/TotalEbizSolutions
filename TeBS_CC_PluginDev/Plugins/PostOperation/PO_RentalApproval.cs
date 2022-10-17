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
using Stripe;
using System.Runtime.InteropServices.ComTypes;

namespace TeBS_CC_PluginDev.Plugins.PostOperation
{
    public class PO_RentalApproval : IPlugin
    {
        private readonly string DEF_CURRENCY = "sgd";
        private readonly string PRODUCT_NAME_TEMPLATE = "{0} Rental - {1} days";

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
                    #region Entity RentalRecord - UPDATE Billing
                    if (entity.LogicalName == "cr90d_rentalrecord" &&
                        (bool)entity.Attributes["cr90d_isapproved"] == true)
                    {
                        //Init Data
                        string vehCat = "";
                        decimal cost = 0;
                        double days = 0;
                        Entity billingEnt = new Entity();
                        PaymentLink paymentLink = new PaymentLink();

                        //Get Rental Record
                        #region Query
                        // Define Condition Values
                        var query_cr90d_rentalrecordid =
                            new EntityReference("cr90d_rentalrecords", entity.Id);

                        // Instantiate QueryExpression query
                        var queryRecord = new QueryExpression("cr90d_rentalrecord");

                        // Add columns to query.ColumnSet
                        queryRecord.ColumnSet.AddColumns("cr90d_rentalrecordid", "cr90d_name", "createdon", "cr90d_userid", "statuscode", "statecode", "cr90d_startdate", "cr90d_rentalvehicleid", "overriddencreatedon", "owningbusinessunit", "ownerid", "modifiedon", "modifiedonbehalfby", "modifiedby", "cr90d_isapproved", "cr90d_enddate", "createdonbehalfby", "createdby", "cr90d_cost", "cr90d_billingid");
                        queryRecord.AddOrder("cr90d_name", OrderType.Ascending);

                        // Define filter query.Criteria
                        queryRecord.Criteria.AddCondition("cr90d_rentalrecordid", ConditionOperator.Equal, query_cr90d_rentalrecordid);

                        //Retrieve Data
                        var res = service.RetrieveMultiple(queryRecord);
                        if (res.Entities.Count != 0)
                        {
                            DateTime startdate = Convert.ToDateTime(entity.Attributes["cr90d_bookingstartdate"]);
                            DateTime enddate = Convert.ToDateTime(entity.Attributes["cr90d_bookingenddate"]);

                            cost = (decimal)res.Entities[0].Attributes["cr90d_cost"];
                            days = Math.Round((enddate - startdate).TotalDays);
                            EntityReference vehicleEntRef = (EntityReference)res.Entities[0].Attributes["cr90d_rentalvehicleid"];

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
                                Entity vehicleEnt = (Entity)res.Entities[0];

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
                                    vehCat = (string)res.Entities[0].Attributes["cr90d_categoryname"];
                                }
                                #endregion
                            }
                            #endregion
                        }
                        #endregion

                        //Generate Payment Link
                        PaymentGateway paymentGateway = new PaymentGateway();
                        try
                        {
                            paymentLink = paymentGateway.GeneratePaymentLink(
                                string.Format(PRODUCT_NAME_TEMPLATE, vehCat, days.ToString()),
                                cost,
                                DEF_CURRENCY);
                        }
                        catch (StripeException e)
                        {
                            #region Error Catching
                            switch (e.StripeError.Type)
                            {
                                case "card_error":
                                    throw new InvalidPluginExecutionException
                                        ($"Code: {e.StripeError.Code}\bMessage: {e.StripeError.Message}");
                                case "api_connection_error":
                                    throw new InvalidPluginExecutionException
                                        ($"Code: {e.StripeError.Code}\bMessage: {e.StripeError.Message}");
                                case "api_error":
                                    throw new InvalidPluginExecutionException
                                        ($"Code: {e.StripeError.Code}\bMessage: {e.StripeError.Message}");
                                case "authentication_error":
                                    throw new InvalidPluginExecutionException
                                        ($"Code: {e.StripeError.Code}\bMessage: {e.StripeError.Message}");
                                case "invalid_request_error":
                                    throw new InvalidPluginExecutionException
                                        ($"Code: {e.StripeError.Code}\bMessage: {e.StripeError.Message}");
                                case "rate_limit_error":
                                    throw new InvalidPluginExecutionException
                                        ($"Code: {e.StripeError.Code}\bMessage: {e.StripeError.Message}");
                                case "validation_error":
                                    throw new InvalidPluginExecutionException
                                        ($"Code: {e.StripeError.Code}\bMessage: {e.StripeError.Message}");
                                default:
                                    // Unknown Error Type
                                    break;
                            }
                            #endregion
                        }

                        //Update Billing
                        #region Query
                        // Define Condition Values
                        var query_cr90d_billingid = entity.Attributes["cr90d_billingid"];

                        // Instantiate QueryExpression query
                        var queryBilling = new QueryExpression("cr90d_billing");

                        // Add columns to query.ColumnSet
                        queryBilling.ColumnSet.AddColumns("cr90d_billingid", "cr90d_receipturl", "createdon", "statuscode", "statecode", "overriddencreatedon", "modifiedon", "modifiedonbehalfby", "modifiedby", "cr90d_lastfourdigits", "cr90d_funding", "cr90d_fingerprint", "cr90d_expiryyear", "cr90d_expirymonth", "createdonbehalfby", "createdby", "cr90d_cardtype", "cr90d_billstatus");
                        queryBilling.AddOrder("cr90d_receipturl", OrderType.Ascending);

                        // Define filter query.Criteria
                        queryBilling.Criteria.AddCondition("cr90d_billingid", ConditionOperator.Equal, query_cr90d_billingid);

                        res = service.RetrieveMultiple(queryBilling);
                        if (res.Entities.Count != 0)
                        {
                            billingEnt = res.Entities[0];
                        }
                        else
                        {
                            throw new InvalidPluginExecutionException("Query Error: Could not retrieve billin record");
                        }
                        #endregion

                        //Update Billing Record Attributes
                        billingEnt.Attributes["cr90d_paymentlink"] = paymentLink.Url;
                        billingEnt.Attributes["cr90d_createdon"] = DateTime.Now;

                        //Save Changes
                        service.Update(billingEnt);

                        //Email Admin
                        #region Email
                        //Retrieve User Info
                        EntityReference userEntRef = (EntityReference)entity.Attributes["cr90d_userid"];
                        Entity userEnt = new Entity();

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

                        string EMAIL_TO = userEnt.Attributes["internalemailaddress"].ToString();
                        string EMAIL_TO_NAME = userEnt.Attributes["fullname"].ToString();

                        EmailModel templateData = new EmailModel()
                        {
                            subject = "Booking Approved - Begin the Payment Process",
                            prehead = "We have approved your booking request, click the link to begin payment.",
                            body = "Follow the link to pay for your rental. The link will expire 4 hours before the" +
                            " set date and time of your booking.",
                            url = paymentLink.Url
                        };

                        EmailService EmailClient = new EmailService();
                        EmailClient.SendTemplateMail(
                            EMAIL_TO,
                            EMAIL_TO_NAME,
                            "d-112daae2df47449384fbbe02e3aa8bf4",
                            templateData);
                        #endregion

                        #region SMS
                        SMSService SMSClient = new SMSService();
                        SMSClient.SendSMS(
                            $"{templateData.prehead}\b{templateData.body} - {templateData.url}",
                            userEnt.Attributes[""].ToString()
                            );
                        #endregion
                        #endregion
                        //TODO: Ensure booking and bill is cancelled if not done 4 hour prior to booking time
                    }
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