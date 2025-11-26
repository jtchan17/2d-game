using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SudokuDataManager
{
    public class Player
    {
        // private int PlayerId;
        // private string Username;
        // private int Score;
        // private int TotalGammes;
        // private TimeSpan BestTime;

        
        public int PlayerId { get; set; }
        public string Username { get; set; }
        public int Score { get; set; }
        public int TotalGames { get; set; }
        public TimeSpan BestTime { get; set; }
    }
}