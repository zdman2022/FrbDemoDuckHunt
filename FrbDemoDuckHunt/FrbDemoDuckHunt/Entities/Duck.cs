using System;
using System.Collections.Generic;
using System.Text;
using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Graphics.Particle;

using FlatRedBall.Math.Geometry;
using FlatRedBall.Math.Splines;
using BitmapFont = FlatRedBall.Graphics.BitmapFont;
using Cursor = FlatRedBall.Gui.Cursor;
using GuiManager = FlatRedBall.Gui.GuiManager;

#if FRB_XNA || SILVERLIGHT
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Vector3 = Microsoft.Xna.Framework.Vector3;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;


#endif

namespace FrbDemoDuckHunt.Entities
{
	public partial class Duck
	{
        private Microsoft.Xna.Framework.Audio.SoundEffectInstance falling = GlobalContent.ClayPigeonFalling.CreateInstance();
        private GameState.DuckTypes _duckType;
        public GameState.DuckTypes DuckType {
            get
            {
                return _duckType;
            }
            set
            {
                _duckType = value;

                switch(_duckType)
                {
                    case GameState.DuckTypes.Black:
                        VisibleInstance.AnimationChains = AnimationChainListFileBlack;
                        break;
                    case GameState.DuckTypes.Blue:
                        VisibleInstance.AnimationChains = AnimationChainListFileBlue;
                        break;
                    case GameState.DuckTypes.Red:
                        VisibleInstance.AnimationChains = AnimationChainListFileRed;
                        break;
                }
            }
        }

        private double GetAngle(Vector3 dest, Vector3 src)
        {
            dest.Normalize();
            src.Normalize();

            return Math.Acos(Vector3.Dot(dest, src));
        }

        public void Fall(Action finishedCallback)
        {
            CurrentState = VariableState.Fall;

            var dest = Position - new Vector3(X, FallPointY, Z);
            var timeToHit = Math.Abs(dest.Length() / FallSpeed);
            dest.Normalize();
            Velocity = dest * FallSpeed;
            this.Call(() => {
                falling.Stop();
                GlobalContent.DuckHittingtheGround.Play();
            }).After(timeToHit);
            this.Call(finishedCallback).After(timeToHit);

            falling.Play();
        }

        public void Shot(Action finishedCallback)
        {
            Velocity = Vector3.Zero;
            if (CurrentState == VariableState.FlyLeft || CurrentState == VariableState.FlyUpLeft)
            {
                CurrentState = VariableState.HitLeft;
            }
            else
            {
                CurrentState = VariableState.HitRight;
            }

            this.Call(finishedCallback).After(TimeHit);
        }

        public void FlyAway(Action finishedCallback, float speed)
        {
            if (CurrentState == VariableState.HitLeft || CurrentState == VariableState.HitRight || CurrentState == VariableState.Fall)
                return;

            var direction = new Vector3(X, FlyAwayY, Z) - Position;
            var timeToPoint = direction.Length() / speed;
            direction.Normalize();
            Velocity = direction * speed;
            CurrentState = VariableState.FlyAway;

            this.Call(finishedCallback).After(timeToPoint);
        }

        public void FlyTo(int toX, int toY, float speed, Action finishedCallback)
        {
            if (CurrentState == VariableState.HitLeft || CurrentState == VariableState.HitRight || CurrentState == VariableState.Fall)
                return;

            var direction = new Vector3(toX, toY, Z) - Position;
            var angle = GetAngle(direction, new Vector3(0, 1, Z));
            var timeToPoint = direction.Length() / speed;
            direction.Normalize();
            Velocity = direction * speed;

            if (angle >= 0 && angle < ((3 * Math.PI) / 8))
            {
                if (Velocity.X >= 0)
                {
                    CurrentState = VariableState.FlyUpRight;
                }
                else
                {
                    CurrentState = VariableState.FlyUpLeft;
                }
            }
            else
            {
                if (Velocity.X >= 0)
                {
                    CurrentState = VariableState.FlyRight;
                }
                else
                {
                    CurrentState = VariableState.FlyLeft;
                }
            }

            this.Call(finishedCallback).After(timeToPoint);
        }

		private void CustomInitialize()
		{


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
