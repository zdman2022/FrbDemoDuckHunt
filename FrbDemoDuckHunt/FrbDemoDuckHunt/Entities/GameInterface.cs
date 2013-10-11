using System;
using System.Collections.Generic;
using System.Text;
using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Graphics.Particle;

#if FRB_XNA || SILVERLIGHT
using FrbDemoDuckHunt.DataTypes;
using FrbUi.Controls;
using FrbUi.Layouts;
using FrbUi.Xml;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Vector3 = Microsoft.Xna.Framework.Vector3;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
#endif

using FlatRedBall.Math.Geometry;
using FlatRedBall.Math.Splines;
using BitmapFont = FlatRedBall.Graphics.BitmapFont;
using Cursor = FlatRedBall.Gui.Cursor;
using GuiManager = FlatRedBall.Gui.GuiManager;

namespace FrbDemoDuckHunt.Entities
{
	public partial class GameInterface
	{
	    private LayoutableText _scoreLabel;
	    private SimpleLayout _mainLayout;

		private void CustomInitialize()
		{
		    var xmlFilePath = GlobalContent.InterfaceConstants[InterfaceConstants.GameInterfaceXml].Value;
		    var scoreLabelName = GlobalContent.InterfaceConstants[InterfaceConstants.GameInterfaceScoreValue].Value;
		    var mainLabelName = GlobalContent.InterfaceConstants[InterfaceConstants.GameInterfaceMainLayout].Value;

            var interfacePackage = new UserInterfacePackage(xmlFilePath, ContentManagerName);
            _scoreLabel = interfacePackage.GetNamedControl<LayoutableText>(scoreLabelName);
            _mainLayout = interfacePackage.GetNamedControl<SimpleLayout>(mainLabelName);

            _mainLayout.AttachTo(this, false);
		}

		private void CustomActivity()
		{
		}

		private void CustomDestroy()
		{
		}

        private static void CustomLoadStaticContent(string contentManagerName)
        {
        }
	}
}
