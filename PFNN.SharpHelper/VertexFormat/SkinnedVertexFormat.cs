using SharpDX;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace PFNN.SharpDX.Classes.VertexFormat
{
    /// <summary>
    /// Skinned Vertex Format
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SkinnedVertexFormat
    {
        /// <summary>
        /// Position
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// Normal
        /// </summary>
        public Vector3 Normal;

        /// <summary>
        /// Texture Set 1
        /// </summary>
        public Vector2 TextureSet1;

        /// <summary>
        /// Texture Set 2
        /// </summary>
        public Vector2 TextureSet2;

        /// <summary>
        /// Binormal
        /// </summary>
        public Vector3 Binormal;

        /// <summary>
        /// Tangent
        /// </summary>
        public Vector3 Tangent;

        /// <summary>
        /// Joint
        /// </summary>
        public Vector4 Joint;

        /// <summary>
        /// Weight
        /// </summary>
        public Vector4 Weight;

        /// <summary>
        /// Byte Size
        /// </summary>
        public const int Size = 96;

        /// <summary>
        /// Return Vertex as Float Array
        /// </summary>
        /// <returns></returns>
        internal float[] GetArray()
        {
            List<float> v = new List<float>();
            v.AddRange(Position.ToArray());
            v.AddRange(Normal.ToArray());
            v.AddRange(TextureSet1.ToArray());
            v.AddRange(TextureSet2.ToArray());
            v.AddRange(Tangent.ToArray());
            v.AddRange(Binormal.ToArray());
            v.AddRange(Joint.ToArray());
            v.AddRange(Weight.ToArray());
            return v.ToArray();
        }

        /// <summary>
        /// Compare 2 Vertices
        /// </summary>
        /// <param name="a">First Vertex</param>
        /// <param name="b">Secon Vertex</param>
        /// <returns>Result</returns>
        public static bool Compare(SkinnedVertexFormat a, SkinnedVertexFormat b)
        {
            return a.Position == b.Position &&
                a.TextureSet1 == b.TextureSet1 &&
                a.TextureSet2 == b.TextureSet2 &&
                a.Weight == b.Weight &&
                a.Joint == b.Joint;
        }

    }
}
