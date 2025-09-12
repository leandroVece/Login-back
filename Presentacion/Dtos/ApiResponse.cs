namespace Presentation.Dtos;

public class ApiResponse<T>
{
    public string Status { get; set; }           // "success", "error", "warning", etc.
    public string Message { get; set; }          // Mensaje para el usuario o desarrollador
    public T Data { get; set; }                  // Contenido específico (puede ser null)
    public DateTime Timestamp { get; set; }      // Fecha y hora de la respuesta
    public string RequestId { get; set; }        // ID único por solicitud (opcional)
    public string ErrorCode { get; set; }        // Código técnico del error (si aplica)
    // public List<ValidationError> ValidationErrors { get; set; } // Errores por campo
    public MetaData Meta { get; set; }           // Información adicional (paginación, duración, etc.)

    public ApiResponse()
    {
        Timestamp = DateTime.UtcNow;
    }
}

public class MetaData
{
    public int? Page { get; set; }
    public int? PageSize { get; set; }
    public int? TotalPages { get; set; }
    public int? TotalItems { get; set; }

}
public class ValidationError
{
    public string Field { get; set; }            // Nombre del campo con error
    public string Message { get; set; }          // Descripción del error
}