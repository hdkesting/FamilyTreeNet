using System;
using System.Threading.Tasks;
using FamilyTreeNet.Core.Dto;
using FamilyTreeNet.Core.Services;

namespace FamilyTreeNet.Core.Gedcom
{
    internal class IndividualReader : IGedcomReader
    {
        private readonly IndividualDto individual = new IndividualDto();

        private bool inBirth;
        private bool inDeath;

        public IndividualReader(PropertyLine startLine)
        {
            // assume line is like: "0 @I123@ INDI"
            if (startLine == null)
            {
                throw new ArgumentNullException(nameof(startLine));
            }

            if (startLine.Value != "INDI")
            {
                throw new ArgumentException("This line should have the INDI type.", nameof(startLine));
            }

            this.individual.Id = GedcomUtil.GetIdFromReference(startLine.Keyword);

        }

        public void ProcessNextLine(PropertyLine line)
        {
            if (line.Level == 1)
            {
                // reset state machine
                inBirth = false;
                inDeath = false;
            }

            switch (line.Keyword)
            {
                case "NAME":
                    ProcessName(line.Value);
                    break;
                case "SEX":
                    switch (line.Value)
                    {
                        case "M":
                            this.individual.Sex = Support.Sex.Male;
                            break;
                        case "F":
                            this.individual.Sex = Support.Sex.Female;
                            break;
                    }
                    break;
                case "BIRT":
                    inBirth = true;
                    break;
                case "DEAT":
                    inDeath = true;
                    break;
                case "DATE":
                    GedcomDate dt = GedcomDate.Parse(line.Value);
                    if (dt != null)
                    {
                        if (inBirth)
                        {
                            this.individual.BirthDate = dt.Date;
                        }
                        else if (inDeath)
                        {
                            this.individual.DeathDate = dt.Date;
                        }
                    }
                    break;
                case "PLAC":
                    if (inBirth)
                    {
                        this.individual.BirthPlace = line.Value;
                    }
                    else if (inDeath)
                    {
                        this.individual.DeathPlace = line.Value;
                    }
                    break;
            }

        }

        public async Task Store(TreeService treeService)
        {
            System.Diagnostics.Debug.WriteLine("Individual {0}, {1} {2}", this.individual.Id, this.individual.Firstnames, this.individual.Lastname);
            await treeService.Update(this.individual);
        }

        private void ProcessName(string fullName)
        {
            // fullName is like "first name /lastname/", so split in "first name" and "lastname"
            int p1 = fullName.IndexOf("/");
            string name = fullName.Substring(0, p1 - 1).Trim();
            this.individual.Firstnames = name;
            int p2 = fullName.IndexOf("/", p1 + 1);
            name = fullName.Substring(p1 + 1, p2 - p1 - 1);
            if (name.StartsWith("??"))
            {
                name = "??";
            }

            this.individual.Lastname = name;
        }
    }
}
