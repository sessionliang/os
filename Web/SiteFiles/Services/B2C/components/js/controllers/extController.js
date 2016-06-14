var extController = {};

extController.guesses = [];
extController.onPreLoad = null;
extController.onLoaded = null;

extController.GetGuesses = function () {
    extService.getGuesses(function (data) {
        extController.guesses = data.guesses;
        utilService.render('extController', extController, extController.onPreLoad, extController.onLoaded);

        $('#extController0').remove();

        jQuery(".sc_con3Box").slide({ titCell: "", mainCell: ".sc_con4 ul", autoPage: true, effect: "leftLoop", scroll: 6, vis: 6 });
        jQuery(".sc_pubFs").slide({ mainCell: ".bd ul", effect: "leftLoop", autoPlay: true });
    });
}

$(function () {
    if (typeof (extControllerOn) == "function") {
        extControllerOn();
    }
    extController.GetGuesses();
});