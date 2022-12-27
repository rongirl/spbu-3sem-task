using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class Test : Attribute
{

    public Type? Expected { get; set; }

    public string? Ignore { get; set; }
}
