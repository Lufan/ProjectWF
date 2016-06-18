
namespace DomainLayer.Contact
{
    public sealed class EnPosition : IPosition
    {
        public EnPosition()
        { }

        public EnPosition(IPosition position)
        {
            if (position != null)
            {
                Position = position.Position;
                Group = position.Group;
                Department = position.Department;
                Team = position.Team;
            }
        }

        public string Position { get; set; }

        public string Group { get; set; }

        public string Department { get; set; }

        public string Team { get; set; }
    }
}
