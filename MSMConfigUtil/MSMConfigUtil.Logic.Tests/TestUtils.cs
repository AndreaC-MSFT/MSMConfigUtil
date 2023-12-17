using Castle.Components.DictionaryAdapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMConfigUtil.Logic.Tests
{
    internal static class TestUtils
    {
        internal static bool KeyValuePairListsAreEquivalent<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> expected,  IEnumerable<KeyValuePair<TKey, TValue>> actual)
        {
            if (expected.Count() != actual.Count()) return false;
            foreach (var expectedItem in expected)
            {
                if (!actual.Any(a => object.Equals(a.Key, expectedItem.Key) && object.Equals(a.Value, expectedItem.Value)))
                    return false;
            }
            return true;
        }
    }
}
