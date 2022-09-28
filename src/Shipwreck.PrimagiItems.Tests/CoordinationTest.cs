using Xunit.Abstractions;

namespace Shipwreck.PrimagiItems;

public class CoordinationTest
{
    private readonly ITestOutputHelper _Output;

    public CoordinationTest(ITestOutputHelper output)
    {
        _Output = output;
    }

    [Theory]
    [InlineData("JENNI loveコラボ")]
    [InlineData("ガッチャモールでゲット！　HIMAWARIちゃんねるおすすめコーデ＆無料たいけんチケット")]
    [InlineData("ちゃおフェス2022")]
    [InlineData("マイキャラルーム お知らせボード")]
    [InlineData("ワッチャプリマジ！Blu-rayBox1")]
    [InlineData("鈴木杏奈「Chasing the dream」アニメ盤初回特典")]
    [InlineData("鈴木杏奈「Magic×Color」アニメ盤初回特典")]
    public void ParseSpanTest(string span)
    {
        var c = new Coordination
        {
            Span = span
        };
        Assert.Null(c.SpanStart);
        Assert.Null(c.SpanEnd);
        Assert.Equal(span, c.SpanEventName);
        Assert.Null(c.SpanFriendPoint);
    }

    [Theory]
    [InlineData("2021年10月10日（日）〜2021年10月16日（土）", 2021, 10, 10, 2021, 10, 16)]
    [InlineData("2021年10月17日（日）〜2021年10月23日（土）", 2021, 10, 17, 2021, 10, 23)]
    [InlineData("2021年10月1日（金）〜2021年10月9日（土）", 2021, 10, 1, 2021, 10, 9)]
    [InlineData("2021年10月1日（金）～2021年12月1日（水） ", 2021, 10, 1, 2021, 12, 1)]
    [InlineData("2021年10月24日（日）〜2021年10月30日（土）", 2021, 10, 24, 2021, 10, 30)]
    [InlineData("2021年10月31日（日）〜2021年11月6日（土）", 2021, 10, 31, 2021, 11, 6)]
    [InlineData("2021年11月14日（日）〜2021年11月20日（土）", 2021, 11, 14, 2021, 11, 20)]
    [InlineData("2021年11月21日（日）〜2021年11月27日（土）", 2021, 11, 21, 2021, 11, 27)]
    [InlineData("2021年11月28日（日）〜2021年12月4日（土）", 2021, 11, 28, 2021, 12, 4)]
    [InlineData("2021年11月4日（木）〜2021年12年1日（水）", 2021, 11, 4, 2021, 12, 1)]
    [InlineData("2021年11月7日（日）〜2021年11月13日（土）", 2021, 11, 7, 2021, 11, 13)]
    [InlineData("2022年9月11日(日)～2022年9月17日(土) ", 2022, 9, 11, 2022, 9, 17)]
    [InlineData("2022年9月15日（木）～ 2022年10月5日（水）", 2022, 9, 15, 2022, 10, 5)]
    [InlineData("2022年9月15日（木）～ 2022年11月1日（火）", 2022, 9, 15, 2022, 11, 1)]
    [InlineData("2022年9月18日(日)～2022年9月24日(土) ", 2022, 9, 18, 2022, 9, 24)]
    [InlineData("2022年9月25日(日)～2022年10月1日(土) ", 2022, 9, 25, 2022, 10, 1)]
    [InlineData("2022年9月29日(木) 〜2022年11月1日（火）", 2022, 9, 29, 2022, 11, 1)]
    [InlineData("2022年9月29日（木）～2022年11月1日（火）", 2022, 9, 29, 2022, 11, 1)]
    [InlineData("2022年9月4日(日)～2022年9月10日(土) ", 2022, 9, 4, 2022, 9, 10)]
    [InlineData("2022年7月10日（日）〜2022年7月16日（土）", 2022, 7, 10, 2022, 7, 16)]
    [InlineData("2022年7月17日（日）〜2022年7月23日（土）", 2022, 7, 17, 2022, 7, 23)]
    [InlineData("2022年7月24日（日）〜2022年7月30日（土）", 2022, 7, 24, 2022, 7, 30)]
    [InlineData("2022年7月31日（日）〜2022年8月3日（水）", 2022, 7, 31, 2022, 8, 3)]
    [InlineData("2022年7月3日（日）〜2022年7月9日（土）", 2022, 7, 3, 2022, 7, 9)]
    [InlineData("2022年7月7日（木）〜2022年8月3日（水）", 2022, 7, 7, 2022, 8, 3)]
    [InlineData("2022年8月14日(日)～2022年8月20日(土) ", 2022, 8, 14, 2022, 8, 20)]
    [InlineData("2022年8月21日(日)～2022年8月27日(土)", 2022, 8, 21, 2022, 8, 27)]
    [InlineData("2022年8月25日（木）～ 2022年9月14日（水）", 2022, 8, 25, 2022, 9, 14)]
    [InlineData("2022年8月28日(日)～2022年9月3日(土) ", 2022, 8, 28, 2022, 9, 3)]
    [InlineData("2022年8月4日(木) 〜 2022年9月14日(水)", 2022, 8, 4, 2022, 9, 14)]
    [InlineData("2022年8月4日(木) 〜 2022年9月7日(水)", 2022, 8, 4, 2022, 9, 7)]
    [InlineData("2022年8月4日（木）～ 2022年8月24日（水）", 2022, 8, 4, 2022, 8, 24)]
    [InlineData("2022年8月4日（木）～ 2022年9月14日（水）", 2022, 8, 4, 2022, 9, 14)]
    [InlineData("2022年8月4日(木)～2022年8月31日(水) ", 2022, 8, 4, 2022, 8, 31)]
    [InlineData("2022年8月4日(木)～2022年8月6日(土) ", 2022, 8, 4, 2022, 8, 6)]
    [InlineData("2022年8月7日(日)～2022年8月13日(土) ", 2022, 8, 7, 2022, 8, 13)]
    [InlineData("2021年12月12日（日） 〜2021年12月18日（土）", 2021, 12, 12, 2021, 12, 18)]
    [InlineData("2021年12月19日（日）〜2021年12月25日（土）", 2021, 12, 19, 2021, 12, 25)]
    [InlineData("2021年12月19日（日）〜2022年1月19日（水）", 2021, 12, 19, 2022, 1, 19)]
    [InlineData("2021年12月26日（日）〜2022年1月1日（土）", 2021, 12, 26, 2022, 1, 1)]
    [InlineData("2021年12月2日（木）〜2021年12月29日（水）", 2021, 12, 2, 2021, 12, 29)]
    [InlineData("2021年12月2日（木）〜2022年2月2日（水）", 2021, 12, 2, 2022, 2, 2)]
    [InlineData("2021年12月5日（日）〜2021年12月11日（土）", 2021, 12, 5, 2021, 12, 11)]
    [InlineData("2022年10月2日(日)～2022年10月8日(土) ", 2022, 10, 2, 2022, 10, 8)]
    [InlineData("2022年10月6日（木）～ 2022年11月1日（火）", 2022, 10, 6, 2022, 11, 1)]
    [InlineData("2022年1月16日（日）〜2022年1月22日（土）", 2022, 1, 16, 2022, 1, 22)]
    [InlineData("2022年1月23日（日）〜2022年1月29日（土）", 2022, 1, 23, 2022, 1, 29)]
    [InlineData("2022年1月2日（日）〜2022年1月8日（土）", 2022, 1, 2, 2022, 1, 8)]
    [InlineData("2022年1月30日（日）〜2022年2月5日（土）", 2022, 1, 30, 2022, 2, 5)]
    [InlineData("2022年1月6日（木）〜2022年2月2日（水）", 2022, 1, 6, 2022, 2, 2)]
    [InlineData("2022年1月9日（日）〜2022年1月15日（土）", 2022, 1, 9, 2022, 1, 15)]
    [InlineData("2022年2月13日（日）〜2022年2月19日（土）", 2022, 2, 13, 2022, 2, 19)]
    [InlineData("2022年2月20日（日）〜2022年2月26日（土）", 2022, 2, 20, 2022, 2, 26)]
    [InlineData("2022年2月27日（日）〜2022年3月5日（土）", 2022, 2, 27, 2022, 3, 5)]
    [InlineData("2022年2月3日（木）〜2022年3月2日（水）", 2022, 2, 3, 2022, 3, 2)]
    [InlineData("2022年2月3日（木）〜2022年3月30日（水）", 2022, 2, 3, 2022, 3, 30)]
    [InlineData("2022年2月6日（日）〜2022年2月12日（土）", 2022, 2, 6, 2022, 2, 12)]
    [InlineData("2022年3月13日（日）〜2022年3月19日（土）", 2022, 3, 13, 2022, 3, 19)]
    [InlineData("2022年3月20日（日）〜2022年3月26日（土）", 2022, 3, 20, 2022, 3, 26)]
    [InlineData("2022年3月27日（日）〜2022年3月30日（水）", 2022, 3, 27, 2022, 3, 30)]
    [InlineData("2022年3月31日（木）〜2022年4月27日（水）", 2022, 3, 31, 2022, 4, 27)]
    [InlineData("2022年3月31日（木）〜2022年4月2日（土）", 2022, 3, 31, 2022, 4, 2)]
    [InlineData("2022年3月31日（木）〜2022年6月1日（水）", 2022, 3, 31, 2022, 6, 1)]
    [InlineData("2022年3月3日（木）〜2022年3月30日（水）", 2022, 3, 3, 2022, 3, 30)]
    [InlineData("2022年3月6日（日）〜2022年3月12日（土）", 2022, 3, 6, 2022, 3, 12)]
    [InlineData("2022年4月10日（日）～2022年4月16日（土）", 2022, 4, 10, 2022, 4, 16)]
    [InlineData("2022年4月17日（日）〜2022年4月23日（土）", 2022, 4, 17, 2022, 4, 23)]
    [InlineData("2022年4月24日（日）〜2022年4月30日（土）", 2022, 4, 24, 2022, 4, 30)]
    [InlineData("2022年4月28日（木）〜2022年6月1日（水）", 2022, 4, 28, 2022, 6, 1)]
    [InlineData("2022年4月3日（日）〜2022年4月9日（土）", 2022, 4, 3, 2022, 4, 9)]
    [InlineData("2022年5月15日（日）〜2022年5月21日（土）", 2022, 5, 15, 2022, 5, 21)]
    [InlineData("2022年5月1日（日）〜2022年5月7日（土）", 2022, 5, 1, 2022, 5, 7)]
    [InlineData("2022年5月22日（日）〜2022年5月28日（土）", 2022, 5, 22, 2022, 5, 28)]
    [InlineData("2022年5月29日（日）〜2022年6月1日（水）", 2022, 5, 29, 2022, 6, 1)]
    [InlineData("2022年5月8日（日）〜2022年5月14日（土）", 2022, 5, 8, 2022, 5, 14)]
    [InlineData("2022年6月12日（日）〜2022年6月18日（土）", 2022, 6, 12, 2022, 6, 18)]
    [InlineData("2022年6月19日（日）〜2022年6月25日（土）", 2022, 6, 19, 2022, 6, 25)]
    [InlineData("2022年6月26日（日）〜2022年7月2日（土）", 2022, 6, 26, 2022, 7, 2)]
    [InlineData("2022年6月2日（木）〜2022年6月4日（土）", 2022, 6, 2, 2022, 6, 4)]
    [InlineData("2022年6月2日（木）〜2022年7月6日（水）", 2022, 6, 2, 2022, 7, 6)]
    [InlineData("2022年6月2日（木）〜2022年8月3日（土）", 2022, 6, 2, 2022, 8, 3)]
    [InlineData("2022年6月5日（日）〜2022年6月11日（土）", 2022, 6, 5, 2022, 6, 11)]
    [InlineData("2022年2月　マイキャラルームガッチャ（2月3日～2月28日）", 2022, 2, 3, 2022, 2, 28, "マイキャラルームガッチャ")]
    [InlineData("2022年3月　マイキャラルームガッチャ（3月1日～3月30日）", 2022, 3, 1, 2022, 3, 30, "マイキャラルームガッチャ")]
    [InlineData("2022年10月 マイキャラルームガッチャ 10月1日（土）〜11月1日（火）", 2022, 10, 1, 2022, 11, 1, "マイキャラルームガッチャ")]
    [InlineData("2022年4月　マイキャラルームガッチャ 3月31日（木）～4月30日（土）", 2022, 3, 31, 2022, 4, 30, "マイキャラルームガッチャ")]
    [InlineData("2022年5月　マイキャラルームガッチャ 5月1日（日）～6月1日（水）", 2022, 5, 1, 2022, 6, 1, "マイキャラルームガッチャ")]
    [InlineData("2022年6月　マイキャラルームガッチャ　6月2日（木）～6月30日（木）", 2022, 6, 2, 2022, 6, 30, "マイキャラルームガッチャ")]
    [InlineData("2022年7月　ウエルシアコラボ　2022年7月7日（木）～8月31日（水）", 2022, 7, 7, 2022, 8, 31, "ウエルシアコラボ")]
    [InlineData("2022年7月　マイキャラルームガッチャ　7月1日（金）～8月3日（水）", 2022, 7, 1, 2022, 8, 3, "マイキャラルームガッチャ")]
    [InlineData("2022年8月 マイキャラルームガッチャ 8月4日（木）～8月31日（水）", 2022, 8, 4, 2022, 8, 31, "マイキャラルームガッチャ")]
    [InlineData("2022年9月 マイキャラルームガッチャ 9月1日（木）～9月30日（金）", 2022, 9, 1, 2022, 9, 30, "マイキャラルームガッチャ")]
    [InlineData("フォーチュンスターオレンジチャンス 2022年6月2日（木）〜2022年7月6日（水）", 2022, 6, 2, 2022, 7, 6, "フォーチュンスターオレンジチャンス")]
    public void ParseSpanTest_Date(string span, int sy, int sm, int sd, int ey, int em, int ed, string? eventName = null)
    {
        var c = new Coordination
        {
            Span = span
        };

        Assert.Equal(sy, c.SpanStart?.Year);
        Assert.Equal(sm, c.SpanStart?.Month);
        Assert.Equal(sd, c.SpanStart?.Day);
        Assert.Equal(ey, c.SpanEnd?.Year);
        Assert.Equal(em, c.SpanEnd?.Month);
        Assert.Equal(ed, c.SpanEnd?.Day);
        Assert.Equal(eventName, c.SpanEventName);
        Assert.Null(c.SpanFriendPoint);
    }

    [Theory]
    [InlineData("2022年2月3日（木）〜3月30日（水）", 2022, 2, 3, 3, 30)]
    public void ParseSpanTest_Date2(string span, int sy, int sm, int sd, int em, int ed)
    {
        var c = new Coordination
        {
            Span = span
        };

        Assert.Equal(sy, c.SpanStart?.Year);
        Assert.Equal(sm, c.SpanStart?.Month);
        Assert.Equal(sd, c.SpanStart?.Day);
        Assert.Equal(sy, c.SpanEnd?.Year);
        Assert.Equal(em, c.SpanEnd?.Month);
        Assert.Equal(ed, c.SpanEnd?.Day);

        Assert.Null(c.SpanEventName);
        Assert.Null(c.SpanFriendPoint);
    }

    [Theory]
    [InlineData("2022年3月31日（木）〜2022年6月1日（水）第4章フレンドポイント　1200ポイント", 1200)]
    [InlineData("2022年3月31日（木）〜2022年6月1日（水）第4章フレンドポイント　2500ポイント", 2500)]
    [InlineData("2022年3月31日（木）〜2022年6月1日（水）第4章フレンドポイント　4000ポイント", 4000)]
    [InlineData("第5章フレンドポイント　1200ポイント", 1200)]
    [InlineData("第5章フレンドポイント　2500ポイント", 2500)]
    [InlineData("第5章フレンドポイント　4000ポイント", 4000)]
    [InlineData("第6章フレンドポイント　 4000ポイント", 4000)]
    [InlineData("第6章フレンドポイント　1200ポイント", 1200)]
    [InlineData("第6章フレンドポイント　2500ポイント", 2500)]
    public void ParseSpanTest_FriendPoint(string span, int p)
    {
        var c = new Coordination
        {
            Span = span
        };
        Assert.Null(c.SpanStart);
        Assert.Null(c.SpanEnd);
        Assert.Null(c.SpanEventName);
        Assert.Equal(p, c.SpanFriendPoint);
    }

    [Theory]
    [InlineData("2021年10月　ちゃお11月号ふろく", 2021, 10, "ちゃお11月号ふろく")]
    [InlineData("2021年10月　ぷっちぐみ11月号ふろく", 2021, 10, "ぷっちぐみ11月号ふろく")]
    [InlineData("2021年10月　プリマジコーデカード♪コレクショングミ vol.1", 2021, 10, "プリマジコーデカード♪コレクショングミ vol.1")]
    [InlineData("2021年10月　マイキャラルームガチャ（10月）", 2021, 10, "マイキャラルームガチャ（10月）")]
    [InlineData("2021年10月　マイキャラルームガチャ（11月）", 2021, 10, "マイキャラルームガチャ（11月）")]
    [InlineData("2021年10月　マクドナルドハッピーセット", 2021, 10, "マクドナルドハッピーセット")]
    [InlineData("2021年10月　まほうつかいみゃむとHIMAWARIちゃんねる まーちゃん・おーちゃんからのしょうたいじょうキャンペーン！など", 2021, 10, "まほうつかいみゃむとHIMAWARIちゃんねる まーちゃん・おーちゃんからのしょうたいじょうキャンペーン！など")]
    [InlineData("2021年10月　公式ファンブック　第1章", 2021, 10, "公式ファンブック　第1章")]
    [InlineData("2021年10月　無料体験チケット", 2021, 10, "無料体験チケット")]
    [InlineData("2021年11月　ちゃお・ぷっちぐみ12月号　スペシャルプリマジ", 2021, 11, "ちゃお・ぷっちぐみ12月号　スペシャルプリマジ")]
    [InlineData("2021年12月　ちゃお1月号ふろく", 2021, 12, "ちゃお1月号ふろく")]
    [InlineData("2021年12月　マイキャラルームガッチャ（12月）", 2021, 12, "マイキャラルームガッチャ（12月）")]
    [InlineData("2021年12月　公式ファンブック　第2章", 2021, 12, "公式ファンブック　第2章")]
    [InlineData("2021年12月　鈴木杏奈「Dreaming Sound」CDアニメ盤（初回生産分）", 2021, 12, "鈴木杏奈「Dreaming Sound」CDアニメ盤（初回生産分）")]
    [InlineData("2022年1月　ちゃお・ぷっちぐみ2月号　スペシャルプリマジ", 2022, 1, "ちゃお・ぷっちぐみ2月号　スペシャルプリマジ")]
    [InlineData("2022年1月　マイキャラルームガッチャ（1月）", 2022, 1, "マイキャラルームガッチャ（1月）")]
    [InlineData("2022年2月　プリマジコーデカード♪コレクショングミ vol.2", 2022, 2, "プリマジコーデカード♪コレクショングミ vol.2")]
    [InlineData("2022年2月　公式ファンブック　第3章", 2022, 2, "公式ファンブック　第3章")]
    [InlineData("2022年3月　ちゃお・ぷっちぐみ4月号　スペシャルプリマジ", 2022, 3, "ちゃお・ぷっちぐみ4月号　スペシャルプリマジ")]
    [InlineData("2022年3月　ディッパーダンコラボ", 2022, 3, "ディッパーダンコラボ")]
    [InlineData("2022年3月　ナムコ　ゲットチャンス", 2022, 3, "ナムコ　ゲットチャンス")]
    [InlineData("2022年3月　モーリーファンタジー・PALO　ゲットキャンペーン", 2022, 3, "モーリーファンタジー・PALO　ゲットキャンペーン")]
    [InlineData("2022年4月　ガチャ　ワッチャプリマジ！ミニカードチャーム", 2022, 4, "ガチャ　ワッチャプリマジ！ミニカードチャーム")]
    [InlineData("2022年4月　ちゃお5月号ふろく", 2022, 4, "ちゃお5月号ふろく")]
    [InlineData("2022年4月　ナムコ　ゲットチャンス", 2022, 4, "ナムコ　ゲットチャンス")]
    [InlineData("2022年4月　モーリーファンタジー・PALO　ゲットキャンペーン", 2022, 4, "モーリーファンタジー・PALO　ゲットキャンペーン")]
    [InlineData("2022年5月　ちゃお・ぷっちぐみ6月号　スペシャルプリマジ", 2022, 5, "ちゃお・ぷっちぐみ6月号　スペシャルプリマジ")]
    [InlineData("2022年6月　ワッチャプリマジ！コーデカード♪コレクショングミ vol.3", 2022, 6, "ワッチャプリマジ！コーデカード♪コレクショングミ vol.3")]
    [InlineData("2022年7月　ちゃお・ぷっちぐみ8月号　スペシャルプリマジ", 2022, 7, "ちゃお・ぷっちぐみ8月号　スペシャルプリマジ")]
    [InlineData("2022年7月　ナムコ　ゲットチャンス", 2022, 7, "ナムコ　ゲットチャンス")]
    [InlineData("2022年7月　モーリーファンタジー・PALO　ゲットキャンペーン", 2022, 7, "モーリーファンタジー・PALO　ゲットキャンペーン")]
    [InlineData("2022年8月　ディッパーダンコラボ第2弾", 2022, 8, "ディッパーダンコラボ第2弾")]
    [InlineData("2022年9月 ナムコ ゲットチャンス", 2022, 9, "ナムコ ゲットチャンス")]
    [InlineData("2022年9月 モーリーファンタジー・PALO ゲットキャンペーン", 2022, 9, "モーリーファンタジー・PALO ゲットキャンペーン")]
    public void ParseSpanTest_Month(string span, int sy, int sm, string? eventName)
    {
        var c = new Coordination
        {
            Span = span
        };

        Assert.Equal(sy, c.SpanStart?.Year);
        Assert.Equal(sm, c.SpanStart?.Month);
        Assert.Equal(1, c.SpanStart?.Day);
        Assert.Equal(sy, c.SpanEnd?.Year);
        Assert.Equal(sm, c.SpanEnd?.Month);
        Assert.NotEqual(sm, c.SpanEnd?.AddDays(1).Month);

        Assert.Equal(eventName, c.SpanEventName);
        Assert.Null(c.SpanFriendPoint);
    }

    [Theory]
    [InlineData("2022年3月31日（木）発売　公式ファンブック　第4章", 2022, 3, 31, "公式ファンブック　第4章")]
    [InlineData("2022年6月2日（木）発売 公式ファンブック 第5章", 2022, 6, 2, "公式ファンブック 第5章")]
    [InlineData("2022年8月3日発売　ちゃお9月号ふろく", 2022, 8, 3, "ちゃお9月号ふろく")]
    [InlineData("2022年8月4日発売　公式ファンブック第6章", 2022, 8, 4, "公式ファンブック第6章")]
    [InlineData("2022年9月1日発売　ちゃお・ぷっちぐみ10月号　スペシャルプリマジ", 2022, 9, 1, "ちゃお・ぷっちぐみ10月号　スペシャルプリマジ")]
    [InlineData("2022年1月1日（土）～", 2022, 1, 1, null)]
    [InlineData("2022年3月～", 2022, 3, 1, null)]
    public void ParseSpanTest_StartDate(string span, int sy, int sm, int sd, string? eventName)
    {
        var c = new Coordination
        {
            Span = span
        };

        Assert.Equal(sy, c.SpanStart?.Year);
        Assert.Equal(sm, c.SpanStart?.Month);
        Assert.Equal(sd, c.SpanStart?.Day);
        Assert.Null(c.SpanEnd);

        Assert.Equal(eventName, c.SpanEventName);
        Assert.Null(c.SpanFriendPoint);
    }


    //[Fact]
    //public void Test1()
    //{
    //    var s = @"C:\Users\kakigahara\source\repos\pgrho\primagi-items\output\items.json";
    //    using var sr = new StreamReader(s);
    //    var ds = PrimagiDataSet.Parse(sr);

    //    foreach (var span in ds.Coordinations.Select(e => e.Span).Distinct().OrderBy(e => e))
    //    {
    //        _Output.WriteLine(span);
    //    }
    //}
}