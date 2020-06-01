using AutoMapper;
using MultiplayerAvalon.AppDomain.Players;

namespace MultiplayerAvalon.Players.Dto
{
    public class PlayerMapProfile : Profile
    {
        public PlayerMapProfile()
        {
            CreateMap<PlayerDto, Player>().ReverseMap();
            CreateMap<CreatePlayerDto, Player>().ReverseMap();
        }
    }
}
