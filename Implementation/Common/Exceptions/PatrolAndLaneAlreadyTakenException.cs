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
using System.Runtime.Serialization;

namespace Allberg.Shooter.Common
{
	/// <summary>
    /// PatrolAndLaneAlreadyTakenException occurs when there is another shooter at
    /// the particular Patrol and Lane
	/// </summary>
	[Serializable]
	public class PatrolAndLaneAlreadyTakenException : System.ApplicationException
	{
		/// <summary>
		/// Initializes a new instance of the ApplicationException class.
		/// </summary>
		public PatrolAndLaneAlreadyTakenException() : base()
		{
		}


		/// <summary>
		/// Initializes a new instance of the ApplicationException class.
		/// </summary>
		/// <param name="message">A message that describes the error.</param>
		public PatrolAndLaneAlreadyTakenException(string message) : base(message)
		{
		}

		/// <summary>
		/// To be able to serialize
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected PatrolAndLaneAlreadyTakenException(SerializationInfo info, StreamingContext context)
			:base(info,context){}
	}
}
