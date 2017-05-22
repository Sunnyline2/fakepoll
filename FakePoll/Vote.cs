namespace FakePoll
{
    public class Vote
    {
        public string ID { get; }
        public string Name { get; }
        public Vote(string id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}