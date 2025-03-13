using System.Linq.Expressions;

namespace BreweryAPI.Extensions
{
    public static class ExtensionMethods
    {
        public static IQueryable<T> AddFilter<T>(this IQueryable<T> query, Dictionary<string, string> queryFilters)
        {
            try
            {
                foreach (var filter in queryFilters)
                {

                    var property = typeof(T).GetProperty(filter.Key);
                    if (property == null) throw new InvalidDataException($"{filter.Key} member doesnt exist in {typeof(T)}");

                    object value = filter.Value;

                    // Handle specific types like Guid, DateTime, etc.
                    if (property.PropertyType == typeof(Guid))
                    {
                        if (Guid.TryParse(filter.Value, out var guidValue))
                        {
                            value = guidValue;
                        }
                        else
                        {
                            throw new InvalidCastException($"Invalid Guid format for property {filter.Key}");
                        }
                    }
                    else if (property.PropertyType == typeof(DateTime))
                    {
                        if (DateTime.TryParse(filter.Value, out var dateTimeValue))
                        {
                            value = dateTimeValue;
                        }
                        else
                        {
                            throw new InvalidCastException($"Invalid DateTime format for property {filter.Key}");
                        }
                    }
                    else
                    {
                        if (property.PropertyType == typeof(float)) value = filter.Value.Replace('.', ',');

                        value = Convert.ChangeType(value, property.PropertyType);
                    }

                    // Build the expression tree
                    var row = Expression.Parameter(typeof(T), "row");
                    var filterEx = Expression.Constant(value, property.PropertyType);
                    var onProperty = Expression.Property(row, property);

                    // Create the equality comparison expression
                    var body = Expression.Equal(onProperty, filterEx);
                    var predicate = Expression.Lambda<Func<T, bool>>(body, row);

                    query = query.Where(predicate);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return query;
        }
    }
}
