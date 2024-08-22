using Domain;
using Domain.Contracts.Reports;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.IO;
using System.Threading;
using Utils;

namespace Reports
{
    public class CaratulaReporte : ICaratulaReport
    {
        private Caratula _caratula;

        private PdfDocument _doc;
        private XGraphics _gfx;
        private const int _marginX = 20;
        private const int _marginY = 20;
        private readonly double _pageX;
        private readonly double _pageY;
        private readonly double _areaY;
        private string _hash;

        public CaratulaReporte()
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("es-AR");
            Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("es-AR");
            _doc = new PdfDocument();

            var securitySettings = _doc.SecuritySettings;

            securitySettings.PermitAccessibilityExtractContent = false;
            securitySettings.PermitAnnotations = false;
            securitySettings.PermitAssembleDocument = false;
            securitySettings.PermitExtractContent = false;
            securitySettings.PermitFormsFill = false;
            securitySettings.PermitModifyDocument = false;

            var page = _doc.AddPage();

            page.Size = PageSize.A4;
            page.Orientation = PageOrientation.Landscape;
            _pageX = page.Width.Point;
            _pageY = page.Height.Point;
            _areaY = _pageY - (_marginY * 2);
            _gfx = XGraphics.FromPdfPage(page);
        }

        public Stream GetReporteCaratula(Caratula caratula)
        {
            _caratula = caratula;
            _hash = HashCode.CodigoUnicoCaratula(_caratula.Operacion.NroOperacion);

            CrearReporte();

            var ms = new MemoryStream();
            _doc.Save(ms, false);

            return ms;
        }

        #region MetodosCreacion

        private void CrearReporte()
        {
            CrearLateralIzquierdo();
            CrearCuerpoCentral();
            CrearLateralDerecho();
        }

        private void CrearLateralIzquierdo()
        {
            _gfx.DrawRectangle(XPens.Black, _marginX, _marginY, 150, _areaY);

            var font = new XFont("Verdana", 9, XFontStyle.Bold);

            CrearCodigoVertical();

            _gfx.DrawString("Formulario:", font, XBrushes.Black, _marginX + 10, _marginY + 515);

            font = new XFont("Verdana", 9);
            _gfx.DrawString(_caratula.NroFormulario.ToString("0,0"), font, XBrushes.Black, _marginX + 75, _marginY + 515);
            _gfx.DrawString(_caratula.Operacion.Fecha.ToString("dd-MMM-yyyy HH:mm"), font, XBrushes.Black, _marginX + 10, _marginY + 530);
            _gfx.DrawString(_caratula.Operacion.UsuarioID.ToString(), font, XBrushes.Black, _marginX + 10, _marginY + 546);
        }

        private void CrearCuerpoCentral()
        {
            const int cuerpoX = 450;

            long nroCorrelativo = 0;
            string nombre = "", descripcion = "";

            if (_caratula.Sociedad != null)
            {
                CrearMarcaDeAgua();

                nroCorrelativo = _caratula.Sociedad.NroCorrelativo;
                nombre = _caratula.Sociedad.Nombre;
                descripcion = _caratula.Sociedad.TipoSociedad.Descripcion;
            }

            //Cuerpo
            _gfx.DrawRectangle(XPens.Black, _marginX + 180, _marginY, cuerpoX, _areaY);

            //Cabeza
            _gfx.DrawRectangle(XBrushes.Black, _marginX + 200, _marginY + 20, 85, 40);
            var font = new XFont("Verdana", 30, XFontStyle.Bold);
            _gfx.DrawString(DateTime.Now.ToString("yyyy"), font, XBrushes.White, new XPoint(_marginX + 200, _marginY + 50));

            font = new XFont("Verdana", 18, XFontStyle.Bold);
            _gfx.DrawString(DateTime.Now.ToString("dd/MM"), font, XBrushes.Black, new XPoint(_marginX + 210, _marginY + 80));

            var center = _pageX / 2;

            var image = XImage.FromGdiPlusImage(global::Reports.Resource.LogoMinisterio);
            _gfx.DrawImage(image, center - 70, _marginY + 20, 170, 100);

            image = XImage.FromGdiPlusImage(BarCodeImage.GenerateH(_caratula.CodigoUnico));
            image.Interpolate = true;
            _gfx.DrawImage(image, center + 70, _marginY + 20, 150, 30);
            font = new XFont("Verdana", 9, XFontStyle.Bold);
            _gfx.DrawString(_caratula.CodigoUnico, font, XBrushes.Black, center + 110, _marginY + 60);

            //Titulo
            _gfx.DrawRectangle(XBrushes.LightGray, _marginX + 181, _marginY + 160, cuerpoX - 2, 40);

            font = new XFont("Verdana", 18, XFontStyle.Bold);
            _gfx.DrawString(_caratula.TituloCaratula, font, XBrushes.Black, _marginX + 200, _marginY + 180);

            font = new XFont("Verdana", 8, XFontStyle.Bold);
            _gfx.DrawString(_caratula.Operacion.UsuarioID.ToString(), font, XBrushes.Black, _marginX + 600, _marginY + 190);

            font = new XFont("Verdana", 10, XFontStyle.Bold);
            _gfx.DrawString("Código de Trámite:", font, XBrushes.Black, _marginX + 200, _marginY + 300);
            _gfx.DrawString(_caratula.CodTramite.ToString(), font, XBrushes.Black, _marginX + 320, _marginY + 300);

            font = new XFont("Verdana", 13);

            font = new XFont("Verdana", 10, XFontStyle.Bold);
            _gfx.DrawString("Nro. Correlativo Entidad:", font, XBrushes.Black, _marginX + 200, _marginY + 250);
            _gfx.DrawString(nroCorrelativo.ToString(), font, XBrushes.Black, _marginX + 360, _marginY + 250);

            font = new XFont("Verdana", 10, XFontStyle.Bold);
            _gfx.DrawString("Tipo Societario:", font, XBrushes.Black, _marginX + 200, _marginY + 350);
            font = new XFont("Verdana", 13);
            _gfx.DrawString(descripcion, font, XBrushes.Black, _marginX + 300, _marginY + 350);

            font = new XFont("Verdana", 10, XFontStyle.Bold);
            _gfx.DrawString("Denominación:", font, XBrushes.Black, _marginX + 200, _marginY + 400);
            font = new XFont("Verdana", 13);

            //Se verifica que la razon social no sea superior a 37 caracteres
            //caso contrario de debe separar en 2 partes
            if (nombre.Length > 37)
            {
                var aux = nombre.Substring(0, 37);
                var index = aux.LastIndexOf(" ");

                if (index == -1)
                    index = 29;

                aux = nombre.Substring(0, index);
                _gfx.DrawString(aux, font, XBrushes.Black, _marginX + 300, _marginY + 400);
                _gfx.DrawString(nombre.Substring(index + 1), font, XBrushes.Black, _marginX + 300, _marginY + 415);
            }
            else
                _gfx.DrawString(nombre, font, XBrushes.Black, _marginX + 300, _marginY + 400);

            font = new XFont("Verdana", 9, XFontStyle.Bold);

            image = XImage.FromGdiPlusImage(BarCodeImage.GenerateH(_caratula.NroTramite.ToString()));
            _gfx.DrawImage(image, _marginX + 200, _marginY + 500, 150, 30);
            _gfx.DrawString(_caratula.NroTramite.ToString("0,0"), font, XBrushes.Black, _marginX + 240, _marginY + 540);

            if (_caratula.Urgente)
            {
                image = XImage.FromGdiPlusImage(global::Reports.Properties.Resources.Urgente);
                _gfx.DrawImage(image, _marginX + 420, _marginY + 430, 200, 50);
            }

            image = XImage.FromGdiPlusImage(BarCodeImage.GenerateH(_hash));
            _gfx.DrawImage(image, _marginX + 470, _marginY + 500, 150, 30);
            _gfx.DrawString(_hash, font, XBrushes.Black, _marginX + 520, _marginY + 540);
        }

        private void CrearLateralDerecho()
        {
            //Lateral derecho

            var x = _pageX - (_marginX + 138);
            _gfx.DrawRectangle(XPens.Black, x, _marginY, 150, _areaY);

            var image = XImage.FromGdiPlusImage(global::Reports.Resource.LogoMinisterio);
            _gfx.DrawImage(image, x + 10, _marginY + 10, 130, 70);

            _gfx.DrawLine(new XPen(XColors.Black, 1), x, _marginY + 100, x + 150, _marginY + 100);

            LeyendaVertical(x);

            _gfx.DrawLine(new XPen(XColors.Black, 1), x, _marginY + 330, x + 150, _marginY + 330);

            var font = new XFont("Verdana", 9, XFontStyle.Bold);

            _gfx.DrawString("Tramite iniciado en IGJ:", font, XBrushes.Black, x + 10, _marginY + 350);

            image = XImage.FromGdiPlusImage(BarCodeImage.GenerateH(_caratula.NroTramite.ToString()));
            _gfx.DrawImage(image, x + 10, _marginY + 360, 120, 33);
            _gfx.DrawString(_caratula.NroTramite.ToString("0,0"), font, XBrushes.Black, x + 43, _marginY + 403);

            image = XImage.FromGdiPlusImage(BarCodeImage.GenerateH(_hash));
            _gfx.DrawImage(image, x + 10, _marginY + 410, 120, 33);
            _gfx.DrawString(_hash, font, XBrushes.Black, x + 50, _marginY + 453);

            image = XImage.FromGdiPlusImage(BarCodeImage.GenerateH(_caratula.NroPresentacion.ToString("0000")));
            _gfx.DrawImage(image, x + 10, _marginY + 460, 120, 30);
            _gfx.DrawString(_caratula.NroPresentacion.ToString("0000"), font, XBrushes.Black,
                x + 55, _marginY + 500);

            _gfx.DrawString("Formulario:", font, XBrushes.Black, x + 10, _marginY + 515);
            _gfx.DrawString(_caratula.NroFormulario.ToString("0,0"), font, XBrushes.Black, x + 75, _marginY + 515);

            font = new XFont("Verdana", 9);
            _gfx.DrawString(_caratula.Operacion.Fecha.ToString("dd-MMM-yyyy HH:mm"), font, XBrushes.Black, x + 10, _marginY + 530);
            _gfx.DrawString(_caratula.Operacion.UsuarioID.ToString(), font, XBrushes.Black, x + 10, _marginY + 546);
        }

        private void CrearCodigoVertical()
        {
            var font = new XFont("Verdana", 9, XFontStyle.Bold);

            var image = XImage.FromGdiPlusImage(BarCodeImage.GenerateV(_caratula.NroTramite.ToString()));
            _gfx.DrawImage(image, _marginX + 45, _marginY + 20, 35, 130);

            image = XImage.FromGdiPlusImage(BarCodeImage.GenerateV(_hash));
            _gfx.DrawImage(image, _marginX + 45, _marginY + 170, 35, 130);

            image = XImage.FromGdiPlusImage(BarCodeImage.GenerateV(_caratula.NroPresentacion.ToString("0000")));
            _gfx.DrawImage(image, _marginX + 45, _marginY + 320, 35, 130);

            var gs = _gfx.Save();
            _gfx.TranslateTransform(_marginX + 90, 130);
            _gfx.RotateTransform(-90);
            _gfx.DrawString(_caratula.NroTramite.ToString("0,0"), font, XBrushes.Black, 1, 1);
            _gfx.DrawString(_hash, font, XBrushes.Black, -150, 1);
            _gfx.DrawString(_caratula.NroPresentacion.ToString("0000"), font, XBrushes.Black, -290, 1);
            _gfx.Restore(gs);
        }

        //Codigo fuente obtenido del foro oficial de PdfSharp con algunas modificaciones
        private void CrearMarcaDeAgua()
        {
            var mgrX = _marginX + 450;
            var mgrY = _marginY + 480;

            var tipo = _caratula.Sociedad.TipoSociedad.Alias;

            var em = 120;
            if (tipo.Length > 6)
                em = 70;

            var font = new XFont("Arial", em, XFontStyle.Bold);
            var gs = _gfx.Save();

            // Get the size (in point) of the text
            var size = _gfx.MeasureString(tipo, font);

            // Define a rotation transformation at the center of the page
            _gfx.TranslateTransform(mgrX / 2, mgrY / 2);
            _gfx.RotateTransform(-Math.Atan(mgrY / mgrX) * 180 / Math.PI);
            _gfx.TranslateTransform(-mgrX / 2, -mgrY / 2);

            // Create a graphical path
            var path = new XGraphicsPath();

            // Add the text to the path
            path.AddString(tipo, font.FontFamily, XFontStyle.BoldItalic, em,
              new XPoint((mgrX + 130 - size.Width) / 2, ((mgrY + 450) - size.Height) / 2),
              XStringFormats.Default);

            // Create a dimmed red pen and brush
            var pen = new XPen(XColor.FromArgb(50, 75, 0, 130), 3);
            var brush = new XSolidBrush(XColors.LightGray);

            // Stroke the outline of the path
            _gfx.DrawPath(brush, path);
            _gfx.Restore(gs);
        }

        private void LeyendaVertical(double center)
        {
            var font = new XFont("MonotypeCorsiva", 15, XFontStyle.Italic);
            var gs = _gfx.Save();
            _gfx.TranslateTransform(center + 50, _marginY + 320);
            _gfx.RotateTransform(-90);
            _gfx.DrawString("Conserve este comprobante de", font, XBrushes.Black, 1, 1);
            _gfx.DrawString("recepción para posteriores", font, XBrushes.Black, 8, 25);
            _gfx.DrawString("consultas sobre el trámite", font, XBrushes.Black, 9, 50);
            _gfx.Restore(gs);
        }

        #endregion MetodosCreacion
    }
}