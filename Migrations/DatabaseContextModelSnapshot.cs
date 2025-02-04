﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RobertDev_Chatbot.Database;

#nullable disable

namespace RobertDev_Chatbot.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("RobertDev_Chatbot.Database.Commands", b =>
                {
                    b.Property<string>("Command")
                        .HasColumnType("TEXT");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("TimesUsed")
                        .HasColumnType("INTEGER");

                    b.HasKey("Command");

                    b.ToTable("Commands");
                });

            modelBuilder.Entity("RobertDev_Chatbot.Database.Config", b =>
                {
                    b.Property<string>("BotUsername")
                        .HasColumnType("TEXT");

                    b.Property<string>("APIClientId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("APIClientSecret")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("BotAccessToken")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("BotRefreshToken")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ChannelUsername")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("BotUsername");

                    b.ToTable("Config");
                });

            modelBuilder.Entity("RobertDev_Chatbot.Database.Users", b =>
                {
                    b.Property<string>("UserID")
                        .HasColumnType("TEXT");

                    b.Property<int>("MessageCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Points")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
