using System;
using System.Collections.Generic;
using System.Text;

namespace Allberg.AutoUpdater.UpdaterLib
{
	public class HandleVista
	{
		public static bool IsRunningVistaProtectedMode()
		{
			if (!VistaTools.IsReallyVista())
				return false; // It wasn't Vista

			//if (!VistaTools.IsElevated())
			//	return false; // We're elevated, so no protected mode

			TOKEN_ELEVATION_TYPE type = VistaTools.GetElevationType();
			switch (type)
			{
				case TOKEN_ELEVATION_TYPE.TokenElevationTypeDefault:
					return false;
				case TOKEN_ELEVATION_TYPE.TokenElevationTypeFull:
					return false;
				case TOKEN_ELEVATION_TYPE.TokenElevationTypeLimited:
					return true;
				default:
					throw new NotImplementedException();
			}
		}
	}
}
