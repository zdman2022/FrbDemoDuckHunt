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
using FlatRedBall.Math;

namespace FrbDemoDuckHunt.Screens
{
	public partial class GameScreen : Screen
	{
		// Generated Fields
		#if DEBUG
		static bool HasBeenLoadedWithGlobalContentManager = false;
		#endif
		
		private FrbDemoDuckHunt.Entities.Dog DogInstance;
		private PositionedObjectList<Duck> DuckList;
		private FrbDemoDuckHunt.Entities.Background BackgroundInstance;
		private FrbDemoDuckHunt.Entities.GameInterface GameInterfaceInstance;
		private FrbDemoDuckHunt.Entities.Shot ShotInstance;

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
			DuckList = new PositionedObjectList<Duck>();
			BackgroundInstance = new FrbDemoDuckHunt.Entities.Background(ContentManagerName, false);
			BackgroundInstance.Name = "BackgroundInstance";
			GameInterfaceInstance = new FrbDemoDuckHunt.Entities.GameInterface(ContentManagerName, false);
			GameInterfaceInstance.Name = "GameInterfaceInstance";
			ShotInstance = new FrbDemoDuckHunt.Entities.Shot(ContentManagerName, false);
			ShotInstance.Name = "ShotInstance";
			
			
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
				for (int i = DuckList.Count - 1; i > -1; i--)
				{
					if (i < DuckList.Count)
					{
						// We do the extra if-check because activity could destroy any number of entities
						DuckList[i].Activity();
					}
				}
				BackgroundInstance.Activity();
				GameInterfaceInstance.Activity();
				ShotInstance.Activity();
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
			for (int i = DuckList.Count - 1; i > -1; i--)
			{
				DuckList[i].Destroy();
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
				DogInstance.Z = 0f;
			}
			else
			{
				DogInstance.RelativeZ = 0f;
			}
			if (BackgroundInstance.Parent == null)
			{
				BackgroundInstance.Z = -1f;
			}
			else
			{
				BackgroundInstance.RelativeZ = -1f;
			}
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
				DogInstance.Z = 0f;
			}
			else
			{
				DogInstance.RelativeZ = 0f;
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
		}
		public virtual void ConvertToManuallyUpdated ()
		{
			DogInstance.ConvertToManuallyUpdated();
			for (int i = 0; i < DuckList.Count; i++)
			{
				DuckList[i].ConvertToManuallyUpdated();
			}
			BackgroundInstance.ConvertToManuallyUpdated();
			GameInterfaceInstance.ConvertToManuallyUpdated();
			ShotInstance.ConvertToManuallyUpdated();
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
			CustomLoadStaticContent(contentManagerName);
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
