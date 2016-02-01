namespace Allberg.Shooter.Windows.Forms.Wizard
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Summary description for Common.
    /// </summary>
    internal class Common
    {
        private Common()
        {
        }

        internal static System.Drawing.Bitmap GetResourceBitmap(string filename)
        {
            System.IO.StreamReader reader = Common.GetResourceReader(filename);
            System.Drawing.Bitmap toReturn = new System.Drawing.Bitmap(reader.BaseStream);
            reader.Close();
            return toReturn;
        }
        internal static System.IO.StreamReader GetResourceReader(string filename)
        {
            filename = "Allberg.Shooter.Windows.Forms." + filename;
            System.Reflection.Assembly ass;

            // Get local assembly
            ass = System.Reflection.Assembly.GetExecutingAssembly();
#if DEBUG
            foreach(string resource in ass.GetManifestResourceNames())
            {
                Trace.WriteLine("Found object in assembly: " + resource);
            }
#endif
            System.IO.Stream stream = ass.GetManifestResourceStream( filename );
            System.IO.StreamReader reader =
                new System.IO.StreamReader( stream );

            return reader;
        }

    }
}
