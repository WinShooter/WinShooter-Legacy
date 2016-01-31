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
// $Id: CPrintPrinterTest.cs 128 2011-05-28 17:07:54Z smuda $ 
using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Reflection;
using Allberg.Shooter.Common;
using Allberg.Shooter.WinShooterServerRemoting;

namespace Allberg.Shooter.Windows
{
	/// <summary>
	/// Summary description for CPrintPatrollist.
	/// </summary>
	public class CPrintPrinterTest : PrintDocument 
	{
		internal CPrintPrinterTest(ref Common.Interface newCommon)
			: base()  
		{
			CommonCode = newCommon;
		}

		Common.Interface CommonCode;
		int pageToPrint = 0;

		//Override OnBeginPrint to set up the font we are going to use
		protected override void OnBeginPrint(PrintEventArgs ev)
		{
			base.OnBeginPrint(ev);
		}

		//Override the OnPrintPage to provide the printing logic for the document
		protected override void OnPrintPage(PrintPageEventArgs ev)
		{
			base.OnPrintPage(ev);

			textYPos = 20;
			
			pageToPrint++;
#if DEBUG
			//pageToPrint = 4;
#endif

			switch (pageToPrint)
			{
				case 1:
					printTest01(ev);
					ev.HasMorePages = true;
					return;
				case 2:
					printTest02(ev);
					ev.HasMorePages = true;
					return;
				case 3:
					printTest03(ev);
					ev.HasMorePages = true;
					return;
				case 4:
					printTest04(ev);
					ev.HasMorePages = false;
					return;
			}

			ev.HasMorePages = false;
		}

		float textYPos = 20;
		private void addTextLine(PrintPageEventArgs ev, string text)
		{
			addTextLine(ev, text, new Font("Times New Roman", 12), Brushes.Black );
		}
		private void addTextLine(PrintPageEventArgs ev, string text, Font font, Brush brush)
		{
			ev.Graphics.DrawString(text,
				font,
				brush,
				20,
				textYPos);

			textYPos += ev.Graphics.MeasureString(text, font).Height;
		}

		#region page 1
		private void printTest01(PrintPageEventArgs ev)
		{
			System.Drawing.Pen pen = new Pen(System.Drawing.Color.Black, 1);

			addTextLine(ev,"Test 1: Detta test visar vilka marginaler som skrivaren/skrivardrivrutinen");
			addTextLine(ev,"lägger på automatiskt.");
			addTextLine(ev,"Det ska finnas ett kryss från varje hörn till motstående hörn.");

			ev.Graphics.DrawLine(pen, 0, 0, ev.PageBounds.Width, ev.PageBounds.Height);
			ev.Graphics.DrawLine(pen, ev.PageBounds.Width, 0, 0, ev.PageBounds.Height);
		}
		#endregion

		#region page 2
		private void printTest02(PrintPageEventArgs ev)
		{
			System.Drawing.Pen pen = new Pen(System.Drawing.Color.Black, 1);

			addTextLine(ev,"Test 2: Detta test visar vilka marginaler som WinShooter");
			addTextLine(ev,"lägger på automatiskt.");
			addTextLine(ev,"Det ska finnas ett kryss från varje hörn till motstående hörn.");

			PrintLabelDocument doc = CommonCode.Settings.PrinterSettings.LabelResultDocument;
			doc.DocumentSizeXPixels = ev.PageBounds.Width;
			doc.DocumentSizeYPixels = ev.PageBounds.Height;

			ev.Graphics.DrawLine(pen, 0, 0, 
				doc.ConvertXmmToDpi( doc.DocumentSizeXmm ), 
				doc.ConvertYmmToDpi(doc.DocumentSizeYmm));
			ev.Graphics.DrawLine(pen, 
				doc.ConvertXmmToDpi(doc.DocumentSizeXmm), 0, 
				0, doc.ConvertYmmToDpi(doc.DocumentSizeYmm));
		}
		#endregion

		#region 3
		private void printTest03(PrintPageEventArgs ev)
		{
			PrintLabelDocument doc = CommonCode.Settings.PrinterSettings.LabelResultDocument;
			doc.DocumentSizeXPixels = ev.PageBounds.Width;
			doc.DocumentSizeYPixels = ev.PageBounds.Height;

			addTextLine(ev, "Test 3: Detta test visar hur WinShooter anser att måtten på papperet är.");
			addTextLine(ev, "På papperet finns två linjaler ritade, en horisontell och en vertikal.");
			addTextLine(ev, "Dessa ska stämma överrens med verkligheten och gå från vänster kant och från övre kant.");
			addTextLine(ev, "");
			addTextLine(ev, "Förutsättningen är att papperet är A4, dvs " +
				doc.DocumentSizeXmm.ToString() + " mm x " + doc.DocumentSizeYmm.ToString() + " mm");

			addHorizontalRuler(ev, doc);
			addVerticalRuler(ev, doc);
		}

		private void addHorizontalRuler(PrintPageEventArgs ev, PrintLabelDocument doc)
		{
			System.Drawing.Pen pen = new Pen(System.Drawing.Color.Black, 1);
			Font font = new Font("Times New Roman", 12);
			float yPos = doc.ConvertYmmToDpi(100);

			ev.Graphics.DrawLine(pen, 0, yPos, doc.ConvertXmmToDpi(doc.DocumentSizeXmm - 9), yPos);
			for (int i = 0; i < doc.DocumentSizeXmm - 9; i++)
			{
				float thisMmLineHeight = 5;
				if (i % 10 == 0)
					thisMmLineHeight = thisMmLineHeight * 2;

				ev.Graphics.DrawLine(pen, 
					doc.ConvertXmmToDpi(i), yPos, 
					doc.ConvertXmmToDpi(i), yPos + thisMmLineHeight);

				if (i % 10 == 0)
				{
					ev.Graphics.DrawString(i.ToString(), font, Brushes.Black, 
						doc.ConvertXmmToDpi(i), yPos - 20);
				}
			}
		}

		private void addVerticalRuler(PrintPageEventArgs ev, PrintLabelDocument doc)
		{
			System.Drawing.Pen pen = new Pen(System.Drawing.Color.Black, 1);
			Font font = new Font("Times New Roman", 12);
			float xPos = doc.ConvertXmmToDpi(150);

			ev.Graphics.DrawLine(pen, xPos, 0, xPos, doc.ConvertYmmToDpi(doc.DocumentSizeYmm - 9));
			for (int i = 0; i < doc.DocumentSizeYmm - 9; i++)
			{
				float thisMmLineHeight = 5;
				if (i % 10 == 0)
					thisMmLineHeight = thisMmLineHeight * 2;

				ev.Graphics.DrawLine(pen, 
					xPos, doc.ConvertYmmToDpi(i), 
					xPos + thisMmLineHeight, doc.ConvertYmmToDpi(i));

				if (i % 10 == 0)
				{
					ev.Graphics.DrawString(i.ToString(), font, Brushes.Black,
						xPos + 20, doc.ConvertYmmToDpi(i));
				}
			}
		}
		#endregion

		#region 4
		private void printTest04(PrintPageEventArgs ev)
		{
			PrintLabelDocument doc = CommonCode.Settings.PrinterSettings.LabelResultDocument;
			doc.DocumentSizeXPixels = ev.PageBounds.Width;
			doc.DocumentSizeYPixels = ev.PageBounds.Height;

			addTextLine(ev, "Test 4: Detta test visar hur WinShooter skriver ut resultatetiketter.");
			addTextLine(ev, "På papperet finns två rader med etiketter, fyra stycken på varje rad.");
			addTextLine(ev, "");
			addTextLine(ev, "Förutsättningen är att papperet är A4, dvs " +
				doc.DocumentSizeXmm.ToString() + " mm x " + doc.DocumentSizeYmm.ToString() + " mm.");
			addTextLine(ev, "Varje etikett är " + doc.LabelXSizeMm.ToString() + " x " + 
				doc.LabelYSizeMm.ToString() + " mm.");
			addTextLine(ev, "Första etiketten börjar " + doc.LeftMarginMm + " mm in. " + 
				"Mellan varje etikett finns en marginal på " + doc.HorizontalInnerMarginMm + " i höjd.");

			int startLabel = 1 * doc.NrOfLabelsX;
			int endlabel = doc.NrOfLabelsX * doc.NrOfLabelsY;
			for (int i = startLabel; i < endlabel; i++)
			{
				PrintLabel label = doc.GetLabel(i);
				printLabelBorders(ev, label);
				printCompetitor(ev, label, i - startLabel);
			}
		}

		private void printLabelBorders(PrintPageEventArgs ev,
			PrintLabel label)
		{
			Pen pen = new Pen(Brushes.Black,1);
			ev.Graphics.DrawLine(pen, label.X, label.Y, label.X, label.Y + label.SizeY);
			ev.Graphics.DrawLine(pen, label.X, label.Y + label.SizeY, label.X + label.SizeX, label.Y + label.SizeY);

			ev.Graphics.DrawLine(pen, label.X + label.SizeX, label.Y + label.SizeY, label.X + label.SizeX, label.Y);
			ev.Graphics.DrawLine(pen, label.X + label.SizeX, label.Y, label.X, label.Y);
		}

		private void printCompetitor(PrintPageEventArgs ev,
			PrintLabel label,
			int i)
		{
			Font printFont = new Font("Times New Roman", 12);
			float y = label.Y + label.MarginalTop;
			float x = label.X + label.MarginalLeft;

			string shooterClass = "A" + i.ToString();

			ev.Graphics.DrawString(
				shooterClass,
				printFont, Brushes.Black,
				x, y, new StringFormat());

			float indent = ev.Graphics.MeasureString("RES", printFont).Width;
			x = x + indent;

			ev.Graphics.DrawString(
				"Skytt nr " + i.ToString(),
				printFont, Brushes.Black,
				x, y, new StringFormat());
			y += printFont.Height;

			ev.Graphics.DrawString(
				"PSK Test",
				printFont, Brushes.Black,
				x, y, new StringFormat());
			y += printFont.Height;

			string resultString = "6/12";

			ev.Graphics.DrawString(
				resultString,
				printFont, Brushes.Black,
				x, y, new StringFormat());

			y += printFont.Height;
		}
		#endregion
	}
}
