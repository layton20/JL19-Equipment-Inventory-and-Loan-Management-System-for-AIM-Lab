using Newtonsoft.Json;

namespace ELMS.WEB.Areas.Email.Data
{
    public class ConfirmEmailTemplate
    {
        [JsonProperty]
        public string Confirm_Loan_URL { get; set; }
    }
}