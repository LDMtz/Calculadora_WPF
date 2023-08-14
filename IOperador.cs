using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculadora
{
    public interface IOperador
    {
        double realizarOperacion(double num1, double num2);
    }
}
