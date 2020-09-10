using Abp.Data;
using Abp.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MultiplayerAvalon.AppDomain.Rounds;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerAvalon.EntityFrameworkCore.Repositories
{
    public class RoundRepository : MultiplayerAvalonRepositoryBase<Round, Guid>, IRoundRepository
    {
        private readonly IActiveTransactionProvider _transactionProvider;

        public RoundRepository(IDbContextProvider<MultiplayerAvalonDbContext> dbContextProvider, IActiveTransactionProvider transactionProvider) : base(dbContextProvider)
        {
            _transactionProvider = transactionProvider;
        }

        public async Task<int> VoteAgainstExpedition(Guid roundId)
        {
            var roundIdParam = new SqlParameter("RoundId", roundId);
            var totalVotesParam = new SqlParameter("TotalVotes", 0) { Direction = System.Data.ParameterDirection.Output };
            await Context.Database.ExecuteSqlRawAsync("EXEC [dbo].[VoteAgainstTeam] @RoundId, @TotalVotes output", new[] { roundIdParam, totalVotesParam });
            return Convert.ToInt32(totalVotesParam.Value);
        }

        public async Task<int> VoteAgainstTeam(Guid roundId)
        {
            var roundIdParam = new SqlParameter("RoundId", roundId);
            var totalVotesParam = new SqlParameter("TotalVotes", 0) { Direction = System.Data.ParameterDirection.Output };
            await Context.Database.ExecuteSqlRawAsync("EXEC [dbo].[VoteAgainstTeam] @RoundId, @TotalVotes output", new[] { roundIdParam, totalVotesParam });
            return Convert.ToInt32(totalVotesParam.Value);
        }

        public async Task<int> VoteForExpedition(Guid roundId)
        {
            var roundIdParam = new SqlParameter("RoundId", roundId);
            var totalVotesParam = new SqlParameter("TotalVotes", 0) { Direction = System.Data.ParameterDirection.Output };
            await Context.Database.ExecuteSqlRawAsync("EXEC [dbo].[VoteForExpedition] @RoundId, @TotalVotes output", new[] { roundIdParam, totalVotesParam });
            return Convert.ToInt32(totalVotesParam.Value);
        }

        public async Task<int> VoteForTeam(Guid roundId)
        {
            var roundIdParam = new SqlParameter("RoundId", roundId);
            var totalVotesParam = new SqlParameter("TotalVotes", 0) { Direction = System.Data.ParameterDirection.Output };
            await Context.Database.ExecuteSqlRawAsync("EXEC [dbo].[VoteForTeam] @RoundId, @TotalVotes output", new[] { roundIdParam, totalVotesParam });
            return Convert.ToInt32(totalVotesParam.Value);
        }
    }
}
