using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Calculadora.MainWindow;

namespace Calculadora
{
    public class Operacion<T> where T : IOperador
    {
        private double num1;
        private double num2;
        private T operador;

        public Operacion(double num1, double num2, T operador)
        {
            this.num1 = num1;
            this.num2 = num2;
            this.operador = operador; ;
        }

        public double operar()
        {
            return operador.realizarOperacion(num1, num2);
        }
    }
}