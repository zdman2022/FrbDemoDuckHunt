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
using Microsoft.Xna.Framework;
#endif

namespace FrbDemoDuckHunt.Screens
{
	public partial class GameScreen
	{
        private Random _rnd;
        private float _currentLevelSpeed;
        private bool _doFlyAway = false;
        private float _currentFlightTime = 0f;
        private Color _blue = new Microsoft.Xna.Framework.Color(63, 191, 255);
        private Color _pink = new Microsoft.Xna.Framework.Color(255, 191, 179);
        private Guid flyingGuid = Guid.Empty;
        private bool _includeDuck2 = true;

        void SetDuck(Duck duck, Guid newGuid)
        {
            duck.Y = StartDuckY;
            duck.X = _rnd.Next(MinDuckX, MaxDuckX);
            duck.Visible = true;
            duck.CurrentState = Duck.VariableState.FlyLeft;
            duck.SetNewPoint = true;
            duck.IsEscaped = false;
            duck.IsShot = false;
            duck.HasFallen = false;
            duck.HasEscaped = false;

            duck.Call(() => { if (newGuid == flyingGuid) { _doFlyAway = true; } }).After(5);
        }

        void CheckDuck(Duck duck)
        {
            if (duck.SetNewPoint && !duck.IsShot)
            {
                duck.FlyTo(_rnd.Next(MinDuckX, MaxDuckX), _rnd.Next(MinDuckY, MaxDuckY), _currentLevelSpeed, () => duck.SetNewPoint = true);
                duck.SetNewPoint = false;
            }
            else if (_doFlyAway && !duck.IsShot)
            {
                duck.FlyAway(() => duck.HasEscaped = true, _currentLevelSpeed);
                CurrentState = VariableState.DucksEscaping;
            }
        }

        void CheckDuckShot(Duck duck)
        {
            if (!duck.IsShot && duck.CollisionCircle.IsPointInside(InputManager.Mouse.WorldXAt(0), InputManager.Mouse.WorldYAt(0)))
            {
                duck.IsShot = true;
                duck.Shot(() => duck.Fall(() => { duck.HasFallen = true; duck.Velocity = Vector3.Zero; }));
            }
        }

		void CustomInitialize()
		{
            _rnd = new Random();
            SpriteManager.Camera.BackgroundColor = _blue;
            CurrentState = VariableState.StartIntro;
            _currentLevelSpeed = InitialDuckSpeed;
            _currentFlightTime = InitialFlightTime;
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
                    {
                        var newGuid = Guid.NewGuid();
                        flyingGuid = newGuid;
                        SetDuck(DuckInstance, newGuid);
                        if (_includeDuck2) SetDuck(DuckInstance2, newGuid);
                    }

                    _doFlyAway = false;
                    CurrentState = VariableState.DucksFlying;

                    break;
                case VariableState.DucksFlying:
                    bool shot = false;
                    if(InputManager.Mouse.ButtonPushed(Mouse.MouseButtons.LeftButton))
                    {
                        ShotInstance.Shoot(() => { });
                        shot = true;
                    }

                    if (shot)
                    {
                        shot = false;
                        CheckDuckShot(DuckInstance);

                        if (_includeDuck2)
                        {
                            CheckDuckShot(DuckInstance2);
                        }
                    }

                    //Duck 1
                    CheckDuck(DuckInstance);

                    //Duck 2
                    if (_includeDuck2)
                    {
                        CheckDuck(DuckInstance2);
                    }

                    if (DuckInstance.HasFallen && (_includeDuck2 && DuckInstance2.HasFallen))
                    {
                        CurrentState = VariableState.PostDucks;
                    }

                    break;
                case VariableState.DucksEscaping:
                    if ((DuckInstance.HasEscaped || DuckInstance.IsShot) && (_includeDuck2 && (DuckInstance2.HasEscaped || DuckInstance2.IsShot)))
                    {
                        CurrentState = VariableState.PostDucks;
                    }

                    SpriteManager.Camera.BackgroundColor = _pink;
                    
                    break;

                case VariableState.PostDucks:
                    flyingGuid = Guid.Empty;
                    SpriteManager.Camera.BackgroundColor = _blue;

                    DuckInstance.Velocity = Vector3.Zero;
                    DuckInstance2.Velocity = Vector3.Zero;

                    if(_includeDuck2)
                    {
                        if(DuckInstance.IsShot && DuckInstance2.IsShot)
                        {
                            DogInstance.TwoDucks(DuckInstance.X, () => CurrentState = VariableState.StartDucks);
                        }else if(DuckInstance.IsShot || DuckInstance2.IsShot){
                            DogInstance.OneDuck(DuckInstance.IsShot 
                                                        ? DuckInstance.X
                                                        : DuckInstance2.X,
                                                () => CurrentState = VariableState.StartDucks);
                        }else{
                            DogInstance.Laugh(() => CurrentState = VariableState.StartDucks);
                        }
                    }else{
                        if(DuckInstance.IsShot)
                        {
                            DogInstance.OneDuck(DuckInstance.X, () => CurrentState = VariableState.StartDucks);
                        }else{
                            DogInstance.Laugh(() => CurrentState = VariableState.StartDucks);
                        }
                    }

                    CurrentState = VariableState.DogAnimation;

                    break;

                case VariableState.DogAnimation:
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
