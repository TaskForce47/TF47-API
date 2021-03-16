using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MimeKit.Encodings;
using Npgsql;
using TF47_Backend.Database.Models;
using TF47_Backend.Database.Models.GameServer;
using TF47_Backend.Database.Models.Services;

namespace TF47_Backend.Database
{
    public class DatabaseContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public DatabaseContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<Campaign> Campaigns { get; set; }
        public virtual DbSet<Mission> Missions { get; set; }
        public virtual DbSet<Playtime> Playtimes { get; set; }
        public virtual DbSet<Kill> Kills { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<Chat> Chats { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<GroupPermission> GroupPermissions { get; set; }
        public virtual DbSet<IssueGroup> IssueGroups { get; set; }
        public virtual DbSet<Issue> Issues { get; set; }
        public virtual DbSet<IssueItem> IssueItems { get; set; }
        public virtual DbSet<IssueTag> IssueTags { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) return;
            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = _configuration["Credentials:Database:Server"],
                Port = int.Parse(_configuration["Credentials:Database:Port"]),
                Username = _configuration["Credentials:Database:Username"],
                Password = _configuration["Credentials:Database:Password"],
                Database = _configuration["Credentials:Database:Database"]
            };
            //Console.WriteLine(builder.ToString());
            optionsBuilder.UseNpgsql(builder.ToString());
            optionsBuilder.UseSnakeCaseNamingConvention();
            optionsBuilder.LogTo(Console.WriteLine);

            //Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasPostgresEnum<Side>();
            builder.HasPostgresEnum<VehicleType>();
            builder.HasPostgresEnum<VehicleType>();

            builder.Entity<Player>(entity =>
            {
                entity.Property(x => x.PlayerId).ValueGeneratedOnAdd();
                entity.HasMany(x => x.PlayerWhitelistings)
                    .WithMany(x => x.Players)
                    .UsingEntity(y => y.ToTable("GameServer_PlayerWhitelistings".ToLower()));
                entity.HasMany(x => x.PlayerPositions).WithOne(x => x.Player);
                entity.HasMany(x => x.PlayerKills).WithOne(x => x.Killer);
                entity.HasMany(x => x.PlayerDeaths).WithOne(x => x.Victim);
                entity.HasMany(x => x.PlayerChats).WithOne(x => x.Player);
                entity.HasOne(x => x.PlayerPlaytime).WithOne(x => x.Player).HasForeignKey<Playtime>(x => x.PlayerId);
                entity.ToTable("GameServer_Players".ToLower());
            });
            builder.Entity<Session>(entity =>
            {
                entity.Property(x => x.SessionId).ValueGeneratedOnAdd();
                entity.HasOne(x => x.Mission).WithMany(x => x.Sessions);
                entity.HasMany(x => x.Kills).WithOne(x => x.Session);
                entity.HasMany(x => x.Chats).WithOne(x => x.Session);
                entity.ToTable("GameServer_Sessions".ToLower());
            });
            builder.Entity<Campaign>(entity =>
            {
                entity.Property(x => x.CampaignId).ValueGeneratedOnAdd();
                entity.HasMany(x => x.Missions).WithOne(x => x.Campaign);
                entity.ToTable("GameServer_Campaigns".ToLower());
            });
            builder.Entity<Mission>(entity =>
            {
                entity.HasKey(key => key.MissionId);
                entity.Property(x => x.MissionId).ValueGeneratedOnAdd();
                entity.HasMany(x => x.Sessions).WithOne(x => x.Mission);
                entity.HasOne(x => x.Campaign).WithMany(x => x.Missions);
                entity.ToTable("GameServer_Missions".ToLower());
            });
            builder.Entity<Position>(entity =>
            {
                entity.HasKey(key => key.PositionTrackerId);
                entity.Property(x => x.PositionTrackerId).ValueGeneratedOnAdd();
                entity.HasOne(x => x.Player).WithMany(x => x.PlayerPositions);
                entity.ToTable("GameServer_PlayerPositions".ToLower());
            });
            builder.Entity<Kill>(entity =>
            {
                entity.Property(x => x.KillId).ValueGeneratedOnAdd();
                entity.HasOne(x => x.Killer).WithMany(x => x.PlayerKills);
                entity.HasOne(x => x.Victim).WithMany(x => x.PlayerDeaths);
                entity.HasOne(x => x.Session).WithMany(x => x.Kills);
                entity.ToTable("GameServer_Kills".ToLower());
            });
            builder.Entity<Playtime>(entity =>
            {
                entity.Property(x => x.PlayTimeId).ValueGeneratedOnAdd();
                entity.HasOne(x => x.Player).WithOne(x => x.PlayerPlaytime);
                entity.HasOne(x => x.Session).WithMany(x => x.PlayTimes);
                entity.ToTable("GameServer_Playtimes".ToLower());
            });
            builder.Entity<Whitelist>(entity =>
            {
                entity.Property(x => x.WhitelistId).ValueGeneratedOnAdd();
                entity.ToTable("GameServer_Whitelists".ToLower());
            });
            builder.Entity<Chat>(entity =>
            {
                entity.Property(x => x.ChatId).ValueGeneratedOnAdd();
                entity.HasOne(x => x.Session).WithMany(x => x.Chats);
                entity.HasOne(x => x.Player).WithMany(x => x.PlayerChats);
                entity.ToTable("GameServer_Chats".ToLower());
            });
            builder.Entity<User>(entity =>
            {
                entity.Property(x => x.UserId).ValueGeneratedOnAdd();
                entity.HasMany(x => x.Groups).WithMany(x => x.Users)
                    .UsingEntity(y => y.ToTable("Service_UserGroups".ToLower()));
                entity.ToTable("Service_Users".ToLower());
            });
            builder.Entity<Group>(entity =>
            {
                entity.Property(x => x.GroupId).ValueGeneratedOnAdd();
                entity.HasOne(x => x.GroupPermission).WithOne(x => x.Group);
                entity.ToTable("Service_Groups".ToLower());
            });
            builder.Entity<GroupPermission>(entity =>
            {
                entity.Property(x => x.GroupPermissionId).ValueGeneratedOnAdd();
                entity.HasOne(x => x.Group).WithOne(x => x.GroupPermission)
                    .HasForeignKey<GroupPermission>(x => x.GroupId);
                entity.ToTable("Service_GroupPermissions".ToLower());
            });
            builder.Entity<IssueGroup>(entity =>
            {
                entity.Property(x => x.IssueGroupId).ValueGeneratedOnAdd();
                entity.ToTable("Service_Issue_Groups".ToLower());
                entity.HasMany(x => x.Issues)
                    .WithOne(y => y.IssueGroup)
                    .HasForeignKey(y => y.IssueGroupId);
            });
            builder.Entity<Issue>(entity =>
            {
                entity.Property(x => x.IssueId).ValueGeneratedOnAdd();
                entity.ToTable("Service_Issues".ToLower());
                entity.HasMany(x => x.IssueItems)
                    .WithOne(y => y.Issue)
                    .HasForeignKey(y => y.IssueId);
                entity.HasMany(x => x.IssueTags)
                    .WithMany(x => x.Issues)
                    .UsingEntity(y => y.ToTable("Service_Issue_Has_Tags".ToLower()));
            });
            builder.Entity<IssueItem>(entity =>
            {
                entity.Property(x => x.IssueItemId).ValueGeneratedOnAdd();
                entity.ToTable("Service_Issue_Items".ToLower());
            });
            builder.Entity<IssueTag>(entity =>
            {
                entity.Property(x => x.IssueTagId).ValueGeneratedOnAdd();
                entity.ToTable("Service_Issue_Tags".ToLower());
            });
        }
    }
}
