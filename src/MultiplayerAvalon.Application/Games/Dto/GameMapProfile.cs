using AutoMapper;
using MultiplayerAvalon.AppDomain.Games;

namespace MultiplayerAvalon.Games.Dto
{
    public class GameMapProfile : Profile
    {
        public GameMapProfile()
        {
            CreateMap<GameDto, Game>().ReverseMap();
            CreateMap<CreateGameDto, Game>().ReverseMap();
        }
    }
}
