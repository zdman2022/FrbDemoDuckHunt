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
		public static Microsoft.Xna.Framework.Media.Song ClayFiringSoundEffect { get; set; }
		public static Microsoft.Xna.Framework.Media.Song ClayPigeonFalling { get; set; }
		public static Microsoft.Xna.Framework.Media.Song ClayPigeonRising { get; set; }
		public static Microsoft.Xna.Framework.Media.Song ClayShootingThemeSong { get; set; }
		public static Microsoft.Xna.Framework.Media.Song DogBarkSoundEffect { get; set; }
		public static Microsoft.Xna.Framework.Media.Song DogLaughingSoundEffect { get; set; }
		public static Microsoft.Xna.Framework.Media.Song DuckHittingtheGround { get; set; }
		public static Microsoft.Xna.Framework.Media.Song DuckHuntEndofRound { get; set; }
		public static Microsoft.Xna.Framework.Media.Song DuckHuntThemeSong { get; set; }
		public static Microsoft.Xna.Framework.Media.Song DuckRelease { get; set; }
		public static Microsoft.Xna.Framework.Media.Song MainThemeSong { get; set; }
		public static Microsoft.Xna.Framework.Media.Song PerfectScoreSoundEffect { get; set; }
		public static Microsoft.Xna.Framework.Media.Song RoundIntroduction { get; set; }
		public static Microsoft.Xna.Framework.Media.Song ShotSoundEffect { get; set; }
		public static Microsoft.Xna.Framework.Media.Song WingFlapSoundEffect { get; set; }
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
				case  "ClayFiringSoundEffect":
					return ClayFiringSoundEffect;
				case  "ClayPigeonFalling":
					return ClayPigeonFalling;
				case  "ClayPigeonRising":
					return ClayPigeonRising;
				case  "ClayShootingThemeSong":
					return ClayShootingThemeSong;
				case  "DogBarkSoundEffect":
					return DogBarkSoundEffect;
				case  "DogLaughingSoundEffect":
					return DogLaughingSoundEffect;
				case  "DuckHittingtheGround":
					return DuckHittingtheGround;
				case  "DuckHuntEndofRound":
					return DuckHuntEndofRound;
				case  "DuckHuntThemeSong":
					return DuckHuntThemeSong;
				case  "DuckRelease":
					return DuckRelease;
				case  "MainThemeSong":
					return MainThemeSong;
				case  "PerfectScoreSoundEffect":
					return PerfectScoreSoundEffect;
				case  "RoundIntroduction":
					return RoundIntroduction;
				case  "ShotSoundEffect":
					return ShotSoundEffect;
				case  "WingFlapSoundEffect":
					return WingFlapSoundEffect;
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
				case  "ClayFiringSoundEffect":
					return ClayFiringSoundEffect;
				case  "ClayPigeonFalling":
					return ClayPigeonFalling;
				case  "ClayPigeonRising":
					return ClayPigeonRising;
				case  "ClayShootingThemeSong":
					return ClayShootingThemeSong;
				case  "DogBarkSoundEffect":
					return DogBarkSoundEffect;
				case  "DogLaughingSoundEffect":
					return DogLaughingSoundEffect;
				case  "DuckHittingtheGround":
					return DuckHittingtheGround;
				case  "DuckHuntEndofRound":
					return DuckHuntEndofRound;
				case  "DuckHuntThemeSong":
					return DuckHuntThemeSong;
				case  "DuckRelease":
					return DuckRelease;
				case  "MainThemeSong":
					return MainThemeSong;
				case  "PerfectScoreSoundEffect":
					return PerfectScoreSoundEffect;
				case  "RoundIntroduction":
					return RoundIntroduction;
				case  "ShotSoundEffect":
					return ShotSoundEffect;
				case  "WingFlapSoundEffect":
					return WingFlapSoundEffect;
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
			ClayFiringSoundEffect = FlatRedBallServices.Load<Microsoft.Xna.Framework.Media.Song>(@"content/globalcontent/clayfiringsoundeffect", ContentManagerName);
			ClayPigeonFalling = FlatRedBallServices.Load<Microsoft.Xna.Framework.Media.Song>(@"content/globalcontent/claypigeonfalling", ContentManagerName);
			ClayPigeonRising = FlatRedBallServices.Load<Microsoft.Xna.Framework.Media.Song>(@"content/globalcontent/claypigeonrising", ContentManagerName);
			ClayShootingThemeSong = FlatRedBallServices.Load<Microsoft.Xna.Framework.Media.Song>(@"content/globalcontent/clayshootingthemesong", ContentManagerName);
			DogBarkSoundEffect = FlatRedBallServices.Load<Microsoft.Xna.Framework.Media.Song>(@"content/globalcontent/dogbarksoundeffect", ContentManagerName);
			DogLaughingSoundEffect = FlatRedBallServices.Load<Microsoft.Xna.Framework.Media.Song>(@"content/globalcontent/doglaughingsoundeffect", ContentManagerName);
			DuckHittingtheGround = FlatRedBallServices.Load<Microsoft.Xna.Framework.Media.Song>(@"content/globalcontent/duckhittingtheground", ContentManagerName);
			DuckHuntEndofRound = FlatRedBallServices.Load<Microsoft.Xna.Framework.Media.Song>(@"content/globalcontent/duckhuntendofround", ContentManagerName);
			DuckHuntThemeSong = FlatRedBallServices.Load<Microsoft.Xna.Framework.Media.Song>(@"content/globalcontent/duckhuntthemesong", ContentManagerName);
			DuckRelease = FlatRedBallServices.Load<Microsoft.Xna.Framework.Media.Song>(@"content/globalcontent/duckrelease", ContentManagerName);
			MainThemeSong = FlatRedBallServices.Load<Microsoft.Xna.Framework.Media.Song>(@"content/globalcontent/mainthemesong", ContentManagerName);
			PerfectScoreSoundEffect = FlatRedBallServices.Load<Microsoft.Xna.Framework.Media.Song>(@"content/globalcontent/perfectscoresoundeffect", ContentManagerName);
			RoundIntroduction = FlatRedBallServices.Load<Microsoft.Xna.Framework.Media.Song>(@"content/globalcontent/roundintroduction", ContentManagerName);
			ShotSoundEffect = FlatRedBallServices.Load<Microsoft.Xna.Framework.Media.Song>(@"content/globalcontent/shotsoundeffect", ContentManagerName);
			WingFlapSoundEffect = FlatRedBallServices.Load<Microsoft.Xna.Framework.Media.Song>(@"content/globalcontent/wingflapsoundeffect", ContentManagerName);
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
