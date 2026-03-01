using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PushSubscription
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public bool Enabled { get; private set; } = false;
        public Platform Platform { get; private set; }
        public string Provider { get; private set; }
        public string DeviceId { get; private set; }
        public DateOnly LastSeenAt { get; private set; }
        public PushSubscription()
        {

        }
        public PushSubscription(Guid userId, Platform platform, string provider, string deviceId, DateOnly lastSeenAt)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Platform = platform;
            Provider = provider;
            DeviceId = deviceId;
            LastSeenAt = lastSeenAt;
        }
    }
}
