using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TF47_Api.Models
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class ForumUser
    {
        [JsonProperty("uid")]
        public long Uid { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("userslug")]
        public string Userslug { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("email:confirmed")]
        public bool EmailConfirmed { get; set; }

        [JsonProperty("joindate")]
        public long Joindate { get; set; }

        [JsonProperty("lastonline")]
        public long Lastonline { get; set; }

        [JsonProperty("picture")]
        public string Picture { get; set; }

        [JsonProperty("fullname")]
        public string Fullname { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("birthday")]
        public string Birthday { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }

        [JsonProperty("aboutme")]
        public object Aboutme { get; set; }

        [JsonProperty("signature")]
        public string Signature { get; set; }

        [JsonProperty("uploadedpicture")]
        public string Uploadedpicture { get; set; }

        [JsonProperty("profileviews")]
        public long Profileviews { get; set; }

        [JsonProperty("reputation")]
        public long Reputation { get; set; }

        [JsonProperty("postcount")]
        public long Postcount { get; set; }

        [JsonProperty("topiccount")]
        public long Topiccount { get; set; }

        [JsonProperty("lastposttime")]
        public long Lastposttime { get; set; }

        [JsonProperty("banned")]
        public bool Banned { get; set; }

        [JsonProperty("banned:expire")]
        public long BannedExpire { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("flags")]
        public object Flags { get; set; }

        [JsonProperty("followerCount")]
        public long FollowerCount { get; set; }

        [JsonProperty("followingCount")]
        public long FollowingCount { get; set; }

        [JsonProperty("cover:url")]
        public string CoverUrl { get; set; }

        [JsonProperty("cover:position")]
        public string CoverPosition { get; set; }

        [JsonProperty("groupTitle")]
        public string GroupTitle { get; set; }

        [JsonProperty("groupTitleArray")]
        public string[] GroupTitleArray { get; set; }

        [JsonProperty("icon:text")]
        public string IconText { get; set; }

        [JsonProperty("icon:bgColor")]
        public string IconBgColor { get; set; }

        [JsonProperty("joindateISO")]
        public DateTimeOffset JoindateIso { get; set; }

        [JsonProperty("lastonlineISO")]
        public DateTimeOffset LastonlineIso { get; set; }

        [JsonProperty("banned_until")]
        public long BannedUntil { get; set; }

        [JsonProperty("banned_until_readable")]
        public string BannedUntilReadable { get; set; }

        [JsonProperty("age")]
        public long Age { get; set; }

        [JsonProperty("emailClass")]
        public string EmailClass { get; set; }

        [JsonProperty("ips")]
        public string[] Ips { get; set; }

        [JsonProperty("isBlocked")]
        public bool IsBlocked { get; set; }

        [JsonProperty("blocksCount")]
        public long BlocksCount { get; set; }

        [JsonProperty("yourid")]
        public long Yourid { get; set; }

        [JsonProperty("theirid")]
        public long Theirid { get; set; }

        [JsonProperty("isTargetAdmin")]
        public bool IsTargetAdmin { get; set; }

        [JsonProperty("isAdmin")]
        public bool IsAdmin { get; set; }

        [JsonProperty("isGlobalModerator")]
        public bool IsGlobalModerator { get; set; }

        [JsonProperty("isModerator")]
        public bool IsModerator { get; set; }

        [JsonProperty("isAdminOrGlobalModerator")]
        public bool IsAdminOrGlobalModerator { get; set; }

        [JsonProperty("isAdminOrGlobalModeratorOrModerator")]
        public bool IsAdminOrGlobalModeratorOrModerator { get; set; }

        [JsonProperty("isSelfOrAdminOrGlobalModerator")]
        public bool IsSelfOrAdminOrGlobalModerator { get; set; }

        [JsonProperty("canEdit")]
        public bool CanEdit { get; set; }

        [JsonProperty("canBan")]
        public bool CanBan { get; set; }

        [JsonProperty("canChangePassword")]
        public bool CanChangePassword { get; set; }

        [JsonProperty("isSelf")]
        public bool IsSelf { get; set; }

        [JsonProperty("isFollowing")]
        public bool IsFollowing { get; set; }

        [JsonProperty("hasPrivateChat")]
        public long HasPrivateChat { get; set; }

        [JsonProperty("showHidden")]
        public bool ShowHidden { get; set; }

        [JsonProperty("groups")]
        public Group[] Groups { get; set; }

        [JsonProperty("disableSignatures")]
        public bool DisableSignatures { get; set; }

        [JsonProperty("reputation:disabled")]
        public bool ReputationDisabled { get; set; }

        [JsonProperty("downvote:disabled")]
        public bool DownvoteDisabled { get; set; }

        [JsonProperty("profile_links")]
        public ProfileLink[] ProfileLinks { get; set; }

        [JsonProperty("sso")]
        public object[] Sso { get; set; }

        [JsonProperty("websiteLink")]
        public string WebsiteLink { get; set; }

        [JsonProperty("websiteName")]
        public string WebsiteName { get; set; }

        [JsonProperty("moderationNote")]
        public string ModerationNote { get; set; }

        [JsonProperty("username:disableEdit")]
        public bool UsernameDisableEdit { get; set; }

        [JsonProperty("email:disableEdit")]
        public bool EmailDisableEdit { get; set; }
    }

    public partial class Group
    {
        [JsonProperty("createtime")]
        public long Createtime { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("disableJoinRequests")]
        public long DisableJoinRequests { get; set; }

        [JsonProperty("hidden")]
        public long Hidden { get; set; }

        [JsonProperty("memberCount")]
        public long MemberCount { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("ownerUid", NullValueHandling = NullValueHandling.Ignore)]
        public long? OwnerUid { get; set; }

        [JsonProperty("private")]
        public long Private { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("system")]
        public long System { get; set; }

        [JsonProperty("userTitle")]
        public string UserTitle { get; set; }

        [JsonProperty("userTitleEnabled")]
        public long UserTitleEnabled { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("labelColor")]
        public string LabelColor { get; set; }

        [JsonProperty("disableLeave")]
        public long DisableLeave { get; set; }

        [JsonProperty("nameEncoded")]
        public string NameEncoded { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("textColor")]
        public string TextColor { get; set; }

        [JsonProperty("createtimeISO")]
        public DateTimeOffset CreatetimeIso { get; set; }

        [JsonProperty("cover:thumb:url")]
        public string CoverThumbUrl { get; set; }

        [JsonProperty("cover:url")]
        public string CoverUrl { get; set; }

        [JsonProperty("cover:position")]
        public string CoverPosition { get; set; }
    }

    public partial class ProfileLink
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("route")]
        public string Route { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("visibility")]
        public Visibility Visibility { get; set; }

        [JsonProperty("public")]
        public bool Public { get; set; }
    }

    public partial class Visibility
    {
        [JsonProperty("self")]
        public bool Self { get; set; }

        [JsonProperty("other")]
        public bool Other { get; set; }

        [JsonProperty("moderator")]
        public bool Moderator { get; set; }

        [JsonProperty("globalMod")]
        public bool GlobalMod { get; set; }

        [JsonProperty("admin")]
        public bool Admin { get; set; }

        [JsonProperty("canViewInfo")]
        public bool CanViewInfo { get; set; }
    }
}
