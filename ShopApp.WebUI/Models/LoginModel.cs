﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUI.Models
{
    public class LoginModel
    {
        #region UserName Login

        //[Required] 
        //public string UserName { get; set; }

        #endregion

        #region Email Login

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        #endregion


        [Required] 
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }

    }
}
