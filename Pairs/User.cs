using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pairs
{
    public class User
    {
        public string username;
        public string avatarName;
        public int gamesPlayed;
        public int wins;

        public User(string username, string avatarName, int gamesPlayed, int wins)
        {
            this.username = username;
            this.avatarName = avatarName;
            this.gamesPlayed = gamesPlayed;
            this.wins = wins;
        }
    }
}
