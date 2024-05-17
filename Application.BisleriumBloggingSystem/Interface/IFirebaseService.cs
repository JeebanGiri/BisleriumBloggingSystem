using Domain.BisleriumBloggingSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BisleriumBloggingSystem.Interface
{
    public interface IFirebaseService
    {
        public Task<FirebaseToken> CreateNewToken(FirebaseToken payload, string userId);

        public Task SendPushNotifications(string userIds, string title, string body);

    }
}
