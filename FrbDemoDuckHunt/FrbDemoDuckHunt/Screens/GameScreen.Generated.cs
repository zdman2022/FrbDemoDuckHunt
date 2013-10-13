using BitmapFont = FlatRedBall.Graphics.BitmapFont;

using Cursor = FlatRedBall.Gui.Cursor;
using GuiManager = FlatRedBall.Gui.GuiManager;

#if XNA4 || WINDOWS_8
using Color = Microsoft.Xna.Framework.Color;
#elif FRB_MDX
using Color = System.Drawing.Color;
#else
using Color = Microsoft.Xna.Framework.Graphics.Color;
#endif

#if FRB_XNA || SILVERLIGHT
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Vector3 = Microsoft.Xna.Framework.Vector3;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using Microsoft.Xna.Framework.Media;
#endif

// Generated Usings
using FrbDemoDuckHunt.Entities;
using FlatRedBall;
using FlatRedBall.Screens;
using System;
using System.Collections.Generic;
using System.Text;

namespace FrbDemoDuckHunt.Screens
{
	public partial class GameScreen : Screen
	{
		// Generated Fields
		#if DEBUG
		static bool HasBeenLoadedWithGlobalContentManager = false;
		#endif
		public enum VariableState
		{
			Uninitialized = 0, //This exists so that the first set call actually does something
			Unknown = 1, //This exists so that if the entity is actually a child entity and has set a child state, you will get this
			Intro = 2, 
			StartDucks = 3, 
			DucksFlying = 4, 
			DucksEscaping = 5, 
			PostDucks = 6, 
			StartIntro = 7, 
			DogAnimation = 8
		}
		protected int mCurrentState = 0;
		public VariableState CurrentState
		{
			get
			{
				if (Enum.IsDefined(typeof(VariableState), mCurrentState))
				{
					return (VariableState)mCurrentState;
				}
				else
				{
					return VariableState.Unknown;
				}
			}
			set
			{
				mCurrentState = (int)value;
				switch(CurrentState)
				{
					case  VariableState.Uninitialized:
						break;
					case  VariableState.Unknown:
						break;
					case  VariableState.Intro:
						break;
					case  VariableState.StartDucks:
						break;
					case  VariableState.DucksFlying:
						break;
					case  VariableState.DucksEscaping:
						break;
					case  VariableState.PostDucks:
						break;
					case  VariableState.StartIntro:
						break;
					case  VariableState.DogAnimation:
						break;
				}
			}
		}
		
		private FrbDemoDuckHunt.Entities.Dog DogInstance;
		private FrbDemoDuckHunt.Entities.Background BackgroundInstance;
		private FrbDemoDuckHunt.Entities.GameInterface GameInterfaceInstance;
		private FrbDemoDuckHunt.Entities.Shot ShotInstance;
		private FrbDemoDuckHunt.Entities.Duck DuckInstance;
		private FrbDemoDuckHunt.Entities.Duck DuckInstance2;
		private FrbDemoDuckHunt.Entities.Score ScoreInstance;
		private FrbDemoDuckHunt.Entities.Score ScoreInstance2;
		public int MinDuckY = 0;
		public int MaxDuckY = 100;
		public int MinDuckX = -100;
		public int MaxDuckX = 100;
		public float StartDuckY = -60f;
		public float InitialDuckSpeed = 70f;
		public float InitialFlightTime = 8f;

		public GameScreen()
			: base("GameScreen")
		{
		}

        public override void Initialize(bool addToManagers)
        {
			// Generated Initialize
			LoadStaticContent(ContentManagerName);
			DogInstance = new FrbDemoDuckHunt.Entities.Dog(ContentManagerName, false);
			DogInstance.Name = "DogInstance";
			BackgroundInstance = new FrbDemoDuckHunt.Entities.Background(ContentManagerName, false);
			BackgroundInstance.Name = "BackgroundInstance";
			GameInterfaceInstance = new FrbDemoDuckHunt.Entities.GameInterface(ContentManagerName, false);
			GameInterfaceInstance.Name = "GameInterfaceInstance";
			ShotInstance = new FrbDemoDuckHunt.Entities.Shot(ContentManagerName, false);
			ShotInstance.Name = "ShotInstance";
			DuckInstance = new FrbDemoDuckHunt.Entities.Duck(ContentManagerName, false);
			DuckInstance.Name = "DuckInstance";
			DuckInstance2 = new FrbDemoDuckHunt.Entities.Duck(ContentManagerName, false);
			DuckInstance2.Name = "DuckInstance2";
			ScoreInstance = new FrbDemoDuckHunt.Entities.Score(ContentManagerName, false);
			ScoreInstance.Name = "ScoreInstance";
			ScoreInstance2 = new FrbDemoDuckHunt.Entities.Score(ContentManagerName, false);
			ScoreInstance2.Name = "ScoreInstance2";
			
			
			PostInitialize();
			base.Initialize(addToManagers);
			if (addToManagers)
			{
				AddToManagers();
			}

        }
        
// Generated AddToManagers
		public override void AddToManagers ()
		{
			base.AddToManagers();
			AddToManagersBottomUp();
			CustomInitialize();
		}


		public override void Activity(bool firstTimeCalled)
		{
			// Generated Activity
			if (!IsPaused)
			{
				
				DogInstance.Activity();
				BackgroundInstance.Activity();
				GameInterfaceInstance.Activity();
				ShotInstance.Activity();
				DuckInstance.Activity();
				DuckInstance2.Activity();
				ScoreInstance.Activity();
				ScoreInstance2.Activity();
			}
			else
			{
			}
			base.Activity(firstTimeCalled);
			if (!IsActivityFinished)
			{
				CustomActivity(firstTimeCalled);
			}


				// After Custom Activity
				
            
		}

		public override void Destroy()
		{
			// Generated Destroy
			
			if (DogInstance != null)
			{
				DogInstance.Destroy();
				DogInstance.Detach();
			}
			if (BackgroundInstance != null)
			{
				BackgroundInstance.Destroy();
				BackgroundInstance.Detach();
			}
			if (GameInterfaceInstance != null)
			{
				GameInterfaceInstance.Destroy();
				GameInterfaceInstance.Detach();
			}
			if (ShotInstance != null)
			{
				ShotInstance.Destroy();
				ShotInstance.Detach();
			}
			if (DuckInstance != null)
			{
				DuckInstance.Destroy();
				DuckInstance.Detach();
			}
			if (DuckInstance2 != null)
			{
				DuckInstance2.Destroy();
				DuckInstance2.Detach();
			}
			if (ScoreInstance != null)
			{
				ScoreInstance.Destroy();
				ScoreInstance.Detach();
			}
			if (ScoreInstance2 != null)
			{
				ScoreInstance2.Destroy();
				ScoreInstance2.Detach();
			}

			base.Destroy();

			CustomDestroy();

		}

		// Generated Methods
		public virtual void PostInitialize ()
		{
			bool oldShapeManagerSuppressAdd = FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue;
			FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue = true;
			if (DogInstance.Parent == null)
			{
				DogInstance.X = 20f;
			}
			else
			{
				DogInstance.RelativeX = 20f;
			}
			if (DogInstance.Parent == null)
			{
				DogInstance.Y = -60f;
			}
			else
			{
				DogInstance.RelativeY = -60f;
			}
			if (DogInstance.Parent == null)
			{
				DogInstance.Z = -1f;
			}
			else
			{
				DogInstance.RelativeZ = -1f;
			}
			if (BackgroundInstance.Parent == null)
			{
				BackgroundInstance.Z = -1f;
			}
			else
			{
				BackgroundInstance.RelativeZ = -1f;
			}
			DuckInstance.Visible = false;
			if (DuckInstance.Parent == null)
			{
				DuckInstance.Z = -1f;
			}
			else
			{
				DuckInstance.RelativeZ = -1f;
			}
			DuckInstance2.Visible = false;
			if (DuckInstance2.Parent == null)
			{
				DuckInstance2.Z = -1f;
			}
			else
			{
				DuckInstance2.RelativeZ = -1f;
			}
			ScoreInstance.Visible = false;
			ScoreInstance2.Visible = false;
			FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue = oldShapeManagerSuppressAdd;
		}
		public virtual void AddToManagersBottomUp ()
		{
			CameraSetup.ResetCamera(SpriteManager.Camera);
			DogInstance.AddToManagers(mLayer);
			DogInstance.CurrentState = FrbDemoDuckHunt.Entities.Dog.VariableState.OneDuck;
			if (DogInstance.Parent == null)
			{
				DogInstance.X = 20f;
			}
			else
			{
				DogInstance.RelativeX = 20f;
			}
			if (DogInstance.Parent == null)
			{
				DogInstance.Y = -60f;
			}
			else
			{
				DogInstance.RelativeY = -60f;
			}
			if (DogInstance.Parent == null)
			{
				DogInstance.Z = -1f;
			}
			else
			{
				DogInstance.RelativeZ = -1f;
			}
			BackgroundInstance.AddToManagers(mLayer);
			if (BackgroundInstance.Parent == null)
			{
				BackgroundInstance.Z = -1f;
			}
			else
			{
				BackgroundInstance.RelativeZ = -1f;
			}
			GameInterfaceInstance.AddToManagers(mLayer);
			ShotInstance.AddToManagers(mLayer);
			DuckInstance.AddToManagers(mLayer);
			DuckInstance.Visible = false;
			if (DuckInstance.Parent == null)
			{
				DuckInstance.Z = -1f;
			}
			else
			{
				DuckInstance.RelativeZ = -1f;
			}
			DuckInstance2.AddToManagers(mLayer);
			DuckInstance2.Visible = false;
			if (DuckInstance2.Parent == null)
			{
				DuckInstance2.Z = -1f;
			}
			else
			{
				DuckInstance2.RelativeZ = -1f;
			}
			ScoreInstance.AddToManagers(mLayer);
			ScoreInstance.Visible = false;
			ScoreInstance2.AddToManagers(mLayer);
			ScoreInstance2.Visible = false;
			MinDuckY = 0;
			MaxDuckY = 100;
			MinDuckX = -100;
			MaxDuckX = 100;
			StartDuckY = -60f;
			InitialDuckSpeed = 70f;
			InitialFlightTime = 8f;
		}
		public virtual void ConvertToManuallyUpdated ()
		{
			DogInstance.ConvertToManuallyUpdated();
			BackgroundInstance.ConvertToManuallyUpdated();
			GameInterfaceInstance.ConvertToManuallyUpdated();
			ShotInstance.ConvertToManuallyUpdated();
			DuckInstance.ConvertToManuallyUpdated();
			DuckInstance2.ConvertToManuallyUpdated();
			ScoreInstance.ConvertToManuallyUpdated();
			ScoreInstance2.ConvertToManuallyUpdated();
		}
		public static void LoadStaticContent (string contentManagerName)
		{
			if (string.IsNullOrEmpty(contentManagerName))
			{
				throw new ArgumentException("contentManagerName cannot be empty or null");
			}
			#if DEBUG
			if (contentManagerName == FlatRedBallServices.GlobalContentManager)
			{
				HasBeenLoadedWithGlobalContentManager = true;
			}
			else if (HasBeenLoadedWithGlobalContentManager)
			{
				throw new Exception("This type has been loaded with a Global content manager, then loaded with a non-global.  This can lead to a lot of bugs");
			}
			#endif
			FrbDemoDuckHunt.Entities.Dog.LoadStaticContent(contentManagerName);
			FrbDemoDuckHunt.Entities.Background.LoadStaticContent(contentManagerName);
			FrbDemoDuckHunt.Entities.GameInterface.LoadStaticContent(contentManagerName);
			FrbDemoDuckHunt.Entities.Shot.LoadStaticContent(contentManagerName);
			FrbDemoDuckHunt.Entities.Duck.LoadStaticContent(contentManagerName);
			FrbDemoDuckHunt.Entities.Score.LoadStaticContent(contentManagerName);
			CustomLoadStaticContent(contentManagerName);
		}
		static VariableState mLoadingState = VariableState.Uninitialized;
		public static VariableState LoadingState
		{
			get
			{
				return mLoadingState;
			}
			set
			{
				mLoadingState = value;
			}
		}
		public FlatRedBall.Instructions.Instruction InterpolateToState (VariableState stateToInterpolateTo, double secondsToTake)
		{
			switch(stateToInterpolateTo)
			{
				case  VariableState.Intro:
					break;
				case  VariableState.StartDucks:
					break;
				case  VariableState.DucksFlying:
					break;
				case  VariableState.DucksEscaping:
					break;
				case  VariableState.PostDucks:
					break;
				case  VariableState.StartIntro:
					break;
				case  VariableState.DogAnimation:
					break;
			}
			var instruction = new FlatRedBall.Instructions.DelegateInstruction<VariableState>(StopStateInterpolation, stateToInterpolateTo);
			instruction.TimeToExecute = FlatRedBall.TimeManager.CurrentTime + secondsToTake;
			FlatRedBall.Instructions.InstructionManager.Add(instruction);
			return instruction;
		}
		public void StopStateInterpolation (VariableState stateToStop)
		{
			switch(stateToStop)
			{
				case  VariableState.Intro:
					break;
				case  VariableState.StartDucks:
					break;
				case  VariableState.DucksFlying:
					break;
				case  VariableState.DucksEscaping:
					break;
				case  VariableState.PostDucks:
					break;
				case  VariableState.StartIntro:
					break;
				case  VariableState.DogAnimation:
					break;
			}
			CurrentState = stateToStop;
		}
		public void InterpolateBetween (VariableState firstState, VariableState secondState, float interpolationValue)
		{
			#if DEBUG
			if (float.IsNaN(interpolationValue))
			{
				throw new Exception("interpolationValue cannot be NaN");
			}
			#endif
			switch(firstState)
			{
				case  VariableState.Intro:
					break;
				case  VariableState.StartDucks:
					break;
				case  VariableState.DucksFlying:
					break;
				case  VariableState.DucksEscaping:
					break;
				case  VariableState.PostDucks:
					break;
				case  VariableState.StartIntro:
					break;
				case  VariableState.DogAnimation:
					break;
			}
			switch(secondState)
			{
				case  VariableState.Intro:
					break;
				case  VariableState.StartDucks:
					break;
				case  VariableState.DucksFlying:
					break;
				case  VariableState.DucksEscaping:
					break;
				case  VariableState.PostDucks:
					break;
				case  VariableState.StartIntro:
					break;
				case  VariableState.DogAnimation:
					break;
			}
			if (interpolationValue < 1)
			{
				mCurrentState = (int)firstState;
			}
			else
			{
				mCurrentState = (int)secondState;
			}
		}
		public override void MoveToState (int state)
		{
			this.CurrentState = (VariableState)state;
		}
		
		/// <summary>Sets the current state, and pushes that state onto the back stack.</summary>
		public void PushState (VariableState state)
		{
			this.CurrentState = state;
			
			#if !MONOGAME
			ScreenManager.PushStateToStack((int)this.CurrentState);
			#endif
		}
		[System.Obsolete("Use GetFile instead")]
		public static object GetStaticMember (string memberName)
		{
			return null;
		}
		public static object GetFile (string memberName)
		{
			return null;
		}
		object GetMember (string memberName)
		{
			return null;
		}


	}
}
