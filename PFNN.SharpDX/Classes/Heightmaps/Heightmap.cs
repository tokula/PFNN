using PFNN.SharpDX.Classes.VertexFormat;
using PFNN.SharpHelper.ModelData;
using SharpDX;
using SharpHelper.Skinning;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PFNN.SharpDX.Classes.Heightmaps
{
    public class Heightmap
    {
        public float HScale { get; private set; }
        public float VScale { get; private set; }
        public float Offset { get; private set; }
        public List<List<float>> Data { get; private set; }

        public ModelNode ModelNode { get; private set; }

        public ModelData GetModelData()
        {
            var modelData = new ModelData()
            {

            };

            modelData.Name = "Heightmap";
            modelData.Nodes.Add(this.ModelNode);

            return modelData;
        }

        public Heightmap()
        {
            HScale = 3.937007874f;
            VScale = 3;
        }

        public static Heightmap Load(string fileName)
        {
            var heightmap = new Heightmap();

            heightmap.Data = new List<List<float>>();


            using (StreamReader sr = File.OpenText(fileName + ".txt"))
            {
                var row = new List<float>();

                string line = String.Empty;

                while ((line = sr.ReadLine()) != null)
                {
                    row = line.Split(new char[] { ' ' }).Select(a => float.Parse(a)).ToList();
                    heightmap.Data.Add(row);
                }
            }

            int w = heightmap.Data.Count();
            int h = heightmap.Data[0].Count();

            heightmap.Offset = 0.0f;
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    heightmap.Offset += heightmap.Data[x][y];
                }

            heightmap.Offset /= w * h;

            // "Loaded Heightmap '%s' (%i %i)\n", filename, (int)w, (int)h


            var posns = new Vector3[w * h];
            var norms = new Vector3[w * h];

            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    float cx = heightmap.HScale * x, cy = heightmap.HScale * y, cw = heightmap.HScale * w, ch = heightmap.HScale * h;
                    posns[x + y * w] = new Vector3(cx - cw / 2, Helpers.Helper.Sample(new Vector2(cx - cw / 2, cy - ch / 2), heightmap), cy - ch / 2);
                }
            //Vector3.Lerp
            // mix — linearly interpolate between two values
            // // performs a linear interpolation between two vectors
            // https://www.khronos.org/registry/OpenGL-Refpages/gl4/html/mix.xhtml
            // https://www.khronos.org/registry/OpenGL-Refpages/gl4/html/cross.xhtml


            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    norms[x + y * w] = (x > 0 && x < w - 1 && y > 0 && y < h - 1) ?
                      Vector3.Normalize(Vector3.Lerp(
                        Vector3.Cross(
                          posns[(x + 0) + (y + 1) * w] - posns[x + y * w],
                          posns[(x + 1) + (y + 0) * w] - posns[x + y * w]),
                        Vector3.Cross(
                          posns[(x + 0) + (y - 1) * w] - posns[x + y * w],
                          posns[(x - 1) + (y + 0) * w] - posns[x + y * w]), 0.5f)) : new Vector3(0, 1, 0);
                }

            // _ao.txt

            //var aos_fromfile = new List<float>();

            //using (StreamReader sr = File.OpenText(fileName + "_ao.txt"))
            //{
            //    var row = new List<float>();

            //    string line = String.Empty;

            //    while ((line = sr.ReadLine()) != null)
            //    {
            //        aos_fromfile.AddRange(line.Split(new char[] { ' ' }).Select(a => float.Parse(a)));
            //    }
            //}

            var ao_generate = false;
            var aos = new float[w * h];

            //var test = aos_fromfile.Count;

            if (ao_generate)
            {

                for (int x = 0; x < w; x++)
                    for (int y = 0; y < h; y++)
                    {

                        //if (ao_generate)
                        //{
                            float ao_amount = 0.0f;
                            float ao_radius = 50.0f;
                            int ao_samples = 1024;
                            int ao_steps = 5;
                            for (int i = 0; i < ao_samples; i++)
                            {
                                Vector3 off = Vector3.Normalize(new Vector3(Helpers.Helper.Random.Next() % 10000 - 5000,
                                    Helpers.Helper.Random.Next() % 10000 - 5000, Helpers.Helper.Random.Next() % 10000 - 5000));

                                if (Vector3.Dot(off, norms[x + y * w]) < 0.0f)
                                {
                                    off = -off;
                                }
                                for (int j = 1; j <= ao_steps; j++)
                                {
                                    Vector3 next = posns[x + y * w] + (((float)j) / ao_steps) * ao_radius * off;
                                    if (Helpers.Helper.Sample(new Vector2(next.X, next.Z), heightmap) > next.Y)
                                    {
                                        ao_amount += 1.0f; break;
                                    }
                                }
                            }

                            aos[x + y * w] = 1.0f - (ao_amount / ao_samples);
                            //fprintf(ao_file, y == h - 1 ? "%f\n" : "%f ", aos[x + y * w]);
                        //}
                        //else
                        //{
                        //    //fscanf(ao_file, y == h - 1 ? "%f\n" : "%f ", &aos[x + y * w]);
                        //}

                    }

                using (StreamWriter wr = new StreamWriter(fileName + "_ao_new.xml"))
                {
                    var myXML = new XmlSerializer(typeof(float[]));
                    myXML.Serialize(wr, aos);
                }
            }

            else
            {
                using (StreamReader myReader = new StreamReader(fileName + "_ao_new.xml"))
                {
                    var myXML = new XmlSerializer(typeof(float[]));
                    aos = (float[])myXML.Deserialize(myReader);
                }
            }

            var vbo_data = new float[7 * w * h];

#if DEBUG
            var tbo_data = new int[3 * 2 * (w - 1) * (h - 1)];
#else
            var tbo_data = new UInt32[3 * 2 * ((w - 1) / 2) * ((h - 1) / 2)];
#endif
            var vertices = new List<SkinnedVertexFormat>();

            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    vbo_data[x * 7 + y * 7 * w + 0] = posns[x + y * w].X;
                    vbo_data[x * 7 + y * 7 * w + 1] = posns[x + y * w].Y;
                    vbo_data[x * 7 + y * 7 * w + 2] = posns[x + y * w].Z;
                    vbo_data[x * 7 + y * 7 * w + 3] = norms[x + y * w].X;
                    vbo_data[x * 7 + y * 7 * w + 4] = norms[x + y * w].Y;
                    vbo_data[x * 7 + y * 7 * w + 5] = norms[x + y * w].Z;
                    vbo_data[x * 7 + y * 7 * w + 6] = aos[x + y * w];

                    vertices.Add(new SkinnedVertexFormat()
                    {
                        Position = new Vector3(posns[x + y * w].X, posns[x + y * w].Y, posns[x + y * w].Z),
                        Normal = new Vector3(norms[x + y * w].X, norms[x + y * w].Y, norms[x + y * w].Z)

                    });
                }


            //free(posns);
            //free(norms);
            //free(aos);
#if DEBUG
            for (int x = 0; x < (w - 1); x++)
                for (int y = 0; y < (h - 1); y++)
                {
                    tbo_data[x * 3 * 2 + y * 3 * 2 * (w - 1) + 0] = (x + 0) + (y + 0) * w;
                    tbo_data[x * 3 * 2 + y * 3 * 2 * (w - 1) + 1] = (x + 0) + (y + 1) * w;
                    tbo_data[x * 3 * 2 + y * 3 * 2 * (w - 1) + 2] = (x + 1) + (y + 0) * w;
                    tbo_data[x * 3 * 2 + y * 3 * 2 * (w - 1) + 3] = (x + 1) + (y + 1) * w;
                    tbo_data[x * 3 * 2 + y * 3 * 2 * (w - 1) + 4] = (x + 1) + (y + 0) * w;
                    tbo_data[x * 3 * 2 + y * 3 * 2 * (w - 1) + 5] = (x + 0) + (y + 1) * w;
                }
#else
            for (int x = 0; x < (w - 1) / 2; x++)
                for (int y = 0; y < (h - 1) / 2; y++)
                {
                    tbo_data[x * 3 * 2 + y * 3 * 2 * ((w - 1) / 2) + 0] = (x * 2 + 0) + (y * 2 + 0) * w;
                    tbo_data[x * 3 * 2 + y * 3 * 2 * ((w - 1) / 2) + 1] = (x * 2 + 0) + (y * 2 + 2) * w;
                    tbo_data[x * 3 * 2 + y * 3 * 2 * ((w - 1) / 2) + 2] = (x * 2 + 2) + (y * 2 + 0) * w;
                    tbo_data[x * 3 * 2 + y * 3 * 2 * ((w - 1) / 2) + 3] = (x * 2 + 2) + (y * 2 + 2) * w;
                    tbo_data[x * 3 * 2 + y * 3 * 2 * ((w - 1) / 2) + 4] = (x * 2 + 2) + (y * 2 + 0) * w;
                    tbo_data[x * 3 * 2 + y * 3 * 2 * ((w - 1) / 2) + 5] = (x * 2 + 0) + (y * 2 + 2) * w;
                }
#endif
            // LoadGeometries



            heightmap.ModelNode = new ModelNode();

            heightmap.ModelNode.Name = "Heightmap";
            heightmap.ModelNode.Type = NodeType.Node;
            heightmap.ModelNode.World = Matrix.Identity;


            heightmap.ModelNode.Children = new List<ModelNode>();

            //Iterate Geometries
            //foreach (XElement g in element.GetNodes("instance_geometry"))
            //{
            //    node.Geometries.AddRange(LoadGeometries(g.GetReference(), association));
            //}

            heightmap.ModelNode.Geometries = new List<ModelGeometry>();

            heightmap.ModelNode.Geometries.Add(new ModelGeometry() {
                Indices = tbo_data.ToList(),
                Vertices = vertices,
                Material = new MaterialData()
                {
                    Ambient = new Vector4(0.179083f, 0.065358f, 0.065358f, 1),
SpecularPower = 54,
Emissive = new Vector4(0, 0, 0, 1),
DiffuseTexture = "./troll2/troltooth_head_diffuse_5.JPG"
                }
            });

            return heightmap;
        }
    }
}
