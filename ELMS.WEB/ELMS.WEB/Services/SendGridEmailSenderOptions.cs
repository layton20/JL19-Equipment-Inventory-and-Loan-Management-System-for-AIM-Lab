﻿namespace ELMS.WEB.Services
{
    public class SendGridEmailSenderOptions
    {
        public string SendGridKey { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
    }
}