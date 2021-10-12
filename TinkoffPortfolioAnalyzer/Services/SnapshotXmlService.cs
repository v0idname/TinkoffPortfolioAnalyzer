﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;
using TinkoffPortfolioAnalyzer.Models;

namespace TinkoffPortfolioAnalyzer.Services
{
    internal class SnapshotXmlService : ISnapshotsRepository
    {
        private const string SnapPath = "./Snapshots";
        private const string DateTimeFormat = "yyyy-MM-dd_HH-mm-ss";
        private readonly ObservableCollection<AvailSecSnapshot> _availSecSnapshots = new();
        XmlSerializer _xmlFormatter = new XmlSerializer(typeof(SecurityInfoList));

        public SnapshotXmlService()
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

        public void Create(SecurityInfoList securityInfoList)
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

        public void Remove(AvailSecSnapshot snapshotToDelete)
        {
            _availSecSnapshots.Remove(snapshotToDelete);
            File.Delete($"{SnapPath}/{snapshotToDelete.CreatedDateTime.ToString(DateTimeFormat)}.xml");
        }

        public IEnumerable<AvailSecSnapshot> GetAll() => _availSecSnapshots;
    }
}
