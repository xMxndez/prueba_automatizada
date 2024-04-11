using System;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

class Tarea4
{
    static void Main() 
    {
        int tiempoEspera = 5000;
        int tiempoEspera1 = 2000;
        try
        {
            // Configuración del navegador Chrome
            var options = new ChromeOptions();
            var driver = new ChromeDriver(options);

            // Obtener la ruta completa para la carpeta "Evidencia"
            string evidenciaFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Evidencia");

            //Crear la carpeta si no existe
            if (!Directory.Exists(evidenciaFolder))
            {
                Directory.CreateDirectory(evidenciaFolder);
            }

            driver.Navigate().GoToUrl("https://clientes.eps.com.do/");

            // Captura de pantalla en la página de inicio
            CaptureScreenshot(driver, Path.Combine(evidenciaFolder, "1aPagina.png"));

            var userBoxb = driver.FindElement(By.Id("lUser"));
            userBoxb.SendKeys("G-14512");

            Thread.Sleep(tiempoEspera);
            var userPassb = driver.FindElement(By.Id("lPass"));
            userPassb.SendKeys("12812");

            // Captura de pantalla en la página de inicio de sesion con datos
            CaptureScreenshot(driver, Path.Combine(evidenciaFolder, "2Datos1.png"));

            Thread.Sleep(tiempoEspera);
            var btnLoginn = driver.FindElement(By.ClassName("btn"));
            ExecuteJavaScriptClick(driver, btnLoginn);


            // Captura de pantalla en la página de inicio de sesion erronea
            CaptureScreenshot(driver, Path.Combine(evidenciaFolder, "3aPaginam.png"));

            Thread.Sleep(tiempoEspera1);
            var error = driver.FindElement(By.ClassName("swal2-confirm"));
            ExecuteJavaScriptClick(driver, error);

            Thread.Sleep(tiempoEspera);
            var userBox = driver.FindElement(By.Id("lUser"));
            userBox.SendKeys("");

            Thread.Sleep(tiempoEspera);
            var userPass = driver.FindElement(By.Id("lPass"));
            userPass.SendKeys("1281");

            // Captura de pantalla en la página de inicio de sesion con datos
            CaptureScreenshot(driver, Path.Combine(evidenciaFolder, "4Datos.png"));

            Thread.Sleep(tiempoEspera);
            var btnLogin = driver.FindElement(By.ClassName("btn"));
            ExecuteJavaScriptClick(driver, btnLogin);

            // Captura de pantalla en la página de inicio luego de hacer el login
            CaptureScreenshot(driver, Path.Combine(evidenciaFolder, "5InicioSesion.png"));
            

            Thread.Sleep(tiempoEspera);
            var miCuenta = driver.FindElement(By.Id("lMicuenta"));
            ExecuteJavaScriptClick(driver, miCuenta);

            // Captura de pantalla en la configuracion de Mi Cuenta
            CaptureScreenshot(driver, Path.Combine(evidenciaFolder, "6MiCuenta.png"));


            Thread.Sleep(tiempoEspera);
            var newPass = driver.FindElement(By.Id("cpBody_lContrasena"));
            newPass.SendKeys("128123");

            // Captura de pantalla con nueva contraseña
            CaptureScreenshot(driver, Path.Combine(evidenciaFolder, "7NuevaContra.png"));

            Thread.Sleep(tiempoEspera);
            var save = driver.FindElement(By.Id("cpBody_Button1"));
            ExecuteJavaScriptClick(driver, save);
            Thread.Sleep(tiempoEspera1);

            // Captura de pantalla con nueva contraseña
            CaptureScreenshot(driver, Path.Combine(evidenciaFolder, "8zConfirmarNuevaContra.png"));
            Thread.Sleep(tiempoEspera);

            // Generar informe HTML
            GenerateHtmlReport(evidenciaFolder);

            // Obtener la ruta completa del informe HTML
            string reportFilePath = Path.Combine(evidenciaFolder, "Reporte.html");
            Console.WriteLine($"Informe HTML creado en: {reportFilePath}");

            // Abrir el informe HTML con el programa predeterminado (normalmente el navegador web)
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = reportFilePath,
                UseShellExecute = true
            });

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocurrió un error: {ex.Message}");
        }
    }

    // Función para hacer clic en un elemento mediante JavaScript
    static void ExecuteJavaScriptClick(IWebDriver driver, IWebElement element)
    {
        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
        js.ExecuteScript("arguments[0].click();", element);
    }

    // Función para capturar la pantalla y guardarla en un archivo
    static void CaptureScreenshot(IWebDriver driver, string filePath)
    {
        try
        {
            ITakesScreenshot screenshotDriver = (ITakesScreenshot)driver;
            Screenshot screenshot = screenshotDriver.GetScreenshot();
            screenshot.SaveAsFile(filePath, ScreenshotImageFormat.Png);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al capturar la pantalla: {ex.Message}");
        }
    }

    // Función para generar el informe HTML
    static void GenerateHtmlReport(string evidenciaFolder)
    {
        try
        {
            string reportFilePath = Path.Combine(evidenciaFolder, "Reporte.html");

            using (StreamWriter sw = new StreamWriter(reportFilePath))
            {
                sw.WriteLine("<html>");
                sw.WriteLine("<head>");
                sw.WriteLine("<title>Reportes</title>");
                sw.WriteLine("</head>");
                sw.WriteLine("<body>");

                sw.WriteLine("<br><center><h1>Informe de Evidencia:</h1></center><hr><br>");

                // Obtener la lista de archivos de imágenes en la carpeta
                string[] imageFiles = Directory.GetFiles(evidenciaFolder, "*.png");

                // Agregar cada imagen al informe HTML
                foreach (string imageFile in imageFiles)
                {
                    sw.WriteLine($"<center><img src=\"{Path.GetFileName(imageFile)}\" alt=\"Captura de pantalla\"><br></center>");
                }

                sw.WriteLine("</body>");
                sw.WriteLine("</html>");
            }
            Console.WriteLine($"Informe HTML creado en: {reportFilePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al generar el informe HTML: {ex.Message}");
        }
    }
}