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
using FrbDemoDuckHunt.Entities;
#endif

namespace FrbDemoDuckHunt.Screens
{
	public partial class GameScreen
	{
        private Random rnd;
        private bool SetNewPoint = false;
        private float CurrentLevelSpeed;

		void CustomInitialize()
		{
            rnd = new Random();
            SpriteManager.Camera.BackgroundColor = new Microsoft.Xna.Framework.Color(63, 191, 255);
            CurrentState = VariableState.StartDucks;
            CurrentLevelSpeed = InitialDuckSpeed;
		}

		void CustomActivity(bool firstTimeCalled)
		{
            switch (CurrentState)
            {
                case VariableState.StartIntro:
                    DogInstance.WalkingSniffingThenDiving(() => CurrentState = VariableState.StartDucks);
                    CurrentState = VariableState.Intro;
                    break;
                case VariableState.Intro:
                    break;
                case VariableState.StartDucks:
                    DuckInstance.Y = StartDuckY;
                    DuckInstance.X = rnd.Next(MinDuckX, MaxDuckX);
                    DuckInstance.Visible = true;
                    SetNewPoint = true;
                    CurrentState = VariableState.DucksFlying;
                    break;
                case VariableState.DucksFlying:
                    if (SetNewPoint)
                    {
                        DuckInstance.FlyTo(rnd.Next(MinDuckX, MaxDuckX), rnd.Next(MinDuckY, MaxDuckY), CurrentLevelSpeed, () => SetNewPoint = true);
                        SetNewPoint = false;
                    }
                    break;
                case VariableState.DucksEscaping:
                    //DuckInstance.FlyAway(() => CurrentState = VariableState.PostDucks);
                    break;
                case VariableState.PostDucks:
                    break;
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
