using Imprinno.DataAccess.Repositories.Interface;
using Imprinno.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imprinno.DataAccess.Repositories.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly EntitiesDbContext _ctx;

        public MessageRepository(EntitiesDbContext ctx)
        {
            _ctx = ctx;
        }

        public void AddEntity(object model)
        {
            _ctx.Add(model);
        }

        public IEnumerable<Message> GetAllMessages()
        {
            try
            {
                return _ctx.Messages
                           .OrderByDescending(p => p.CreatedOn)
                           .ToList();
            }
            catch
            {
                return null;
            }
        }

        public bool SaveAll()
        {
            return _ctx.SaveChanges() > 0;
        }
    }
}
