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
        [HttpPost("user/addLoan")]
        public async Task<IActionResult> addLoan([FromBody] LoanApplicantModel loanobj)
        {
            if (loanobj == null)
            {
                return BadRequest();
            }

            await _context.LoanApplicant.AddAsync(loanobj);
            await _context.SaveChangesAsync();
            string loanAmountRequired = loanobj.loanAmountRequired;
            string loanRepayMonths = loanobj.loanRepaymentMonths;
            string salary = loanobj.applicantSalary;

            decimal loanAmount = Convert.ToDecimal(loanAmountRequired);
            int loanMonths = Convert.ToInt32(loanRepayMonths);
            decimal applicantsalary = Convert.ToDecimal(salary);

            decimal totalRepaymentAmount = loanMonths * applicantsalary;
            decimal interestAmount = totalRepaymentAmount - loanAmount;
            decimal monthlyInterestRate = (interestAmount / loanAmount) / loanMonths / 100;
            decimal emi = loanAmount * monthlyInterestRate *
                          (decimal)Math.Pow(1 + (double)monthlyInterestRate, loanMonths) /
                          ((decimal)Math.Pow(1 + (double)monthlyInterestRate, loanMonths) - 1);

            loanobj.MonthlyEMI = emi;

            await _context.SaveChangesAsync();

            return Ok(new { message = "success" });
        }

        
        


        
        [HttpPost("user/addDocuments")]
        public async Task<IActionResult> AddDocument([FromForm] DocumentModel documentModel)
        {
            if (documentModel == null || documentModel.DocumentUploads == null)
            {
                return BadRequest();
            }

            using (var memoryStream = new MemoryStream())
            {
                await documentModel.DocumentUploads.CopyToAsync(memoryStream);
                documentModel.documentupload = memoryStream.ToArray();

                _context.Document.Add(documentModel);
                await _context.SaveChangesAsync();



                return Ok(documentModel.documentId);
            }
        }
        
    }
}