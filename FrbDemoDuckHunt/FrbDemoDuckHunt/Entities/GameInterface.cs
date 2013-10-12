using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Graphics.Particle;

#if FRB_XNA || SILVERLIGHT
using FrbDemoDuckHunt.DataTypes;
using FrbUi;
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
        public enum DuckDisplayType { Active, Missed, Hit }

	    private LayoutableText _scoreLabel;
	    private SimpleLayout _mainLayout;
	    private List<LayoutableSprite> _shotIndicators;
	    private BoxLayout _shotContainer;
	    private LayoutableSprite _hitDuckTemplate;
	    private LayoutableSprite _missedDuckTemplate;
	    private LayoutableSprite _activeDuckTemplate;
	    private BoxLayout _duckContainer;
	    private List<DuckDisplayType> _duckTypes; 

        public void SetDuckDisplay(int duckIndex, DuckDisplayType type)
        {
            if (duckIndex >= _duckTypes.Count)
            {
                throw new IndexOutOfRangeException(
                    string.Format("Tried to set the type for duck at index {0} but only {1} ducks are in the display",
                                  duckIndex, _duckTypes.Count));
            }

            _duckTypes[duckIndex] = type;
            UpdateDuckDisplay();
        }

		private void CustomInitialize()
		{
		    var xmlFilePath = GlobalContent.InterfaceConstants[InterfaceConstants.GameInterfaceXml].Value;
		    var scoreLabelName = GlobalContent.InterfaceConstants[InterfaceConstants.GameInterfaceScoreValue].Value;
		    var mainLabelName = GlobalContent.InterfaceConstants[InterfaceConstants.GameInterfaceMainLayout].Value;
		    var shotName = GlobalContent.InterfaceConstants[InterfaceConstants.GameInterfaceShot].Value;
		    var shotContainerName = GlobalContent.InterfaceConstants[InterfaceConstants.GameInterfaceShotContainer].Value;
		    var hitDuckName = GlobalContent.InterfaceConstants[InterfaceConstants.GameInterfaceHitDuck].Value;
		    var missedDuckName = GlobalContent.InterfaceConstants[InterfaceConstants.GameInterfaceMissedDuck].Value;
		    var activeDuckName = GlobalContent.InterfaceConstants[InterfaceConstants.GameInterfaceActiveDuck].Value;
		    var duckContainerName = GlobalContent.InterfaceConstants[InterfaceConstants.GameInterfaceDuckContainer].Value;

            var interfacePackage = new UserInterfacePackage(xmlFilePath, ContentManagerName);
            _scoreLabel = interfacePackage.GetNamedControl<LayoutableText>(scoreLabelName);
            _mainLayout = interfacePackage.GetNamedControl<SimpleLayout>(mainLabelName);
		    _shotContainer = interfacePackage.GetNamedControl<BoxLayout>(shotContainerName);
		    _activeDuckTemplate = interfacePackage.GetNamedControl<LayoutableSprite>(activeDuckName);
		    _hitDuckTemplate = interfacePackage.GetNamedControl<LayoutableSprite>(hitDuckName);
		    _missedDuckTemplate = interfacePackage.GetNamedControl<LayoutableSprite>(missedDuckName);
		    _duckContainer = interfacePackage.GetNamedControl<BoxLayout>(duckContainerName);

            _shotIndicators = new List<LayoutableSprite>();
		    var shotSprite = interfacePackage.GetNamedControl<LayoutableSprite>(shotName);
            for (int x = 0; x < MaxShots; x++)
            {
                var clonedShotSprite = (LayoutableSprite)shotSprite.Clone();
                _shotIndicators.Add(clonedShotSprite);
                _shotContainer.AddItem(clonedShotSprite);

                if (x < AvailableShots)
                    clonedShotSprite.Visible = true;
            }

            _mainLayout.AttachTo(this, false);
		    _scoreLabel.DisplayText = Score.ToString();

            // Fill the duck container with missed/inactive duck indicators
            _duckTypes = new List<DuckDisplayType>();

		    for (int x = 0; x < TotalDucks; x++)
		        _duckTypes.Add(DuckDisplayType.Missed);

		    SetDuckDisplay(3, DuckDisplayType.Hit);
            SetDuckDisplay(5, DuckDisplayType.Active);
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

        private void UpdateDuckDisplay()
        {
            var existingItems = _duckContainer.Items.ToArray();
            foreach (var existingItem in existingItems)
            {
                _duckContainer.RemoveItem(existingItem);

                // TODO: This most likely needs to be updated to re-use clones
                // instead of destroying them and recreating the clonse
                UiControlManager.Instance.DestroyControl(existingItem);
            }

            foreach (var duckType in _duckTypes)
            {
                ILayoutable clonedItem;
                switch (duckType)
                {
                    case DuckDisplayType.Hit:
                        clonedItem = _hitDuckTemplate.Clone();
                        break;

                    case DuckDisplayType.Active:
                        clonedItem = _activeDuckTemplate.Clone();
                        break;

                    case DuckDisplayType.Missed:
                    default:
                        clonedItem = _missedDuckTemplate.Clone();
                        break;
                }

                clonedItem.Visible = true;
                _duckContainer.AddItem(clonedItem);
            }
        }
	}
}
