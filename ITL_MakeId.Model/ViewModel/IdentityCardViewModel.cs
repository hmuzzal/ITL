﻿using ITL_MakeId.Model.DomainModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ITL_MakeId.Model.ViewModel
{
    public class IdentityCardViewModel

    {
        [Required(ErrorMessage = "Please, Enter your name")]
        public string Name { get; set; }

        [Display(Name = "Designation")]
        [Required(ErrorMessage = "Please, Select your designation")]
        public int DesignationId { get; set; }
        public Designation Designation { get; set; }

        [Required(ErrorMessage = "Please, Select your blood group")]
        public int BloodGroupId { get; set; }

        [Display(Name = "Blood Group")]
        public BloodGroup BloodGroup { get; set; }

        [Display(Name = "Card Number")]
        public string CardNumber { get; set; }

        [Display(Name = "User Image")]
        [Required(ErrorMessage = "Please, Select your image")]
        public IFormFile ImagePathOfUser { get; set; }

        public IFormFile ImagePathOfUserSignature { get; set; }
        public IFormFile ImagePathOfAuthorizedSignature { get; set; }

        [Display(Name = "Company ")]
        public string CompanyName { get; set; }

        [Display(Name = "Company Address")]
        public string CompanyAddress { get; set; }

        public IFormFile CompanyLogoPath { get; set; }

        public string CardInfo { get; set; }

        public DateTime ValidationStartDate { get; set; }
        public DateTime ValidationEndDate { get; set; }
        public string GetCardNumber(string dbCardNumber)
        {
            if (dbCardNumber == null)
            {
                dbCardNumber = "ITL-0000";
            }
            string result = string.Empty;
            string numberStr = string.Empty;

            int i = dbCardNumber.Length - 1;
            for (; i > 0; i--)
            {
                char c = dbCardNumber[i];
                if (!char.IsDigit(c))
                    break;
                numberStr = c + numberStr;

            }

            int number = int.Parse(numberStr);
            number++;

            result += dbCardNumber.Substring(0, i + 1);
            result += "000";
            result += number;

            return result;
        }


        public IdentityCard IdentityCard { get; set; }

        public IEnumerable<IdentityCard> IdentityCards { get; set; }
        public IdentityCardViewModel()
        {
            IdentityCard = new IdentityCard();
            IdentityCards=new List<IdentityCard>();
        }


    }
}
