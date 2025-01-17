using System.Collections.Generic;
using System.Linq;

namespace WpfMath.Colors
{
    /// <summary>A base class for color parsers that only require one component.</summary>
    internal abstract class SingleComponentColorParser : FixedComponentCountColorParser
    {
        protected SingleComponentColorParser() : base(1)
        {
        }

        protected abstract RgbaColor? ParseSingleComponent(string component);

        protected override RgbaColor? ParseComponents(List<string> components) =>
            ParseSingleComponent(components.Single());
    }
}
