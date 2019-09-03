using FamilyTreeNet.Core.Services;
using System.Threading.Tasks;

namespace FamilyTreeNet.Core.Gedcom
{
    internal interface GedcomReader
    {
        /// <summary>
        /// Processes the next line (level &gt; 0).
        /// </summary>
        /// <param name="line">The line.</param>
        void ProcessNextLine(PropertyLine line);

        /// <summary>
        /// Stores this object using the specified tree service.
        /// </summary>
        /// <param name="treeService">The tree service.</param>
        Task Store(TreeService treeService);
    }
}
