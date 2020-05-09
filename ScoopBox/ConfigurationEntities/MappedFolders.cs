﻿using System.Collections.Generic;
using System.Xml.Serialization;

namespace ScoopBox.ConfigurationEntities
{
    [XmlRoot(ElementName = nameof(MappedFolders))]
    public class MappedFolders
    {
        [XmlElement(ElementName = nameof(MappedFolder))]
        public List<MappedFolder> MappedFolder { get; set; }
    }
}
