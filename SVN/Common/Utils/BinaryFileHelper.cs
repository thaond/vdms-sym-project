using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace VDMS.II.Common.Utils
{
    public class BinaryFileHelper
    {
        public static byte[] ReadFully(Stream stream)
        {
            byte[] buffer = new byte[32768];
            using (MemoryStream ms = new MemoryStream())
            {
                while (true)
                {
                    int read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                        return ms.ToArray();
                    ms.Write(buffer, 0, read);
                }
            }
        }

        public static void SaveFile(byte[] data, string fullFileName)
        {
            BinaryFileHelper.SaveFile(data, fullFileName, FileMode.Create);
        }
        public static void SaveNewFile(byte[] data, string fullFileName)
        {
            BinaryFileHelper.SaveFile(data, fullFileName, FileMode.CreateNew);
        }
        public static void SaveFile(byte[] data, string fullFileName, FileMode fileMode)
        {
            FileStream stream = new FileStream(fullFileName, fileMode);
            BinaryWriter w = new BinaryWriter(stream);
            w.Write(data);
            w.Close();
            stream.Close();
        }

        public static byte[] ReadFile(string fullFileName)
        {
            var fs = new FileStream(fullFileName, FileMode.Open, FileAccess.Read);
            return ReadFully(fs);
        }
    }
}
