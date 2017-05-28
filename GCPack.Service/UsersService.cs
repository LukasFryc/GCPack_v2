﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCPack.Model;
using GCPack.Repository.Interfaces;
using AutoMapper;
using GCPack.Service.Interfaces;

namespace GCPack.Service
{
    public class UsersService : IUsersService
    {
        readonly IUsersRepository usersRepository;
        public UsersService(IUsersRepository usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public string Login(string username, string password)
        {
            // pokud se uzivatel prihlasi, pak se prida ticket do databaze
            string ticket = "";
            UserModel user = usersRepository.GetUser(username, password);
            if (user != null)
            {
                ticket = Guid.NewGuid().ToString();
                usersRepository.UpdateTicket(ticket, user);
            }
            return ticket;
        }

        public UserModel GetUser(string ticket)
        {
            return usersRepository.GetUser(ticket);
        }


        public UserModel GetUser(int userID)
        {
            return new UserModel();
        }

        public void UpdateTicket(string ticket, UserModel user)
        {
            usersRepository.UpdateTicket(ticket,user);
        }

    }
}
