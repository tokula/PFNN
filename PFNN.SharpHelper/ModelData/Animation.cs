using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFNN.SharpHelper.ModelData
{
    /// <summary>
    /// Animations
    /// </summary>
    public class Animation
    {
        /// <summary>
        /// Animation Nodes
        /// </summary>
        public List<AnimationNode> Nodes { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Animation()
        {
            Nodes = new List<AnimationNode>();
        }

    }
}
