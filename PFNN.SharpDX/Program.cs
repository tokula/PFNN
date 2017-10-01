using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using SharpHelper;
using SharpHelper.Skinning;
using Buffer11 = SharpDX.Direct3D11.Buffer;
using PFNN.SharpDX.Classes.Heightmaps;

namespace PFNN.SharpDX
{
    static class Program
    {

        public struct Data
        {

        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            if (!SharpDevice.IsDirectX11Supported())
            {
                System.Windows.Forms.MessageBox.Show("DirectX11 Not Supported");
                return;
            }



            //render form
            RenderForm form = new RenderForm();
            form.Text = "PFNN";
            SharpFPS fpsCounter = new SharpFPS();

            //number of cube
            int count = 1000;

            using (SharpDevice device = new SharpDevice(form))
            {
                //Input layout for Skinning Mesh
                var description = new InputElement[]
                {
                    new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0),
                    new InputElement("NORMAL", 0, Format.R32G32B32_Float, 12, 0),
                    new InputElement("TEXCOORD", 0, Format.R32G32_Float, 24, 0),
                    new InputElement("BINORMAL", 0, Format.R32G32B32_Float, 32, 0),
                    new InputElement("TANGENT", 0, Format.R32G32B32_Float, 44, 0),
                    new InputElement("JOINT", 0, Format.R32G32B32A32_Float, 56, 0),
                    new InputElement("WEIGHT", 0, Format.R32G32B32A32_Float, 72, 0),
                };

                var staticShader = new SharpShader(device, "../../BasicTexture.hlsl",
                    new SharpShaderDescription()
                    {
                        VertexShaderFunction = "VSMain",
                        PixelShaderFunction = "PSMain"
                    }, description);

                var skinShader = new SharpShader(device, "../../BasicTextureSkin.hlsl",
                    new SharpShaderDescription()
                    {
                        VertexShaderFunction = "VSMain",
                        PixelShaderFunction = "PSMain"
                    }, description);


                var lightBuffer = skinShader.CreateBuffer<Vector4>();

                // string pathHeightmap = @"../../Content/Heightmaps/";
                // var heigthMap = Heightmap.Load(pathHeightmap + "hmap_004_smooth");
                // var model = new SharpModel(device, ColladaImporter.Import(path + "troll.dae"), staticShader, skinShader, path);
                // model = new SharpModel(device, heigthMap.GetModelData());

                string path = @"../../Content/Models/Troll/";


                var models = new List<SharpModel>();

                models.Add(new SharpModel(device, ColladaImporter.Import(path + "troll.dae"), staticShader, skinShader, path)
                    {
                        Postion = new Vector3(100, 0, 0)
                    });

                models.Add(new SharpModel(device, ColladaImporter.Import(path + "troll.dae"), staticShader, skinShader, path)
                    {
                        Postion = new Vector3(-100, 0, 0)
                    });

                fpsCounter.Reset();

                form.KeyDown += (sender, e) =>
                {
                    switch (e.KeyCode)
                    {
                        case Keys.Up:
                            if (count < 1000)
                                count++;
                            break;
                        case Keys.Down:
                            if (count > 0)
                                count--;
                            break;
                    }
                };

                int lastTick = Environment.TickCount;

                // Main loop
                RenderLoop.Run(form, () =>
                {
                    // Resizing
                    if (device.MustResize)
                    {
                        device.Resize();
                    }

                    // Apply state
                    device.UpdateAllStates();

                    // Clear color
                    device.Clear(Color.CornflowerBlue);


                    // Set transformation matrix
                    float ratio = (float)form.ClientRectangle.Width / (float)form.ClientRectangle.Height;
                    Matrix projection = Matrix.PerspectiveFovLH(3.14F / 3.0F, ratio, 1, 10000);
                    Matrix view = Matrix.LookAtLH(new Vector3(0, -300, 50), new Vector3(0, 0, 50), Vector3.UnitZ);
                    

                    float angle = Environment.TickCount / 2000.0F;
                    Vector3 light = new Vector3((float)Math.Sin(angle), (float)Math.Cos(angle), 0);
                    light.Normalize();
                    device.UpdateData<Vector4>(lightBuffer, new Vector4(light, 1));
                    device.DeviceContext.VertexShader.SetConstantBuffer(2, lightBuffer);

                    foreach (var model in models)
	                {
                        float animationTime = (Environment.TickCount - lastTick) / 1000.0F;

                        if (model.Animations.Any() && animationTime >= model.Animations.First().Duration)
                        {
                            lastTick = Environment.TickCount;
                            animationTime = 0;
                        }

                        model.SetTime(animationTime);

                        model.Draw(device, new SkinShaderInformation()
                        {
                            Transform = model.World * view * projection,
                            World = model.World
                        });
	                }

                    device.Font.Begin();

                    // Draw string
                    fpsCounter.Update();
                    device.Font.DrawString("FPS: " + fpsCounter.FPS, 0, 0);
                    device.Font.DrawString("Skinning Animation With Collada", 0, 30);
                    device.Font.DrawString("Count: " + count, 0, 60);
                    device.Font.DrawString("Environment.TickCount: " + Environment.TickCount, 0, 90);
                    
                    // Flush text to view
                    device.Font.End();
                    
                    // Present
                    device.Present();

                });

            }
        }
    }
}
