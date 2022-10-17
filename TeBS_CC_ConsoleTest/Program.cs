using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Crm.Sdk.Messages;
using System.Net;
using System.ServiceModel.Description;
using Microsoft.Xrm.Tooling.Connector;

namespace TeBS_CC_PluginConsoleTest
{
    class Program
    {
        //Pretend this is in an env var
        /*private static readonly string D365_USERNAME = "";
        private static readonly string D365_PASSWORD = "";
        private static readonly string D365_URL = "https://org7f3624ff.api.crm5.dynamics.com";*/

        /*static void Main(string[] args)
        {
            IOrganizationService oServiceProxy;
            try
            {
                Console.WriteLine("Setting up Dynamics 365 connection");

                //Create the Dynamics 365 Connection:
                CrmServiceClient oMSCRMConn = new CrmServiceClient($"AuthType=Office365;Username={D365_USERNAME};Password={D365_PASSWORD};URL={D365_URL};");

                //Create the IOrganizationService:
                oServiceProxy = (IOrganizationService)oMSCRMConn.OrganizationWebProxyClient != null ?
                        (IOrganizationService)oMSCRMConn.OrganizationWebProxyClient :
                        (IOrganizationService)oMSCRMConn.OrganizationServiceProxy;

                Console.WriteLine("Validating Connection");

                if (oServiceProxy != null)
                {
                    //Get the current user ID:
                    Guid userid = ((WhoAmIResponse)oServiceProxy.Execute(new WhoAmIRequest())).UserId;

                    if (userid != Guid.Empty)
                    {
                        Console.WriteLine("Connection Successful!");
                    }
                }
                else
                {
                    Console.WriteLine("Connection failed...");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - " + ex.ToString());
            }

            Console.ReadKey();
        }*/
    }
}