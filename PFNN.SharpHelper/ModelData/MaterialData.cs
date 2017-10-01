using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFNN.SharpHelper.ModelData
{
    /// <summary>
    /// Material Data
    /// </summary>
    public class MaterialData
    {

        /// <summary>
        /// Ambient Color
        /// </summary>
        public Vector4 Ambient { get; set; }

        /// <summary>
        /// Diffuse Color
        /// </summary>
        public Vector4 Diffuse { get; set; }

        /// <summary>
        /// Specular Color
        /// </summary>
        public Vector4 Specular { get; set; }

        /// <summary>
        /// Specular Power
        /// </summary>
        public float SpecularPower { get; set; }

        /// <summary>
        /// Emissiva Color
        /// </summary>
        public Vector4 Emissive { get; set; }

        /// <summary>
        /// Diffuse Texture Name
        /// </summary>
        public string DiffuseTexture { get; set; }

        /// <summary>
        /// Normal Texture Name
        /// </summary>
        public string NormalTexture { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public MaterialData()
        {
            Ambient = new Vector4(0, 0, 0, 0);
            DiffuseTexture = "";
            NormalTexture = "";

            Diffuse = new Vector4();
            Specular = new Vector4(0, 0, 0, 0);
            Emissive = new Vector4();
            SpecularPower = 0;
        }

    }
}
