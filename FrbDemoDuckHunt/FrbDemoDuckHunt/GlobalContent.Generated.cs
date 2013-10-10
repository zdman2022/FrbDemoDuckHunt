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
		public static FlatRedBall.Graphics.BitmapFont GreenNumbers { get; set; }
		public static FlatRedBall.Graphics.BitmapFont WhiteNumbers { get; set; }
		public static Microsoft.Xna.Framework.Graphics.Texture2D dhunttitle { get; set; }
		public static Microsoft.Xna.Framework.Graphics.Texture2D dhunttitlepointer { get; set; }
		[System.Obsolete("Use GetFile instead")]
		public static object GetStaticMember (string memberName)
		{
			switch(memberName)
			{
				case  "DogHunt":
					return DogHunt;
				case  "GreenNumbers":
					return GreenNumbers;
				case  "WhiteNumbers":
					return WhiteNumbers;
				case  "dhunttitle":
					return dhunttitle;
				case  "dhunttitlepointer":
					return dhunttitlepointer;
			}
			return null;
		}
		public static object GetFile (string memberName)
		{
			switch(memberName)
			{
				case  "DogHunt":
					return DogHunt;
				case  "GreenNumbers":
					return GreenNumbers;
				case  "WhiteNumbers":
					return WhiteNumbers;
				case  "dhunttitle":
					return dhunttitle;
				case  "dhunttitlepointer":
					return dhunttitlepointer;
			}
			return null;
		}
		public static bool IsInitialized { get; private set; }
		public static bool ShouldStopLoading { get; set; }
		static string ContentManagerName = "Global";
		public static void Initialize ()
		{
			
			DogHunt = FlatRedBallServices.Load<Microsoft.Xna.Framework.Graphics.Texture2D>(@"content/globalcontent/doghunt.png", ContentManagerName);
			GreenNumbers = FlatRedBallServices.Load<FlatRedBall.Graphics.BitmapFont>(@"content/globalcontent/greennumbers.fnt", ContentManagerName);
			WhiteNumbers = FlatRedBallServices.Load<FlatRedBall.Graphics.BitmapFont>(@"content/globalcontent/whitenumbers.fnt", ContentManagerName);
			dhunttitle = FlatRedBallServices.Load<Microsoft.Xna.Framework.Graphics.Texture2D>(@"content/globalcontent/dhunttitle.png", ContentManagerName);
			dhunttitlepointer = FlatRedBallServices.Load<Microsoft.Xna.Framework.Graphics.Texture2D>(@"content/globalcontent/dhunttitlepointer.png", ContentManagerName);
						IsInitialized = true;
		}
		public static void Reload (object whatToReload)
		{
		}
		
		
	}
}
