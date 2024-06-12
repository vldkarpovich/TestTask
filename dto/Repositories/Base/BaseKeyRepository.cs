using DAL.Contracts;
using DAL.Extensions;
using DAL.Models.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAL.Repositories.Base
{
    public class BaseKeyRepository<S, T> : BaseRepository<S, T>, IBaseKeyRepository<T> where T : Entity
    {

        public BaseKeyRepository(
                ILogger<S> logger,
                ApplicationDataContext context)
            : base(logger, context)
        {
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new ArgumentNullException("Id for input can not be empty");

                var query = _set.AsQueryable().IncludeNavigationProperties();

                return await query.FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception e)
            {
                _logger.LogError("Could not fetch by id {type} : {exception}", typeof(T).ToString(), e.ToString());
                throw;
            }
        }
    }
}
