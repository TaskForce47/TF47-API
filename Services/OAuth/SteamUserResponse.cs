using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TF47_Backend.Services.OAuth
{
    public class Player
    {
        [JsonPropertyName("steamid")]
        public string Steamid { get; set; }

        [JsonPropertyName("communityvisibilitystate")]
        public int Communityvisibilitystate { get; set; }

        [JsonPropertyName("profilestate")]
        public int Profilestate { get; set; }

        [JsonPropertyName("personaname")]
        public string Personaname { get; set; }

        [JsonPropertyName("profileurl")]
        public string Profileurl { get; set; }

        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }

        [JsonPropertyName("avatarmedium")]
        public string Avatarmedium { get; set; }

        [JsonPropertyName("avatarfull")]
        public string Avatarfull { get; set; }

        [JsonPropertyName("avatarhash")]
        public string Avatarhash { get; set; }

        [JsonPropertyName("personastate")]
        public int Personastate { get; set; }

        [JsonPropertyName("realname")]
        public string Realname { get; set; }

        [JsonPropertyName("primaryclanid")]
        public string Primaryclanid { get; set; }

        [JsonPropertyName("timecreated")]
        public int Timecreated { get; set; }

        [JsonPropertyName("personastateflags")]
        public int Personastateflags { get; set; }

        [JsonPropertyName("loccountrycode")]
        public string Loccountrycode { get; set; }

        [JsonPropertyName("locstatecode")]
        public string Locstatecode { get; set; }

        [JsonPropertyName("loccityid")]
        public int Loccityid { get; set; }
    }

    public class Response
    {
        [JsonPropertyName("players")]
        public List<Player> Players { get; set; }
    }

    public class SteamUserResponse
    {
        [JsonPropertyName("response")]
        public Response Response { get; set; }
    }

}
