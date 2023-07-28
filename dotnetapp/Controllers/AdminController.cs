using dotnetapp.Data;
using dotnetapp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Runtime.Intrinsics.X86;
using System;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;


//using EMI.Data;
//using EMI.Model;

namespace Admin.Controller
{
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public AdminController(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }
        [HttpGet("admin/getAllLoans")]
        public async Task<IActionResult> getAllLoan()
        {
            var appliedloans = await _context.LoanApplicant.ToListAsync();

            if (appliedloans == null || appliedloans.Count == 0)
            {
                return NotFound(new
                {
                    Message = "No user found"
                });
            }

            return Ok(appliedloans);

        }

        [HttpPost("admin/getAllLoans/approve/{id}")]
        public IActionResult ApproveLoan(int id, [FromBody] LoanApplicantModel request)
        {
            var result = _context.LoanApplicant.Where(x => x.loanId == id).FirstOrDefault();
            if (result != null)
            {
                result.loanStatus = "approved";
                _context.SaveChanges();
                return Ok(new { Status = "Success" });
            }
            else
            {
                return BadRequest(new { Status = "Error", Error = "Error in updating approval status" });
            }

        }

        [HttpPost("admin/getAllLoans/reject/{id}")]
        public IActionResult RejectLoan(int id, [FromBody] LoanApplicantModel request)
        {
            var loanApplicant = _context.LoanApplicant.FirstOrDefault(a => a.loanId == id);
            if (loanApplicant == null)
            {
                return BadRequest(new { Status = "Error", Error = "Loan applicant not found" });
            }

            loanApplicant.loanStatus = "rejected";

            _context.SaveChanges();

            return Ok(new { Status = "Success" });

        }
        [HttpPut("approve")]
        public IActionResult ApproveLoans([FromBody] int[] loanIds)
        {
            var loans = _context.LoanApplicant.Where(x => loanIds.Contains(x.loanId));
            foreach (var loan in loans)
            {
                loan.loanStatus = "approved";
            }

            _context.SaveChanges();

            return Ok(new { Status = "Success", Message = "Loans approved successfully" });
        }

        [HttpPut("reject")]
        public IActionResult RejectLoans([FromBody] int[] loanIds)
        {
            var loans = _context.LoanApplicant.Where(x => loanIds.Contains(x.loanId));
            foreach (var loan in loans)
            {
                loan.loanStatus = "rejected";
            }

            _context.SaveChanges();

            return Ok(new { Status = "Success", Message = "Loans rejected successfully" });
        }

         [HttpGet("admin/generateSchedule")]
            public async Task<IActionResult>generateSchedule ()
            {
                var generateSchedule = await _context.LoanApplicant.ToListAsync();

                if (generateSchedule == null || generateSchedule.Count == 0)
                {
                    return NotFound(new
                    {
                        Message = "No user found"
                    });
                }

                return Ok(new
                {
                    Message = "Schedule generated",
                    UserDetails = generateSchedule
                });

            }

        [HttpGet("admin/getAllLoans/{loanId}")]
        public async Task<IActionResult> viewLoan(int loanId)
        {
            var appliedLoan = await _context.LoanApplicant.FirstOrDefaultAsync(p => p.loanId == loanId);

            if (appliedLoan == null)
            {
                return NotFound(new
                {
                    Message = "No user found"
                });
            }

            return Ok(appliedLoan);
        }
        [HttpGet("admin/getLoans/{name}")]
        public async Task<IActionResult> viewLoan(string name)
        {
            var appliedLoan = await _context.LoanApplicant.FirstOrDefaultAsync(p => p.applicantName == name);

            if (appliedLoan == null)
            {
                return NotFound(new
                {
                    Message = "No user found"
                });
            }

            return Ok(appliedLoan);
        }

        [HttpPut("admin/editLoan/{loanId}")]
        public async Task<IActionResult> editLoan(int loanId, [FromBody] LoanApplicantModel loanApplicantModel)
        {
            if (loanApplicantModel == null || loanId != loanApplicantModel.loanId)
            {
                return BadRequest();
            }

            var loanApplicant = await _context.LoanApplicant.FindAsync(loanId);

            if (loanApplicant == null)
            {
                return NotFound();
            }

            // loanApplicant.loanId = loanApplicantModel.loanId;
            loanApplicant.loantype = loanApplicantModel.loantype;
            loanApplicant.applicantName = loanApplicantModel.applicantName;
            loanApplicant.applicantAddress = loanApplicantModel.applicantAddress;
            loanApplicant.applicantMobile = loanApplicantModel.applicantMobile;
            loanApplicant.applicantEmail = loanApplicantModel.applicantEmail;
            loanApplicant.applicantAadhaar = loanApplicantModel.applicantAadhaar;
            loanApplicant.applicantPan = loanApplicantModel.applicantPan;
            loanApplicant.applicantSalary = loanApplicantModel.applicantSalary;
            loanApplicant.loanAmountRequired = loanApplicantModel.loanAmountRequired;
            loanApplicant.loanRepaymentMonths = loanApplicantModel.loanRepaymentMonths;



            _context.LoanApplicant.Update(loanApplicant);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Edit Applied loan",
                loandetails = loanApplicant
            });
        }
        [HttpDelete("admin/deleteLoan/{loanId}")]
        public async Task<IActionResult> deleteloan(int loanId)
        {
            var Loandetails = await _context.LoanApplicant.FindAsync(loanId);

            if (Loandetails == null)
            {
                return NotFound();
            }

            _context.LoanApplicant.Remove(Loandetails);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Delete loanapplication",
                loandetails = Loandetails
            });
        }

    }
}
