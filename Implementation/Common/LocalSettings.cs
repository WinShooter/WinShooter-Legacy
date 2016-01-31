using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace Allberg.Shooter.Common
{
	public class LocalSettings
	{
		internal const string RegistryPlace = @"SOFTWARE\Allberg\WinShooter";
		public static string DataPath
		{
			get
			{
				string result = regRead(Registry.CurrentUser, "DataPath");
				if (result == null)
					return string.Empty;

				return result;
			}
			set
			{
				regWrite(Registry.CurrentUser, "DataPath", value);
			}
		}

		private static string regRead(RegistryKey rk, string valuename)
		{
			RegistryKey winshooterKey = rk.OpenSubKey(RegistryPlace);
			if (winshooterKey == null)
			{
				return null;
			}
			object objvalue = winshooterKey.GetValue(valuename);
			if (objvalue == null || objvalue.GetType() != typeof(string))
			{
				return null;
			}
			return (string)objvalue;
		}

		private static void regWrite(RegistryKey rk, string valuename, string valuecontent)
		{
			RegistryKey winshooterKey = rk.CreateSubKey(RegistryPlace);
			if (winshooterKey == null)
			{
				throw new ApplicationException("ops");
			}
			winshooterKey.SetValue(valuename, valuecontent);
		}
	}
}
