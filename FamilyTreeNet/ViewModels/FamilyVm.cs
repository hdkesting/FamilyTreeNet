using FamilyTreeNet.Core.Dto;
using System;
using System.Collections.Generic;

namespace FamilyTreeNet.ViewModels
{
    public class FamilyVm
    {
        public FamilyVm()
        {
        }

        public FamilyVm(FamilyDto dto)
        {
            this.Id = dto.Id;
            this.MarriageDate = dto.MarriageDate;
            this.MarriagePlace = dto.MarriagePlace;
            this.DivorceDate = dto.DivorceDate;
            this.DivorcePlace = dto.DivorcePlace;

            foreach(var sp in dto.Spouses)
            {
                AddSpouse(new IndividualVm(sp));
            }

            foreach(var ch in dto.Children)
            {
                this.Children.Add(new IndividualVm(ch));
            }
        }

        public long Id { get; set; }

        public DateTime? MarriageDate { get; set; }
        public string MarriageDateFmt => Utils.FormatDate(MarriageDate);

        public string MarriagePlace { get; set; }

        public DateTime? DivorceDate { get; set; }
        public string DivorceDateFmt => Utils.FormatDate(DivorceDate);

        public string DivorcePlace { get; set; }

        public IndividualVm Husband { get; set; }
        public IndividualVm Wife { get; set; }

        public List<IndividualVm> Children { get; } = new List<IndividualVm>();

        private void AddSpouse(IndividualVm spouse)
        {
            if (spouse.IsMale)
            {
                if (this.Husband != null && this.Wife == null)
                {
                    this.Wife = this.Husband;
                }
                this.Husband = spouse;
            }
            else if (spouse.IsFemale)
            {
                if (this.Husband == null && this.Wife != null)
                {
                    this.Husband = this.Wife;
                }
                this.Wife = spouse;
            }
            else
            {
                // sex unknown, add in any open spot
                if (this.Husband == null)
                {
                    this.Husband = spouse;
                }
                else if (this.Wife == null)
                {
                    this.Wife = spouse;
                }
                // else ignore: don't want to overwrite with third spouse
            }

        }
    }
}
