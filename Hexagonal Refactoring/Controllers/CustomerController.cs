using System.ComponentModel.DataAnnotations;
using Hexagonal_Refactoring.Application.UseCases;
using Hexagonal_Refactoring.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Hexagonal_Refactoring.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomerController(
    CreateCustomerUseCase createCustomerUseCase,
    GetCustomerByIdUseCase getCustomerByIdUseCase) : ControllerBase
{
    [HttpPost]
    public IActionResult Create([FromBody] CustomerDto dto)
    {
        try
        {
            var output =
                createCustomerUseCase.Execute(new CreateCustomerUseCase.Input(dto.GetCpf(), dto.GetEmail(),
                    dto.GetName()));

            return Created($"/customers/{output.Id}", output);
        }
        catch (ValidationException e)
        {
            return UnprocessableEntity(e.Message);
        }
    }

    [HttpGet("{id:long}")]
    public IActionResult GetCustomer(long id)
    {
        var output = getCustomerByIdUseCase.Execute(new GetCustomerByIdUseCase.Input(id));
        return output == null ? NotFound() : Ok(output);
    }
}