using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Serialization;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace TicketReservation
{
    public class ReservationFileInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void UpdateProperty(string name){ PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        private FileInfo actualfileinfo;

        public ReservationFileInfo(string path)
        {
            actualfileinfo = new FileInfo(path);
            Sections = new ObservableCollection<SectionEditor>();
            Layouts = new ObservableCollection<RoomLayout>();
        }
        
        public FileAttributes Attributes { get { return actualfileinfo.Attributes; } set { actualfileinfo.Attributes = value; UpdateProperty(nameof(Attributes)); } }
        public DateTime CreationTime { get { return actualfileinfo.CreationTime; } set { actualfileinfo.CreationTime = value; UpdateProperty(nameof(CreationTime)); } }
        public DateTime CreationTimeUtc { get { return actualfileinfo.CreationTimeUtc; } set { actualfileinfo.CreationTimeUtc = value; UpdateProperty(nameof(CreationTimeUtc)); } }
        public DirectoryInfo Directory { get { return actualfileinfo.Directory; } }
        public string DirectoryName { get { return actualfileinfo.DirectoryName; } }
        public bool Exists { get { return actualfileinfo.Exists; } }
        public string Extension { get { return actualfileinfo.Extension; } }
        public string FullName { get { return actualfileinfo.FullName; } }
        public bool IsReadOnly { get { return actualfileinfo.IsReadOnly; } set { actualfileinfo.IsReadOnly = value; UpdateProperty(nameof(IsReadOnly)); } }
        public DateTime LastAccessTime { get { return actualfileinfo.LastAccessTime; } set { actualfileinfo.LastAccessTime = value; UpdateProperty(nameof(LastAccessTime)); } }
        public DateTime LastAccessTimeUtc { get { return actualfileinfo.LastAccessTimeUtc; } set { actualfileinfo.LastAccessTimeUtc = value; UpdateProperty(nameof(LastAccessTimeUtc)); } }
        public DateTime LastWriteTime { get { return actualfileinfo.LastWriteTime; } set { actualfileinfo.LastWriteTime = value; UpdateProperty(nameof(LastWriteTime)); } }
        public DateTime LastWriteTimeUtc { get { return actualfileinfo.LastWriteTimeUtc; } set { actualfileinfo.LastWriteTimeUtc = value; UpdateProperty(nameof(LastWriteTimeUtc)); } }
        public long Length { get { return actualfileinfo.Length; } }
        public string Name { get { return actualfileinfo.Name; } }

        public StreamWriter AppendText() { return actualfileinfo.AppendText(); }
        public FileInfo CopyTo(string destFileName) { return actualfileinfo.CopyTo(destFileName); }
        public FileInfo CopyTo(string destFileName, bool overwrite) { return actualfileinfo.CopyTo(destFileName, overwrite); }
        public FileStream Create() { return actualfileinfo.Create(); }
        public ObjRef CreateObjRef(Type type) { return actualfileinfo.CreateObjRef(type); }
        public StreamWriter CreateText() { return actualfileinfo.CreateText(); }
        public void Decrypt() { actualfileinfo.Decrypt(); }
        public void Delete() { actualfileinfo.Delete(); }
        public void Encrypt() { actualfileinfo.Encrypt(); }
        public FileSecurity GetAccessControl() { return actualfileinfo.GetAccessControl(); }
        public FileSecurity GetAccessControl(AccessControlSections includeSections) { return actualfileinfo.GetAccessControl(includeSections); }
        public object GetLifeTimeService() { return actualfileinfo.GetLifetimeService(); }
        public void GetObjectData(SerializationInfo info, StreamingContext context) { actualfileinfo.GetObjectData(info, context); }
        public object InitializeLifeTimeService() { return actualfileinfo.InitializeLifetimeService(); }
        public void MoveTo(string path) { actualfileinfo.MoveTo(path); }
        public FileStream Open(FileMode mode) { return actualfileinfo.Open(mode); }
        public FileStream Open(FileMode mode, FileAccess access) { return actualfileinfo.Open(mode, access); }
        public FileStream Open(FileMode mode, FileAccess access, FileShare share) { return actualfileinfo.Open(mode, access, share); }
        public FileStream OpenRead() { return actualfileinfo.OpenRead(); }
        public StreamReader OpenText() { return actualfileinfo.OpenText(); }
        public FileStream OpenWrite() { return actualfileinfo.OpenWrite(); }
        public void Refresh() { actualfileinfo.Refresh(); }
        public FileInfo Replace(string destinationFileName, string destinationBackupFileName) { return actualfileinfo.Replace(destinationFileName, destinationBackupFileName); }
        public FileInfo Replace(string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors) { return actualfileinfo.Replace(destinationFileName, destinationBackupFileName, ignoreMetadataErrors); }
        public void SetAccessControl(FileSecurity fileSecurity) { actualfileinfo.SetAccessControl(fileSecurity); }

        public ObservableCollection<SectionEditor> Sections { get; private set; }
        public ObservableCollection<RoomLayout> Layouts { get; private set; }
    }
}
