﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;

namespace Volo.CmsKit.Reactions
{
    public class EfCoreUserReactionRepository : EfCoreRepository<ICmsKitDbContext, UserReaction, Guid>,
        IUserReactionRepository
    {
        public EfCoreUserReactionRepository(IDbContextProvider<ICmsKitDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<UserReaction> FindAsync(
            Guid userId,
            string entityType,
            string entityId,
            string reactionName)
        {
            return await DbSet
                .Where(x =>
                    x.UserId == userId &&
                    x.EntityType == entityType &&
                    x.EntityId == entityId &&
                    x.ReactionName == reactionName)
                .FirstOrDefaultAsync();
        }

        public async Task<List<UserReaction>> GetListForUserAsync(Guid userId, string entityType, string entityId)
        {
            return await DbSet
                .Where(x =>
                    x.UserId == userId &&
                    x.EntityType == entityType &&
                    x.EntityId == entityId)
                .ToListAsync();
        }

        public async Task<List<ReactionSummaryQueryResultItem>> GetSummariesAsync(string entityType, string entityId)
        {
            return await DbSet
                .Where(x =>
                    x.EntityType == entityType &&
                    x.EntityId == entityId)
                .GroupBy(x => x.ReactionName)
                .Select(g => new ReactionSummaryQueryResultItem
                {
                    ReactionName = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();
        }
    }
}
