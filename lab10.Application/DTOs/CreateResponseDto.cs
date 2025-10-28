using System.ComponentModel.DataAnnotations;

namespace lab10.Application.DTOs;

public class CreateResponseDto
{
    [Required(ErrorMessage = "El mensaje es requerido")]
    public string Message { get; set; } = string.Empty;
}
