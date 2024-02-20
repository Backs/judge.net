using System.Collections.Generic;
using System.Xml.Serialization;

namespace Judge.JudgeService.Settings
{
    [XmlRoot(ElementName = "Problem")]
    public class ProblemSettings
    {
        [XmlAttribute(AttributeName = "ProblemId")]
        public long ProblemId { get; set; }

        [XmlAttribute(AttributeName = "Language")]
        public int Language { get; set; }
    }

    [XmlRoot(ElementName = "Contest")]
    public class ContestSettings
    {
        [XmlElement(ElementName = "Problem")]
        public List<ProblemSettings> Problem { get; set; }

        [XmlAttribute(AttributeName = "Id")]
        public int Id { get; set; }
    }

    [XmlRoot(ElementName = "Contests")]
    public class ContestsSettings
    {
        [XmlElement(ElementName = "Contest")]
        public List<ContestSettings> Contest { get; set; } = new List<ContestSettings>();
    }

    [XmlRoot(ElementName = "CustomProblemSettings")]
    public class CustomProblemSettings
    {
        [XmlElement(ElementName = "Contests")]
        public ContestsSettings Contests { get; set; } = new ContestsSettings();

        public static CustomProblemSettings Empty { get; } = new CustomProblemSettings();
    }
}