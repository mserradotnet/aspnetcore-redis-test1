using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RedisWebApi.Models
{
    public class User
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email adress is required")]
        [EmailAddress(ErrorMessage = "Incorrect email adress format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public Gender? Gender { get; set; }

        public string IpAdress { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum Gender
    {
        Male,
        Female
    }
}
