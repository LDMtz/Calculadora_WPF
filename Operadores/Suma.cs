using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculadora.Operadores
{
    internal class Suma : IOperador
    {
        public double realizarOperacion(double num1, double num2)
        {
            return num1 + num2;
        }

    }
}
