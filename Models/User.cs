using System.ComponentModel.DataAnnotations;
using AutoMapper;

namespace api.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public Boolean Active { get; set; }
        public string? Role { get; set; }

        public DateTime Creation { get; set; }
        public DateTime Modification { get; set; }
    }

    public class UserInfo
    {
        [Required]
        public string? Username { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public Boolean Active { get; set; }
        public string? Role { get; set; }

        public DateTime Creation { get; set; }
        public DateTime Modification { get; set; }
    }

    public class UserCreate
    {
        [Required]
        public string? Username { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        public Boolean Active { get; set; }
        public string? Role { get; set; }
    }

    public class UserResponse : UserInfo
    {
        public int Id { get; set; }
    }

    public class UserUpdate
    {
    }

    public class Login
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        [MinLength(5)]
        public string? Password { get; set; }
    }

    public class Check
    {
        [Required]
        public string? Username { get; set; }
        [Required, EmailAddress]
        public string? Email { get; set; }
    }

    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserResponse>();
            CreateMap<UserCreate, User>();
            CreateMap<UserUpdate, User>();
            CreateMap<User, UserUpdate>();
            CreateMap<Login, User>();
            CreateMap<Check, User>();
        }
    }

}
