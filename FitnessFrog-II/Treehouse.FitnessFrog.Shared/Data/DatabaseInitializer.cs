using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;
using Treehouse.FitnessFrog.Shared.Models;
using Treehouse.FitnessFrog.Shared.Security;

namespace Treehouse.FitnessFrog.Shared.Data
{
    /// <summary>
    /// Custom database initializer class used to populate
    /// the database with seed data.
    /// </summary>
    internal class DatabaseInitializer : DropCreateDatabaseIfModelChanges<Context>
    {
        protected override void Seed(Context context)
        {
            var activityBasketball = new Activity() { Name = "Basketball" };
            var activityBiking = new Activity() { Name = "Biking" };
            var activityHiking = new Activity() { Name = "Hiking" };
            var activityKayaking = new Activity() { Name = "Kayaking" };
            var activityPokemonGo = new Activity() { Name = "Pokemon Go" };
            var activityRunning = new Activity() { Name = "Running" };
            var activitySkiing = new Activity() { Name = "Skiing" };
            var activitySwimming = new Activity() { Name = "Swimming" };
            var activityWalking = new Activity() { Name = "Walking" };
            var activityWeightLifting = new Activity() { Name = "Weight Lifting" };

            var activities = new List<Activity>()
            {
                activityBasketball,
                activityBiking,
                activityHiking,
                activityKayaking,
                activityPokemonGo,
                activityRunning,
                activitySkiing,
                activitySwimming,
                activityWalking,
                activityWeightLifting
            };

            context.Activities.AddRange(activities);

            // Create the users
            var user1 = new User { Email = "nicko@omegasoft.bg", UserName = "nicko@omegasoft.bg", DisplayName = "Nicko P" };
            var user2 = new User { Email = "n.pekhlivanov@gmail.com", UserName = "n.pekhlivanov@gmail.com", DisplayName = "N. Pekhlivanov" };
            using (var userStore = new UserStore<User>(context))
            {
                using (var userManager = new ApplicationUserManager(userStore))
                {
                    var result1 = userManager.Create(user1, "123456");
                    var result2 = userManager.Create(user2, "123456");
                }
            }

            var entries = new List<Entry>()
            {
                new Entry(user1, 2017, 7, 8, activityBiking, 10.0m),
                new Entry(user1, 2017, 7, 9, activityBiking, 12.2m),
                new Entry(user1, 2017, 7, 10, activityHiking, 123.0m),
                new Entry(user1, 2017, 7, 12, activityBiking, 10.0m),
                new Entry(user1, 2017, 7, 13, activityWalking, 32.2m),
                new Entry(user1, 2017, 7, 13, activityBiking, 13.3m),
                new Entry(user2, 2017, 7, 14, activityBiking, 10.0m),
                new Entry(user2, 2017, 7, 15, activityWalking, 28.6m),
                new Entry(user2, 2017, 7, 16, activityBiking, 12.7m),
                new Entry(user2, 2017, 7, 16, activityPokemonGo, 23.4m)
            };

            context.Entries.AddRange(entries);

            context.SaveChanges();
        }
    }
}
