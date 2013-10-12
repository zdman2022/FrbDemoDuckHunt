
using BitmapFont = FlatRedBall.Graphics.BitmapFont;
using Cursor = FlatRedBall.Gui.Cursor;
using GuiManager = FlatRedBall.Gui.GuiManager;
// Generated Usings
using FrbDemoDuckHunt.Screens;
using FlatRedBall.Graphics;
using FlatRedBall.Math;
using FrbDemoDuckHunt.Entities;
using FlatRedBall;
using FlatRedBall.Screens;
using System;
using System.Collections.Generic;
using System.Text;
using FlatRedBall.Math.Geometry;
using FlatRedBall.Graphics.Animation;

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
#endif

#if FRB_XNA && !MONODROID
using Model = Microsoft.Xna.Framework.Graphics.Model;
#endif

namespace FrbDemoDuckHunt.Entities
{
	public partial class Duck : PositionedObject, IDestroyable
	{
        // This is made global so that static lazy-loaded content can access it.
        public static string ContentManagerName
        {
            get;
            set;
        }

		// Generated Fields
		#if DEBUG
		static bool HasBeenLoadedWithGlobalContentManager = false;
		#endif
		public enum VariableState
		{
			Uninitialized = 0, //This exists so that the first set call actually does something
			Unknown = 1, //This exists so that if the entity is actually a child entity and has set a child state, you will get this
			FlyRight = 2, 
			FlyLeft = 3, 
			FallLeft = 4, 
			FallRight = 5, 
			FlyUpLeft = 6, 
			FlyUpRight = 7, 
			HitLeft = 8, 
			HitRight = 9
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
					case  VariableState.FlyRight:
						VisibleInstanceCurrentChainName = "FlyRight";
						break;
					case  VariableState.FlyLeft:
						VisibleInstanceCurrentChainName = "FlyLeft";
						break;
					case  VariableState.FallLeft:
						VisibleInstanceCurrentChainName = "FallLeft";
						break;
					case  VariableState.FallRight:
						VisibleInstanceCurrentChainName = "FallRight";
						break;
					case  VariableState.FlyUpLeft:
						VisibleInstanceCurrentChainName = "FlyUpLeft";
						break;
					case  VariableState.FlyUpRight:
						VisibleInstanceCurrentChainName = "FlyUpRight";
						break;
					case  VariableState.HitLeft:
						VisibleInstanceCurrentChainName = "HitLeft";
						break;
					case  VariableState.HitRight:
						VisibleInstanceCurrentChainName = "HitRight";
						break;
				}
			}
		}
		static object mLockObject = new object();
		static List<string> mRegisteredUnloads = new List<string>();
		static List<string> LoadedContentManagers = new List<string>();
		protected static FlatRedBall.Graphics.Animation.AnimationChainList AnimationChainListFileBlack;
		protected static FlatRedBall.Graphics.Animation.AnimationChainList AnimationChainListFileBlue;
		protected static FlatRedBall.Graphics.Animation.AnimationChainList AnimationChainListFileRed;
		
		private FlatRedBall.Math.Geometry.Circle CollisionCircle;
		private FlatRedBall.Sprite VisibleInstance;
		public string VisibleInstanceCurrentChainName
		{
			get
			{
				return VisibleInstance.CurrentChainName;
			}
			set
			{
				VisibleInstance.CurrentChainName = value;
			}
		}
		public bool Visible
		{
			get
			{
				return VisibleInstance.Visible;
			}
			set
			{
				VisibleInstance.Visible = value;
			}
		}
		protected Layer LayerProvidedByContainer = null;

        public Duck(string contentManagerName) :
            this(contentManagerName, true)
        {
        }


        public Duck(string contentManagerName, bool addToManagers) :
			base()
		{
			// Don't delete this:
            ContentManagerName = contentManagerName;
            InitializeEntity(addToManagers);

		}

		protected virtual void InitializeEntity(bool addToManagers)
		{
			// Generated Initialize
			LoadStaticContent(ContentManagerName);
			CollisionCircle = new FlatRedBall.Math.Geometry.Circle();
			VisibleInstance = new FlatRedBall.Sprite();
			
			PostInitialize();
			if (addToManagers)
			{
				AddToManagers(null);
			}


		}

// Generated AddToManagers
		public virtual void AddToManagers (Layer layerToAddTo)
		{
			LayerProvidedByContainer = layerToAddTo;
			SpriteManager.AddPositionedObject(this);
			AddToManagersBottomUp(layerToAddTo);
			CustomInitialize();
		}

		public virtual void Activity()
		{
			// Generated Activity
			
			CustomActivity();
			
			// After Custom Activity
		}

		public virtual void Destroy()
		{
			// Generated Destroy
			SpriteManager.RemovePositionedObject(this);
			
			if (CollisionCircle != null)
			{
				CollisionCircle.Detach(); ShapeManager.Remove(CollisionCircle);
			}
			if (VisibleInstance != null)
			{
				VisibleInstance.Detach(); SpriteManager.RemoveSprite(VisibleInstance);
			}


			CustomDestroy();
		}

		// Generated Methods
		public virtual void PostInitialize ()
		{
			bool oldShapeManagerSuppressAdd = FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue;
			FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue = true;
			if (CollisionCircle.Parent == null)
			{
				CollisionCircle.CopyAbsoluteToRelative();
				CollisionCircle.AttachTo(this, false);
			}
			CollisionCircle.Radius = 16f;
			if (VisibleInstance.Parent == null)
			{
				VisibleInstance.CopyAbsoluteToRelative();
				VisibleInstance.AttachTo(this, false);
			}
			VisibleInstance.AnimationChains = AnimationChainListFileBlack;
			VisibleInstance.CurrentChainName = "FlyRight";
			VisibleInstance.PixelSize = 0.4f;
			if (VisibleInstance.Parent == null)
			{
				VisibleInstance.Z = -1f;
			}
			else
			{
				VisibleInstance.RelativeZ = -1f;
			}
			VisibleInstance.UseAnimationRelativePosition = false;
			FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue = oldShapeManagerSuppressAdd;
		}
		public virtual void AddToManagersBottomUp (Layer layerToAddTo)
		{
			// We move this back to the origin and unrotate it so that anything attached to it can just use its absolute position
			float oldRotationX = RotationX;
			float oldRotationY = RotationY;
			float oldRotationZ = RotationZ;
			
			float oldX = X;
			float oldY = Y;
			float oldZ = Z;
			
			X = 0;
			Y = 0;
			Z = 0;
			RotationX = 0;
			RotationY = 0;
			RotationZ = 0;
			ShapeManager.AddToLayer(CollisionCircle, layerToAddTo);
			CollisionCircle.Radius = 16f;
			SpriteManager.AddToLayer(VisibleInstance, layerToAddTo);
			VisibleInstance.AnimationChains = AnimationChainListFileBlack;
			VisibleInstance.CurrentChainName = "FlyRight";
			VisibleInstance.PixelSize = 0.4f;
			if (VisibleInstance.Parent == null)
			{
				VisibleInstance.Z = -1f;
			}
			else
			{
				VisibleInstance.RelativeZ = -1f;
			}
			VisibleInstance.UseAnimationRelativePosition = false;
			X = oldX;
			Y = oldY;
			Z = oldZ;
			RotationX = oldRotationX;
			RotationY = oldRotationY;
			RotationZ = oldRotationZ;
			VisibleInstanceCurrentChainName = "FlyRight";
			if (Parent == null)
			{
				Y = 0f;
			}
			else
			{
				RelativeY = 0f;
			}
			if (Parent == null)
			{
				X = 0f;
			}
			else
			{
				RelativeX = 0f;
			}
			Visible = true;
			if (Parent == null)
			{
				Z = 0f;
			}
			else if (Parent is Camera)
			{
				RelativeZ = 0f - 40.0f;
			}
			else
			{
				RelativeZ = 0f;
			}
		}
		public virtual void ConvertToManuallyUpdated ()
		{
			this.ForceUpdateDependenciesDeep();
			SpriteManager.ConvertToManuallyUpdated(this);
			SpriteManager.ConvertToManuallyUpdated(VisibleInstance);
		}
		public static void LoadStaticContent (string contentManagerName)
		{
			if (string.IsNullOrEmpty(contentManagerName))
			{
				throw new ArgumentException("contentManagerName cannot be empty or null");
			}
			ContentManagerName = contentManagerName;
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
			bool registerUnload = false;
			if (LoadedContentManagers.Contains(contentManagerName) == false)
			{
				LoadedContentManagers.Add(contentManagerName);
				lock (mLockObject)
				{
					if (!mRegisteredUnloads.Contains(ContentManagerName) && ContentManagerName != FlatRedBallServices.GlobalContentManager)
					{
						FlatRedBallServices.GetContentManagerByName(ContentManagerName).AddUnloadMethod("DuckStaticUnload", UnloadStaticContent);
						mRegisteredUnloads.Add(ContentManagerName);
					}
				}
				if (!FlatRedBallServices.IsLoaded<FlatRedBall.Graphics.Animation.AnimationChainList>(@"content/entities/duck/animationchainlistfileblack.achx", ContentManagerName))
				{
					registerUnload = true;
				}
				AnimationChainListFileBlack = FlatRedBallServices.Load<FlatRedBall.Graphics.Animation.AnimationChainList>(@"content/entities/duck/animationchainlistfileblack.achx", ContentManagerName);
				if (!FlatRedBallServices.IsLoaded<FlatRedBall.Graphics.Animation.AnimationChainList>(@"content/entities/duck/animationchainlistfileblue.achx", ContentManagerName))
				{
					registerUnload = true;
				}
				AnimationChainListFileBlue = FlatRedBallServices.Load<FlatRedBall.Graphics.Animation.AnimationChainList>(@"content/entities/duck/animationchainlistfileblue.achx", ContentManagerName);
				if (!FlatRedBallServices.IsLoaded<FlatRedBall.Graphics.Animation.AnimationChainList>(@"content/entities/duck/animationchainlistfilered.achx", ContentManagerName))
				{
					registerUnload = true;
				}
				AnimationChainListFileRed = FlatRedBallServices.Load<FlatRedBall.Graphics.Animation.AnimationChainList>(@"content/entities/duck/animationchainlistfilered.achx", ContentManagerName);
			}
			if (registerUnload && ContentManagerName != FlatRedBallServices.GlobalContentManager)
			{
				lock (mLockObject)
				{
					if (!mRegisteredUnloads.Contains(ContentManagerName) && ContentManagerName != FlatRedBallServices.GlobalContentManager)
					{
						FlatRedBallServices.GetContentManagerByName(ContentManagerName).AddUnloadMethod("DuckStaticUnload", UnloadStaticContent);
						mRegisteredUnloads.Add(ContentManagerName);
					}
				}
			}
			CustomLoadStaticContent(contentManagerName);
		}
		public static void UnloadStaticContent ()
		{
			if (LoadedContentManagers.Count != 0)
			{
				LoadedContentManagers.RemoveAt(0);
				mRegisteredUnloads.RemoveAt(0);
			}
			if (LoadedContentManagers.Count == 0)
			{
				if (AnimationChainListFileBlack != null)
				{
					AnimationChainListFileBlack= null;
				}
				if (AnimationChainListFileBlue != null)
				{
					AnimationChainListFileBlue= null;
				}
				if (AnimationChainListFileRed != null)
				{
					AnimationChainListFileRed= null;
				}
			}
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
				case  VariableState.FlyRight:
					break;
				case  VariableState.FlyLeft:
					break;
				case  VariableState.FallLeft:
					break;
				case  VariableState.FallRight:
					break;
				case  VariableState.FlyUpLeft:
					break;
				case  VariableState.FlyUpRight:
					break;
				case  VariableState.HitLeft:
					break;
				case  VariableState.HitRight:
					break;
			}
			var instruction = new FlatRedBall.Instructions.DelegateInstruction<VariableState>(StopStateInterpolation, stateToInterpolateTo);
			instruction.TimeToExecute = FlatRedBall.TimeManager.CurrentTime + secondsToTake;
			this.Instructions.Add(instruction);
			return instruction;
		}
		public void StopStateInterpolation (VariableState stateToStop)
		{
			switch(stateToStop)
			{
				case  VariableState.FlyRight:
					break;
				case  VariableState.FlyLeft:
					break;
				case  VariableState.FallLeft:
					break;
				case  VariableState.FallRight:
					break;
				case  VariableState.FlyUpLeft:
					break;
				case  VariableState.FlyUpRight:
					break;
				case  VariableState.HitLeft:
					break;
				case  VariableState.HitRight:
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
				case  VariableState.FlyRight:
					if (interpolationValue < 1)
					{
						this.VisibleInstanceCurrentChainName = "FlyRight";
					}
					break;
				case  VariableState.FlyLeft:
					if (interpolationValue < 1)
					{
						this.VisibleInstanceCurrentChainName = "FlyLeft";
					}
					break;
				case  VariableState.FallLeft:
					if (interpolationValue < 1)
					{
						this.VisibleInstanceCurrentChainName = "FallLeft";
					}
					break;
				case  VariableState.FallRight:
					if (interpolationValue < 1)
					{
						this.VisibleInstanceCurrentChainName = "FallRight";
					}
					break;
				case  VariableState.FlyUpLeft:
					if (interpolationValue < 1)
					{
						this.VisibleInstanceCurrentChainName = "FlyUpLeft";
					}
					break;
				case  VariableState.FlyUpRight:
					if (interpolationValue < 1)
					{
						this.VisibleInstanceCurrentChainName = "FlyUpRight";
					}
					break;
				case  VariableState.HitLeft:
					if (interpolationValue < 1)
					{
						this.VisibleInstanceCurrentChainName = "HitLeft";
					}
					break;
				case  VariableState.HitRight:
					if (interpolationValue < 1)
					{
						this.VisibleInstanceCurrentChainName = "HitRight";
					}
					break;
			}
			switch(secondState)
			{
				case  VariableState.FlyRight:
					if (interpolationValue >= 1)
					{
						this.VisibleInstanceCurrentChainName = "FlyRight";
					}
					break;
				case  VariableState.FlyLeft:
					if (interpolationValue >= 1)
					{
						this.VisibleInstanceCurrentChainName = "FlyLeft";
					}
					break;
				case  VariableState.FallLeft:
					if (interpolationValue >= 1)
					{
						this.VisibleInstanceCurrentChainName = "FallLeft";
					}
					break;
				case  VariableState.FallRight:
					if (interpolationValue >= 1)
					{
						this.VisibleInstanceCurrentChainName = "FallRight";
					}
					break;
				case  VariableState.FlyUpLeft:
					if (interpolationValue >= 1)
					{
						this.VisibleInstanceCurrentChainName = "FlyUpLeft";
					}
					break;
				case  VariableState.FlyUpRight:
					if (interpolationValue >= 1)
					{
						this.VisibleInstanceCurrentChainName = "FlyUpRight";
					}
					break;
				case  VariableState.HitLeft:
					if (interpolationValue >= 1)
					{
						this.VisibleInstanceCurrentChainName = "HitLeft";
					}
					break;
				case  VariableState.HitRight:
					if (interpolationValue >= 1)
					{
						this.VisibleInstanceCurrentChainName = "HitRight";
					}
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
		public static void PreloadStateContent (VariableState state, string contentManagerName)
		{
			ContentManagerName = contentManagerName;
			switch(state)
			{
				case  VariableState.FlyRight:
					{
						object throwaway = "FlyRight";
					}
					break;
				case  VariableState.FlyLeft:
					{
						object throwaway = "FlyLeft";
					}
					break;
				case  VariableState.FallLeft:
					{
						object throwaway = "FallLeft";
					}
					break;
				case  VariableState.FallRight:
					{
						object throwaway = "FallRight";
					}
					break;
				case  VariableState.FlyUpLeft:
					{
						object throwaway = "FlyUpLeft";
					}
					break;
				case  VariableState.FlyUpRight:
					{
						object throwaway = "FlyUpRight";
					}
					break;
				case  VariableState.HitLeft:
					{
						object throwaway = "HitLeft";
					}
					break;
				case  VariableState.HitRight:
					{
						object throwaway = "HitRight";
					}
					break;
			}
		}
		[System.Obsolete("Use GetFile instead")]
		public static object GetStaticMember (string memberName)
		{
			switch(memberName)
			{
				case  "AnimationChainListFileBlack":
					return AnimationChainListFileBlack;
				case  "AnimationChainListFileBlue":
					return AnimationChainListFileBlue;
				case  "AnimationChainListFileRed":
					return AnimationChainListFileRed;
			}
			return null;
		}
		public static object GetFile (string memberName)
		{
			switch(memberName)
			{
				case  "AnimationChainListFileBlack":
					return AnimationChainListFileBlack;
				case  "AnimationChainListFileBlue":
					return AnimationChainListFileBlue;
				case  "AnimationChainListFileRed":
					return AnimationChainListFileRed;
			}
			return null;
		}
		object GetMember (string memberName)
		{
			switch(memberName)
			{
				case  "AnimationChainListFileBlack":
					return AnimationChainListFileBlack;
				case  "AnimationChainListFileBlue":
					return AnimationChainListFileBlue;
				case  "AnimationChainListFileRed":
					return AnimationChainListFileRed;
			}
			return null;
		}
		protected bool mIsPaused;
		public override void Pause (FlatRedBall.Instructions.InstructionList instructions)
		{
			base.Pause(instructions);
			mIsPaused = true;
		}
		public virtual void SetToIgnorePausing ()
		{
			FlatRedBall.Instructions.InstructionManager.IgnorePausingFor(this);
			FlatRedBall.Instructions.InstructionManager.IgnorePausingFor(CollisionCircle);
			FlatRedBall.Instructions.InstructionManager.IgnorePausingFor(VisibleInstance);
		}
		public virtual void MoveToLayer (Layer layerToMoveTo)
		{
			if (LayerProvidedByContainer != null)
			{
				LayerProvidedByContainer.Remove(VisibleInstance);
			}
			SpriteManager.AddToLayer(VisibleInstance, layerToMoveTo);
			LayerProvidedByContainer = layerToMoveTo;
		}

    }
	
	
	// Extra classes
	public static class DuckExtensionMethods
	{
	}
	
}
