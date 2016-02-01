// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrintDocument.cs" company="John Allberg">
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
//   Defines the PrintDocumentBase type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.WinShooterServerRemoting
{
    using System;

    [Serializable]
    public class PrintDocumentBase
    {
        protected int documentSizeXmm;
        protected int documentSizeYmm;
        protected int leftMarginMm;
        protected int topMarginMm;
        protected int documentSizeXPixels = -1;
        protected int documentSizeYPixels = -1;

        public PrintDocumentBase(int DocumentSizeXmm,
            int DocumentSizeYmm,
            int LeftMarginMm,
            int TopMarginMm)
        {
            documentSizeXmm = DocumentSizeXmm;
            documentSizeYmm = DocumentSizeYmm;
            leftMarginMm = LeftMarginMm;
            topMarginMm = TopMarginMm;
        }

        /// <summary>
        /// Document horizontal size in mm
        /// </summary>
        public int DocumentSizeXmm
        {
            get
            {
                return documentSizeXmm;
            }
        }
        /// <summary>
        /// Document vertical size in mm
        /// </summary>
        public int DocumentSizeYmm
        {
            get
            {
                return documentSizeYmm;
            }
        }
        /// <summary>
        /// Document horizontal size in pixels
        /// </summary>
        public int DocumentSizeXPixels
        {
            get
            {
                return documentSizeXPixels;
            }
            set
            {
                documentSizeXPixels = value;
            }
        }
        /// <summary>
        /// Document vertical size in pixels
        /// </summary>
        public int DocumentSizeYPixels
        {
            get
            {
                return documentSizeYPixels;
            }
            set
            {
                documentSizeYPixels = value;
            }
        }
        /// <summary>
        /// Left margin in mm
        /// </summary>
        public int LeftMarginMm
        {
            get
            {
                return leftMarginMm;
            }
        }
        /// <summary>
        /// Top marign in mm
        /// </summary>
        public int TopMarginMm
        {
            get
            {
                return topMarginMm;
            }
        }

        public float ConvertXmmToDpi(int mm)
        {
            return ConvertXmmToDpi((float)mm);
        }
        public float ConvertXmmToDpi(float mm)
        {
            if (documentSizeXPixels <= 0)
                throw new DivideByZeroException("documentSizeXPixels is 0");

            float factor = (float)documentSizeXPixels / (float)documentSizeXmm;
            float toReturn = factor * (float)mm;
            return toReturn;
        }
        public float ConvertYmmToDpi(int mm)
        {
            return ConvertYmmToDpi((float)mm);
        }
        public float ConvertYmmToDpi(float mm)
        {
            if (documentSizeYPixels <= 0)
                throw new DivideByZeroException("documentSizeYPixels is 0");

            float factor = (float)documentSizeYPixels / (float)documentSizeYmm;
            float toReturn = factor * (float)mm;
            return toReturn;
        }

    }
}
