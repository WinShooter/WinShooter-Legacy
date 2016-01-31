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
using System;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;

namespace Allberg.Shooter.Windows.Forms.Wizard
{
	/// <summary>
	/// Summary description for WizardPageDesigner.
	/// </summary>
	public class WizardPageDesigner : ParentControlDesigner
	{
	
	
		public override DesignerVerbCollection Verbs
		{
			get
			{
				DesignerVerbCollection verbs = new DesignerVerbCollection();
				verbs.Add(new DesignerVerb("Remove Page", new EventHandler(handleRemovePage)));

				return verbs;
			}
		}

		private void handleRemovePage(object sender, EventArgs e)
		{			
			WizardPage page = this.Control as WizardPage;

			IDesignerHost h  = (IDesignerHost) GetService(typeof(IDesignerHost));
			IComponentChangeService c = (IComponentChangeService) GetService(typeof (IComponentChangeService));

			DesignerTransaction dt = h.CreateTransaction("Remove Page");
			
			if (page.Parent is Wizard)
			{
				Wizard wiz = page.Parent as Wizard;

				c.OnComponentChanging(wiz, null);
				//Drop from wizard
				wiz.Pages.Remove(page);
				wiz.Controls.Remove(page);
				c.OnComponentChanged(wiz, null, null, null);
				h.DestroyComponent(page);
			}
			else
			{
				c.OnComponentChanging(page, null);
				//Mark for destruction
				page.Dispose();
				c.OnComponentChanged(page, null, null, null);
			}
			dt.Commit();
		}

	}
}
