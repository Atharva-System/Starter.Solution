using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;

namespace Starter.Identity.Services;
public static class SpecificationExtensions
{
    public static List<T> ApplySpecification<T>(this IEnumerable<T> source, ISpecification<T> specification)
        where T : class
    {
        var queryable = source.AsQueryable();
        var specificationEvaluator = new SpecificationEvaluator();
        var result = specificationEvaluator.GetQuery(queryable, specification);
        return result.ToList<T>();
    }

    public static int ApplySpecificationCount<T>(this IEnumerable<T> source, ISpecification<T> specification)
        where T : class
    {
        var queryable = source.AsQueryable();
        var specificationEvaluator = new SpecificationEvaluator(new IEvaluator[] { WhereEvaluator.Instance });
        var result = specificationEvaluator.GetQuery(queryable, specification);
        return result.Count();
    }

}
