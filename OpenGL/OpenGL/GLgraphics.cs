using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics.OpenGL;


namespace OpenGL
{
    class GLgraphics
    {
        // параметры положения камеры
        Vector3 cameraPosition = new Vector3(2, 3, 4);
        Vector3 cameraDirecton = new Vector3(0, 0, 0);
        Vector3 cameraUp = new Vector3(0, 0, 1);

        // камера устремлена в центр и движется по окружающей сфере
        public float latitude = 47.98f; // широта
        public float longitude = 60.41f; // долгота
        public float radius = 5.385f; // радиус сферы

        float rotateAngle; // угол поворота

        public List<int> texturesIDs = new List<int>();

        public void Setup(int width, int height)
        {
            GL.ClearColor(Color.DarkGray); // заливка буфера серым цветом
            GL.ShadeModel(ShadingModel.Smooth); // устанавливаем тип отрисовки 
                                                // полигонов с оттенками (smooth shading). 
            GL.Enable(EnableCap.DepthTest); // Включаем буфер глубины

            // устанавливаем матрицу проекции
            Matrix4 perspectiveMat = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4, width / (float)height, 1, 64);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspectiveMat);
            SetupLightning();
        }

        public void Update()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Matrix4 viewMat = Matrix4.LookAt(cameraPosition, cameraDirecton, cameraUp);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref viewMat);
            Render();
            cameraPosition = new Vector3(// переход от сферических к декартовым координатам
    (float)(radius * Math.Cos(Math.PI / 180.0f * latitude) * Math.Cos(Math.PI / 180.0f * longitude)),
    (float)(radius * Math.Cos(Math.PI / 180.0f * latitude) * Math.Sin(Math.PI / 180.0f * longitude)),
    (float)(radius * Math.Sin(Math.PI / 180.0f * latitude)));
            rotateAngle += 0.1f; // делаем вращение
        }

        private void drawTestQuad()
        {
            GL.Begin(PrimitiveType.Quads);
            GL.Color3(Color.Blue);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Color3(Color.Red);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);
            GL.Color3(Color.White);
            GL.Vertex3(1.0f, 1.0f, -1.0f);
            GL.Color3(Color.Green);
            GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.End();
        }

        public void Render()
        {
            drawTestQuad();
            GL.PushMatrix();
            GL.Translate(1, 1, 1); // перемещение
            GL.Rotate(rotateAngle, Vector3.UnitZ); // поворот
            GL.Scale(0.5f, 0.5f, 0.5f); // масштабирование
            drawTestQuad();
            drawTexturedQuad();
            GL.Color3(Color.BlueViolet);
            DrawSphere(1.0f, 20, 20);
            GL.PopMatrix();
        }

        public int LoadTexture(String filePath)
        {
            try
            {
                Bitmap image = new Bitmap(filePath);
                int texID = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, texID);
                BitmapData data = image.LockBits(
                    new Rectangle(0, 0, image.Width, image.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0,
                    PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                image.UnlockBits(data);
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
                return texID;
            }
            catch (System.IO.FileNotFoundException е)
            {
                return -1;
            }
        }

        private void drawTexturedQuad()
        {
            int texID = LoadTexture("..\\..\\..\\img\\unn.jpg");
            texturesIDs.Add(texID);
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texturesIDs[0]);
            GL.Begin(PrimitiveType.Quads);
            GL.Color3(Color.Blue);
            GL.TexCoord2(0.0, 0.0);
            GL.Vertex3(1.0f, -1.0f, -3.0f);
            GL.Color3(Color.Red);
            GL.TexCoord2(0.0, 1.0);
            GL.Vertex3(1.0f, 1.0f, -3.0f);
            GL.Color3(Color.White);
            GL.TexCoord2(1.0, 1.0);
            GL.Vertex3(3.0f, 1.0f, -3.0f);
            GL.Color3(Color.Green);
            GL.TexCoord2(1.0, 0.0);
            GL.Vertex3(3.0f, -1.0f, -3.0f);
            GL.End();
            GL.Disable(EnableCap.Texture2D);
        }

        void SetupLightning()
        {
            GL.Enable(EnableCap.Lighting); // расчёт освещения
            GL.Enable(EnableCap.Light0); // нулевой источник цвета
            GL.Enable(EnableCap.ColorMaterial); // освещение цветных вершин

            // устанавливаем положение источника света
            Vector4 lightPosition = new Vector4(1.0f, 1.0f, 4.0f, 0.0f);
            GL.Light(LightName.Light0, LightParameter.Position, lightPosition);

            // устанавливаем ambient цвет источника – цвет, 
            // который будет иметь объект, не освещенный источником
            Vector4 ambientColor = new Vector4(0.2f, 0.2f, 0.2f, 1.0f);
            GL.Light(LightName.Light0, LightParameter.Ambient, ambientColor);

            // устанавливаем diffuse цвет источника – цвет, 
            // который будет иметь объект, освещенный источником
            Vector4 diffuseColor = new Vector4(0.6f, 0.6f, 0.6f, 1.0f);
            GL.Light(LightName.Light0, LightParameter.Diffuse, diffuseColor);

            // Для создания бликов на поверхностях
            // устанавливаем всем материалам зеркальную составляющую
            Vector4 materialSpecular = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            GL.Material(MaterialFace.Front, MaterialParameter.Specular, materialSpecular);
            float materialShininess = 100;
            GL.Material(MaterialFace.Front, MaterialParameter.Shininess, materialShininess);

        }

        private void DrawSphere(double r, int nx, int ny)
        {
            int ix, iy;
            double x, y, z;
            for (iy = 0; iy < ny; ++iy)
            {
                GL.Begin(BeginMode.QuadStrip);
                for (ix = 0; ix <= nx; ++ix)
                {
                    x = r * Math.Sin(iy * Math.PI / ny) * Math.Cos(2 * ix * Math.PI / nx);
                    y = r * Math.Sin(iy * Math.PI / ny) * Math.Sin(2 * ix * Math.PI / nx);
                    z = r * Math.Cos(iy * Math.PI / ny);
                    GL.Normal3(x, y, z);
                    GL.Vertex3(x, y, z);

                    x = r * Math.Sin((iy + 1) * Math.PI / ny) * Math.Cos(2 * ix * Math.PI / nx);
                    y = r * Math.Sin((iy + 1) * Math.PI / ny) * Math.Sin(2 * ix * Math.PI / nx);
                    z = r * Math.Cos((iy + 1) * Math.PI / ny);
                    GL.Normal3(x, y, z);
                    GL.Vertex3(x, y, z);
                }
                GL.End();
            }
        }

    }
}
