﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using NpgsqlTypes;
using TF47_Backend.Database;

namespace TF47_Backend.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20210110005038_table_prefix")]
    partial class table_prefix
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasPostgresEnum(null, "side", new[] { "civilian", "bluefor", "redfor", "independent" })
                .HasPostgresEnum(null, "vehicle_type", new[] { "infantry", "light_vehicle", "tank", "helicopter", "fixed_wing", "boat" })
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("TF47_Backend.Database.Models.Campaign", b =>
                {
                    b.Property<long>("CampaignId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("campaign_id")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<DateTime>("TimeCreated")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("time_created");

                    b.HasKey("CampaignId")
                        .HasName("pk_gameserver_campaigns");

                    b.ToTable("gameserver_campaigns");
                });

            modelBuilder.Entity("TF47_Backend.Database.Models.Chat", b =>
                {
                    b.Property<long>("ChatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("chat_id")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("Channel")
                        .HasColumnType("text")
                        .HasColumnName("channel");

                    b.Property<long?>("PlayerId")
                        .HasColumnType("bigint")
                        .HasColumnName("player_id");

                    b.Property<long?>("SessionId")
                        .HasColumnType("bigint")
                        .HasColumnName("session_id");

                    b.Property<string>("Text")
                        .HasColumnType("text")
                        .HasColumnName("text");

                    b.Property<DateTime>("TimeSend")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("time_send");

                    b.HasKey("ChatId")
                        .HasName("pk_gameserver_chats");

                    b.HasIndex("PlayerId")
                        .HasDatabaseName("ix_gameserver_chats_player_id");

                    b.HasIndex("SessionId")
                        .HasDatabaseName("ix_gameserver_chats_session_id");

                    b.ToTable("gameserver_chats");
                });

            modelBuilder.Entity("TF47_Backend.Database.Models.Kill", b =>
                {
                    b.Property<long>("KillId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("kill_id")
                        .UseIdentityByDefaultColumn();

                    b.Property<long>("Distance")
                        .HasColumnType("bigint")
                        .HasColumnName("distance");

                    b.Property<long>("GameTime")
                        .HasColumnType("bigint")
                        .HasColumnName("game_time");

                    b.Property<long?>("KillerPlayerId")
                        .HasColumnType("bigint")
                        .HasColumnName("killer_player_id");

                    b.Property<int>("KillerSide")
                        .HasColumnType("integer")
                        .HasColumnName("killer_side");

                    b.Property<int>("KillerVehicleType")
                        .HasColumnType("integer")
                        .HasColumnName("killer_vehicle_type");

                    b.Property<DateTime>("RealTime")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("real_time");

                    b.Property<long?>("SessionId")
                        .HasColumnType("bigint")
                        .HasColumnName("session_id");

                    b.Property<string>("VehicleName")
                        .HasColumnType("text")
                        .HasColumnName("vehicle_name");

                    b.Property<long?>("VictimPlayerId")
                        .HasColumnType("bigint")
                        .HasColumnName("victim_player_id");

                    b.Property<int>("VictimSide")
                        .HasColumnType("integer")
                        .HasColumnName("victim_side");

                    b.Property<int>("VictimVehicleType")
                        .HasColumnType("integer")
                        .HasColumnName("victim_vehicle_type");

                    b.Property<string>("Weapon")
                        .HasColumnType("text")
                        .HasColumnName("weapon");

                    b.HasKey("KillId")
                        .HasName("pk_gameserver_kills");

                    b.HasIndex("KillerPlayerId")
                        .HasDatabaseName("ix_gameserver_kills_killer_player_id");

                    b.HasIndex("SessionId")
                        .HasDatabaseName("ix_gameserver_kills_session_id");

                    b.HasIndex("VictimPlayerId")
                        .HasDatabaseName("ix_gameserver_kills_victim_player_id");

                    b.ToTable("gameserver_kills");
                });

            modelBuilder.Entity("TF47_Backend.Database.Models.Mission", b =>
                {
                    b.Property<long>("MissionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("mission_id")
                        .UseIdentityByDefaultColumn();

                    b.Property<long?>("CampaignId")
                        .HasColumnType("bigint")
                        .HasColumnName("campaign_id");

                    b.Property<int>("MissionType")
                        .HasColumnType("integer")
                        .HasColumnName("mission_type");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("MissionId")
                        .HasName("pk_gameserver_missions");

                    b.HasIndex("CampaignId")
                        .HasDatabaseName("ix_gameserver_missions_campaign_id");

                    b.ToTable("gameserver_missions");
                });

            modelBuilder.Entity("TF47_Backend.Database.Models.Player", b =>
                {
                    b.Property<long>("PlayerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("player_id")
                        .UseIdentityByDefaultColumn();

                    b.Property<DateTime>("FirstVisit")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("first_visit");

                    b.Property<DateTime>("LastVisit")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("last_visit");

                    b.Property<string>("PlayerName")
                        .HasColumnType("text")
                        .HasColumnName("player_name");

                    b.Property<string>("PlayerUid")
                        .HasColumnType("text")
                        .HasColumnName("player_uid");

                    b.HasKey("PlayerId")
                        .HasName("pk_gameserver_players");

                    b.ToTable("gameserver_players");
                });

            modelBuilder.Entity("TF47_Backend.Database.Models.Playtime", b =>
                {
                    b.Property<long>("PlayTimeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("play_time_id")
                        .UseIdentityByDefaultColumn();

                    b.Property<long>("PlayerId")
                        .HasColumnType("bigint")
                        .HasColumnName("player_id");

                    b.Property<long?>("SessionId")
                        .HasColumnType("bigint")
                        .HasColumnName("session_id");

                    b.Property<TimeSpan>("TimePlayedBoat")
                        .HasColumnType("interval")
                        .HasColumnName("time_played_boat");

                    b.Property<TimeSpan>("TimePlayedFixedWing")
                        .HasColumnType("interval")
                        .HasColumnName("time_played_fixed_wing");

                    b.Property<TimeSpan>("TimePlayedHelicopter")
                        .HasColumnType("interval")
                        .HasColumnName("time_played_helicopter");

                    b.Property<TimeSpan>("TimePlayedInfantry")
                        .HasColumnType("interval")
                        .HasColumnName("time_played_infantry");

                    b.Property<TimeSpan>("TimePlayedTank")
                        .HasColumnType("interval")
                        .HasColumnName("time_played_tank");

                    b.Property<TimeSpan>("TimePlayedVehicle")
                        .HasColumnType("interval")
                        .HasColumnName("time_played_vehicle");

                    b.Property<TimeSpan>("TimeTrackedObjective")
                        .HasColumnType("interval")
                        .HasColumnName("time_tracked_objective");

                    b.HasKey("PlayTimeId")
                        .HasName("pk_gameserver_playtimes");

                    b.HasIndex("PlayerId")
                        .IsUnique()
                        .HasDatabaseName("ix_gameserver_playtimes_player_id");

                    b.HasIndex("SessionId")
                        .HasDatabaseName("ix_gameserver_playtimes_session_id");

                    b.ToTable("gameserver_playtimes");
                });

            modelBuilder.Entity("TF47_Backend.Database.Models.Position", b =>
                {
                    b.Property<long>("PositionTrackerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("position_tracker_id")
                        .UseIdentityByDefaultColumn();

                    b.Property<long>("Direction")
                        .HasColumnType("bigint")
                        .HasColumnName("direction");

                    b.Property<string>("Group")
                        .HasColumnType("text")
                        .HasColumnName("group");

                    b.Property<bool>("IsAwake")
                        .HasColumnType("boolean")
                        .HasColumnName("is_awake");

                    b.Property<bool>("IsDriver")
                        .HasColumnType("boolean")
                        .HasColumnName("is_driver");

                    b.Property<long?>("PlayerId")
                        .HasColumnType("bigint")
                        .HasColumnName("player_id");

                    b.Property<NpgsqlPoint>("Pos")
                        .HasColumnType("point")
                        .HasColumnName("pos");

                    b.Property<long?>("SessionId")
                        .HasColumnType("bigint")
                        .HasColumnName("session_id");

                    b.Property<int>("Side")
                        .HasColumnType("integer")
                        .HasColumnName("side");

                    b.Property<string>("VehicleName")
                        .HasColumnType("text")
                        .HasColumnName("vehicle_name");

                    b.Property<int>("VehicleType")
                        .HasColumnType("integer")
                        .HasColumnName("vehicle_type");

                    b.Property<long>("Velocity")
                        .HasColumnType("bigint")
                        .HasColumnName("velocity");

                    b.HasKey("PositionTrackerId")
                        .HasName("pk_gameserver_playerpositions");

                    b.HasIndex("PlayerId")
                        .HasDatabaseName("ix_gameserver_playerpositions_player_id");

                    b.HasIndex("SessionId")
                        .HasDatabaseName("ix_gameserver_playerpositions_session_id");

                    b.ToTable("gameserver_playerpositions");
                });

            modelBuilder.Entity("TF47_Backend.Database.Models.Services.Group", b =>
                {
                    b.Property<long>("GroupId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("group_id")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("BackgroundColor")
                        .HasColumnType("text")
                        .HasColumnName("background_color");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("boolean")
                        .HasColumnName("is_visible");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("TextColor")
                        .HasColumnType("text")
                        .HasColumnName("text_color");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("GroupId")
                        .HasName("pk_service_groups");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_service_groups_user_id");

                    b.ToTable("service_groups");
                });

            modelBuilder.Entity("TF47_Backend.Database.Models.Services.GroupPermission", b =>
                {
                    b.Property<long>("GroupPermissionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("group_permission_id")
                        .UseIdentityByDefaultColumn();

                    b.Property<bool>("CanBanUsers")
                        .HasColumnType("boolean")
                        .HasColumnName("can_ban_users");

                    b.Property<bool>("CanCreateServers")
                        .HasColumnType("boolean")
                        .HasColumnName("can_create_servers");

                    b.Property<bool>("CanDeleteUsers")
                        .HasColumnType("boolean")
                        .HasColumnName("can_delete_users");

                    b.Property<bool>("CanEditGroups")
                        .HasColumnType("boolean")
                        .HasColumnName("can_edit_groups");

                    b.Property<bool>("CanEditServers")
                        .HasColumnType("boolean")
                        .HasColumnName("can_edit_servers");

                    b.Property<bool>("CanEditUsers")
                        .HasColumnType("boolean")
                        .HasColumnName("can_edit_users");

                    b.Property<bool>("CanPermaBan")
                        .HasColumnType("boolean")
                        .HasColumnName("can_perma_ban");

                    b.Property<bool>("CanUseServers")
                        .HasColumnType("boolean")
                        .HasColumnName("can_use_servers");

                    b.Property<long>("GroupId")
                        .HasColumnType("bigint")
                        .HasColumnName("group_id");

                    b.HasKey("GroupPermissionId")
                        .HasName("pk_service_grouppermissions");

                    b.HasIndex("GroupId")
                        .IsUnique()
                        .HasDatabaseName("ix_service_grouppermissions_group_id");

                    b.ToTable("service_grouppermissions");
                });

            modelBuilder.Entity("TF47_Backend.Database.Models.Services.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<bool>("Banned")
                        .HasColumnType("boolean")
                        .HasColumnName("banned");

                    b.Property<DateTime>("FirstTimeSeen")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("first_time_seen");

                    b.Property<bool>("IsConnectedSteam")
                        .HasColumnType("boolean")
                        .HasColumnName("is_connected_steam");

                    b.Property<DateTime>("LastTimeSeen")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("last_time_seen");

                    b.Property<string>("Mail")
                        .HasColumnType("text")
                        .HasColumnName("mail");

                    b.Property<string>("Password")
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<string>("ProfileBackground")
                        .HasColumnType("text")
                        .HasColumnName("profile_background");

                    b.Property<string>("ProfilePicture")
                        .HasColumnType("text")
                        .HasColumnName("profile_picture");

                    b.Property<string>("SteamId")
                        .HasColumnType("text")
                        .HasColumnName("steam_id");

                    b.Property<string>("Username")
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.HasKey("UserId")
                        .HasName("pk_service_players");

                    b.ToTable("service_players");
                });

            modelBuilder.Entity("TF47_Backend.Database.Models.Session", b =>
                {
                    b.Property<long>("SessionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("session_id")
                        .UseIdentityByDefaultColumn();

                    b.Property<long?>("MissionId")
                        .HasColumnType("bigint")
                        .HasColumnName("mission_id");

                    b.Property<int>("MissionType")
                        .HasColumnType("integer")
                        .HasColumnName("mission_type");

                    b.Property<DateTime>("SessionCreated")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("session_created");

                    b.Property<DateTime?>("SessionEnded")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("session_ended");

                    b.Property<string>("WorldName")
                        .HasColumnType("text")
                        .HasColumnName("world_name");

                    b.HasKey("SessionId")
                        .HasName("pk_gameserver_sessions");

                    b.HasIndex("MissionId")
                        .HasDatabaseName("ix_gameserver_sessions_mission_id");

                    b.ToTable("gameserver_sessions");
                });

            modelBuilder.Entity("TF47_Backend.Database.Models.Whitelist", b =>
                {
                    b.Property<long>("WhitelistId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("whitelist_id")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("WhitelistId")
                        .HasName("pk_gameserver_whitelists");

                    b.ToTable("gameserver_whitelists");
                });

            modelBuilder.Entity("TF47_Backend.Database.Models.Whitelisting", b =>
                {
                    b.Property<long>("WhitelistingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("whitelisting_id")
                        .UseIdentityByDefaultColumn();

                    b.Property<long?>("PlayerId")
                        .HasColumnType("bigint")
                        .HasColumnName("player_id");

                    b.Property<long?>("WhitelistId")
                        .HasColumnType("bigint")
                        .HasColumnName("whitelist_id");

                    b.HasKey("WhitelistingId")
                        .HasName("pk_gameserver_whitelistings");

                    b.HasIndex("PlayerId")
                        .HasDatabaseName("ix_gameserver_whitelistings_player_id");

                    b.HasIndex("WhitelistId")
                        .HasDatabaseName("ix_gameserver_whitelistings_whitelist_id");

                    b.ToTable("gameserver_whitelistings");
                });

            modelBuilder.Entity("TF47_Backend.Database.Models.Chat", b =>
                {
                    b.HasOne("TF47_Backend.Database.Models.Player", "Player")
                        .WithMany("PlayerChats")
                        .HasForeignKey("PlayerId")
                        .HasConstraintName("fk_gameserver_chats_gameserver_players_player_id");

                    b.HasOne("TF47_Backend.Database.Models.Session", "Session")
                        .WithMany("Chats")
                        .HasForeignKey("SessionId")
                        .HasConstraintName("fk_gameserver_chats_gameserver_sessions_session_id");

                    b.Navigation("Player");

                    b.Navigation("Session");
                });

            modelBuilder.Entity("TF47_Backend.Database.Models.Kill", b =>
                {
                    b.HasOne("TF47_Backend.Database.Models.Player", "Killer")
                        .WithMany("PlayerKills")
                        .HasForeignKey("KillerPlayerId")
                        .HasConstraintName("fk_gameserver_kills_gameserver_players_killer_player_id");

                    b.HasOne("TF47_Backend.Database.Models.Session", "Session")
                        .WithMany("Kills")
                        .HasForeignKey("SessionId")
                        .HasConstraintName("fk_gameserver_kills_gameserver_sessions_session_id");

                    b.HasOne("TF47_Backend.Database.Models.Player", "Victim")
                        .WithMany("PlayerDeaths")
                        .HasForeignKey("VictimPlayerId")
                        .HasConstraintName("fk_gameserver_kills_gameserver_players_victim_player_id");

                    b.Navigation("Killer");

                    b.Navigation("Session");

                    b.Navigation("Victim");
                });

            modelBuilder.Entity("TF47_Backend.Database.Models.Mission", b =>
                {
                    b.HasOne("TF47_Backend.Database.Models.Campaign", "Campaign")
                        .WithMany("Missions")
                        .HasForeignKey("CampaignId")
                        .HasConstraintName("fk_gameserver_missions_gameserver_campaigns_campaign_id");

                    b.Navigation("Campaign");
                });

            modelBuilder.Entity("TF47_Backend.Database.Models.Playtime", b =>
                {
                    b.HasOne("TF47_Backend.Database.Models.Player", "Player")
                        .WithOne("PlayerPlaytime")
                        .HasForeignKey("TF47_Backend.Database.Models.Playtime", "PlayerId")
                        .HasConstraintName("fk_gameserver_playtimes_gameserver_players_player_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TF47_Backend.Database.Models.Session", "Session")
                        .WithMany("PlayTimes")
                        .HasForeignKey("SessionId")
                        .HasConstraintName("fk_gameserver_playtimes_gameserver_sessions_session_id");

                    b.Navigation("Player");

                    b.Navigation("Session");
                });

            modelBuilder.Entity("TF47_Backend.Database.Models.Position", b =>
                {
                    b.HasOne("TF47_Backend.Database.Models.Player", "Player")
                        .WithMany("PlayerPositions")
                        .HasForeignKey("PlayerId")
                        .HasConstraintName("fk_gameserver_playerpositions_gameserver_players_player_id");

                    b.HasOne("TF47_Backend.Database.Models.Session", "Session")
                        .WithMany()
                        .HasForeignKey("SessionId")
                        .HasConstraintName("fk_gameserver_playerpositions_gameserver_sessions_session_id");

                    b.Navigation("Player");

                    b.Navigation("Session");
                });

            modelBuilder.Entity("TF47_Backend.Database.Models.Services.Group", b =>
                {
                    b.HasOne("TF47_Backend.Database.Models.Services.User", null)
                        .WithMany("Groups")
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_service_groups_service_players_user_id");
                });

            modelBuilder.Entity("TF47_Backend.Database.Models.Services.GroupPermission", b =>
                {
                    b.HasOne("TF47_Backend.Database.Models.Services.Group", "Group")
                        .WithOne("GroupPermission")
                        .HasForeignKey("TF47_Backend.Database.Models.Services.GroupPermission", "GroupId")
                        .HasConstraintName("fk_service_grouppermissions_service_groups_group_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");
                });

            modelBuilder.Entity("TF47_Backend.Database.Models.Session", b =>
                {
                    b.HasOne("TF47_Backend.Database.Models.Mission", "Mission")
                        .WithMany("Sessions")
                        .HasForeignKey("MissionId")
                        .HasConstraintName("fk_gameserver_sessions_missions_mission_id");

                    b.Navigation("Mission");
                });

            modelBuilder.Entity("TF47_Backend.Database.Models.Whitelisting", b =>
                {
                    b.HasOne("TF47_Backend.Database.Models.Player", "Player")
                        .WithMany("PlayerWhitelistings")
                        .HasForeignKey("PlayerId")
                        .HasConstraintName("fk_gameserver_whitelistings_gameserver_players_player_id");

                    b.HasOne("TF47_Backend.Database.Models.Whitelist", "Whitelist")
                        .WithMany()
                        .HasForeignKey("WhitelistId")
                        .HasConstraintName("fk_gameserver_whitelistings_gameserver_whitelists_whitelist_id");

                    b.Navigation("Player");

                    b.Navigation("Whitelist");
                });

            modelBuilder.Entity("TF47_Backend.Database.Models.Campaign", b =>
                {
                    b.Navigation("Missions");
                });

            modelBuilder.Entity("TF47_Backend.Database.Models.Mission", b =>
                {
                    b.Navigation("Sessions");
                });

            modelBuilder.Entity("TF47_Backend.Database.Models.Player", b =>
                {
                    b.Navigation("PlayerChats");

                    b.Navigation("PlayerDeaths");

                    b.Navigation("PlayerKills");

                    b.Navigation("PlayerPlaytime");

                    b.Navigation("PlayerPositions");

                    b.Navigation("PlayerWhitelistings");
                });

            modelBuilder.Entity("TF47_Backend.Database.Models.Services.Group", b =>
                {
                    b.Navigation("GroupPermission");
                });

            modelBuilder.Entity("TF47_Backend.Database.Models.Services.User", b =>
                {
                    b.Navigation("Groups");
                });

            modelBuilder.Entity("TF47_Backend.Database.Models.Session", b =>
                {
                    b.Navigation("Chats");

                    b.Navigation("Kills");

                    b.Navigation("PlayTimes");
                });
#pragma warning restore 612, 618
        }
    }
}