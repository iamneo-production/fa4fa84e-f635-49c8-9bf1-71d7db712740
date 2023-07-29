using dotnetapp.Data;
using dotnetapp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Loan.Controller
{

    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public LoanController(ApplicationDbContext applicationdbcontext)
        {
            _context = applicationdbcontext;
        }
        

        [HttpPut("user/editLoan/{loanId}")]
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
        [HttpGet("user/viewLoan/{loanId}")]
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


        [HttpGet("user/viewLoan")]
            public async Task<IActionResult> viewLoan()
            {
                var viewLoan = await _context.User.ToListAsync();

                if (viewLoan == null || viewLoan.Count == 0)
                {
                    return NotFound(new
                    {
                        Message = "No user found"
                    });
                }

                return Ok(new
                {
                    Message = "View Loan",
                    UserDetails = viewLoan
                });

            }
        
        


        [HttpDelete("user/deleteLoan/{loanId}")]
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