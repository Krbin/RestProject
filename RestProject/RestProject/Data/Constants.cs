using Microsoft.Maui.Storage;
using System;
using System.IO;

namespace RestProject.Data
{
    public static class Constants
    {

        public const string NasaApiKey = "DEMO_KEY";
        public const string DatabaseFilename = "NasaApod.db3";
        public const string SyncStateKey = "LastSyncDate";
        public static readonly DateTime ApiStartDate = new DateTime(1995, 6, 16);

        public static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);

        public const int ApiRequestsPerBatch = 30;
        public const int DelayBetweenBatchesMs = 5000;
    }
}