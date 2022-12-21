namespace Backbone.Events
{
    public interface IProvider<TReturn,TRequest>
    {
        /// <summary>
        /// Request for a type of information, provided as another type.
        /// </summary>
        /// <param name="infoType">What type of information requested, probably as an enum</param>
        /// <returns>The data requested, can be any type.</returns>
        TReturn Provide(TRequest infoType);
    }
}
