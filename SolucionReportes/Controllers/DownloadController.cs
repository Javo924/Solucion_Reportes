using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DinkToPdf;
using SolucionReportes.Models;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SolucionReportes.Controllers
{
    public class DownloadController : Controller
    {
        private readonly IConverter _converter;

        public DownloadController(IConverter converter)
        {
            _converter = converter;
        }

        
        public IActionResult Descarga()
        {
            DescargaViewModel dc = new DescargaViewModel();
            return View();
        }

        [HttpPost]
        public IActionResult Descarga(DescargaViewModel dc)
        { 
            string pagina_actual = HttpContext.Request.Path;
            string url_pagina = HttpContext.Request.GetEncodedUrl();
            url_pagina = url_pagina.Replace(pagina_actual, "");
            url_pagina = $"{url_pagina}/Download/VistaParaPDF";

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings()
                {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait
                },
                Objects = {
                    new ObjectSettings(){
                        Page = url_pagina,
                        HtmlContent = dc.contenido,
                        WebSettings = { DefaultEncoding = "utf-8" }
                    }
                }

            };

            var archivoPDF = _converter.Convert(pdf);
            string nombrePDF = "Reporte_" + dc.nombre + ".pdf";//DateTime.Now.ToString("ddMMyyyyHHmmss") + ".pdf";

            return File(archivoPDF, "application/pdf", nombrePDF);
        }

        public IActionResult VistaParaPDF()
        {
            return View();
        }

        public IActionResult MostrarPDFenPagina()
        {
            string pagina_actual = HttpContext.Request.Path; // LocalHost -> Controlador -> Vista
            string url_pagina = HttpContext.Request.GetEncodedUrl(); // Ruta del proyecto
            url_pagina = url_pagina.Replace(pagina_actual, ""); // Limpia la url de la pagina
            url_pagina = $"{url_pagina}/Download/VistaParaPDF"; // La sustituye por la ruta del proyecto


            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings()
                {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait
                },
                Objects = {
                    new ObjectSettings(){
                        Page = url_pagina
                    }
                }

            };

            var archivoPDF = _converter.Convert(pdf);


            return File(archivoPDF, "application/pdf");
        }

        public IActionResult DescargarPDF()
        {
            string pagina_actual = HttpContext.Request.Path;
            string url_pagina = HttpContext.Request.GetEncodedUrl();
            url_pagina = url_pagina.Replace(pagina_actual, "");
            url_pagina = $"{url_pagina}/Download/VistaParaPDF";


            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings()
                {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait
                },
                Objects = {
                    new ObjectSettings(){
                        Page = url_pagina
                    }
                }

            };

            var archivoPDF = _converter.Convert(pdf);
            string nombrePDF = "reporte_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".pdf";

            return File(archivoPDF, "application/pdf", nombrePDF);
        }
    }
}

