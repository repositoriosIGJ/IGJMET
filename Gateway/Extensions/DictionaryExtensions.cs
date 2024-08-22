using System;
using System.Collections.Generic;
using Gateway.Queue;

namespace Gateway.Extensions
{
    public static class DictionaryExtensions
    {
        public static Pair[] ToPairs(this Dictionary<String, Object> value)
        {
            var pairList = new List<Pair>();

            foreach (var dic in value)
            {
                var element = new Pair { Key = dic.Key, Value = dic.Value.ToString() };

                if (dic.Value is Int32)
                    element.Type = ElementType.Integer;
                else if (dic.Value is String)
                    element.Type = ElementType.String;
                else if (dic.Value is Int64)
                    element.Type = ElementType.Long;
                else if (dic.Value is Boolean)
                    element.Type = ElementType.Boolean;

                pairList.Add(element);
            }

            return pairList.ToArray();
        }

        public static BPM.pair[] ToBpmPairs(this Dictionary<String, Object> value)
        {
            var pairList = new List<BPM.pair>();

            foreach (var dic in value)
            {
                var element = new BPM.pair { key = dic.Key, value = dic.Value.ToString() };

                pairList.Add(element);
            }

            return pairList.ToArray();
        }
    }
}
