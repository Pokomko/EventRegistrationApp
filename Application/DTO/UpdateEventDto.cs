using Microsoft.AspNetCore.Http;

namespace Application.DTO;

public class UpdateEventDto : EventBaseDto
{
    public Guid Id { get; set; }
}
