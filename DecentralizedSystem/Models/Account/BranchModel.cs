﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DecentralizedSystem.API.Models.Account
{
    public class BranchModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName
        {
            get { return $"{Code} - {Name}"; }
        }

        [JsonProperty("is_parent")]
        public int IsParent { get; set; }
    }
}
