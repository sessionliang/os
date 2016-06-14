using Xunit;
using System;
using System.Collections.Generic;
using BaiRong.Core;

public class StringUtilsTests
{
    [Fact]
    public void EqualsIgnoreOrderTests()
    {
        Assert.True(StringUtils.EqualsIgnoreOrder(new List<int> { 1, 2, 3 }, "1,3,2"));
        Assert.False(StringUtils.EqualsIgnoreOrder(new List<int> { 1, 2, 3 }, "1,2"));
    }
}
