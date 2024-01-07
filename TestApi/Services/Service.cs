using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Test.Data;

namespace TestApi.Services
{
    public class Service<S, E, R>
        where S : class //service
        where E : class //db entity
        where R : class //return dto
    {
        protected Expression<Func<E, R>> _toDto;
        protected Func<E, R> AsDto => _toDto.Compile();
        protected readonly MSTestDataContext _dataContext;
        protected readonly ILogger<S> _logger;

        public Service(ILogger<S> logger, MSTestDataContext dataContext)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        protected IQueryable<R> Dtos(Expression<Func<E, bool>>? predicate = null)
        {
            var baseQ = _dataContext.Set<E>().AsNoTracking();
            if (predicate != null) baseQ = baseQ.Where(predicate);
            return baseQ.Select(_toDto);
        }

        protected async Task<bool> ValidateParentAsync<P>(Expression<Func<P, bool>> predicate, CancellationToken token) where P : class 
            => await _dataContext.Set<P>().AsNoTracking().AnyAsync(predicate, token);
    }
}
