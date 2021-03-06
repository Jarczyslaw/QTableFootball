using System;
using System.Collections.Generic;
using System.Linq;

namespace JToolbox.Core.Results
{
    public class Result
    {
        public Result()
        {
        }

        public Result(Result other)
        {
            Messages = other.Messages;
        }

        public Message Error => Errors.FirstOrDefault();

        public List<Message> Errors => GetMessagesOfType(MessageType.Error);

        public Message Information => Informations.FirstOrDefault();

        public List<Message> Informations => GetMessagesOfType(MessageType.Information);

        public bool IsError => !IsSuccess;

        public bool IsSuccess => !Messages.Any(s => s.Type == MessageType.Error);

        public Messages Messages { get; } = new Messages();

        public Message Warning => Warnings.FirstOrDefault();

        public List<Message> Warnings => GetMessagesOfType(MessageType.Warning);

        public static Result AsError(string error)
        {
            var result = new Result();
            result.Messages.AddError(error);
            return result;
        }

        public static Result AsError(Exception exc)
        {
            var result = new Result();
            result.Messages.AddError(exc);
            return result;
        }

        public virtual void Clear()
        {
            Messages.Clear();
        }

        private List<Message> GetMessagesOfType(MessageType messageType)
        {
            return Messages.Where(s => s.Type == messageType)
                .ToList();
        }
    }
}