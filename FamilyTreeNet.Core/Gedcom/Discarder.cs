using FamilyTreeNet.Core.Services;
using System.Threading.Tasks;

namespace FamilyTreeNet.Core.Gedcom
{
    internal class Discarder : IGedcomReader
    {
        public void ProcessNextLine(PropertyLine line)
        {
            // no implementation needed
        }

        public Task Store(TreeService treeService)
        {
            return Task.CompletedTask;
        }
    }
}
