using FamilyTreeNet.Core.Gedcom;
using System.IO;
using System.Threading.Tasks;

namespace FamilyTreeNet.Core.Services
{
    internal class GedcomFileReader
    {
        private readonly TreeService treeService;

        public GedcomFileReader(TreeService treeService)
        {
            this.treeService = treeService;
        }

        public async Task<bool> ReadFile(Stream input)
        {
            using (StreamReader sr = new StreamReader(input))
            {
                await ReadFile(sr);
                return true;
            }
        }

        public async Task ReadFile(StreamReader reader)
        {
            string line;
            IGedcomReader objectReader = new Discarder();

            while ((line = reader.ReadLine()) != null)
            {
                PropertyLine pline = new PropertyLine(line);

                if (pline.Level == 0)
                {
                    // save previous object
                    await objectReader.Store(this.treeService);

                    switch (pline.Value)
                    {
                        case "INDI":
                            objectReader = new IndividualReader(pline);
                            break;

                        case "FAM":
                            objectReader = new FamilyReader(pline);
                            break;

                        default:
                            // HEAD, TRLR, SOUR, NOTE, ...
                            objectReader = new Discarder();
                            break;
                    }
                }
                else
                {
                    objectReader.ProcessNextLine(pline);
                }
            }

            // and store the last object (although that would be an ignored TRLR in a real file)
            await objectReader.Store(this.treeService);
        }
    }
}
