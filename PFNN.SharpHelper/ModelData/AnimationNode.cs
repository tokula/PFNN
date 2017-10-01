using SharpDX;
using System.Collections.Generic;

namespace PFNN.SharpHelper.ModelData
{
    /// <summary>
    /// Animation Node
    /// </summary>
    public class AnimationNode
    {
        /// <summary>
        /// Children Nodes
        /// </summary>
        public List<AnimationNode> Children { get; set; }

        /// <summary>
        /// Model Node that is target of this animation node
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// Input
        /// </summary>
        public List<float> Input { get; set; }

        /// <summary>
        /// Output matrices
        /// </summary>
        public List<Matrix> Output { get; set; }

        /// <summary>
        /// Input Tangents
        /// </summary>
        public List<Matrix> In_Tangent { get; set; }

        /// <summary>
        /// Output Tangents
        /// </summary>
        public List<Matrix> Out_Tangent { get; set; }

        /// <summary>
        /// Interpolation Type
        /// </summary>
        public Interpolation Interpolation { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public AnimationNode()
        {
            Target = "";
            Input = new List<float>();
            Output = new List<Matrix>();
            In_Tangent = new List<Matrix>();
            Out_Tangent = new List<Matrix>();
            Interpolation = 0;
            Children = new List<AnimationNode>();
        }
    }
}
