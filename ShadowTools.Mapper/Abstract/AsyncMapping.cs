using System.Threading.Tasks;

namespace ShadowTools.Mapper.Abstract
{
    public abstract class AsyncMapping<TSource, TDestination> : IMapping<TSource, TDestination> where TDestination : class, new()
    {
        async Task<TDestination> IMapping<TSource, TDestination>.Map(TSource source, TDestination destination)
        {
            if (source == null)
            {
                return null;
            }

            if (destination == null)
            {
                destination = new TDestination();
            }
            
            await MapFieldsAsync(source, destination);
            return destination;
        }

        protected abstract Task MapFieldsAsync(TSource source, TDestination destination);

        async Task<object> IMapping.Map(object source, object destination)
        {
            return await ((IMapping<TSource, TDestination>)this).Map((TSource)source, (TDestination)destination);
        }
    }
}
