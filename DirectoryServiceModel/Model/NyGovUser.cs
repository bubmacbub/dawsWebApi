using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryServiceModel.Model
{
    public class NyGovUser
    {
        public String NysSogUid { get; set; }
        public String Uid { get; set; }
        public String CommonName { get; set; }
        public String DistinguishedName { get; set; }
        public String Surname { get; set; }
        public String Firstname { get; set; }
        public String MiddleInitial { get; set; }
        public Boolean IsGovernmentAccount { get; set; }
        public Boolean IsCitizenAccount { get; set; }
        public Boolean IsBusinessPartnerAccount { get; set; }
        public String EmailAddress { get; set; }
        public String StreetAddress { get; set; }
        public String City { get; set; }

    }
}
