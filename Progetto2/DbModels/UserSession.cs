using System;

namespace Progetto2.DbModels
{
    public class UserSession
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public bool IsLoggedIn { get; set; }
        public DateTime LastLogin { get; set; }
    }
}