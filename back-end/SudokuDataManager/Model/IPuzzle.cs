using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SudokuDataManager
{
    public interface IPuzzle : IPrint
    {
        string Difficulty { get; set; }
        bool IsCompleted { get; set; }
        TimeSpan CompletionTime { get; set; }
    }
}