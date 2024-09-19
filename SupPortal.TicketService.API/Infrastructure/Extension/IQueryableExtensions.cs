using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace SupPortal.TicketService.API.Infrastructure.Extension;

    public static class IQueryableExtensions
    {
    public static async Task<PaginatedList<T>> ToPagedAsync<T>(this IQueryable<T> query, QueryParameters? parameters) where T : class
    {
        if (parameters is not null)
        {
            int totalCount = await query.CountAsync();

            if (!string.IsNullOrEmpty(parameters?.SortBy))
            {
                //var parameter = Expression.Parameter(typeof(T), "x");
                //Expression property = Expression.Property(parameter,  parameters.SortBy);
                // var convert = Expression.Convert(property, typeof(object));
                // var expression= Expression.Lambda<Func<T, object>>(convert, parameter);

                //query = (bool)parameters.IsSortDescending ?
                //    query.OrderByDescending(e => expression) :
                //    query.OrderBy(e => expression);

                query = query.CustomOrderByProperty(parameters.SortBy, (bool)parameters.IsSortDescending);
            }

            if (!string.IsNullOrEmpty(parameters?.SearchText) && !string.IsNullOrEmpty(parameters.SearchBy))
            {
                query = query.SearchByPropertyContains(parameters.SearchBy, parameters.SearchText);
                var sdf = await query.ToListAsync();

            }



            List<T> items = await query
               .Skip((parameters.PageNumber) * parameters.PageSize)
               .Take(parameters.PageSize)
               .ToListAsync();

            return new PaginatedList<T>(items, totalCount, parameters.PageNumber, parameters.PageSize == int.MaxValue ? totalCount : parameters.PageSize);
        }
        else
        {
            var dsf = await query.ToListAsync();
            return new PaginatedList<T>(dsf, await query.CountAsync(), 0, await query.CountAsync());
        }

    }


    public static IQueryable<T> CustomOrderByProperty<T>(this IQueryable<T> source, string propertyName, bool descending = false)
    {
        var propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        if (propertyInfo == null)
            throw new ArgumentException($"Property '{propertyName}' does not exist on type '{typeof(T).Name}'.");

        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyAccess = Expression.MakeMemberAccess(parameter, propertyInfo);
        var orderByExpression = Expression.Lambda(propertyAccess, parameter);

        var methodName = descending ? "OrderByDescending" : "OrderBy";
        var method = typeof(Queryable).GetMethods()
            .First(m => m.Name == methodName && m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(T), propertyInfo.PropertyType);

        return (IQueryable<T>)method.Invoke(null, new object[] { source, orderByExpression });
    }

    public static IQueryable<T> SearchByPropertyContains<T>(this IQueryable<T> source, string propertyName, string searchText)
    {
        if (string.IsNullOrEmpty(searchText))
            return source;

        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, propertyName);

        // Check if the property type is string or needs to be converted to string
        Expression propertyAsString;
        if (property.Type == typeof(string))
        {
            propertyAsString = property;
        }
        else if (!property.Type.IsCollectionType()) // Exclude collections
        {
            var toStringMethod = property.Type.GetMethod("ToString", Type.EmptyTypes);
            if (toStringMethod == null)
                throw new InvalidOperationException("No suitable ToString method found.");

            propertyAsString = Expression.Call(property, toStringMethod);
        }
        else
        {
            throw new InvalidOperationException("Cannot perform 'Contains' operation on collections directly.");
        }

        var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
        var propertyAsStringLower = Expression.Call(propertyAsString, toLowerMethod);
        var searchTextLower = Expression.Constant(searchText.ToLower(), typeof(string));

        var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        var containsMethodExp = Expression.Call(propertyAsStringLower, containsMethod, searchTextLower);

        var lambda = Expression.Lambda<Func<T, bool>>(containsMethodExp, parameter);
        return source.Where(lambda);
    }

    private static bool IsCollectionType(this Type type)
    {
        return type.GetInterfaces().Any(t => t == typeof(System.Collections.IEnumerable)) && type != typeof(string);
    }
}
