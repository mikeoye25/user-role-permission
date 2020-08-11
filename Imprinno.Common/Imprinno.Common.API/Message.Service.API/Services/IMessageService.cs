using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Messager = Imprinno.Models.Entities.Message;

namespace Message.Service.API.Services
{
    public interface IMessageService
    {
        Task<bool> SendEmailAsync(Messager message);
        bool SendSms(Messager message);
        List<Messager> GetAllMessages();
        void AddMessage(Messager message);
    }
}
