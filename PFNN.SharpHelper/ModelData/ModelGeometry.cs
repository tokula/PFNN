using PFNN.SharpDX.Classes.VertexFormat;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFNN.SharpHelper.ModelData
{
    /// <summary>
    /// Geometry Data
    /// </summary>
    public class ModelGeometry
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Vertices List
        /// </summary>
        public List<SkinnedVertexFormat> Vertices { get; set; }

        /// <summary>
        /// Indices List
        /// </summary>
        public List<int> Indices { get; set; }

        /// <summary>
        /// Material
        /// </summary>
        public MaterialData Material { get; set; }



        /// <summary>
        /// Constructor
        /// </summary>
        public ModelGeometry()
        {
            Vertices = new List<SkinnedVertexFormat>();
            Indices = new List<int>();
        }


        /// <summary>
        /// Optimize Geometry
        /// </summary>
        public void Optimize()
        {
            CorrectTexture();
            GenerateIndices();
            GenerateNormal();
            GenerateTangentBinormal();
        }

        private void CorrectTexture()
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                SkinnedVertexFormat v = Vertices[i];
                v.TextureSet1.Y = 1.0F - v.TextureSet1.Y;
                v.TextureSet2.Y = 1.0F - v.TextureSet2.Y;
                Vertices[i] = v;
            }
        }


        private void GenerateNormal()
        {

            for (int i = 0; i < Vertices.Count; i++)
            {
                SkinnedVertexFormat v = Vertices[i];
                v.Normal = new Vector3();
                Vertices[i] = v;
            }

            for (int i = 0; i < Indices.Count; i += 3)
            {

                SkinnedVertexFormat p1 = Vertices[Indices[i]];
                SkinnedVertexFormat p2 = Vertices[Indices[i + 1]];
                SkinnedVertexFormat p3 = Vertices[Indices[i + 2]];

                Vector3 V1 = p2.Position - p1.Position;
                Vector3 V2 = p3.Position - p1.Position;

                Vector3 N = Vector3.Cross(V1, V2);
                N.Normalize();

                p1.Normal += N;
                p2.Normal += N;
                p3.Normal += N;

                Vertices[Indices[i]] = p1;
                Vertices[Indices[i + 1]] = p2;
                Vertices[Indices[i + 2]] = p3;
            }

            //normalize
            for (int i = 0; i < Vertices.Count; i++)
            {
                SkinnedVertexFormat v = Vertices[i];
                v.Normal.Normalize();
                Vertices[i] = v;
            }

        }

        private void GenerateTangentBinormal()
        {
            //Reset Vertices
            for (int i = 0; i < Vertices.Count; i++)
            {
                SkinnedVertexFormat v = Vertices[i];
                v.Normal = new Vector3();
                v.Tangent = new Vector3();
                v.Binormal = new Vector3();
                Vertices[i] = v;
            }

            for (int i = 0; i < Indices.Count; i += 3)
            {
                SkinnedVertexFormat P0 = Vertices[Indices[i]];
                SkinnedVertexFormat P1 = Vertices[Indices[i + 1]];
                SkinnedVertexFormat P2 = Vertices[Indices[i + 2]];


                Vector3 e0 = P1.Position - P0.Position;
                Vector3 e1 = P2.Position - P0.Position;
                Vector3 normal = Vector3.Cross(e0, e1);
                //using Eric Lengyel's approach with a few modifications
                //from Mathematics for 3D Game Programmming and Computer Graphics
                // want to be able to trasform a vector in Object Space to Tangent Space
                // such that the x-axis cooresponds to the 's' direction and the
                // y-axis corresponds to the 't' direction, and the z-axis corresponds
                // to <0,0,1>, straight up out of the texture map

                Vector3 P = P1.Position - P0.Position;
                Vector3 Q = P2.Position - P0.Position;

                float s1 = P1.TextureSet1.X - P0.TextureSet1.X;
                float t1 = P1.TextureSet1.Y - P0.TextureSet1.Y;
                float s2 = P2.TextureSet1.X - P0.TextureSet1.X;
                float t2 = P2.TextureSet1.Y - P0.TextureSet1.Y;


                //we need to solve the equation
                // P = s1*T + t1*B
                // Q = s2*T + t2*B
                // for T and B


                //this is a linear system with six unknowns and six equatinos, for TxTyTz BxByBz
                //[px,py,pz] = [s1,t1] * [Tx,Ty,Tz]
                // qx,qy,qz     s2,t2     Bx,By,Bz

                //multiplying both sides by the inverse of the s,t matrix gives
                //[Tx,Ty,Tz] = 1/(s1t2-s2t1) *  [t2,-t1] * [px,py,pz]
                // Bx,By,Bz                      -s2,s1	    qx,qy,qz  

                //solve this for the unormalized T and B to get from tangent to object space

                float tmp = 0.0f;
                if (Math.Abs(s1 * t2 - s2 * t1) <= 0.0001f)
                {
                    tmp = 1.0f;
                }
                else
                {
                    tmp = 1.0f / (s1 * t2 - s2 * t1);
                }

                Vector3 tangent = new Vector3();
                Vector3 binormal = new Vector3();

                tangent.X = (t2 * P.X - t1 * Q.X);
                tangent.Y = (t2 * P.Y - t1 * Q.Y);
                tangent.Z = (t2 * P.Z - t1 * Q.Z);

                tangent = tangent * tmp;

                binormal.X = (s1 * Q.X - s2 * P.X);
                binormal.Y = (s1 * Q.Y - s2 * P.Y);
                binormal.Z = (s1 * Q.Z - s2 * P.Z);

                binormal = binormal * tmp;

                normal.Normalize();
                tangent.Normalize();
                binormal.Normalize();

                //Add Normal

                P0.Normal += normal;
                P1.Normal += normal;
                P2.Normal += normal;

                P0.Tangent += tangent;
                P1.Tangent += tangent;
                P2.Tangent += tangent;

                P0.Binormal += binormal;
                P1.Binormal += binormal;
                P2.Binormal += binormal;

                Vertices[Indices[i]] = P0;
                Vertices[Indices[i + 1]] = P1;
                Vertices[Indices[i + 2]] = P2;
            }

            //normalize
            for (int i = 0; i < Vertices.Count; i++)
            {
                SkinnedVertexFormat v = Vertices[i];
                v.Normal.Normalize();
                v.Binormal.Normalize();
                v.Tangent.Normalize();
                Vertices[i] = v;
            }
        }

        private void GenerateIndices()
        {
            List<SkinnedVertexFormat> tempVertices = new List<SkinnedVertexFormat>();
            List<int> tempIndices = new List<int>();

            foreach (SkinnedVertexFormat v in Vertices)
            {
                //Search for existing vertex
                int i = 0;
                bool found = false;
                foreach (SkinnedVertexFormat v2 in tempVertices)
                {
                    if (SkinnedVertexFormat.Compare(v, v2))
                    {
                        //found
                        found = true;
                        break;
                    }
                    i++;
                }

                //In found join the normals
                if (found)
                {
                    tempIndices.Add(i);
                    SkinnedVertexFormat v2 = tempVertices[i];
                }
                else
                {
                    i = tempVertices.Count;
                    tempVertices.Add(v);
                    tempIndices.Add(i);
                }

                //normal
                SkinnedVertexFormat vTemp = tempVertices[i];
                vTemp.Normal += v.Normal;
                tempVertices[i] = vTemp;
            }


            //Normalize all Vertices
            Vertices.Clear();
            foreach (SkinnedVertexFormat v in tempVertices)
            {
                v.Normal.Normalize();
                Vertices.Add(v);
            }
            Vertices.AddRange(tempVertices);

            Indices.Clear();
            Indices.AddRange(tempIndices);

        }
    }
}
