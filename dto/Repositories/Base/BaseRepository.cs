using DAL.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAL.Repositories.Base
{
    public class BaseRepository<S, T> : IBaseRepository<T> where T : class
    {
        protected readonly ApplicationDataContext _context;
        protected readonly DbSet<T> _set;
        protected readonly ILogger<S> _logger;

        public BaseRepository(
                ILogger<S> logger,
                ApplicationDataContext context)
        {
            _logger = logger;
            _context = context;
            _set = context.Set<T>();
        }

        public async virtual Task<T> AddAsync(T input)
        {
            try
            {
                if (input == null)
                    throw new ArgumentNullException("input can not be null");

                await _set.AddAsync(input);
                return input;
            }
            catch (Exception e)
            {
                _logger.LogError("Could not add for set {type} : {exception}", typeof(T).ToString(), e.ToString());
                throw;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not update changes for set {type} : {exception}", typeof(T).ToString(), e.ToString());
                throw;
            }
        }
    }
}
