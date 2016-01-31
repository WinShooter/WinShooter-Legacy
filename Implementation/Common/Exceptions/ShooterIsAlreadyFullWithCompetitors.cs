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
// $Id$
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Allberg.Shooter.Common
{
	public class ShooterIsAlreadyFullWithCompetitors : System.ApplicationException
	{
		/// <summary>
		/// Initializes a new instance of the ShooterIsAlreadyFullWithCompetitors class.
		/// </summary>
		public ShooterIsAlreadyFullWithCompetitors() : base()
		{
		}


		/// <summary>
		/// Initializes a new instance of the ShooterIsAlreadyFullWithCompetitors class.
		/// </summary>
		/// <param name="message">A message that describes the error.</param>
		public ShooterIsAlreadyFullWithCompetitors(string message) : base(message)
		{
		}

		/// <summary>
		/// To be able to serialize
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected ShooterIsAlreadyFullWithCompetitors(SerializationInfo info, StreamingContext context)
			:base(info,context){}
	}
}

