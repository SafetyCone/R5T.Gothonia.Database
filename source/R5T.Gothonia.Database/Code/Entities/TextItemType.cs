using System;


namespace R5T.Gothonia.Database.Entities
{
    public class TextItemType
    {
        public int ID { get; set; }

        public Guid GUID { get; set; }

        /// <summary>
        /// Will also be a unique identifier.
        /// </summary>
        public string Name { get; set; }
    }
}
