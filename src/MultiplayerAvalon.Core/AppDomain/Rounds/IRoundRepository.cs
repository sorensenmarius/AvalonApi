using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerAvalon.AppDomain.Rounds
{
    public interface IRoundRepository: IRepository<Round, Guid>
    {
        public Task<int> VoteForTeam(Guid roundId);
        public Task<int> VoteAgainstTeam(Guid roundId);
        public Task<int> VoteForExpedition(Guid roundId);
        public Task<int> VoteAgainstExpedition(Guid roundId);
    }
}
