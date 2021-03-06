﻿using System.Threading.Tasks;
using PriceCollector.Api.WebAPI.Responses;

namespace PriceCollector.Api.WebAPI.User
{
    public interface IUserApi
    {
        /// <summary>
        /// Rotina de login do usuário;
        /// </summary>
        /// <param name="userName">Nome do usuário</param>
        /// <param name="password">Senha do usuário</param>
        /// <returns></returns>
        Task<UserResponse> Login(string userName, string password);
    }
}