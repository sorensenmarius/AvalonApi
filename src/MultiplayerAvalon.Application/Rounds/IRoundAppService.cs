using MultiplayerAvalon.AppDomain.Games;
using MultiplayerAvalon.AppDomain.Players;
using MultiplayerAvalon.AppDomain.Rounds;
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
        public Task<Round> VoteForTeam(VoteDto model);
        public Task<Round> ExpeditonVote(VoteDto model);
        public HowManyPlayerHelper HowManyPlayers(int RoundNmr, int TotalNmrPlayers);
    }
}
