using System;
using System.Collections.Generic;

namespace Awards
{
    [Serializable]
    public class StoredAwards
    {
        public List<StoredAward> awards;

        public StoredAwards(List<StoredAward> list) => awards = list;
    }
    
    [Serializable]
    public class StoredAward
    {
        public int count;
        public AwardType type;
        public bool isNew;

        public StoredAward(int count, AwardType type)
        {
            this.count = count;
            this.type = type;
            this.isNew = false;
        }
    }
}