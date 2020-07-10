namespace FamilyTreeNet.Core.Dto
{
    public class NameCount
    {
        public NameCount(string lastname, long count)
        {
            this.Lastname = lastname;
            this.Count = count;
        }

        public string Lastname { get; }

        public long Count { get; }

        public override string ToString() => $"{Lastname} ({Count})";
    }
}
