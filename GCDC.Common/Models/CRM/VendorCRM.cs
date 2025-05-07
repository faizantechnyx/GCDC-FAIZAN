using GCDC.Common.Models.Api.Request.CRM_Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDC.Common.Models.CRM
{
    public class VendorCRM
    {
        public FieldData FirstName { get; set; }
        public FieldData LastName { get; set; }
        public FieldData EmailAddress { get; set; }
        public FieldData PhoneNumber { get; set; }
        public FieldData Message { get; set; }
        public FieldData DeviceType { get; set; }
        public FieldData Language { get; set; }
        public FieldData ReferralId { get; set; }
        public FieldData UTMCampaign { get; set; }
        public FieldData UTMContent { get; set; }
        public FieldData UTMMedium { get; set; }
        public FieldData UTMSource { get; set; }
        public FieldData UTMTerm { get; set; }
        public FieldData Company { get; set; }
        public FieldData JobTitle { get; set; }

        public VendorCRM(string firstName, string lastName, string emailAddress, string phoneNumber, string message, string company, string jobTitle, string referralId,
            string utmCampaign, string utmContent, string utmMedium, string utmSource, string utmTerm
        )
        {
            FirstName = new FieldData("471", "First Name", firstName);
            LastName = new FieldData("472", "Last Name", lastName);
            EmailAddress = new FieldData("473", "Email Address", emailAddress);
            PhoneNumber = new FieldData("478", "Phone Number", phoneNumber);
            Company = new FieldData("485", "Company", company);
            JobTitle = new FieldData("486", "Job Title", jobTitle);
            Message = new FieldData("474", "Message", message);
            DeviceType = new FieldData("479", "Device Type", "");
            Language = new FieldData("475", "Language", "");
            ReferralId = new FieldData("492", "Referral Id", referralId);
            UTMCampaign = new FieldData("480", "UTM Campaign", utmCampaign);
            UTMContent = new FieldData("481", "UTM Content", utmContent);
            UTMMedium = new FieldData("482", "UTM Medium", utmMedium);
            UTMSource = new FieldData("483", "UTM Source", utmSource);
            UTMTerm = new FieldData("484", "UTM Term", utmTerm);
        }
    }
}
