#region copyright
/*
Copyright ©2009 John Allberg

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY OR FITNESS FOR A PARTICULAR PURPOSE. See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
*/
#endregion
// $Id: Formatter.cs 105 2009-01-29 10:54:00Z smuda $ 
using System;
using System.Collections.Generic;
using System.Text;

namespace Allberg.Shooter.Common
{
	public class Formatter
	{
		public static string FormatTimeSpan(TimeSpan span)
		{
			StringBuilder toReturn = new StringBuilder();

			if (span.Minutes > 0)
				toReturn.Append(span.Minutes.ToString() + ":");

			toReturn.Append(span.Seconds.ToString() + ".");
			StringBuilder ms = new StringBuilder();
			ms.Append(span.Milliseconds.ToString());
			while (ms.Length < 3)
				ms.Insert(0, '0');
			toReturn.Append(ms.ToString());

			return toReturn.ToString();
		}
	}
}
