using System;
using System.Collections.Generic;
using System.Linq;
using FrbDemoDuckHunt.DataTypes;
using FrbUi;
using FrbUi.Controls;
using FrbUi.Layouts;
using FrbUi.Xml;

namespace FrbDemoDuckHunt.Entities
{
	public partial class GameInterface
	{
        public enum DuckDisplayType { Active, Missed, Hit, Scored }

	    private bool _uiIsReady;
	    private LayoutableText _scoreLabel;
	    private SimpleLayout _mainLayout;
	    private List<LayoutableSprite> _shotIndicators;
	    private BoxLayout _shotContainer;
	    private LayoutableSprite _hitDuckTemplate;
	    private LayoutableSprite _missedDuckTemplate;
	    private LayoutableSprite _activeDuckTemplate;
	    private LayoutableSprite _scoredDuckTemplate;
	    private LayoutableSprite _blueBarTemplate;
	    private BoxLayout _duckContainer;
	    private BoxLayout _barContainer;
	    private List<ILayoutable> _barIndicators; 
	    private List<DuckDisplayType> _duckTypes;
	    private LayoutableText _roundLabel;
	    private BoxLayout _dialog;
	    private LayoutableText _dialogText;
	    private bool _updateDuckDisplay;

        public DuckDisplayType GetDuckDisplay(int duckIndex)
        {
            if (duckIndex >= _duckTypes.Count)
            {
                throw new IndexOutOfRangeException(
                    string.Format("Tried to get the type for duck at index {0} but only {1} ducks are in the display",
                                  duckIndex, _duckTypes.Count));
            }

            return _duckTypes[duckIndex];
        }

        public void SetDuckDisplay(int duckIndex, DuckDisplayType type)
        {
            if (duckIndex >= _duckTypes.Count)
            {
                throw new IndexOutOfRangeException(
                    string.Format("Tried to set the type for duck at index {0} but only {1} ducks are in the display",
                                  duckIndex, _duckTypes.Count));
            }

            _duckTypes[duckIndex] = type;
            _updateDuckDisplay = true;
        }

        public void ShowDialog(string text)
        {
            _dialogText.DisplayText = text;
            _dialog.Visible = true;
        }

        public void HideDialog()
        {
            _dialog.Visible = false;
        }

		private void CustomInitialize()
		{
		    LoadUserInterface();

		    _mainLayout.AttachTo(this, false);
            _scoreLabel.DisplayText = Score.ToString("000000");
		    _roundLabel.DisplayText = Round.ToString();

            _duckTypes = new List<DuckDisplayType>();
		    for (int x = 0; x < TotalDucks; x++)
		        _duckTypes.Add(DuckDisplayType.Missed);
		    
            UpdateDuckDisplay();
            UpdateBarDisplay();
		}

	    private void CustomActivity()
		{
		}

		private void CustomDestroy()
		{
		}

        public override void UpdateDependencies(double currentTime)
        {
            base.UpdateDependencies(currentTime);

            if (_updateDuckDisplay)
            {
                UpdateDuckDisplay();
                _updateDuckDisplay = false;
            }
        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {
        }

        private void LoadUserInterface()
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
            var roundLabelName = GlobalContent.InterfaceConstants[InterfaceConstants.GameInterfaceRoundLabel].Value;
            var blueBarName = GlobalContent.InterfaceConstants[InterfaceConstants.GameInterfaceBlueBar].Value;
            var barContainerName = GlobalContent.InterfaceConstants[InterfaceConstants.GameInterfaceBarContainer].Value;
            var dialogName = GlobalContent.InterfaceConstants[InterfaceConstants.GameInterfaceDialog].Value;
            var dialogTextName = GlobalContent.InterfaceConstants[InterfaceConstants.GameInterfaceDialogText].Value;
            var scoredDuckName = GlobalContent.InterfaceConstants[InterfaceConstants.GameInterfaceScoredDuck].Value;

            var interfacePackage = new UserInterfacePackage(xmlFilePath, ContentManagerName);
            _scoreLabel = interfacePackage.GetNamedControl<LayoutableText>(scoreLabelName);
            _mainLayout = interfacePackage.GetNamedControl<SimpleLayout>(mainLabelName);
            _shotContainer = interfacePackage.GetNamedControl<BoxLayout>(shotContainerName);
            _activeDuckTemplate = interfacePackage.GetNamedControl<LayoutableSprite>(activeDuckName);
            _hitDuckTemplate = interfacePackage.GetNamedControl<LayoutableSprite>(hitDuckName);
            _missedDuckTemplate = interfacePackage.GetNamedControl<LayoutableSprite>(missedDuckName);
            _duckContainer = interfacePackage.GetNamedControl<BoxLayout>(duckContainerName);
            _roundLabel = interfacePackage.GetNamedControl<LayoutableText>(roundLabelName);
            _blueBarTemplate = interfacePackage.GetNamedControl<LayoutableSprite>(blueBarName);
            _barContainer = interfacePackage.GetNamedControl<BoxLayout>(barContainerName);
            _dialog = interfacePackage.GetNamedControl<BoxLayout>(dialogName);
            _dialogText = interfacePackage.GetNamedControl<LayoutableText>(dialogTextName);
            _scoredDuckTemplate = interfacePackage.GetNamedControl<LayoutableSprite>(scoredDuckName);

            SetupShotContainer(interfacePackage, shotName);
            SetupBarContainer();

            _uiIsReady = true;
        }

        private void SetupBarContainer()
        {
            _barIndicators = new List<ILayoutable>();
            var maxBars = TotalDucks * BarsPerDuck;
            for (int x = 0; x < maxBars; x++)
            {
                var clone = _blueBarTemplate.Clone();
                _barContainer.AddItem(clone);
                _barIndicators.Add(clone);
            }
        }

        private void SetupShotContainer(UserInterfacePackage interfacePackage, string shotName)
        {
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
        }

        private void UpdateDuckDisplay()
        {
            if (!_uiIsReady)
                return;

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

                    case DuckDisplayType.Scored:
                        clonedItem = _scoredDuckTemplate.Clone();
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

        private void UpdateBarDisplay()
        {
            if (!_uiIsReady)
                return;

            var visibleBarCount = BarsPerDuck * DucksRequiredForRound;
            for (int x = 0; x < _barIndicators.Count; x++)
            {
                _barIndicators[x].Visible = (x < visibleBarCount);
            }
        }
	}
}
