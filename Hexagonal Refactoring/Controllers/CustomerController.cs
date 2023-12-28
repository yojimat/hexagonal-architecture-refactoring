using Hexagonal_Refactoring.Models;
using Microsoft.AspNetCore.Mvc;
using Hexagonal_Refactoring.DTOs;
using Hexagonal_Refactoring.Services;

namespace Hexagonal_Refactoring.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomerController(ICustomerService customerService) : ControllerBase
{
    [HttpPost]
    public IActionResult Create([FromBody] CustomerDto dto)
    {
        if (customerService.FindByCpf(dto.GetCpf()) != null)
        {
            return UnprocessableEntity("Customer already exists");
        }

        if (customerService.FindByEmail(dto.GetEmail()) != null)
        {
            return UnprocessableEntity("Customer already exists");
        }

        var customer = new Customer();
        customer.SetName(dto.GetName());
        customer.SetCpf(dto.GetCpf());
        customer.SetEmail(dto.GetEmail());
        customer = customerService.Save(customer);

        return Created($"/customers/{customer.GetId()}", new CustomerDto(customer));
    }

    [HttpGet("{id:long}")]
    public IActionResult GetCustomer(long id)
    {
        var customer = customerService!.FindById(id);
        return customer == null ? NotFound() : Ok(customer);
    }
}
