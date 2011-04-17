using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace VBKiller.Entity
{
    [DataObject(Size = 0x4)]
    public class ProcName : EntityBase<ProcName>
    {
        private Int32 _Proc;
        /// <summary>过程名</summary>
        [DataField(typeof(String), RefKinds.Virtual)]
        public Int32 Proc
        {
            get { return _Proc; }
            set { _Proc = value; }
        }

        public String Name { get { return Proc <= 0 ? null : (String)this["Proc"]; } set { Extends["Proc"] = value; } }

        private String _FriendName;
        /// <summary>友好名称</summary>
        public String FriendName
        {
            get { return _FriendName; }
            set { _FriendName = value; }
        }

        public override string ToString()
        {
            if (Proc > 0 && !String.IsNullOrEmpty(FriendName))
                return FriendName;
            else
                return base.ToString();
        }

        public override EntityBase2[] ReadExtendList(BinaryReader reader, int count)
        {
            EntityBase2[] list = base.ReadExtendList(reader, count);
            if (list == null || list.Length <= 0) return list;

            for (int i = 0; i < list.Length; i++)
            {
                ProcName entity1 = list[i] as ProcName;
                if (i < list.Length - 1)
                {
                    // 如果与下一个相等，则为set/get
                    ProcName entity2 = list[i + 1] as ProcName;

                    if (!String.IsNullOrEmpty(entity1.Name) && !String.IsNullOrEmpty(entity2.Name) && entity1.Name == entity2.Name)
                    {
                        entity1.FriendName = "set_" + entity1.Name;
                        entity2.FriendName = "get_" + entity2.Name;
                        i++;
                        continue;
                    }
                }

                if (String.IsNullOrEmpty(entity1.Name))
                    entity1.FriendName = (i + 1).ToString("X2");
                else
                    entity1.FriendName = entity1.Name;
            }

            return list;
        }
    }
}
