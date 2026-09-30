using System.Diagnostics;
using System.IO;

// Este proyecto lanza el dev server de la app React situada en la misma carpeta del proyecto
var frontendFolder = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));

Console.WriteLine($"Frontend folder: {frontendFolder}");

if (!Directory.Exists(frontendFolder))
{
    Console.Error.WriteLine($"Carpeta Frontend no encontrada: {frontendFolder}");
    return 1;
}

var psi = new ProcessStartInfo
{
    FileName = "cmd.exe",
    Arguments = "/c npm run dev",
    WorkingDirectory = frontendFolder,
    UseShellExecute = false,
    RedirectStandardOutput = true,
    RedirectStandardError = true,
    CreateNoWindow = false,
};

var p = Process.Start(psi) ?? throw new InvalidOperationException("No se pudo iniciar proceso npm");

p.OutputDataReceived += (s, e) => { if (e.Data != null) Console.WriteLine(e.Data); };
p.ErrorDataReceived += (s, e) => { if (e.Data != null) Console.Error.WriteLine(e.Data); };

p.BeginOutputReadLine();
p.BeginErrorReadLine();

// No bloqueamos: dejamos el proceso npm en ejecución y devolvemos 0.
await Task.Delay(-1);
return 0;
