using Imprinno.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Imprinno.DataAccess.Repositories.Interface
{
    public interface IMessageRepository
    {
        IEnumerable<Message> GetAllMessages();
        bool SaveAll();
        void AddEntity(object model);
    }
}
