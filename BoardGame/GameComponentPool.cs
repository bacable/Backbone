using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Backbone.BoardGame
{
    public class GameComponentPool : IResourcePool
    {
        private List<IResource> pool = new List<IResource>();

        public void Add(IResource component, int amount = 1)
        {
            pool.Add(component);

            for(var i=1;i<amount;i++)
            {
                pool.Add(component.Copy());
            }
        }

        public void Add(IEnumerable<IResource> components, int amount)
        {
            pool.AddRange(components);

            for(var i=1; i<amount;i++)
            {
                components.ToList().ForEach(component =>
                {
                    pool.Add(component.Copy());
                });
            }
        }

        public IResource Draw()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IResource> Draw(int amount)
        {
            throw new NotImplementedException();
        }

        public void Reshuffle()
        {
            throw new NotImplementedException();
        }
    }
}
