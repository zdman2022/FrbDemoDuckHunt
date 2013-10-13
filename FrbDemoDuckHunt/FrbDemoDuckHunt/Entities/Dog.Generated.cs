
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
	public partial class Dog : PositionedObject, IDestroyable
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
			Sniffing = 2, 
			Walking = 3, 
			Happy = 4, 
			Jumping = 5, 
			OneDuck = 6, 
			TwoDucks = 7, 
			Laughing = 8
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
					case  VariableState.Sniffing:
						CurrentChain = "Sniffing";
						break;
					case  VariableState.Walking:
						CurrentChain = "Walking";
						break;
					case  VariableState.Happy:
						CurrentChain = "Happy";
						break;
					case  VariableState.Jumping:
						CurrentChain = "Jumping";
						break;
					case  VariableState.OneDuck:
						CurrentChain = "OneDuck";
						break;
					case  VariableState.TwoDucks:
						CurrentChain = "TwoDucks";
						break;
					case  VariableState.Laughing:
						CurrentChain = "Laugh";
						break;
				}
			}
		}
		static object mLockObject = new object();
		static List<string> mRegisteredUnloads = new List<string>();
		static List<string> LoadedContentManagers = new List<string>();
		protected static FlatRedBall.Graphics.Animation.AnimationChainList AnimationChainListFile;
		
		private FlatRedBall.Sprite VisibleInstance;
		public string CurrentChain
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
		public float WalkingSpeed = 20f;
		public float JumpingXSpeed = 15f;
		public float JumpingYSpeed = 75f;
		public float JumpingYDeceleration = -350f;
		public float DogDuckMoveSpeed = 86f;
		public float WalkingStartY = -44f;
		public float WalkingStartX = -95f;
		public float DuckStartY = -60f;
		public float DuckMaxStartX = 40f;
		public float DuckMinStartX = -50f;
		protected Layer LayerProvidedByContainer = null;

        public Dog(string contentManagerName) :
            this(contentManagerName, true)
        {
        }


        public Dog(string contentManagerName, bool addToManagers) :
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
			if (VisibleInstance.Parent == null)
			{
				VisibleInstance.CopyAbsoluteToRelative();
				VisibleInstance.AttachTo(this, false);
			}
			VisibleInstance.PixelSize = 0.5f;
			VisibleInstance.AnimationChains = AnimationChainListFile;
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
			SpriteManager.AddToLayer(VisibleInstance, layerToAddTo);
			VisibleInstance.PixelSize = 0.5f;
			VisibleInstance.AnimationChains = AnimationChainListFile;
			X = oldX;
			Y = oldY;
			Z = oldZ;
			RotationX = oldRotationX;
			RotationY = oldRotationY;
			RotationZ = oldRotationZ;
			CurrentChain = "Walking";
			if (Parent == null)
			{
				X = 0f;
			}
			else
			{
				RelativeX = 0f;
			}
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
			WalkingSpeed = 20f;
			JumpingXSpeed = 15f;
			JumpingYSpeed = 75f;
			JumpingYDeceleration = -350f;
			DogDuckMoveSpeed = 86f;
			WalkingStartY = -44f;
			WalkingStartX = -95f;
			DuckStartY = -60f;
			DuckMaxStartX = 40f;
			DuckMinStartX = -50f;
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
						FlatRedBallServices.GetContentManagerByName(ContentManagerName).AddUnloadMethod("DogStaticUnload", UnloadStaticContent);
						mRegisteredUnloads.Add(ContentManagerName);
					}
				}
				if (!FlatRedBallServices.IsLoaded<FlatRedBall.Graphics.Animation.AnimationChainList>(@"content/entities/dog/animationchainlistfile.achx", ContentManagerName))
				{
				}
				AnimationChainListFile = FlatRedBallServices.Load<FlatRedBall.Graphics.Animation.AnimationChainList>(@"content/entities/dog/animationchainlistfile.achx", ContentManagerName);
			}
			if (registerUnload && ContentManagerName != FlatRedBallServices.GlobalContentManager)
			{
				lock (mLockObject)
				{
					if (!mRegisteredUnloads.Contains(ContentManagerName) && ContentManagerName != FlatRedBallServices.GlobalContentManager)
					{
						FlatRedBallServices.GetContentManagerByName(ContentManagerName).AddUnloadMethod("DogStaticUnload", UnloadStaticContent);
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
				if (AnimationChainListFile != null)
				{
					AnimationChainListFile= null;
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
				case  VariableState.Sniffing:
					break;
				case  VariableState.Walking:
					break;
				case  VariableState.Happy:
					break;
				case  VariableState.Jumping:
					break;
				case  VariableState.OneDuck:
					break;
				case  VariableState.TwoDucks:
					break;
				case  VariableState.Laughing:
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
				case  VariableState.Sniffing:
					break;
				case  VariableState.Walking:
					break;
				case  VariableState.Happy:
					break;
				case  VariableState.Jumping:
					break;
				case  VariableState.OneDuck:
					break;
				case  VariableState.TwoDucks:
					break;
				case  VariableState.Laughing:
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
				case  VariableState.Sniffing:
					if (interpolationValue < 1)
					{
						this.CurrentChain = "Sniffing";
					}
					break;
				case  VariableState.Walking:
					if (interpolationValue < 1)
					{
						this.CurrentChain = "Walking";
					}
					break;
				case  VariableState.Happy:
					if (interpolationValue < 1)
					{
						this.CurrentChain = "Happy";
					}
					break;
				case  VariableState.Jumping:
					if (interpolationValue < 1)
					{
						this.CurrentChain = "Jumping";
					}
					break;
				case  VariableState.OneDuck:
					if (interpolationValue < 1)
					{
						this.CurrentChain = "OneDuck";
					}
					break;
				case  VariableState.TwoDucks:
					if (interpolationValue < 1)
					{
						this.CurrentChain = "TwoDucks";
					}
					break;
				case  VariableState.Laughing:
					if (interpolationValue < 1)
					{
						this.CurrentChain = "Laugh";
					}
					break;
			}
			switch(secondState)
			{
				case  VariableState.Sniffing:
					if (interpolationValue >= 1)
					{
						this.CurrentChain = "Sniffing";
					}
					break;
				case  VariableState.Walking:
					if (interpolationValue >= 1)
					{
						this.CurrentChain = "Walking";
					}
					break;
				case  VariableState.Happy:
					if (interpolationValue >= 1)
					{
						this.CurrentChain = "Happy";
					}
					break;
				case  VariableState.Jumping:
					if (interpolationValue >= 1)
					{
						this.CurrentChain = "Jumping";
					}
					break;
				case  VariableState.OneDuck:
					if (interpolationValue >= 1)
					{
						this.CurrentChain = "OneDuck";
					}
					break;
				case  VariableState.TwoDucks:
					if (interpolationValue >= 1)
					{
						this.CurrentChain = "TwoDucks";
					}
					break;
				case  VariableState.Laughing:
					if (interpolationValue >= 1)
					{
						this.CurrentChain = "Laugh";
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
				case  VariableState.Sniffing:
					{
						object throwaway = "Sniffing";
					}
					break;
				case  VariableState.Walking:
					{
						object throwaway = "Walking";
					}
					break;
				case  VariableState.Happy:
					{
						object throwaway = "Happy";
					}
					break;
				case  VariableState.Jumping:
					{
						object throwaway = "Jumping";
					}
					break;
				case  VariableState.OneDuck:
					{
						object throwaway = "OneDuck";
					}
					break;
				case  VariableState.TwoDucks:
					{
						object throwaway = "TwoDucks";
					}
					break;
				case  VariableState.Laughing:
					{
						object throwaway = "Laugh";
					}
					break;
			}
		}
		[System.Obsolete("Use GetFile instead")]
		public static object GetStaticMember (string memberName)
		{
			switch(memberName)
			{
				case  "AnimationChainListFile":
					return AnimationChainListFile;
			}
			return null;
		}
		public static object GetFile (string memberName)
		{
			switch(memberName)
			{
				case  "AnimationChainListFile":
					return AnimationChainListFile;
			}
			return null;
		}
		object GetMember (string memberName)
		{
			switch(memberName)
			{
				case  "AnimationChainListFile":
					return AnimationChainListFile;
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
	public static class DogExtensionMethods
	{
	}
	
}
