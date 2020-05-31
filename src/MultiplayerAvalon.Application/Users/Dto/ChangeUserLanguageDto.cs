using System.ComponentModel.DataAnnotations;

namespace MultiplayerAvalon.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}