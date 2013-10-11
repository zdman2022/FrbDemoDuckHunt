
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
	public partial class Background : PositionedObject, IDestroyable
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
		static object mLockObject = new object();
		static List<string> mRegisteredUnloads = new List<string>();
		static List<string> LoadedContentManagers = new List<string>();
		protected static FlatRedBall.Scene SceneFile;
		
		private FlatRedBall.Sprite EntireScene;
		protected Layer LayerProvidedByContainer = null;

        public Background(string contentManagerName) :
            this(contentManagerName, true)
        {
        }


        public Background(string contentManagerName, bool addToManagers) :
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
			EntireScene = SceneFile.Sprites.FindByName("doghunt1").Clone();
			
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
			
			if (EntireScene != null)
			{
				EntireScene.Detach(); SpriteManager.RemoveSprite(EntireScene);
			}


			CustomDestroy();
		}

		// Generated Methods
		public virtual void PostInitialize ()
		{
			bool oldShapeManagerSuppressAdd = FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue;
			FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue = true;
			if (EntireScene.Parent == null)
			{
				EntireScene.CopyAbsoluteToRelative();
				EntireScene.AttachTo(this, false);
			}
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
			SpriteManager.AddToLayer(EntireScene, layerToAddTo);
			X = oldX;
			Y = oldY;
			Z = oldZ;
			RotationX = oldRotationX;
			RotationY = oldRotationY;
			RotationZ = oldRotationZ;
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
			SpriteManager.ConvertToManuallyUpdated(EntireScene);
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
						FlatRedBallServices.GetContentManagerByName(ContentManagerName).AddUnloadMethod("BackgroundStaticUnload", UnloadStaticContent);
						mRegisteredUnloads.Add(ContentManagerName);
					}
				}
				if (!FlatRedBallServices.IsLoaded<FlatRedBall.Scene>(@"content/entities/background/scenefile.scnx", ContentManagerName))
				{
					registerUnload = true;
				}
				SceneFile = FlatRedBallServices.Load<FlatRedBall.Scene>(@"content/entities/background/scenefile.scnx", ContentManagerName);
			}
			if (registerUnload && ContentManagerName != FlatRedBallServices.GlobalContentManager)
			{
				lock (mLockObject)
				{
					if (!mRegisteredUnloads.Contains(ContentManagerName) && ContentManagerName != FlatRedBallServices.GlobalContentManager)
					{
						FlatRedBallServices.GetContentManagerByName(ContentManagerName).AddUnloadMethod("BackgroundStaticUnload", UnloadStaticContent);
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
				if (SceneFile != null)
				{
					SceneFile.RemoveFromManagers(ContentManagerName != "Global");
					SceneFile= null;
				}
			}
		}
		[System.Obsolete("Use GetFile instead")]
		public static object GetStaticMember (string memberName)
		{
			switch(memberName)
			{
				case  "SceneFile":
					return SceneFile;
			}
			return null;
		}
		public static object GetFile (string memberName)
		{
			switch(memberName)
			{
				case  "SceneFile":
					return SceneFile;
			}
			return null;
		}
		object GetMember (string memberName)
		{
			switch(memberName)
			{
				case  "SceneFile":
					return SceneFile;
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
			FlatRedBall.Instructions.InstructionManager.IgnorePausingFor(EntireScene);
		}
		public virtual void MoveToLayer (Layer layerToMoveTo)
		{
			if (LayerProvidedByContainer != null)
			{
				LayerProvidedByContainer.Remove(EntireScene);
			}
			SpriteManager.AddToLayer(EntireScene, layerToMoveTo);
			LayerProvidedByContainer = layerToMoveTo;
		}

    }
	
	
	// Extra classes
	public static class BackgroundExtensionMethods
	{
	}
	
}
