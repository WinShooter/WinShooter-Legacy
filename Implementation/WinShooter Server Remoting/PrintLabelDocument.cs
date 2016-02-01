// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrintLabelDocument.cs" company="John Allberg">
//   Copyright ©2001-2016 John Allberg
//   
//   This program is free software; you can redistribute it and/or
//   modify it under the terms of the GNU General Public License
//   as published by the Free Software Foundation; either version 2
//   of the License, or (at your option) any later version.
//   
//   This program is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY OR FITNESS FOR A PARTICULAR PURPOSE. See the
//   GNU General Public License for more details.
//   
//   You should have received a copy of the GNU General Public License
//   along with this program; if not, write to the Free Software
//   Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
// </copyright>
// <summary>
//   Summary description for PrintLabelDocument.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.WinShooterServerRemoting
{
    using System;

    /// <summary>
    /// Summary description for PrintLabelDocument.
    /// </summary>
    [Serializable]
    public class PrintLabelDocument : PrintDocumentBase
    {
        /// <summary>
        /// PrintLabelDocument prints labels on A4
        /// </summary>
        /// <param name="type"></param>
        /// <param name="DocumentSizeXmm"></param>
        /// <param name="DocumentSizeYmm"></param>
        /// <param name="NrOfLabelsX"></param>
        /// <param name="NrOfLabelsY"></param>
        /// <param name="LabelXSizeMm"></param>
        /// <param name="LabelYSizeMm"></param>
        /// <param name="LeftMarginMm"></param>
        /// <param name="TopMarginMm"></param>
        /// <param name="horizontalInnerMarginMm"></param>
        /// <param name="verticalInnerMarginMm"></param>
        /// <param name="fontName"></param>
        /// <param name="fontSize"></param>
        public PrintLabelDocument(PrintLabelDocumentTypeEnum type,
            int DocumentSizeXmm, 
            int DocumentSizeYmm, 
            int NrOfLabelsX, 
            int NrOfLabelsY, 
            int LabelXSizeMm,
            int LabelYSizeMm,
            int LeftMarginMm, 
            int TopMarginMm,
            int horizontalInnerMarginMm,
            int verticalInnerMarginMm,
            string fontName,
            int fontSize) : 
                base(DocumentSizeXmm,
                    DocumentSizeYmm,
                    LeftMarginMm,
                    TopMarginMm)
        {
            _printLabelDocumentType = type;
            _nrOfLabelsX = NrOfLabelsX;
            _nrOfLabelsY = NrOfLabelsY;
            _labelXSizeMm = LabelXSizeMm;
            _labelYSizeMm = LabelYSizeMm;
            _horizontalInnerMarginMm = horizontalInnerMarginMm;
            _verticalInnerMarginMm = verticalInnerMarginMm;
            _fontName = fontName;
            _fontSize = fontSize;
        }

        //PrintLabelDocumentTypeEnum type = PrintLabelDocumentTypeEnum.Avery6150;
        /// <summary>
        /// Enum to set document type
        /// </summary>
        public enum PrintLabelDocumentTypeEnum
        {
            /// <summary>
            /// Avery6150
            /// </summary>
            Avery6150,
            /// <summary>
            /// Anpassad
            /// </summary>
            Anpassad
        }


        readonly int _nrOfLabelsX;
        readonly int _nrOfLabelsY;
        readonly int _labelXSizeMm;
        readonly int _labelYSizeMm;
        readonly int _horizontalInnerMarginMm;
        readonly int _verticalInnerMarginMm;
        string _fontName = "Arial";
        int _fontSize = 12;



        /// <summary>
        /// Nr of labels horizontal
        /// </summary>
        public int NrOfLabelsX
        {
            get
            {
                return _nrOfLabelsX;
            }
        }
        /// <summary>
        /// Nr of labels vertical
        /// </summary>
        public int NrOfLabelsY
        {
            get
            {
                return _nrOfLabelsY;
            }
        }
        /// <summary>
        /// Size of label horizontal
        /// </summary>
        public int LabelXSizeMm
        {
            get
            {
                return _labelXSizeMm;
            }
        }
        /// <summary>
        /// Size of label vertical
        /// </summary>
        public int LabelYSizeMm
        {
            get
            {
                return _labelYSizeMm;
            }
        }
        /// <summary>
        /// Horizontal inner margin in mm
        /// </summary>
        public int HorizontalInnerMarginMm
        {
            get
            {
                return _horizontalInnerMarginMm;
            }
        }
        /// <summary>
        /// Vertical inner margin in mm
        /// </summary>
        public int VerticalInnerMarginMm
        {
            get
            {
                return _verticalInnerMarginMm;
            }
        }

        PrintLabelDocumentTypeEnum _printLabelDocumentType = PrintLabelDocumentTypeEnum.Anpassad;
        /// <summary>
        /// Type of document
        /// </summary>
        public PrintLabelDocumentTypeEnum PrintLabelDocumentType
        {
            get
            {
                return _printLabelDocumentType;
            }
            set
            {
                _printLabelDocumentType = value;
            }
        }

        /// <summary>
        /// Font name
        /// </summary>
        public string FontName 
        {
            get
            {
                return _fontName;
            }
            set
            {
                _fontName = value;
            }
        }
        /// <summary>
        /// Font size in pixels
        /// </summary>
        public int FontSize
        {
            get
            {
                return _fontSize;
            }
            set
            {
                _fontSize = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="labelCount"></param>
        /// <returns></returns>
        public PrintLabel GetLabel(int labelCount)
        {
            if (labelCount >= (_nrOfLabelsX * _nrOfLabelsY) |
                labelCount < 0)
            {
                throw new PrintLabelDoesNotExistException("Label does not exist");
            }
            int line = labelCount / _nrOfLabelsX;
            int nrInLine = labelCount-(line*_nrOfLabelsX);

            int xInMm = leftMarginMm + nrInLine*(_labelXSizeMm + _horizontalInnerMarginMm);
            int yInMm = topMarginMm + line*(_labelYSizeMm + _verticalInnerMarginMm);
            float x = ConvertXmmToDpi(xInMm);
            float y = ConvertYmmToDpi(yInMm);
            float sizeX = ConvertXmmToDpi(_labelXSizeMm);
            float sizeY = ConvertYmmToDpi(_labelYSizeMm);
            float marginalLeft = ConvertXmmToDpi(leftMarginMm);
            float marginalTop = ConvertYmmToDpi(topMarginMm);

            var toReturn = new PrintLabel(x, y, sizeX, sizeY, marginalLeft, marginalTop);
            return toReturn;
        }

    }
}
