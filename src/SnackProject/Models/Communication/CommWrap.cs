using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SnackProject.Models.Communication
{
    [DataContract]
    public class CommWrap<T>
    {
        [DataMember]
        public int RequestStatus { get; set; }
        [DataMember]
        public T Content { get; set; }
    }
}
