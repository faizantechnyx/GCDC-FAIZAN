using GCDC.Common.Models.Api.Request.CRM_Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDC.Common.Models.CRM
{
    public class GeneralInquiry
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

        public GeneralInquiry(
            string firstName, string lastName, string emailAddress, string phoneNumber, string message, string referralId,
            string utmCampaign, string utmContent, string utmMedium, string utmSource, string utmTerm
        )
        {
            FirstName = new FieldData("409", "First Name",firstName);
            LastName = new FieldData("410", "Last Name", lastName);
            EmailAddress = new FieldData("411", "Email Address", emailAddress);
            PhoneNumber = new FieldData("417", "Phone Number", phoneNumber);
            Message = new FieldData("412", "Message", message);
            DeviceType = new FieldData("418", "Device Type", "");
            Language = new FieldData("414", "Language", "");
            ReferralId = new FieldData("491", "Referral Id", referralId);
            UTMCampaign = new FieldData("419", "UTM Campaign", utmCampaign);
            UTMContent = new FieldData("420", "UTM Content", utmContent);
            UTMMedium = new FieldData("421", "UTM Medium", utmMedium);
            UTMSource = new FieldData("422", "UTM Source", utmSource);
            UTMTerm = new FieldData("423", "UTM Term", utmTerm);

        }
    }
}
