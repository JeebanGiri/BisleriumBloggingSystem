using Application.BisleriumBloggingSystem.Interface;
using Domain.BisleriumBloggingSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Insfrastructure.BisleriumBloggingSystem.Config;
using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Identity;
using Google;

namespace Infrastructure.BisleriumBloggingSystem.Service // Corrected namespace
{
    public class FirebaseService : IFirebaseService
    {

        private readonly ApplicationDBContext _dbContext;
        private readonly UserManager<AppUser> _userManager;
        private static readonly object _lock = new object();
        private static bool _firebaseInitialized = false;

        public FirebaseService(ApplicationDBContext context, UserManager<AppUser> userManager)
        {
            _dbContext = context;
            _userManager = userManager;
            InitializeFirebaseApp();
        }

        private void InitializeFirebaseApp()
        {
            lock (_lock)
            {
                if (!_firebaseInitialized && FirebaseApp.DefaultInstance == null)
                {
                    FirebaseApp.Create(new AppOptions()
                    {
                        Credential = GoogleCredential.FromFile("C:\\Users\\Jeebang\\Desktop\\Application Development\\BisleriumBloggingSystem\\Insfrastructure.BisleriumBloggingSystem\\Service\\AppData\\bislerium-blogging-system-firebase-adminsdk-60gcq-d94dee37b3.json")
                    });

                    _firebaseInitialized = true;
                }
            }
        }

        // Save Firebase Token
        public async Task<FirebaseToken> CreateNewToken(FirebaseToken payload, string userId)
        {
            Console.WriteLine(payload.UserID);

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("Token cannot be saved before login.");

            // Check if the user already has a token
            var existingToken = await _dbContext.FirebaseToken.FirstOrDefaultAsync(t => t.UserID == userId);

            if (existingToken != null)
            {
                // If the token already exists for the user, update it
                existingToken.Token = payload.Token;
                //await _dbContext.SaveChangesAsync();
                return existingToken;
            }
            else
            {
                // If the token doesn't exist for the user, create a new one
                var newToken = new FirebaseToken
                {
                    Token = payload.Token,
                    UserID = userId
                };
                await _dbContext.FirebaseToken.AddAsync(newToken);
                await _dbContext.SaveChangesAsync();
                return newToken;
            }
        }

        // Send push notification
        public async Task SendPushNotifications(string userId, string title, string body)
        {
            // Fetch tokens for the given user IDs
            var tokens = await _dbContext.FirebaseToken
                .Where(token => token.UserID == userId && token.Token != null)
                .Select(token => token.Token)
                .ToListAsync();
            Console.WriteLine("HEllo");
            var message = new MulticastMessage
            {
                Tokens = tokens,
                Notification = new FirebaseAdmin.Messaging.Notification
                {
                    Title = title,
                    Body = body
                }
            };
            try
            {
                var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
                Console.WriteLine("Hello I am Here");
            }
            catch (FirebaseMessagingException ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
            }
        }
    }
}
