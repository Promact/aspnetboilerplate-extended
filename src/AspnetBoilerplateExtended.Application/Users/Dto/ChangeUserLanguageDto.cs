using System.ComponentModel.DataAnnotations;

namespace AspnetBoilerplateExtended.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}