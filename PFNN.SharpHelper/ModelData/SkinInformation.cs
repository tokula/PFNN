using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFNN.SharpHelper.ModelData
{
    /// <summary>
    /// Skin Information
    /// </summary>
    public class SkinInformation
    {
        /// <summary>
        /// Bind Matrix
        /// </summary>
        public Matrix BindMatrix { get; set; }

        /// <summary>
        /// Joint Names List
        /// </summary>
        public List<string> JointNames { get; set; }


        /// <summary>
        /// Inverse Binding Matrices
        /// </summary>
        public List<Matrix> InverseBinding { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public SkinInformation()
        {
            JointNames = new List<string>();
            InverseBinding = new List<Matrix>();
        }

    }
}
