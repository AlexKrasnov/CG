using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;
using static System.Math;

namespace OpenGL
{
    class View
    {
        Bitmap textureImage;
        int VBOtexture; // номер текстуры в памяти видеокарты
        public void Load2DTexture()
        {
            GL.BindTexture(TextureTarget.Texture2D, VBOtexture);
            BitmapData data = textureImage.LockBits(
                new Rectangle(0, 0, textureImage.Width, textureImage.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                PixelType.UnsignedByte, data.Scan0); // загружает текстуру в память видеокарты
            textureImage.UnlockBits(data);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int)TextureMinFilter.Linear);
            ErrorCode Er = GL.GetError();
            string str = Er.ToString();
        }

        public void generateTextureImage(int LayerNumber) // генерация изображения из томограммы
        {
            textureImage = new Bitmap(Bin.X, Bin.Y);
            for (int i = 0; i < Bin.X; i++)
                for (int j = 0; j < Bin.Y; j++)
                {
                    int PixelNumber = i + j * Bin.X + LayerNumber * Bin.X * Bin.Y;
                    textureImage.SetPixel(i, j, TransferFunction(Bin.array[PixelNumber]));
                }
        }

        public void DrawTexture()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.Texture2D); // включение 2D-текстурирования
            GL.BindTexture(TextureTarget.Texture2D, VBOtexture);

            // отрисовка прямоугольника с наложенной структурой
            GL.Begin(BeginMode.Quads);
            GL.Color3(Color.White);
            GL.TexCoord2(0f, 0f);
            GL.Vertex2(0, 0);
            GL.TexCoord2(0f, 1f);
            GL.Vertex2(0, Bin.Y);
            GL.TexCoord2(1f, 1f);
            GL.Vertex2(Bin.X, Bin.Y);
            GL.TexCoord2(1f, 0f);
            GL.Vertex2(Bin.X, 0);
            GL.End();

            GL.Disable(EnableCap.Texture2D); // выключение 2D-текстурирования
        }

        public void SetupView(int width, int height)
        {
            //currentLayer = 0;
            GL.ShadeModel(ShadingModel.Smooth); // интерполирование цветов
            GL.MatrixMode(MatrixMode.Projection); // инициализация матрицы проекции
            GL.LoadIdentity(); // делаем матрицу равной тождественному преобразованию
            GL.Ortho(0, Bin.X, 0, Bin.Y, -1, 1); // ортогональное проецирование 
                                                 // массива данных томограммы
            GL.Viewport(0, 0, width, height); // задание начальной точки, а также ширины
                                              // и высоты доступной области на экране
        }

        int Clamp(int value, int min, int max)
        {
            return Max(0, Min(value, max));
        }

        Color TransferFunction(short value)
        {
            int min = 0;
            int max = 2000;
            int newval = Clamp((value - min) * 255 / (max - min), 0, 255);
            return Color.FromArgb(newval, newval, newval);
        }

        public void DrawQuads(int layerNumber)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Begin(BeginMode.Quads);
            for (int x = 0; x < Bin.X - 1; x++)
                for (int y = 0; y < Bin.Y - 1; y++)
                {
                    short value;

                    // 1 вершина
                    value = Bin.array[x + y * Bin.X + layerNumber * Bin.X * Bin.Y];
                    GL.Color3(TransferFunction(value));
                    GL.Vertex2(x, y);

                    // 2 вершина
                    value = Bin.array[x + (y + 1) * Bin.X + layerNumber * Bin.X * Bin.Y];
                    GL.Color3(TransferFunction(value));
                    GL.Vertex2(x, y + 1);

                    // 3 вершина
                    value = Bin.array[x + 1 + (y + 1) * Bin.X + layerNumber * Bin.X * Bin.Y];
                    GL.Color3(TransferFunction(value));
                    GL.Vertex2(x + 1, y + 1);

                    // 4 вершина
                    value = Bin.array[x + 1 + y * Bin.X + layerNumber * Bin.X * Bin.Y];
                    GL.Color3(TransferFunction(value));
                    GL.Vertex2(x + 1, y);
                }

            GL.End();
        }

        public void QuadStrip(int layerNumber)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Begin(BeginMode.Quads);
            short value;
            for (int x = 0; x < Bin.X - 1; x++)
            {
                value = Bin.array[x + layerNumber * Bin.X * Bin.Y];
                GL.Color3(TransferFunction(value));
                GL.Vertex2(x, 0);

                for (int y = 0; y < Bin.Y - 1; y++)
                {
                    //  добавляем 2 вершины к 4-угольнику
                    value = Bin.array[x + (y + 1) * Bin.X + layerNumber * Bin.X * Bin.Y];
                    GL.Color3(TransferFunction(value));
                    GL.Vertex2(x, y + 1);

                    value = Bin.array[x + 1 + (y + 1) * Bin.X + layerNumber * Bin.X * Bin.Y];
                    GL.Color3(TransferFunction(value));
                    GL.Vertex2(x + 1, y + 1);
                }
            }
            value = Bin.array[Bin.X - 1 + layerNumber * Bin.X * Bin.Y];
            GL.Color3(TransferFunction(value));
            GL.Vertex2(Bin.X - 1, 0);

            GL.End();
        }
    }
}
