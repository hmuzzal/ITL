using System;

namespace ITL_MakeId.Model.DomainModel
{
    public class IdentityCard
    {
        public int Id { get; set; }
        //[Display(Name = "Name")]
        public string Name { get; set; }

        public int DesignationId { get; set; }
        public Designation Designation { get; set; }

        public int BloodGroupId { get; set; }

        //[Display(Name = "Blood Group")]
        public BloodGroup BloodGroup { get; set; }



        //[Display(Name = "Card Number")]
        public string CardNumber { get; set; }

        public string ImagePathOfUser { get; set; }
        public string ImagePathOfUserSignature { get; set; }

        public string ImagePathOfAuthorizedSignature { get; set; }

        //[Display(Name = "Company ")]
        public string CompanyName { get; set; }

        //[Display(Name = "Company Address")]
        public string CompanyAddress { get; set; }

        public string CompanyLogoPath { get; set; }

        public string CardInfo { get; set; }

        public DateTime ValidationStartDate { get; set; }
        public DateTime ValidationEndDate { get; set; }

    }
}
