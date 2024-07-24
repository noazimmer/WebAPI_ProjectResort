﻿//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;
//using System;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace BL.Middlewares
//{
//    public class JwtMiddleware
//    {
//        private readonly RequestDelegate _next;
//        private readonly IConfiguration _configuration;

//        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
//        {
//            _next = next;
//            _configuration = configuration;
//        }

//        public async Task InvokeAsync(HttpContext context)
//        {
//            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

//            if (token != null)
//                await AttachUserToContext(context, token);

//            await _next(context);
//        }

//        private async Task AttachUserToContext(HttpContext context, string token)
//        {
//            try
//            {
//                var tokenHandler = new JwtSecurityTokenHandler();
//                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
//                tokenHandler.ValidateToken(token, new TokenValidationParameters
//                {
//                    ValidateIssuerSigningKey = true,
//                    IssuerSigningKey = new SymmetricSecurityKey(key),
//                    ValidateIssuer = true,
//                    ValidIssuer = _configuration["Jwt:Issuer"],
//                    ValidateAudience = true,
//                    ValidAudience = _configuration["Jwt:Audience"],
//                    ClockSkew = TimeSpan.Zero
//                }, out SecurityToken validatedToken);

//                var jwtToken = (JwtSecurityToken)validatedToken;
//                var userId = jwtToken.Claims.First(x => x.Type == "id").Value;

//                // attach user to context on successful jwt validation
//                context.Items["User"] = userId;
//            }
//            catch
//            {
//                // do nothing if jwt validation fails
//                // user is not attached to context so request won't have access to secure routes
//            }
//        }
//    }
//}
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;
//using System;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace BL.Middlewares
//{
//    public class JwtMiddleware
//    {
//        private readonly RequestDelegate _next;
//        private readonly IConfiguration _configuration;

//        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
//        {
//            _next = next;
//            _configuration = configuration;
//        }

//        public async Task InvokeAsync(HttpContext context)
//        {
//            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

//            if (token != null)
//                await AttachUserToContext(context, token);

//            await _next(context);
//        }

//        private async Task AttachUserToContext(HttpContext context, string token)
//        {
//            try
//            {
//                var tokenHandler = new JwtSecurityTokenHandler();
//                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
//                tokenHandler.ValidateToken(token, new TokenValidationParameters
//                {
//                    ValidateIssuerSigningKey = true,
//                    IssuerSigningKey = new SymmetricSecurityKey(key),
//                    ValidateIssuer = true,
//                    ValidIssuer = _configuration["Jwt:Issuer"],
//                    ValidateAudience = true,
//                    ValidAudience = _configuration["Jwt:Audience"],
//                    ClockSkew = TimeSpan.Zero
//                }, out SecurityToken validatedToken);

//                var jwtToken = (JwtSecurityToken)validatedToken;
//                var userPhone = jwtToken.Claims.First(x => x.Type == "phone").Value;

//                // Attach user phone to context on successful jwt validation
//                context.Items["UserPhone"] = userPhone;
//            }
//            catch
//            {
//                // Do nothing if jwt validation fails
//                // User is not attached to context so request won't have access to secure routes
//            }
//        }
//    }
//}

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BL.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                await AttachUserToContext(context, token);

            await _next(context);
        }

        private async Task AttachUserToContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

                // Attach user id to context on successful jwt validation
                context.Items["User"] = userId;
            }
            catch
            {
                // Do nothing if jwt validation fails
                // User is not attached to context so request won't have access to secure routes
            }
        }

    }
}