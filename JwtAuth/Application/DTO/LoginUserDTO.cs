﻿namespace JwtAuth.Application.DTO;

public class LoginUserDTO
{
    public string email { get; set; } = string.Empty;
    public string password { get; set; } = string.Empty;
}
