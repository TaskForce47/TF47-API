﻿namespace TF47_Backend.Dto
{
    public class UserRegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool AcceptTermsOfService { get; set; }

    }
}
