using AdventureWorksClassLib.Services;
using AdventureWorksClassLib.ViewModels;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AdventureWorksClassLib
{
    public class AuthClass
    {
        #region DB Context
        private readonly AppDbContext _dbCtx;

        public AuthClass(AppDbContext dbCtx)
        {
            _dbCtx = dbCtx;
        }
        #endregion

        #region main
        public UserDTO verifyAuth(StringValues reqHeadAuth)
        {
            //TODO: Temp measures since db contains no emails and passwords

            UserDTO? user = null;

            //Assign auth header to var
            var authHeaderVal = AuthenticationHeaderValue.Parse(reqHeadAuth);

            //Convert auth header from base64 to string
            var bytes = Convert.FromBase64String(authHeaderVal.Parameter!);

            //Seperate email and password
            string[] credentials = Encoding.UTF8.GetString(bytes).Split(":");

            //Assign email and password to vars
            string email = credentials[0];
            string password = credentials[1];

            if (email.ToLower().Equals("titus.lim@totalebizsolutions.com") && password.Equals("password123"))
            {
                user = new UserDTO(email, password);
            }

            return user!;
        }
        #endregion
    }
}
