using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attributes;

/// <summary>
/// Attribute for tests
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class Test : Attribute
{
    /// <summary>
    /// Expected exception
    /// </summary>
    public Type? Expected { get; set; }

    /// <summary>
    /// Ignoring test
    /// </summary>
    public string? Ignore { get; set; }
}