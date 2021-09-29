using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;
using TinkoffPortfolioAnalyzer.Models;

namespace TinkoffPortfolioAnalyzer.Services
{
    internal class SnapshotService : ISnapshotService
    {
        private const string SnapPath = "./Snapshots";
        private const string DateTimeFormat = "yyyy-MM-dd_HH-mm-ss";
        private ObservableCollection<AvailSecSnapshot> _availSecSnapshots = new();
        XmlSerializer _xmlFormatter = new XmlSerializer(typeof(SecurityInfoList));

        public SnapshotService()
        {
            Directory.CreateDirectory(SnapPath);
            var files = Directory.GetFiles(SnapPath, "*.xml");
            foreach (var file in files)
            {
                using (var stream = File.OpenRead(file))
                {
                    var securityInfoList = _xmlFormatter.Deserialize(stream);
                    var newSnapshot = new AvailSecSnapshot
                    {
                        CreatedDateTime = DateTime.ParseExact(Path.GetFileNameWithoutExtension(file), DateTimeFormat, null),
                        Securities = (securityInfoList as SecurityInfoList).List
                    };
                    _availSecSnapshots.Add(newSnapshot);
                }
            }
        }

        public void CreateSnapshot(SecurityInfoList securityInfoList)
        {
            var dateTimeNow = DateTime.Now;
            var dateTimeStr = dateTimeNow.ToString(DateTimeFormat);
            using (var stream = File.OpenWrite($"{SnapPath}/{dateTimeStr}.xml"))
            {
                _xmlFormatter.Serialize(stream, securityInfoList);
            }
            var newSnapshot = new AvailSecSnapshot
            {
                CreatedDateTime = dateTimeNow,
                Securities = securityInfoList.List
            };
            _availSecSnapshots.Add(newSnapshot);
        }

        public IEnumerable<AvailSecSnapshot> GetSnapshots()
        {
            return _availSecSnapshots;
        }
    }
}
