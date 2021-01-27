using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmailService
{
    public class Email
    {
        protected List<string> _receiverAddress = new List<string>();
        
        protected string _senderAddress = "";
        protected string _subject = "";
        protected string _body = "";
        protected string _htmlBody = "";
        protected bool _isHtml = false;

        public Email(string senderAddress, List<string> receiverAddresses, string subject, string body, bool isHtml=false)
        {
            _senderAddress = senderAddress;
            _receiverAddress = receiverAddresses;
            _subject = subject;
            _isHtml = isHtml;

            if (isHtml == true)
            {
                _htmlBody = body;
                _body = body;
            }
        }

        public async Task SendAsync()
        {
            using (var client = new AmazonSimpleEmailServiceClient(RegionEndpoint.USWest2))
            {
                var sendRequest = new SendEmailRequest
                {
                    Source = _senderAddress,
                    Destination = new Destination
                    {
                        ToAddresses = _receiverAddress
                    },

                    Message = new Message
                    {
                        Subject = new Content(_subject),
                        Body = new Body
                        {
                            Html = new Content
                            {
                                Charset = "UTF-8",
                                Data = _body
                            },
                            Text = new Content
                            {
                                Charset = "UTF-8",
                                Data = _body
                            }
                        }
                    },
                };

                var response = await client.SendEmailAsync(sendRequest);
            }
        }
    }
}
