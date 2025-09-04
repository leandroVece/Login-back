namespace Application.Helpers;

public static class EmailTemplateBuilder
{
    public static string GetConfirmationEmail(string link)
    {
        return $@"
            <html>
                <body>
                    <h2>Confirmación de Email</h2>
                    <p>Gracias por registrarte. Por favor, hacé clic en el siguiente botón para confirmar tu correo electrónico:</p>
                    <a href='{link}' style='
                        display:inline-block;
                        padding:10px 20px;
                        background-color:#28a745;
                        color:white;
                        text-decoration:none;
                        border-radius:5px;
                        font-weight:bold;'>Confirmar Email</a>
                </body>
            </html>";
    }
}
