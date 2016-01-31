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
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace WindowsGuiTest
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class CMain
	{
		static Form WinShooterMainForm = null;
		static BindingFlags flags = BindingFlags.Public |
			BindingFlags.NonPublic |
			BindingFlags.Static | BindingFlags.Instance;
		static int delay = 1500;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			System.Reflection.Assembly ass = System.Reflection.Assembly.GetExecutingAssembly();
			string exePath = ass.Location;
			for(int i=0;i<4;i++)
			{
				int endSlash = exePath.LastIndexOf("\\");
				exePath = exePath.Substring(0, endSlash);
			}
			exePath += "\\Windows\\bin\\Debug\\WinShooter.exe";


			Console.WriteLine("\r\nStarting WinShooter Application: " + exePath);
			WinShooterMainForm = LaunchApp(exePath, "Allberg.Shooter.Windows.FMain");

			for(int i=15;i>0;i--)
			{
				Console.WriteLine("Waiting for startup " + i.ToString() + " secs...");
				System.Threading.Thread.Sleep(1000);
			}
			
			//
			// Start testing
			//
			Console.WriteLine("Moving form");
			SetFormPropertyValue("Location", new System.Drawing.Point(200,200), ref WinShooterMainForm);
			SetFormPropertyValue("Location", new System.Drawing.Point(500,500), ref WinShooterMainForm);

			//
			// Ending program
			//
			Console.WriteLine("Clicking menu");
			InvokeMethod("menuFileExit", ref WinShooterMainForm, new object[] {null, new EventArgs()} );
		}

		static Form LaunchApp(string exePath, string formName)
		{
			Thread.Sleep(delay);
			Assembly a = Assembly.LoadFrom(exePath);
			Type formType = a.GetType(formName);
			Form resultForm = (Form)a.CreateInstance(formType.FullName);
			Thread t = new Thread(new ThreadStart(new AppState(resultForm).RunApp));
			t.IsBackground = true;
			t.Start();
			return resultForm;
		}

		static void SetFormPropertyValue(string propertyName, object newValue, ref Form testForm)
		{
			if (testForm.InvokeRequired)
			{
				Thread.Sleep(delay);
				testForm.Invoke(
					new SetFormPropertyValueHandler(SetFormPropertyValue),
					new object[]{propertyName, newValue, testForm});
				return;
			}
			Type t = testForm.GetType();
			PropertyInfo pi = t.GetProperty(propertyName);
			pi.SetValue(testForm, newValue, new object[0]);
		}
		delegate void SetFormPropertyValueHandler(
			string propertyName, object newValue, ref Form testForm);

		static void SetControlPropertyValue(
			string controlName, string propertyName, object newValue, ref Form testForm)
		{
			if (testForm.InvokeRequired)
			{
				Thread.Sleep(delay);
				testForm.Invoke(
					new SetControlPropertyValueHandler(SetControlPropertyValue),
					new object[]{controlName, propertyName, newValue});
				return;
			}
			Type t1 = testForm.GetType();
			FieldInfo fi = t1.GetField(controlName, flags);
			object ctrl = fi.GetValue(testForm);
			Type t2 = ctrl.GetType();
			PropertyInfo pi = t2.GetProperty(propertyName, flags);
			pi.SetValue(ctrl, newValue, new object[0]);
		}
		delegate void SetControlPropertyValueHandler(
			string controlName, string propertyName, object newValue, ref Form testForm);

		static void InvokeMethod(string methodName, ref Form testForm, params object[] parms)
		{
			if (testForm.InvokeRequired)
			{
				Thread.Sleep(delay);
				testForm.Invoke(new InvokeMethodHandler(InvokeMethod),
					new object[]{methodName, testForm, parms});
				return;
			}
			Type t = testForm.GetType();
			MethodInfo mi = t.GetMethod(methodName, flags);
			mi.Invoke(testForm, parms);
		}
		delegate void InvokeMethodHandler(
			string methodName, ref Form testForm, params object [] parms);

		private class AppState
		{
			public AppState(Form f) { FormToRun = f; }
			public readonly Form FormToRun;
			public void RunApp()
			{
				Application.Run(FormToRun);
			}
		}


	}
}
