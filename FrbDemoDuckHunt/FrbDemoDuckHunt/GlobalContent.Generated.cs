using System.Collections.Generic;
using System.Threading;
using FlatRedBall;
using FlatRedBall.Math.Geometry;
using FlatRedBall.ManagedSpriteGroups;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Graphics.Particle;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Utilities;
using BitmapFont = FlatRedBall.Graphics.BitmapFont;
using FlatRedBall.Localization;

namespace FrbDemoDuckHunt
{
	public static partial class GlobalContent
	{
		
		public static Microsoft.Xna.Framework.Graphics.Texture2D DogHunt { get; set; }
		public static FlatRedBall.Graphics.BitmapFont DuckHuntFont { get; set; }
		public static Microsoft.Xna.Framework.Graphics.Texture2D DuckHuntFont_0 { get; set; }
		public static FlatRedBall.Graphics.BitmapFont GreenNumbers { get; set; }
		public static FlatRedBall.Graphics.BitmapFont WhiteNumbers { get; set; }
		[System.Obsolete("Use GetFile instead")]
		public static object GetStaticMember (string memberName)
		{
			switch(memberName)
			{
				case  "DogHunt":
					return DogHunt;
				case  "DuckHuntFont":
					return DuckHuntFont;
				case  "DuckHuntFont_0":
					return DuckHuntFont_0;
				case  "GreenNumbers":
					return GreenNumbers;
				case  "WhiteNumbers":
					return WhiteNumbers;
			}
			return null;
		}
		public static object GetFile (string memberName)
		{
			switch(memberName)
			{
				case  "DogHunt":
					return DogHunt;
				case  "DuckHuntFont":
					return DuckHuntFont;
				case  "DuckHuntFont_0":
					return DuckHuntFont_0;
				case  "GreenNumbers":
					return GreenNumbers;
				case  "WhiteNumbers":
					return WhiteNumbers;
			}
			return null;
		}
		public static bool IsInitialized { get; private set; }
		public static bool ShouldStopLoading { get; set; }
		static string ContentManagerName = "Global";
		public static void Initialize ()
		{
			
			DogHunt = FlatRedBallServices.Load<Microsoft.Xna.Framework.Graphics.Texture2D>(@"content/globalcontent/doghunt.png", ContentManagerName);
			DuckHuntFont = FlatRedBallServices.Load<FlatRedBall.Graphics.BitmapFont>(@"content/globalcontent/duckhuntfont.fnt", ContentManagerName);
			DuckHuntFont_0 = FlatRedBallServices.Load<Microsoft.Xna.Framework.Graphics.Texture2D>(@"content/globalcontent/duckhuntfont_0.tga", ContentManagerName);
			GreenNumbers = FlatRedBallServices.Load<FlatRedBall.Graphics.BitmapFont>(@"content/globalcontent/greennumbers.fnt", ContentManagerName);
			WhiteNumbers = FlatRedBallServices.Load<FlatRedBall.Graphics.BitmapFont>(@"content/globalcontent/whitenumbers.fnt", ContentManagerName);
						IsInitialized = true;
		}
		public static void Reload (object whatToReload)
		{
		}
		
		
	}
}
