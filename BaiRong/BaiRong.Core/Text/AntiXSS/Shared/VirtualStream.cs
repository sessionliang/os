// ***************************************************************
// <copyright file="VirtualStream.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Security;
    using System.Security.AccessControl;
    using System.Security.Principal;
    using System.Security.Permissions;
    using Strings = Microsoft.Exchange.CtsResources.SharedStrings;

    

    internal abstract class RefCountable
    {
        private int refCount;

        protected RefCountable()
        {
            
            this.refCount = 1;
        }

        public int RefCount { get { return this.refCount; } }

        public void AddRef()
        {
            int rc = System.Threading.Interlocked.Increment(ref this.refCount);
            InternalDebug.Assert(rc > 1);
        }

        public void Release()
        {
            int rc = System.Threading.Interlocked.Decrement(ref this.refCount);
            InternalDebug.Assert(rc >= 0);
            if (rc == 0)
            {
                this.Destroy();
            }
        }

        protected virtual void Destroy()
        {
        }
    }

    

    internal abstract class DataStorage : RefCountable
    {
        protected bool isReadOnly;
        protected object readOnlyLock;

        protected DataStorage() : base()
        {
        }

        
        
        
        

        public abstract Stream OpenReadStream(long start, long end);

        
        
        
        
        
        
        

        public virtual long CopyContentToStream(
                            long start,
                            long end, 
                            Stream destStream,
                            ref byte[] scratchBuffer)
        {
            InternalDebug.Assert(0 <= start && start <= end);

            if (destStream == null && end != long.MaxValue)
            {
                
                
                
                
                return end - start;
            }

            
            using (Stream srcStream = this.OpenReadStream(start, end))
            {
                return DataStorage.CopyStreamToStream(srcStream, destStream, long.MaxValue, ref scratchBuffer);
            }
        }

        
        
        
        
        
        
        
        

        public static long CopyStreamToStream(
                            Stream srcStream,
                            Stream destStream,
                            long lengthToCopy,
                            ref byte[] scratchBuffer)
        {
            if (scratchBuffer == null || scratchBuffer.Length < 4096)
            {
                scratchBuffer = new byte[4096];
            }

            long written = 0;

            while (lengthToCopy != 0)
            {
                int toRead = (int)Math.Min(lengthToCopy, (long)scratchBuffer.Length);

                int read = srcStream.Read(scratchBuffer, 0, toRead);
                if (0 == read)
                {
                    

                    
                    
                    InternalDebug.Assert(lengthToCopy == long.MaxValue);
                    break;
                }

                if (destStream != null)
                {
                    destStream.Write(scratchBuffer, 0, read);
                }

                written += read;

                if (lengthToCopy != long.MaxValue)
                {
                    lengthToCopy -= read;
                }
            }

            return written;
        }

        public static Stream NewEmptyReadStream()
        {
            return new StreamOnReadableDataStorage(null, 0, 0);
        }

        
        
        internal virtual void SetReadOnly(bool makeReadOnly)
        {
            if (makeReadOnly == this.isReadOnly)
            {
                return;
            }

            if (makeReadOnly)
            {
                this.readOnlyLock = new Object();
            }
            else
            {
                this.readOnlyLock = null;
            }

            this.isReadOnly = makeReadOnly;
        }
    }

    

    internal abstract class StreamOnDataStorage : Stream
    {
        

        public abstract DataStorage Storage { get; }
        public abstract long Start { get; }
        public abstract long End { get; }

        
        
        
        
        protected override void Dispose(bool disposing)
        {
            
            
            base.Dispose(disposing);
        }
    }

    

    internal abstract class ReadableDataStorage : DataStorage
    {
        public ReadableDataStorage() : base()
        {
        }

        public abstract long Length { get; }

        public abstract int Read(long position, byte[] buffer, int offset, int count);

        public override Stream OpenReadStream(long start, long end)
        {
            return new StreamOnReadableDataStorage(this, start, end);
        }

        public override long CopyContentToStream(
                            long start,
                            long end, 
                            Stream destStream,
                            ref byte[] scratchBuffer)
        {
            
            

            if (scratchBuffer == null || scratchBuffer.Length < 4096)
            {
                scratchBuffer = new byte[4096];
            }

            long written = 0;
            long remaining = end == long.MaxValue ? long.MaxValue : end - start;

            while (remaining != 0)
            {
                int toRead = (int)Math.Min(remaining, (long)scratchBuffer.Length);

                int read = this.Read(start, scratchBuffer, 0, toRead);
                if (0 == read)
                {
                    
                    

                    
                    
                    break;
                }

                start += read;

                destStream.Write(scratchBuffer, 0, read);

                written += read;

                if (remaining != long.MaxValue)
                {
                    remaining -= read;
                }
            }

            return written;
        }
    }

    

    internal abstract class ReadableWritableDataStorage : ReadableDataStorage
    {
        public ReadableWritableDataStorage() : base()
        {
        }

        public abstract void Write(long position, byte[] buffer, int offset, int count);
        public abstract void SetLength(long length);

        public virtual StreamOnDataStorage OpenWriteStream(bool append)
        {
            
            

            if (append)
            {
                return new AppendStreamOnDataStorage(this);
            }

            return new ReadWriteStreamOnDataStorage(this);
        }

#if DEBUG
        private bool writeStreamOpen;

        
        internal void SignalWriteStreamOpen()
        {
            InternalDebug.Assert(!this.writeStreamOpen);
            this.writeStreamOpen = true;
        }

        
        internal void SignalWriteStreamClose()
        {
            InternalDebug.Assert(this.writeStreamOpen);
            this.writeStreamOpen = false;
        }
#endif
    }

    

    internal class StreamOnReadableDataStorage : StreamOnDataStorage, ICloneableStream
    {
        private ReadableDataStorage baseStorage;
        private long start;
        private long end;
        private long position;
        private bool disposed;

        public StreamOnReadableDataStorage(ReadableDataStorage baseStorage, long start, long end)
        {
            InternalDebug.Assert(baseStorage != null || (start == 0 && end == 0));
            InternalDebug.Assert(start >= 0 && start <= end);

            if (baseStorage != null)
            {
                baseStorage.AddRef();
                this.baseStorage = baseStorage;
            }

            this.start = start;
            this.end = end;
        }

        

        private StreamOnReadableDataStorage(ReadableDataStorage baseStorage, long start, long end, long position)
        {
            InternalDebug.Assert(baseStorage != null || (start == 0 && end == 0));
            InternalDebug.Assert(start >= 0 && start <= end);

            if (baseStorage != null)
            {
                baseStorage.AddRef();
                this.baseStorage = baseStorage;
            }

            this.start = start;
            this.end = end;
            this.position = position;
        }

        

        public override DataStorage Storage
        {
            get
            {
                this.ThrowIfDisposed();
                return this.baseStorage;
            }
        }

        public override long Start
        {
            get
            {
                this.ThrowIfDisposed();
                return this.start;
            }
        }

        public override long End
        {
            get
            {
                this.ThrowIfDisposed();
                return this.end;
            }
        }

        
        

        public override bool CanRead
        {
            get
            {
                return !this.disposed;
            }
        }

        public override bool CanWrite
        {
             get
             {
                 return false;
             }
        }

        public override bool CanSeek
        {
            get
            {
                return !this.disposed;
            }
        }

        public override long Length
        {
            get
            {
                this.ThrowIfDisposed();
                
                return this.end == long.MaxValue ? this.baseStorage.Length - this.start : this.end - this.start;
            }
        }

        public override long Position
        {
            get
            {
                this.ThrowIfDisposed();
                
                return this.position;
            }

            set
            {
                this.ThrowIfDisposed();
                
                InternalDebug.Assert(0 <= this.Position);

                if (value < 0)
                {
                    
                    throw new System.ArgumentOutOfRangeException("value", Strings.CannotSeekBeforeBeginning);
                }

                
                this.position = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            this.ThrowIfDisposed();

            if (null == buffer)
            {
                throw new ArgumentNullException("buffer");
            }

            if (offset > buffer.Length || offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset", Strings.OffsetOutOfRange);
            }

            if (count < 0)
            {
                
                throw new ArgumentOutOfRangeException("count", Strings.CountOutOfRange);
            }

            if (count + offset > buffer.Length)
            {
                throw new ArgumentOutOfRangeException("count", Strings.CountTooLarge);
            }

            int readTotal = 0;
            int read;

            if ((this.end == long.MaxValue || this.position < this.end - this.start) && count != 0)
            {
                if (this.end != long.MaxValue && count > this.end - this.start - this.position)
                {
                    count = (int)(this.end - this.start - this.position);
                }

                do
                {
                    read = this.baseStorage.Read(this.start + this.position, buffer, offset, count);

                    count -= read;
                    offset += read;

                    this.position += read;

                    readTotal += read;
                }
                while (count != 0 && read != 0);
            }

            return readTotal;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override void Flush()
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            this.ThrowIfDisposed();

            switch (origin)
            {
                case SeekOrigin.Begin:
                {
                    break;
                }
                case SeekOrigin.Current:
                {
                    offset += this.position;
                    break;
                }
                case SeekOrigin.End:
                {
                    offset += this.Length;
                    break;
                }
                default:
                {
                    
                    throw new System.ArgumentException("Invalid Origin enumeration value", "origin");
                }
            }
            
            if (offset < 0)
            {
                throw new System.ArgumentOutOfRangeException("offset", Strings.CannotSeekBeforeBeginning);
            }
            
            this.position = offset;

            return this.position;
        }

        public override void Close()
        {
            if (this.baseStorage != null)
            {
                this.baseStorage.Release();
                this.baseStorage = null;
            }

            this.disposed = true;
            base.Close();
        }

        public Stream Clone()
        {
            this.ThrowIfDisposed();

            return new StreamOnReadableDataStorage(this.baseStorage, this.start, this.end, this.position);
        }

        private void ThrowIfDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException("StreamOnReadableDataStorage");
            }
        }

        
        
        
        
        protected override void Dispose(bool disposing)
        {
            
            
            base.Dispose(disposing);
        }
    }

    

    internal class ReadableDataStorageOnStream : ReadableDataStorage
    {
        private Stream stream;
        private bool ownsStream;

        public ReadableDataStorageOnStream(Stream stream, bool ownsStream) : base()
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            InternalDebug.Assert(this.RefCount == 1);
            InternalDebug.Assert(stream.CanSeek && stream.CanRead);

            this.stream = stream;
            this.ownsStream = ownsStream;
        }

        public override long Length { get { return this.stream.Length; } }

        public override int Read(long position, byte[] buffer, int offset, int count)
        {
            InternalDebug.Assert(this.RefCount > 0);

            if (this.isReadOnly)
            {
                lock (this.readOnlyLock)
                {
                    return this.InternalRead(position, buffer, offset, count);
                }
            }
            else
            {
                return this.InternalRead(position, buffer, offset, count);
            }
        }
        private int InternalRead(long position, byte[] buffer, int offset, int count)
        {
            this.stream.Position = position;
            return this.stream.Read(buffer, offset, count);
        }
#if false
        public override Stream OpenReadStream(long start, long end)
        {
            
            return new StreamOnReadableDataStorage(this, start, end);
        }
#endif
        protected override void Destroy()
        {
            InternalDebug.Assert(this.RefCount == 0);

            if (this.ownsStream)
            {
                this.stream.Close();
            }

            this.stream = null;

            base.Destroy();
        }
    }

    

    internal class ReadableWritableDataStorageOnStream : ReadableWritableDataStorage
    {
        protected Stream stream;
        protected bool ownsStream;

        public ReadableWritableDataStorageOnStream(Stream stream, bool ownsStream) : base()
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            InternalDebug.Assert(this.RefCount == 1);
            InternalDebug.Assert(stream.CanSeek && stream.CanRead && stream.CanWrite);

            this.stream = stream;
            this.ownsStream = ownsStream;
        }

        public override long Length { get { return this.stream.Length; } }

        public override int Read(long position, byte[] buffer, int offset, int count)
        {
            InternalDebug.Assert(this.RefCount > 0);

            if (this.isReadOnly)
            {
                lock (this.readOnlyLock)
                {
                    return this.InternalRead(position, buffer, offset, count);
                }
            }
            else
            {
                return this.InternalRead(position, buffer, offset, count);
            }
        }
        private int InternalRead(long position, byte[] buffer, int offset, int count)
        {
            this.stream.Position = position;
            return this.stream.Read(buffer, offset, count);
        }

        public override void Write(long position, byte[] buffer, int offset, int count)
        {
            InternalDebug.Assert(this.RefCount > 0);

            if (this.isReadOnly)
            {
                throw new InvalidOperationException("Write to read-only DataStorage");
            }

            this.stream.Position = position;
            this.stream.Write(buffer, offset, count);
        }

        public override void SetLength(long length)
        {
            InternalDebug.Assert(this.RefCount > 0);

            if (this.isReadOnly)
            {
                throw new InvalidOperationException("Write to read-only DataStorage");
            }
            this.stream.SetLength(length);
        }
#if false
        public override Stream OpenReadStream(long start, long end)
        {
            
            return new StreamOnReadableDataStorage(this, start, end);
        }
#endif
        protected override void Destroy()
        {
            InternalDebug.Assert(this.RefCount == 0);

            if (this.ownsStream)
            {
                this.stream.Close();
            }

            this.stream = null;

            base.Destroy();
        }
    }






    internal class Func
    {

    }
    
    
    internal class TemporaryDataStorage : ReadableWritableDataStorage
    {
        private long totalLength = 0;
        private long filePosition = 0;
        private Stream fileStream;

        public static int defaultBufferBlockSize = 8 * 1024;                            
        public static int defaultBufferMaximumSize = defaultBufferBlockSize * 16;       
        public static string defaultPath = null;
        public static Action<byte[]> defaultReleaseBuffer = null;

        internal static volatile bool configured = false;  
        private static object configurationLockObject = new object();

        


        
        
        
        public override long Length { get { return this.totalLength; } }

        
        
        
        protected override void Destroy()
        {
            InternalDebug.Assert(this.RefCount == 0);

            if (this.fileStream != null)
            {
                this.fileStream.Close();
                this.fileStream = null;
            }

            base.Destroy();
        }

        
        
        
        
        
        
        
        
        
        
        
        public override int Read(long position, byte[] buffer, int offset, int count)
        {
            int readTotal = 0;

           

            return readTotal;
        }

        private int InternalRead(long position, byte[] buffer, int offset, int count)
        {

            int readFromFile = this.fileStream.Read(buffer, offset, count);

            return readFromFile;
        }

        
        
        
        
        
        
        
        public override void Write(long position, byte[] buffer, int offset, int count)
        {
            if (this.isReadOnly)
            {
                throw new InvalidOperationException("Write to read-only DataStorage");
            }

            if (count != 0)
            {

                if (this.fileStream == null)
                {
                    this.fileStream = TempFileStream.CreateInstance();
                    this.filePosition = 0;
                }

                this.fileStream.Write(buffer, offset, count);

                position += count;

                if (position > this.totalLength)
                {
                    this.totalLength = position;
                }
            }
        }

        
        
        
        
        public override void SetLength(long length)
        {
            if (this.isReadOnly)
            {
                throw new InvalidOperationException("Write to read-only DataStorage");
            }

            this.totalLength = length;
        }

        internal static void RefreshConfiguration()
        {
            TemporaryDataStorage.configured = false;
        }

        internal static string GetTempPath()
        {
            if (!TemporaryDataStorage.configured)
            {
                TemporaryDataStorage.Configure();
            }

            return TempFileStream.Path;
        }

        public static void Configure(
            int defaultMaximumSize,
            int defaultBlockSize,
            string defaultPath,
            Action<byte[]> defaultReleaseBuffer)
        {
            TemporaryDataStorage.defaultBufferMaximumSize = defaultMaximumSize;
            TemporaryDataStorage.defaultBufferBlockSize = defaultBlockSize;
            TemporaryDataStorage.defaultPath = defaultPath;
            TemporaryDataStorage.defaultReleaseBuffer = defaultReleaseBuffer;

            TemporaryDataStorage.configured = false;
            TemporaryDataStorage.Configure();
        }

        private static void Configure()
        {
            lock (TemporaryDataStorage.configurationLockObject)
            {
                if (!TemporaryDataStorage.configured)
                {
                    int maximumSize = TemporaryDataStorage.defaultBufferMaximumSize;
                    int blockSize = TemporaryDataStorage.defaultBufferBlockSize;
                    string path = TemporaryDataStorage.defaultPath;

                    if (maximumSize < blockSize || maximumSize % blockSize != 0)
                    {                        
                        maximumSize = defaultBufferMaximumSize;
                        blockSize = defaultBufferBlockSize;
                    }

                    TemporaryDataStorage.defaultBufferMaximumSize = maximumSize;
                    TemporaryDataStorage.defaultBufferBlockSize = blockSize;

                    string defaultPath = System.IO.Path.GetTempPath();

                    if (path != null)
                    {
                        
                        path = TemporaryDataStorage.ValidatePath(path);
                    }

                    if (path == null)
                    {
                        
                        

                        path = defaultPath;
                    }

                    TempFileStream.SetTemporaryPath(path);

                    TemporaryDataStorage.configured = true;
                }
            }
        }

        private static readonly FileSystemAccessRule[] DirectoryAccessRules = new FileSystemAccessRule[]
        {
            new FileSystemAccessRule(
                new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null),
                FileSystemRights.FullControl,
                InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
                PropagationFlags.None,
                AccessControlType.Allow),
            new FileSystemAccessRule(
                new SecurityIdentifier(WellKnownSidType.LocalSystemSid, null),
                FileSystemRights.FullControl,
                InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
                PropagationFlags.None,
                AccessControlType.Allow),
            new FileSystemAccessRule(
                new SecurityIdentifier(WellKnownSidType.NetworkServiceSid, null),
                FileSystemRights.FullControl & ~(FileSystemRights.ChangePermissions | FileSystemRights.TakeOwnership),
                InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
                PropagationFlags.None,
                AccessControlType.Allow)
        };

        private static DirectorySecurity GetDirectorySecurity()
        {
            DirectorySecurity security = new DirectorySecurity();

            
            
            security.SetAccessRuleProtection(true, false);

            
            security.SetOwner(WindowsIdentity.GetCurrent().User);

            
            for (int i = 0; i < DirectoryAccessRules.Length; i++)
            {
                security.AddAccessRule(DirectoryAccessRules[i]);
            }

            if (!WindowsIdentity.GetCurrent().User.IsWellKnown(WellKnownSidType.LocalSystemSid) &&
                !WindowsIdentity.GetCurrent().User.IsWellKnown(WellKnownSidType.NetworkServiceSid))
            {
                

                security.AddAccessRule(new FileSystemAccessRule(
                    WindowsIdentity.GetCurrent().User,
                    FileSystemRights.FullControl,
                    InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
                    PropagationFlags.None,
                    AccessControlType.Allow));
            }

            return security;
        }


        
        
        [SuppressMessage("Microsoft.Security", "CA2103:ReviewImperativeSecurity")]
        
        private static string ValidatePath(string path)
        {
            

            try
            {
                if (System.IO.Path.IsPathRooted(path))
                {
                    if (Directory.Exists(path))
                    {
                        
                    }
                    else
                    {
                        Directory.CreateDirectory(path, GetDirectorySecurity());
                    }

                    
                    
                    
                    
                    
                    
                    new FileIOPermission(FileIOPermissionAccess.Write, path).Demand();
                }
                else
                {
                    
                    path = null;
                }
            }
            catch (PathTooLongException /*exception*/)
            {
                
                path = null;
            }
            catch (DirectoryNotFoundException /*exception*/)
            {
                
                path = null;
            }
            catch (IOException /*exception*/)
            {
                
                path = null;
            }
            catch (UnauthorizedAccessException /*exception*/)
            {
                
                path = null;
            }
            catch (ArgumentException /*exception*/)
            {
                
                path = null;
            }
            catch (NotSupportedException /*exception*/)
            {
                
                path = null;
            }

            return path;
        }

        
        
        
      
    }

    

    internal class ReadWriteStreamOnDataStorage : StreamOnDataStorage, ICloneableStream
    {
        private ReadableWritableDataStorage storage;
        private long position;

        internal ReadWriteStreamOnDataStorage(ReadableWritableDataStorage storage)
        {
#if DEBUG
            
            
#endif
            storage.AddRef();

            this.storage = storage;
        }

        private ReadWriteStreamOnDataStorage(ReadableWritableDataStorage storage, long position)
        {
#if DEBUG
            
            
#endif
            storage.AddRef();

            this.storage = storage;
            this.position = position;
        }

        

        public override DataStorage Storage
        {
            get
            {
                if (this.storage == null)
                {
                    throw new ObjectDisposedException("ReadWriteStreamOnDataStorage");
                }

                return this.storage;
            }
        }

        public override long Start
        {
            get
            {
                return 0;
            }
        }

        public override long End
        {
            get
            {
                return long.MaxValue;
            }
        }

        
        

        public override bool CanRead
        {
            get
            { 
                if (this.storage == null)
                {
                    return false;
                }

                return true;
            }
        }

        public override bool CanWrite
        {
            get
            { 
                if (this.storage == null)
                {
                    return false;
                }

                return true;
            }
        }

        public override bool CanSeek
        {
            get
            { 
                if (this.storage == null)
                {
                    return false;
                }

                return true;
            }
        }

        public override long Length
        {
            get
            { 
                if (this.storage == null)
                {
                    throw new ObjectDisposedException("ReadWriteStreamOnDataStorage");
                }

                return this.storage.Length; 
            }
        }

        public override long Position
        {
            get
            { 
                if (this.storage == null)
                {
                    throw new ObjectDisposedException("ReadWriteStreamOnDataStorage");
                }

                return this.position; 
            }
            set 
            { 
                if (this.storage == null)
                {
                    throw new ObjectDisposedException("ReadWriteStreamOnDataStorage");
                }
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", Strings.CannotSeekBeforeBeginning);
                }

                this.position = value; 
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int bytesRead;

            if (this.storage == null)
            {
                throw new ObjectDisposedException("ReadWriteStreamOnDataStorage");
            }

            if (null == buffer)
            {
                throw new ArgumentNullException("buffer");
            }

            if (offset > buffer.Length || offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset", Strings.OffsetOutOfRange);
            }

            if (count < 0)
            {
                
                throw new ArgumentOutOfRangeException("count", Strings.CountOutOfRange);
            }

            if (count + offset > buffer.Length)
            {
                throw new ArgumentOutOfRangeException("count", Strings.CountTooLarge);
            }

            bytesRead = this.storage.Read(this.position, buffer, offset, count);

            this.position += bytesRead;

            return bytesRead;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (this.storage == null)
            {
                throw new ObjectDisposedException("ReadWriteStreamOnDataStorage");
            }

            if (null == buffer)
            {
                throw new ArgumentNullException("buffer");
            }

            if (offset > buffer.Length || offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset", Strings.OffsetOutOfRange);
            }

            if (count > buffer.Length || count < 0)
            {
                throw new ArgumentOutOfRangeException("count", Strings.CountOutOfRange);
            }

            if (count + offset > buffer.Length)
            {
                throw new ArgumentOutOfRangeException("count", Strings.CountTooLarge);
            }

            this.storage.Write(this.position, buffer, offset, count);

            this.position += count;
        }

        public override void SetLength(long value)
        {
            if (this.storage == null)
            {
                throw new ObjectDisposedException("ReadWriteStreamOnDataStorage");
            }

            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("value", Strings.CannotSetNegativelength);
            }

            this.storage.SetLength(value);
        }

        public override void Flush()
        {
            if (this.storage == null)
            {
                throw new ObjectDisposedException("ReadWriteStreamOnDataStorage");
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (this.storage == null)
            {
                throw new ObjectDisposedException("ReadWriteStreamOnDataStorage");
            }

            switch (origin)
            {
                case SeekOrigin.Begin:
                {
                    this.position = offset;
                    break;
                }
                case SeekOrigin.Current:
                {
                    offset = this.position + offset;
                    break;
                }
                case SeekOrigin.End:
                {
                    offset = this.storage.Length + offset;
                    break;
                }
                default:
                {
                    
                    throw new System.ArgumentException("Invalid Origin enumeration value", "origin");
                }
            }

            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset", Strings.CannotSeekBeforeBeginning);
            }

            this.position = offset;

            return this.position;
        }

        public override void Close()
        {
            if (null != this.storage)
            {
#if DEBUG
                
                
#endif
                this.storage.Release();
                this.storage = null;
            }
        }

        
        
        
        
        protected override void Dispose(bool disposing)
        {
            
            
            base.Dispose(disposing);
        }

        Stream ICloneableStream.Clone()
        {
            if (this.storage == null)
            {
                throw new ObjectDisposedException("ReadWriteStreamOnDataStorage");
            }

            return new ReadWriteStreamOnDataStorage(this.storage, this.position);
        }
    }

    

    internal class AppendStreamOnDataStorage : StreamOnDataStorage
    {
        private ReadableWritableDataStorage storage;
        private long position;

        public AppendStreamOnDataStorage(ReadableWritableDataStorage storage)
        {
#if DEBUG
            storage.SignalWriteStreamOpen();
#endif
            storage.AddRef();

            this.storage = storage;
            this.position = storage.Length;     
        }

        

        public override DataStorage Storage
        {
            get
            {
                return this.storage;
            }
        }

        public override long Start
        {
            get
            {
                return 0;
            }
        }

        
        
        
        public override long End
        {
            get
            {
                
                InternalDebug.Assert(this.storage.Length == 0 || this.storage.Length == this.position);
                return this.position;
            }
        }

        public ReadableWritableDataStorage ReadableWritableStorage
        {
            get
            {
                return this.storage;
            }
        }

        
        

        public override bool CanRead
        {
            get
            { 
                return false;
            }
        }

        public override bool CanWrite
        {
            get
            { 
                return this.storage != null;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return false;
            }
        }

        public override long Length
        {
            get
            { 
                throw new NotSupportedException();
            }
        }

        public override long Position
        {
            get
            { 
                throw new NotSupportedException();
            }
            set 
            { 
                throw new NotSupportedException();
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (this.storage == null)
            {
                throw new ObjectDisposedException("AppendStreamOnDataStorage");
            }

            if (null == buffer)
            {
                throw new ArgumentNullException("buffer");
            }

            if (offset > buffer.Length || offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset", Strings.OffsetOutOfRange);
            }

            if (count > buffer.Length || count < 0)
            {
                throw new ArgumentOutOfRangeException("count", Strings.CountOutOfRange);
            }

            if (count + offset > buffer.Length)
            {
                throw new ArgumentOutOfRangeException("count", Strings.CountTooLarge);
            }

            
            InternalDebug.Assert(this.storage.Length == 0 || this.storage.Length == this.position);

            this.storage.Write(this.position, buffer, offset, count);

            this.position += count;
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Flush()
        {
            if (this.storage == null)
            {
                throw new ObjectDisposedException("AppendStreamOnDataStorage");
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void Close()
        {
            if (null != this.storage)
            {
#if DEBUG
                
                InternalDebug.Assert(this.storage.Length == 0 || this.storage.Length == this.position);
                this.storage.SignalWriteStreamClose();
#endif
                this.storage.Release();
                this.storage = null;
            }

            base.Close();
        }

        
        
        
        
        protected override void Dispose(bool disposing)
        {
            
            
            base.Dispose(disposing);
        }
    }
}

