using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFNN.SharpHelper.ModelData
{
    /// <summary>
    /// Model Data
    /// </summary>
    public class ModelData
    {
        /// <summary>
        /// Model Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Nodes
        /// </summary>
        public List<ModelNode> Nodes { get; set; }

        /// <summary>
        /// Animations
        /// </summary>
        public List<Animation> Animations { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ModelData()
        {
            Nodes = new List<ModelNode>();
            Animations = new List<Animation>();
        }
    }
}
