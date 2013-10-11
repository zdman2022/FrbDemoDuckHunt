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
	public partial class GameScreen
	{
        private void DogWalking()
        {
            DogInstance.WalkingSniffingThenDiving(DogOneDuck);
        }

        private void DogOneDuck()
        {
            DogInstance.OneDuck((new Random()).Next(-75, 75), DogTwoDucks);
        }

        private void DogTwoDucks()
        {
            DogInstance.TwoDucks((new Random()).Next(-75, 75), DogLaugh);
        }

        private void DogLaugh()
        {
            DogInstance.Laugh(DogOneDuck);
        }

		void CustomInitialize()
		{
            DogWalking();
		}

		void CustomActivity(bool firstTimeCalled)
		{


		}

		void CustomDestroy()
		{


		}

        static void CustomLoadStaticContent(string contentManagerName)
        {


        }

	}
}
