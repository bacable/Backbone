using System;
using System.Collections.Generic;
using System.Text;

namespace Backbone.BoardGame
{
    public interface IResourcePool
    {
        /// <summary>
        /// Add a resource to the pool with 'amount' number of times.
        /// </summary>
        /// <param name="resource">The resource to add.</param>
        /// <param name="amount">Number of times to add that resource. If not included, default is 1.</param>
        void Add(IResource resource, int amount = 1);

        /// <summary>
        /// Add a list of components to the pool.
        /// </summary>
        /// <param name="resources">The resources to add.</param>
        /// <param name="amount">The number of times each resource should be added (for multiple copies of the same resource).</param>
        void Add(IEnumerable<IResource> resources, int amount = 1);


        /// <summary>
        /// Draw a single resource at random from the pool.
        /// </summary>
        /// <returns>The resource drawn.</returns>
        IResource Draw();

        /// <summary>
        ///  Draw a certain number of resources at random from the pool and return them.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns>List of resources drawn.</returns>
        IEnumerable<IResource> Draw(int amount);

        /// <summary>
        /// Reshuffles all resources in the pool.
        /// </summary>
        void Reshuffle();
    }
}
