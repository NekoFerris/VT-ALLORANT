using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using Svg;
using Svg.DataTypes;

//This only works on Windows
public class SvgHelper
{
    Font _defaultFont = new("Helvetica", 16);
    SvgDocument _svgDocument = new();
    public SvgHelper()
    {
        _svgDocument.Width = new SvgUnit(1000);
        _svgDocument.Height = new SvgUnit(1000);
        _svgDocument.ColorInterpolation = SvgColourInterpolation.SRGB;
        _svgDocument.Font = "Helvetica";
        _svgDocument.FontSize = 20;
        _svgDocument.ShapeRendering = SvgShapeRendering.GeometricPrecision;
        _svgDocument.Fill = new SvgColourServer(Color.White);
    }

    public void AddTable(DataTable table)
    {
        float padding = 5;
        float posX = 20;
        float posY;
        SvgText[,] textElements = new SvgText[table.Columns.Count, table.Rows.Count];
        float[] columnWidths = new float[table.Columns.Count];
        for (int i = 0; i < table.Columns.Count - 1; i++)
        {
            for (int j = 0; j < table.Rows.Count; j++)
            {
                SvgText textElement = new()
                {
                    Text = table.Select()[i][j].ToString(),
                    Font = "Helvetica",
                    FontSize = 20,
                    ShapeRendering = SvgShapeRendering.CrispEdges
                };
                textElements[i, j] = textElement;
                if (textElement.TextLength > columnWidths[i])
                {
                    columnWidths[i] = textElement.FontSize;
                }
            }
        }
        for (int i = 0; i < table.Columns.Count - 1; i++)
        {
            posY = 20;
            for (int j = 0; j < table.Rows.Count; j++)
            {
                SvgText textElement = textElements[i, j];
                textElement.Y =  [new SvgUnit(posX)];
                textElement.X =  [new SvgUnit(posY)];
                _svgDocument.Children.Add(textElement);
                posY += (textElement.FontSize + padding) * 10;
            }
            posX += (columnWidths[i] + padding) * 10;
        }
    }

    public MemoryStream ConvertSvgToJpeg()
    {
        MemoryStream jpegStream = new MemoryStream();
        Bitmap bitmap = _svgDocument.Draw();
        bitmap.Save(jpegStream, ImageFormat.Jpeg);
        File.Create("test.jpg").Write(jpegStream.ToArray());
        return jpegStream;
    }
}