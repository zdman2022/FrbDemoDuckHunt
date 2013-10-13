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
        public float LevelSpeed
        {
            get
            {
                return InitialSpeed;
            }
        }
        public float FlightTime
        {
            get
            {
                return InitialFlightTime;
            }
        }
        public bool IncludeDuck2 { get; set; }
        public int DuckFlight { get; set; }
        public float InitialFlightTime { get; set; }
        public float InitialSpeed { get; set; }

        public enum DuckTypes
        {
            Black,
            Blue,
            Red
        }

        public int GetScore(DuckTypes type)
        {
            switch (type)
            {
                case DuckTypes.Black:
                    if (Round <= 5)
                    {
                        return 500;
                    }
                    else if (Round <= 10)
                    {
                        return 800;
                    }
                    else if (Round <= 15)
                    {
                        return 1000;
                    }
                    else if (Round <= 20)
                    {
                        return 1000;
                    }
                    else
                    {
                        return 1000;
                    }

                case DuckTypes.Blue:
                    if (Round <= 5)
                    {
                        return 1000;
                    }
                    else if (Round <= 10)
                    {
                        return 1500;
                    }
                    else if (Round <= 15)
                    {
                        return 2000;
                    }
                    else if (Round <= 20)
                    {
                        return 2000;
                    }
                    else
                    {
                        return 2000;
                    }

                case DuckTypes.Red:
                    if (Round <= 5)
                    {
                        return 1500;
                    }
                    else if (Round <= 10)
                    {
                        return 2400;
                    }
                    else if (Round <= 15)
                    {
                        return 3000;
                    }
                    else if (Round <= 20)
                    {
                        return 3000;
                    }
                    else
                    {
                        return 3000;
                    }

            }

            return 0;
        }

        public int GetBonus()
        {
            if (Round <= 5)
            {
                return 10000;
            }
            else if (Round <= 10)
            {
                return 10000;
            }
            else if (Round <= 15)
            {
                return 15000;
            }
            else if (Round <= 20)
            {
                return 20000;
            }
            else
            {
                return 30000;
            }
        }

        public int DucksRequiredToAdvance()
        {
            if (Round <= 10)
            {
                return 6;
            }
            else if (Round <= 12)
            {
                return 7;
            }
            else if (Round <= 14)
            {
                return 8;
            }
            else if (Round <= 19)
            {
                return 9;
            }
            else
            {
                return 10;
            }
        }
    }
}
