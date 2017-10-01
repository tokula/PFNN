using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFNN.SharpHelper.ModelData
{
    /// <summary>
    /// Model Node
    /// </summary>
    public class ModelNode
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Node Type
        /// </summary>
        public NodeType Type { get; set; }

        /// <summary>
        /// World Matrix
        /// </summary>
        public Matrix World { get; set; }

        /// <summary>
        /// Children Nodes
        /// </summary>
        public List<ModelNode> Children { get; set; }

        /// <summary>
        /// Geometries Inside this node
        /// </summary>
        public List<ModelGeometry> Geometries { get; set; }

        /// <summary>
        /// Skinning Information
        /// </summary>
        public SkinInformation Skinning { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ModelNode()
        {
            Children = new List<ModelNode>();
            Geometries = new List<ModelGeometry>();
            World = Matrix.Identity;
        }
    }
}
