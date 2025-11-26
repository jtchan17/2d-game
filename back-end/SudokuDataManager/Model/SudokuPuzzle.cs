using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SudokuDataManager
{
    public class SudokuPuzzle
    {
        public int PuzzleId { get; set; }
        public string Difficulty { get; set; }   // Easy, Medium, Hard
        public string PuzzleGrid { get; set; }   // you can store it as string
        public bool IsCompleted { get; set; }
        public TimeSpan CompletionTime { get; set; }
    }
}