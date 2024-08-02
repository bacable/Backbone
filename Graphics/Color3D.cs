namespace Backbone.Graphics
{
    public class Color3D
    {
        public float R { get; set; }
        public float G { get; set; }
        public float B { get; set; }
        public float A { get; set; }
        public Color3D(float r, float g, float b, float a = 1.0f)
        {
            R = r; G = g; B = b; A = a;
        }
    }
}
