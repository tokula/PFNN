using PFNN.SharpHelper.ModelData;
using SharpDX;
using SharpHelper;
using SharpHelper.Skinning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFNN.SharpDX.Classes.Helpers
{
    public static class ShapeModelHelper
    {
        public static ModelData GetCube()
        {
            var modelNode = new ModelNode();

            modelNode.Name = "Heightmap";
            modelNode.Type = NodeType.Node;
            modelNode.World = Matrix.Identity;


            modelNode.Children = new List<ModelNode>();

            //Iterate Geometries
            //foreach (XElement g in element.GetNodes("instance_geometry"))
            //{
            //    node.Geometries.AddRange(LoadGeometries(g.GetReference(), association));
            //}

            modelNode.Geometries = new List<ModelGeometry>();

            modelNode.Geometries.Add(new ModelGeometry()
            {
                Indices = new int[]
                            {
                                0,1,2,0,2,3,
                                4,6,5,4,7,6,
                                8,9,10,8,10,11,
                                12,14,13,12,15,14,
                                16,18,17,16,19,18,
                                20,21,22,20,22,23
                            }.ToList(),
            //    Vertices = new[]
            //{
            //    ////TOP
            //    new ColoredVertex(new Vector3(-5,5,5),new Vector4(0,1,0,0)),
            //    new ColoredVertex(new Vector3(5,5,5),new Vector4(0,1,0,0)),
            //    new ColoredVertex(new Vector3(5,5,-5),new Vector4(0,1,0,0)),
            //    new ColoredVertex(new Vector3(-5,5,-5),new Vector4(0,1,0,0)),
            //    //BOTTOM
            //    new ColoredVertex(new Vector3(-5,-5,5),new Vector4(1,0,1,1)),
            //    new ColoredVertex(new Vector3(5,-5,5),new Vector4(1,0,1,1)),
            //    new ColoredVertex(new Vector3(5,-5,-5),new Vector4(1,0,1,1)),
            //    new ColoredVertex(new Vector3(-5,-5,-5),new Vector4(1,0,1,1)),
            //    //LEFT
            //    new ColoredVertex(new Vector3(-5,-5,5),new Vector4(1,0,0,1)),
            //    new ColoredVertex(new Vector3(-5,5,5),new Vector4(1,0,0,1)),
            //    new ColoredVertex(new Vector3(-5,5,-5),new Vector4(1,0,0,1)),
            //    new ColoredVertex(new Vector3(-5,-5,-5),new Vector4(1,0,0,1)),
            //    //RIGHT
            //    new ColoredVertex(new Vector3(5,-5,5),new Vector4(1,1,0,1)),
            //    new ColoredVertex(new Vector3(5,5,5),new Vector4(1,1,0,1)),
            //    new ColoredVertex(new Vector3(5,5,-5),new Vector4(1,1,0,1)),
            //    new ColoredVertex(new Vector3(5,-5,-5),new Vector4(1,1,0,1)),
            //    //FRONT
            //    new ColoredVertex(new Vector3(-5,5,5),new Vector4(0,1,1,1)),
            //    new ColoredVertex(new Vector3(5,5,5),new Vector4(0,1,1,1)),
            //    new ColoredVertex(new Vector3(5,-5,5),new Vector4(0,1,1,1)),
            //    new ColoredVertex(new Vector3(-5,-5,5),new Vector4(0,1,1,1)),
            //    //BACK
            //    new ColoredVertex(new Vector3(-5,5,-5),new Vector4(0,0,1,1)),
            //    new ColoredVertex(new Vector3(5,5,-5),new Vector4(0,0,1,1)),
            //    new ColoredVertex(new Vector3(5,-5,-5),new Vector4(0,0,1,1)),
            //    new ColoredVertex(new Vector3(-5,-5,-5),new Vector4(0,0,1,1))
            //},
                Material = new MaterialData()
                {
                    Ambient = new Vector4(0.179083f, 0.065358f, 0.065358f, 1),
                    SpecularPower = 54,
                    Emissive = new Vector4(0, 0, 0, 1),
                    DiffuseTexture = "./troll2/troltooth_head_diffuse_5.JPG"
                }
            });

            var modelData = new ModelData()
            {

            };

            modelData.Name = "Cube";
            modelData.Nodes.Add(modelNode);

            return modelData;
        }

    }
}
