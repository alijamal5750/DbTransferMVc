using System;
using System.Diagnostics;
using DbTransfer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DbTransfer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext1 _context1;
        private readonly ApplicationDbContext2 _context2;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext1 context, ApplicationDbContext2 context2)
        {
            _logger = logger;
            _context1 = context;
            _context2 = context2;
        }

        public async Task<IActionResult> Index()
        {
            await MakeTransaction();
            return View();
        }

        async Task MakeTransaction()
        {
           
            using var transaction = await _context2.Database.BeginTransactionAsync();
            try
            {
                // source 1
                var AllCars = await _context1.cars.ToListAsync();

                // Enable IDENTITY_INSERT for Cars table
                await _context2.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Cars ON");

                // Insert cars including Id values
                await _context2.Cars.AddRangeAsync(AllCars);
                await _context2.SaveChangesAsync();

                // Disable IDENTITY_INSERT
                await _context2.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Cars OFF");


                // Commit if everything is successful
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                // Rollback on error
                await transaction.RollbackAsync();
                Console.WriteLine($"Transaction failed: {ex.Message}");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
