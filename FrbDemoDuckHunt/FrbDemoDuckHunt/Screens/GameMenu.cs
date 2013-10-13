using System;
using System.Collections.Generic;
using System.Text;
using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Graphics.Particle;

using FlatRedBall.Graphics.Model;
using FlatRedBall.Math.Geometry;
using FlatRedBall.Math.Splines;

using Cursor = FlatRedBall.Gui.Cursor;
using GuiManager = FlatRedBall.Gui.GuiManager;
using FlatRedBall.Localization;

#if FRB_XNA || SILVERLIGHT
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Vector3 = Microsoft.Xna.Framework.Vector3;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
#endif

namespace FrbDemoDuckHunt.Screens
{
	public partial class GameMenu
	{
        private Microsoft.Xna.Framework.Audio.SoundEffectInstance _theme = GlobalContent.MainThemeSong.CreateInstance();

		void CustomInitialize()
		{
            CurrentState = VariableState.GameA;
            HighScoreStorage.NumOfDucks = 1;
            _theme.Play();
            Camera.Main.DrawsShapes = false;
            SpriteManager.Camera.BackgroundColor = Microsoft.Xna.Framework.Color.Black;
		}

		void CustomActivity(bool firstTimeCalled)
		{
            HighScoreInstance.TextInstanceDisplayText = HighScoreStorage.HighScore.ToString("00000");

            if (InputManager.Keyboard.KeyPushed(Keys.Up))
            {
                CurrentState = VariableState.GameA;
                HighScoreStorage.NumOfDucks = 1;
                GlobalContent.ShotSoundEffect.Play();
            }

            if (InputManager.Keyboard.KeyPushed(Keys.Down))
            {
                CurrentState = VariableState.GameB;
                HighScoreStorage.NumOfDucks = 2;
                GlobalContent.ShotSoundEffect.Play();
            }

            if (InputManager.Keyboard.KeyPushed(Keys.Enter))
            {
                _theme.Stop();
                MoveToScreen(typeof(GameScreen));
            }
		}

		void CustomDestroy()
		{


		}

        static void CustomLoadStaticContent(string contentManagerName)
        {


        }

	}
}
