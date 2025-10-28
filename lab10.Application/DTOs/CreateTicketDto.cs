using System.ComponentModel.DataAnnotations;

namespace lab10.Application.DTOs;

public class CreateTicketDto
{
    [Required(ErrorMessage = "El título es requerido")]
    [StringLength(255, ErrorMessage = "El título no puede exceder 255 caracteres")]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "La descripción no puede exceder 1000 caracteres")]
    public string? Description { get; set; }
}
