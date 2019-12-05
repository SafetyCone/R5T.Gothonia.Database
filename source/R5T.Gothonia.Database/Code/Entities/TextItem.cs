using System;


namespace R5T.Gothonia.Database.Entities
{
    public class TextItem
    {
        public int ID { get; set; }

        public Guid GUID { get; set; }

        public string Value { get; set; }

        public int? TextItemTypeID { get; set; }
        public TextItemType TextItemType { get; set; }
    }
}
