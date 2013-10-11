using System;
using System.Collections.Generic;
using System.Text;
using FlatRedBall;
using Microsoft.Xna.Framework;

#if !FRB_MDX
using System.Linq;
#endif

namespace FrbDemoDuckHunt
{
	internal static class CameraSetup
	{
			internal static void SetupCamera (Camera cameraToSetUp, GraphicsDeviceManager graphicsDeviceManager)
			{
				cameraToSetUp.UsePixelCoordinates(false, 256, 240);
			}
			internal static void ResetCamera (Camera cameraToReset)
			{
				cameraToReset.X = 0;
				cameraToReset.Y = 0;
				cameraToReset.XVelocity = 0;
				cameraToReset.YVelocity = 0;
			}

	}
}
