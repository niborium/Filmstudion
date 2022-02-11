using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace filmstudion.api.Models
{

    interface IUserRegister
    {
      public bool IsAdmin { get; set; }
      public string Username { get; set; }
      public string Password { get; set; }
    }
}