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
        private GameState _state = new GameState();
        private Microsoft.Xna.Framework.Audio.SoundEffectInstance _wings = GlobalContent.WingFlapSoundEffect.CreateInstance();
        private Microsoft.Xna.Framework.Audio.SoundEffectInstance _quake = GlobalContent.duck.CreateInstance();

        void SetDuck(Duck duck)
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
            _state.DuckFlight++;
            GameInterfaceInstance.SetDuckDisplay(_state.DuckFlight - 1, GameInterface.DuckDisplayType.Active);

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

            var currentDuckRound = _state.DuckFlight;
            duck.Call(() => { if (currentDuckRound == _state.DuckFlight) { _doFlyAway = true; } }).After(5);
        }

        void CheckDuck(Duck duck, int offset)
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
                GameInterfaceInstance.SetDuckDisplay(_state.DuckFlight - offset, GameInterface.DuckDisplayType.Missed);
            }
        }

        bool CheckDuckShot(Duck duck, Score score, int offset)
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
                GameInterfaceInstance.SetDuckDisplay(_state.DuckFlight - offset, GameInterface.DuckDisplayType.Hit);

                return true;
            }

            return false;
        }

        void MoveHits(Action finishedCallback)
        {
            if (HitsMoved())
            {
                finishedCallback();
                return;
            }

            var missFound = false;
            for (var i = 0; i < 9; i++)
            {
                if (GameInterfaceInstance.GetDuckDisplay(i) == GameInterface.DuckDisplayType.Missed)
                {
                    missFound = true;
                }
                
                if (missFound)
                {
                    GameInterfaceInstance.SetDuckDisplay(i, GameInterfaceInstance.GetDuckDisplay(i + 1));
                }
            }

            if (missFound)
            {
                GameInterfaceInstance.SetDuckDisplay(9, GameInterface.DuckDisplayType.Missed);
            }

            GlobalContent.point.Play();
            DuckInstance.Call(() => MoveHits(finishedCallback)).After(.3);
        }

        bool HitsMoved()
        {
            var missFound = false;
            for (var i = 0; i < 10; i++)
            {
                if (GameInterfaceInstance.GetDuckDisplay(i) == GameInterface.DuckDisplayType.Missed)
                {
                    missFound = true;
                }
                else if (GameInterfaceInstance.GetDuckDisplay(i) == GameInterface.DuckDisplayType.Hit && missFound)
                {
                    return false;
                }
            }

            return true;
        }

        void CustomInitialize()
		{
            _rnd = new Random();
            SpriteManager.Camera.BackgroundColor = _blue;
            CurrentState = VariableState.StartIntro;
            _state.InitialSpeed = InitialDuckSpeed;
            _state.InitialFlightTime = InitialFlightTime;
            _state.IncludeDuck2 = true;
            _state.Ammo = 3;
            _state.Round = 1;
            _state.DuckFlight = 0;
            _wings.IsLooped = true;
            _quake.IsLooped = true;
            
            //testing
            //CurrentState = VariableState.AnimateEndOfRound;
            //GameInterfaceInstance.SetDuckDisplay(0, GameInterface.DuckDisplayType.Missed);
            //GameInterfaceInstance.SetDuckDisplay(1, GameInterface.DuckDisplayType.Hit);
            //GameInterfaceInstance.SetDuckDisplay(2, GameInterface.DuckDisplayType.Missed);
            //GameInterfaceInstance.SetDuckDisplay(3, GameInterface.DuckDisplayType.Hit);
            //GameInterfaceInstance.SetDuckDisplay(4, GameInterface.DuckDisplayType.Missed);
            //GameInterfaceInstance.SetDuckDisplay(5, GameInterface.DuckDisplayType.Missed);
            //GameInterfaceInstance.SetDuckDisplay(6, GameInterface.DuckDisplayType.Hit);
            //GameInterfaceInstance.SetDuckDisplay(7, GameInterface.DuckDisplayType.Missed);
            //GameInterfaceInstance.SetDuckDisplay(8, GameInterface.DuckDisplayType.Missed);
            //GameInterfaceInstance.SetDuckDisplay(9, GameInterface.DuckDisplayType.Missed);
            //_state.DuckFlight = 10;
		}

		void CustomActivity(bool firstTimeCalled)
		{
            switch (CurrentState)
            {
                case VariableState.StartIntro:
                    if (_state.Round <= 1)
                    {
                        GlobalContent.RoundIntroduction.Play();
                        DogInstance.WalkingSniffingThenDiving(() => CurrentState = VariableState.StartDucks);
                    }
                    else
                    {
                        DogInstance.ShortWalkingSniffingThenDiving(() => CurrentState = VariableState.StartDucks);
                    }
                    GameInterfaceInstance.ShowDialog("ROUND \n\n" + _state.Round);
                    
                    CurrentState = VariableState.Intro;
                    break;
                case VariableState.Intro:
                    break;
                case VariableState.StartDucks:
                    GameInterfaceInstance.HideDialog();
                    {
                        var newGuid = Guid.NewGuid();
                        SetDuck(DuckInstance);
                        if (_state.IncludeDuck2) SetDuck(DuckInstance2);
                    }

                    _doFlyAway = false;
                    CurrentState = VariableState.DucksFlying;
                    _state.Ammo = 3;

                    
                    _wings.Play();
                    _quake.Play();

                    break;
                case VariableState.DucksFlying:
                    bool shot = false;
                    if(_state.Ammo > 0 && GuiManager.Cursor.PrimaryPush)
                    {
                        GlobalContent.ShotSoundEffect.Play();
                        ShotInstance.Shoot(() => { });
                        shot = true;
                        _state.Ammo--;
                    }

                    if (shot)
                    {
                        shot = false;
                        if (!CheckDuckShot(DuckInstance, ScoreInstance, _state.IncludeDuck2 ? 2 : 1))
                        {
                            if (_state.IncludeDuck2)
                            {
                                CheckDuckShot(DuckInstance2, ScoreInstance2, 1);
                            }
                        }

                        if (DuckInstance.IsShot && (!_state.IncludeDuck2 || DuckInstance2.IsShot))
                        {
                            _wings.Stop();
                            _quake.Stop();
                        }
                    }

                    //Duck 1
                    CheckDuck(DuckInstance, _state.IncludeDuck2 ? 2 : 1);

                    //Duck 2
                    if (_state.IncludeDuck2)
                    {
                        CheckDuck(DuckInstance2, 1);
                    }

                    if (DuckInstance.HasFallen && (!_state.IncludeDuck2 || DuckInstance2.HasFallen))
                    {
                        CurrentState = VariableState.PostDucks;
                    }

                    break;
                case VariableState.DucksEscaping:
                    if ((DuckInstance.HasEscaped || DuckInstance.IsShot) && (!_state.IncludeDuck2 || (DuckInstance2.HasEscaped || DuckInstance2.IsShot)))
                    {
                        _wings.Stop();
                        _quake.Stop();
                        CurrentState = VariableState.PostDucks;
                    }

                    if (!_state.IncludeDuck2 || (!DuckInstance.IsShot && !DuckInstance2.IsShot))
                    {
                        SpriteManager.Camera.BackgroundColor = _pink;
                        GameInterfaceInstance.ShowDialog("FLY AWAY");
                    }
                    
                    break;

                case VariableState.PostDucks:
                    GameInterfaceInstance.HideDialog();
                    SpriteManager.Camera.BackgroundColor = _blue;

                    DuckInstance.Velocity = Vector3.Zero;
                    DuckInstance2.Velocity = Vector3.Zero;

                    if(_state.IncludeDuck2)
                    {
                        if(DuckInstance.IsShot && DuckInstance2.IsShot)
                        {
                            DogInstance.TwoDucks(DuckInstance.X, () => CurrentState = VariableState.AnimateEndOfRound);
                        }else if(DuckInstance.IsShot || DuckInstance2.IsShot){
                            DogInstance.OneDuck(DuckInstance.IsShot 
                                                        ? DuckInstance.X
                                                        : DuckInstance2.X,
                                                () => CurrentState = VariableState.AnimateEndOfRound);
                        }else{
                            DogInstance.Laugh(() => CurrentState = VariableState.AnimateEndOfRound);
                        }
                    }else{
                        if(DuckInstance.IsShot)
                        {
                            DogInstance.OneDuck(DuckInstance.X, () => CurrentState = VariableState.AnimateEndOfRound);
                        }else{
                            DogInstance.Laugh(() => CurrentState = VariableState.AnimateEndOfRound);
                        }
                    }

                    CurrentState = VariableState.DogAnimation;

                    break;

                case VariableState.DogAnimation:
                    break;

                case VariableState.AnimateEndOfRound:
                    if (_state.DuckFlight == 10)
                    {
                        CurrentState = VariableState.AnimatingEndOfRound;
                        MoveHits(() => CurrentState = VariableState.CheckEndOfRound);
                    }
                    else
                    {
                        CurrentState = VariableState.StartDucks;
                    }

                    break;
                case VariableState.AnimatingEndOfRound:
                    break;
                case VariableState.CheckEndOfRound:
                    var hits = 0;
                    for (var i = 0; i < 10; i++)
                    {
                        if (GameInterfaceInstance.GetDuckDisplay(i) == GameInterface.DuckDisplayType.Hit)
                        {
                            hits++;
                        }
                    }

                    if (hits > _state.DucksRequiredToAdvance())
                    {
                        CurrentState = VariableState.ContinueAnimation;
                        for (var i = 0; i < 10; i++)
                        {
                            if (GameInterfaceInstance.GetDuckDisplay(i) == GameInterface.DuckDisplayType.Hit)
                            {
                                GameInterfaceInstance.SetDuckDisplay(i, GameInterface.DuckDisplayType.Active);
                            }
                        }
                        GlobalContent.DuckHuntEndofRound.Play();
                        DuckInstance.Call(() =>
                        {
                            var waitTime = 0f;

                            if (hits == 10)
                            {
                                waitTime = 1.2f;
                                GameInterfaceInstance.ShowDialog("PERFECT!! \n\n" + _state.GetBonus());
                                _state.Score += _state.GetBonus();
                                GlobalContent.PerfectScoreSoundEffect.Play();
                                for (var i = 0; i < 10; i++)
                                {
                                    GameInterfaceInstance.SetDuckDisplay(i, GameInterface.DuckDisplayType.Hit);
                                }
                            }

                            DuckInstance.Call(() =>
                                {
                                    GameInterfaceInstance.HideDialog();
                                    CurrentState = VariableState.StartIntro;
                                    _state.Round++;
                                    _state.DuckFlight = 0;
                                    for (var i = 0; i < 10; i++)
                                    {
                                        GameInterfaceInstance.SetDuckDisplay(i, GameInterface.DuckDisplayType.Missed);
                                    }
                                }).After(waitTime);

                            
                        }).After(4.2);
                    }
                    else
                    {
                        GlobalContent.lose.Play();
                        CurrentState = VariableState.Lose;
                        DuckInstance.Call(() =>
                        {
                            GlobalContent.DuckHuntThemeSong.Play();
                            DogInstance.EndGame(() => {
                                MoveToScreen(this.GetType());
                            });
                        }).After(2);
                    }

                    break;
                case VariableState.ContinueAnimation:
                    break;
                case VariableState.Lose:
                    break;

            }

            GameInterfaceInstance.Score = _state.Score;
            GameInterfaceInstance.AvailableShots = _state.Ammo;
            GameInterfaceInstance.Round = _state.Round;
            GameInterfaceInstance.DucksRequiredForRound = _state.DucksRequiredToAdvance();
		}

		void CustomDestroy()
		{


		}

        static void CustomLoadStaticContent(string contentManagerName)
        {


        }

	}
}
