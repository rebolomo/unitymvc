using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityMVC.Protocol
{
    /// <summary>
    /// 
    /// 
    /// </summary>
    public enum MobaChannel:byte
    {
        /// <summary>
        /// 
        /// </summary>
        Default = 0,
        PVP = 1,    //pvp message
        Friend = 2  //friend reponse
    }

}
