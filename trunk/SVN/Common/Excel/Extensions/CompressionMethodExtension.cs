using System;
using System.Collections.Generic;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using System.Collections;

public static class CompressionMethodExtension
{
    public static bool IsCompressionMethodSupported(this ZipEntry entry)
    {
        if (entry.CompressionMethod != CompressionMethod.BZip2 &&
            entry.CompressionMethod != CompressionMethod.Deflate64 &&
            entry.CompressionMethod != CompressionMethod.Deflated &&
            entry.CompressionMethod != CompressionMethod.Stored &&
            entry.CompressionMethod != CompressionMethod.WinZipAES)
            return false;
        return true;
    }
}

