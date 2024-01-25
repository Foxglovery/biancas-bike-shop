// This code snippet defines a controller class for managing bikes in an API. It includes methods for retrieving all bikes, retrieving a bike by its ID, and retrieving the count of bikes in inventory that have pending work orders. These methods require authorization.
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BiancasBikes.Data;
using Microsoft.EntityFrameworkCore;
using BiancasBikes.Models;
using BiancasBikes.Models.DTOs;

namespace BiancasBikes.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BikeController : ControllerBase
{
    private BiancasBikesDbContext _dbContext;

    public BikeController(BiancasBikesDbContext context)
    {
        _dbContext = context;
    }

    [HttpGet]
    [Authorize]
    public IActionResult Get()
    {
        return Ok(_dbContext
            .Bikes
            .Include(b => b.Owner)
            .Select(b => new BikeDTO
            {
                Id = b.Id,
                Brand = b.Brand,
                Color = b.Color,
                BikeTypeId = b.BikeTypeId,
                OwnerId = b.OwnerId,
                Owner = new OwnerDTO
                {
                    Id = b.Owner.Id,
                    Name = b.Owner.Name,
                    Address = b.Owner.Address,
                    Email = b.Owner.Email,
                    Telephone = b.Owner.Telephone
                }
            })
            .ToList());
    }

    [HttpGet("{id}")]
    [Authorize]
    public IActionResult GetById(int id)
    {
        Bike bike = _dbContext
        .Bikes
        .Include(b => b.Owner)
        .Include(b => b.BikeType)
        .Include(b => b.WorkOrders)
        .SingleOrDefault(b => b.Id == id);

        if (bike == null)
        {
            return NotFound();
        }

        return Ok(bike);
    }

    [HttpGet("inventory")]
    [Authorize]
    public IActionResult Inventory()
    {
        int inventory = _dbContext
        .Bikes
        .Where(b => b.WorkOrders.Any(wo => wo.DateCompleted == null))
        .Count();

        return Ok(inventory);
    }


}