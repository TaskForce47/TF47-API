using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TF47_API.Database;
using TF47_API.Database.Models.Services;
using TF47_API.Dto.Mappings;
using TF47_API.Dto.RequestModels;
using TF47_API.Dto.ResponseModels;
using TF47_API.Filters;

namespace TF47_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DonationController : ControllerBase
    {
        private readonly ILogger<DonationController> _logger;
        private readonly DatabaseContext _database;

        public DonationController(
            ILogger<DonationController> logger, 
            DatabaseContext database)
        {
            _logger = logger;
            _database = database;
        }

        [AllowAnonymous]
        [HttpGet("{donationId:int}")]
        [ProducesResponseType(typeof(DonationResponse), 200)]
        public async Task<IActionResult> GetDonation(long donationId)
        {
            var donation = await _database.Donations.FirstOrDefaultAsync(x => x.DonationId == donationId);
            if (donation == null) return BadRequest("Donation Id provided does not exist");

            return Ok(donation.ToDonationResponse());
        }

        [AllowAnonymous]
        [HttpGet("statistics/{year:int}/{month:int}")]
        [ProducesResponseType(typeof(DonationResponse[]), 200)]
        public async Task<IActionResult> GetDonationOfMonth(int year, int month)
        {
            var donations = await _database.Donations
                .AsNoTracking()
                .Where(x => x.TimeOfDonation.Year == year && x.TimeOfDonation.Month == month)
                .OrderBy(x => x.TimeOfDonation)
                .ToListAsync();

            return Ok(donations.ToDonationResponseIEnumerable());
        }

        [AllowAnonymous]
        [HttpGet("statistics/{year:int}")]
        [ProducesResponseType(typeof(DonationResponse[]), 200)]
        public async Task<IActionResult> GetDonationsOfYear(int year)
        {
            var donations = await _database.Donations
                .AsNoTracking()
                .Where(x => x.TimeOfDonation.Year == year)
                .OrderBy(x => x.TimeOfDonation)
                .ToListAsync();

            return Ok(donations.ToDonationResponseIEnumerable());
        }

        [AllowAnonymous]
        [HttpGet("statistics/topDonators/{limit:int}")]
        [ProducesResponseType(typeof(DonationResponse[]), 200)]
        public async Task<IActionResult> GetTopDonatorListDescending(int limit = 10)
        {
            var topDonators = await _database.Donations
                .AsNoTracking()
                .AsSplitQuery()
                .Include(x => x.User)
                .Where(x => x.UserId != null)
                .GroupBy(x => x.UserId)
                .Select(x => new
                {
                    UserId = x.Key,
                    UserName = x.GetEnumerator().Current.User.Username,
                    SumDonations = x.Sum(y => y.Amount)
                })
                .OrderByDescending(x => x.SumDonations)
                .Take(limit)
                .ToListAsync();

            return Ok(topDonators);
        }

        [RequirePermission("donation:create")]
        [HttpPost]
        [ProducesResponseType(typeof(DonationResponse), 201)]
        public async Task<IActionResult> CreateDonation([FromBody] CreateDonationRequest request)
        {
            var donation = new Donation
            {
                Amount = request.Amount,
                Note = request.Note,
                TimeOfDonation = request.TimeOfDonation,
                UserId = request.UserId
            };
            await _database.Donations.AddAsync(donation);
            await _database.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDonation), new {DonationId = donation.DonationId},
                donation.ToDonationResponse());
        }

        [RequirePermission("donation:update")]
        [HttpPut("{donationId:int}")]
        [ProducesResponseType(typeof(DonationResponse), 200)]
        public async Task<IActionResult> UpdateDonation(long donationId, [FromBody] UpdateDonationRequest request)
        {
            var donation = await _database.Donations.FirstOrDefaultAsync(x => x.DonationId == donationId);
            if (donation == null) return BadRequest("Donation Id provided does not exist");

            if (request.Amount.HasValue)
                donation.Amount = request.Amount.Value;
            if (!string.IsNullOrWhiteSpace(request.Note))
                donation.Note = request.Note;
            if (request.TimeOfDonation.HasValue)
                donation.TimeOfDonation = request.TimeOfDonation.Value;
            if (request.UserId.HasValue)
            {
                var user = await _database.Users.FirstOrDefaultAsync(x => x.UserId == request.UserId);
                if (user == null) return BadRequest("User Id provided does not exist");
                donation.User = user;
            }

            await _database.SaveChangesAsync();
            return Ok(donation.ToDonationResponse());
        }

        [RequirePermission("donation:remove")]
        [HttpDelete("{donationId:int}")]
        [ProducesResponseType( 200)]
        public async Task<IActionResult> RemoveDonation(long donationId)
        {
            var donation = await _database.Donations
                .FirstOrDefaultAsync(x => x.DonationId == donationId);
            if (donation == null) return BadRequest("Donation Id provided does not exist");

            _database.Donations.Remove(donation);
            await _database.SaveChangesAsync();

            return Ok();
        }
    }
}