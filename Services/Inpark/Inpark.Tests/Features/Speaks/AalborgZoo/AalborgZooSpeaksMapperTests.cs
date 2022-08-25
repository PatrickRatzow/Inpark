using Microsoft.Extensions.Logging;
using Moq;
using Zeta.Inpark.Features.Speaks.AalborgZoo;

namespace Zoo.Inpark.Tests.Features.Speaks.AalborgZoo;

[TestFixture]
[Category(Categories.Unit)]
public class AalborgZooSpeaksMapperTests
{
    private Mock<ILogger<AalborgZooSpeaksMapper>> _loggerMock = null!;
    private AalborgZooSpeaksMapper _mapper = null!;
    
    [SetUp]
    public void Setup()
    {
        _loggerMock = new();
        _mapper = new(_loggerMock.Object);
    }
    
    [Test]
    public void Parse_ShouldReturn_ListOfSpeaks()
    {
        // Arrange
        const string input =
            "{\"items\":[{\"item\":{\"title\":\"Pingvin\",\"description\":null,\"url\":\"/aktiviteter/pingvin\",\"content\":[],\"properties\":{\"image\":{\"umbracoBytes\":224760,\"umbracoExtension\":\"jpg\",\"umbracoHeight\":350,\"umbracoWidth\":700,\"umbracoFile\":{\"src\":\"/media/ydefk2ty/pingvin-700x350.jpg\",\"name\":\"Pingvin\"}},\"times\":[{\"allDay\":false,\"weekDays\":[\"Monday\",\"Tuesday\",\"Wednesday\",\"Thursday\",\"Friday\",\"Saturday\",\"Sunday\"],\"start\":\"2020-10-19T11:10:00\",\"header\":\"Pingvin hele året\",\"end\":\"2022-10-23T00:00:00\",\"type\":\"Activity\"}],\"header\":\"Pingvin\",\"noIndex\":false,\"imageFullscreen\":{\"umbracoBytes\":1099583,\"umbracoExtension\":\"jpg\",\"umbracoHeight\":900,\"umbracoWidth\":1920,\"umbracoFile\":{\"src\":\"/media/u34hhd1m/pingvin-1920x900.jpg\",\"name\":\"Pingvin\"}}}},\"score\":0},{\"item\":{\"title\":\"Søløve\",\"description\":null,\"url\":\"/aktiviteter/solove\",\"content\":[],\"properties\":{\"image\":{\"umbracoBytes\":233679,\"umbracoExtension\":\"jpg\",\"umbracoHeight\":350,\"umbracoWidth\":700,\"umbracoFile\":{\"src\":\"/media/csip2abz/søløve-700x350.jpg\",\"name\":\"Søløve\"}},\"times\":[{\"allDay\":false,\"weekDays\":[\"Wednesday\",\"Monday\",\"Tuesday\",\"Thursday\",\"Friday\",\"Saturday\",\"Sunday\"],\"start\":\"2020-10-19T11:30:00\",\"header\":\"Søløve alle dage\",\"end\":\"2022-10-23T00:00:00\",\"type\":\"Activity\"},{\"allDay\":false,\"weekDays\":[\"Saturday\",\"Sunday\"],\"start\":\"2021-10-25T14:45:00\",\"header\":\"Søløve vinter weekend 2\",\"end\":\"2022-04-08T00:00:00\",\"type\":\"Activity\"}],\"header\":\"Søløve\",\"noIndex\":false,\"imageFullscreen\":{\"umbracoBytes\":983572,\"umbracoExtension\":\"jpg\",\"umbracoHeight\":900,\"umbracoWidth\":1920,\"umbracoFile\":{\"src\":\"/media/l15bj5bm/søløve-1920x900.jpg\",\"name\":\"Søløve\"}}}},\"score\":0},{\"item\":{\"title\":\"Næsebjørn\",\"description\":null,\"url\":\"/aktiviteter/naesebjorn\",\"content\":[],\"properties\":{\"image\":{\"umbracoBytes\":232495,\"umbracoExtension\":\"jpg\",\"umbracoHeight\":350,\"umbracoWidth\":700,\"umbracoFile\":{\"src\":\"/media/mnrbhgzy/næsebjørn-700x350.jpg\",\"name\":\"Næsebjørn\"}},\"times\":[{\"allDay\":false,\"weekDays\":[\"Monday\",\"Tuesday\",\"Wednesday\",\"Thursday\",\"Friday\",\"Saturday\",\"Sunday\"],\"start\":\"2020-10-19T14:00:00\",\"header\":\"Næsebjørn hele året\",\"end\":\"2022-04-08T00:00:00\",\"type\":\"Activity\"},{\"allDay\":false,\"weekDays\":[\"Monday\",\"Tuesday\",\"Wednesday\",\"Thursday\",\"Friday\",\"Saturday\",\"Sunday\"],\"start\":\"2022-04-09T14:30:00\",\"header\":\"Næsebjørn sommer 22\",\"end\":\"2022-10-23T00:00:00\",\"type\":\"Activity\"}],\"header\":\"Næsebjørn\",\"noIndex\":false,\"imageFullscreen\":{\"umbracoBytes\":1129758,\"umbracoExtension\":\"jpg\",\"umbracoHeight\":900,\"umbracoWidth\":1920,\"umbracoFile\":{\"src\":\"/media/5vknrbv2/næsebjørn-1920x900.jpg\",\"name\":\"Næsebjørn\"}}}},\"score\":0},{\"item\":{\"title\":\"Vortesvin\",\"description\":null,\"url\":\"/aktiviteter/vortesvin\",\"content\":[],\"properties\":{\"image\":{\"umbracoBytes\":302881,\"umbracoExtension\":\"jpg\",\"umbracoHeight\":350,\"umbracoWidth\":700,\"umbracoFile\":{\"src\":\"/media/jgpmuafu/vortesvin-700x350.jpg\",\"name\":\"Vortesvin\"}},\"times\":[{\"allDay\":false,\"weekDays\":[\"Friday\",\"Monday\",\"Tuesday\",\"Wednesday\",\"Thursday\",\"Saturday\",\"Sunday\"],\"start\":\"2021-03-27T11:00:00\",\"header\":\"Vortesvin sommer 22\",\"end\":\"2022-10-23T00:00:00\",\"type\":\"Activity\"}],\"header\":\"Vortesvin\",\"noIndex\":false,\"imageFullscreen\":{\"umbracoBytes\":1619982,\"umbracoExtension\":\"jpg\",\"umbracoHeight\":900,\"umbracoWidth\":1920,\"umbracoFile\":{\"src\":\"/media/yvfp3tm4/vortesvin-1920x900.jpg\",\"name\":\"Vortesvin\"}}}},\"score\":0},{\"item\":{\"title\":\"Rovdyr\",\"description\":null,\"url\":\"/aktiviteter/rovdyr\",\"content\":[],\"properties\":{\"image\":{\"umbracoBytes\":299708,\"umbracoExtension\":\"jpg\",\"umbracoHeight\":350,\"umbracoWidth\":700,\"umbracoFile\":{\"src\":\"/media/3qfbyero/tiger700x350.jpg\",\"name\":\"Rovdyr\"}},\"times\":[{\"allDay\":false,\"weekDays\":[\"Saturday\",\"Sunday\",\"Tuesday\",\"Wednesday\"],\"start\":\"2021-10-25T13:50:00\",\"header\":\"Rovdyr vinter 2\",\"end\":\"2022-04-08T00:00:00\",\"type\":\"Activity\"},{\"allDay\":false,\"weekDays\":[\"Tuesday\",\"Wednesday\",\"Saturday\",\"Sunday\"],\"start\":\"2022-04-09T13:00:00\",\"header\":\"Rovdyr sommer 22\",\"end\":\"2022-10-23T00:00:00\",\"type\":\"Activity\"}],\"header\":\"Rovdyr\",\"noIndex\":false,\"imageFullscreen\":{\"umbracoBytes\":1312948,\"umbracoExtension\":\"jpg\",\"umbracoHeight\":900,\"umbracoWidth\":1920,\"umbracoFile\":{\"src\":\"/media/eyklqiyi/tiger-1920x900.jpg\",\"name\":\"Rovdyr\"}}}},\"score\":0},{\"item\":{\"title\":\"Safari Simon\",\"description\":null,\"url\":\"/aktiviteter/safari-simon\",\"content\":[],\"properties\":{\"image\":{\"umbracoBytes\":247192,\"umbracoExtension\":\"jpg\",\"umbracoHeight\":350,\"umbracoWidth\":700,\"umbracoFile\":{\"src\":\"/media/shpdfkv2/safari-simon-700x350.jpg\",\"name\":\"Safari Simon\"}},\"times\":[{\"allDay\":false,\"weekDays\":[\"Saturday\",\"Sunday\"],\"start\":\"2022-04-09T12:00:00\",\"header\":\"Safari Simon weekend 22\",\"end\":\"2022-10-23T00:00:00\",\"type\":\"Activity\"},{\"allDay\":false,\"weekDays\":[\"Monday\",\"Tuesday\",\"Wednesday\",\"Thursday\",\"Friday\"],\"start\":\"2022-06-27T12:00:00\",\"header\":\"Safari Simon Sommerferie 22\",\"end\":\"2022-08-05T00:00:00\",\"type\":\"Activity\"},{\"allDay\":false,\"weekDays\":[\"Monday\",\"Tuesday\",\"Wednesday\",\"Thursday\",\"Friday\",\"Saturday\",\"Sunday\"],\"start\":\"2022-06-25T15:00:00\",\"header\":\"Safari Simon sommerferie #2 22\",\"end\":\"2022-08-07T00:00:00\",\"type\":\"Activity\"},{\"allDay\":false,\"weekDays\":[\"Monday\",\"Tuesday\",\"Wednesday\",\"Thursday\",\"Friday\"],\"start\":\"2022-04-11T12:00:00\",\"header\":\"Safari Simon påske 22\",\"end\":\"2022-04-18T00:00:00\",\"type\":\"Activity\"},{\"allDay\":false,\"weekDays\":[\"Friday\"],\"start\":\"2022-05-13T12:00:00\",\"header\":\"Safari Simon St. Bededag 22\",\"end\":\"2022-05-13T00:00:00\",\"type\":\"Activity\"},{\"allDay\":false,\"weekDays\":[\"Thursday\",\"Friday\"],\"start\":\"2022-05-26T12:00:00\",\"header\":\"Safari Simon Kr. Himmelfart 22\",\"end\":\"2022-05-27T00:00:00\",\"type\":\"Activity\"},{\"allDay\":false,\"weekDays\":[\"Monday\"],\"start\":\"2022-06-06T12:00:00\",\"header\":\"Safari Simon Pinse 22\",\"end\":\"2022-06-06T00:00:00\",\"type\":\"Activity\"}],\"header\":\"Safari Simon\",\"noIndex\":false,\"imageFullscreen\":{\"umbracoBytes\":1096993,\"umbracoExtension\":\"jpg\",\"umbracoHeight\":900,\"umbracoWidth\":1920,\"umbracoFile\":{\"src\":\"/media/fspldyix/safari-simon-1920x900.jpg\",\"name\":\"Safari Simon\"}}}},\"score\":0},{\"item\":{\"title\":\"Elefant\",\"description\":null,\"url\":\"/aktiviteter/elefant\",\"content\":[],\"properties\":{\"image\":{\"umbracoBytes\":280076,\"umbracoExtension\":\"jpg\",\"umbracoHeight\":350,\"umbracoWidth\":700,\"umbracoFile\":{\"src\":\"/media/33qlvdaj/elefant-700x350.jpg\",\"name\":\"Elefant\"}},\"times\":[{\"allDay\":false,\"weekDays\":[\"Monday\",\"Tuesday\",\"Wednesday\",\"Thursday\",\"Friday\",\"Saturday\",\"Sunday\"],\"start\":\"2022-04-09T13:10:00\",\"header\":\"Sommer 22\",\"end\":\"2022-04-29T00:00:00\",\"type\":\"Activity\"},{\"allDay\":false,\"weekDays\":[\"Monday\",\"Tuesday\",\"Wednesday\",\"Thursday\",\"Friday\",\"Saturday\",\"Sunday\"],\"start\":\"2022-05-02T13:10:00\",\"header\":\"Sommer 22\",\"end\":\"2022-10-02T00:00:00\",\"type\":\"Activity\"}],\"header\":\"Elefant\",\"noIndex\":false,\"imageFullscreen\":{\"umbracoBytes\":1490831,\"umbracoExtension\":\"jpg\",\"umbracoHeight\":900,\"umbracoWidth\":1920,\"umbracoFile\":{\"src\":\"/media/cxnnj2jo/elefant-1920x900.jpg\",\"name\":\"Elefant\"}}}},\"score\":0},{\"item\":{\"title\":\"Bavian\",\"description\":null,\"url\":\"/aktiviteter/bavian\",\"content\":[],\"properties\":{\"image\":{\"umbracoBytes\":127122,\"umbracoExtension\":\"jpg\",\"umbracoHeight\":350,\"umbracoWidth\":700,\"umbracoFile\":{\"src\":\"/media/zw1necge/bavian-700x350.jpg\",\"name\":\"Bavian\"}},\"times\":[{\"allDay\":false,\"weekDays\":[\"Monday\",\"Tuesday\",\"Wednesday\",\"Thursday\",\"Friday\",\"Saturday\",\"Sunday\"],\"start\":\"2022-04-09T14:00:00\",\"header\":\"Bavian sommer 22\",\"end\":\"2022-10-23T00:00:00\",\"type\":\"Activity\"}],\"header\":\"Bavian\",\"noIndex\":false,\"imageFullscreen\":{\"umbracoBytes\":545814,\"umbracoExtension\":\"jpg\",\"umbracoHeight\":900,\"umbracoWidth\":1920,\"umbracoFile\":{\"src\":\"/media/crzjaxb0/bavian-1920x900.jpg\",\"name\":\"Bavian\"}}}},\"score\":0}],\"itemsTotal\":8,\"facetOptions\":{}}";
        
        // Act
        var response = _mapper.Parse(input);
        
        // Assert
        response.IsSuccess(out var result).Should().BeTrue();
        result.Should().HaveCount(8);
    }
    
}
