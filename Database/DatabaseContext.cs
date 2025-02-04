﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobertDev_Chatbot.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Config> Config { get; set; }
        public DbSet<Commands> Commands { get; set; }
        public DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=RobertDev-Chatbot.db");
        }
    }

    public class Config
    {
        [Key]
        [Required]
        public string BotUsername { get; set; }
        [Required]
        public string BotAccessToken { get; set; }
        [Required]
        public string BotRefreshToken { get; set; }
        [Required]
        public string APIClientId { get; set; }
        [Required]
        public string APIClientSecret { get; set; }
        [Required]
        public string ChannelUsername { get; set; }
    }

    public class Commands
    {
        [Key]
        [Required]
        public string Command { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public int TimesUsed { get; set; }
    }

    public class Users
    {
        [Key]
        [Required]
        public string UserID { get; set; }
        [Required]
        public string Username { get; set; }
        public int MessageCount { get; set; }
        public int Points { get; set; }

    }

}
