using System.Diagnostics.CodeAnalysis;

// Suppress CA1052 only for the Program class specifically
[assembly: SuppressMessage("Design", "CA1052:Static holder types should be Static",
    Justification = "Program class cannot be static because it's used as a generic type parameter in WebApplicationFactory<Program> for integration testing",
    Target = "~T:CleanArchitecture.API.Program")]