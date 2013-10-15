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
        private Microsoft.Xna.Framework.Audio.SoundEffectInstance _quack = GlobalContent.duck.CreateInstance();
        private Microsoft.Xna.Framework.Audio.SoundEffectInstance _point = GlobalContent.point.CreateInstance();
        private Microsoft.Xna.Framework.Audio.SoundEffectInstance _duckHuntEndofRound = GlobalContent.DuckHuntEndofRound.CreateInstance();
        private Microsoft.Xna.Framework.Audio.SoundEffectInstance _lose = GlobalContent.lose.CreateInstance();
        private Microsoft.Xna.Framework.Audio.SoundEffectInstance _perfectScoreSoundEffect = GlobalContent.PerfectScoreSoundEffect.CreateInstance();
        private Microsoft.Xna.Framework.Audio.SoundEffectInstance _roundIntroduction = GlobalContent.RoundIntroduction.CreateInstance();
        private Microsoft.Xna.Framework.Audio.SoundEffectInstance _shotSoundEffect1 = GlobalContent.ShotSoundEffect.CreateInstance();
        private Microsoft.Xna.Framework.Audio.SoundEffectInstance _shotSoundEffect2 = GlobalContent.ShotSoundEffect.CreateInstance();
        private Microsoft.Xna.Framework.Audio.SoundEffectInstance _shotSoundEffect3 = GlobalContent.ShotSoundEffect.CreateInstance();
        private Microsoft.Xna.Framework.Audio.SoundEffectInstance _duckHuntThemeSong = GlobalContent.DuckHuntThemeSong.CreateInstance();
        private List<Microsoft.Xna.Framework.Audio.SoundEffectInstance> _sounds = new List<Microsoft.Xna.Framework.Audio.SoundEffectInstance>();

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
                score.X = duck.X;
                score.Y = duck.Y;
                // don't set Z - that's set in Glue

                var val = _state.GetScore(duck.DuckType);
                score.TextInstanceDisplayText = val.ToString();
                _state.Score += val;
                score.Set("Visible").To(true).After(score.DelayAfterShotBeforeShowing);
                score.Set("Visible").To(false).After(score.TimeToShow + score.DelayAfterShotBeforeShowing);

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

            _point.Play();
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
            DuckInstance.Visible = false;
            DuckInstance2.Visible = false;
            ScoreInstance.Visible = false;
            ScoreInstance2.Visible = false;


            _rnd = new Random();
            SpriteManager.Camera.BackgroundColor = _blue;
            CurrentState = VariableState.StartIntro;
            _state.InitialSpeed = InitialDuckSpeed;
            _state.InitialFlightTime = InitialFlightTime;
            _state.IncludeDuck2 = HighScoreStorage.NumOfDucks == 2;
            _state.Ammo = 3;
            _state.Round = 1;
            _state.DuckFlight = 0;
            _wings.IsLooped = true;
            _quack.IsLooped = true;

            _sounds.Add(_wings);
            _sounds.Add(_quack);
            _sounds.Add(_point);
            _sounds.Add(_duckHuntEndofRound);
            _sounds.Add(_lose);
            _sounds.Add(_perfectScoreSoundEffect);
            _sounds.Add(_roundIntroduction);
            _sounds.Add(_shotSoundEffect1);
            _sounds.Add(_shotSoundEffect2);
            _sounds.Add(_shotSoundEffect3);
            _sounds.Add(_duckHuntThemeSong);
            _sounds.Add(DuckInstance.Falling);
            _sounds.Add(DuckInstance2.Falling);

            Game1.OffsetCameraForPixelPerfectRenering();

        }

        void CustomActivity(bool firstTimeCalled)
        {
            if (InputManager.Keyboard.KeyPushed(Keys.Escape))
            {
                if (IsPaused)
                {
                    UnpauseThisScreen();
                    foreach (var snd in _sounds)
                    {
                        if (snd.State == Microsoft.Xna.Framework.Audio.SoundState.Paused)
                            snd.Resume();
                    }
                }
                else
                {
                    PauseThisScreen();
                    foreach (var snd in _sounds)
                    {
                        if(snd.State == Microsoft.Xna.Framework.Audio.SoundState.Playing)
                            snd.Pause();
                    }
                    GlobalContent.pause.Play();
                }
            }

            if (!IsPaused)
            {
                switch (CurrentState)
                {
                    case VariableState.StartIntro:
                        StartIntroActivity();
                        break;
                    case VariableState.Intro:
                        break;
                    case VariableState.StartDucks:
                        StartDucksActivity();

                        break;
                    case VariableState.DucksFlying:
                        DucksFlyingActivity();

                        break;
                    case VariableState.DucksEscaping:
                        DucksEscapingActivity();

                        break;

                    case VariableState.PostDucks:
                        PostDucksActivity();
                        break;

                    case VariableState.DogAnimation:
                        break;

                    case VariableState.AnimateEndOfRound:
                        AnimateEndOfRoundActivity();

                        break;
                    case VariableState.AnimatingEndOfRound:
                        break;
                    case VariableState.CheckEndOfRound:
                        CheckEndOfRoundActivity();
                        break;
                    case VariableState.ContinueAnimation:
                        break;
                    case VariableState.Lose:
                        break;

                }
            }

            GameInterfaceInstance.Score = _state.Score;
            GameInterfaceInstance.AvailableShots = _state.Ammo;
            GameInterfaceInstance.Round = _state.Round;
            GameInterfaceInstance.DucksRequiredForRound = _state.DucksRequiredToAdvance();
        }

        private void CheckEndOfRoundActivity()
        {
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
                        GameInterfaceInstance.SetDuckDisplay(i, GameInterface.DuckDisplayType.Scored);
                    }
                }
                _duckHuntEndofRound.Play();
                DuckInstance.Call(() =>
                {
                    var waitTime = 0f;

                    if (hits == 10)
                    {
                        waitTime = 1.2f;
                        GameInterfaceInstance.ShowDialog("PERFECT!! \n\n" + _state.GetBonus());
                        _state.Score += _state.GetBonus();
                        _perfectScoreSoundEffect.Play();
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
                _lose.Play();
                CurrentState = VariableState.Lose;
                DuckInstance.Call(() =>
                {
                    _duckHuntThemeSong.Play();
                    DogInstance.EndGame(() =>
                    {
                        MoveToScreen(typeof(GameMenu));
                    });
                }).After(2);
            }

        }

        private void AnimateEndOfRoundActivity()
        {
            if (_state.DuckFlight == 10)
            {
                CurrentState = VariableState.AnimatingEndOfRound;
                MoveHits(() => CurrentState = VariableState.CheckEndOfRound);
            }
            else
            {
                CurrentState = VariableState.StartDucks;
            }
        }

        private void PostDucksActivity()
        {
            GameInterfaceInstance.HideDialog();
            SpriteManager.Camera.BackgroundColor = _blue;

            DuckInstance.Velocity = Vector3.Zero;
            DuckInstance2.Velocity = Vector3.Zero;

            if (_state.IncludeDuck2)
            {
                if (DuckInstance.IsShot && DuckInstance2.IsShot)
                {
                    DogInstance.TwoDucks(DuckInstance.X, () => CurrentState = VariableState.AnimateEndOfRound);
                }
                else if (DuckInstance.IsShot || DuckInstance2.IsShot)
                {
                    DogInstance.OneDuck(DuckInstance.IsShot
                                                ? DuckInstance.X
                                                : DuckInstance2.X,
                                        () => CurrentState = VariableState.AnimateEndOfRound);
                }
                else
                {
                    DogInstance.Laugh(() => CurrentState = VariableState.AnimateEndOfRound);
                }
            }
            else
            {
                if (DuckInstance.IsShot)
                {
                    DogInstance.OneDuck(DuckInstance.X, () => CurrentState = VariableState.AnimateEndOfRound);
                }
                else
                {
                    DogInstance.Laugh(() => CurrentState = VariableState.AnimateEndOfRound);
                }
            }

            CurrentState = VariableState.DogAnimation;

        }

        private void DucksEscapingActivity()
        {
            if ((DuckInstance.HasEscaped || DuckInstance.IsShot) && (!_state.IncludeDuck2 || (DuckInstance2.HasEscaped || DuckInstance2.IsShot)))
            {
                _wings.Stop();
                _quack.Stop();
                CurrentState = VariableState.PostDucks;
            }

            if (!_state.IncludeDuck2 || (!DuckInstance.IsShot && !DuckInstance2.IsShot))
            {
                SpriteManager.Camera.BackgroundColor = _pink;
                GameInterfaceInstance.ShowDialog("FLY AWAY");
            }
        }

        private void DucksFlyingActivity()
        {
            bool shot = false;
            if (_state.Ammo > 0 && GuiManager.Cursor.PrimaryPush)
            {
                switch(_state.Ammo)
                {
                    case 3:
                        _shotSoundEffect3.Play();
                        break;
                    case 2:
                        _shotSoundEffect2.Play();
                        break;
                    case 1:
                        _shotSoundEffect1.Play();
                        break;
                }
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
                    _quack.Stop();
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
        }

        private void StartDucksActivity()
        {
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
            _quack.Play();
        }

        private void StartIntroActivity()
        {
            if (_state.Round <= 1)
            {
                _roundIntroduction.Play();
                DogInstance.WalkingSniffingThenDiving(() => CurrentState = VariableState.StartDucks);
            }
            else
            {
                DogInstance.ShortWalkingSniffingThenDiving(() => CurrentState = VariableState.StartDucks);
            }
            GameInterfaceInstance.ShowDialog("ROUND \n\n" + _state.Round, 2);

            CurrentState = VariableState.Intro;
        }

        void CustomDestroy()
        {


        }

        static void CustomLoadStaticContent(string contentManagerName)
        {


        }

    }
}
