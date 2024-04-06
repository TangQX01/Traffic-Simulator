using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SIP.Common;

namespace SIP.FastVRTools
{
    [AddComponentMenu("SIP/FastVRTool/Managers/Base Manager")]
    public class FVRManagerBase : MonoSingleton<FVRManagerBase>
    {
        public virtual void Awake()
        {
            MsgHandler();
        }

        public void MsgHandler()
        {
            Messenger.AddListener<MSG_TYPES, object, object>(MSG_TYPES.SysMsg.ToString(), SystemMsg);
            Messenger.MarkAsPermanent(MSG_TYPES.SysMsg.ToString());
        }

        public virtual void SystemMsg(MSG_TYPES msgType, object msgId, object param)
        {

        }
    }
}
