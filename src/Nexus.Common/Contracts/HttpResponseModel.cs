namespace Nexus.Common.Contracts;

/// <summary>
/// Represents a generic HTTP response model containing data, error information, and error messages.
/// </summary>
/// <typeparam name="T">The type of the data in the response.</typeparam>
public class HttpResponseModel<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HttpResponseModel{T}"/> class.
    /// </summary>
    /// <param name="data">The data contained in the response.</param>
    /// <param name="isError">A flag indicating whether the response represents an error.</param>
    /// <param name="errorMessage">The error message associated with the response, if applicable.</param>
    private HttpResponseModel(T data, bool isError, string errorMessage)
    {
        Data = data;
        IsError = isError;
        ErrorMessage = errorMessage;
    }

    /// <summary>
    /// Gets the data contained in the response.
    /// </summary>
    public T Data { get; }

    /// <summary>
    /// Gets a flag indicating whether the response represents an error.
    /// </summary>
    public bool IsError { get; }

    /// <summary>
    /// Gets the error message associated with the response, if applicable.
    /// </summary>
    public string ErrorMessage { get; }

    /// <summary>
    /// Creates a new <see cref="HttpResponseModel{T}"/> instance representing a successful response.
    /// </summary>
    /// <param name="data">The data to be included in the response.</param>
    /// <returns>A new instance of <see cref="HttpResponseModel{T}"/> representing a successful response.</returns>
    public static HttpResponseModel<T> Success(T data)
    {
        return new HttpResponseModel<T>(data, false, string.Empty);
    }

    /// <summary>
    /// Creates a new <see cref="HttpResponseModel{T}"/> instance representing a failed response.
    /// </summary>
    /// <param name="data">The data to be included in the response.</param>
    /// <param name="errorMessage">The error message associated with the response.</param>
    /// <returns>A new instance of <see cref="HttpResponseModel{T}"/> representing a failed response.</returns>
    public static HttpResponseModel<T> Failure(T data, string errorMessage)
    {
        return new HttpResponseModel<T>(data, true, errorMessage);
    }
}
