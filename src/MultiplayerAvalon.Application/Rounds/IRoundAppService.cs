using MultiplayerAvalon.AppDomain.Players;
using MultiplayerAvalon.Games.Dto;
using MultiplayerAvalon.Rounds.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static MultiplayerAvalon.Rounds.RoundAppService;

namespace MultiplayerAvalon.Rounds
{
    public interface IRoundAppService
    {
        public Task<RoundDto> VoteForTeam(Guid GameId, Guid PlayerId, bool Vote);
        public Task<RoundDto> ExpeditonVote(Guid GameId, Guid PlayerId, bool Vote);
        public Task<RoundDto> VoteForTeamResults(Guid GameId);
        public Task<RoundDto> ExpeditionResults(Guid GameId);
        public Task<RoundDto> AddPlayerToTeam(GameAndPlayerIdDto model);
        public Task<RoundDto> RemovePlayerFromTeam(GameAndPlayerIdDto model);
        public Task<HowManyPlayerHelper> HowManyPlayers(int RoundNmr, int TotalNmrPlayers);
    }
}
