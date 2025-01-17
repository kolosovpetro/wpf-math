using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using WpfMath.Data;
using WpfMath.Utils;

namespace WpfMath.Colors
{
    public class PredefinedColorParser : IColorParser
    {
        private const string ResourceName = TexUtilities.ResourcesDataDirectory + "PredefinedColors.xml";
        public static readonly PredefinedColorParser Instance = new PredefinedColorParser(ResourceName);

        private readonly IReadOnlyDictionary<string, RgbaColor> _colors;

        private PredefinedColorParser(string resourceName)
        {
            using (var resource = typeof(WpfMathResourceMarker).Assembly.ReadResource(resourceName))
            {
                var doc = XDocument.Load(resource);
                _colors = Parse(doc.Root);
            }
        }

        public RgbaColor? Parse(IEnumerable<string> components)
        {
            var componentList = components.ToList();
            var hasAlphaComponent = componentList.Count == 2;
            if (componentList.Count != 1 && !hasAlphaComponent)
                return null;

            var colorName = componentList[0];
            if (!_colors.TryGetValue(colorName, out var color))
                return null;

            byte? alpha = 255;
            if (hasAlphaComponent)
            {
                var alphaFraction = double.TryParse(
                    componentList[1],
                    NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture,
                    out var a)
                    ? (double?) a
                    : null;
                if (alphaFraction == null || alphaFraction < 0.0 || alphaFraction > 1.0)
                    return null;

                alpha = (byte) Math.Round(255.0 * a, MidpointRounding.AwayFromZero);
            }

            if (alpha == null)
                return null;

            color.A = alpha.Value;
            return color;
        }

        private Dictionary<string, RgbaColor> Parse(XElement rootElement)
        {
            var colors = new Dictionary<string, RgbaColor>();
            foreach (var colorElement in rootElement.Elements("color"))
            {
                var name = colorElement.AttributeValue("name");
                var r = colorElement.AttributeValue("r");
                var g = colorElement.AttributeValue("g");
                var b = colorElement.AttributeValue("b");
                colors.Add(name, RgbaColor.FromRgb(Convert.ToByte(r), Convert.ToByte(g), Convert.ToByte(b)));
            }

            return colors;
        }
    }
}
