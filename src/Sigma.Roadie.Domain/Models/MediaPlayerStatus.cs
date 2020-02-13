using System;
using System.Collections.Generic;
using System.Text;

namespace Sigma.Roadie.Domain.Models
{

    public class MediaPlayerStatus
    {

        public string Hostname { get; set; }

        public DateTime LocalDateTime { get; set; }

        public List<MediaFileStatus> MediaFiles { get; set; }

    }


    public class MediaFileStatus
    {

        public Guid MediaFileId { get; set; }

        public TimeSpan PlaysIn { get; set; }

        public TimeSpan PlayingFor { get; set; }

    }

}
