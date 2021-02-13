using ELMS.WEB.Adapters.Email;
using ELMS.WEB.Areas.Email.Data;
using ELMS.WEB.Entities.Email;
using ELMS.WEB.Enums.Email;
using ELMS.WEB.Helpers;
using ELMS.WEB.Managers.Email.Interface;
using ELMS.WEB.Models.Base.Response;
using ELMS.WEB.Models.Email.Request;
using ELMS.WEB.Models.Email.Response;
using ELMS.WEB.Models.Equipment.Response;
using ELMS.WEB.Models.Loan.Response;
using ELMS.WEB.Repositories.Email.Interface;
using ELMS.WEB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELMS.WEB.Managers.Email.Concrete
{
    public class EmailScheduleManager : IEmailScheduleManager
    {
        private readonly IEmailScheduleRepository __EmailScheduleRepository;
        private readonly IEmailScheduleParameterRepository __EmailScheduleParameterRepository;
        private readonly IApplicationEmailSender __EmailSender;
        private readonly IEmailTemplateRepository __EmailTemplateRepository;
        private const string ENTITY_ANEM = "Email Schedule";

        public EmailScheduleManager(IEmailScheduleRepository emailScheduleRepository, IEmailScheduleParameterRepository emailScheduleParameterRepository, IApplicationEmailSender emailSender, IEmailTemplateRepository emailTemplateRepository)
        {
            __EmailScheduleRepository = emailScheduleRepository ?? throw new ArgumentNullException(nameof(emailScheduleRepository));
            __EmailScheduleParameterRepository = emailScheduleParameterRepository ?? throw new ArgumentNullException(nameof(emailScheduleParameterRepository));
            __EmailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            this.__EmailTemplateRepository = emailTemplateRepository;
        }

        public async Task<EmailScheduleResponse> CreateAsync(CreateEmailScheduleRequest request)
        {
            EmailScheduleResponse _Response = (await __EmailScheduleRepository.CreateAsync(request?.ToEntity()))?.ToResponse();

            if (_Response == null)
            {
                _Response.Success = false;
                _Response.Message = $"{GlobalConstants.ERROR_ACTION_PREFIX} create {ENTITY_ANEM}.";
            }

            return _Response;
        }

        public async Task<IList<EmailScheduleResponse>> GetAsync()
        {
            return (await __EmailScheduleRepository.GetAsync()).ToResponse();
        }

        public async Task<EmailScheduleResponse> GetByUIDAsync(Guid uid)
        {
            return (await __EmailScheduleRepository.GetByUIDAsync(uid)).ToResponse();
        }

        public async Task<BaseResponse> DeleteAsync(Guid uid)
        {
            BaseResponse _Response = new BaseResponse();

            if (!await __EmailScheduleRepository.DeleteAsync(uid))
            {
                _Response.Success = false;
                _Response.Message = $"{GlobalConstants.ERROR_ACTION_PREFIX} delete {ENTITY_ANEM}.";
            }

            return _Response;
        }

        public async Task<BaseResponse> CreateEquipmentWarrantyScheduleAsync(EquipmentResponse equipment, string baseURL)
        {
            // Nearly Expired Schedule
            CreateEmailScheduleRequest _NearlyExpiredScheduleRequest = new CreateEmailScheduleRequest
            {
                EmailType = EmailType.Warranty_Nearly_Expired,
                RecipientEmailAddress = "lej1@aston.ac.uk",
                SendTimestamp = equipment.WarrantyExpirationDate.Date.AddDays(-3)
            };
            EmailScheduleEntity _NearlyExpiredScheduleEntity = await __EmailScheduleRepository.CreateAsync(_NearlyExpiredScheduleRequest.ToEntity());

            CreateEmailScheduleParameterRequest _ParameterNearlyExpiredRequest = new CreateEmailScheduleParameterRequest
            {
                EmailScheduleUID = _NearlyExpiredScheduleEntity.UID,
                Name = "Warranty_Expiry_URL",
                Value = $"{baseURL}/Equipment/Equipment/EquipmentDetails?uid={equipment.UID}"
            };
            await __EmailScheduleParameterRepository.CreateAsync(_ParameterNearlyExpiredRequest.ToEntity());

            // Expired Schedule
            CreateEmailScheduleRequest _ExpiredScheduleRequest = new CreateEmailScheduleRequest
            {
                EmailType = EmailType.Warranty_Expired,
                RecipientEmailAddress = "lej1@aston.ac.uk",
                SendTimestamp = equipment.WarrantyExpirationDate.Date
            };
            EmailScheduleEntity _ExpiredScheduleEntity = await __EmailScheduleRepository.CreateAsync(_ExpiredScheduleRequest.ToEntity());

            CreateEmailScheduleParameterRequest _ParameterExpiredRequest = new CreateEmailScheduleParameterRequest
            {
                EmailScheduleUID = _ExpiredScheduleEntity.UID,
                Name = "Warranty_Expiry_URL",
                Value = $"{baseURL}/Equipment/Equipment/EquipmentDetails?uid={equipment.UID}"
            };
            await __EmailScheduleParameterRepository.CreateAsync(_ParameterExpiredRequest.ToEntity());

            return new BaseResponse();
        }

        public async Task<BaseResponse> BulkCreateEquipmentWarrantyScheduleAsync(IList<EquipmentResponse> equipmentList, string baseURL)
        {
            foreach (EquipmentResponse equipment in equipmentList)
            {
                // Nearly Expired Schedule
                CreateEmailScheduleRequest _NearlyExpiredScheduleRequest = new CreateEmailScheduleRequest
                {
                    EmailType = EmailType.Warranty_Nearly_Expired,
                    RecipientEmailAddress = "lej1@aston.ac.uk",
                    SendTimestamp = equipment.WarrantyExpirationDate.Date.AddDays(-3)
                };
                EmailScheduleEntity _NearlyExpiredScheduleEntity = await __EmailScheduleRepository.CreateAsync(_NearlyExpiredScheduleRequest.ToEntity());

                CreateEmailScheduleParameterRequest _ParameterNearlyExpiredRequest = new CreateEmailScheduleParameterRequest
                {
                    EmailScheduleUID = _NearlyExpiredScheduleEntity.UID,
                    Name = "Warranty_Expiry_URL",
                    Value = $"{baseURL}/Equipment/Equipment/EquipmentDetails?uid={equipment.UID}"
                };
                await __EmailScheduleParameterRepository.CreateAsync(_ParameterNearlyExpiredRequest.ToEntity());

                // Expired Schedule
                CreateEmailScheduleRequest _ExpiredScheduleRequest = new CreateEmailScheduleRequest
                {
                    EmailType = EmailType.Warranty_Expired,
                    RecipientEmailAddress = "lej1@aston.ac.uk",
                    SendTimestamp = equipment.WarrantyExpirationDate.Date
                };
                EmailScheduleEntity _ExpiredScheduleEntity = await __EmailScheduleRepository.CreateAsync(_ExpiredScheduleRequest.ToEntity());

                CreateEmailScheduleParameterRequest _ParameterExpiredRequest = new CreateEmailScheduleParameterRequest
                {
                    EmailScheduleUID = _ExpiredScheduleEntity.UID,
                    Name = "Warranty_Expiry_URL",
                    Value = $"{baseURL}/Equipment/Equipment/EquipmentDetails?uid={equipment.UID}"
                };
                await __EmailScheduleParameterRepository.CreateAsync(_ParameterExpiredRequest.ToEntity());
            }

            return new BaseResponse();
        }

        public async Task<BaseResponse> CreateLoanConfirmScheduleAsync(LoanResponse loan, string baseURL, bool forceSend = false)
        {
            CreateEmailScheduleRequest _LoanConfirmScheduleRequest = new CreateEmailScheduleRequest
            {
                EmailType = EmailType.Loan_Confirm,
                RecipientEmailAddress = loan.LoaneeEmail,
                SendTimestamp = loan.CreatedTimestamp,
                Sent = forceSend
            };
            EmailScheduleEntity _LoanConfirmScheduleEntity = await __EmailScheduleRepository.CreateAsync(_LoanConfirmScheduleRequest.ToEntity());

            CreateEmailScheduleParameterRequest _ParameterLoanConfirmRequest = new CreateEmailScheduleParameterRequest
            {
                EmailScheduleUID = _LoanConfirmScheduleEntity.UID,
                Name = "Confirm_Loan_URL",
                Value = $"<a href='{baseURL}/Loan/Loan/AcceptTermsAndConditionsView?loanUID={loan.UID}'>Accept Terms and Conditions</a> "
            };
            await __EmailScheduleParameterRepository.CreateAsync(_ParameterLoanConfirmRequest.ToEntity());

            if (forceSend)
            {
                await __EmailSender.SendLoanConfirmEmail(loan.LoaneeEmail, "AIM LAB - Activate Loan", new ConfirmEmailTemplate
                {
                    Confirm_Loan_URL = $"<a href='{baseURL}/Loan/Loan/AcceptTermsAndConditionsView?loanUID={loan.UID}'>Accept Terms and Conditions</a> "
                });
            }

            return new BaseResponse();
        }

        public async Task<BaseResponse> CreateLoanConfirmedScheduleAsync(LoanResponse loan, string baseURL, bool forceSend = false)
        {
            CreateEmailScheduleRequest _LoanConfirmedScheduleRequest = new CreateEmailScheduleRequest
            {
                EmailType = EmailType.Loan_Confirmed,
                RecipientEmailAddress = loan.LoaneeEmail,
                SendTimestamp = loan.CreatedTimestamp,
                Sent = forceSend
            };
            EmailScheduleEntity _LoanConfirmScheduleEntity = await __EmailScheduleRepository.CreateAsync(_LoanConfirmedScheduleRequest.ToEntity());

            CreateEmailScheduleParameterRequest _ParamStartTimestampRequest = new CreateEmailScheduleParameterRequest
            {
                EmailScheduleUID = _LoanConfirmScheduleEntity.UID,
                Name = "Start_Timestamp",
                Value = loan.FromTimestamp.ToString()
            };
            await __EmailScheduleParameterRepository.CreateAsync(_ParamStartTimestampRequest.ToEntity());

            CreateEmailScheduleParameterRequest _ParamLoanDetailsURLRequest = new CreateEmailScheduleParameterRequest
            {
                EmailScheduleUID = _LoanConfirmScheduleEntity.UID,
                Name = "Loan_Details_URL",
                Value = $"{baseURL}/Loan/Loan/DetailsView?loanUID={loan.UID}"
            };
            await __EmailScheduleParameterRepository.CreateAsync(_ParamLoanDetailsURLRequest.ToEntity());

            if (forceSend)
            {
                await __EmailSender.SendLoanConfirmEmail(loan.LoaneeEmail, "AIM LAB - Confirmed Loan", new ConfirmEmailTemplate
                {
                    Confirm_Loan_URL = $"{baseURL}/Loan/Loan/DetailsView?loanUID={loan.UID}"
                });
            }

            return new BaseResponse();
        }

        public async Task<BaseResponse> CreateLoanExpiryScheduleAsync(LoanResponse loan, string baseURL, bool forceSend = false)
        {
            // Nearly Expired Loan
            CreateEmailScheduleRequest _NearlyExpiredScheduleRequest = new CreateEmailScheduleRequest
            {
                EmailType = EmailType.Loan_Nearly_Due,
                RecipientEmailAddress = loan.LoaneeEmail,
                SendTimestamp = loan.ExpiryTimestamp.Date.AddDays(-3),
                Sent = forceSend
            };
            EmailScheduleEntity _NearlyExpiredScheduleEntity = await __EmailScheduleRepository.CreateAsync(_NearlyExpiredScheduleRequest.ToEntity());

            CreateEmailScheduleParameterRequest _ParameterLoanDetailsURLRequest = new CreateEmailScheduleParameterRequest
            {
                EmailScheduleUID = _NearlyExpiredScheduleEntity.UID,
                Name = "Loan_Details_URL",
                Value = $"{baseURL}/Loan/Loan/DetailsView?loanUID={loan.UID}"
            };
            await __EmailScheduleParameterRepository.CreateAsync(_ParameterLoanDetailsURLRequest.ToEntity());

            CreateEmailScheduleParameterRequest _ParameterDueDaysRequest = new CreateEmailScheduleParameterRequest
            {
                EmailScheduleUID = _NearlyExpiredScheduleEntity.UID,
                Name = "Due_Days",
                Value = 3.ToString()
            };
            await __EmailScheduleParameterRepository.CreateAsync(_ParameterDueDaysRequest.ToEntity());

            // Expired Schedule
            CreateEmailScheduleRequest _ExpiredScheduleRequest = new CreateEmailScheduleRequest
            {
                EmailType = EmailType.Loan_Overdue,
                RecipientEmailAddress = loan.LoaneeEmail,
                SendTimestamp = loan.ExpiryTimestamp
            };
            EmailScheduleEntity _ExpiredScheduleEntity = await __EmailScheduleRepository.CreateAsync(_ExpiredScheduleRequest.ToEntity());

            CreateEmailScheduleParameterRequest _ParameterExpiredRequest = new CreateEmailScheduleParameterRequest
            {
                EmailScheduleUID = _ExpiredScheduleEntity.UID,
                Name = "Overdue_Loan_URL",
                Value = $"{baseURL}/Loan/Loan/DetailsView?uid={loan.UID}"
            };
            await __EmailScheduleParameterRepository.CreateAsync(_ParameterExpiredRequest.ToEntity());

            CreateEmailScheduleParameterRequest _ParameterExpiryDateRequest = new CreateEmailScheduleParameterRequest
            {
                EmailScheduleUID = _ExpiredScheduleEntity.UID,
                Name = "Loan_Expiry_Date",
                Value = loan.ExpiryTimestamp.ToString()
            };
            await __EmailScheduleParameterRepository.CreateAsync(_ParameterExpiryDateRequest.ToEntity());

            return new BaseResponse();
        }

        public async Task SendScheduledEmails()
        {
            IList<EmailScheduleResponse> _ScheduledEmails = (await __EmailScheduleRepository.GetEmailsToSendAsync()).ToResponse();

            if (_ScheduledEmails != null)
            {
                foreach (EmailScheduleResponse schedule in _ScheduledEmails)
                {
                    await SendScheduledEmail(schedule);
                }
            }
        }

        private async Task SendScheduledEmail(EmailScheduleResponse schedule)
        {
            IList<EmailScheduleParameterResponse> _Parameters = (await __EmailScheduleParameterRepository.GetAsync(schedule.UID))?.ToResponse();

            switch (schedule.EmailType)
            {
                case EmailType.Loan_Confirm:
                    await __EmailSender.SendLoanConfirmEmail(schedule.RecipientEmailAddress, "AIM - Loan Confirm", new ConfirmEmailTemplate
                    {
                        Confirm_Loan_URL = _Parameters.FirstOrDefault(p => p.Name.ToUpper() == "Confirm_Loan_URL")?.Value ?? GlobalConstants.ASTON_URL
                    });
                    break;

                case EmailType.Loan_Confirmed:
                    await __EmailSender.SendLoanConfirmedEmail(schedule.RecipientEmailAddress, "AIM - Loan Confirmed", new ConfirmedEmailTemplate
                    {
                        Loan_Details_URL = _Parameters.FirstOrDefault(p => p.Name.ToUpper() == "Loan_Details_URL")?.Value ?? GlobalConstants.ASTON_URL,
                        Start_Timestamp = _Parameters.FirstOrDefault(p => p.Name.ToUpper() == "Start_Timestamp")?.Value ?? "[Value not found]"
                    });
                    break;

                case EmailType.Loan_Nearly_Due:
                    await __EmailSender.SendLoanNearlyDueEmail(schedule.RecipientEmailAddress, "AIM - Loan Nearly Due", new LoanNearlyDueTemplate
                    {
                        Due_days = int.Parse(_Parameters.FirstOrDefault(p => p.Name.ToUpper() == "Due_days").Value),
                        Loan_Details_URL = _Parameters.FirstOrDefault(p => p.Name.ToUpper() == "Loan_Details_URL")?.Value ?? "[Value not found]"
                    });
                    break;

                case EmailType.Loan_Overdue:
                    await __EmailSender.SendLoanOverdueEmail(schedule.RecipientEmailAddress, "AIM - Loan Overdue", new LoanOverdueTemplate
                    {
                        Loan_Expiry_Date = _Parameters.FirstOrDefault(p => p.Name.ToUpper() == "Loan_Expiry_Date")?.Value ?? "[Value not found]",
                        Overdue_Loan_URL = _Parameters.FirstOrDefault(p => p.Name.ToUpper() == "Overdue_Loan_URL")?.Value ?? "[Value not found]",
                    });
                    break;

                case EmailType.Warranty_Nearly_Expired:
                    await __EmailSender.SendWarrantyNearlyExpiredEmail(schedule.RecipientEmailAddress, "AIM - Warranty Nearly Expired", new WarrantyNearlyExpiredTemplate
                    {
                        Warranty_Expiry_URL = _Parameters.FirstOrDefault(p => p.Name.ToUpper() == "Warranty_Expiry_URL")?.Value ?? "[Value not found]"
                    });
                    break;

                case EmailType.Warranty_Expired:
                    await __EmailSender.SendWarrantyExpiredEmail(schedule.RecipientEmailAddress, "AIM - Warranty Expired", new WarrantyExpiredTemplate
                    {
                        Warranty_Expiry_URL = _Parameters.FirstOrDefault(p => p.Name.ToUpper() == "Warranty_Expiry_URL")?.Value ?? "[Value not found]",
                    });
                    break;

                case EmailType.Custom:
                default:
                    EmailTemplateResponse _Template = (await __EmailTemplateRepository.GetByUIDAsync(schedule.EmailTemplateUID))?.ToResponse();
                    if (_Template != null)
                    {
                        await __EmailSender.SendEmailAsync(schedule.RecipientEmailAddress, _Template.Subject, _Template.Body);
                    }
                    break;
            }

            await __EmailScheduleRepository.UpdateSentAsync(schedule.UID, true);
        }

        public async Task<BaseResponse> UpdateSentAsync(Guid uid, bool sent)
        {
            BaseResponse _Response = new BaseResponse();

            if (!await __EmailScheduleRepository.UpdateSentAsync(uid, sent))
            {
                _Response.Success = false;
                _Response.Message = $"{GlobalConstants.ERROR_ACTION_PREFIX} update Sent field of {ENTITY_ANEM}";
            }

            return _Response;
        }
    }
}