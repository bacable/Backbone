using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backbone.Graphics
{
    public interface IMenuCard
    {
        public int ID { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }        
        public string Title { get; set; }
        public object Data { get; set; }
        void Draw(Matrix view, Matrix projection);
        void SetData(object data);
        void Update(GameTime gameTime);
    }
}
