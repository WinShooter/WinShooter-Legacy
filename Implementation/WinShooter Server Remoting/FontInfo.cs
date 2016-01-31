using System;
using System.Drawing;

namespace Allberg.Shooter.WinShooterServerRemoting
{
	[Serializable]
	public class FontInfo
	{
		public FontInfo(Font printFont, string printName)
		{
			PrintFont = printFont;
			PrintName = printName;
		}

		public Font PrintFont;
		public string PrintName;
	}
}
