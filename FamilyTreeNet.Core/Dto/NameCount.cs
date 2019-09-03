namespace FamilyTreeNet.Core.Dto
{
    public class NameCount
    {
        public NameCount(string lastname, int count)
        {
            this.Lastname = lastname;
            this.Count = count;
        }

        public string Lastname { get; }
        public int Count { get; }

        public override string ToString() => $"{Lastname} ({Count})";
    }
}
