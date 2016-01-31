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
// $Id: TraceListenerConsole.cs 105 2009-01-29 10:54:00Z smuda $ 
using System;

namespace Allberg.Shooter.Windows
{
	/// <summary>
	/// Summary description for TraceListenerConsole.
	/// </summary>
	public class TraceListenerConsole : System.Diagnostics.TraceListener
	{
		public TraceListenerConsole()
		{
		}

		public override void Write(string toWrite)
		{
			Console.Write(toWrite);
		}

		public override void WriteLine(string toWrite)
		{
			Console.WriteLine(toWrite);
		}
	}
}
