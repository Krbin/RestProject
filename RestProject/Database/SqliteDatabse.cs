﻿using BlazorRestProject.Models;
using SQLite;

namespace BlazorRestProject.Database
{
    public sealed class SqliteDatabase
    {
        private readonly SQLiteAsyncConnection database;
        private static Lazy<SqliteDatabase> lazy = null;

        private SqliteDatabase(string path)
        {
            database = new SQLiteAsyncConnection(path);
        }

        public SQLiteAsyncConnection Database => database;

        public static SqliteDatabase Instance
        {
            get
            {
                if (lazy is null)
                {
                    throw new InstanceNotCreatedException();
                }
                return lazy.Value;
            }
        }

        private static void CreateSharedDatabase(string path)
        {
            if (lazy is null)
            {
                lazy = new Lazy<SqliteDatabase>(() => new SqliteDatabase(path));
            }
        }

        public static async Task InitializeDatabaseAsync()
        {
            var databasePath = Path.Combine(
                            Environment.GetFolderPath(
                            Environment.SpecialFolder.LocalApplicationData),
                            Constants.DatabaseName
                            );
            CreateSharedDatabase(databasePath);
            await CreateAllTablesAsync();
        }

        private static async Task CreateAllTablesAsync()
        {
            await SqliteDatabase.Instance.Database.CreateTableAsync<ApodModel>();
        }
    }
    public class InstanceNotCreatedException : Exception
    {
        public InstanceNotCreatedException() : base()
        {
        }

        public InstanceNotCreatedException(string message) : base(message)
        {
        }

        public InstanceNotCreatedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}