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
        private bool _doFlyAway = false;
        private Color _blue = new Microsoft.Xna.Framework.Color(63, 191, 255);
        private Color _pink = new Microsoft.Xna.Framework.Color(255, 191, 179);
        private Guid flyingGuid = Guid.Empty;
        private GameState _state = new GameState();

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

            switch (_rnd.Next(0, 3))
            {
                case 0:
                    duck.DuckType = GameState.DuckTypes.Black;
                    break;
                case 1:
                    duck.DuckType = GameState.DuckTypes.Blue;
                    break;
                case 2:
                    duck.DuckType = GameState.DuckTypes.Red;
                    break;
            }

            duck.Call(() => { if (newGuid == flyingGuid) { _doFlyAway = true; } }).After(5);
        }

        void CheckDuck(Duck duck)
        {
            if (duck.SetNewPoint && !duck.IsShot)
            {
                duck.FlyTo(_rnd.Next(MinDuckX, MaxDuckX), _rnd.Next(MinDuckY, MaxDuckY), _state.LevelSpeed, () => duck.SetNewPoint = true);
                duck.SetNewPoint = false;
            }
            else if (_doFlyAway && !duck.IsShot)
            {
                duck.FlyAway(() => duck.HasEscaped = true, _state.LevelSpeed);
                CurrentState = VariableState.DucksEscaping;
            }
        }

        void CheckDuckShot(Duck duck, Score score)
        {
            if (!duck.IsShot && duck.CollisionCircle.IsPointInside(InputManager.Mouse.WorldXAt(0), InputManager.Mouse.WorldYAt(0)))
            {
                score.Position = duck.Position;
                var val = _state.GetScore(duck.DuckType);
                score.TextInstanceDisplayText = val.ToString();
                _state.Score += val;
                score.Visible = true;
                score.Set("Visible").To(false).After(score.TimeToShow);
                duck.IsShot = true;
                duck.Shot(() => duck.Fall(() => { duck.HasFallen = true; duck.Velocity = Vector3.Zero; }));
            }
        }

		void CustomInitialize()
		{
            _rnd = new Random();
            SpriteManager.Camera.BackgroundColor = _blue;
            CurrentState = VariableState.StartIntro;
            _state.LevelSpeed = InitialDuckSpeed;
            _state.FlightTime = InitialFlightTime;
            _state.IncludeDuck2 = false;
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
                        if (_state.IncludeDuck2) SetDuck(DuckInstance2, newGuid);
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
                        CheckDuckShot(DuckInstance, ScoreInstance);

                        if (_state.IncludeDuck2)
                        {
                            CheckDuckShot(DuckInstance2, ScoreInstance2);
                        }
                    }

                    //Duck 1
                    CheckDuck(DuckInstance);

                    //Duck 2
                    if (_state.IncludeDuck2)
                    {
                        CheckDuck(DuckInstance2);
                    }

                    if (DuckInstance.HasFallen && (!_state.IncludeDuck2 || DuckInstance2.HasFallen))
                    {
                        CurrentState = VariableState.PostDucks;
                    }

                    break;
                case VariableState.DucksEscaping:
                    if ((DuckInstance.HasEscaped || DuckInstance.IsShot) && (!_state.IncludeDuck2 || (DuckInstance2.HasEscaped || DuckInstance2.IsShot)))
                    {
                        CurrentState = VariableState.PostDucks;
                    }

                    if (!_state.IncludeDuck2 || (!DuckInstance.IsShot && !DuckInstance2.IsShot))
                    {
                        SpriteManager.Camera.BackgroundColor = _pink;
                    }
                    
                    break;

                case VariableState.PostDucks:
                    flyingGuid = Guid.Empty;
                    SpriteManager.Camera.BackgroundColor = _blue;

                    DuckInstance.Velocity = Vector3.Zero;
                    DuckInstance2.Velocity = Vector3.Zero;

                    if(_state.IncludeDuck2)
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
