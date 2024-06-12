
using DAL.Models.Base;

namespace DAL.Contracts
{
    /// <summary>
    /// EF Base class contract for models with a key
    /// </summary>
    public interface IBaseKeyRepository<T> : IBaseRepository<T> where T : Entity
    {
        /// <summary>
        /// Gets the entity with the given ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetByIdAsync(Guid id);
    }
}
