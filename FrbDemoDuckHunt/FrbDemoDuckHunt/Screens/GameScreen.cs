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
        private bool _setNewPoint = false;
        private float _currentLevelSpeed;
        private bool _doFlyAway = false;
        private bool _duckEscaped = false;
        private bool _duckShot = false;
        private float _currentFlightTime = 0f;
        private Color _blue = new Microsoft.Xna.Framework.Color(63, 191, 255);
        private Color _pink = new Microsoft.Xna.Framework.Color(255, 191, 179);
        private Guid flyingGuid = Guid.Empty;


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
            bool shot = false;
            if(InputManager.Mouse.ButtonPushed(Mouse.MouseButtons.LeftButton))
            {
                ShotInstance.Shoot(() => { });
                shot = true;
            }

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
                    DuckInstance.X = _rnd.Next(MinDuckX, MaxDuckX);
                    DuckInstance.Visible = true;
                    _setNewPoint = true;
                    CurrentState = VariableState.DucksFlying;
                    _doFlyAway = false;
                    {
                        var newGuid = Guid.NewGuid();
                        flyingGuid = newGuid;
                        DuckInstance.Call(() => { if (newGuid == flyingGuid) { _doFlyAway = true; } }).After(5);
                    }
                    _duckEscaped = false;
                    _duckShot = false;
                    DuckInstance.CurrentState = Duck.VariableState.FlyLeft;

                    break;
                case VariableState.DucksFlying:
                    if (shot)
                    {
                        shot = false;
                        if (DuckInstance.CollisionCircle.IsPointInside(InputManager.Mouse.WorldXAt(0), InputManager.Mouse.WorldYAt(0)))
                        {
                            CurrentState = VariableState.PostDucks;
                            DuckInstance.Shot(() => DuckInstance.Fall(() => _duckShot = true));
                        }
                    }

                    if (_setNewPoint)
                    {
                        DuckInstance.FlyTo(_rnd.Next(MinDuckX, MaxDuckX), _rnd.Next(MinDuckY, MaxDuckY), _currentLevelSpeed, () => _setNewPoint = true);
                        _setNewPoint = false;
                    }else if (_doFlyAway)
                    {
                        DuckInstance.FlyAway(() => CurrentState = VariableState.PostDucks, _currentLevelSpeed);
                        CurrentState = VariableState.DucksEscaping;
                    }

                    break;
                case VariableState.DucksEscaping:
                    _duckEscaped = true;
                    SpriteManager.Camera.BackgroundColor = _pink;
                    
                    break;

                case VariableState.PostDucks:
                    flyingGuid = Guid.Empty;
                    SpriteManager.Camera.BackgroundColor = _blue;
                    if (_duckEscaped)
                    {
                        DogInstance.Laugh(() => CurrentState = VariableState.StartDucks);
                        _duckEscaped = false;
                    }

                    if (_duckShot)
                    {
                        DuckInstance.Velocity = Vector3.Zero;
                        DogInstance.OneDuck(DuckInstance.X, () => CurrentState = VariableState.StartDucks);
                        _duckShot = false;
                    }

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
