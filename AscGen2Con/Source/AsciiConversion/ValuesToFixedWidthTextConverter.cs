//---------------------------------------------------------------------------------------
// <copyright file="ValuesToFixedWidthTextConverter.cs" company="Jonathan Mathews Software">
//     ASCII Generator dotNET - Image to ASCII Art Conversion Program
//     Copyright (C) 2011 Jonathan Mathews Software. All rights reserved.
// </copyright>
// <author>Jonathan Mathews</author>
// <email>info@jmsoftware.co.uk</email>
// <email>jmsoftware@gmail.com</email>
// <website>http://www.jmsoftware.co.uk/</website>
// <website>http://ascgen2.sourceforge.net/</website>
// <license>
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the license, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see http://www.gnu.org/licenses/.
// </license>
//---------------------------------------------------------------------------------------

using System;

namespace JMSoftware.AsciiConversion
{
    using System.Text;
    public class ValuesToFixedWidthTextConverter : ValuesToTextConverter
    {
        private string _ramp;

        private char[] _valueCharacters;

        public ValuesToFixedWidthTextConverter(string ramp)
        {
            Ramp = ramp;
        }

        public string Ramp
        {
            get
            {
                return _ramp;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new NotSupportedException();
                }

                _ramp = value;

                _valueCharacters = new char[256];

                float length = Ramp.Length - 1;

                for (int x = 0; x < 256; x++)
                {
                    _valueCharacters[x] = _ramp[(int)(x / 255f * length + 0.5f)];
                }
            }
        }

        public override string[] Apply(byte[][] values)
        {
            if (values == null)
            {
                return null;
            }

            int numberOfColumns = values[0].Length;
            int numberOfRows = values.Length;

            string[] result = new string[numberOfRows];

            for (int y = 0; y < numberOfRows; y++)
            {
                StringBuilder builder = new StringBuilder();

                for (int x = 0; x < numberOfColumns; x++)
                {
                    builder.Append(_valueCharacters[values[y][x]]);
                }

                result[y] = builder.ToString();
            }

            return result;
        }
    }
}