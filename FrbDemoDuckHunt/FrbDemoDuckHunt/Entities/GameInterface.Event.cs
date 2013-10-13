using System;
using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Specialized;
using FlatRedBall.Audio;
using FlatRedBall.Screens;
using FrbDemoDuckHunt.Entities;
using FrbDemoDuckHunt.Screens;
namespace FrbDemoDuckHunt.Entities
{
    public partial class GameInterface
    {
        void OnAfterScoreSet(object sender, EventArgs e)
        {
            // __scoreLabel will be null when the score's default value is set
            if (_scoreLabel != null)
                _scoreLabel.DisplayText = Score.ToString();
        }

        void OnAfterAvailableShotsSet(object sender, EventArgs e)
        {
            if (_shotIndicators != null)
            {
                for (int x = 0; x < _shotIndicators.Count; x++)
                {
                    var indicator = _shotIndicators[x];
                    indicator.Visible = (x < AvailableShots);
                }
            }
        }

        void OnAfterRoundSet (object sender, EventArgs e)
        {
            if (_roundLabel != null)
            {
                _roundLabel.DisplayText = Round.ToString();
            }
        }

        void OnAfterDucksRequiredForRoundSet (object sender, EventArgs e)
        {
            UpdateBarDisplay();
        }

    }
}
