﻿#version 430

// Входные переменные
attribute vec3 vPosition; /* позиция вершины, 
передаётся из приложения в вершинный шейдер */

// Выходные переменные
out vec3 glPosition; /* позиция вершины, передаётся далее 
по конвееру от вершинного фрагментному шейдеру */

void main ()
{
    gl_Position = vec4(vPosition, 1.0);
    glPosition = vPosition;
}

/* Вершинный шейдер вызывается для каждой вершины.
Его задача - переложить интерполированные 
координаты вершин в выходную переменную */