using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FamilyTreeNet.Core.Dto;
using FamilyTreeNet.Core.Services;

namespace FamilyTreeNet.Core.Gedcom
{
    internal class FamilyReader : IGedcomReader
    {
        private readonly FamilyDto family = new FamilyDto();
        private readonly List<long> spouses = new List<long>();
        private readonly List<long> children = new List<long>();

        private bool inMarriage = false;
        private bool inDivorce = false;

        public FamilyReader(PropertyLine startLine)
        {
            // assume line is like: "0 @F123@ FAM"
            if (startLine == null)
            {
                throw new ArgumentNullException(nameof(startLine));
            }

            if (startLine.Value != "FAM")
            {
                throw new ArgumentException("This line should have the FAM type.", nameof(startLine));
            }

            this.family.Id = GedcomUtil.GetIdFromReference(startLine.Keyword);
        }

        public void ProcessNextLine(PropertyLine line)
        {
            if (line.Level == 1)
            {
                // reset date state machine
                inMarriage = false;
                inDivorce = false;
            }

            switch (line.Keyword)
            {
                case "MARR":
                    inMarriage = true;
                    break;
                case "DIV":
                    inDivorce = true;
                    break;
                case "DATE":
                    GedcomDate dt = GedcomDate.Parse(line.Value);
                    if (dt != null)
                    {
                        if (inMarriage)
                        {
                            this.family.MarriageDate = dt.Date;
                        }
                        else if (inDivorce)
                        {
                            this.family.DivorceDate = dt.Date;
                        }
                    }
                    break;
                case "PLAC":
                    if (inMarriage)
                    {
                        this.family.MarriagePlace = line.Value;
                    }
                    else if (inDivorce)
                    {
                        this.family.DivorcePlace= line.Value;
                    }
                    break;
                case "CHIL":
                    // 	1 CHIL @I321@
                    long cid = GedcomUtil.GetIdFromReference(line.Value);
                    this.children.Add(cid);
                    break;
                case "WIFE":
                // fall through to HUSB
                case "HUSB":
                    long sid = GedcomUtil.GetIdFromReference(line.Value);
                    this.spouses.Add(sid);
                    break;
            }
        }

        public async Task Store(TreeService treeService)
        {
            System.Diagnostics.Debug.WriteLine("Family {0}", this.family.Id);
            await treeService.Update(this.family);
            await treeService.UpdateRelations(this.family.Id, this.spouses, this.children);
        }
    }
}
