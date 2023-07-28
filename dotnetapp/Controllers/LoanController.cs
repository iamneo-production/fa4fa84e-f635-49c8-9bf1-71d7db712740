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
         [HttpGet("user/getDocuments/{loanId}")]
         public async Task<IActionResult> getDocuments(int loanId)
         {

            var appliedLoanDocs = await _context.Document.FirstOrDefaultAsync(p => p.documentId == loanId);

            if (appliedLoanDocs == null)
            {
                return Ok(new
                 {
                    Message = "No document found"
               });
             }

             return Ok(appliedLoanDocs);
         }



         [HttpGet("user/getDocuments")]
            public async Task<IActionResult> getDocuments()
            {
                var UserDocuments = await _context.User.ToListAsync();

                if (UserDocuments == null || UserDocuments.Count == 0)
                {
                    return NotFound(new
                    {
                        Message = "No user found"
                    });
                }

                return Ok(new
                {
                    Message = "View Documents",
                    UserDetails = UserDocuments
                });

            }



        [HttpPut("user/editDocuments/{documentId}")]
        public async Task<IActionResult> editDocuments(int documentId, [FromBody] DocumentModel documentModel)
        {
            if (documentModel == null || documentId != documentModel.documentId)
            {
                return BadRequest();
            }

            var Documentdetails = await _context.Document.FindAsync(documentId);

            if (Documentdetails == null)
            {
                return NotFound();
            }
            Documentdetails.documenttype = documentModel.documenttype;
            Documentdetails.documentupload = documentModel.documentupload;
            _context.Document.Update(Documentdetails);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Edit documents",
                loandetails = Documentdetails
            });
        }

        [HttpDelete("user/deletedocuments/{documentId}")]
        public async Task<IActionResult> deletedocuments(int documentId)
        {
            var documentdetails = await _context.Document.FindAsync(documentId);

            if (documentdetails == null)
            {
                return NotFound();
            }

            _context.Document.Remove(documentdetails);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Delete documents",
                loandetails = documentdetails
            });
        }
    }
}