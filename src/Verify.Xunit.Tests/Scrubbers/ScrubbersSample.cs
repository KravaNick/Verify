﻿#region ScrubbersSampleXunit

[UsesVerify]
public class ScrubbersSample
{
    [Fact]
    public Task Lines()
    {
        var settings = new VerifySettings();
        settings.ScrubLinesWithReplace(
            replaceLine: line =>
            {
                if (line.Contains("LineE"))
                {
                    return "NoMoreLineE";
                }

                return line;
            });
        settings.ScrubLines(removeLine: line => line.Contains("J"));
        settings.ScrubLinesContaining("b", "D");
        settings.ScrubLinesContaining(StringComparison.Ordinal, "H");
        return Verify(
            settings: settings,
            target: @"
                    LineA
                    LineB
                    LineC
                    LineD
                    LineE
                    LineH
                    LineI
                    LineJ
                    ");
    }

    [Fact]
    public Task EmptyLine()
    {
        var settings = new VerifySettings();
        settings.ScrubLinesWithReplace(
            replaceLine: _ => "");
        return Verify(
            settings: settings,
            target: @"");
    }

    [Fact]
    public Task LinesFluent() =>
        Verify(
                target: @"
                        LineA
                        LineB
                        LineC
                        LineD
                        LineE
                        LineH
                        LineI
                        LineJ
                        ")
            .ScrubLinesWithReplace(
                replaceLine: line =>
                {
                    if (line.Contains("LineE"))
                    {
                        return "NoMoreLineE";
                    }

                    return line;
                })
            .ScrubLines(removeLine: line => line.Contains("J"))
            .ScrubLinesContaining("b", "D")
            .ScrubLinesContaining(StringComparison.Ordinal, "H");

    [Fact]
    public Task AfterSerialization()
    {
        var target = new ToBeScrubbed
        {
            RowVersion = "7D3"
        };

        var settings = new VerifySettings();
        settings.AddScrubber(
            _ => _.Replace("7D3", "TheRowVersion"));
        return Verify(target, settings);
    }

    [Fact]
    public Task AfterSerializationFluent()
    {
        var target = new ToBeScrubbed
        {
            RowVersion = "7D3"
        };

        return Verify(target)
            .AddScrubber(_ => _.Replace("7D3", "TheRowVersion"));
    }

    [Fact]
    public Task RemoveOrReplace() =>
        Verify(
                target: @"
                        LineA
                        LineB
                        LineC
                        ")
            .ScrubLinesWithReplace(
                replaceLine: line =>
                {
                    if (line.Contains("LineB"))
                    {
                        return null;
                    }

                    return line.ToLower();
                });

    [Fact]
    public Task EmptyLines() =>
        Verify(
                target: @"
                        LineA

                        LineC
                        ")
            .ScrubEmptyLines();
}

#endregion