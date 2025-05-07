namespace GCDC.Common.Models.Api.Request
{
    public class FormSubmissionRequest
    {
        public int FormId { get; set; }
        public string Payload { get; set; }
        public string Response { get; set; }
    }

}
