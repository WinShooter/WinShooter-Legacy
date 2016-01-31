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
// $Id: PrintDocumentStd.cs 128 2011-05-28 17:07:54Z smuda $ 
using System;
using System.Collections.Generic;

namespace Allberg.Shooter.WinShooterServerRemoting
{
	[Serializable]
	public class PrintDocumentStd : PrintDocumentBase
	{
		public PrintDocumentStd(int DocumentSizeXmm, int DocumentSizeYmm, int LeftMarginMm, int TopMarginMm) 
			: base(DocumentSizeXmm, DocumentSizeYmm, LeftMarginMm, TopMarginMm)

		{
		}

		readonly List<FontInfo> _fonts = new List<FontInfo>();
		public List<FontInfo> Fonts
		{
			get
			{
				return _fonts;
			}
		}

		readonly List<PrintColumnInfo> _columns = new List<PrintColumnInfo>();
		public List<PrintColumnInfo> Columns
		{
			get
			{
				UpdateColumnsDpiSizes();
				return _columns;
			}
		}

		private bool _landscape;
		public bool Landscape
		{
			get
			{
				return _landscape;
			}
			set
			{
				if (_landscape != value)
				{
					var temp = documentSizeYmm;
					documentSizeYmm = documentSizeXmm;
					documentSizeXmm = temp;
				}
				_landscape = value;
			}
		}


		private void UpdateColumnsDpiSizes()
		{
			if (documentSizeXPixels <= 0)
				return; // This happens during prg init

			foreach (var info in _columns)
			{
				info.SizeDpi = ConvertXmmToDpi(info.SizeMm);
			}
		}
	}
}
