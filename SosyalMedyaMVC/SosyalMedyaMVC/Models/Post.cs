using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SosyalMedyaMVC.Models
{
    public class Post
    {
        public int Id { get; set; }

        [StringLength(100, ErrorMessage = "Başlık en fazla 100 karakter olmalıdır.")]
        public string? Title { get; set; }

        [StringLength(400, ErrorMessage = "İçerik en fazla 400 karakter olmalıdır.")]
        public string? Content { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string? UserName { get; set; }
        public string? FullName { get; set; }

        public string? ImagePath { get; set; }

        public int LikeCount { get; set; } = 0;

        // Bu, Comment nesnelerini doğrudan referans olarak tutar
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
