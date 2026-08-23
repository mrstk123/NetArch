using System.ComponentModel.DataAnnotations;

namespace NetArch.Template.Application.DTOs;

public record CreateProductRequest(
    [property: Required]
    [property: MaxLength(200)]
    string Name);
