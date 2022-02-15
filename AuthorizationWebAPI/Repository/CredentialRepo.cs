using AuthorizationWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationWebAPI.Repository
{
    public class CredentialRepo:ICredentialRepo
    {
      /* private Dictionary<string, string> ValidUsersDictionary = new Dictionary<string, string>()
        {
             {"Shaqiq","Raihan"},
             {"Tharun","Thota"},
             {"Aarnisha","Deivandran"},
             {"Debajyoti","Majhi"},
             {"Pawan","Pratap Singh" }
        };*/
        private AuthorizeDBContext _context;

        public CredentialRepo(AuthorizeDBContext context)
        {
            _context = context;
        }
        public Dictionary<string, string> GetCredentials()
        {

            List<Authenticate> list = _context.Credentials.ToList();

            Dictionary<string, string> dict = list.ToDictionary(x=>x.Name,x=>x.Password);

            return dict;


        }
    }
}
