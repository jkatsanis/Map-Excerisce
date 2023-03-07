using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary;

public enum Usage
{
    often = 0,
    sometimes = 1,
    rarely = 2,
    never = 3,
}

public record WordData(string Word, Usage Usage);