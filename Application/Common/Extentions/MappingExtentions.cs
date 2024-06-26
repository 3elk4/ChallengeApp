using AutoMapper.QueryableExtensions;
using ChallengeApp.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ChallengeApp.Application.Common.Extentions
{
    public static class MappingExtensions
    {
        public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize) where TDestination : class
            => PaginatedList<TDestination>.CreateAsync(queryable.AsNoTracking(), pageNumber, pageSize);

        public static IQueryable<TDestination> ProjectToQueryable<TDestination>(this IQueryable queryable, AutoMapper.IConfigurationProvider configuration) where TDestination : class
            => queryable.ProjectTo<TDestination>(configuration).AsNoTracking();

        public static Task<TDestination> ProjectToSingle<TSource, TDestination>(this IQueryable<TSource> queryable, Expression<Func<TSource, bool>> predicate, AutoMapper.IConfigurationProvider configuration) where TDestination : class where TSource : class
        {
            return queryable.Where(predicate).ProjectTo<TDestination>(configuration).FirstOrDefaultAsync();
        }
    }
}
