using System;
using System.Threading.Tasks;

namespace RestProject.Services
{
    public interface ISynchronizationService
    {
        Task StartSyncIfNeededAsync();
        bool IsSyncing { get; }
        event EventHandler SyncProgressChanged;
        string SyncStatus { get; }
    }
}