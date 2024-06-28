﻿using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebChat.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        [JsonIgnore]
        public List<Message> Messages { get; set; }
        public long IdLong { get; set; }
    }
}