using System;
using System.Collections.Generic;
using System.Text;

namespace Sigma.Roadie.Domain.Models
{

    public class SceneModel
    {

        public Guid SceneId { get; set; }

        public string Name { get; set; }


        public List<MediaFileModel> MediaFiles { get; set; }


    }



    public class MediaFileModel
    {

        public Guid MediaFileId { get; set; }

        public string LocalUri { get; set; }

    }

}
