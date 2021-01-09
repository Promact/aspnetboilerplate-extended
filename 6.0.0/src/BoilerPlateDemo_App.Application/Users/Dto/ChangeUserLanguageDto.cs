using System.ComponentModel.DataAnnotations;

namespace BoilerPlateDemo_App.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}