using GCDC.Common.Models.Api.Request.CRM_Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDC.Common.Models.CRM
{
    public class CareerCRM
    {
        public FieldData FirstName { get; set; }
        public FieldData LastName { get; set; }
        public FieldData EmailAddress { get; set; }
        public FieldData PhoneNumber { get; set; }
        public FieldData DeviceType { get; set; }
        public FieldData Language { get; set; }
        public FieldData ReferralId { get; set; }
        public FieldData UTMCampaign { get; set; }
        public FieldData UTMContent { get; set; }
        public FieldData UTMMedium { get; set; }
        public FieldData UTMSource { get; set; }
        public FieldData UTMTerm { get; set; }
        public FieldData LinkedinUrl { get; set; }
        public FieldData CoverLetter { get; set; }

        public CareerCRM(string firstName, string lastName, string emailAddress, string phoneNumber, string linkedinUrl, string coverletter, string referralId,
            string utmCampaign, string utmContent, string utmMedium, string utmSource, string utmTerm
        )
        {
            FirstName = new FieldData("424", "First Name", firstName);
            LastName = new FieldData("425", "Last Name", lastName);
            EmailAddress = new FieldData("426", "Email Address", emailAddress);
            PhoneNumber = new FieldData("427", "Phone Number", phoneNumber);
            LinkedinUrl = new FieldData("487", "Linkedin URL", linkedinUrl);
            CoverLetter = new FieldData("488", "Cover Letter", coverletter);
            DeviceType = new FieldData("433", "Device Type", "");
            Language = new FieldData("430", "Language", "");
            ReferralId = new FieldData("494", "Referral Id", referralId);
            UTMCampaign = new FieldData("434", "UTM Campaign", utmCampaign);
            UTMContent = new FieldData("435", "UTM Content", utmContent);
            UTMMedium = new FieldData("436", "UTM Medium", utmMedium);
            UTMSource = new FieldData("437", "UTM Source", utmSource);
            UTMTerm = new FieldData("438", "UTM Term", utmTerm);
        }
    }
}
