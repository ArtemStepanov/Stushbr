using System.Text.Json.Serialization;

namespace Stushbr.Application.ExceptionHandling
{
    /// <summary>
    /// Use this generic response in backend to report about single or multiple errors.
    /// </summary>
    public class ErrorsResponse
    {
        public class Error
        {
            /// <summary>
            /// Property that caused an error. Used for model validation.
            /// </summary>
            [JsonPropertyName("prop")]
            public string? Property { get; set; }

            /// <summary>
            /// Human readable error.
            /// </summary>
            [JsonPropertyName("msg")]
            public string? Message { get; set; }

            public Error()
            {
            }

            public Error(string? message)
            {
                Message = message;
            }

            public Error(string message, string property) : this(message)
            {
                Property = property;
            }

            public override string ToString()
            {
                return $"{(!string.IsNullOrEmpty(Property) ? (" [" + Property + "]") : string.Empty)} {Message}";
            }
        }

        [JsonPropertyName("errors")]
        public List<Error> Errors { get; } = new();

        [JsonConstructor]
        internal ErrorsResponse()
        {
        }

        public ErrorsResponse(string? errorMessage) : this()
        {
            Errors.Add(new Error(errorMessage ?? "Internal Server Error"));
        }

        public ErrorsResponse(Exception? exception) : this(exception?.Message)
        {
        }

        public ErrorsResponse(IEnumerable<Error>? errors) : this()
        {
            Errors.AddRange(errors ?? Enumerable.Empty<Error>());
        }

        public override string ToString()
        {
            return $"ErrorsResponse. Errors[{Errors.Count}]:\n" + string.Join('\n', Errors);
        }
    }
}
