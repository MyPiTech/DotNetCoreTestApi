﻿using System.ComponentModel.DataAnnotations;

namespace TestApi.Dtos
{
    public class CreateUserDto
    {
        [Required]
		[StringLength(20)]
		public string FirstName { get; set; }
        [Required]
		[StringLength(20)]
		public string LastName { get; set; }
		[StringLength(500)]
		public string Notes { get; set; }
    }
}
