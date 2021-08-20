using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TF47_API.Database.Models.GameServer
{
    [Table("GameServerConfig")]
    public class ServerConfiguration
    {
        [Key]
        public uint ServerConfigId { get; set; }


        //Global Settings
        public string HostName { get; set; }
        public string ServerPassword { get; set; }
        public string AdminPassword { get; set; }
        public string CommandPassword { get; set; }
        public string[] MotdMessages { get; set; }
        public uint MotdInterval { get; set; }
        public string[] AdminSteamUIDs { get; set; } //only this steamIds are allowed to login
        public uint SteamMaxProtocolSize { get; set; } //this is enables the server to transmit all the mod information
        
        //Connection Settings
        public uint MaxPlayers { get; set; }
        public bool KickDuplicate { get; set; }
        public bool VerifySignatures { get; set; }
        public uint DisconnectTimeout { get; set; }
        public uint MaxDeync { get; set; }
        public uint MaxPing { get; set; }
        public uint MaxPacketLoss { get; set; }
        public bool EnablePlayerDiag { get; set; }
        public bool AllowFilePatching { get; set; } //TODO: maybe make this one an enum

        //In-Game settings
        public bool EnableBattleye { get; set; }
        public bool AllowMapDrawing { get; set; }
        public bool Persistence { get; set; }
        public AdvancedFlightModelSetting AdvancedFlightModel { get; set; }
        public uint VoteThreshold { get; set; }
        public uint VoteMissionPlayers { get; set; }
        public bool DisableVon { get; set; }
        public VonCodecSetting AudioCodecSetting { get; set; }
        public uint VonQuality { get; set; }

        //Mission settings
        public bool ForceDifficulty { get; set; }
        public DifficultySetting DifficultySetting { get; set; }
        public bool ReducedDamage { get; set; }
        public NeverDistanceOrFadeoutSetting GroupIndicators { get; set; }
        public NeverDistanceOrFadeoutSetting FriendlyTags { get; set; }
        public NeverDistanceOrFadeoutSetting EnemyTags { get; set;}
        public NeverDistanceOrFadeoutSetting DetectMines { get; set; }
        public NeverFadeOutAlwaysSetting Commands { get; set; }
        public NeverFadeOutAlwaysSetting Waypoints { get; set; }
        public NeverFadeOutAlwaysSetting WeaponInfo { get; set; }
        public NeverFadeOutAlwaysSetting StanceIndicator { get; set; }
        public bool TacticalPing { get; set; }
        public bool StaminaBar { get; set;}
        public bool WeaponCrosshair { get; set; }
        public bool VisionAid { get; set; }
        public bool ThirdPersonView { get; set; }
        public bool CameraShake { get; set; }
        public bool ScoreTable { get; set; }
        public bool DeathMessages { get; set; }
        public bool ShowVonId { get; set; }
        public bool ShowMapContent { get; set; }
        public bool AutoReport { get; set; }
        public bool MultipleSaves { get; set; }
        public float AiSkill { get; set; }
        public float AiPrecision { get; set; }

        //basic.cfg settings
        public uint MaxMessageSend { get; set;}
        public uint MaxSizeGuaranteed { get; set; }
        public uint MaxSizeNonGuaranteed { get; set; }
        
        public uint MinBandwidth { get; set; } //here in mbit/s! It will be converted to bp/s
        public uint MaxBandwidth { get; set; } //here in mbit/s! It will be converted to bp/s
        public double MinErrorToSend { get; set; }
        public double MinErrorToSendNear { get; set; }
        public uint MaxCustomFileSize { get; set; }
        public ulong SocketMaxPacketSize { get; set; }
        public ulong SocketInitBandwidth { get; set; }
        public ulong SocketMaxBandwidth { get; set; } //here in mbit/s! It will be converted to bp/s
        public uint TerrainGrid { get; set; }
        public uint ViewDistance { get; set; }

        //parameters 
        public bool EnableHyperthreading { get; set; }
        public uint Port { get; set; }

        public ICollection<Server> Servers { get; set; }
       
    }
}