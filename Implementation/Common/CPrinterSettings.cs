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
// $Id: CPrinterSettings.cs 107 2009-02-01 06:25:33Z smuda $
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization.Formatters.Binary;
using Allberg.Shooter.WinShooterServerRemoting;

namespace Allberg.Shooter.Common
{
	/// <summary>
	/// Summary description for CPrinterSettings.
	/// </summary>
	public class CPrinterSettings : IPrinterSettings
	{
		internal CPrinterSettings(CSettings settings)
		{
			createDefaultLabels();
			readLabelSettings(settings);

			createDefaultPrintouts();
			readPrintoutSettings(settings);


			/*

						SoapFormatter formatter = new SoapFormatter();
						formatter.Serialize(stream, mirrorPrintLabelDocument);
						byte[] mirrorPrintLabelDocumentBytes = stream.ToArray();
						string mirrorPrintLabelDocumentString = 
							Convert.ToBase64String(
							mirrorPrintLabelDocumentBytes);
						CSettings.WriteSetting(
							CSettings.SettingsNameEnum.MirrorPrintLabelDocument, 
							mirrorPrintLabelDocumentString);

						stream = new MemoryStream();			
						formatter.Serialize(stream, stickPrintLabelDocument);
						byte[] stickPrintLabelDocumentBytes = stream.ToArray();
						string stickPrintLabelDocumentString = 
							Convert.ToBase64String(
							stickPrintLabelDocumentBytes);
						CSettings.WriteSetting(
							CSettings.SettingsNameEnum.StickPrintLabelDocument, 
							stickPrintLabelDocumentString);*/
		}

		internal CPrinterSettings(IPrinterSettings newSettings)
		{
			// TODO Set current values from new settings
			throw new ApplicationException("Not implemented yet");
			//SetValuesToRegistry();
		}

		internal void SetValuesToRegistry()
		{
			MemoryStream stream = new MemoryStream();

			// Save mirrorPrintLabel
			//SoapFormatter formatter = new SoapFormatter();
			BinaryFormatter formatter = new BinaryFormatter();
			formatter.Serialize(stream, mirrorPrintLabelDocument);
			byte[] mirrorPrintLabelDocumentBytes = stream.ToArray();
			string mirrorPrintLabelDocumentString = 
				Convert.ToBase64String(
					mirrorPrintLabelDocumentBytes);
			CSettings.Instance.WriteSetting(
				CSettings.SettingsNameEnum.LabelMirrorPrintDocument, 
				mirrorPrintLabelDocumentString);

			// Save resultPrintLabel
			stream = new MemoryStream();
			formatter.Serialize(stream, resultLabelDocument);
			byte[] resultLabelDocumentBytes = stream.ToArray();
			string resultLabelDocumentString =
				Convert.ToBase64String(
					resultLabelDocumentBytes);
			CSettings.Instance.WriteSetting(
				CSettings.SettingsNameEnum.LabelResultPrintDocument,
				resultLabelDocumentString);

			// Save stickPrintLabel
			stream = new MemoryStream();
			formatter.Serialize(stream, stickPrintLabelDocument);
			byte[] stickPrintLabelDocumentBytes = stream.ToArray();
			string stickPrintLabelDocumentString =
				Convert.ToBase64String(
					stickPrintLabelDocumentBytes);
			CSettings.Instance.WriteSetting(
				CSettings.SettingsNameEnum.LabelStickPrintDocument,
				stickPrintLabelDocumentString);

			// Save paperResultDocument
			stream = new MemoryStream();
			formatter.Serialize(stream, paperResultDocument);
			byte[] paperResultDocumentBytes = stream.ToArray();
			string paperResultDocumentString = Convert.ToBase64String(
				paperResultDocumentBytes);
			CSettings.Instance.WriteSetting(
				CSettings.SettingsNameEnum.PaperResultDocument,
				paperResultDocumentString);

			// Save paperResultTeamDocument
			stream = new MemoryStream();
			formatter.Serialize(stream, paperResultTeamDocument);
			byte[] paperResultTeamDocumentBytes = stream.ToArray();
			string paperResultTeamDocumentString = Convert.ToBase64String(
				paperResultTeamDocumentBytes);
			CSettings.Instance.WriteSetting(
				CSettings.SettingsNameEnum.PaperResultTeamDocument,
				paperResultTeamDocumentString);
		}

		#region IPrinterSettings Members
		#region Logo
		private System.Drawing.Image logo = null;
		public System.Drawing.Image Logo
		{
			get
			{
				if (logo == null)
				{
					System.Drawing.Image image =
						CEmbeddedResources.GetEmbeddedPicture(
						"Allberg.Shooter.Common.WinShooterLogga.jpg");
					return image;
				}
				else
				{
					return logo;
				}
			}
			set
			{
				throw new ApplicationException("Not implemented yet");
			}
		}

		private string copyright = "John Allberg";
		public string Copyright
		{
			get
			{
				return copyright;
			}
		}
		#endregion

		#region Labels
		private void createDefaultLabels()
		{
			mirrorPrintLabelDocument = new PrintLabelDocument(
				PrintLabelDocument.PrintLabelDocumentTypeEnum.Avery6150,
				210, 297, 4, 8, 50, 30, 20, 20, 0, 0, "Arial", 10);
			stickPrintLabelDocument = new PrintLabelDocument(
				PrintLabelDocument.PrintLabelDocumentTypeEnum.Avery6150,
				210, 297, 4, 8, 50, 30, 20, 20, 0, 0, "Arial", 10);
			resultLabelDocument = new PrintLabelDocument(
				PrintLabelDocument.PrintLabelDocumentTypeEnum.Avery6150,
				210, 297, 4, 8, 50, 30, 20, 20, 0, 0, "Arial", 10);
		}

		private void readLabelSettings(CSettings settings)
		{
			MemoryStream stream = new MemoryStream();
			//SoapFormatter formatter = new SoapFormatter();
			BinaryFormatter formatter = new BinaryFormatter();

			try
			{
				string MirrorPrintLabelDocumentString =
					settings.ReadSettingString(
					CSettings.SettingsNameEnum.LabelMirrorPrintDocument);
				if (MirrorPrintLabelDocumentString != null &
					MirrorPrintLabelDocumentString != "")
				{
					byte[] MirrorPrintLabelDocumentBytes =
						Convert.FromBase64String(
						MirrorPrintLabelDocumentString);
					stream.Write(MirrorPrintLabelDocumentBytes,
						0,
						MirrorPrintLabelDocumentBytes.Length);
					stream.Seek(0, SeekOrigin.Begin);
					mirrorPrintLabelDocument =
						(PrintLabelDocument)
						formatter.Deserialize(stream);
				}
			}
			catch (Exception exc)
			{
				Trace.WriteLine(
					"CPrinterSettings: Exception occured while reading " +
					"MirrorPrintLabelDocument from Registry: " +
					exc.ToString());
			}

			try
			{
				stream = new MemoryStream();
				string resultPrintLabelDocumentString =
					settings.ReadSettingString(
					CSettings.SettingsNameEnum.LabelResultPrintDocument);
				if (resultPrintLabelDocumentString != null &
					resultPrintLabelDocumentString != "")
				{
					byte[] resultPrintLabelDocumentBytes =
						Convert.FromBase64String(
						resultPrintLabelDocumentString);
					stream.Write(resultPrintLabelDocumentBytes,
						0,
						resultPrintLabelDocumentBytes.Length);
					stream.Seek(0, SeekOrigin.Begin);
					resultLabelDocument =
						(PrintLabelDocument)
						formatter.Deserialize(stream);
				}
			}
			catch (Exception exc)
			{
				Trace.WriteLine(
					"CPrinterSettings: Exception occured while reading " +
					"resultPrintLabelDocument from Registry: " +
					exc.ToString());
			}

			try
			{
				stream = new MemoryStream();
				string stickPrintLabelDocumentString =
					settings.ReadSettingString(
					CSettings.SettingsNameEnum.LabelMirrorPrintDocument);
				if (stickPrintLabelDocumentString != null &
					stickPrintLabelDocumentString != "")
				{
					byte[] stickPrintLabelDocumentBytes =
						Convert.FromBase64String(
						stickPrintLabelDocumentString);
					stream.Write(stickPrintLabelDocumentBytes,
						0,
						stickPrintLabelDocumentBytes.Length);
					stream.Seek(0, SeekOrigin.Begin);
					stickPrintLabelDocument =
						(PrintLabelDocument)
						formatter.Deserialize(stream);
				}
			}
			catch (Exception exc)
			{
				Trace.WriteLine(
					"CPrinterSettings: Exception occured while reading " +
					"stickPrintLabelDocument from Registry: " +
					exc.ToString());
			}
		}

		PrintLabelDocument mirrorPrintLabelDocument;
		[CLSCompliant(false)]
		public PrintLabelDocument LabelMirrorPrintDocument
		{
			get
			{
				return mirrorPrintLabelDocument;
			}
			set
			{
				mirrorPrintLabelDocument = value;
				SetValuesToRegistry();
			}
		}

		PrintLabelDocument resultLabelDocument;
		[CLSCompliant(false)]
		public PrintLabelDocument LabelResultDocument
		{
			get
			{
				return resultLabelDocument;
			}
			set
			{
				resultLabelDocument = value;
				SetValuesToRegistry();
			}
		}

		PrintLabelDocument stickPrintLabelDocument;
		[CLSCompliant(false)]
		public PrintLabelDocument StickPrintLabelDocument
		{
			get
			{
				return stickPrintLabelDocument;
			}
			set
			{
				stickPrintLabelDocument = value;
				SetValuesToRegistry();
			}
		}
		#endregion

		#region Printouts
		private void createDefaultPrintouts()
		{
			paperResultDocument = 
				new PrintDocumentStd(210, 297, 10, 10);
			paperResultDocument.Fonts.Add( new FontInfo( new System.Drawing.Font("Arial", 14,
				System.Drawing.FontStyle.Bold &
				System.Drawing.FontStyle.Underline), "Rubrik")); //printCompetitorHeaderFont - Rubrik
			paperResultDocument.Fonts.Add( new FontInfo(new System.Drawing.Font("Arial", 12,
				System.Drawing.FontStyle.Regular), "Tävlande")); //printCompetitorFont - Tävlande
			paperResultDocument.Fonts.Add( new FontInfo(new System.Drawing.Font("Arial", 8,
				System.Drawing.FontStyle.Regular), "Copyright")); //printAllbergFont- Copyright

			paperResultDocument.Columns.Add(new PrintColumnInfo(8, "Pl"));
			paperResultDocument.Columns.Add(new PrintColumnInfo(47, "Namn"));
			paperResultDocument.Columns.Add(new PrintColumnInfo(50, "Klubb"));
			paperResultDocument.Columns.Add(new PrintColumnInfo(42, "Resultat"));
			paperResultDocument.Columns.Add(new PrintColumnInfo(13, "Tot"));
			paperResultDocument.Columns.Add(new PrintColumnInfo(5, "P"));
			paperResultDocument.Columns.Add(new PrintColumnInfo(15, "Stm"));

			paperResultTeamDocument =
				new PrintDocumentStd(210, 297, 10, 10);
			paperResultTeamDocument.Fonts.Add( new FontInfo( new System.Drawing.Font("Arial", 14,
				System.Drawing.FontStyle.Bold &
				System.Drawing.FontStyle.Underline), "printCompetitorHeaderFont")); //printCompetitorHeaderFont
			paperResultTeamDocument.Fonts.Add( new FontInfo(new System.Drawing.Font("Arial", 12,
				System.Drawing.FontStyle.Regular), "printCompetitorFont")); //printCompetitorFont
			paperResultTeamDocument.Fonts.Add( new FontInfo( new System.Drawing.Font("Arial", 8,
				System.Drawing.FontStyle.Regular), "Copyright")); //printAllbergFont

			paperResultTeamDocument.Columns.Add(new PrintColumnInfo(8, "Pl"));
			paperResultTeamDocument.Columns.Add(new PrintColumnInfo(53, "Klubb"));
			paperResultTeamDocument.Columns.Add(new PrintColumnInfo(45, "Lagnamn"));
			paperResultTeamDocument.Columns.Add(new PrintColumnInfo(50, "Resultat"));
			paperResultTeamDocument.Columns.Add(new PrintColumnInfo(13, "Tot"));
			paperResultTeamDocument.Columns.Add(new PrintColumnInfo(10, "P"));
		}
		private void readPrintoutSettings(CSettings settings)
		{
			MemoryStream stream;
			BinaryFormatter formatter = new BinaryFormatter();

			try
			{
				stream = new MemoryStream();
				string paperResultDocumentString =
					settings.ReadSettingString(
					CSettings.SettingsNameEnum.PaperResultDocument);
				if (paperResultDocumentString != null &
					paperResultDocumentString != "")
				{
					byte[] paperResultDocumentBytes =
						Convert.FromBase64String(
						paperResultDocumentString);
					stream.Write(paperResultDocumentBytes,
						0,
						paperResultDocumentBytes.Length);
					stream.Seek(0, SeekOrigin.Begin);
					paperResultDocument =
						(PrintDocumentStd)
						formatter.Deserialize(stream);
				}
			}
			catch (Exception exc)
			{
				Trace.WriteLine(
					"CPrinterSettings: Exception occured while reading " +
					"PaperResultDocument from Registry: " +
					exc.ToString());
			}

			try
			{
				stream = new MemoryStream();
				string paperResultTeamDocumentString =
					settings.ReadSettingString(
					CSettings.SettingsNameEnum.PaperResultTeamDocument);
				if (paperResultTeamDocumentString != null &&
					paperResultTeamDocumentString != "")
				{
					byte[] paperResultTeamDocumentBytes =
						Convert.FromBase64String(
						paperResultTeamDocumentString);
					stream.Write(paperResultTeamDocumentBytes,
						0,
						paperResultTeamDocumentBytes.Length);
					stream.Seek(0, SeekOrigin.Begin);
					paperResultTeamDocument =
						(PrintDocumentStd)
						formatter.Deserialize(stream);
				}
			}
			catch (Exception exc)
			{
				Trace.WriteLine(
					"CPrinterSettings: Exception occured while reading " +
					"PaperResultDocument from Registry: " +
					exc.ToString());
			}

		}

		PrintDocumentStd paperResultDocument;
		[CLSCompliant(false)]
		public PrintDocumentStd PaperResultDocument
		{
			get
			{
				return paperResultDocument;
			}
			set
			{
				paperResultDocument = value;
				SetValuesToRegistry();
			}
		}

		PrintDocumentStd paperResultTeamDocument;
		[CLSCompliant(false)]
		public PrintDocumentStd PaperResultTeamDocument
		{
			get
			{
				return paperResultTeamDocument;
			}
			set
			{
				paperResultTeamDocument = value;
				SetValuesToRegistry();
			}
		}
		#endregion
		#endregion
	}
}
