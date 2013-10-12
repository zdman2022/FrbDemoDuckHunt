using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrbDemoDuckHunt
{
    public class GameState
    {
        public int Round { get; set; }
        public int Score { get; set; }
        public int Ammo { get; set; }
        public float LevelSpeed { get; set; }
        public float FlightTime { get; set; }
        public bool IncludeDuck2 { get; set; }
    }
}
