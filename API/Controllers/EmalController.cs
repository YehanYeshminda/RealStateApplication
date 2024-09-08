using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Pop3;
using Microsoft.Data.SqlClient;
using API.Repos.Dtos;
using API.Repos.Interfaces;
using System.Net;
using System.Net.Mail;
using MailKit;
using MailKit.Net.Imap;
using API.Repos.Helpers;
using MimeKit;

namespace API.Controllers
{
    [Route("api/email")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        private readonly ResponseDto _response;

        public EmailController(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _response = new ResponseDto();
        }

        [HttpPost]
        [Route("getemails")]
        public async Task<ResponseDto> GetEmails([FromBody] AuthDto authDto, [FromQuery]int page = 1, [FromQuery]int pageSize = 10)
        {
            //var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(authDto);

            //if (!authResponse.IsSuccess)
            //{
            //    _response.Message = authResponse.Message;
            //    _response.IsSuccess = authResponse.IsSuccess;
            //    _response.Result = authResponse.Result;
            //    return _response;
            //}

            try
            {

                using (var client = new ImapClient())
                {
                    client.Connect("imap.hostinger.com", 993, true);
                    client.Authenticate("crmtest@thinkview.click", "Binance987@#");

                    var inbox = client.Inbox;
                    inbox.Open(FolderAccess.ReadOnly);

                    var messageCount = inbox.Count;
                    var emails = new List<EmailMessage>();

                    int startIndex = (page - 1) * pageSize;
                    int endIndex = Math.Min(startIndex + pageSize, messageCount);

                    for (int i = startIndex; i < endIndex; i++)
                    {
                        var message = inbox.GetMessage(i);

                        // Extract email address using regular expression
                        string from = message.From.ToString();
                        string emailAddress = RegexHelpers.ExtractEmailAddress(from);

                        string to = ExtractRecipients(message.To);

                        emails.Add(new EmailMessage
                        {
                            Subject = message.Subject,
                            From = emailAddress,
                            Date = message.Date.DateTime,
                            Body = message.TextBody,
                            To = to
                        });
                    }

                    foreach (var summary in inbox.Fetch(0, -1, MessageSummaryItems.Envelope))
                    {
                        Console.WriteLine("[summary] {0:D2}: {1}", summary.Folder, summary.Envelope.Subject);

                        client.Disconnect(true);
                    }


                    _response.Result = emails;
                    _response.Message = "Success in fetching emails";
                    _response.IsSuccess = true;
                    return _response;
                }

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return _response;
            }
        }

        [HttpPost]
        [Route("getdraftemails")]
        public async Task<ResponseDto> GetDraftEmails([FromBody] AuthDto authDto, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                using (var client = new ImapClient())
                {
                    client.Connect("imap.hostinger.com", 993, true);
                    client.Authenticate("crmtest@thinkview.click", "Binance987@#"); // Replace with your password

                    var drafts = client.GetFolder(SpecialFolder.Drafts); // Access the Drafts folder
                    drafts.Open(FolderAccess.ReadOnly); // Open the Drafts folder

                    var draftMessageCount = drafts.Count;

                    var draftEmails = new List<EmailMessage>();

                    int startIndex = (page - 1) * pageSize;
                    int endIndex = Math.Min(startIndex + pageSize, draftMessageCount);

                    for (int i = startIndex; i < endIndex; i++)
                    {
                        var draftMessage = drafts.GetMessage(i);

                        draftEmails.Add(new EmailMessage
                        {
                            Subject = draftMessage.Subject,
                            From = draftMessage.From.ToString(),
                            Date = draftMessage.Date.DateTime,
                            Body = draftMessage.TextBody,
                        });
                    }

                    _response.Result = draftEmails;
                    _response.Message = "Success in fetching draft emails";
                    _response.IsSuccess = true;
                    return _response;
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return _response;
            }
        }


        [HttpPost]
        [Route("getsentemails")]
        public async Task<ResponseDto> GetSentEmails([FromBody] AuthDto authDto, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                using (var client = new ImapClient())
                {
                    client.Connect("imap.hostinger.com", 993, true);
                    client.Authenticate("crmtest@thinkview.click", "Binance987@#");

                    var sent = client.GetFolder(SpecialFolder.Sent);
                    sent.Open(FolderAccess.ReadOnly);

                    var sentMessageCount = sent.Count;

                    var sentEmails = new List<EmailMessage>();

                    int startIndex = (page - 1) * pageSize;
                    int endIndex = Math.Min(startIndex + pageSize, sentMessageCount);

                    for (int i = startIndex; i < endIndex; i++)
                    {
                        var sentMessage = sent.GetMessage(i);

                        sentEmails.Add(new EmailMessage
                        {
                            Subject = sentMessage.Subject,
                            From = sentMessage.From.ToString(),
                            Date = sentMessage.Date.DateTime,
                            Body = sentMessage.TextBody,
                        });
                    }

                    _response.Result = sentEmails;
                    _response.Message = "Success in fetching sent emails";
                    _response.IsSuccess = true;
                    return _response;
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return _response;
            }
        }


        [HttpPost]
        [Route("sendemail")]
        public async Task<ResponseDto> SendEmail([FromBody] EmailDto emailDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(emailDto.AuthDto);

            if (!authResponse.IsSuccess)
            {
                _response.Message = authResponse.Message;
                _response.IsSuccess = authResponse.IsSuccess;
                _response.Result = authResponse.Result;
                return _response;
            }

            try
            {
                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.Host = "smtp.hostinger.com";
                    smtpClient.Port = 587;

                    smtpClient.Credentials = new NetworkCredential(
                        "crmtest@thinkview.click",
                        "Binance987@#"
                    );

                    smtpClient.EnableSsl = true;

                    string htmlString = $@"
                    <!DOCTYPE html>
                    <html>
                    <body>
                        {emailDto.Body}
                    </body>
                    </html>
                    ";

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("crmtest@thinkview.click"),
                        Subject = emailDto.Subject,
                        Body = htmlString,
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(emailDto.ToEmail);

                    smtpClient.Send(mailMessage);
                }

                    _response.Message = "Email sent successfully";
                    _response.IsSuccess = true;
                    return _response;
                
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return _response;
            }
        }

        [NonAction]
        private string ExtractRecipients(InternetAddressList addressList)
        {
            var recipients = new List<string>();

            foreach (var address in addressList)
            {
                recipients.Add(address.ToString());
            }

            return string.Join(", ", recipients); // Join recipients as a comma-separated string
        }

        public class EmailMessage
        {
            public string Subject { get; set; }
            public string From { get; set; }
            public DateTime Date { get; set; }
            public string Body { get; set; }
            public string To { get; set; }
        }

        public class EmailDto
        {
            public AuthDto AuthDto { get; set; }
            public string ToEmail { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }
        }
    }
}
