using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TinkoffPortfolioAnalyzer.Models;

namespace TinkoffPortfolioAnalyzer.Data.Repositories
{
    internal class SnapshotsXmlRepository : ISnapshotsRepository
    {
        private const string SnapPath = "./Snapshots";
        private const string DateTimeFormat = "yyyy-MM-dd_HH-mm-ss";
        private readonly ObservableCollection<AvailSecSnapshot> _availSecSnapshots = new();
        XmlSerializer _xmlFormatter = new XmlSerializer(typeof(SecurityInfoList));

        public SnapshotsXmlRepository()
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

        public async Task CreateAsync(SecurityInfoList securityInfoList)
        {
            var dateTimeNow = DateTime.Now;
            var dateTimeStr = dateTimeNow.ToString(DateTimeFormat);
            await Task.Run(() =>
            {
                using var stream = File.OpenWrite($"{SnapPath}/{dateTimeStr}.xml");
                _xmlFormatter.Serialize(stream, securityInfoList);
            });
            var newSnapshot = new AvailSecSnapshot
            {
                CreatedDateTime = dateTimeNow,
                Securities = securityInfoList.List
            };
            _availSecSnapshots.Add(newSnapshot);
        }

        public async Task RemoveAsync(AvailSecSnapshot snapshotToDelete)
        {
            _availSecSnapshots.Remove(snapshotToDelete);
            await Task.Run(() =>
            {
                File.Delete($"{SnapPath}/{snapshotToDelete.CreatedDateTime.ToString(DateTimeFormat)}.xml");
            });
        }

        public async Task<IEnumerable<AvailSecSnapshot>> GetAllAsync()
        {
            return await Task.FromResult(_availSecSnapshots);
        }
    }
}
