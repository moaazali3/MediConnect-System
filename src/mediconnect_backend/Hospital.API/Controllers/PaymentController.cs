using Hospital.Application.DTOs.Payment;
using Hospital.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("{appointmentId}")]
        public async Task<IActionResult> GetPaymentsByAppointmentId(Guid appointmentId)
        {
            try
            {
                var payments = await _paymentService.GetPaymentsByAppointmentId(appointmentId);
                return Ok(payments);
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }

        [HttpPost("{appointmentId}")]
        public async Task<IActionResult> CreatePayment(Guid appointmentId, CreatePaymentDto model)
        {
            try
            {
                await _paymentService.CreatePayment(appointmentId, model);
                return Ok("Payment created successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }
    }
}
