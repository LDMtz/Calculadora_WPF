using Calculadora.Operadores;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calculadora
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnCE_Click(object sender, RoutedEventArgs e)
        {
            if (txtBPantallaAnterior.Text == "")
            {
                setValueNum1(0);
                txtBPantallaActual.Text = num1.ToString();
            }
            else
            {
                setValueNum2(0);
                txtBPantallaActual.Text = num2.ToString();
            }
        }

        private void btnC_Click(object sender, RoutedEventArgs e)
        {
            setValueNum1(0);
            setValueNum2(0);
            txtBPantallaAnterior.Text = "";
            txtBPantallaActual.Text = "0";
            setResultado(double.NaN);
        }

        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            int cantidad = txtBPantallaActual.Text.Length;
            txtBPantallaActual.Text = txtBPantallaActual.Text.Substring(0, cantidad - 1);

            if (cantidad == 1)
                txtBPantallaActual.Text = "0";
        }

        private void btnPorcentaje_Click(object sender, RoutedEventArgs e)
        {

            ClickToOperator('%');
        }

        private void btn7_Click(object sender, RoutedEventArgs e)
        {
            AddToActualScreen('7');
        }

        private void btn8_Click(object sender, RoutedEventArgs e)
        {
            AddToActualScreen('8');
        }

        private void btn9_Click(object sender, RoutedEventArgs e)
        {
            AddToActualScreen('9');
        }

        private void btnMultiplicar_Click(object sender, RoutedEventArgs e)
        {
            ClickToOperator('X');
        }

        private void btn4_Click(object sender, RoutedEventArgs e)
        {
            AddToActualScreen('4');
        }

        private void btn5_Click(object sender, RoutedEventArgs e)
        {
            AddToActualScreen('5');
        }

        private void btn6_Click(object sender, RoutedEventArgs e)
        {
            AddToActualScreen('6');
        }

        private void btnMenos_Click(object sender, RoutedEventArgs e)
        {
            ClickToOperator('-');
        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            AddToActualScreen('1');
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            AddToActualScreen('2');
        }

        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            AddToActualScreen('3');
        }

        private void btnMas_Click(object sender, RoutedEventArgs e)
        {
            ClickToOperator('+');
        }

        private void btnPunto_Click(object sender, RoutedEventArgs e)
        {
            AddToActualScreen('.');
        }

        private void btn0_Click(object sender, RoutedEventArgs e)
        {
            AddToActualScreen('0');
        }

        private void btnIgual_Click(object sender, RoutedEventArgs e)
        {
            AddToPreviousScreen('=');
        }

        private void btnDivision_Click(object sender, RoutedEventArgs e)
        {
            ClickToOperator('/');
        }

        private void ValidateEqualSignInPreviousScreen(char car)
        {
            if (txtBPantallaAnterior.Text == "")
                return;

            if (!txtBPantallaAnterior.Text.Contains("="))
            {
                setValueNum2(ConvertToNumber(txtBPantallaActual.Text));
                realizarOperacion();
                txtBPantallaAnterior.Text += " " + getValueNum2() + " " + car;
                txtBPantallaActual.Text = getResultado().ToString();
                setValueNum1(getResultado());
                setResultado(double.NaN);
            }
            else
            {
                //si ya hay un operador
                setValueNum1(ConvertToNumber(txtBPantallaActual.Text));
                realizarOperacion();
                char oper = default;
                char[] noOperadores = { ' ', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
                foreach (char i in txtBPantallaAnterior.Text)
                {
                    if (!noOperadores.Contains(i))
                    {
                        oper = i;
                        break;
                    }
                }
                txtBPantallaAnterior.Text = getValueNum1() + " " + oper + " " + getValueNum2() + " " + car;
                txtBPantallaActual.Text = getResultado().ToString();
                setValueNum1(getResultado());
                //setValueNum2(0);
                setResultado(double.NaN);
            }
        }

        private void ValidateOperatorInPreviousScreen(char car)
        {
            if (txtBPantallaAnterior.Text.Contains('='))
            {
                setValueNum2(ConvertToNumber(txtBPantallaActual.Text));
                txtBPantallaAnterior.Text = getValueNum1() + " " + car;
                setValueNum2(0);
                txtBPantallaActual.Text = getValueNum2().ToString();
                return;
            }

            if (txtBPantallaAnterior.Text == "") //<-- Si no hay ningun operador en la pantalla previa
            {
                setValueNum1(ConvertToNumber(txtBPantallaActual.Text));
                AddToPreviousScreen(car);
                txtBPantallaActual.Text = getValueNum2().ToString();
            }
            else //<-- Si hay un operador realiza la operacion, para proceder con la otra operacion despúes
            {
                setValueNum2(ConvertToNumber(txtBPantallaActual.Text));
                realizarOperacion();
                setValueNum2(0);
                setValueNum1(getResultado());
                txtBPantallaActual.Text = getValueNum2().ToString();//getResultado().ToString();
                txtBPantallaAnterior.Text = getValueNum1() + " " + car;
                setResultado(double.NaN);
            }
        }

        private void AddToActualScreen(char car)
        {

            if (car == '.' && txtBPantallaActual.Text.Contains(".")) //<-- Si ya hay algun punto, se sale
                return;

            if (car != '.' && txtBPantallaAnterior.Text.Contains('='))//<- Si se acaba de resolver una operacion y quiero hacer otra
            {
                txtBPantallaActual.Text = "" + car;
                txtBPantallaAnterior.Text = "";
                return;
            }

            if (car != '.' && txtBPantallaActual.Text == "0") //<-- Si el numero por defecto es 0 (cuando arranca el programa) o cuando se resuelve una operacion
            {
                if (double.IsNaN(resultado))
                    txtBPantallaActual.Text = "";

                txtBPantallaActual.Text += car;
                return;
            }

            if (double.IsNaN(resultado))
                txtBPantallaActual.Text += car;

        }

        private void AddToPreviousScreen(char car)
        {
            if (car == '=') //<-- caso de que se tenga que actualizar con el operador más el signo igual <-- falta
            {
                ValidateEqualSignInPreviousScreen(car);
            }
            else //<-- caso de que se tenga que actualizar con el primer operador
            {
                txtBPantallaAnterior.Text = getValueNum1() + " " + car;
                txtBPantallaActual.Text = "";
            }
        }

        private void ClickToOperator(char operadorEntrada)
        {
            ValidateOperatorInPreviousScreen(operadorEntrada); //<-- Valida si hay algun operador en la pantalla previa, resolverlo, para despues actualizar al nuevo operador
            switch (operadorEntrada)
            {
                case '%': operadorActual = new Porcentaje(); break;
                case 'X': operadorActual = new Multiplicacion(); break;
                case '-': operadorActual = new Resta(); break;
                case '+': operadorActual = new Suma(); break;
                case '/': operadorActual = new Division(); break;
            }
        }

        private void realizarOperacion()
        {
            if (operadorActual != null)
            {
                Operacion<IOperador> operacion = new Operacion<IOperador>(num1, num2, operadorActual);
                setResultado(operacion.operar());
            }
        }

        //Metodos de Acceso get y set a mis propiedades
        private double ConvertToNumber(string str)
        {
            return Convert.ToDouble(str);
        }

        private void setValueNum1(double num)
        {
            this.num1 = num;
        }

        private void setValueNum2(double num)
        {
            this.num2 = num;
        }

        private double getValueNum1()
        {
            return this.num1;
        }

        private double getValueNum2()
        {
            return this.num2;
        }

        private void setResultado(double res)
        {
            this.resultado = res;
        }
        private double getResultado()
        {
            return resultado;
        }

        //Atributos  de la clase
        IOperador? operadorActual;
        private double num1;
        private double num2;
        private double resultado = double.NaN;
    }
}
