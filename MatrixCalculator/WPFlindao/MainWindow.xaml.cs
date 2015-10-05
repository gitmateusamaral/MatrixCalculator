using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using WPFLindao;

namespace WPFlindao
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static PointCollection myPointCollection = new PointCollection();
        public static Canvas canvas;

        public MainWindow() {
            InitializeComponent();
            canvas = matrixCanvas;
            DrawCartesianGrid(25, "#555555");
            
            Polygon  polygon = new Polygon();
            polygon.FillRule = FillRule.Nonzero;
            polygon.Fill = new SolidColorBrush(Colors.DarkOrchid);
            polygon.StrokeThickness = 1;
            polygon.Stroke = new SolidColorBrush(Colors.AntiqueWhite);
            polygon.Points = myPointCollection;

            canvas.Children.Add(polygon);

        }


        #region Handler_methods
        public static void DrawLine(int X1, int Y1, int X2, int Y2, String color, int thickness) {

            Line line = new Line();
            line.X1 = X1;
            line.X2 = X2;
            line.Y1 = Y1;
            line.Y2 = Y2;

            line.StrokeThickness = thickness;
            line.Stroke = (Brush)new BrushConverter().ConvertFromString(color);

            canvas.Children.Add(line);

        }
        public static void DrawCartesianGrid(int gridsize, string color) {

            for (int i = 0; i < 250 - gridsize; i++) {
                DrawLine(gridsize * i, 0, gridsize * i, 250, color, 1);
                DrawLine(0, gridsize * i, 250, gridsize * i, color, 1);
            }

            DrawLine(0, 250 / 2, 250, 250 / 2, "#ededed", 1);
            DrawLine(250 / 2, 0, 250 / 2, 250, "#ededed", 1);
        }
        private void Text(double x, double y, string text, Color color)
        {

            TextBlock textBlock = new TextBlock();

            textBlock.Text = text;

            textBlock.Foreground = new SolidColorBrush(color);

            Canvas.SetLeft(textBlock, x);

            Canvas.SetTop(textBlock, y);

            canvas.Children.Add(textBlock);

        }
        #endregion

        #region Canvas_methods
        public Point GetMousePosition() {
            Point point = Mouse.GetPosition(canvas);
            Text(point.X, point.Y, "(" + point.X + "," + point.Y + ")", Colors.White);
            if(point.X > 0 && point.Y > 0) myPointCollection.Add(point);

            if (buttonsDisplay.Children.Count < 10)
            {
                Button b = new Button();
                b.Content = "Change";
                b.Click += delegate
                {
                    int index = buttonsDisplay.Children.IndexOf(b);
                    myPointCollection[index] = new Point(double.Parse((xDisplay.Children[index] as TextBox).Text),
                    double.Parse((yDisplay.Children[index] as TextBox).Text));
                };
                buttonsDisplay.Children.Add(b);

                TextBox tx = new TextBox();
                tx.Text = point.X.ToString();
                xDisplay.Children.Add(tx);

                TextBox ty = new TextBox();
                ty.Text = point.Y.ToString();
                yDisplay.Children.Add(ty);
            }



            return new Point(point.X, point.Y);
        }
        public void updateCanvas()
        {
            foreach (object t in canvas.Children)
            {
                if (t is TextBlock) canvas.Children.Remove(t as TextBlock);
            }
            for (int i = 0; i < myPointCollection.Count; i++)
            {
                (xDisplay.Children[i] as TextBox).Text = myPointCollection[i].X.ToString();
                (yDisplay.Children[i] as TextBox).Text = myPointCollection[i].Y.ToString();
                Text(myPointCollection[i].X,
                    myPointCollection[i].Y, "(" +
                    Math.Round((decimal)myPointCollection[i].X, 2) + "," +
                    Math.Round((decimal)myPointCollection[i].Y, 2) + ")",
                    Colors.White);
            }
        }
        public static void ClearPoints() {
            myPointCollection.Clear();
            canvas.Children.Clear();
            DrawCartesianGrid(25, "#555555");
            Polygon polygon = new Polygon();
            polygon.FillRule = FillRule.Nonzero;
            polygon.Fill = new SolidColorBrush(Colors.DarkOrchid);
            polygon.StrokeThickness = 1;
            polygon.Stroke = new SolidColorBrush(Colors.AntiqueWhite);
            polygon.Points = myPointCollection;

            canvas.Children.Add(polygon);
        }
        private void RotatePoly(object sender, RoutedEventArgs e)
        {
            Polygon _polygon;
            canvas.Children.Clear();
            DrawCartesianGrid(25, "#555555");
            _polygon = new Polygon();
            _polygon.Fill = new SolidColorBrush(Colors.DarkOrchid);
            _polygon.StrokeThickness = 1;
            _polygon.Stroke = new SolidColorBrush(Colors.AntiqueWhite);
            myPointCollection = Matrix_calc.matrixColl(
                Matrix_calc.RotatePoly(
                Matrix_calc.CollMatrix(
                myPointCollection, -125, -125), float.Parse(rotate.Text)), 125, 125);
            _polygon.Points = myPointCollection;
            canvas.Children.Add(_polygon);
            updateCanvas();
        }
        private void TranslatePoly(object sender, RoutedEventArgs e)
        {
            Polygon _polygon;
            canvas.Children.Clear();
            DrawCartesianGrid(25, "#555555");
            _polygon = new Polygon();
            _polygon.Fill = new SolidColorBrush(Colors.DarkOrchid);
            _polygon.StrokeThickness = 1;
            _polygon.Stroke = new SolidColorBrush(Colors.AntiqueWhite);
            myPointCollection = Matrix_calc.matrixColl(
                Matrix_calc.TranslatePoly(
                Matrix_calc.CollMatrix(myPointCollection, 0, 0),
                float.Parse(transladarX.Text), float.Parse(transladarY.Text)), 0, 0);
            _polygon.Points = myPointCollection;
            canvas.Children.Add(_polygon);
            updateCanvas();
        }
        private void ScalePoly(object sender, RoutedEventArgs e) {

            Polygon _polygon;
            canvas.Children.Clear();
            DrawCartesianGrid(25, "#555555");
            _polygon = new Polygon();
            _polygon.Fill = new SolidColorBrush(Colors.DarkOrchid);
            _polygon.StrokeThickness = 1;
            _polygon.Stroke = new SolidColorBrush(Colors.AntiqueWhite);
            myPointCollection = Matrix_calc.matrixColl(
                Matrix_calc.ScalePoly(
                Matrix_calc.CollMatrix(myPointCollection, -125, -125),
                float.Parse(escalonarX.Text), float.Parse(escalonarY.Text)), 125, 125);
            _polygon.Points = myPointCollection;
            canvas.Children.Add(_polygon);
            updateCanvas();
        }
        #endregion

        #region Matrix_validation
        public static void NumberValidationTextBox(object sender, TextCompositionEventArgs e) {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        public void exceptionsMatrixTextBox(ref Label label,TextBox sender)
        {
            string _matrixInputText = (sender as TextBox).Text;
            if (String.IsNullOrEmpty(_matrixInputText) || String.IsNullOrWhiteSpace(_matrixInputText))
            {
                label.Content = "No matrix";
                label.Foreground = (Brush)new BrushConverter().ConvertFromString("#ededed");
            }
            else
            {
                string[] _matrixInputTextSplit = Regex.Split(_matrixInputText, "\r\n");
                int count = 0;
                for (int i = 0; i < _matrixInputTextSplit.Length; i++)
                {
                    string[] _matrixInputSecondaryTextSplit = _matrixInputTextSplit[i].Split(' ');

                    if (i == 0)
                    {
                        count = _matrixInputSecondaryTextSplit.Length;
                    }
                    else if (count != _matrixInputSecondaryTextSplit.Length)
                    {
                        label.Content = "!";
                        label.Foreground = (Brush)new BrushConverter().ConvertFromString("#e74c3c");
                        break;
                    }

                    for (int j = 0; j < _matrixInputSecondaryTextSplit.Length; j++)
                    {
                        if (String.IsNullOrWhiteSpace(_matrixInputSecondaryTextSplit[j])
                         || String.IsNullOrEmpty(_matrixInputSecondaryTextSplit[j]))
                        {
                            label.Content = "!";
                            label.Foreground = (Brush)new BrushConverter().ConvertFromString("#e74c3c");
                            break;
                        }
                        label.Content = "Ok";
                        label.Foreground = (Brush)new BrushConverter().ConvertFromString("#2ecc71");
                    }
                }

                if ((string)label.Content == "Ok")
                {
                    try
                    {
                        Console.WriteLine(Matrix_calc.toMatrix(_matrixInputText).ToString());
                        label.Content = "Ok";
                        label.Foreground = (Brush)new BrushConverter().ConvertFromString("#2ecc71");
                    }
                    catch
                    {
                        label.Content = "!";
                        label.Foreground = (Brush)new BrushConverter().ConvertFromString("#e74c3c");
                    }
                }
            }
        }
        private void matrixInputTextChanged(object sender, TextChangedEventArgs e) {

            if ((sender as TextBox).Name == "matrixInput1")
            {
                exceptionsMatrixTextBox(ref validMatrix1, (sender as TextBox));
            }
            else
            {
                exceptionsMatrixTextBox(ref validMatrix2, (sender as TextBox));
            }
        }
        private void matrixOperation(object sender, RoutedEventArgs e) {
            float[,] array2D = new float[,] { { 1, 2 }, { 3, 4 }, { 5, 6 }, { 7, 8 } };
            if ((string)validMatrix1.Content == "Ok" && ((string)validMatrix2.Content == "Ok" ||
                comboBoxOperation.Text == "Determinante" || comboBoxOperation.Text == "Inversa" ||
                comboBoxOperation.Text == "Transposta"))
            { 
                switch (comboBoxOperation.Text)
                {
                    case "Soma":
                        displayMatrix.Text =
                            Matrix_calc.toString(Matrix_calc.Add2dArray(
                                Matrix_calc.toMatrix(matrixInput1.Text), Matrix_calc.toMatrix(matrixInput2.Text)));
                        break;
                    case "Subtração":
                        displayMatrix.Text =
                            Matrix_calc.toString(Matrix_calc.Sub2dArray(
                                Matrix_calc.toMatrix(matrixInput1.Text), Matrix_calc.toMatrix(matrixInput2.Text)));
                        break;
                    case "Multiplicação":
                        displayMatrix.Text =
                            Matrix_calc.toString(Matrix_calc.Mult2dArray(
                                Matrix_calc.toMatrix(matrixInput1.Text), Matrix_calc.toMatrix(matrixInput2.Text)));
                        break;
                    case "Multiplicação por número real":
                        try {
                            displayMatrix.Text =
                                Matrix_calc.toString(Matrix_calc.Mult2dArraybyRealNumb(
                                    Matrix_calc.toMatrix(matrixInput2.Text), float.Parse(matrixInput1.Text)));
                        } catch {
                            displayMatrix.Text = "Não válido!";
                        }
                        break;
                    case "Determinante":
                        displayMatrix.Text =
                            Matrix_calc.Determinante(Matrix_calc.toMatrix(matrixInput1.Text)).ToString();
                        break;
                    case "Inversa":
                        displayMatrix.Text =
                            Matrix_calc.toString(Matrix_calc.Inversa(Matrix_calc.toMatrix(matrixInput1.Text)));
                        break;
                    case "Transposta":
                        displayMatrix.Text =
                            Matrix_calc.toString(Matrix_calc.Transp2dArray(Matrix_calc.toMatrix(matrixInput1.Text)));
                        break;
                            default: Console.WriteLine("Não funcionou!");
                        break;
                }
            }
        }
        private void formulaGenerate(object sender, RoutedEventArgs e) {
            string result = Matrix_calc.toString(Matrix_calc.Formula(formulaInput.Text));
            Console.WriteLine(result);
            result = result.TrimEnd();
            matrixInput1.Text = result;
        }
        #endregion
    }
}
