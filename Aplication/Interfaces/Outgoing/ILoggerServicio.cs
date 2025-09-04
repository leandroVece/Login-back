

public interface ILoggerServices
{
    void LogInformacion(string mensaje);
    void LogAdvertencia(string mensaje);
    void LogError(string mensaje, Exception ex = null);
}