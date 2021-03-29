﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MimeKit.Encodings;
using Npgsql;
using TF47_API.Database.Models;
using TF47_API.Database.Models.GameServer;
using TF47_API.Database.Models.GameServer.AAR;
using TF47_API.Database.Models.Services;

namespace TF47_API.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<Campaign> Campaigns { get; set; }
        public virtual DbSet<Mission> Missions { get; set; }
        public virtual DbSet<Playtime> Playtimes { get; set; }
        public virtual DbSet<Kill> Kills { get; set; }
        public virtual DbSet<Chat> Chats { get; set; }
        public virtual DbSet<Note> Notes { get; set; }
        public virtual DbSet<ReplayItem> ReplayItems { get; set; }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<GroupPermission> GroupPermissions { get; set; }
        public virtual DbSet<IssueGroup> IssueGroups { get; set; }
        public virtual DbSet<Issue> Issues { get; set; }
        public virtual DbSet<IssueItem> IssueItems { get; set; }
        public virtual DbSet<IssueTag> IssueTags { get; set; }
        public virtual DbSet<ApiKey> ApiKeys { get; set; }
        public virtual DbSet<Changelog> Changelogs { get; set; }
        public virtual DbSet<Squad> Squads { get; set; }
        public virtual DbSet<SquadMember> SquadMembers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasPostgresEnum<Side>();
            builder.HasPostgresEnum<VehicleType>();
            builder.HasPostgresEnum<VehicleType>();

            builder.Entity<Player>(entity =>
            {
                entity.HasMany(x => x.PlayerWhitelistings)
                    .WithMany(x => x.Players)
                    .UsingEntity(y => y.ToTable("GameServerPlayerWhitelistings"));

                entity.HasMany(x => x.PlayerKills).WithOne(x => x.Killer).HasForeignKey(x => x.KillerId);
                entity.HasMany(x => x.PlayerDeaths).WithOne(x => x.Victim).HasForeignKey(x => x.VictimId);
                entity.HasMany(x => x.PlayerChats).WithOne(x => x.Player).HasForeignKey(x => x.ChatId);
                entity.HasMany(x => x.PlayerNotes).WithOne(x => x.Player).HasForeignKey(x => x.PlayerId);
                entity.HasOne(x => x.PlayerPlaytime).WithOne(x => x.Player);
            });
            builder.Entity<Session>(entity =>
            {
                entity.Property(x => x.SessionId).ValueGeneratedOnAdd();
                entity.HasOne(x => x.Mission).WithMany(x => x.Sessions).HasForeignKey(x => x.MissionId);
                entity.HasMany(x => x.Kills).WithOne(x => x.Session).HasForeignKey(x => x.SessionId);
                entity.HasMany(x => x.Chats).WithOne(x => x.Session).HasForeignKey(x => x.ChatId);
            });
            builder.Entity<Campaign>(entity =>
            {
                entity.Property(x => x.CampaignId).ValueGeneratedOnAdd();
                entity.HasMany(x => x.Missions).WithOne(x => x.Campaign).HasForeignKey(x => x.CampaignId);
            });
            builder.Entity<Mission>(entity =>
            {
                entity.Property(x => x.MissionId).ValueGeneratedOnAdd();
                entity.HasMany(x => x.Sessions).WithOne(x => x.Mission).HasForeignKey(x => x.MissionId);
                entity.HasOne(x => x.Campaign).WithMany(x => x.Missions).HasForeignKey(x => x.CampaignId);
            });
            builder.Entity<Kill>(entity =>
            {
                entity.Property(x => x.KillId).ValueGeneratedOnAdd();
                entity.HasOne(x => x.Killer).WithMany(x => x.PlayerKills).HasForeignKey(x => x.KillerId);
                entity.HasOne(x => x.Victim).WithMany(x => x.PlayerDeaths).HasForeignKey(x => x.VictimId);
                entity.HasOne(x => x.Session).WithMany(x => x.Kills).HasForeignKey(x => x.SessionId);
            });
            builder.Entity<Playtime>(entity =>
            {
                entity.HasOne(x => x.Player).WithOne(x => x.PlayerPlaytime);
                entity.HasOne(x => x.Session).WithMany(x => x.PlayTimes).HasForeignKey(x => x.SessionId);
            });
            builder.Entity<Whitelist>(entity => { entity.Property(x => x.WhitelistId).ValueGeneratedOnAdd(); });
            builder.Entity<Chat>(entity =>
            {
                entity.Property(x => x.ChatId).ValueGeneratedOnAdd();
                entity.HasOne(x => x.Session).WithMany(x => x.Chats).HasForeignKey(x => x.SessionId);
                entity.HasOne(x => x.Player).WithMany(x => x.PlayerChats).HasForeignKey(x => x.PlayerId);
            });
            builder.Entity<Note>(entity => { entity.Property(x => x.NoteId).ValueGeneratedOnAdd(); });
            builder.Entity<User>(entity =>
            {
                entity.Property(x => x.UserId).ValueGeneratedOnAdd();
                entity.HasMany(x => x.WrittenNotes)
                    .WithOne(x => x.Writer)
                    .HasForeignKey(x => x.WriterId);
                entity.HasMany(x => x.Groups)
                    .WithMany(x => x.Users)
                    .UsingEntity(y => y.ToTable("ServiceUserGroups"));
                entity.HasMany(x => x.ApiKeys)
                    .WithOne(x => x.Owner)
                    .HasForeignKey(x => x.OwnerId);
                entity.HasMany(x => x.WrittenChangelogs)
                    .WithOne(x => x.Author)
                    .HasForeignKey(x => x.AuthorId);
            });
            builder.Entity<Group>(entity =>
            {
                entity.Property(x => x.GroupId).ValueGeneratedOnAdd();
                entity.HasOne(x => x.GroupPermission)
                    .WithOne(x => x.Group);
            });
            builder.Entity<GroupPermission>(entity =>
            {
                entity.Property(x => x.GroupPermissionId).ValueGeneratedOnAdd();
                entity.HasOne(x => x.Group).WithOne(x => x.GroupPermission)
                    .HasForeignKey<GroupPermission>(x => x.GroupId);
            });
            builder.Entity<IssueGroup>(entity =>
            {
                entity.Property(x => x.IssueGroupId).ValueGeneratedOnAdd();
                entity.HasMany(x => x.Issues)
                    .WithOne(y => y.IssueGroup)
                    .HasForeignKey(y => y.IssueGroupId);
            });
            builder.Entity<Issue>(entity =>
            {
                entity.Property(x => x.IssueId).ValueGeneratedOnAdd();
                entity.HasMany(x => x.IssueItems)
                    .WithOne(y => y.Issue)
                    .HasForeignKey(y => y.IssueId);
                entity.HasMany(x => x.IssueTags)
                    .WithMany(x => x.Issues)
                    .UsingEntity(y => y.ToTable("ServiceIssueHasTags"));
            });
            builder.Entity<IssueItem>(entity => { entity.Property(x => x.IssueItemId).ValueGeneratedOnAdd(); });
            builder.Entity<IssueTag>(entity => { entity.Property(x => x.IssueTagId).ValueGeneratedOnAdd(); });
            builder.Entity<ApiKey>(entity =>
            {
                entity.Property(x => x.ApiKeyId).ValueGeneratedOnAdd();
                entity.HasOne(x => x.Owner)
                    .WithMany(x => x.ApiKeys)
                    .HasForeignKey(x => x.OwnerId);
            });
            builder.Entity<Changelog>(entity =>
            {
                entity.Property(x => x.ChangelogId).ValueGeneratedOnAdd();
                entity.HasOne(x => x.Author)
                    .WithMany(x => x.WrittenChangelogs)
                    .HasForeignKey(x => x.AuthorId);
            });
            builder.Entity<Squad>(entity =>
            {
                entity.Property(x => x.SquadId).ValueGeneratedOnAdd();
                entity.HasMany(x => x.SquadMembers)
                    .WithOne(x => x.Squad)
                    .HasForeignKey(x => x.SquadId);
            });
            builder.Entity<SquadMember>(entity =>
            {
                entity.Property(x => x.SquadMemberId).ValueGeneratedOnAdd();
                entity.HasOne(x => x.Squad)
                    .WithMany(x => x.SquadMembers)
                    .HasForeignKey(x => x.SquadId);
            });
        }
    }
}