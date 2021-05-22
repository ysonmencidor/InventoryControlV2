using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public interface ISqlDataAccess
    {
        Task<List<T>> LoadData<T, U>(string sql, U parameters);
        Task<int> SaveData<T>(string sql, T parameters);
        Task<T> LoadSingleData<T>(string sql, object parameters = null);
        Task SpSaveData<T>(string sql, T parameters);
        Task<int> SaveDataReturnId<T>(string sql, T parameters);
    }
}