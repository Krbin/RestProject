using System.Collections;
using System.Linq.Expressions;


namespace RestProject.Database
{
    public class SqliteService : ISqliteService
    {
        public async Task<bool> DeleteAllDataAsync<T>()
        {
            return await SqliteDatabase.Instance.Database.DeleteAllAsync<T>() != 0;
        }
        public async Task<bool> DeleteDataAsync<T>(object primaryKey)
        {
            return await SqliteDatabase.Instance.Database.DeleteAsync<T>(primaryKey) != 0;
        }
        public async Task<bool> ExecuteAsync(string query, params object[] args)
        {
            return await SqliteDatabase.Instance.Database.ExecuteAsync(query, args) != 0;
        }
        public async Task<List<T>> GetAllDataAsync<T>() where T : new()
        {
            return await SqliteDatabase.Instance.Database.Table<T>().ToListAsync();
        }
        public async Task<T> GetDataByPkAsync<T>(object pk) where T : new()
        {
            return await SqliteDatabase.Instance.Database.GetAsync<T>(pk);
        }
        public async Task<T> GetDataAsync<T>(Expression<Func<T, bool>> predicate) where T : new()
        {
            try
            {
                return await SqliteDatabase.Instance.Database.GetAsync<T>(predicate);
            }
            catch (Exception)
            {
                //Todo: Log this exception.
                return default;
            }
        }
        public async Task<bool> InsertAllDataAsync<T>(IEnumerable<T> Data)
        {
            return await SqliteDatabase.Instance.Database.InsertAllAsync(Data, typeof(T), true) != 0;
        }
        public async Task<bool> InsertDataAsync<T>(T Data)
        {
            return await SqliteDatabase.Instance.Database.InsertAsync(Data, typeof(T)) != 0;
        }
        public async Task<bool> InsertOrReplace<T>(T Data)
        {
            return await SqliteDatabase.Instance.Database.InsertOrReplaceAsync(Data, typeof(T)) != 0;
        }
        public async Task<bool> UpdateAllDataAsync(IEnumerable Data)
        {
            return await SqliteDatabase.Instance.Database.UpdateAllAsync(Data, true) != 0;
        }
        public async Task<bool> UpdateDataAsync<T>(T Data)
        {
            return await SqliteDatabase.Instance.Database.UpdateAsync(Data, typeof(T)) != 0;
        }
        public async Task<List<T>> GetAllDataQueryAsync<T>(Expression<Func<T, bool>> predicate = null) where T : new()
        {
            return await SqliteDatabase.Instance.Database.Table<T>().Where(predicate).ToListAsync();
        }
    }
}