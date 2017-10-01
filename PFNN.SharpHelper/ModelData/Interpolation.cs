using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFNN.SharpHelper.ModelData
{
    /// <summary>
    /// Interpolation Type
    /// </summary>
    public enum Interpolation
    {
        /// <summary>
        /// Undefined
        /// </summary>
        Undefined = 0,
        /// <summary>
        /// Linear
        /// </summary>
        Linear = 1,
        /// <summary>
        /// Bezier
        /// </summary>
        Bezier = 2
    }
}
