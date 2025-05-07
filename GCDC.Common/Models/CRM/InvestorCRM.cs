using GCDC.Common.Models.Api.Request.CRM_Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDC.Common.Models.CRM
{
    public class InvestorCRM
    {
        public FieldData FirstName { get; set; }
        public FieldData LastName { get; set; }
        public FieldData EmailAddress { get; set; }
        public FieldData PhoneNumber { get; set; }
        public FieldData DeviceType { get; set; }
        public FieldData Language { get; set; }
        public FieldData Message { get; set; }
        public FieldData ReferralId { get; set; }
        public FieldData UTMCampaign { get; set; }
        public FieldData UTMContent { get; set; }
        public FieldData UTMMedium { get; set; }
        public FieldData UTMSource { get; set; }
        public FieldData UTMTerm { get; set; }
        public FieldData Company { get; set; }
        public FieldData JobTitle { get; set; }

        public InvestorCRM(string firstName, string lastName, string emailAddress, string phoneNumber, string company, string message,string jobTitle, string referralId,
            string utmCampaign, string utmContent, string utmMedium, string utmSource, string utmTerm
        )
        {
            FirstName = new FieldData("455", "First Name", firstName);
            LastName = new FieldData("456", "Last Name", lastName);
            EmailAddress = new FieldData("457", "Email Address", emailAddress);
            PhoneNumber = new FieldData("462", "Phone Number", phoneNumber);
            Company = new FieldData("469", "Company", company);
            Message = new FieldData("489", "Message", message);
            JobTitle = new FieldData("470", "Job Title", jobTitle);
            DeviceType = new FieldData("463", "Device Type", "");
            Language = new FieldData("459", "Language", "");
            ReferralId = new FieldData("490", "Referral Id", referralId);
            UTMCampaign = new FieldData("464", "UTM Campaign", utmCampaign);
            UTMContent = new FieldData("465", "UTM Content", utmContent);
            UTMMedium = new FieldData("466", "UTM Medium", utmMedium);
            UTMSource = new FieldData("467", "UTM Source", utmSource);
            UTMTerm = new FieldData("468", "UTM Term", utmTerm);
        }
    }
}
