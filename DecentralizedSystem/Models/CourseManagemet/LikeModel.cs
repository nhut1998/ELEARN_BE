using Newtonsoft.Json;
using System;

namespace DecentralizedSystem.Models.CourseManagemet
{
    public class LikeModel
    {
        [JsonProperty("course_id")]
        public string CourseId { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        //[JsonProperty("like_at")]
        //public DateTime LikeAt { get; set; }


    }
}
