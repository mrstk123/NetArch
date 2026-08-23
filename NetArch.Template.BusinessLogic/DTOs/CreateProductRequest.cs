using System.ComponentModel.DataAnnotations;

namespace NetArch.Template.BusinessLogic.DTOs;

public record CreateProductRequest(
    [property: Required]
    [property: MaxLength(200)]
    string Name);
