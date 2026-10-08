// Helper para extraer el response de excepción devuelto por .NET (ProblemDetails)
export function getErrorMessage(
    error,
    fallbackMessage = "An unexpected error occurred."
) {
    const problemDetails = error.response?.data;

    // ProblemDetails estándar
    if (problemDetails?.detail) {
        return problemDetails.detail;
    }

    // ValidationProblemDetails
    if (problemDetails?.errors) {
        return Object.values(problemDetails.errors)
            .flat()
            .join(" ");
    }

    // Error de Axios/JavaScript
    if (error.message) {
        return error.message;
    }

    return fallbackMessage;
}