using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.Enums.Email
{
    public enum EmailTemplateType : int
    {
        [Display(Name = "Plain Text")]
        PlainText = 0,

        [Display(Name = "HTML Formatted")]
        Formatted = 1
    }
}