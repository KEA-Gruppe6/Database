using Database_project.Core.MongoDB.DTO;
using Database_project.Core.MongoDB.Entities;
using Database_project.Core.MongoDB.Services;
using Microsoft.AspNetCore.Mvc;


namespace Database_project.Core.MongoDB.Controllers;

[ApiController]
[Route("MongoDB/[controller]")]
public class OrderController : ControllerBase
{
    //not sure if this one is needed, mabey just to see all orders.
    //but orders are created with ticket,
    //with each new ticket a new order is created and any tickets after that added to that order.
}