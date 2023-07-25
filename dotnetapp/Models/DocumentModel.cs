using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace dotnetapp.Models
{
    public class DocumentModel
    {
        [Key]
        public int documentId { get; set; }

        [Required]
        public string documenttype { get; set; }

        [NotMapped]
        public IFormFile DocumentUploads { get; set; }

        public byte[] documentupload { get; set; }

        public string documentVerified { get; set; }

    }
}