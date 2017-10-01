using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFNN.SharpDX.Classes.Helpers
{
    public static class Helper
    {
        public static Random Random = new Random();

        public static float Sample(Vector2 pos, Heightmaps.Heightmap heightmap)
        {
            int w = heightmap.Data.Count;
            int h = heightmap.Data[0].Count;

            pos.X = (pos.X / heightmap.HScale) + w / 2;
            pos.Y = (pos.Y / heightmap.HScale) + h / 2;

            float a0 = FMod(pos.X, 1.0f);
            float a1 = FMod(pos.Y, 1.0f);
			
            int x0 = (int)Math.Floor(pos.X), x1 = (int)Math.Ceiling(pos.X);
            int y0 = (int)Math.Floor(pos.Y), y1 = (int)Math.Ceiling(pos.Y);

            x0 = x0 < 0 ? 0 : x0; x0 = x0 >= w ? w - 1 : x0;
            x1 = x1 < 0 ? 0 : x1; x1 = x1 >= w ? w - 1 : x1;
            y0 = y0 < 0 ? 0 : y0; y0 = y0 >= h ? h - 1 : y0;
            y1 = y1 < 0 ? 0 : y1; y1 = y1 >= h ? h - 1 : y1;

            float s0 = heightmap.VScale * (heightmap.Data[x0][y0] - heightmap.Offset);
            float s1 = heightmap.VScale * (heightmap.Data[x1][y0] - heightmap.Offset);
            float s2 = heightmap.VScale * (heightmap.Data[x0][y1] - heightmap.Offset);
            float s3 = heightmap.VScale * (heightmap.Data[x1][y1] - heightmap.Offset);

            return (s0 * (1 - a0) + s1 * a0) * (1 - a1) + (s2 * (1 - a0) + s3 * a0) * a1;

        }

		public static float FMod(float a, float b)
        {
            return a % b;
        }
    }
}
