using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Imprinno.Models.Enums;
using Message.Service.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Messager = Imprinno.Models.Entities.Message;

namespace Message.Service.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Get()
        {
            try
            {
                var messages = _messageService.GetAllMessages();
                return StatusCode((int)HttpStatusCode.OK, messages);
            }
            catch (Exception ex) { return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message); }
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> PostAsync([FromBody] Messager message)
        {
            try
            {
                if (HasValidationError(message, out List<string> errors)) { return StatusCode((int)HttpStatusCode.PreconditionFailed, string.Join(" | ", errors)); }

                // Send message
                var messageType = message.MessageType;
                var status = false;
                switch (messageType)
                {
                    case MessageType.Email:
                        status = await _messageService.SendEmailAsync(message);
                        break;
                    case MessageType.Sms:
                        status = _messageService.SendSms(message);
                        break;
                    default:
                        return StatusCode((int)HttpStatusCode.BadRequest, string.Format("Please enter a valid message type"));
                }

                // Save Message
                message.Status = status;
                message.CreatedOn = DateTime.Now;
                _messageService.AddMessage(message);

                // Return delivery status
                if (status) return StatusCode((int)HttpStatusCode.OK, "Successful. Message sent !!!");
                return StatusCode((int)HttpStatusCode.OK, "Failed. Message not sent !!!");
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private bool HasValidationError(Messager message, out List<string> errors)
        {
            errors = new List<string>();
            var messageType = message.MessageType;

            switch (messageType)
            {
                case MessageType.Email:
                    if (!ValidEmail(message.To)) { errors.Add("Please enter a valid email"); }
                    if (string.IsNullOrWhiteSpace(message.Subject)) { errors.Add("Subject cannot be empty"); }
                    break;
                case MessageType.Sms:
                    if (!ValidPhone(message.To)) { errors.Add("Please enter a valid phone"); }
                    break;
                default:
                    errors.Add("Invalid message type");
                    break;
            }

            return errors.Count > 0 ? true : false;
        }

        private bool ValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email && new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(email);
            }

            catch { return false; }
        }

        private bool ValidPhone(string phone)
        {
            try
            {
                return new System.ComponentModel.DataAnnotations.PhoneAttribute().IsValid(phone);
            }

            catch { return false; }
        }
    }
}
