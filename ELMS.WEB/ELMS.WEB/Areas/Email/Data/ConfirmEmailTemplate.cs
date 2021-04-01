using Newtonsoft.Json;

namespace ELMS.WEB.Areas.Email.Data
{
    public class ConfirmEmailTemplate
    {
        [JsonProperty]
        public string Confirm_Loan_URL { get; set; }
        
        [JsonProperty]
        public string Equipment_String { get; set; }

        [JsonProperty]
        public string Loan_Period_String { get; set; }
    }
}