using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.IO;

namespace RayTracing
{
    public partial class Form1 : Form
    {
        int BasicProgramID; // Номер дескриптора на графической карте
        int BasicVertexShader; // Адрес вершинного шейдера  
        int BasicFragmentShader; // Адрес фрагментного шейдера

        int vbo_position; // хранение дескриптора объекта массива вершин
        int attribute_vpos; // передаётся из приложения в вершинный шейдер
        int attribute_rotate; // Адрес параметра вращения сферы

        float angle = 0;

        Vector3 Rotate; // Для вращения сферы

        void loadShader(String filename, ShaderType type, int program, out int address)
        {
            address = GL.CreateShader(type); // создаёт объект шейдера
            using (StreamReader sr = new StreamReader(filename))
            {
                GL.ShaderSource(address, sr.ReadToEnd());
                // загружает исходный код в созданный шейдерный объект
            }
            GL.CompileShader(address); // компилирование шейдера
            GL.AttachShader(program, address); // компоновка в шейдерную программу
            Console.WriteLine(GL.GetShaderInfoLog(address)); // 
        }

        private void InitShaders()
        {
            string glVersion = GL.GetString(StringName.Version);
            string glslVersion = GL.GetString(StringName.ShadingLanguageVersion);
            // создание объекта программы
            BasicProgramID = GL.CreateProgram();
            loadShader("..\\..\\Shaders\\raytracing.vert", ShaderType.VertexShader, BasicProgramID,
            out BasicVertexShader);
            loadShader("..\\..\\Shaders\\raytracing.frag", ShaderType.FragmentShader, BasicProgramID,
            out BasicFragmentShader);
            //Компановка программы
            GL.LinkProgram(BasicProgramID);
            // Проверить успех компановки
            int status = 0;
            GL.GetProgram(BasicProgramID, GetProgramParameterName.LinkStatus, out status);
            Console.WriteLine(GL.GetProgramInfoLog(BasicProgramID));

            attribute_vpos = GL.GetAttribLocation(BasicProgramID, "vPosition");
            attribute_rotate = GL.GetUniformLocation(BasicProgramID, "Rotate");

            Vector3[] vertdata = new Vector3[] // массив вершин
            {
                            new Vector3(-1f, -1f, 0f),
                            new Vector3( 1f, -1f, 0f),
                            new Vector3( 1f, 1f, 0f),
                            new Vector3(-1f, 1f, 0f)
            };

            GL.GenBuffers(1, out vbo_position);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_position);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertdata.Length * Vector3.SizeInBytes),
                                   vertdata, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attribute_vpos, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.UseProgram(BasicProgramID);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.Uniform3(attribute_rotate, Rotate);
        }

        private void Draw()
        {
            GL.Viewport(0, 0, glControl1.Width, glControl1.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
            angle += 0.2f;
            Rotate = new Vector3(-2 + (float)Math.Cos(angle), -1 , -1 + (float)Math.Cos(angle));

            GL.EnableVertexAttribArray(attribute_vpos);
            GL.DrawArrays(PrimitiveType.Quads, 0, 4);
            GL.DisableVertexAttribArray(attribute_vpos);

            
            glControl1.SwapBuffers();

            GL.UseProgram(0);
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            InitShaders();
            Draw();
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void glControl1_Resize(object sender, EventArgs e)
        {
            InitShaders();
            Draw();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            InitShaders();
            Draw();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Application.Idle += Application_Idle;
        }

        private void Application_Idle(Object sender, EventArgs e)
        {
            while (glControl1.IsIdle)
            {
                glControl1.Invalidate();
            }
        }
    }
}
