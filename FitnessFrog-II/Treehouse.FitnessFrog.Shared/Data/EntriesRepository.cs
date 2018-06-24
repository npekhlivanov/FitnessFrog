using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Treehouse.FitnessFrog.Shared.Models;

namespace Treehouse.FitnessFrog.Shared.Data
{
    /// <summary>
    /// The repository for entries.
    /// </summary>
    public class EntriesRepository : BaseRepository<Entry>
    {
        public EntriesRepository(Context context) 
            : base(context)
        {
        }

        public override Entry Get(int id, bool includeRelatedEntities = true)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a single entry for the provided ID.
        /// </summary>
        /// <param name="id">The ID for the entry to return.</param>
        /// <param name="includeRelatedEntities">Indicates whether or not to include related entities.</param>
        /// <returns>An entry.</returns>
        public Entry Get(int id, string userId, bool includeRelatedEntities = true)
        {
            var entries = Context.Entries.AsQueryable();

            if (includeRelatedEntities)
            {
                entries = entries
                    .Include(e => e.Activity);
            }

            return entries
                .Where(e => e.Id == id && e.UserId.Equals(userId))
                .SingleOrDefault();
        }

        public override IList<Entry> GetList()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a collection of entries.
        /// </summary>
        /// <param name="userId">The user ID to retrieve entries for.</param>
        /// <returns>A list of entries.</returns>
        public IList<Entry> GetList(string userId)
        {
            return Context.Entries
                .Where(e => e.UserId == userId)
                .Include(e => e.Activity)
                .OrderByDescending(e => e.Date)
                .ThenByDescending(e => e.Id)
                .ToList();
        }

        /// <summary>
        /// Determines if an entry is owned by the provided user ID.
        /// </summary>
        /// <param name="id">The entry ID to check.</param>
        /// <param name="userId">The user ID that should own the entry.</param>
        /// <returns>Returns a boolean indicating if the user ID owns the entry.</returns>
        public bool EntryIsOwnedByUser(int id, string userId)
        {
            var count = Context.Entries.Count(e => e.Id == id && e.UserId.Equals(userId));
            return count == 1;
        }
    }
}