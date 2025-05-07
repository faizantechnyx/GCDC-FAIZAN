using GCDC.Common.Models.Api.Request.CRM_Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDC.Common.Models.CRM
{
    public class MediaAndPress
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

        public MediaAndPress(string firstName, string lastName, string emailAddress, string phoneNumber, string message, string company, string jobTitle, string referralId,
            string utmCampaign, string utmContent, string utmMedium, string utmSource, string utmTerm
        )
        {
            FirstName = new FieldData("439", "First Name", firstName);
            LastName = new FieldData("440", "Last Name", lastName);
            EmailAddress = new FieldData("441", "Email Address", emailAddress);
            PhoneNumber = new FieldData("446", "Phone Number", phoneNumber);
            Message = new FieldData("442", "Message", message);
            DeviceType = new FieldData("447", "Device Type", "");
            Language = new FieldData("443", "Language", "");
            ReferralId = new FieldData("493", "Referral Id", referralId);
            UTMCampaign = new FieldData("448", "UTM Campaign", utmCampaign);
            UTMContent = new FieldData("449", "UTM Content", utmContent);
            UTMMedium = new FieldData("450", "UTM Medium", utmMedium);
            UTMSource = new FieldData("451", "UTM Source", utmSource);
            UTMTerm = new FieldData("452", "UTM Term", utmTerm);
            Company = new FieldData("453", "Company", company);
            JobTitle = new FieldData("454", "Job Title", jobTitle);
        }
    }

}
