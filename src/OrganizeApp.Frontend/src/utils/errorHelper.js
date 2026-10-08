// export function getErrorMessage(error, fallbackMessage = "An unexpected error occurred.") {
//     return error.response?.data?.detail
//         ?? error.message
//         ?? fallbackMessage;
// }

// Helper para extraer el mensaje de excepción devuelto por .NET (ProblemDetails)
export const getErrorMessage = (error) => {
    if (error.response && error.response.data) {
        const data = error.response.data;

        // 1. Mensaje de detalle de la excepción de dominio (ProductDomainException)
        if (data.detail) return data.detail;

        // 2. Título genérico si no hay detalle
        if (data.title) return data.title;

        // 3. Si hay errores de validación (ValidationException)
        if (data.errors && typeof data.errors === 'object') {
            const firstErrorKey = Object.keys(data.errors)[0];
            if (firstErrorKey && data.errors[firstErrorKey].length > 0) {
                return data.errors[firstErrorKey][0];
            }
        }

        if (typeof data === 'string') return data;
    }

    return error.message || "Ocurrió un error inesperado al procesar la solicitud.";
};



