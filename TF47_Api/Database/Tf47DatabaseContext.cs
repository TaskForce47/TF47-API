using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TF47_Api.Database
{
    public partial class Tf47DatabaseContext : DbContext
    {
        public Tf47DatabaseContext()
        {
        }

        public Tf47DatabaseContext(DbContextOptions<Tf47DatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Tf47GadgetActionLog> Tf47GadgetActionLog { get; set; }
        public virtual DbSet<Tf47GadgetSquad> Tf47GadgetSquad { get; set; }
        public virtual DbSet<Tf47GadgetSquadUser> Tf47GadgetSquadUser { get; set; }
        public virtual DbSet<Tf47GadgetTicket> Tf47GadgetTicket { get; set; }
        public virtual DbSet<Tf47GadgetTicketCategory> Tf47GadgetTicketCategory { get; set; }
        public virtual DbSet<Tf47GadgetTicketMessage> Tf47GadgetTicketMessage { get; set; }
        public virtual DbSet<Tf47GadgetUser> Tf47GadgetUser { get; set; }
        public virtual DbSet<Tf47GadgetUserNotes> Tf47GadgetUserNotes { get; set; }
        public virtual DbSet<Tf47GadgetWhitelistMessages> Tf47GadgetWhitelistMessages { get; set; }
        public virtual DbSet<Tf47GadgetWhitelistRequests> Tf47GadgetWhitelistRequests { get; set; }
        public virtual DbSet<Tf47ServerChatLog> Tf47ServerChatLog { get; set; }
        public virtual DbSet<Tf47ServerEventLog> Tf47ServerEventLog { get; set; }
        public virtual DbSet<Tf47ServerEventTypes> Tf47ServerEventTypes { get; set; }
        public virtual DbSet<Tf47ServerKillLog> Tf47ServerKillLog { get; set; }
        public virtual DbSet<Tf47ServerMissions> Tf47ServerMissions { get; set; }
        public virtual DbSet<Tf47ServerPerformance> Tf47ServerPerformance { get; set; }
        public virtual DbSet<Tf47ServerPerformanceHc> Tf47ServerPerformanceHc { get; set; }
        public virtual DbSet<Tf47ServerPerformancePlayer> Tf47ServerPerformancePlayer { get; set; }
        public virtual DbSet<Tf47ServerPlayerStats> Tf47ServerPlayerStats { get; set; }
        public virtual DbSet<Tf47ServerPlayerStatsCreatedOnce> Tf47ServerPlayerStatsCreatedOnce { get; set; }
        public virtual DbSet<Tf47ServerPlayerWhitelisting> Tf47ServerPlayerWhitelisting { get; set; }
        public virtual DbSet<Tf47ServerPlayers> Tf47ServerPlayers { get; set; }
        public virtual DbSet<Tf47ServerPositionTracking> Tf47ServerPositionTracking { get; set; }
        public virtual DbSet<Tf47ServerSessions> Tf47ServerSessions { get; set; }
        public virtual DbSet<Tf47ServerTicket> Tf47ServerTicket { get; set; }
        public virtual DbSet<Tf47ServerTicketLog> Tf47ServerTicketLog { get; set; }
        public virtual DbSet<Tf47ServerWhitelists> Tf47ServerWhitelists { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tf47GadgetActionLog>(entity =>
            {
                entity.ToTable("tf47_gadget_action_log");

                entity.HasIndex(e => e.UserId)
                    .HasName("user_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Action)
                    .HasColumnName("action")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("'unknown'")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.ActionPerformed)
                    .HasColumnName("action_performed")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Tf47GadgetActionLog)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("tf47_gadget_action_log_ibfk_1");
            });

            modelBuilder.Entity<Tf47GadgetSquad>(entity =>
            {
                entity.ToTable("tf47_gadget_squad");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.SquadEmail)
                    .HasColumnName("squad_email")
                    .HasColumnType("varchar(64)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.SquadName)
                    .HasColumnName("squad_name")
                    .HasColumnType("varchar(64)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.SquadNick)
                    .HasColumnName("squad_nick")
                    .HasColumnType("varchar(64)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.SquadHasPicture)
                    .HasColumnName("squad_has_picture");

                entity.Property(e => e.SquadTitle)
                    .HasColumnName("squad_title")
                    .HasColumnType("varchar(64)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.SquadWeb)
                    .HasColumnName("squad_web")
                    .HasColumnType("varchar(64)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Tf47GadgetSquadUser>(entity =>
            {
                entity.ToTable("tf47_gadget_squad_user");

                entity.HasIndex(e => e.SquadId)
                    .HasName("squad_id");

                entity.HasIndex(e => e.UserId)
                    .HasName("user_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.SquadId).HasColumnName("squad_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.UserSquadEmail)
                    .HasColumnName("user_squad_email")
                    .HasColumnType("varchar(64)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UserSquadIcq)
                    .HasColumnName("user_squad_icq")
                    .HasColumnType("varchar(64)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UserSquadName)
                    .HasColumnName("user_squad_name")
                    .HasColumnType("varchar(64)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UserSquadNick)
                    .HasColumnName("user_squad_nick")
                    .HasColumnType("varchar(64)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UserSquadRemark)
                    .HasColumnName("user_squad_remark")
                    .HasColumnType("varchar(128)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.Squad)
                    .WithMany(p => p.Tf47GadgetSquadUser)
                    .HasForeignKey(d => d.SquadId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("tf47_gadget_squad_user_ibfk_1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Tf47GadgetSquadUser)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("tf47_gadget_squad_user_ibfk_2");
            });

            modelBuilder.Entity<Tf47GadgetTicket>(entity =>
            {
                entity.ToTable("tf47_gadget_ticket");

                entity.HasIndex(e => e.CategoryId)
                    .HasName("category_id");

                entity.HasIndex(e => e.TicketIssuer)
                    .HasName("ticket_issuer");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.TicketDateCreated)
                    .HasColumnName("ticket_date_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.TicketDescription)
                    .HasColumnName("ticket_description")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.TicketIsConfidential).HasColumnName("ticket_is_confidential");

                entity.Property(e => e.TicketIssuer).HasColumnName("ticket_issuer");

                entity.Property(e => e.TicketStatus)
                    .IsRequired()
                    .HasColumnName("ticket_status")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("'NEW'")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Tf47GadgetTicket)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("tf47_gadget_ticket_ibfk_1");

                entity.HasOne(d => d.TicketIssuerNavigation)
                    .WithMany(p => p.Tf47GadgetTicket)
                    .HasForeignKey(d => d.TicketIssuer)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tf47_gadget_ticket_ibfk_2");
            });

            modelBuilder.Entity<Tf47GadgetTicketCategory>(entity =>
            {
                entity.ToTable("tf47_gadget_ticket_category");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CategoryDescription)
                    .HasColumnName("category_description")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasColumnName("category_name")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Tf47GadgetTicketMessage>(entity =>
            {
                entity.ToTable("tf47_gadget_ticket_message");

                entity.HasIndex(e => e.TicketId)
                    .HasName("ticket_id");

                entity.HasIndex(e => e.UserId)
                    .HasName("user_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Message)
                    .HasColumnName("message")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.TicketId).HasColumnName("ticket_id");

                entity.Property(e => e.TimeSend)
                    .HasColumnName("time_send")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Ticket)
                    .WithMany(p => p.Tf47GadgetTicketMessage)
                    .HasForeignKey(d => d.TicketId)
                    .HasConstraintName("tf47_gadget_ticket_message_ibfk_1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Tf47GadgetTicketMessage)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tf47_gadget_ticket_message_ibfk_2");
            });

            modelBuilder.Entity<Tf47GadgetUser>(entity =>
            {
                entity.ToTable("tf47_gadget_user");

                entity.HasIndex(e => e.ForumId)
                    .HasName("forum_id")
                    .IsUnique();

                entity.HasIndex(e => e.PlayerUid)
                    .HasName("player_uid");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ForumAvatarPath)
                    .HasColumnName("forum_avatar_path")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.ForumId).HasColumnName("forum_id");

                entity.Property(e => e.ForumIsAdmin).HasColumnName("forum_is_admin");

                entity.Property(e => e.ForumIsModerator).HasColumnName("forum_is_moderator");

                entity.Property(e => e.ForumIsSponsor).HasColumnName("forum_is_sponsor");

                entity.Property(e => e.ForumIsTf).HasColumnName("forum_is_tf");

                entity.Property(e => e.ForumLastLogin)
                    .HasColumnName("forum_last_login")
                    .HasColumnType("datetime");

                entity.Property(e => e.ForumMail)
                    .HasColumnName("forum_mail")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.ForumName)
                    .HasColumnName("forum_name")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.PlayerUid)
                    .HasColumnName("player_uid")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.PlayerU)
                    .WithMany(p => p.Tf47GadgetUser)
                    .HasPrincipalKey(p => p.PlayerUid)
                    .HasForeignKey(d => d.PlayerUid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("tf47_gadget_user_ibfk_1");
            });

            modelBuilder.Entity<Tf47GadgetUserNotes>(entity =>
            {
                entity.ToTable("tf47_gadget_user_notes");

                entity.HasIndex(e => e.AuthorId)
                    .HasName("author_id");

                entity.HasIndex(e => e.PlayerId)
                    .HasName("player_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AuthorId).HasColumnName("author_id");

                entity.Property(e => e.PlayerId).HasColumnName("player_id");

                entity.Property(e => e.PlayerNote)
                    .IsRequired()
                    .HasColumnName("player_note")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.TimeWritten)
                    .HasColumnName("time_written")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("'Info'")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.IsModified)
                    .HasColumnName("modified");

                entity.Property(e => e.LastTimeModified)
                    .HasColumnName("modified_timestamp")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Tf47GadgetUserNotes)
                    .HasForeignKey(d => d.AuthorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tf47_gadget_user_notes_ibfk_2");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.Tf47GadgetUserNotes)
                    .HasForeignKey(d => d.PlayerId)
                    .HasConstraintName("tf47_gadget_user_notes_ibfk_1");
            });

            modelBuilder.Entity<Tf47GadgetWhitelistMessages>(entity =>
            {
                entity.ToTable("tf47_gadget_whitelist_messages");

                entity.HasIndex(e => e.MessageAuthor)
                    .HasName("message_author");

                entity.HasIndex(e => e.RequestId)
                    .HasName("request_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Message)
                    .HasColumnName("message")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.MessageAuthor).HasColumnName("message_author");

                entity.Property(e => e.RequestId).HasColumnName("request_id");

                entity.Property(e => e.TimeOfMessage)
                    .HasColumnName("time_of_message")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.MessageAuthorNavigation)
                    .WithMany(p => p.Tf47GadgetWhitelistMessages)
                    .HasForeignKey(d => d.MessageAuthor)
                    .HasConstraintName("tf47_gadget_whitelist_messages_ibfk_2");

                entity.HasOne(d => d.Request)
                    .WithMany(p => p.Tf47GadgetWhitelistMessages)
                    .HasForeignKey(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tf47_gadget_whitelist_messages_ibfk_1");
            });

            modelBuilder.Entity<Tf47GadgetWhitelistRequests>(entity =>
            {
                entity.ToTable("tf47_gadget_whitelist_requests");

                entity.HasIndex(e => e.RequestAcceptorId)
                    .HasName("request_acceptor_id");

                entity.HasIndex(e => e.UserId)
                    .HasName("user_id");

                entity.HasIndex(e => e.WhitelistId)
                    .HasName("whitelist_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.RequestAcceptorId).HasColumnName("request_acceptor_id");

                entity.Property(e => e.RequestStatus)
                    .HasColumnName("request_status")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("'open'")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.RequestTime)
                    .HasColumnName("request_time")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.WhitelistId).HasColumnName("whitelist_id");

                entity.HasOne(d => d.RequestAcceptor)
                    .WithMany(p => p.Tf47GadgetWhitelistRequestsRequestAcceptor)
                    .HasForeignKey(d => d.RequestAcceptorId)
                    .HasConstraintName("tf47_gadget_whitelist_requests_ibfk_2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Tf47GadgetWhitelistRequestsUser)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("tf47_gadget_whitelist_requests_ibfk_1");

                entity.HasOne(d => d.Whitelist)
                    .WithMany(p => p.Tf47GadgetWhitelistRequests)
                    .HasForeignKey(d => d.WhitelistId)
                    .HasConstraintName("tf47_gadget_whitelist_requests_ibfk_3");
            });

            modelBuilder.Entity<Tf47ServerChatLog>(entity =>
            {
                entity.ToTable("tf47_server_chat_log");

                entity.HasIndex(e => e.PlayerId)
                    .HasName("player_id");

                entity.HasIndex(e => e.SessionId)
                    .HasName("session_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Channel)
                    .IsRequired()
                    .HasColumnName("channel")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("'side'")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasColumnName("message")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.PlayerId).HasColumnName("player_id");

                entity.Property(e => e.SessionId).HasColumnName("session_id");

                entity.Property(e => e.TimeSend)
                    .HasColumnName("time_send")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.Tf47ServerChatLog)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tf47_server_chat_log_ibfk_2");

                entity.HasOne(d => d.Session)
                    .WithMany(p => p.Tf47ServerChatLog)
                    .HasForeignKey(d => d.SessionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tf47_server_chat_log_ibfk_1");
            });

            modelBuilder.Entity<Tf47ServerEventLog>(entity =>
            {
                entity.ToTable("tf47_server_event_log");

                entity.HasIndex(e => e.EventId)
                    .HasName("event_id");

                entity.HasIndex(e => e.SenderId)
                    .HasName("sender_id");

                entity.HasIndex(e => e.SessionId)
                    .HasName("session_id");

                entity.HasIndex(e => e.TargetId)
                    .HasName("target_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EventId).HasColumnName("event_id");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasColumnName("message")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.SenderId).HasColumnName("sender_id");

                entity.Property(e => e.SessionId).HasColumnName("session_id");

                entity.Property(e => e.TargetId).HasColumnName("target_id");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Tf47ServerEventLog)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tf47_server_event_log_ibfk_4");

                entity.HasOne(d => d.Sender)
                    .WithMany(p => p.Tf47ServerEventLogSender)
                    .HasForeignKey(d => d.SenderId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("tf47_server_event_log_ibfk_1");

                entity.HasOne(d => d.Session)
                    .WithMany(p => p.Tf47ServerEventLog)
                    .HasForeignKey(d => d.SessionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tf47_server_event_log_ibfk_3");

                entity.HasOne(d => d.Target)
                    .WithMany(p => p.Tf47ServerEventLogTarget)
                    .HasForeignKey(d => d.TargetId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("tf47_server_event_log_ibfk_2");
            });

            modelBuilder.Entity<Tf47ServerEventTypes>(entity =>
            {
                entity.ToTable("tf47_server_event_types");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Tf47ServerKillLog>(entity =>
            {
                entity.ToTable("tf47_server_kill_log");

                entity.HasIndex(e => e.KillerId)
                    .HasName("killer_id");

                entity.HasIndex(e => e.SessionId)
                    .HasName("session_id");

                entity.HasIndex(e => e.VictimId)
                    .HasName("victim_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Distance).HasColumnName("distance");

                entity.Property(e => e.KillerId).HasColumnName("killer_id");

                entity.Property(e => e.SessionId).HasColumnName("session_id");

                entity.Property(e => e.Vehicle)
                    .HasColumnName("vehicle")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.VictimId).HasColumnName("victim_id");

                entity.Property(e => e.WeaponName)
                    .IsRequired()
                    .HasColumnName("weapon_name")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.Killer)
                    .WithMany(p => p.Tf47ServerKillLogKiller)
                    .HasForeignKey(d => d.KillerId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("tf47_server_kill_log_ibfk_1");

                entity.HasOne(d => d.Session)
                    .WithMany(p => p.Tf47ServerKillLog)
                    .HasForeignKey(d => d.SessionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tf47_server_kill_log_ibfk_3");

                entity.HasOne(d => d.Victim)
                    .WithMany(p => p.Tf47ServerKillLogVictim)
                    .HasForeignKey(d => d.VictimId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("tf47_server_kill_log_ibfk_2");
            });

            modelBuilder.Entity<Tf47ServerMissions>(entity =>
            {
                entity.ToTable("tf47_server_missions");

                entity.HasIndex(e => e.MissionName)
                    .HasName("mission_name")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.MissionName)
                    .IsRequired()
                    .HasColumnName("mission_name")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.MissionType)
                    .IsRequired()
                    .HasColumnName("mission_type")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("'COOP'")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Tf47ServerPerformance>(entity =>
            {
                entity.ToTable("tf47_server_performance");

                entity.HasIndex(e => e.SessionId)
                    .HasName("session_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Fps).HasColumnName("fps");

                entity.Property(e => e.ObjCount).HasColumnName("obj_count");

                entity.Property(e => e.RunningFsm).HasColumnName("running_fsm");

                entity.Property(e => e.RunningSqfExecvm).HasColumnName("running_sqf_execvm");

                entity.Property(e => e.RunningSqfSpawned).HasColumnName("running_sqf_spawned");

                entity.Property(e => e.ServerTickTime).HasColumnName("server_tick_time");

                entity.Property(e => e.SessionId).HasColumnName("session_id");

                entity.Property(e => e.UnitCount).HasColumnName("unit_count");

                entity.HasOne(d => d.Session)
                    .WithMany(p => p.Tf47ServerPerformance)
                    .HasForeignKey(d => d.SessionId)
                    .HasConstraintName("tf47_server_performance_ibfk_1");
            });

            modelBuilder.Entity<Tf47ServerPerformanceHc>(entity =>
            {
                entity.ToTable("tf47_server_performance_hc");

                entity.HasIndex(e => e.SessionId)
                    .HasName("session_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Fps).HasColumnName("fps");

                entity.Property(e => e.HcId).HasColumnName("hc_id");

                entity.Property(e => e.ObjCount).HasColumnName("obj_count");

                entity.Property(e => e.RunningFsm).HasColumnName("running_fsm");

                entity.Property(e => e.RunningSqfExecvm).HasColumnName("running_sqf_execvm");

                entity.Property(e => e.RunningSqfSpawned).HasColumnName("running_sqf_spawned");

                entity.Property(e => e.ServerTickTime).HasColumnName("server_tick_time");

                entity.Property(e => e.SessionId).HasColumnName("session_id");

                entity.Property(e => e.UnitCount).HasColumnName("unit_count");

                entity.HasOne(d => d.Session)
                    .WithMany(p => p.Tf47ServerPerformanceHc)
                    .HasForeignKey(d => d.SessionId)
                    .HasConstraintName("tf47_server_performance_hc_ibfk_1");
            });

            modelBuilder.Entity<Tf47ServerPerformancePlayer>(entity =>
            {
                entity.ToTable("tf47_server_performance_player");

                entity.HasIndex(e => e.PlayerId)
                    .HasName("player_id");

                entity.HasIndex(e => e.SessionId)
                    .HasName("session_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Fps).HasColumnName("fps");

                entity.Property(e => e.PlayerId).HasColumnName("player_id");

                entity.Property(e => e.RunningFsm).HasColumnName("running_fsm");

                entity.Property(e => e.RunningSqfExecvm).HasColumnName("running_sqf_execvm");

                entity.Property(e => e.RunningSqfSpawned).HasColumnName("running_sqf_spawned");

                entity.Property(e => e.ServerTickTime).HasColumnName("server_tick_time");

                entity.Property(e => e.SessionId).HasColumnName("session_id");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.Tf47ServerPerformancePlayer)
                    .HasForeignKey(d => d.PlayerId)
                    .HasConstraintName("tf47_server_performance_player_ibfk_1");

                entity.HasOne(d => d.Session)
                    .WithMany(p => p.Tf47ServerPerformancePlayer)
                    .HasForeignKey(d => d.SessionId)
                    .HasConstraintName("tf47_server_performance_player_ibfk_2");
            });

            modelBuilder.Entity<Tf47ServerPlayerStats>(entity =>
            {
                entity.HasKey(e => e.PlayerId)
                    .HasName("PRIMARY");

                entity.ToTable("tf47_server_player_stats");

                entity.Property(e => e.PlayerId).HasColumnName("player_id");

                entity.Property(e => e.DeathsInf)
                    .HasColumnName("deaths_inf")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.DeathsVehHelo)
                    .HasColumnName("deaths_veh_helo")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.DeathsVehPlane)
                    .HasColumnName("deaths_veh_plane")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.DeathsVehSmall)
                    .HasColumnName("deaths_veh_small")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.DeathsVehTracked)
                    .HasColumnName("deaths_veh_tracked")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.DistanceInf)
                    .HasColumnName("distance_inf")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.DistanceVehHelo)
                    .HasColumnName("distance_veh_helo")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.DistanceVehPlane)
                    .HasColumnName("distance_veh_plane")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.DistanceVehSmall)
                    .HasColumnName("distance_veh_small")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.DistanceVehTracked)
                    .HasColumnName("distance_veh_tracked")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.KillsInf)
                    .HasColumnName("kills_inf")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.KillsVehHelo)
                    .HasColumnName("kills_veh_helo")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.KillsVehPlane)
                    .HasColumnName("kills_veh_plane")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.KillsVehSmall)
                    .HasColumnName("kills_veh_small")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.KillsVehTracked)
                    .HasColumnName("kills_veh_tracked")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.LastTimeSeen)
                    .HasColumnName("last_time_seen")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.NumberConnections)
                    .HasColumnName("number_connections")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.TimePlayedBase)
                    .HasColumnName("time_played_base")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.TimePlayedInf)
                    .HasColumnName("time_played_inf")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.TimePlayedObjective)
                    .HasColumnName("time_played_objective")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.TimePlayedVehHelo)
                    .HasColumnName("time_played_veh_helo")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.TimePlayedVehPlane)
                    .HasColumnName("time_played_veh_plane")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.TimePlayedVehSmall)
                    .HasColumnName("time_played_veh_small")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.TimePlayedVehTracked)
                    .HasColumnName("time_played_veh_tracked")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.Player)
                    .WithOne(p => p.Tf47ServerPlayerStats)
                    .HasForeignKey<Tf47ServerPlayerStats>(d => d.PlayerId)
                    .HasConstraintName("tf47_server_player_stats_ibfk_1");
            });

            modelBuilder.Entity<Tf47ServerPlayerStatsCreatedOnce>(entity =>
            {
                entity.HasKey(e => e.PlayerId)
                    .HasName("PRIMARY");

                entity.ToTable("tf47_server_player_stats_created_once");

                entity.Property(e => e.PlayerId).HasColumnName("player_id");

                entity.Property(e => e.FirstConnectionTime)
                    .HasColumnName("first_connection_time")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.PlayerNameConnected)
                    .HasColumnName("player_name_connected")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("'John Doe'")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.Player)
                    .WithOne(p => p.Tf47ServerPlayerStatsCreatedOnce)
                    .HasForeignKey<Tf47ServerPlayerStatsCreatedOnce>(d => d.PlayerId)
                    .HasConstraintName("tf47_server_player_stats_created_once_ibfk_1");
            });

            modelBuilder.Entity<Tf47ServerPlayerWhitelisting>(entity =>
            {
                entity.ToTable("tf47_server_player_whitelisting");

                entity.HasIndex(e => e.PlayerId)
                    .HasName("player_id");

                entity.HasIndex(e => e.WhitelistId)
                    .HasName("whitelist_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PlayerId).HasColumnName("player_id");

                entity.Property(e => e.WhitelistId).HasColumnName("whitelist_id");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.Tf47ServerPlayerWhitelisting)
                    .HasForeignKey(d => d.PlayerId)
                    .HasConstraintName("tf47_server_player_whitelisting_ibfk_1");

                entity.HasOne(d => d.Whitelist)
                    .WithMany(p => p.Tf47ServerPlayerWhitelisting)
                    .HasForeignKey(d => d.WhitelistId)
                    .HasConstraintName("tf47_server_player_whitelisting_ibfk_2");
            });

            modelBuilder.Entity<Tf47ServerPlayers>(entity =>
            {
                entity.ToTable("tf47_server_players");

                entity.HasIndex(e => e.PlayerUid)
                    .HasName("player_uid")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PlayerName)
                    .HasColumnName("player_name")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("'John Doe'")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.PlayerUid)
                    .IsRequired()
                    .HasColumnName("player_uid")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Tf47ServerPositionTracking>(entity =>
            {
                entity.ToTable("tf47_server_position_tracking");

                entity.HasIndex(e => e.PlayerId)
                    .HasName("player_id");

                entity.HasIndex(e => e.SessionId)
                    .HasName("session_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.LookingDir).HasColumnName("looking_dir");

                entity.Property(e => e.PlayerId).HasColumnName("player_id");

                entity.Property(e => e.PosX).HasColumnName("pos_x");

                entity.Property(e => e.PosY).HasColumnName("pos_y");

                entity.Property(e => e.PosZ).HasColumnName("pos_z");

                entity.Property(e => e.ServerTime).HasColumnName("server_time");

                entity.Property(e => e.SessionId).HasColumnName("session_id");

                entity.Property(e => e.TravelingMethod)
                    .IsRequired()
                    .HasColumnName("traveling_method")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.Tf47ServerPositionTracking)
                    .HasForeignKey(d => d.PlayerId)
                    .HasConstraintName("tf47_server_position_tracking_ibfk_2");

                entity.HasOne(d => d.Session)
                    .WithMany(p => p.Tf47ServerPositionTracking)
                    .HasForeignKey(d => d.SessionId)
                    .HasConstraintName("tf47_server_position_tracking_ibfk_1");
            });

            modelBuilder.Entity<Tf47ServerSessions>(entity =>
            {
                entity.ToTable("tf47_server_sessions");

                entity.HasIndex(e => e.MissionId)
                    .HasName("mission_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.MissionId).HasColumnName("mission_id");

                entity.Property(e => e.SessionStarted)
                    .HasColumnName("session_started")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.WorldName)
                    .HasColumnName("world_name")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("'unknown world'")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.Mission)
                    .WithMany(p => p.Tf47ServerSessions)
                    .HasForeignKey(d => d.MissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tf47_server_sessions_ibfk_1");
            });

            modelBuilder.Entity<Tf47ServerTicket>(entity =>
            {
                entity.HasKey(e => e.SessionId)
                    .HasName("PRIMARY");

                entity.ToTable("tf47_server_ticket");

                entity.Property(e => e.SessionId).HasColumnName("session_id");

                entity.Property(e => e.SessionFinished).HasColumnName("session_finished");

                entity.Property(e => e.TicketCount).HasColumnName("ticket_count");

                entity.HasOne(d => d.Session)
                    .WithOne(p => p.Tf47ServerTicket)
                    .HasForeignKey<Tf47ServerTicket>(d => d.SessionId)
                    .HasConstraintName("tf47_server_ticket_ibfk_1");
            });

            modelBuilder.Entity<Tf47ServerTicketLog>(entity =>
            {
                entity.ToTable("tf47_server_ticket_log");

                entity.HasIndex(e => e.SessionId)
                    .HasName("session_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Message)
                    .HasColumnName("message")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("'unknown'")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.SessionId).HasColumnName("session_id");

                entity.Property(e => e.TicketChange).HasColumnName("ticket_change");

                entity.Property(e => e.TicketChangeTime)
                    .HasColumnName("ticket_change_time")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.TicketNow).HasColumnName("ticket_now");

                entity.HasOne(d => d.Session)
                    .WithMany(p => p.Tf47ServerTicketLog)
                    .HasForeignKey(d => d.SessionId)
                    .HasConstraintName("tf47_server_ticket_log_ibfk_1");
            });

            modelBuilder.Entity<Tf47ServerWhitelists>(entity =>
            {
                entity.ToTable("tf47_server_whitelists");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
