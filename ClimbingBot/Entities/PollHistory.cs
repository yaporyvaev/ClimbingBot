using System;
using ClimbingBot.Abstractions;

namespace ClimbingBot.Entities
{
    public class PollHistory : BaseEntity
    {
        public long GroupId { get; set; }
        public int MessageId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}