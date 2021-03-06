// ***************************************************************
// <copyright file="TempFileStream.cs" company="Microsoft">
//      Copyright (C) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
//      ...
// </summary>
// ***************************************************************

namespace Microsoft.Exchange.Data.Internal
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.ComponentModel;
    using System.Threading;
    using System.Text;
    using System.Security.Permissions;
    using System.Diagnostics;
    using Microsoft.Win32.SafeHandles;
    using Strings = Microsoft.Exchange.CtsResources.SharedStrings;

    
    internal class TempFileStream : FileStream
    {
        private static string tempPath;

        internal static string Path
        {
            get { return GetTempPath(); }
        }

        private static int NextId = unchecked(Environment.TickCount ^ Process.GetCurrentProcess().Id);


        private string filePath;

        public string FilePath
        {
            get { return this.filePath; }
        }


        
        
        

        
        
        
        
        private TempFileStream(SafeFileHandle handle) : base(handle, FileAccess.ReadWrite)
        {
        }

        
        
        

        
        
        
        
        public static TempFileStream CreateInstance()
        {
            return TempFileStream.CreateInstance("Cts");
        }

        
        
        
        
        public static TempFileStream CreateInstance(string prefix) 
        {
            return CreateInstance(prefix, true);
        }

        
        
        
        
        
        
        
        
        
        
        public static TempFileStream CreateInstance(string prefix, bool deleteOnClose)
        {
            NativeMethods.SecurityAttributes securityAttribute = new NativeMethods.SecurityAttributes(false);

            string tempPath = TempFileStream.Path;

            
            
            
            new FileIOPermission(FileIOPermissionAccess.Write, tempPath).Demand();

            SafeFileHandle safeHandle;
            string tempFile;
            int errorCode = 0;

            
            int retry = 10;

            do
            {
                uint id = (uint)Interlocked.Increment(ref TempFileStream.NextId);

                tempFile =
                    System.IO.Path.Combine(
                        tempPath,
                        string.Concat(prefix, id.ToString("X5"), ".tmp"));

                uint deleteOnCloseFlag = (deleteOnClose) ? NativeMethods.FILE_FLAG_DELETE_ON_CLOSE : 0;
                safeHandle = NativeMethods.CreateFile(
                        tempFile,
                        NativeMethods.FILE_GENERIC_READ | NativeMethods.FILE_GENERIC_WRITE,
                        0,
                        ref securityAttribute,
                        NativeMethods.CREATE_NEW,
                        NativeMethods.FILE_ATTRIBUTE_TEMPORARY | deleteOnCloseFlag | NativeMethods.FILE_ATTRIBUTE_NOT_CONTENT_INDEXED,
                        IntPtr.Zero);

                retry--;

                if (safeHandle.IsInvalid)
                {
                    errorCode = Marshal.GetLastWin32Error();

                    if (errorCode == NativeMethods.ERROR_FILE_EXISTS)
                    {
                        
                        retry++;
                    }

                    
                    Interlocked.Add(ref TempFileStream.NextId, Process.GetCurrentProcess().Id);
                }
                else
                {
                    
                    retry = 0;
                }
            }
            while (retry > 0);

            if (safeHandle.IsInvalid)
            {
                string message = Strings.CreateFileFailed(tempFile);
                throw new IOException(message, new Win32Exception(errorCode, message));
            }

            TempFileStream tempFileStream = new TempFileStream(safeHandle);
            tempFileStream.filePath = tempFile;
            return tempFileStream;
        }

        internal static void SetTemporaryPath(string path)
        {
            tempPath = path;
        }

        private static string GetTempPath()
        {
            if (tempPath == null)
            {
                tempPath = System.IO.Path.GetTempPath();
            }

            return tempPath;
        }

        
        
        
        
        protected override void Dispose(bool disposing)
        {
            
            
            
            
            
            try
            {
                base.Dispose(disposing);
            }
            catch (IOException)
            {
                
            }
        }
    }
}

