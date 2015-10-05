using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Windows;
using WPFlindao;

namespace WPFLindao
{
    public class Matrix_calc {
        public float[,] _matrix;
        public int rows;
        public int columns;

        public static float[,] toMatrix(string text) {
            int rowCount;
            int columnCount;
            float[,] _matrix;
            string[] allLines = Regex.Split(text, "\r\n");
            string[] line;

            rowCount = allLines.Length;
            columnCount = allLines[0].Split(' ').Length;
            _matrix = new float[rowCount, columnCount];

            for (int row = 0; row < rowCount; row++) {
                line = Regex.Split(allLines[row], " ");

                for (int column = 0; column < columnCount; column++) {
                    _matrix[row, column] = int.Parse(line[column]);
                }
            }
            return _matrix;
        }

        public static string toString(float[,] matrix) {
            string txt = "";
            float[,] _matrix;
            _matrix = matrix;
            if (_matrix != null)
            {
                for (int i = 0; i < _matrix.GetLength(0); i++)
                {

                    for (int j = 0; j < _matrix.GetLength(1); j++)
                    {

                        txt += _matrix[i, j].ToString() + " ";
                    }
                    txt += "\n";
                }
            }
            if (_matrix != null)
                return txt;
            else
                return "Não funcionou!";
        }

        public static PointCollection matrixColl(float[,] _matrix, float x, float y)
        {
            PointCollection p = new PointCollection();
            for (int i = 0; i < _matrix.GetLength(1); i++) {
                Point point = new Point();
                point.X = _matrix[0, i] + x;
                point.Y = _matrix[1, i] + y;
                p.Add(point);
            }
            return p;
        }

        public static float[,] CollMatrix(PointCollection pointColl, float x, float y)
        {
            float[,] _matrix = new float[2, pointColl.Count];
            for (int i = 0; i < pointColl.Count; i++) {
                _matrix[0, i] = (float)pointColl[i].X + x;
                _matrix[1, i] = (float)pointColl[i].Y + y;
            }
            return _matrix;
        }

        public static float[,] RotatePoly(float[,] _matrix, float a)
        {
            float[,] rotateMatrix = new float[2, 2];
            rotateMatrix[0, 0] = (float)Math.Cos(a * (Math.PI / 180));
            rotateMatrix[0, 1] = (float)-Math.Sin(a * (Math.PI / 180));
            rotateMatrix[1, 0] = (float)Math.Sin(a * (Math.PI / 180));
            rotateMatrix[1, 1] = (float)Math.Cos(a * (Math.PI / 180));

            return Mult2dArray(rotateMatrix, _matrix);

        }

        public static float[,] TranslatePoly(float[,] _matrix, float _x, float _y)
        {
            float[,] translateMatrix = new float[_matrix.GetLength(0), _matrix.GetLength(1)];
            for (int j = 0; j < _matrix.GetLength(1); j++) {
                translateMatrix[0, j] = _x;
                translateMatrix[1, j] = _y;
            }

            return Add2dArray(translateMatrix, _matrix);
        }

        public static float[,] ScalePoly(float[,] _matrix, float _x, float _y) {

            float[,] scaleMatrix = new float[2, 2];
            Array.Clear(scaleMatrix, 0, scaleMatrix.Length);
            scaleMatrix[0, 0] = _x;
            scaleMatrix[1, 1] = _y;

            return Mult2dArray(scaleMatrix, _matrix);

        }

        public static float[,] Add2dArray(float[,] A, float[,] B)//função de soma de duas matrizes
        {
            float[,] R = new float[A.GetLength(0), A.GetLength(1)];
            for (int i = 0; i < A.GetLength(0); i++)
            {
                for (int j = 0; j < A.GetLength(1); j++)
                {
                    R[i, j] = A[i, j] + B[i, j];
                }
            }

            return R;
        }

        public static float[,] Sub2dArray(float[,] A, float[,] B)//função de subtração de duas matrizes
        {
            float[,] R = new float[A.GetLength(0), A.GetLength(1)];
            for (int i = 0; i < A.GetLength(0); i++)
            {
                for (int j = 0; j < A.GetLength(1); j++)
                {
                    R[i, j] = A[i, j] - B[i, j];
                }
            }

            return R;
        }


        public static float[,] Mult2dArray(float[,] A, float[,] B)//função de multiplicação de duas matrizes
        {
            float[,] R = new float[A.GetLength(0), B.GetLength(1)];

            if (A.GetLength(1) == B.GetLength(0))
            {
                for (int i = 0; i < A.GetLength(0); i++)
                {
                    for (int j = 0; j < B.GetLength(1); j++)
                    {
                        for (int n = 0; n < A.GetLength(1); n++)
                        {
                            R[i, j] += A[i, n] * B[n, j];
                        }
                    }
                }
            }
            return R;
        }

        public static float[,] Mult2dArraybyRealNumb(float[,] A, float B)//função de multiplicação de matriz por numero real
        {
            float[,] R = new float[A.GetLength(0), A.GetLength(1)];
            for (int i = 0; i < A.GetLength(0); i++)
            {
                for (int j = 0; j < A.GetLength(1); j++)
                {
                    R[i, j] = A[i, j] * B;
                }
            }

            return R;
        }

        public static float[,] Transp2dArray(float[,] A)// função que retorna a transposta de uma matriz
        {
            float[,] R = new float[A.GetLength(1), A.GetLength(0)];
            for (int i = 0; i < A.GetLength(0); i++)
            {
                for (int j = 0; j < A.GetLength(1); j++)
                {
                    R[j, i] = A[i, j];
                }
            }

            return R;
        }

        public static bool Sim(float[,] A)// função que verifica se uma matriz é simétrica
        {
            float[,] transp = Transp2dArray(A);
            bool IsSim = true;

            if (A.GetLength(1) == A.GetLength(0))
            {
                for (int i = 0; i < A.GetLength(0); i++)
                {
                    for (int j = 0; j < A.GetLength(1); j++)
                    {
                        if (transp[i, j] != A[i, j])
                        {
                            IsSim = false;
                        }
                    }
                }
                return IsSim;
            }

            else
                return false;
        }

        public static float Determinante(float[,] A)// função que retorna a determinante de uma matriz quadrada 
        {
            float r = 0;
            float m = 1;
            float m2 = 1;

            if (A.GetLength(1) == A.GetLength(0))
            {
                if (A.GetLength(0) == 1)// matriz 1x1
                {
                    r = A[0, 0];
                }

                else if (A.GetLength(0) == 2)// matriz 2x2
                {
                    r = A[0, 0] * A[1, 1] - A[0, 1] * A[1, 0];
                }

                else if (A.GetLength(0) == 3)// matriz 3x3
                {
                    r = A[0, 0] * A[1, 1] * A[2, 2] + A[0, 1] * A[1, 2] * A[2, 0] + A[0, 2] * A[1, 0] * A[2, 1]
                      - A[0, 2] * A[1, 1] * A[2, 0] - A[0, 0] * A[1, 2] * A[2, 1] - A[0, 1] * A[1, 0] * A[2, 2];
                }

                else// matriz nxn, por método de shió
                {
                    bool isZero = true;
                    float[] L = new float[A.GetLength(1)];

                    for (int i = 0; i < A.GetLength(0); i++)
                    {
                        if (A[i, 0] != 0)
                        {
                            isZero = false;
                            if (i > 0)
                            {
                                for (int j = 0; j < A.GetLength(1); j++)
                                {
                                    L[j] = A[i, j];
                                    A[i, j] = A[0, j];
                                    A[0, j] = L[j];
                                }
                                m2 = -1;
                            }
                            i = A.GetLength(0);
                        }
                    }

                    if (!isZero)
                    {
                        m /= A[0, 0];

                        float[,] B = new float[A.GetLength(1) - 1, A.GetLength(0) - 1];
                        for (int i = 0; i < A.GetLength(0); i++)
                        {
                            A[i, 0] *= m;
                        }

                        for (int i = 1; i < A.GetLength(0); i++)
                        {
                            for (int j = 1; j < A.GetLength(1); j++)
                            {
                                B[i - 1, j - 1] = A[i, j] - (A[0, j] * A[i, 0]);
                            }
                        }

                        r = Determinante(B);
                    }
                }

            }
            return r / m * m2;
        }
        public static float Cofator(float[,] A, float L, float C) {
            //Função que retorna o cofator de um elemento de uma matriz
            float[,] B = new float[A.GetLength(0) - 1, A.GetLength(1) - 1];
            int i2 = 0;
            int j2 = 0;
            for (int i = 0; i < A.GetLength(0); i++)
            {
                if (i == L) i++;

                for (int j = 0; j < A.GetLength(1); j++)
                {
                    if (j == C) j++;
                    if (i < A.GetLength(0) && j < A.GetLength(1)) B[i2, j2] = A[i, j];

                    j2++;
                }
                i2++;
                j2 = 0;
            }

            return Determinante(B);

        }

        public static float[,] Adjunta(float[,] A) {
            //Função que retorna a matriz adjunta de uma matriz
            float[,] R = new float[A.GetLength(0), A.GetLength(1)];

            for (int i = 0; i < A.GetLength(0); i++)
            {
                for (int j = 0; j < A.GetLength(1); j++)
                {
                    R[i, j] = float.Parse(Math.Pow(-1, i + j + 2).ToString()) * Cofator(A, i, j);
                }
            }

            return Transp2dArray(R);
        }

        public static float[,] Inversa(float[,] A) {
            //Função que retorna a matriz inversa de uma matriz
            //jão, no botão, verifica se Determinante != 0
            float _det = Determinante(A);
            if (_det != 0)
                return Mult2dArraybyRealNumb(Adjunta(A), (1 / _det));
            else
                return null;
        }

        public static float[,] Formula(string formula) {
            string[] formulaPreview = formula.Split('|');
            int _i = Convert.ToInt32(formulaPreview[0]);
            int _j = Convert.ToInt32(formulaPreview[1]);
            var formulaCalculator = new System.Data.DataTable();
            float[,] _matrix = new float[_i, _j];

            for (int i = 0; i < _i; i++) {
                for (int j = 0; j < _j; j++) {
                    string i_formula = formulaPreview[2].Replace("i", (i + 1).ToString());
                    string x_formula = formulaPreview[3].Replace("j", (j + 1).ToString());
                    Console.WriteLine(i_formula);
                    _matrix[i, j] = Convert.ToSingle(formulaCalculator.Compute("(" + i_formula + " + " + x_formula + ")", ""));
                }
            }
            return _matrix;
        }

        /*public static float[,] Formula(string A,string B, string L, string C,string E,string E2, string E3)
        {
            float a = 1;
            float b = 1;
            float e1 = 1;
            float e2 = 1;
            float e3 = 1;

            bool ocarafezmerda = false;
            string qualmerdaelefez = "";

            #region Coisas Feias
            if (A == "j" || A == "J" || A == "coluna" || A == "colunas")
            {
                ocarafezmerda = true;
            }
            if (E == "j" || E == "J")
            {
                qualmerdaelefez = "a^j";
            }
            if (E == "i" || E == "I")
            {
                qualmerdaelefez = "a^i";
            }
            if (E2 == "j" || E2 == "J")
            {
                qualmerdaelefez = "b^j";
            }
            if (E2 == "i" || E2 == "I")
            {
                qualmerdaelefez = "b^i";
            }
            if (E == "j*i" || E == "J*I")
            {
                qualmerdaelefez = "a^j*i";
            }
            if (E2 == "i*j" || E2 == "I*J")
            {
                qualmerdaelefez = "b^i*j";
            }
            if (!ocarafezmerda)
            {
                a = float.Parse(A);//Coeficiente do I | Caso não seja usado tem que ser 1
            }
            b = float.Parse(B);//Coeficiente do J | Caso não seja usado tem que ser 1
            e1 = float.Parse(E);//Expoente do I | Caso não seja usado tem que ser 1
            e2 = float.Parse(E2);//Expoente do J | Caso não seja usado tem que ser 1
            e3 = float.Parse(E3);//Expoente da Expressão | Caso não seja usado tem que ser 1
            #endregion
            float[,] R = new float[int.Parse(L), int.Parse(C)];

            for (int i = 0; i < R.GetLength(0); i++)
            {
                for (int j = 0; j < R.GetLength(1); j++)
                {
                    #region Mais Coisa Feia
                    if (ocarafezmerda)
                    {
                        a = j;
                    }
                    switch (qualmerdaelefez)
                    {
                        case "a^j":
                            e1 = j;
                            break;
                        case "a^i":
                            e1 = i;
                            break;
                        case "b^i":
                            e2 = i;
                            break;
                        case "b^j":
                            e2 = j;
                            break;
                        case "b^j*i":
                            e2 = j*i;
                            break;
                        case "a^j*i":
                            e3 = j*i;
                            break;
                    }
                    #endregion
                    R[i, j] = (float)Math.Pow(a * (float)Math.Pow(i, e1) + b * (float)Math.Pow(j, e2), e3);
                }
            }
            return R;
        }*/
    }
}
