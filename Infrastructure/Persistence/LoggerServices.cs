
using Application.Interface;
using Serilog;

namespace Infrastructure.Persistence;

public class SerilogLoggerServices : ILoggerServices
{
    public void LogInformacion(string mensaje)
    {
        Log.Information(mensaje);
    }

    public void LogAdvertencia(string mensaje)
    {
        Log.Warning(mensaje);
    }

    public void LogError(string mensaje, Exception ex = null)
    {
        Log.Error(ex, mensaje);
    }
}