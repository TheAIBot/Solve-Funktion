using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquationCreator
{
    public enum MathOperators
    {
        Plus, //Connector
        Subtract, //Connector
        Multiply, //Connector
        Divide, //Connector

        PowerOf, //Connector
        Root,
        Exponent,
        NaturalLog,
        Log,

        Modulos,
        Floor,
        Ceil,
        Round,

        Sin,
        Cos,
        Tan,
        ASin,
        ACos,
        ATan,

        Parentheses,
        Constant,
        Absolute,

        AND, //Connector
        NAND, //Connector
        OR, //Connector
        NOR, //Connector
        XOR, //Connector
        XNOR, //Connector
        NOT
    }
}
