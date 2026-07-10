namespace Tetas.Web.Models.Api
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class RegisterRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string NickName { get; set; }

        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class AuthResponse
    {
        public string Token { get; set; }

        public DateTime Expiration { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }
    }

    public class UserDto
    {
        public string Email { get; set; }

        public string FullName { get; set; }

        public string NickName { get; set; }

        public string Phone { get; set; }

        public string Bio { get; set; }
    }

    public class CreatePostRequest
    {
        public string Name { get; set; }

        [Required]
        public string Body { get; set; }
    }

    public class CreateCommentRequest
    {
        public string Name { get; set; }

        [Required]
        public string Body { get; set; }
    }

    public class CommentDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Body { get; set; }

        public DateTime Date { get; set; }

        public string AuthorName { get; set; }

        public string AuthorEmail { get; set; }
    }

    public class PostDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Body { get; set; }

        public DateTime Date { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string AuthorName { get; set; }

        public string AuthorEmail { get; set; }

        public bool IsMine { get; set; }

        public List<CommentDto> Comments { get; set; } = new List<CommentDto>();
    }

    public class GroupDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string TypeName { get; set; }

        public string PrivacyName { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsMember { get; set; }
    }
}
