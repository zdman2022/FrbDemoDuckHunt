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
using FrbDemoDuckHunt.DataTypes;
using FlatRedBall.IO.Csv;

namespace FrbDemoDuckHunt
{
	public static partial class GlobalContent
	{
		
		public static Microsoft.Xna.Framework.Graphics.Texture2D DogHunt { get; set; }
		public static FlatRedBall.Graphics.BitmapFont GreenNumbers { get; set; }
		public static FlatRedBall.Graphics.BitmapFont WhiteNumbers { get; set; }
		public static Microsoft.Xna.Framework.Graphics.Texture2D dhunttitle { get; set; }
		public static Microsoft.Xna.Framework.Graphics.Texture2D dhunttitlepointer { get; set; }
		public static Dictionary<string, InterfaceConstants> InterfaceConstants { get; set; }
		public static Microsoft.Xna.Framework.Graphics.Texture2D duckhunt2 { get; set; }
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
				case  "InterfaceConstants":
					return InterfaceConstants;
				case  "duckhunt2":
					return duckhunt2;
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
				case  "InterfaceConstants":
					return InterfaceConstants;
				case  "duckhunt2":
					return duckhunt2;
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
			if (InterfaceConstants == null)
			{
				{
					// We put the { and } to limit the scope of oldDelimiter
					char oldDelimiter = CsvFileManager.Delimiter;
					CsvFileManager.Delimiter = ',';
					Dictionary<string, InterfaceConstants> temporaryCsvObject = new Dictionary<string, InterfaceConstants>();
					CsvFileManager.CsvDeserializeDictionary<string, InterfaceConstants>("content/globalcontent/interfaceconstants.csv", temporaryCsvObject);
					CsvFileManager.Delimiter = oldDelimiter;
					InterfaceConstants = temporaryCsvObject;
				}
			}
			duckhunt2 = FlatRedBallServices.Load<Microsoft.Xna.Framework.Graphics.Texture2D>(@"content/globalcontent/duckhunt2.png", ContentManagerName);
						IsInitialized = true;
		}
		public static void Reload (object whatToReload)
		{
			if (whatToReload == InterfaceConstants)
			{
				{
					// We put the { and } to limit the scope of oldDelimiter
					char oldDelimiter = CsvFileManager.Delimiter;
					CsvFileManager.Delimiter = ',';
					InterfaceConstants.Clear();
					CsvFileManager.CsvDeserializeDictionary<string, InterfaceConstants>("content/globalcontent/interfaceconstants.csv", InterfaceConstants);
					CsvFileManager.Delimiter = oldDelimiter;
				}
			}
		}
		
		
	}
}
