﻿using System;
using System.IO;
using Mono.Unix;
using OpenFileSystem.IO.FileSystem.Local;

namespace OpenFileSystem.IO.FileSystems.Local.Unix
{
    public class UnixDirectory : LocalDirectory
    {
        public UnixDirectory(DirectoryInfo di)
                : base(di)
        {
            GetUnixInfo(di.FullName);
        }

        public UnixDirectory(string directoryPath)
                : base(directoryPath)
        {
        }

        public override bool IsHardLink
        {
            get { return UnixDirectoryInfo.IsSymbolicLink; }
        }

        public override IDirectory Target
        {
            get
            {
                if (IsHardLink)
                    throw new NotImplementedException("Dunno how to get that from Mono.Unix");
                return this;
            }
        }

        protected UnixFileSystemInfo UnixDirectoryInfo { get; set; }

        public override IDirectory LinkTo(string path)
        {
            UnixDirectoryInfo.CreateSymbolicLink(path);
            return CreateDirectory(path);
        }

        protected override LocalDirectory CreateDirectory(string path)
        {
            return new UnixDirectory(path);
        }

        protected override LocalDirectory CreateDirectory(DirectoryInfo di)
        {
            return new UnixDirectory(di);
        }

        void GetUnixInfo(string fullName)
        {
            this.UnixDirectoryInfo = UnixFileSystemInfo.GetFileSystemEntry(fullName);
        }
    }
}