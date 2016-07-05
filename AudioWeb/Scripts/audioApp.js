var app = angular.module('module1', ['ngAudio']);

app.controller("controller1", ['$scope', 'ngAudio', 'AudioData', function ($scope, ngAudio, audioData) {
    var self = this;

    audioData.getAll({
    }).then(function (response) {
        console.log(response.data);
        if (response.data) {
            $scope.audios = [];
            $.map(response.data, function (val, i) {
                console.log(val.Src);
                var value = {
                    src: val.Src,
                    audio: ngAudio.load(val.Src),
                    name: val.Name,
                    plays: val.Plays,
                    skips: val.Skips,
                    playlistIndex: i
                };

                $scope.audios.push(value);
            });

            $scope.currentAudio = $scope.audios[0];

            $.map($scope.audios, function (val, i) {
                var audio = val.audio;
                val.audio.complete(function () {
                    if (audio.paused) {
                        audio.setProgress(0);
                        $scope.playNext(val);
                    }
                    return false;
                });
            });
        }
    });

    $scope.reset = function (audio) {
        audio.pause();
        audio.setProgress(0);
    };

    $scope.play = function (audio) {
        $.map($scope.audios, function (val, i) {
            //force to pause other tracks
            if (audio != val.audio && !val.audio.paused) {
                val.audio.pause();
                val.audio.setProgress(0);
            }
        });
        audio.paused ? audio.play() : audio.pause()
    };

    $scope.playSong = function (audio) {
        $scope.currentAudio = audio;
        $scope.play($scope.currentAudio.audio);
    };

    $scope.playNext = function (audio) {
        if (audio.playlistIndex < $scope.audios.length - 1) {
            $scope.playSong($scope.audios[audio.playlistIndex + 1]);
        }
    };

    $scope.playPrev = function (audio) {
        if (audio.playlistIndex > 0) {
            $scope.playSong($scope.audios[audio.playlistIndex - 1]);
        }
    };

    //audioEntity
    //$scope.refreshData = function () {

    //    audioData.get({
    //    }).then(function (response) {
    //        console.log(response.data);
    //        if (response.data) {

    //            $.map($scope.audios, function (val, i) {
    //                var audio = val.audio;
    //                val.audio.complete(function () {
    //                    if (audio.paused) {
    //                        audio.setProgress(0);
    //                        $scope.playNext(val);
    //                    }
    //                    return false;
    //                });
    //            });
    //        }
    //    });
    //};
}]);