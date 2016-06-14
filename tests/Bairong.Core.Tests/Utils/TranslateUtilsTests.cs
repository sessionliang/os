using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaiRong.Core;

public class TranslateUtilsTests
{
    [Fact]
    public void StringCollectionToIntList()
    {
        Assert.Equal(TranslateUtils.StringCollectionToIntList("1,2,3"), new List<int> { 1, 2, 3 });
    }
}
