using Microsoft.Rest;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;

namespace TeBS_CC_WebAPIDev.Services
{
    public class CrmContext
    {
        private static readonly string CRM_AUTH_TYPE = "Office365";
        private static readonly string CRM_USERNAME = "titus.lim@totalebizsolutions.com";
        private static readonly string CRM_PASSWORD = "";
        private static readonly string CRM_APPID = "0b0c1470-f9ad-e165-9e84-98360288e787";
        private static readonly string CONNECTION_STRING = string.Format(
            "AuthType={0};Url=https://org7f3624ff.api.crm5.dynamics.com/;Username={1};Password={2}",
            CRM_AUTH_TYPE,
            CRM_USERNAME,
            CRM_PASSWORD);

        public IOrganizationService GetCrmContext()
        {
            CrmServiceClient conn = new CrmServiceClient(CONNECTION_STRING);

            IOrganizationService _orgSrv = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

            return _orgSrv;
        }
    }
}
