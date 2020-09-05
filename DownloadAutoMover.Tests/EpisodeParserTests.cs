﻿using DownloadAutoMover.Classes;
using System.Linq;
using Xunit;

namespace DownloadAutoMover.Tests
{
    public class EpisodeParserTests
    {
        public EpisodeParser episodeParser;
        private readonly string[] ShowNames = {
            "[Judas] Mugen no Juunin (Blade of the Immortal) - 15 [1080p][HEVC x265 10bit][Eng-Subs].mkv",
            "Americas.Got.Talent.S14E24.WEB.h264-TBS[rarbg].mp4",
            "Dracula.2020.S01E01.HDTV.x264-PHOENiX[TGx].mkv",
            "outlander.S04E10.web.h264-memento.mkv",
            "This.Is.Us.S04E08.HDTV.x264-SVA.mkv",
            "SpongeBob.SquarePants.S12E21.WEBRip.x264-ION10.mp4",
            "Teen.Titans.Go.S06E09.WEBRip.x264-ION10.mp4",
            "kims.convenience.S04E02.webrip.x264-cookiemonster.mkv",
            "Real.Time.With.Bill.Maher.2020.01.17.HDTV.x264-aAF.mkv",
            "shameless.us.s10e11.web.h264-tbs.mkv",
            "kims.convenience.s04e03.webrip.x264-cookiemonster.mkv",
            "saturday.night.live.s45e11.adam.driver.web.x264-trump.mkv",
            "NCIS.S17E12.HDTV.x264-SVA.mkv",
            "power.2014.s06e13.web.h264-xlf.mkv",
            "This.Is.Us.S04E08.HDTV.x264-SVA.mkv",
            "Hotel.Transylvania.The.Series.S02E07.HDTV.x264-W4F[rarbg].mp4"
        };

        public EpisodeParserTests() 
        {
            episodeParser = new EpisodeParser();
        }

        [Fact]
        public void GetCodec_Tests()
        {
            foreach (string ShowName in ShowNames)
            {
                Assert.NotEmpty(episodeParser.GetCodec(ShowName));
            }
        }

        [Fact]
        public void GetGroup_Tests()
        {
            foreach (string ShowName in ShowNames)
            {
                Assert.NotEmpty(episodeParser.GetGroup(ShowName));
            }
        }

        [Fact]
        public void GetQuality_Tests()
        {
            foreach (string ShowName in ShowNames)
            {
                Assert.NotEmpty(episodeParser.GetQuality(ShowName));
            }
        }

        [Fact]
        public void GetTitle_Tests()
        {
            foreach (string ShowName in ShowNames)
            {
                Assert.NotEmpty(episodeParser.GetTitle(ShowName));
            }
        }

        [Fact]
        public void GetSeasonEpisode_Tests()
        {
            foreach (string ShowName in ShowNames)
            {
                Assert.NotNull(episodeParser.GetSeasonEpisode(ShowName));
            }
        }

        [Fact]
        public void GetTvEpisode_Tests()
        {
            foreach (string ShowName in ShowNames)
            {
                Assert.NotNull(episodeParser.GetTvEpisode(ShowName));
            }
        }
    }
}