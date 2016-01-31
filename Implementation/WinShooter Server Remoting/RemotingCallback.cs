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
// $Id: RemotingCallback.cs 128 2011-05-28 17:07:54Z smuda $
using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.Remoting.Lifetime;

namespace Allberg.Shooter.WinShooterServerRemoting
{
	/// <summary>
	/// Summary description for RemotingCallback.
	/// </summary>
	[Serializable]
	public class RemotingCallback : MarshalByRefObject 
	{
		/// <summary>
		/// This is to insure that when created as a Singleton, the first instance never dies,
		/// regardless of the time between users.
		/// </summary>
		/// <returns>null</returns>
		public override object InitializeLifetimeService()
		{
			var lease = (ILease)base.InitializeLifetimeService();
			if (lease != null)
			{
				if (lease.CurrentState == LeaseState.Initial)
				{
					lease.InitialLeaseTime = TimeSpan.Zero;
				}
			}
			return lease;
		}

		/// <summary>
		/// 
		/// </summary>
		public event MethodInvoker UpdatedClubEvent;
		/// <summary>
		/// 
		/// </summary>
		public void UpdatedClub()
		{
			try
			{
				UpdatedClubEvent();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(
					"Exception occured in Common.Interface.updatedCompetition:" + 
					exc);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public event MethodInvoker UpdatedCompetitionEvent;
		/// <summary>
		/// 
		/// </summary>
		public void UpdatedCompetition()
		{
			try
			{
				UpdatedCompetitionEvent();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(
					"Exception occured in Common.Interface.updatedCompetition:" + 
					exc);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public event MethodInvoker UpdatedCompetitorEvent;
		/// <summary>
		/// 
		/// </summary>
		public void UpdatedCompetitor()
		{
			try
			{
				UpdatedCompetitorEvent();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(
					"Exception occured in Common.Interface.updatedCompetition:" + 
					exc);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public event MethodInvoker UpdatedCompetitorResultEvent;
		/// <summary>
		/// 
		/// </summary>
		public void UpdatedCompetitorResult()
		{
			try
			{
				UpdatedCompetitorResultEvent();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(
					"Exception occured in Common.Interface.updatedCompetition:" + 
					exc);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public event MethodInvoker UpdatedPatrolEvent;
		/// <summary>
		/// 
		/// </summary>
		public void UpdatedPatrol()
		{
			try
			{
				UpdatedPatrolEvent();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(
					"Exception occured in Common.Interface.updatedCompetition:" + 
					exc);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public event MethodInvoker UpdatedShooterEvent;
		/// <summary>
		/// 
		/// </summary>
		public void UpdatedShooter()
		{
			try
			{
				UpdatedShooterEvent();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(
					"Exception occured in Common.Interface.updatedCompetition:" + 
					exc);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public event MethodInvoker UpdatedStationEvent;
		/// <summary>
		/// 
		/// </summary>
		public void UpdatedStation()
		{
			try
			{
				UpdatedStationEvent();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(
					"Exception occured in Common.Interface.updatedStation:" + 
					exc);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public event MethodInvoker UpdatedWeaponEvent;
		/// <summary>
		/// 
		/// </summary>
		public void UpdatedWeapon()
		{
			try
			{
				UpdatedWeaponEvent();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(
					"Exception occured in Common.Interface.updatedCompetition:" + 
					exc);
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public event MethodInvoker UpdatedTeamEvent;
		/// <summary>
		/// 
		/// </summary>
		public void UpdatedTeam()
		{
			try
			{
				UpdatedTeamEvent();
			}
			catch (Exception exc)
			{
				Trace.WriteLine(
					"Exception occured in Common.Interface.updatedTeam:" +
					exc);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public event MethodInvoker SyncronizeEvent;
		/// <summary>
		/// 
		/// </summary>
		public void Syncronize()
		{
			try
			{
				SyncronizeEvent();
			}
			catch (Exception exc)
			{
				Trace.WriteLine(
					"Exception occured in Common.Interface.Syncronize:" +
					exc);
			}
		}
	}
}
