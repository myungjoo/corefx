// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using Xunit;

public partial class CompareInfoTests
{
    [Theory]
    [InlineData("")]
    [InlineData("en")]
    [InlineData("en-US")]
    public static void GetCompareInfo(string localeName)
    {
        CompareInfo ci = CompareInfo.GetCompareInfo(localeName);

        Assert.Equal(localeName, ci.Name);
    }

    [Fact]
    public static void GetCompareInfoBadCompareType()
    {
        Assert.Throws<ArgumentNullException>(() => CompareInfo.GetCompareInfo(null));
    }

    [Theory]
    [InlineData(null, -1)]
    [InlineData("", -1)]
    [InlineData("abc", -1)]
    [InlineData("abc", 4)]
    public static void CompareArgumentOutOfRangeIndex(string badString, int badOffset)
    {
        CompareInfo ci = CultureInfo.InvariantCulture.CompareInfo;

        Assert.Throws<ArgumentOutOfRangeException>(() => ci.Compare("good", 0, badString, badOffset));
        Assert.Throws<ArgumentOutOfRangeException>(() => ci.Compare(badString, badOffset, "good", 0));

        Assert.Throws<ArgumentOutOfRangeException>(() => ci.Compare("good", 0, badString, badOffset, CompareOptions.None));
        Assert.Throws<ArgumentOutOfRangeException>(() => ci.Compare(badString, badOffset, "good", 0, CompareOptions.None));
    }

    [Theory]
    [InlineData(null, 0, -1)]
    [InlineData(null, -1, 0)]
    [InlineData(null, -1, -1)]
    [InlineData("", 0, -1)]
    [InlineData("", -1, 0)]
    [InlineData("", -1, -1)]
    [InlineData("abc", 0, 4)]
    [InlineData("abc", 4, 0)]
    [InlineData("abc", 0, -1)]
    [InlineData("abc", -1, 3)]
    [InlineData("abc", 2, 2)]
    public static void CompareArgumentOutOfRangeIndexAndCount(string badString, int badOffset, int badCount)
    {
        CompareInfo ci = CultureInfo.InvariantCulture.CompareInfo;

        Assert.Throws<ArgumentOutOfRangeException>(() => ci.Compare("good", 0, 4, badString, badOffset, badCount));
        Assert.Throws<ArgumentOutOfRangeException>(() => ci.Compare(badString, badOffset, badCount, "good", 0, 4));

        Assert.Throws<ArgumentOutOfRangeException>(() => ci.Compare("good", 0, 4, badString, badOffset, badCount, CompareOptions.Ordinal));
        Assert.Throws<ArgumentOutOfRangeException>(() => ci.Compare(badString, badOffset, badCount, "good", 0, 4, CompareOptions.Ordinal));
    }

    [Fact]
    public static void CompareBadCompareOptions()
    {
        CompareInfo ci = CultureInfo.InvariantCulture.CompareInfo;

        Assert.Throws<ArgumentException>(() => ci.Compare("a", "b", CompareOptions.Ordinal | CompareOptions.IgnoreWidth));
        Assert.Throws<ArgumentException>(() => ci.Compare("a", "b", CompareOptions.OrdinalIgnoreCase | CompareOptions.IgnoreWidth));
        Assert.Throws<ArgumentException>(() => ci.Compare("a", "b", (CompareOptions)(-1)));
    }

    [Fact]
    public static void IndexOfMinusOneCompatability()
    {
        CompareInfo ci  = CultureInfo.InvariantCulture.CompareInfo;

        // This behavior was for .NET Framework 1.1 comparability.  We early outed for empty source strings
        // even with invalid offsets.

        Assert.Equal(0, ci.IndexOf("", "", -1, CompareOptions.None));
        Assert.Equal(-1, ci.IndexOf("", "a", -1, CompareOptions.None));
    }

    [Theory]
    [InlineData("", 'a', 1)]
    [InlineData("abc", 'a', -1)]
    [InlineData("abc", 'a', 4)]
    public static void IndexOfArgumentOutOfRangeIndex(string source, char value, int badStartIndex)
    {
        CompareInfo ci = CultureInfo.InvariantCulture.CompareInfo;
        string valueAsString = value.ToString();

        Assert.Throws<ArgumentOutOfRangeException>(() => ci.IndexOf(source, value, badStartIndex, CompareOptions.Ordinal));
        Assert.Throws<ArgumentOutOfRangeException>(() => ci.IndexOf(source, valueAsString, badStartIndex, CompareOptions.Ordinal));
    }

    [Theory]
    [InlineData("abc", 'a', 0, -1)]
    [InlineData("abc", 'a', 0, 4)]
    [InlineData("abc", 'a', 2, 2)]
    [InlineData("abc", 'a', 4, 0)]
    [InlineData("abc", 'a', 4, -1)]
    public static void IndexOfArgumentOutOfRangeIndexAndCount(string source, char value, int badStartIndex, int badCount)
    {
        CompareInfo ci = CultureInfo.InvariantCulture.CompareInfo;
        string valueAsString = value.ToString();

        Assert.Throws<ArgumentOutOfRangeException>(() => ci.IndexOf(source, value, badStartIndex, badCount));
        Assert.Throws<ArgumentOutOfRangeException>(() => ci.IndexOf(source, value, badStartIndex, badCount, CompareOptions.Ordinal));

        Assert.Throws<ArgumentOutOfRangeException>(() => ci.IndexOf(source, valueAsString, badStartIndex, badCount));
        Assert.Throws<ArgumentOutOfRangeException>(() => ci.IndexOf(source, valueAsString, badStartIndex, badCount, CompareOptions.Ordinal));
    }

    [Fact]
    public static void IndexOfArgumentNullException()
    {
        CompareInfo ci = CultureInfo.InvariantCulture.CompareInfo;

        Assert.Throws<ArgumentNullException>(() => ci.IndexOf(null, 'a'));
        Assert.Throws<ArgumentNullException>(() => ci.IndexOf(null, "a"));
        Assert.Throws<ArgumentNullException>(() => ci.IndexOf("a", null));

        Assert.Throws<ArgumentNullException>(() => ci.IndexOf(null, 'a', CompareOptions.Ordinal));
        Assert.Throws<ArgumentNullException>(() => ci.IndexOf(null, "a", CompareOptions.Ordinal));
        Assert.Throws<ArgumentNullException>(() => ci.IndexOf("a", null, CompareOptions.Ordinal));

        Assert.Throws<ArgumentNullException>(() => ci.IndexOf(null, 'a', 0, CompareOptions.Ordinal));
        Assert.Throws<ArgumentNullException>(() => ci.IndexOf(null, "a", 0, CompareOptions.Ordinal));
        Assert.Throws<ArgumentNullException>(() => ci.IndexOf("a", null, 0, CompareOptions.Ordinal));

        Assert.Throws<ArgumentNullException>(() => ci.IndexOf(null, 'a', 0, 1));
        Assert.Throws<ArgumentNullException>(() => ci.IndexOf(null, "a", 0, 1));
        Assert.Throws<ArgumentNullException>(() => ci.IndexOf("a", null, 0, 1));

        Assert.Throws<ArgumentNullException>(() => ci.IndexOf(null, 'a', 0, 1, CompareOptions.Ordinal));
        Assert.Throws<ArgumentNullException>(() => ci.IndexOf(null, "a", 0, 1, CompareOptions.Ordinal));
        Assert.Throws<ArgumentNullException>(() => ci.IndexOf("a", null, 0, 1, CompareOptions.Ordinal));
    }

    [Fact]
    public static void IndexOfBadCompareOptions()
    {
        CompareInfo ci = CultureInfo.InvariantCulture.CompareInfo;

        Assert.Throws<ArgumentException>(() => ci.IndexOf("abc", "a", CompareOptions.Ordinal | CompareOptions.IgnoreWidth));
        Assert.Throws<ArgumentException>(() => ci.IndexOf("abc", "a", CompareOptions.OrdinalIgnoreCase | CompareOptions.IgnoreWidth));
        Assert.Throws<ArgumentException>(() => ci.IndexOf("abc", "a", (CompareOptions)(-1)));
        Assert.Throws<ArgumentException>(() => ci.IndexOf("abc", "a", CompareOptions.StringSort));
    }
    
    [Theory]
    [InlineData("", 'a', 1)]
    [InlineData("abc", 'a', -1)]
    [InlineData("abc", 'a', 4)]
    public static void LastIndexOfArgumentOutOfRangeIndex(string source, char value, int badStartIndex)
    {
        CompareInfo ci = CultureInfo.InvariantCulture.CompareInfo;
        string valueAsString = value.ToString();

        Assert.Throws<ArgumentOutOfRangeException>(() => ci.LastIndexOf(source, value, badStartIndex, CompareOptions.Ordinal));
        Assert.Throws<ArgumentOutOfRangeException>(() => ci.LastIndexOf(source, valueAsString, badStartIndex, CompareOptions.Ordinal));
    }

    [Theory]
    [InlineData("abc", 'a', 2, -1)]
    [InlineData("abc", 'a', 2, 4)]
    [InlineData("abc", 'a', 1, 3)]
    [InlineData("abc", 'a', 4, 0)]
    [InlineData("abc", 'a', 4, -1)]
    public static void LastIndexOfArgumentOutOfRangeIndexAndCount(string source, char value, int badStartIndex, int badCount)
    {
        CompareInfo ci = CultureInfo.InvariantCulture.CompareInfo;
        string valueAsString = value.ToString();

        Assert.Throws<ArgumentOutOfRangeException>(() => ci.LastIndexOf(source, value, badStartIndex, badCount));
        Assert.Throws<ArgumentOutOfRangeException>(() => ci.LastIndexOf(source, value, badStartIndex, badCount, CompareOptions.Ordinal));

        Assert.Throws<ArgumentOutOfRangeException>(() => ci.LastIndexOf(source, valueAsString, badStartIndex, badCount));
        Assert.Throws<ArgumentOutOfRangeException>(() => ci.LastIndexOf(source, valueAsString, badStartIndex, badCount, CompareOptions.Ordinal));
    }

    [Fact]
    public static void LastIndexOfArgumentNullException()
    {
        CompareInfo ci = CultureInfo.InvariantCulture.CompareInfo;

        Assert.Throws<ArgumentNullException>(() => ci.LastIndexOf(null, 'a'));
        Assert.Throws<ArgumentNullException>(() => ci.LastIndexOf(null, "a"));
        Assert.Throws<ArgumentNullException>(() => ci.LastIndexOf("a", null));

        Assert.Throws<ArgumentNullException>(() => ci.LastIndexOf(null, 'a', CompareOptions.Ordinal));
        Assert.Throws<ArgumentNullException>(() => ci.LastIndexOf(null, "a", CompareOptions.Ordinal));
        Assert.Throws<ArgumentNullException>(() => ci.LastIndexOf("a", null, CompareOptions.Ordinal));

        Assert.Throws<ArgumentNullException>(() => ci.LastIndexOf(null, 'a', 1, CompareOptions.Ordinal));
        Assert.Throws<ArgumentNullException>(() => ci.LastIndexOf(null, "a", 1, CompareOptions.Ordinal));
        Assert.Throws<ArgumentNullException>(() => ci.LastIndexOf("a", null, 1, CompareOptions.Ordinal));

        Assert.Throws<ArgumentNullException>(() => ci.LastIndexOf(null, 'a', 1, 1));
        Assert.Throws<ArgumentNullException>(() => ci.LastIndexOf(null, "a", 1, 1));
        Assert.Throws<ArgumentNullException>(() => ci.LastIndexOf("a", null, 1, 1));

        Assert.Throws<ArgumentNullException>(() => ci.LastIndexOf(null, 'a', 1, 1, CompareOptions.Ordinal));
        Assert.Throws<ArgumentNullException>(() => ci.LastIndexOf(null, "a", 1, 1, CompareOptions.Ordinal));
        Assert.Throws<ArgumentNullException>(() => ci.LastIndexOf("a", null, 1, 1, CompareOptions.Ordinal));
    }

    [Fact]
    public static void LastIndexOfBadCompareOptions()
    {
        CompareInfo ci = CultureInfo.InvariantCulture.CompareInfo;

        Assert.Throws<ArgumentException>(() => ci.LastIndexOf("abc", "a", CompareOptions.Ordinal | CompareOptions.IgnoreWidth));
        Assert.Throws<ArgumentException>(() => ci.LastIndexOf("abc", "a", CompareOptions.OrdinalIgnoreCase | CompareOptions.IgnoreWidth));
        Assert.Throws<ArgumentException>(() => ci.LastIndexOf("abc", "a", (CompareOptions)(-1)));
        Assert.Throws<ArgumentException>(() => ci.LastIndexOf("abc", "a", CompareOptions.StringSort));
    }

    [Fact]
    public static void IsPrefixBadCompareOptions()
    {
        CompareInfo ci = CultureInfo.InvariantCulture.CompareInfo;

        Assert.Throws<ArgumentException>(() => ci.IsPrefix("aaa", "a", CompareOptions.Ordinal | CompareOptions.IgnoreWidth));
        Assert.Throws<ArgumentException>(() => ci.IsPrefix("aaa", "a", CompareOptions.OrdinalIgnoreCase | CompareOptions.IgnoreWidth));
        Assert.Throws<ArgumentException>(() => ci.IsPrefix("aaa", "a", CompareOptions.StringSort));
        Assert.Throws<ArgumentException>(() => ci.IsPrefix("aaa", "a", (CompareOptions)(-1)));
    }

    [Fact]
    public static void IsPrefixArgumentNullException()
    {
        CompareInfo ci = CultureInfo.InvariantCulture.CompareInfo;

        Assert.Throws<ArgumentNullException>(() => ci.IsPrefix(null, ""));
        Assert.Throws<ArgumentNullException>(() => ci.IsPrefix(null, "", CompareOptions.Ordinal));

        Assert.Throws<ArgumentNullException>(() => ci.IsPrefix("", null));
        Assert.Throws<ArgumentNullException>(() => ci.IsPrefix("", null, CompareOptions.Ordinal));

        Assert.Throws<ArgumentNullException>(() => ci.IsPrefix(null, null));
        Assert.Throws<ArgumentNullException>(() => ci.IsPrefix(null, null, CompareOptions.Ordinal));
    }
    
    [Fact]
    public static void IsSuffixBadCompareOptions()
    {
        CompareInfo ci = CultureInfo.InvariantCulture.CompareInfo;

        Assert.Throws<ArgumentException>(() => ci.IsSuffix("aaa", "a", CompareOptions.Ordinal | CompareOptions.IgnoreWidth));
        Assert.Throws<ArgumentException>(() => ci.IsSuffix("aaa", "a", CompareOptions.OrdinalIgnoreCase | CompareOptions.IgnoreWidth));
        Assert.Throws<ArgumentException>(() => ci.IsSuffix("aaa", "a", CompareOptions.StringSort));
        Assert.Throws<ArgumentException>(() => ci.IsSuffix("aaa", "a", (CompareOptions)(-1)));
    }

    [Fact]
    public static void IsSuffixArgumentNullException()
    {
        CompareInfo ci = CultureInfo.InvariantCulture.CompareInfo;

        Assert.Throws<ArgumentNullException>(() => ci.IsSuffix(null, ""));
        Assert.Throws<ArgumentNullException>(() => ci.IsSuffix(null, "", CompareOptions.Ordinal));

        Assert.Throws<ArgumentNullException>(() => ci.IsSuffix("", null));
        Assert.Throws<ArgumentNullException>(() => ci.IsSuffix("", null, CompareOptions.Ordinal));

        Assert.Throws<ArgumentNullException>(() => ci.IsSuffix(null, null));
        Assert.Throws<ArgumentNullException>(() => ci.IsSuffix(null, null, CompareOptions.Ordinal));
    }

    [Fact]
    public static void EqualsAndHashCode()
    {
        CompareInfo ciInvariantFromCultureInfo = CultureInfo.InvariantCulture.CompareInfo;
        CompareInfo ciInvariantFromFactory = CompareInfo.GetCompareInfo("");
        CompareInfo ciEnUs = CompareInfo.GetCompareInfo("en-US");

        Assert.True(ciInvariantFromCultureInfo.Equals(ciInvariantFromFactory));
        Assert.False(ciEnUs.Equals(ciInvariantFromCultureInfo));
        Assert.False(ciEnUs.Equals(new object()));

        Assert.Equal(ciInvariantFromCultureInfo.GetHashCode(), ciInvariantFromFactory.GetHashCode());
    }

    [Fact]
    [ActiveIssue(5463, PlatformID.AnyUnix)]
    public static void GetHashCodeOfString()
    {
        CompareInfo ci = CultureInfo.InvariantCulture.CompareInfo;

        Assert.Equal("abc".GetHashCode(), ci.GetHashCode("abc", CompareOptions.Ordinal));

        Assert.Equal(ci.GetHashCode("abc", CompareOptions.OrdinalIgnoreCase), ci.GetHashCode("ABC", CompareOptions.OrdinalIgnoreCase));

        // This behavior of the empty string is special-cased today.
        Assert.Equal(0, ci.GetHashCode("", CompareOptions.None));

        // Not much we can assert about the hashcode of a string itself, but we can assume that computing it twice yields the same value.
        int hashCode1 = ci.GetHashCode("abc", CompareOptions.None);
        int hashCode2 = ci.GetHashCode("abc", CompareOptions.None);

        Assert.Equal(hashCode1, hashCode2);
    }

    [Fact]
    public static void GetHashCodeOfStringNullSource()
    {
        CompareInfo ci = CultureInfo.InvariantCulture.CompareInfo;

        Assert.Throws<ArgumentNullException>(() => ci.GetHashCode(null, CompareOptions.None));
    }

    [Fact]
    public static void GetHashCodeOfStringBadCompareOptions()
    {
        CompareInfo ci = CultureInfo.InvariantCulture.CompareInfo;

        Assert.Throws<ArgumentException>(() => ci.GetHashCode("", CompareOptions.StringSort));
        Assert.Throws<ArgumentException>(() => ci.GetHashCode("", CompareOptions.Ordinal | CompareOptions.IgnoreSymbols));
        Assert.Throws<ArgumentException>(() => ci.GetHashCode("", (CompareOptions)(-1)));
    }

    [Fact]
    public static void ToStringReturnsNonNullNonEmpty()
    {
        CompareInfo ci = CultureInfo.InvariantCulture.CompareInfo;

        Assert.NotNull(ci.ToString());
        Assert.NotEqual("", ci.ToString());
    }
}
